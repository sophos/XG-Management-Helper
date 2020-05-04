Imports System.IO
Imports System.Text.RegularExpressions
Imports Renci.SshNet

Public Class XGShellConnection
    Private xg As SshClient
    Public Shared LogFile As String = "action.log"
    Public LogLevel As LogSeverity = LogSeverity.Debug
    Public Enum LogSeverity
        'Emergency = 0
        'Alert = 1
        Critical = 2
        [Error] = 3
        'Warning = 4
        'Notice = 5
        Informational = 6
        Debug = 7
    End Enum

    Private AdvancedShell As Boolean = False

#Region "Public Methods"

    Public Shared Sub WriteToLog(Severity As LogSeverity, Message As String, LogLevel As LogSeverity)
        If Severity > LogLevel Then Exit Sub
        Dim stackTrace As New Diagnostics.StackFrame(1)
        Dim caller As String = stackTrace.GetMethod.Name
        Dim timestamp As String = Now.ToString("yyyy-mm-dd hh:MM:ss ff")
        Message = Message.Replace(vbCr, "").Replace(vbLf, " ")
        Message = Regex.Replace(Message, """password"": "".*?""", """password"": ""**********""")
        System.IO.File.AppendAllText(LogFile, String.Format("{0} - severity='{1}' caller='{2}' message='{3}'{4}", timestamp, Severity.ToString, caller, Message.Replace("{", "{{").Replace("}", "}}"), vbNewLine))
    End Sub

    Public Function CheckSQLiActivity(Host As String, shell_user As String, shell_pass As String, LogLevel As LogSeverity) As String
        Me.LogLevel = LogLevel
        Try
            Dim Shell As ShellStream = GetAdvancedShell(Host, shell_user, shell_pass)
            If Not AdvancedShell Then Return "Failed to connect successfully"

            'Dim cmd As String = "psql -n pgrouser -d corporate -tAc ""Select servicevalue from tblclientservices where servicekey = 'm1files'"" | awk -F"","" '{ for (i=1; $i; i++) print $i }' | grep ""generate_curl_ca_bundle.sh"" | cut -d' ' -f3-"
            Dim patched As Boolean = False
            Dim affected As Boolean = False
            Dim nfo As String = Expect("cat /static/sfm_sync_hfix_stamp", "#", Shell)
            patched = Not nfo.Contains("No such file")

            If patched Then
                nfo = Expect("nvram get sfostainted 2>/dev/nul", "#", Shell)
                affected = nfo.Contains("1")
                Return String.Format("Hotfix applied: YES Affected: {0}", If(affected, "YES", "NO"))

            Else
                Return String.Format("Hotfix applied: NO Affected: UNKNOWN")

            End If

        Catch tex As TimeoutException
            WriteToLog(LogSeverity.Error, String.Format("action='connect' host='{0}' message='Not Connected' error='timeout'", Host))
            Return "Connection Timed Out"

        Catch ex As Exception
            If ex.Message.Contains("after a period of time") Then
                WriteToLog(LogSeverity.Error, String.Format("action='connect' host='{0}' message='Not Connected' error='timeout'", Host))
                Return "Timed Out"

            Else
                WriteToLog(LogSeverity.Critical, String.Format("action='connect' host='{0}' message='Not Connected' error='{1}'", Host, ex.Message))
                Return "E100:" & ex.Message
            End If

        End Try


    End Function

    Public Function RegisterToCentral(Host As String, shell_user As String, shell_pass As String, central_user As String, central_pass As String, EnableManagement As Boolean, EnableReporting As Boolean, EnableBackup As Boolean, LogLevel As LogSeverity) As String
        Me.LogLevel = LogLevel
        'xg = New SshClient(Host, shell_user, shell_pass)
        'WriteToLog(LogSeverity.Debug, String.Format("action='showArgs' host='{0}' shell_user='{1}' shell_pass='{2}' central_user='{3}' central_pass='{4}' EnableManagement={5} EnableReporting={6} EnableBackup={7}", Host, shell_user, shell_pass.Length & " chars", central_user, central_pass.Length & " chars", EnableManagement, EnableReporting, EnableBackup))
        Try
            Dim Shell As ShellStream = GetAdvancedShell(Host, shell_user, shell_pass)
            If Not AdvancedShell Then Return "Failed to connect successfully"
            Dim version As String = GetVersion(Shell)
            Dim nfo As String = Expect(" central-register --status", "#", Shell)

            'check if it's already registered to Central
            If nfo.Contains("currently not registered") Then
                WriteToLog(LogSeverity.Informational, String.Format("action='registration check' host='{0}' result=False message='{1}'", Host, nfo))

                'if not yet registered, then attempt to register it
                nfo = Expect(String.Format("opcode -ds nosync SophosCentralRegistration -t json -b '{{""username"":""{0}"", ""password"": ""{1}""}}'", central_user, central_pass), "#", Shell)

                If nfo.Contains("success") Then
                    WriteToLog(LogSeverity.Debug, String.Format("action='registration check' host='{0}' result=True message='{1}'", Host, nfo))
                    nfo = "Registered"
                Else
                    WriteToLog(LogSeverity.Informational, String.Format("action='registration check' host='{0}' result=False message='{1}'", Host, nfo))
                    nfo = "E1:" & nfo 'leave nfo value to return to the UI
                End If

            ElseIf nfo.Contains("currently registered") Then
                WriteToLog(LogSeverity.Debug, String.Format("action='registration check' host='{0}' result=True message='{1}'", Host, nfo))
                nfo = "Registered"
            Else
                WriteToLog(LogSeverity.Debug, String.Format("action='registration check' host='{0}' result='unknown' message='{1}'", Host, nfo))
                nfo = "E2:" & nfo 'leave nfo value to return to the UI
            End If

            'device is registered - now set management, reporting, backup services
            If nfo = "Registered" Then
                'ensure it is not managed by cfm or sfm
                nfo = Expect("opcode apiInterface -s nosync -t json -b '{""cmtype"":""-1"",""CCCAsAppMgt"":""1"",""mode"":765}'", "#", Shell)
                If nfo.Contains("success") Or nfo.Contains("Cemntral Management is enable") Then
                    'what firmware version is the firewall running?
                    If version.Contains("17.5.") Then
                        'enable/disable management, reporting, backup
                        nfo = Expect(String.Format("opcode sophos_central_enable -s nosync -t json -b '{{ ""cmdiv"": ""{0}"", ""joinmethod"": ""Manual"", ""fwbackup"": ""{2}"" }}'",
                                           If(EnableManagement, "1", "0"), If(EnableReporting, "1", "0"), If(EnableBackup And EnableManagement, "1", "0")), "#", Shell)

                        If nfo.Contains("success") Then
                            nfo = Expect("central-connect --check_status", "#", Shell)
                            If nfo.Contains("approval_pending") Then
                                nfo = "Approval Pending"
                            ElseIf nfo.Contains("approved_by_customer") Then
                                nfo = "Central Service(s) enabled"
                            Else
                                nfo = "E3:" & nfo 'leave nfo value to return to the UI
                            End If
                        End If

                    ElseIf version.Contains("18.") Then
                        'enable/disable management, reporting, backup
                        nfo = Expect(String.Format("opcode sophos_central_enable -s nosync -t json -b '{{ ""cmdiv"": ""{0}"", ""crdiv"": ""{1}"", ""joinmethod"": ""Manual"", ""fwbackup"": ""{2}"" }}'",
                                           If(EnableManagement, "1", "0"), If(EnableReporting, "1", "0"), If(EnableBackup And EnableManagement, "1", "0")), "#", Shell)

                        If nfo.Contains("success") Then
                            nfo = Expect("central-connect --check_status", "#", Shell)
                            If nfo.Contains("approval_pending") Then
                                nfo = "Approval Pending"
                            ElseIf nfo.Contains("approved_by_customer") Then
                                nfo = "Central Service(s) enabled"
                            Else
                                nfo = "E4:" & nfo 'leave nfo value to return to the UI
                            End If
                        ElseIf nfo.Contains("sophos_central_enable failed") Then
                            nfo = "Could not register with Central. Connectivity problem?"
                        Else
                            nfo = "E5:" & nfo 'leave nfo value to return to the UI
                        End If
                    Else 'some other version..
                        nfo = "E10:" & nfo 'leave nfo value to return to the UI
                    End If
                Else
                    nfo = "E11:" & nfo 'leave nfo value to return to the UI
                End If
            End If
            xg.Disconnect()

            Return nfo

        Catch tex As TimeoutException
            WriteToLog(LogSeverity.Error, String.Format("action='connect' host='{0}' message='Not Connected' error='timeout'", Host))
            Return "Timed Out"

        Catch ex As Exception
            If ex.Message.Contains("after a period of time") Then
                WriteToLog(LogSeverity.Error, String.Format("action='connect' host='{0}' message='Not Connected' error='timeout'", Host))
                Return "Timed Out"

            Else
                WriteToLog(LogSeverity.Critical, String.Format("action='connect' host='{0}' message='Not Connected' error='{1}'", Host, ex.Message))
                Return "E100:" & ex.Message
            End If
        End Try

    End Function

    Public Function CheckCurrentFirmwareVersion(Host As String, shell_user As String, shell_pass As String, LogLevel As LogSeverity) As String
        Me.LogLevel = LogLevel
        Try
            Dim Shell As ShellStream = GetAdvancedShell(Host, shell_user, shell_pass)
            If Not AdvancedShell Then Return "Failed to get to advanced shell for some reason."

            Dim ver As String = GetVersion(Shell)
            Debug.Print("{0}", ver)
            Dim hf As String = GetHotfix(Shell)
            Debug.Print("Hotfix {0}", hf)
            Return String.Format("{0} Hotfix#: {1}", ver, hf)

        Catch tex As TimeoutException
            WriteToLog(LogSeverity.Error, String.Format("action='connect' host='{0}' message='Not Connected' error='timeout'", Host))
            Return "Connection Timed Out"

        Catch ex As Exception
            If ex.Message.Contains("after a period of time") Then
                WriteToLog(LogSeverity.Error, String.Format("action='connect' host='{0}' message='Not Connected' error='timeout'", Host))
                Return "Timed Out"

            Else
                WriteToLog(LogSeverity.Critical, String.Format("action='connect' host='{0}' message='Not Connected' error='{1}'", Host, ex.Message))
                Return "E100:" & ex.Message
            End If

        End Try
    End Function

    Public Function InstallHotfixes(Host As String, shell_user As String, shell_pass As String, LogLevel As LogSeverity) As String
        Me.LogLevel = LogLevel
        Try
            Dim Shell As ShellStream = GetAdvancedShell(Host, shell_user, shell_pass)
            If Not AdvancedShell Then Return "Failed to get to advanced shell for some reason."

            Dim hf1 As String = GetHotfix(Shell)
            Dim result As String = Expect("opcode get_SOA -ds nosync", "#", Shell)
            Dim hf2 As String = GetHotfix(Shell)
            If hf1 = hf2 Then
                Return String.Format("{0}: Version remains {1}", result, hf2)
            Else
                Return String.Format("{0}: Version changed from {1} to {2}", result, hf1, hf2)
            End If

        Catch tex As TimeoutException
            WriteToLog(LogSeverity.Error, String.Format("action='connect' host='{0}' message='Not Connected' error='timeout'", Host))
            Return "Connection Timed Out"

        Catch ex As Exception
            WriteToLog(LogSeverity.Critical, String.Format("action='connect' host='{0}' message='Not Connected' error='{1}'", Host, ex.Message))
            Return ex.Message

        End Try

    End Function

    Public Function DeRegisterFromCentral(Host As String, shell_user As String, shell_pass As String, LogLevel As LogSeverity) As String
        Me.LogLevel = LogLevel
        Try
            Dim Shell As ShellStream = GetAdvancedShell(Host, shell_user, shell_pass)
            If Not AdvancedShell Then Return "Failed to get to advanced shell for some reason."

            Dim nfo As String = Expect(" central-register --status", "#", Shell)

            'check if it's already registered to Central
            If nfo.Contains("currently registered") Then
                nfo = Expect("opcode apiInterface -s nosync -t json -b '{""mode"":1324}'", "#", Shell)
                If nfo.Contains("success") Then
                    WriteToLog(LogSeverity.Informational, String.Format("action='connect' host='{0}' message='cleared registration'", Host))
                    Return "Registration cleared successfully"
                Else
                    WriteToLog(LogSeverity.Informational, String.Format("action='connect' host='{0}' message='{1}' error='unknown'", Host, nfo))
                    Return nfo
                End If
            Else
                WriteToLog(LogSeverity.Informational, String.Format("action='connect' host='{0}' message='not registered'", Host))
                Return "Not registered"
            End If
        Catch tex As TimeoutException
            WriteToLog(LogSeverity.Error, String.Format("action='connect' host='{0}' message='Not Connected' error='timeout'", Host))
            Return "Connection Timed Out"

        Catch ex As Exception
            If ex.Message.Contains("after a period of time") Then
                WriteToLog(LogSeverity.Error, String.Format("action='connect' host='{0}' message='Not Connected' error='timeout'", Host))
                Return "Timed Out"

            Else
                WriteToLog(LogSeverity.Critical, String.Format("action='connect' host='{0}' message='Not Connected' error='{1}'", Host, ex.Message))
                Return "E100:" & ex.Message
            End If

        End Try

    End Function

    Public Function SetAdminPassword(Host As String, shell_user As String, shell_pass As String, new_pass As String) As String
        xg = New SshClient(Host, shell_user, shell_pass)
        WriteToLog(LogSeverity.Debug, String.Format("action='showArgs' host='{0}' shell_user='{1}' shell_pass='{2}'", Host, shell_user, shell_pass.Length & " chars"))
        Try
            xg.Connect()
            Dim nfo As String = "Not Connected"
            If xg.IsConnected Then
                Dim Version As String = "18"

                'nfo = xg.ConnectionInfo.ServerVersion
                Dim shell As ShellStream = xg.CreateShellStream("dumb", 80, 24, 800, 600, 1024)
                Threading.Thread.Sleep(100)
                Dim r As New StreamReader(shell)
                nfo = Expect("Login", ":", shell, r, Now.AddSeconds(120))
                nfo = Expect("2", "Select Menu Number \[0-4\]:", shell)
                nfo = Expect("1", "Enter new password:", shell)
                nfo = Expect(new_pass & vbLf, "Re-Enter new password:", shell)
                nfo = Expect(new_pass & vbLf, "Password Changed.", shell)
                xg.Disconnect()
                Return nfo
            End If

            WriteToLog(LogSeverity.Informational, String.Format("action='connect' host='{0}' message='Not Connected' error='unknown'", Host))
            Return Nothing

        Catch tex As TimeoutException
            WriteToLog(LogSeverity.Error, String.Format("action='connect' host='{0}' message='Not Connected' error='timeout'", Host))
            Return Nothing

        Catch ex As Exception
            WriteToLog(LogSeverity.Critical, String.Format("action='connect' host='{0}' message='Not Connected' error='{1}'", Host, ex.Message))
            Return Nothing
        End Try

    End Function

#End Region

    Private Sub WriteToLog(Severity As LogSeverity, Message As String)
        WriteToLog(Severity, Message, LogLevel)
    End Sub

    Private Function GetAdvancedShell(Host As String, shell_user As String, shell_pass As String) As ShellStream
        xg = New SshClient(Host, shell_user, shell_pass)
        WriteToLog(LogSeverity.Debug, String.Format("action='showArgs' host='{0}' shell_user='{1}' shell_pass='{2}'", Host, shell_user, shell_pass.Length & " chars"))
        'Try
        xg.Connect()
        Dim nfo As String = "Not Connected"
        If xg.IsConnected Then
            Dim Version As String = "18"

            'nfo = xg.ConnectionInfo.ServerVersion
            Dim shell As ShellStream = xg.CreateShellStream("dumb", 80, 24, 800, 600, 1024)
            Threading.Thread.Sleep(100)
            Dim r As New StreamReader(shell)
            nfo = Expect("Login", ":", shell, r, Now.AddSeconds(120))

            WriteToLog(LogSeverity.Debug, String.Format("action='connect' host='{0}' message='Connected successfully' version='{1}'", Host, Version))

            nfo = Expect("5", "Select Menu Number \[0-4\]:", shell)
            nfo = Expect("3", "#", shell)
            AdvancedShell = True
            Return shell
        End If

        WriteToLog(LogSeverity.Informational, String.Format("action='connect' host='{0}' message='Not Connected' error='unknown'", Host))
        Return Nothing

        'Catch tex As TimeoutException
        '    WriteToLog(LogSeverity.Error, String.Format("action='connect' host='{0}' message='Not Connected' error='timeout'", Host))
        '    Return Nothing

        'Catch ex As Exception
        '    WriteToLog(LogSeverity.Critical, String.Format("action='connect' host='{0}' message='Not Connected' error='{1}'", Host, ex.Message))
        '    Return Nothing
        'End Try

    End Function

    Private Function GetVersion(shell As ShellStream) As String

        If Not AdvancedShell Then Throw New Exception("Must get to advanced shell first")
        Dim nfo As String = "UNKNOWN"

        nfo = Expect("cat /etc/version", "#", shell)
        Dim ver As String() = Split(nfo, "_")
        If ver.Count = 3 Then
            Return String.Format("v{0} ({1})", ver(2), ver(0))
        Else
            Return nfo
        End If
    End Function

    Private Function GetHotfix(shell As ShellStream) As String
        If Not AdvancedShell Then Throw New Exception("Must get to advanced shell first")
        'Dim nfo As String = "UNKNOWN"

        Dim nfo As String = Expect("cat /conf/soa", "#", shell)
        If nfo.Contains("No such file or directory") Then Return "NO HOTFIXES"
        Return nfo
    End Function

    Public Function SendCommand(cmd As String, sh As ShellStream) As String

        Dim reader As StreamReader = Nothing
        Try
            WriteToLog(LogSeverity.Debug, String.Format("action='{0}' ", cmd))
            reader = New StreamReader(sh)
            Dim writer As New StreamWriter(sh)
            writer.NewLine = vbLf
            writer.AutoFlush = True
            writer.WriteLine(cmd)
            While (sh.Length = 0)
                Threading.Thread.Sleep(500)
            End While

        Catch ex As Exception
            WriteToLog(LogSeverity.Debug, String.Format("error='{0}' ", ex.ToString))

        End Try
        Return reader.ReadToEnd()
    End Function

    Public Function Expect(cmd As String, WaitFor As String, sh As ShellStream, Optional Timeout As Integer = 120) As String

        Dim reader As StreamReader = Nothing
        Try

            reader = New StreamReader(sh)
            Dim writer As New StreamWriter(sh)
            writer.NewLine = vbLf
            writer.AutoFlush = True
            writer.WriteLine(cmd)

            Dim expiry As DateTime = Now.AddSeconds(Timeout)

            'read response
            Dim ret As String = Expect(cmd, WaitFor, sh, reader, expiry).Substring(cmd.Length).Trim
            ret = ret.Substring(0, ret.LastIndexOf(vbLf) - 1)

            WriteToLog(LogSeverity.Debug, String.Format("action='{0}' waitfor='{1}' timeout={2} result='{3}'", cmd, WaitFor, Timeout, ret))
            Return ret

        Catch ex As Exception
            WriteToLog(LogSeverity.Debug, String.Format("action='{0}' waitfor='{1}' timeout={2} result='error' error=''", cmd, WaitFor, Timeout, ex.Message))
            Return "Error: " & ex.Message
        End Try

    End Function

    Private Function Expect(cmd As String, WaitFor As String, sh As ShellStream, reader As StreamReader, expiry As DateTime) As String
        Return Expect(cmd, WaitFor, sh, reader, expiry, 0)
    End Function

    Private Function Expect(cmd As String, WaitFor As String, sh As ShellStream, reader As StreamReader, expiry As DateTime, loops As Integer) As String
        loops += 1

        If Now > expiry Then Throw New TimeoutException

        'wait for something to read
        While (sh.Length = 0)
            Threading.Thread.Sleep(200)
            If Now > expiry Then Throw New TimeoutException
        End While
        WriteToLog(LogSeverity.Debug, String.Format("waitfor='{0}' timeout_at='{1}' reply='no reply yet' result='waiting for reply' loops={2}", WaitFor, expiry.ToString, loops))

        Dim ret As String = reader.ReadToEnd
        If ret IsNot Nothing Then
            If Regex.IsMatch(ret, WaitFor, RegexOptions.IgnoreCase + RegexOptions.Multiline) Then
                WriteToLog(LogSeverity.Debug, String.Format("waitfor='{0}' reply='{1}' result='match found' loops={2}", WaitFor, ret.Replace(cmd, ""), loops))
                Return ret '.Replace(cmd, "")
            End If
            WriteToLog(LogSeverity.Debug, String.Format("waitfor='{0}' reply='{1}' result='waiting for value' loops={2}", WaitFor, ret.Replace(cmd, ""), loops))
            Return ret & Expect(cmd, WaitFor, sh, reader, expiry, loops)
        Else
            WriteToLog(LogSeverity.Debug, String.Format("waitfor='{0}' reply='no reply yet' result='waiting for reply' loops={1}", WaitFor, loops))
            Return Expect(cmd, WaitFor, sh, reader, expiry, loops)
        End If




    End Function
End Class
