' Copyright 2020  Sophos Ltd.  All rights reserved.
' Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
' You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
' Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, 
' WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing 
' permissions and limitations under the License.

Imports System.IO
Imports System.Text.RegularExpressions
Imports Renci.SshNet
Imports Renci.SshNet.Common

Public Class XGShellConnection
    Private WithEvents XG As SshClient
    Public Shared LogFile As String = "action.log"
    Public LogLevel As LogSeverity = LogSeverity.Debug
    Private AdvancedShell As Boolean = False
    Private knownpassword As String = ""
    Public Property Version As String = "UNKNOWN"
    Public OEM As String = "UNKNOWN"
    Public DisplayVersion As String = "UNKNOWN"
    Public MRVersion As String = "UNKNOWN"
    Public Property Timeout As Integer = 60
    Public Property FingerprintReceived As String
    Public Property ErrorReceived As String
    Private Property Host As String
    Public Property AutoTrust As Boolean = False
    ReadOnly DataKey As String
    ReadOnly DataIV As String
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

#Region "Public Methods"

    Sub New(Autotrust As Boolean, Key As String, IV As String)
        Me.AutoTrust = Autotrust
        DataKey = Key
        DataIV = IV
    End Sub

#Region "WriteLog"

    Public Shared Sub WriteToLog(Severity As LogSeverity, Message As String, LogLevel As LogSeverity)
        WriteToLog(Severity, Message, LogLevel, "")
    End Sub

    Public Shared Sub WriteToLog(Severity As LogSeverity, Message As String, LogLevel As LogSeverity, knownpassword As String)
        WriteToLog(LogFile, Severity, Message, LogLevel, knownpassword)
    End Sub

    Public Shared Sub WriteToLog(ByVal LogFile As String, ByVal Severity As LogSeverity, ByVal Message As String, ByVal LogLevel As LogSeverity, knownpassword As String)
        If Severity > LogLevel Then Exit Sub
        Dim stackTrace As New Diagnostics.StackFrame(1)
        Dim caller As String = stackTrace.GetMethod.Name
        Dim timestamp As String = Now.ToString("yyyy-MM-dd hh:mm:ss ff")
        Message = Message.Replace(vbCr, "").Replace(vbLf, " ")
        Message = Regex.Replace(Message, """password"": "".*?""", """password"": ""**REDACTED**""")
        If knownpassword.Length > 0 Then
            Message = Replace(Message, knownpassword, "**REDACTED**")
            For x As Integer = 0 To knownpassword.Length - 2
                Dim Part1 As String = knownpassword.Substring(0, x)
                Dim Part2 As String = knownpassword.Substring(x + 1)
                Message = Regex.Replace(Message, String.Format("{0}\s{{1,3}}{1}", Regex.Escape(Part1), Regex.Escape(Part2)), "**REDACTED**")
            Next
        End If
        System.IO.File.AppendAllText(LogFile, String.Format("{0} - severity=""{1}"" caller=""{2}"" message=""{3}""{4}", timestamp, Severity.ToString, caller, Message.Replace("{", "{{").Replace("}", "}}"), vbNewLine))
    End Sub

#End Region

    Public Function RegisterToCentral(Host As String, shell_user As String, shell_pass As String, central_user As String, central_pass As String, EnableManagement As Boolean, EnableReporting As Boolean, EnableBackup As Boolean, LogLevel As LogSeverity) As ExpectResult
        Me.LogLevel = LogLevel
        knownpassword = central_pass
        'xg = New SshClient(Host, shell_user, shell_pass)
        'WriteToLog(LogSeverity.Debug, String.Format("action='showArgs' host='{0}' shell_user='{1}' shell_pass='{2}' central_user='{3}' central_pass='{4}' EnableManagement={5} EnableReporting={6} EnableBackup={7}", Host, shell_user, shell_pass.Length & " chars", central_user, central_pass.Length & " chars", EnableManagement, EnableReporting, EnableBackup))
        Try
            Dim Shell As ShellStream = GetAdvancedShell(Host, shell_user, shell_pass)
            If Not AdvancedShell Then Throw New Exception("Could not connect to shell")
            Dim version As String = GetVersion(Shell)
            Dim nfo As ExpectResult = ExtendedExpect(" central-register --status", "#", Shell)

            'check if it's already registered to Central
            If nfo.Reply.Contains("currently not registered") Then
                WriteToLog(LogSeverity.Informational, String.Format("action='registration check' host='{0}' result=False message='{1}'", Host, nfo.Summary))

                'if not yet registered, then attempt to register it
                Dim lastnfo As ExpectResult = nfo
                nfo = ExtendedExpect(String.Format("opcode -ds nosync SophosCentralRegistration -t json -b '{{""username"":""{0}"", ""password"": ""{1}""}}'", central_user, central_pass), "#", Shell)
                nfo.InnerResults.Add(lastnfo)
                If nfo.Reply.Contains("success") Then
                    WriteToLog(LogSeverity.Debug, String.Format("action='registration check' host='{0}' result=True message='{1}'", Host, nfo.Summary))
                    nfo.Reply = "Registered"
                ElseIf nfo.Reply.Contains("Temporary error while accessing Sophos Central, please try again later") Then
                    nfo.Success = False
                    nfo.Reply = "Temporary error while accessing Sophos Central, please try again later."
                    WriteToLog(LogSeverity.Error, String.Format("action='registration check' host='{0}' result=False message='{1}'", Host, nfo.Summary))
                Else
                    nfo.Success = False
                    WriteToLog(LogSeverity.Error, String.Format("action='registration check' host='{0}' result=False message='{1}'", Host, nfo.Summary))
                End If

            ElseIf nfo.Reply.Contains("currently registered") Then
                WriteToLog(LogSeverity.Debug, String.Format("action='registration check' host='{0}' result=True message='{1}'", Host, nfo.Summary))
                nfo.Reply = "Registered"
            Else
                nfo.Success = False
                WriteToLog(LogSeverity.Debug, String.Format("action='registration check' host='{0}' result='unknown' message='{1}'", Host, nfo.Summary))
            End If

            'device is registered - now set management, reporting, backup services
            If nfo.Success Then
                'ensure it is not managed by cfm or sfm
                nfo = ExtendedExpect("opcode apiInterface -s nosync -t json -b '{""cmtype"":""-1"",""CCCAsAppMgt"":""1"",""mode"":765}'", "#", Shell)
                If nfo.Reply.Contains("success") Or nfo.Reply.Contains("Cemntral Management is enable") Then
                    'what firmware version is the firewall running?
                    If version.Contains("17.5.") Then
                        'enable/disable management, reporting, backup
                        nfo = ExtendedExpect(String.Format("opcode sophos_central_enable -s nosync -t json -b '{{ ""cmdiv"": ""{0}"", ""joinmethod"": ""Manual"", ""fwbackup"": ""{2}"" }}'",
                                           If(EnableManagement, "1", "0"), If(EnableReporting, "1", "0"), If(EnableBackup And EnableManagement, "1", "0")), "#", Shell)

                        If nfo.Reply.Contains("success") Then
                            nfo = ExtendedExpect("central-connect --check_status", "#", Shell)
                            If nfo.Reply.Contains("approval_pending") Then
                                nfo.Reply = "Approval Pending"
                            ElseIf nfo.Reply.Contains("approved_by_customer") Then
                                nfo.Reply = "Central Service(s) enabled"
                            Else
                                nfo.Reply = "E30924:" & nfo.Summary 'leave nfo value to return to the UI
                            End If
                        End If

                    ElseIf version.Contains("18.") Then
                        'enable/disable management, reporting, backup
                        nfo = ExtendedExpect(String.Format("opcode sophos_central_enable -s nosync -t json -b '{{ ""cmdiv"": ""{0}"", ""crdiv"": ""{1}"", ""joinmethod"": ""Manual"", ""fwbackup"": ""{2}"" }}'",
                                           If(EnableManagement, "1", "0"), If(EnableReporting, "1", "0"), If(EnableBackup And EnableManagement, "1", "0")), "#", Shell)

                        If nfo.Reply.Contains("success") Then
                            nfo = ExtendedExpect("central-connect --check_status", "#", Shell)
                            If nfo.Reply.Contains("approval_pending") Then
                                nfo.Reply = "Approval Pending"
                            ElseIf nfo.Reply.Contains("approved_by_customer") Then
                                nfo.Reply = "Central Service(s) enabled"
                            Else
                                nfo.Success = False
                            End If
                        ElseIf nfo.Reply.Contains("sophos_central_enable failed") Then
                            nfo.Reply = "Could not register with Central. Connectivity problem?"
                        Else
                            nfo.Success = False
                        End If
                    Else 'some other version..
                        nfo.Success = False
                        nfo.Reply = "Unsupported version"

                    End If
                Else
                    '
                End If
            End If
            xg.Disconnect()
            Return nfo

        Catch ex As Exception
            Dim ret As ExpectResult = InterpretError(ex, "SetAdminPassword")
            WriteToLog(LogSeverity.Error, String.Format("action='connect' caller='SetAdminPassword' host='{0}' message='{1}' error='{1}'", Host, ex.Message, ret.FailReason))
            Return ret

        End Try

    End Function

    Public Function CheckCurrentFirmwareVersion(Host As String, shell_user As String, shell_pass As String, LogLevel As LogSeverity) As ExpectResult
        Me.LogLevel = LogLevel
        Try
            Dim Shell As ShellStream = GetAdvancedShell(Host, shell_user, shell_pass)
            If Not AdvancedShell Then Throw New Exception("Could not connect to shell")
            Dim ver As String = GetVersion(Shell) 'DisplayVersion
            Debug.Print("{0}", ver)

            Return New ExpectResult(True, "VersionCheck", "", String.Format("{0} ", ver))

        Catch ex As Exception
            Dim ret As ExpectResult = InterpretError(ex, "SetAdminPassword")
            WriteToLog(LogSeverity.Error, String.Format("action='connect' caller='SetAdminPassword' host='{0}' message='{1}' error='{1}'", Host, ex.Message, ret.FailReason))
            Return ret


        End Try
    End Function

    Public Function InstallHotfixes(Host As String, shell_user As String, shell_pass As String, LogLevel As LogSeverity) As ExpectResult
        Me.LogLevel = LogLevel
        Try
            Dim Shell As ShellStream = GetAdvancedShell(Host, shell_user, shell_pass)
            If Not AdvancedShell Then Throw New Exception("Could not connect to shell")

            Dim hf1 As String = GetHotfix(Shell)
            Dim result As ExpectResult = ExtendedExpect("opcode get_SOA -ds nosync", "#", Shell)
            Dim hf2 As String = GetHotfix(Shell)
            If hf1 = hf2 Then
                Return New ExpectResult(True, "InstallHotfixes", "", String.Format("{0}: Version remains {1}", result, hf2))
            Else
                Return New ExpectResult(True, "InstallHotfixes", "", String.Format("{0}: Version changed from {1} to {2}", result, hf1, hf2))
            End If


        Catch ex As Exception
            Dim ret As ExpectResult = InterpretError(ex, "SetAdminPassword")
            WriteToLog(LogSeverity.Error, String.Format("action='connect' caller='SetAdminPassword' host='{0}' message='{1}' error='{1}'", Host, ex.Message, ret.FailReason))
            Return ret

        End Try

    End Function

    Public Function DeRegisterFromCentral(Host As String, shell_user As String, shell_pass As String, LogLevel As LogSeverity) As ExpectResult
        Me.LogLevel = LogLevel
        Try
            Dim Shell As ShellStream = GetAdvancedShell(Host, shell_user, shell_pass)
            If Not AdvancedShell Then Return New ExpectResult(New Exception("Failed to connect successfully"), "Connection", "Login", "")
            Dim nfo As ExpectResult = ExtendedExpect(" central-register --status", "#", Shell)

            'check if it's already registered to Central
            If nfo.Reply.Contains("currently registered") Then
                nfo = ExtendedExpect("opcode apiInterface -s nosync -t json -b '{""mode"":1324}'", "#", Shell)
                If nfo.Reply.Contains("success") Then
                    WriteToLog(LogSeverity.Informational, String.Format("action='connect' host='{0}' message='cleared registration'", Host))
                    nfo.Reply = "Registration cleared successfully"
                    Return nfo
                Else
                    nfo.Success = False
                    WriteToLog(LogSeverity.Informational, String.Format("action='connect' host='{0}' message='{1}' error='unknown'", Host, nfo))
                    Return nfo
                End If
            Else
                WriteToLog(LogSeverity.Informational, String.Format("action='connect' host='{0}' message='not registered'", Host))
                nfo.Success = False
                nfo.Reply = "Not registered"
                Return nfo
            End If

        Catch ex As Exception
            Dim ret As ExpectResult = InterpretError(ex, "SetAdminPassword")
            WriteToLog(LogSeverity.Error, String.Format("action='connect' caller='SetAdminPassword' host='{0}' message='{1}' error='{1}'", Host, ex.Message, ret.FailReason))
            Return ret
        End Try

    End Function

    Public Function SetAdminPassword(Host As String, shell_user As String, shell_pass As String, new_pass As String, LogLevel As LogSeverity) As XGShellConnection.ExpectResult
        Me.LogLevel = LogLevel
        'xg = New SshClient(Host, shell_user, shell_pass)
        knownpassword = new_pass
        WriteToLog(LogSeverity.Debug, String.Format("action='showArgs' host='{0}' shell_user='{1}' shell_pass='{2}'", Host, shell_user, shell_pass.Length & " chars"))
        Try
            Dim nfo As ExpectResult
            Dim shell As ShellStream = GetShellMenu(Host, shell_user, shell_pass)
            nfo = ExtendedExpect("2", "Select Menu Number \[0-4\]:", shell)
            WriteToLog(LogSeverity.Debug, String.Format("action='menu_nav_1' host='{0}' waitfor='Select Menu Number \[0-4\]:' success='{1}' reply='{2}'", Host, nfo.Success, nfo.Summary))
            nfo = ExtendedExpect("1", "Enter new password:", shell)
            WriteToLog(LogSeverity.Debug, String.Format("action='menu_nav_2' host='{0}' waitfor='Enter new password:' success='{1}' reply='{2}'", Host, nfo.Success, nfo.Summary))
            nfo = ExtendedExpect(new_pass, "Re-Enter new Password:", shell)
            WriteToLog(LogSeverity.Debug, String.Format("action='set_pass' host='{0}' waitfor='Re-Enter new Password:' success='{1}' reply='{2}'", Host, nfo.Success, nfo.Summary))
            nfo = ExtendedExpect(new_pass, {"Password Changed", "Failed to change Password"}, shell)
            Debug.Print(nfo.Reply & ", " & nfo.LookingFor)
            Select Case nfo.LookingFor
                Case "Password Changed"

                Case "Failed to change Password"
                    nfo.Success = False
                    nfo.Reply = nfo.Reply.Replace("Failed to change Password.", "").Trim
            End Select
            WriteToLog(LogSeverity.Debug, String.Format("action='confirm_pass' host='{0}' waitfor='Password Changed'  success='{1}' reply='{2}'", Host, nfo.Success, nfo.Summary))
            xg.Disconnect()

            WriteToLog(LogSeverity.Informational, String.Format("action='SetAdminPassword' host='{0}' success='{1}' reply='{2}'", Host, nfo.Success, nfo.Summary))
            Return nfo

        Catch ex As Exception
            Dim ret As ExpectResult = InterpretError(ex, "SetAdminPassword")
            WriteToLog(LogSeverity.Error, String.Format("action='connect' caller='SetAdminPassword' host='{0}' message='{1}' error='{1}'", Host, ex.Message, ret.FailReason))
            Return ret

        End Try

    End Function

    Public Function GetCaptchaSetting(Host As String, shell_user As String, shell_pass As String, LogLevel As LogSeverity) As XGShellConnection.ExpectResult
        Me.LogLevel = LogLevel

        WriteToLog(LogSeverity.Debug, String.Format("action='showArgs' host='{0}' shell_user='{1}' shell_pass='{2}'", Host, shell_user, shell_pass.Length & " chars"))
        Try
            Dim nfo As ExpectResult
            Dim shell As ShellStream = GetConsole(Host, shell_user, shell_pass)
            nfo = ExtendedExpect("system captcha_authentication_VPN show", "console>", shell)
            If nfo.Reply.Contains("% Error: Unknown Parameter 'captcha_authentication_VPN'") Then
                nfo.Success = False
                nfo.Reply = "Option not available on current firmware/hotfix version."
            ElseIf nfo.Reply.Contains("enabled") Then
                nfo.Reply = "ENABLED on VPN zone"
            Else
                nfo.Reply = "DISABLED on VPN zone"
            End If
            Return nfo

        Catch ex As Exception
            Dim ret As ExpectResult = InterpretError(ex, "SetAdminPassword")
            WriteToLog(LogSeverity.Error, String.Format("action='connect' caller='SetAdminPassword' host='{0}' message='{1}' error='{1}'", Host, ex.Message, ret.FailReason))
            Return ret
        End Try
    End Function

    Public Function DisableCaptchaOnVPN(Host As String, shell_user As String, shell_pass As String, LogLevel As LogSeverity) As XGShellConnection.ExpectResult
        Me.LogLevel = LogLevel

        WriteToLog(LogSeverity.Debug, String.Format("action='showArgs' host='{0}' shell_user='{1}' shell_pass='{2}'", Host, shell_user, shell_pass.Length & " chars"))
        Try
            Dim nfo As ExpectResult
            Dim shell As ShellStream = GetConsole(Host, shell_user, shell_pass)
            nfo = ExtendedExpect("system captcha_authentication_VPN disable", {"console>", "(Y/N) ?"}, shell)
            If nfo.Reply.Contains("% Error: Unknown Parameter 'captcha_authentication_VPN'") Then
                nfo.Success = False
                nfo.Reply = "Option not available on current firmware/hotfix version."
                Return nfo
            End If
            If nfo.LookingFor = "(Y/N) ?" Then
                nfo = ExtendedExpect("Y", "console>", shell)
            End If

            If nfo.Reply.Contains("VPN zone is turned off") Then
                    nfo.Reply = "CAPCHA disabled on VPN zone"
                Else
                    nfo.Success = False
                End If

                Return nfo

        Catch ex As Exception
            Dim ret As ExpectResult = InterpretError(ex, "SetAdminPassword")
            WriteToLog(LogSeverity.Error, String.Format("action='connect' caller='SetAdminPassword' host='{0}' message='{1}' error='{1}'", Host, ex.Message, ret.FailReason))
            Return ret
        End Try
    End Function

    Public Function EnableCaptchaOnVPN(Host As String, shell_user As String, shell_pass As String, LogLevel As LogSeverity) As XGShellConnection.ExpectResult
        Me.LogLevel = LogLevel

        WriteToLog(LogSeverity.Debug, String.Format("action='showArgs' host='{0}' shell_user='{1}' shell_pass='{2}'", Host, shell_user, shell_pass.Length & " chars"))
        Try
            Dim nfo As ExpectResult
            Dim shell As ShellStream = GetConsole(Host, shell_user, shell_pass)

            nfo = ExtendedExpect("system captcha_authentication_VPN enable", {"console>", "(Y/N) ?"}, shell)
            If nfo.Reply.Contains("% Error: Unknown Parameter 'captcha_authentication_VPN'") Then
                nfo.Success = False
                nfo.Reply = "Option not available on current firmware/hotfix version."
                Return nfo
            End If

            If nfo.LookingFor = "(Y/N) ?" Then
                nfo = ExtendedExpect("Y", "console>", shell)
            End If

            If nfo.Reply.Contains("VPN zone is turned on") Then
                nfo.Reply = "CAPCHA enabled on VPN zone"
            Else
                nfo.Success = False
            End If

            Return nfo

        Catch ex As Exception
            Dim ret As ExpectResult = InterpretError(ex, "SetAdminPassword")
            WriteToLog(LogSeverity.Error, String.Format("action='connect' caller='SetAdminPassword' host='{0}' message='{1}' error='{1}'", Host, ex.Message, ret.FailReason))
            Return ret
        End Try
    End Function

    Public Function GetManditoryPasswordResetStatus(Host As String, shell_user As String, shell_pass As String, LogLevel As LogSeverity) As XGShellConnection.ExpectResult
        Me.LogLevel = LogLevel

        WriteToLog(LogSeverity.Debug, String.Format("action='showArgs' host='{0}' shell_user='{1}' shell_pass='{2}'", Host, shell_user, shell_pass.Length & " chars"))
        Try
            Dim nfo As ExpectResult
            Dim shell As ShellStream = GetConsole(Host, shell_user, shell_pass)
            nfo = ExtendedExpect("system mandatory_password_reset show", "console>", shell)
            If nfo.Reply.Contains("% Error: Unknown Parameter 'mandatory_password_reset'") Then
                nfo.Success = False
                nfo.Reply = "Option not available on current firmware/hotfix version."
            ElseIf nfo.Reply.Contains("enabled") Then
                nfo.Reply = "ENABLED on VPN zone"
            Else
                nfo.Reply = "DISABLED on VPN zone"
            End If
            Return nfo

        Catch ex As Exception
            Dim ret As ExpectResult = InterpretError(ex, "SetAdminPassword")
            WriteToLog(LogSeverity.Error, String.Format("action='connect' caller='SetAdminPassword' host='{0}' message='{1}' error='{1}'", Host, ex.Message, ret.FailReason))
            Return ret
        End Try
    End Function

    Public Function DisableManditoryPasswordReset(Host As String, shell_user As String, shell_pass As String, LogLevel As LogSeverity) As XGShellConnection.ExpectResult
        Me.LogLevel = LogLevel

        WriteToLog(LogSeverity.Debug, String.Format("action='showArgs' host='{0}' shell_user='{1}' shell_pass='{2}'", Host, shell_user, shell_pass.Length & " chars"))
        Try
            Dim nfo As ExpectResult
            Dim shell As ShellStream = GetConsole(Host, shell_user, shell_pass)
            nfo = ExtendedExpect("system mandatory_password_reset disable", {"console>", "(Y/N) ?"}, shell)
            If nfo.Reply.Contains("% Error: Unknown Parameter 'mandatory_password_reset'") Then
                nfo.Success = False
                nfo.Reply = "Option not available on current firmware/hotfix version."
                Return nfo
            End If
            If nfo.LookingFor = "(Y/N) ?" Then
                nfo = ExtendedExpect("Y", "console>", shell)
            End If

            If nfo.Reply.Contains("is turned off") Then
                nfo.Reply = "Password reset pop-up is DISABLED"
            Else
                nfo.Success = False
            End If

            Return nfo

        Catch ex As Exception
            Dim ret As ExpectResult = InterpretError(ex, "SetAdminPassword")
            WriteToLog(LogSeverity.Error, String.Format("action='connect' caller='SetAdminPassword' host='{0}' message='{1}' error='{1}'", Host, ex.Message, ret.FailReason))
            Return ret
        End Try
    End Function

#End Region

#Region "Private Methods"

    Private Function ByteArrayToHex(ByRef ByteArray() As Byte) As String
        Dim l As Long, strRet As String

        For l = LBound(ByteArray) To UBound(ByteArray)
            strRet = strRet & Hex$(ByteArray(l)) & " "
        Next l

        'Remove last space at end.
        ByteArrayToHex = Left$(strRet, Len(strRet) - 1)
    End Function

    Private Function InterpretError(ex As Exception, action As String) As ExpectResult
        If ex.Message.Contains("after a period of time") Then
            Return New ExpectResult(New TimeoutException("Connection Timed Out", ex), "Timeout", action, "")
        ElseIf ex.Message.Contains("password") Then
            Return New ExpectResult(New Exception("Username or password not accepted", ex), "Credentials", action, "")
        ElseIf ex.Message.Contains("connect to shell") Then
            Return New ExpectResult(New Exception("Connection Failed", ex), "Connection", action, "")
        Else
            Return New ExpectResult(ex, "unknown", action, "")
        End If
    End Function

    Private Sub WriteToLog(Severity As LogSeverity, Message As String)
        WriteToLog(Severity, Message, LogLevel, knownpassword)
    End Sub

    Private Function HostKeyReceived(Sender As Object, e As EventArgs)

    End Function

    Private Function GetShellMenu(Host As String, shell_user As String, shell_pass As String) As ShellStream
        xg = New SshClient(Host, shell_user, shell_pass)
        Me.Host = Host
        WriteToLog(LogSeverity.Debug, String.Format("action='showArgs' host='{0}' shell_user='{1}' shell_pass='{2}'", Host, shell_user, shell_pass.Length & " chars"))

        xg.Connect()

        'Dim Version As String = xg.ConnectionInfo.ServerVersion
        Dim nfo As ExpectResult '= "Not Connected"
        If xg.IsConnected Then
            Dim shell As ShellStream = xg.CreateShellStream("dumb", 80, 24, 800, 600, 1024)

            Threading.Thread.Sleep(100)
            Dim r As New StreamReader(shell)
            nfo = ExtendedExpect("Login", ":", shell, r)

            'Debug.Print("LOGIN: " & nfo)
            Dim vMatch As Match = Regex.Match(nfo.Reply, "Firmware\s+Version\s+(?<DISPLAYVERSION>(?<OEM>\w+)\s+(?<VERSION>[0-9.]+)\s(?<MR>[A-Z0-9-.]*))")
            If vMatch.Success Then
                DisplayVersion = vMatch.Groups("DISPLAYVERSION").Value
                Version = vMatch.Groups("VERSION").Value
                OEM = vMatch.Groups("OEM").Value
                MRVersion = vMatch.Groups("MR").Value
            End If
            Debug.Print("Display Version: {0}, Version: {1}, MR: {2}, OEM: {3}", DisplayVersion, Version, MRVersion, OEM)

            WriteToLog(LogSeverity.Debug, String.Format("action='connect' host='{0}' message='Connected successfully' version='{1}'", Host, DisplayVersion))
            AdvancedShell = False
            Return shell
        End If

        WriteToLog(LogSeverity.Informational, String.Format("action='connect' host='{0}' message='Not Connected' error='unknown'", Host))
        Return Nothing

    End Function

    Private Function GetConsole(Host As String, shell_user As String, shell_pass As String) As ShellStream
        WriteToLog(LogSeverity.Debug, String.Format("action='showArgs' host='{0}' shell_user='{1}' shell_pass='{2}'", Host, shell_user, shell_pass.Length & " chars"))
        If AdvancedShell Then Throw New Exception("Already dropped to advanced shell.")
        Dim nfo As ExpectResult
        Dim shell As ShellStream = GetShellMenu(Host, shell_user, shell_pass)
        nfo = ExtendedExpect("4", "console>", shell)
        Return shell
        '
    End Function

    Private Function GetAdvancedShell(Host As String, shell_user As String, shell_pass As String) As ShellStream
        WriteToLog(LogSeverity.Debug, String.Format("action='showArgs' host='{0}' shell_user='{1}' shell_pass='{2}'", Host, shell_user, shell_pass.Length & " chars"))
        Dim nfo As ExpectResult
        Dim shell As ShellStream = GetShellMenu(Host, shell_user, shell_pass)
        nfo = ExtendedExpect("5", "Select Menu Number \[0-4\]:", shell)
        nfo = ExtendedExpect("3", "#", shell)
        AdvancedShell = True
        Return shell
        '
    End Function

    Private Function GetVersion(shell As ShellStream) As String
        If Not AdvancedShell Then Throw New Exception("Must get to advanced shell first")
        'Dim nfo As ExpectResult '= "UNKNOWN"
        Dim hf As String = GetHotfix(shell)
        Return String.Format("{0} (v{1}-HF#{2})", DisplayVersion, Version, hf)

    End Function

    Private Function GetHotfix(shell As ShellStream) As String
        If Not AdvancedShell Then Throw New Exception("Must get to advanced shell first")
        Dim nfo As ExpectResult = ExtendedExpect("cat /conf/soa", "#", shell)
        If nfo.Reply.Contains("No such file or directory") Then Return "NO HOTFIXES"
        Return nfo.Summary
    End Function

    Public Function SendCommand(cmd As String, sh As ShellStream) As String
        Dim reader As StreamReader = Nothing
        Try
            WriteToLog(LogSeverity.Debug, String.Format("action='{0}' ", cmd))
            reader = New StreamReader(sh)
            Dim writer As New StreamWriter(sh) With {.NewLine = vbLf, .AutoFlush = True}
            writer.WriteLine(cmd)
            While (sh.Length = 0)
                Threading.Thread.Sleep(500)
            End While

        Catch ex As Exception
            WriteToLog(LogSeverity.Debug, String.Format("error='{0}' ", ex.ToString))

        End Try
        Return reader.ReadToEnd()
    End Function

    Private Sub XG_HostKeyReceived(sender As Object, e As HostKeyEventArgs) Handles XG.HostKeyReceived
        Using AESWrapper As New AES256Wrapper(DataKey, DataIV)
            Using key As Microsoft.Win32.RegistryKey = My.Computer.Registry.CurrentUser.CreateSubKey("Software\XGMigrationHelper\Hosts\Host-" & Me.Host)
                Dim ExpectedFingerprint As String = AESWrapper.Decrypt(key.GetValue("Fingerprint"))
                If ExpectedFingerprint IsNot Nothing Then
                    If ExpectedFingerprint.StartsWith(Me.Host) Then
                        ExpectedFingerprint = ExpectedFingerprint.Substring(Me.Host.Length)
                    Else
                        ExpectedFingerprint = Nothing
                    End If
                End If
                    If ExpectedFingerprint = "" Then ExpectedFingerprint = Nothing
                FingerprintReceived = ByteArrayToHex(e.FingerPrint)
                If ExpectedFingerprint IsNot Nothing Then
                    If ExpectedFingerprint.Equals(FingerprintReceived) Then
                        e.CanTrust = True
                    Else
                        Dim fc As New FingerprintConfirmation(FingerprintReceived, True, Host)
                        e.CanTrust = fc.ShowDialog = DialogResult.OK
                    End If
                Else
                    If AutoTrust Then
                        e.CanTrust = True
                    Else
                        Dim fc As New FingerprintConfirmation(FingerprintReceived, False, Host)
                        e.CanTrust = fc.ShowDialog = DialogResult.OK
                    End If
                End If
                If e.CanTrust Then
                    key.SetValue("Fingerprint", AESWrapper.Encrypt(Me.Host & FingerprintReceived))
                End If
            End Using
        End Using

    End Sub

    Private Sub XG_ErrorOccurred(sender As Object, e As ExceptionEventArgs) Handles XG.ErrorOccurred
        ErrorReceived = e.Exception.Message
    End Sub

#End Region

#Region "Expect"

    Public Class ExpectResult
        Public Property Success As Boolean
        Public Property FailReason As String
        Public Property Command As String
        Public Property Reply As String
        Public Property LookingFor As String
        Public Property Exception As Exception
        Public Property InnerResults As New List(Of ExpectResult)
        Public Function Summary() As String
            If Success Then
                Return CleanupReply()
            Else
                If Exception IsNot Nothing Then
                    Return "ERROR: " & Exception.Message
                Else
                    Return "FAIL: " & CleanupReply()
                End If
            End If
        End Function

        Public Function CleanupReply() As String
            Dim This As String = Reply
            If This.StartsWith(Command) Then This = This.Trim.Substring(Command.Length).Trim
            If This.Contains(vbLf) Then This = This.Substring(0, This.LastIndexOf(vbLf) - 1)

            'If This.IndexOf(LookingFor) + LookingFor.Length > This.Length Then This = This.Substring(This.IndexOf(LookingFor) + LookingFor.Length)
            Return This
        End Function

        Sub New(success As Boolean, Command As String, LookingFor As String, Reply As String)
            Me.Success = success
            Me.Command = Command
            Me.Reply = Reply
            Me.LookingFor = LookingFor
            Me.Exception = Nothing
        End Sub

        Sub New(Exception As Exception, FailReason As String, Command As String, LookingFor As String)
            Me.Success = False
            Me.Command = Command
            Me.Reply = Nothing
            Me.FailReason = FailReason
            Me.LookingFor = LookingFor
            Me.Exception = Exception
        End Sub

        Sub New(Exception As Exception, FailReason As String, Command As String, LookingFor As String())
            Me.Success = False
            Me.Command = Command
            Me.Reply = Nothing
            Me.FailReason = FailReason
            Me.LookingFor = """" & Join(LookingFor, """,""") & """"
            Me.Exception = Exception
        End Sub

    End Class

    Public Function ExtendedExpect(cmd As String, WaitFor As String, sh As ShellStream) As ExpectResult
        Return ExtendedExpect(cmd, New String() {WaitFor}, sh)
    End Function

    Public Function ExtendedExpect(cmd As String, WaitFor As String(), sh As ShellStream) As ExpectResult
        Dim reader As StreamReader ' = Nothing
        Try
            reader = New StreamReader(sh)
            Dim writer As New StreamWriter(sh) With {.NewLine = vbLf, .AutoFlush = True}
            writer.WriteLine(cmd)
            Dim expiry As DateTime = Now.AddSeconds(Timeout)
            Dim ret As ExpectResult = LookForExpect(cmd, WaitFor, sh, reader, expiry)
            WriteToLog(LogSeverity.Debug, String.Format("action='{0}' waitfor='{1}' timeout={2} success='{3}' reply='{4}'", cmd, WaitFor, Timeout, ret.Success, ret.Summary))
            Return ret

        Catch ex As Exception
            WriteToLog(LogSeverity.Debug, String.Format("action='{0}' waitfor='{1}' timeout={2} result='error' error=''", cmd, WaitFor, Timeout, ex.Message))
            Return New ExpectResult(ex, "Unknown", Command, WaitFor)

        End Try

    End Function

    Private Function ExtendedExpect(cmd As String, WaitFor As String, sh As ShellStream, reader As StreamReader) As ExpectResult
        Return LookForExpect(cmd, {WaitFor}, sh, reader, Now.AddSeconds(Timeout))
    End Function

    Private Function LookForExpect(cmd As String, WaitFor As String(), sh As ShellStream, reader As StreamReader, expiry As DateTime) As ExpectResult
        Return LookForExpect(cmd, WaitFor, sh, reader, expiry, 0)
    End Function

    Private Function LookForExpect(cmd As String, WaitFor As String(), sh As ShellStream, reader As StreamReader, expiry As DateTime, loops As Integer) As ExpectResult
        loops += 1

        If Now > expiry Then Throw New TimeoutException

        'wait for something to read
        While (sh.Length = 0)
            Threading.Thread.Sleep(200)
            If Now > expiry Then Return New ExpectResult(New TimeoutException("Connection attempt timed out"), "Timeout", cmd, WaitFor)
        End While
        WriteToLog(LogSeverity.Debug, String.Format("waitfor='{0}' timeout_at='{1}' reply='no reply yet' result='waiting for reply' cycles={2}", WaitFor, expiry.ToString, loops))

        Dim ret As String = reader.ReadToEnd
        If ret IsNot Nothing Then
            For Each waitforthis As String In WaitFor
                If Regex.IsMatch(ret, waitforthis, RegexOptions.IgnoreCase + RegexOptions.Multiline) Then
                    WriteToLog(LogSeverity.Debug, String.Format("waitfor='{0}' reply='{1}' result='match found' cycles={2}", waitforthis, ret.Replace(cmd, ""), loops))
                    Return New ExpectResult(True, cmd, waitforthis, ret)
                End If
            Next


            Dim rslt As ExpectResult = LookForExpect(cmd, WaitFor, sh, reader, expiry, loops)
            If ret.Length > 1 Then ret = ret.Substring(0, ret.Length - 2)
            rslt.Reply = ret & rslt.Reply.Trim
            WriteToLog(LogSeverity.Debug, String.Format("waitfor='{0}' reply='{1}' result='waiting for value' cycles={2}", WaitFor, rslt.Reply.Replace(cmd, ""), loops))
            Return rslt
        Else
            WriteToLog(LogSeverity.Debug, String.Format("waitfor='{0}' reply='no reply yet' result='waiting for reply' cycles={1}", WaitFor, loops))
            Return LookForExpect(cmd, WaitFor, sh, reader, expiry, loops)
        End If
        '
    End Function

#End Region

End Class
