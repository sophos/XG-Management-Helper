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
Imports XGManagementHelper.EncryptionHelper

Public Class XGShellConnection
    Private WithEvents XG As SshClient
    Public Shared LogFile As String = My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\action.log"
    Public LogLevel As LogSeverity = LogSeverity.Debug
    Private AdvancedShell As Boolean = False
    Private knownpassword As String = ""
    Public Property Version As String = "UNKNOWN"
    Public Property OEM As String = "UNKNOWN"
    Public Property DisplayVersion As String = "UNKNOWN"
    Public Property MRVersion As String = "UNKNOWN"
    Public Property Timeout As Integer = 60
    Public Property FingerprintReceived As String
    Public Property ErrorReceived As String
    Private Property Host As KeyValuePair(Of String, String)
    Public Property AutoTrust As Boolean = False

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

#Region "WriteLog"
    Private Sub WriteToLog(Severity As LogSeverity, Message As String)
        WriteToLog(Host.Key, Severity, Message, LogLevel, knownpassword)
    End Sub

    Public Shared Sub WriteToLog(host As String, Severity As LogSeverity, Message As String, LogLevel As LogSeverity)
        WriteToLog(host, Severity, Message, LogLevel, "")
    End Sub

    Public Shared Sub WriteToLog(Host As String, Severity As LogSeverity, Message As String, LogLevel As LogSeverity, knownpassword As String)
        WriteToLog(LogFile, Host, Severity, Message, LogLevel, knownpassword)
    End Sub

    Public Shared Sub WriteToLog(ByVal LogFile As String, host As String, ByVal Severity As LogSeverity, ByVal Message As String, ByVal LogLevel As LogSeverity, knownpassword As String)
        If Severity > LogLevel Then Exit Sub
        Dim fp As String = IO.Path.GetDirectoryName(LogFile)
        Dim fn As String = IO.Path.GetFileNameWithoutExtension(LogFile)
        Dim fe As String = IO.Path.GetExtension(LogFile)
        If Not fe.StartsWith(".") Then fe = "." & fe
        fn &= String.Format(".{0}{1}", host, fe)

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
        System.IO.File.AppendAllText(IO.Path.Combine(fp, fn), String.Format("{0} - severity=""{1}"" caller=""{2}"" message=""{3}""{4}", timestamp, Severity.ToString, caller, Message.Replace("{", "{{").Replace("}", "}}"), vbNewLine))
    End Sub

#End Region

#Region "Public Methods"

    Sub New(Autotrust As Boolean)
        Me.AutoTrust = Autotrust

    End Sub

    Public Function RegisterToCentral(Host As KeyValuePair(Of String, String), shell_user As String, shell_pass As String, central_user As String, central_pass As String, EnableManagement As Boolean, EnableReporting As Boolean, EnableBackup As Boolean, LogLevel As LogSeverity) As ActionResult
        Me.LogLevel = LogLevel
        knownpassword = central_pass
        'xg = New SshClient(Host, shell_user, shell_pass)
        'WriteToLog(LogSeverity.Debug, String.Format("action='showArgs' host='{0}' shell_user='{1}' shell_pass='{2}' central_user='{3}' central_pass='{4}' EnableManagement={5} EnableReporting={6} EnableBackup={7}", Host.key, shell_user, shell_pass.Length & " chars", central_user, central_pass.Length & " chars", EnableManagement, EnableReporting, EnableBackup))
        Try
            Dim Shell As ShellStream = GetAdvancedShell(Host, shell_user, shell_pass)
            If Not AdvancedShell Then Throw New Exception("Could not connect to shell")
            Dim nfo As ActionResult = ExtendedExpect(Host, "central-register --status", "#", Shell)

            'check if it's already registered to Central
            If nfo.Reply.Contains("currently not registered") Then
                WriteToLog(LogSeverity.Informational, String.Format("action='registration check' host='{0}' result=False message='{1}'", Host.Key, nfo.Summary))

                'if not yet registered, then attempt to register it
                Dim lastnfo As ActionResult = nfo
                nfo = ExtendedExpect(Host, String.Format("opcode -ds nosync SophosCentralRegistration -t json -b '{{""username"":""{0}"", ""password"": ""{1}""}}'", central_user, central_pass), "#", Shell)
                If nfo.Reply.Contains("success") Then
                    WriteToLog(LogSeverity.Debug, String.Format("action='registration check' host='{0}' result=True message='{1}'", Host.Key, nfo.Summary))
                    nfo.Reply = "Registered"
                ElseIf nfo.Reply.Contains("Temporary error while accessing Sophos Central, please try again later") Then
                    nfo.Success = False
                    nfo.Reply = "Temporary error while accessing Sophos Central, please try again later."
                    WriteToLog(LogSeverity.Error, String.Format("action='registration check' host='{0}' result=False message='{1}'", Host.Key, nfo.Summary))
                Else
                    nfo.Success = False
                    WriteToLog(LogSeverity.Error, String.Format("action='registration check' host='{0}' result=False message='{1}'", Host.Key, nfo.Summary))
                End If

            ElseIf nfo.Reply.Contains("currently registered") Then
                WriteToLog(LogSeverity.Debug, String.Format("action='registration check' host='{0}' result=True message='{1}'", Host.Key, nfo.Summary))
                nfo.Reply = "Registered"
            Else
                nfo.Success = False
                WriteToLog(LogSeverity.Debug, String.Format("action='registration check' host='{0}' result='unknown' message='{1}'", Host.Key, nfo.Summary))
            End If

            'device is registered - now set management, reporting, backup services
            If nfo.Success Then
                'ensure it is not managed by cfm or sfm
                nfo = ExtendedExpect(Host, "opcode apiInterface -s nosync -t json -b '{""cmtype"":""-1"",""CCCAsAppMgt"":""1"",""mode"":765}'", "#", Shell)
                If nfo.Reply.Contains("success") Or nfo.Reply.Contains("Cemntral Management is enable") Then
                    'what firmware version is the firewall running?
                    If Version.Contains("17.5.") Then
                        'enable/disable management, reporting, backup
                        nfo = ExtendedExpect(Host, String.Format("opcode sophos_central_enable -s nosync -t json -b '{{ ""cmdiv"": ""{0}"", ""joinmethod"": ""Manual"", ""fwbackup"": ""{2}"" }}'",
                                           If(EnableManagement, "1", "0"), If(EnableReporting, "1", "0"), If(EnableBackup And EnableManagement, "1", "0")), "#", Shell)

                        If nfo.Reply.Contains("success") Then
                            nfo = ExtendedExpect(Host, "central-connect --check_status", "#", Shell)
                            If nfo.Reply.Contains("approval_pending") Then
                                nfo.Reply = "Approval Pending"
                            ElseIf nfo.Reply.Contains("approved_by_customer") Then
                                nfo.Reply = "Central Service(s) enabled"
                            Else
                                nfo.Reply = "E30924:" & nfo.Summary 'leave nfo value to return to the UI
                            End If
                        End If

                    ElseIf Version.Contains("18.") Then
                        'enable/disable management, reporting, backup
                        nfo = ExtendedExpect(Host, String.Format("opcode sophos_central_enable -s nosync -t json -b '{{ ""cmdiv"": ""{0}"", ""crdiv"": ""{1}"", ""joinmethod"": ""Manual"", ""fwbackup"": ""{2}"" }}'",
                                           If(EnableManagement, "1", "0"), If(EnableReporting, "1", "0"), If(EnableBackup And EnableManagement, "1", "0")), "#", Shell)

                        If nfo.Reply.Contains("success") Then
                            nfo = ExtendedExpect(Host, "central-connect --check_status", "#", Shell)
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
            XG.Disconnect()
            Return nfo

        Catch ex As Exception
            Dim ret As ActionResult = InterpretError(Host, ex, "SetAdminPassword")
            WriteToLog(LogSeverity.Error, String.Format("action='connect' caller='SetAdminPassword' host='{0}' message='{1}' error='{1}'", Host.Key, ex.Message, ret.FailReason))
            Return ret

        End Try

    End Function

    Public Function CheckCurrentFirmwareVersion(Host As KeyValuePair(Of String, String), shell_user As String, shell_pass As String, LogLevel As LogSeverity) As ActionResult
        Me.LogLevel = LogLevel
        Try
            Dim Shell As ShellStream = GetAdvancedShell(Host, shell_user, shell_pass)
            If Not AdvancedShell Then Throw New Exception("Could not connect to shell")
            Dim HF As String
            Dim nfo As ActionResult = ExtendedExpect(Host, "cat /conf/soa", "#", Shell)
            If nfo.Reply.Contains("No such file or directory") Then
                HF = "NO HOTFIXES"
            Else
                HF = nfo.Reply
            End If

            Dim Ver As String = String.Format("{0} (v{1}-HF#{2})", DisplayVersion, Version, HF)
            Return New ActionResult(Host, True, "VersionCheck", "", String.Format("{0} ", Ver))

        Catch ex As Exception
            Dim ret As ActionResult = InterpretError(Host, ex, "SetAdminPassword")
            WriteToLog(LogSeverity.Error, String.Format("action='connect' caller='SetAdminPassword' host='{0}' message='{1}' error='{1}'", Host, ex.Message, ret.FailReason))
            Return ret


        End Try
    End Function

    Public Function InstallHotfixes(Host As KeyValuePair(Of String, String), shell_user As String, shell_pass As String, LogLevel As LogSeverity) As ActionResult
        Me.LogLevel = LogLevel
        Try
            Dim Shell As ShellStream = GetAdvancedShell(Host, shell_user, shell_pass)
            If Not AdvancedShell Then Throw New Exception("Could not connect to shell")


            Dim result As ActionResult = ExtendedExpect(Host, "opcode get_SOA -ds nosync", "#", Shell)
            result.Host = Host
            If result.Reply.Contains("200 OK") Then
                Return New ActionResult(Host, True, "InstallHotfixes", "", "OK")
            Else
                Return result
            End If



        Catch ex As Exception
            Dim ret As ActionResult = InterpretError(Host, ex, "InstallHotfixes")
            WriteToLog(LogSeverity.Error, String.Format("action='connect' caller='{3}' host='{0}' message='{2}' error='{1}'", Host.Key, ex.Message, ret.FailReason, ret.Command))
            Return ret

        End Try

    End Function

    Public Function DeRegisterFromCentral(Host As KeyValuePair(Of String, String), shell_user As String, shell_pass As String, LogLevel As LogSeverity) As ActionResult
        Me.LogLevel = LogLevel
        Try
            Dim Shell As ShellStream = GetAdvancedShell(Host, shell_user, shell_pass)
            If Not AdvancedShell Then Return New ActionResult(Host, New Exception("Failed to connect successfully"), "Connection", "Login", "")
            Dim nfo As ActionResult = ExtendedExpect(Host, "central-register --status", "#", Shell)

            'check if it's already registered to Central
            If nfo.Reply.Contains("currently registered") Then
                nfo = ExtendedExpect(Host, "opcode apiInterface -s nosync -t json -b '{""mode"":1324}'", "#", Shell)
                If nfo.Reply.Contains("success") Then
                    WriteToLog(LogSeverity.Informational, String.Format("action='connect' host='{0}' message='cleared registration'", Host.Key))
                    nfo.Reply = "Registration cleared successfully"
                    Return nfo
                Else
                    nfo.Success = False
                    WriteToLog(LogSeverity.Informational, String.Format("action='connect' host='{0}' message='{1}' error='unknown'", Host.Key, nfo))
                    Return nfo
                End If
            Else
                WriteToLog(LogSeverity.Informational, String.Format("action='connect' host='{0}' message='not registered'", Host.Key))
                nfo.Success = False
                nfo.Reply = "Not registered"
                Return nfo
            End If

        Catch ex As Exception
            Dim ret As ActionResult = InterpretError(Host, ex, "SetAdminPassword")
            WriteToLog(LogSeverity.Error, String.Format("action='connect' caller='SetAdminPassword' host='{0}' message='{1}' error='{1}'", Host.Key, ex.Message, ret.FailReason))
            Return ret
        End Try

    End Function

    Public Function SetAdminPassword(Host As KeyValuePair(Of String, String), shell_user As String, shell_pass As String, new_pass As String, LogLevel As LogSeverity) As XGShellConnection.ActionResult
        Me.LogLevel = LogLevel
        'xg = New SshClient(Host, shell_user, shell_pass)
        knownpassword = new_pass
        WriteToLog(LogSeverity.Debug, String.Format("action='showArgs' host='{0}' shell_user='{1}' shell_pass='{2}'", Host, shell_user, shell_pass.Length & " chars"))
        Try
            Dim nfo As ActionResult
            Dim shell As ShellStream = LoginToFirewall(Host, shell_user, shell_pass)
            nfo = ExtendedExpect(Host, "2", "Select Menu Number \[0-4\]:", shell)
            WriteToLog(LogSeverity.Debug, String.Format("action='menu_nav_1' host='{0}' waitfor='Select Menu Number \[0-4\]:' success='{1}' reply='{2}'", Host.Key, nfo.Success, nfo.Summary))
            nfo = ExtendedExpect(Host, "1", "Enter new password:", shell)
            WriteToLog(LogSeverity.Debug, String.Format("action='menu_nav_2' host='{0}' waitfor='Enter new password:' success='{1}' reply='{2}'", Host.Key, nfo.Success, nfo.Summary))
            nfo = ExtendedExpect(Host, new_pass, "Re-Enter new Password:", shell)
            WriteToLog(LogSeverity.Debug, String.Format("action='set_pass' host='{0}' waitfor='Re-Enter new Password:' success='{1}' reply='{2}'", Host.Key, nfo.Success, nfo.Summary))
            nfo = ExtendedExpect(Host, new_pass, {"Password Changed", "Failed to change Password"}, shell)
            Debug.Print(nfo.Reply & ", " & nfo.LookingFor)
            Select Case nfo.LookingFor
                Case "Password Changed"

                Case "Failed to change Password"
                    nfo.Success = False
                    nfo.Reply = nfo.Reply.Replace("Failed to change Password.", "").Trim
            End Select
            WriteToLog(LogSeverity.Debug, String.Format("action='confirm_pass' host='{0}' waitfor='Password Changed'  success='{1}' reply='{2}'", Host.Key, nfo.Success, nfo.Summary))
            XG.Disconnect()

            WriteToLog(LogSeverity.Informational, String.Format("action='SetAdminPassword' host='{0}' success='{1}' reply='{2}'", Host.Key, nfo.Success, nfo.Summary))
            Return nfo

        Catch ex As Exception
            Dim ret As ActionResult = InterpretError(Host, ex, "SetAdminPassword")
            WriteToLog(LogSeverity.Error, String.Format("action='connect' caller='SetAdminPassword' host='{0}' message='{1}' error='{1}'", Host.Key, ex.Message, ret.FailReason))
            Return ret

        End Try

    End Function

    Public Function GetCaptchaSetting(Host As KeyValuePair(Of String, String), shell_user As String, shell_pass As String, LogLevel As LogSeverity) As XGShellConnection.ActionResult
        Me.LogLevel = LogLevel

        WriteToLog(LogSeverity.Debug, String.Format("action='showArgs' host='{0}' shell_user='{1}' shell_pass='{2}'", Host.Key, shell_user, shell_pass.Length & " chars"))
        Try
            Dim nfo As ActionResult
            Dim shell As ShellStream = GetConsole(Host, shell_user, shell_pass)
            nfo = ExtendedExpect(Host, "system captcha_authentication_VPN show", "console>", shell)
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
            Dim ret As ActionResult = InterpretError(Host, ex, "SetAdminPassword")
            WriteToLog(LogSeverity.Error, String.Format("action='connect' caller='SetAdminPassword' host='{0}' message='{1}' error='{1}'", Host.Key, ex.Message, ret.FailReason))
            Return ret
        End Try
    End Function

    Public Function DisableCaptchaOnVPN(Host As KeyValuePair(Of String, String), shell_user As String, shell_pass As String, LogLevel As LogSeverity) As XGShellConnection.ActionResult
        Me.LogLevel = LogLevel

        WriteToLog(LogSeverity.Debug, String.Format("action='showArgs' host='{0}' shell_user='{1}' shell_pass='{2}'", Host.Key, shell_user, shell_pass.Length & " chars"))
        Try
            Dim nfo As ActionResult
            Dim shell As ShellStream = GetConsole(Host, shell_user, shell_pass)
            nfo = ExtendedExpect(Host, "system captcha_authentication_VPN disable", {"console>", "(Y/N) ?"}, shell)
            If nfo.Reply.Contains("% Error: Unknown Parameter 'captcha_authentication_VPN'") Then
                nfo.Success = False
                nfo.Reply = "Option not available on current firmware/hotfix version."
                Return nfo
            End If
            If nfo.LookingFor = "(Y/N) ?" Then
                nfo = ExtendedExpect(Host, "Y", "console>", shell)
            End If

            If nfo.Reply.Contains("VPN zone is turned off") Then
                nfo.Reply = "CAPCHA disabled on VPN zone"
            Else
                nfo.Success = False
            End If

            Return nfo

        Catch ex As Exception
            Dim ret As ActionResult = InterpretError(Host, ex, "SetAdminPassword")
            WriteToLog(LogSeverity.Error, String.Format("action='connect' caller='SetAdminPassword' host='{0}' message='{1}' error='{1}'", Host.Key, ex.Message, ret.FailReason))
            Return ret
        End Try
    End Function

    Public Function EnableCaptchaOnVPN(Host As KeyValuePair(Of String, String), shell_user As String, shell_pass As String, LogLevel As LogSeverity) As XGShellConnection.ActionResult
        Me.LogLevel = LogLevel

        WriteToLog(LogSeverity.Debug, String.Format("action='showArgs' host='{0}' shell_user='{1}' shell_pass='{2}'", Host.Key, shell_user, shell_pass.Length & " chars"))
        Try
            Dim nfo As ActionResult
            Dim shell As ShellStream = GetConsole(Host, shell_user, shell_pass)

            nfo = ExtendedExpect(Host, "system captcha_authentication_VPN enable", {"console>", "(Y/N) ?"}, shell)
            If nfo.Reply.Contains("% Error: Unknown Parameter 'captcha_authentication_VPN'") Then
                nfo.Success = False
                nfo.Reply = "Option not available on current firmware/hotfix version."
                Return nfo
            End If

            If nfo.LookingFor = "(Y/N) ?" Then
                nfo = ExtendedExpect(Host, "Y", "console>", shell)
            End If

            If nfo.Reply.Contains("VPN zone is turned on") Then
                nfo.Reply = "CAPCHA enabled on VPN zone"
            Else
                nfo.Success = False
            End If

            Return nfo

        Catch ex As Exception
            Dim ret As ActionResult = InterpretError(Host, ex, "SetAdminPassword")
            WriteToLog(LogSeverity.Error, String.Format("action='connect' caller='SetAdminPassword' host='{0}' message='{1}' error='{1}'", Host.Key, ex.Message, ret.FailReason))
            Return ret
        End Try
    End Function

    Public Function GetManditoryPasswordResetStatus(Host As KeyValuePair(Of String, String), shell_user As String, shell_pass As String, LogLevel As LogSeverity) As XGShellConnection.ActionResult
        Me.LogLevel = LogLevel

        WriteToLog(LogSeverity.Debug, String.Format("action='showArgs' host='{0}' shell_user='{1}' shell_pass='{2}'", Host.Key, shell_user, shell_pass.Length & " chars"))
        Try
            Dim nfo As ActionResult
            Dim shell As ShellStream = GetConsole(Host, shell_user, shell_pass)
            nfo = ExtendedExpect(Host, "system mandatory_password_reset show", "console>", shell)
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
            Dim ret As ActionResult = InterpretError(Host, ex, "SetAdminPassword")
            WriteToLog(LogSeverity.Error, String.Format("action='connect' caller='SetAdminPassword' host='{0}' message='{1}' error='{1}'", Host.Key, ex.Message, ret.FailReason))
            Return ret
        End Try
    End Function

    Public Function DisableManditoryPasswordReset(Host As KeyValuePair(Of String, String), shell_user As String, shell_pass As String, LogLevel As LogSeverity) As XGShellConnection.ActionResult
        Me.LogLevel = LogLevel

        WriteToLog(LogSeverity.Debug, String.Format("action='showArgs' host='{0}' shell_user='{1}' shell_pass='{2}'", Host.Key, shell_user, shell_pass.Length & " chars"))
        Try
            Dim nfo As ActionResult
            Dim shell As ShellStream = GetConsole(Host, shell_user, shell_pass)
            nfo = ExtendedExpect(Host, "system mandatory_password_reset disable", {"console>", "(Y/N) ?"}, shell)
            If nfo.Reply.Contains("% Error: Unknown Parameter 'mandatory_password_reset'") Then
                nfo.Success = False
                nfo.Reply = "Option not available on current firmware/hotfix version."
                Return nfo
            End If
            If nfo.LookingFor = "(Y/N) ?" Then
                nfo = ExtendedExpect(Host, "Y", "console>", shell)
            End If

            If nfo.Reply.Contains("is turned off") Then
                nfo.Reply = "Password reset pop-up is DISABLED"
            Else
                nfo.Success = False
            End If

            Return nfo

        Catch ex As Exception
            Dim ret As ActionResult = InterpretError(Host, ex, "SetAdminPassword")
            WriteToLog(LogSeverity.Error, String.Format("action='connect' caller='SetAdminPassword' host='{0}' message='{1}' error='{1}'", Host.Key, ex.Message, ret.FailReason))
            Return ret
        End Try
    End Function

    Public Function SetAPIAccess(Host As KeyValuePair(Of String, String), shell_user As String, shell_pass As String, AllowedIPs() As String, LogLevel As LogSeverity) As XGShellConnection.ActionResult
        Me.LogLevel = LogLevel
        'xg = New SshClient(Host, shell_user, shell_pass)
        Dim setstring As String = String.Format("""{0}""", Join(AllowedIPs))
        WriteToLog(LogSeverity.Debug, String.Format("action='showArgs' host='{0}' shell_user='{1}' shell_pass='{2}'", Host.Key, shell_user, shell_pass.Length & " chars"))
        Try
            Dim shell As ShellStream = GetAdvancedShell(Host, shell_user, shell_pass)
            Dim nfo As ActionResult = ExtendedExpect(Host, String.Format("opcode -s nosync -t json -d -D apiInterface -b '{{""mode"": 804,  ""isenable"": ""true"",  ""allowedipaddress"": [{0}]}}'", setstring), "#", shell)

            'check if it's already registered to Central
            If nfo.Reply.Contains("success") Then
                nfo = ExtendedExpect(Host, "opcode apiInterface -s nosync -t json -b '{""mode"":1324}'", "#", shell)
                If nfo.Reply.Contains("success") Then
                    WriteToLog(LogSeverity.Informational, String.Format("action='connect' host='{0}' message='cleared registration'", Host.Key))
                    nfo.Reply = "Updated successfully"
                    Return nfo
                Else
                    nfo.Success = False
                    WriteToLog(LogSeverity.Informational, String.Format("action='connect' host='{0}' message='{1}' error='unknown'", Host.Key, nfo))
                    Return nfo
                End If
            Else
                WriteToLog(LogSeverity.Informational, String.Format("action='connect' host='{0}' message='not registered'", Host.Key))
                nfo.Success = False
                nfo.Reply = "Not registered"
                Return nfo
            End If

            WriteToLog(LogSeverity.Informational, String.Format("action='SetAdminPassword' host='{0}' success='{1}' reply='{2}'", Host.Key, nfo.Success, nfo.Summary))
            Return nfo

        Catch ex As Exception
            Dim ret As ActionResult = InterpretError(Host, ex, "SetAdminPassword")
            WriteToLog(LogSeverity.Error, String.Format("action='connect' caller='SetAdminPassword' host='{0}' message='{1}' error='{1}'", Host.Key, ex.Message, ret.FailReason))
            Return ret

        End Try

    End Function

    Public Function StatusCheck(Host As KeyValuePair(Of String, String), shell_user As String, shell_pass As String, LogLevel As LogSeverity) As ActionResult
        Me.LogLevel = LogLevel
        Try
            Dim Shell As ShellStream = LoginToFirewall(Host, shell_user, shell_pass)
            If XG.IsConnected Then
                Return New ActionResult(Host, True, "StatusCheck", "", "OK")
            Else
                Return New ActionResult(Host, False, "StatusCheck", "", "Unable to connect successfully")
            End If

        Catch ex As Exception
            Dim ret As ActionResult = InterpretError(Host, ex, "StatusCheck")
            WriteToLog(LogSeverity.Error, String.Format("action='connect' caller='StatusCheck' host='{0}' message='{1}' error='{1}'", Host.Key, ex.Message, ret.FailReason))
            Return ret

        End Try

    End Function

    Public Function CheckUnchangedUserPasswords(Host As KeyValuePair(Of String, String), shell_user As String, shell_pass As String, LogLevel As LogSeverity) As XGShellConnection.ActionResult
        Me.LogLevel = LogLevel

        WriteToLog(LogSeverity.Debug, String.Format("action='showArgs' host='{0}' shell_user='{1}' shell_pass='{2}'", Host.Key, shell_user, shell_pass.Length & " chars"))
        Try
            Dim nfo As ActionResult
            Dim shell As ShellStream = GetConsole(Host, shell_user, shell_pass)
            nfo = ExtendedExpect(Host, "system localusers localuser_list_not_changed_passwords show", "console>", shell)
            If nfo.Reply.Contains("% Error: Unknown Parameter 'mandatory_password_reset'") Then
                nfo.Success = False
                nfo.Reply = "Option not available on current firmware/hotfix version."
                Return nfo
            End If


            Return nfo

        Catch ex As Exception
            Dim ret As ActionResult = InterpretError(Host, ex, "SetAdminPassword")
            WriteToLog(LogSeverity.Error, String.Format("action='connect' caller='SetAdminPassword' host='{0}' message='{1}' error='{1}'", Host.Key, ex.Message, ret.FailReason))
            Return ret
        End Try
    End Function

    Public Function ChangeUnchangedUserPasswords(Host As KeyValuePair(Of String, String), shell_user As String, shell_pass As String, pin As String, LogLevel As LogSeverity) As XGShellConnection.ActionResult
        Me.LogLevel = LogLevel

        WriteToLog(LogSeverity.Debug, String.Format("action='showArgs' host='{0}' shell_user='{1}' shell_pass='{2}'", Host.Key, shell_user, shell_pass.Length & " chars"))
        Try
            Dim nfo As ActionResult
            Dim shell As ShellStream = GetConsole(Host, shell_user, shell_pass)
            nfo = ExtendedExpect(Host, String.Format("system localusers append_PIN_to_password all_localuser PIN {0}", pin), {"console>", "(Y/N)"}, shell)
            If nfo.Reply.Contains("% Error: Unknown Parameter 'mandatory_password_reset'") Then
                nfo.Success = False
                nfo.Reply = "Option not available on current firmware/hotfix version."
                Return nfo
            End If
            If nfo.LookingFor = "(Y/N)" Then
                nfo = ExtendedExpect(Host, "Y", "console>", shell)
            End If

            If nfo.Reply.Contains("changed successfully") Then
                Dim m As Match = nfo.ReplyMatch("Passwords for \d+ users have been changed successfully")
                nfo.Reply = m.Value
            Else
                nfo.Success = False
            End If

            Return nfo

        Catch ex As Exception
            Dim ret As ActionResult = InterpretError(Host, ex, "SetAdminPassword")
            WriteToLog(LogSeverity.Error, String.Format("action='connect' caller='SetAdminPassword' host='{0}' message='{1}' error='{1}'", Host.Key, ex.Message, ret.FailReason))
            Return ret
        End Try
    End Function

#End Region

#Region "Private Comms Methods"

    Private Function LoginToFirewall(Host As KeyValuePair(Of String, String), shell_user As String, shell_pass As String) As ShellStream
        XG = New SshClient(Host.Key, shell_user, shell_pass)
        Me.Host = Host
        WriteToLog(LogSeverity.Debug, String.Format("action='showArgs' host='{0}' shell_user='{1}' shell_pass='{2}'", Host, shell_user, shell_pass.Length & " chars"))
        XG.Connect()

        'Dim Version As String = xg.ConnectionInfo.ServerVersion
        Dim nfo As ActionResult '= "Not Connected"
        If XG.IsConnected Then
            Dim shell As ShellStream = XG.CreateShellStream("dumb", 80, 24, 800, 600, 1024)

            Threading.Thread.Sleep(100)
            Dim r As New StreamReader(shell)
            nfo = ExtendedExpect(Host, "Login", ":", shell, r)
            Dim vMatch As Match = nfo.ReplyMatch("Firmware\s+Version\s+(?<DISPLAYVERSION>(?<OEM>\w+)\s+(?<VERSION>[0-9.]+)\s(?<MR>[a-zA-Z0-9-.]*))")
            If vMatch.Success Then
                Using key As SecureRegistryKey = GetHostKey(Host.Key)
                    DisplayVersion = vMatch.Groups("DISPLAYVERSION").Value
                    Version = vMatch.Groups("VERSION").Value
                    OEM = vMatch.Groups("OEM").Value
                    MRVersion = vMatch.Groups("MR").Value
                    key.SetValue("LastUpdate", Now.ToString)
                    key.SetValue("DisplayVersion", DisplayVersion)
                    key.SetValue("Version", Version)
                    key.SetValue("OEM", OEM)
                    key.SetValue("MRVersion", MRVersion)

                    nfo = DropToAdvancedShell(Host, shell)
                    If nfo.Success Then
                        Dim m As Match = nfo.ReplyMatch("(?<model>\w+?)_\w+\s+[0-9.]+?\s+[a-zA-Z0-9-.]+?#")
                        If m.Success Then key.SetValue("Model", m.Groups("model").Value)
                        nfo = ExtendedExpect(Host, "uptime", "#", shell)
                        m = nfo.ReplyMatch("(?<systime>\d+:\d+:\d+)\sup\s(?<uptime>.*?),\s+load average:\s+(?<load1>.*?),\s+(?<load2>.*?),\s+(?<load3>[0-9.]+)")
                        key.SetValue("SystemTime", m.Groups("systime").Value)
                        key.SetValue("Uptime", m.Groups("uptime").Value)
                        key.SetValue("LoadAvg1min", m.Groups("load1").Value)
                        key.SetValue("LoadAvg5min", m.Groups("load2").Value)
                        key.SetValue("Loadavg15min", m.Groups("load3").Value)
                        BackOutOfAdvancedShell(shell)
                    End If

                End Using
            End If
            Debug.Print("Display Version: {0}, Version: {1}, MR: {2}, OEM: {3}", DisplayVersion, Version, MRVersion, OEM)

            WriteToLog(LogSeverity.Debug, String.Format("action='connect' host='{0}' message='Connected successfully' version='{1}'", Host, DisplayVersion))
            AdvancedShell = False
            Return shell
        End If

        WriteToLog(LogSeverity.Informational, String.Format("action='connect' host='{0}' message='Not Connected' error='unknown'", Host.Key))
        Return Nothing

    End Function

    Private Function GetConsole(Host As KeyValuePair(Of String, String), shell_user As String, shell_pass As String) As ShellStream
        WriteToLog(LogSeverity.Debug, String.Format("action='showArgs' host='{0}' shell_user='{1}' shell_pass='{2}'", Host, shell_user, shell_pass.Length & " chars"))
        If AdvancedShell Then Throw New Exception("Already dropped to advanced shell.")
        Dim nfo As ActionResult
        Dim shell As ShellStream = LoginToFirewall(Host, shell_user, shell_pass)
        nfo = ExtendedExpect(Host, "4", "console>", shell)
        Return shell
        '
    End Function

    Private Function GetAdvancedShell(Host As KeyValuePair(Of String, String), shell_user As String, shell_pass As String) As ShellStream
        WriteToLog(LogSeverity.Debug, String.Format("action='showArgs' host='{0}' shell_user='{1}' shell_pass='{2}'", Host, shell_user, shell_pass.Length & " chars"))
        Dim shell As ShellStream = LoginToFirewall(Host, shell_user, shell_pass)
        Dim nfo As ActionResult = DropToAdvancedShell(Host, shell)
        Return shell
        '
    End Function

    Private Function DropToAdvancedShell(host As KeyValuePair(Of String, String), shell As ShellStream) As ActionResult
        Dim nfo As ActionResult = ExtendedExpect(host, "5", "Select Menu Number \[0-4\]:", shell)
        If nfo.Success Then nfo = ExtendedExpect(host, "3", "#", shell)

        AdvancedShell = True
        Return nfo
    End Function

    Private Function BackOutOfAdvancedShell(shell As ShellStream) As ActionResult
        Dim nfo As ActionResult = ExtendedExpect(Host, "exit", "Select Menu Number \[0-4\]:", shell)
        If nfo.Success Then nfo = ExtendedExpect(Host, "0", "Select Menu Number \[0-7\]:", shell)
        AdvancedShell = False
        Return nfo
        '
    End Function

#End Region

#Region "SSH Event Handlers"

    Private Sub XG_HostKeyReceived(sender As Object, e As HostKeyEventArgs) Handles XG.HostKeyReceived
        Using key As SecureRegistryKey = GetHostKey(Me.Host.Key)
            Dim ExpectedFingerprint As String = key.GetSecureValue("Fingerprint")
            If ExpectedFingerprint IsNot Nothing Then
                If ExpectedFingerprint.StartsWith(Me.Host.Key) Then
                    ExpectedFingerprint = ExpectedFingerprint.Substring(Me.Host.Key.Length)
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
                    Dim fc As New FingerprintConfirmation(FingerprintReceived, True, Host.Key)
                    e.CanTrust = fc.ShowDialog = DialogResult.OK
                End If
            Else
                If AutoTrust Then
                    e.CanTrust = True
                Else
                    Dim fc As New FingerprintConfirmation(FingerprintReceived, False, Host.Key)
                    e.CanTrust = fc.ShowDialog = DialogResult.OK
                End If
            End If
            If e.CanTrust Then
                key.SetSecureValue("Fingerprint", Me.Host.Key & FingerprintReceived)
            End If
        End Using


    End Sub

    Private Sub XG_ErrorOccurred(sender As Object, e As ExceptionEventArgs) Handles XG.ErrorOccurred
        ErrorReceived = e.Exception.Message
    End Sub

#End Region

#Region "Private Methods"

    Private Function ByteArrayToHex(ByRef ByteArray() As Byte) As String
        Dim l As Long, strRet As String = ""

        For l = LBound(ByteArray) To UBound(ByteArray)
            strRet = strRet & Hex$(ByteArray(l)) & " "
        Next l

        'Remove last space at end.
        ByteArrayToHex = Left$(strRet, Len(strRet) - 1)
    End Function

    Private Function InterpretError(Host As KeyValuePair(Of String, String), ex As Exception, action As String) As ActionResult
        If ex.Message.Contains("after a period of time") Then
            Return New ActionResult(Host, New TimeoutException("Connection Timed Out", ex), "Timeout", action, "")
        ElseIf ex.Message.Contains("password") Then
            Return New ActionResult(Host, New Exception("Username or password not accepted", ex), "Credentials", action, "")
        ElseIf ex.Message.Contains("connect to shell") Then
            Return New ActionResult(Host, New Exception("Connection Failed", ex), "Connection", action, "")
        Else
            Return New ActionResult(Host, ex, "unknown", action, "")
        End If
    End Function

#End Region

#Region "Expect"
    Public Class ActionRequest
        Public Property Host As KeyValuePair(Of String, String)
        Public Property Action As String
        Public Property LogLevel As LogSeverity
        Private Property Params As KeyValuePair(Of String, String)()
        Sub New(Host As KeyValuePair(Of String, String), action As String, LogLevel As LogSeverity, ParamArray Params() As KeyValuePair(Of String, String))
            Me.Host = Host
            Me.Action = action
            Me.LogLevel = LogLevel
            Me.Params = Params
        End Sub
        Public Function GetValue(Key As String) As String
            Try
                For Each param As KeyValuePair(Of String, String) In Params
                    If param.Key.ToLower.Equals(Key.ToLower) Then Return param.Value
                Next
            Catch ex As Exception

            End Try
            Return Nothing
        End Function
        Public Sub SetValue(Key As String, Value As String)
            Try
                Dim newparam As New KeyValuePair(Of String, String)(Key, Value)
                Dim newparams As KeyValuePair(Of String, String)() = {}
                If Params.Contains(newparam, New KeyComparer) Then
                    For Each thisParam As KeyValuePair(Of String, String) In Params
                        If Not thisParam.Key.ToLower.Equals(newparam.Key.ToLower) Then
                            ReDim Preserve newparams(newparams.Count)
                            newparams(newparams.GetUpperBound(0)) = thisParam
                        End If
                    Next
                End If
                ReDim Preserve newparams(newparams.Count)
                newparams(newparams.GetUpperBound(0)) = newparam

                ReDim Params(newparams.GetUpperBound(0))
                newparams.CopyTo(Params, 0)

            Catch ex As Exception

            End Try
        End Sub
    End Class

    Public Class ActionResult
        Public Property Host As KeyValuePair(Of String, String)
        Public Property Action As String
        Public Property Success As Boolean
        Public Property FailReason As String
        Public Property Command As String
        Public Property Reply As String
        Public Property LookingFor As String
        Public Property Exception As Exception
        Public Property InnerResults As New List(Of ActionResult)
        Public Property LogLevel As LogSeverity
        Public Property Request As ActionRequest
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

        Public Function ReplyMatch(RegularExpression As String) As Match
            Return Regex.Match(Reply, RegularExpression)
        End Function

        Public Function ReplyMatches(RegularExpression As String) As MatchCollection
            Return Regex.Matches(Reply, RegularExpression)
        End Function

        Public Function CleanupReply() As String
            Dim This As String = Reply
            If This.StartsWith(Command) Then This = This.Trim.Substring(Command.Length).Trim
            If This.Contains(vbLf) Then This = This.Substring(0, This.LastIndexOf(vbLf) - 1)

            'If This.IndexOf(LookingFor) + LookingFor.Length > This.Length Then This = This.Substring(This.IndexOf(LookingFor) + LookingFor.Length)
            Return This
        End Function

        Sub New(host As KeyValuePair(Of String, String), success As Boolean, Command As String, LookingFor As String, Reply As String)
            Me.Host = host
            Me.Success = success
            Me.Command = Command
            Me.Reply = Reply
            Me.LookingFor = LookingFor
            Me.Exception = Nothing
        End Sub

        Sub New(host As KeyValuePair(Of String, String), Exception As Exception, FailReason As String, Command As String, LookingFor As String)
            Me.Host = host
            Me.Success = False
            Me.Command = Command
            Me.Reply = Nothing
            Me.FailReason = FailReason
            Me.LookingFor = LookingFor
            Me.Exception = Exception
        End Sub

        Sub New(host As KeyValuePair(Of String, String), Exception As Exception, FailReason As String, Command As String, LookingFor As String())
            Me.Host = host
            Me.Success = False
            Me.Command = Command
            Me.Reply = Nothing
            Me.FailReason = FailReason
            Me.LookingFor = """" & Join(LookingFor, """,""") & """"
            Me.Exception = Exception
        End Sub

    End Class

    Public Function ExtendedExpect(Host As KeyValuePair(Of String, String), cmd As String, WaitFor As String, sh As ShellStream) As ActionResult
        Return ExtendedExpect(Host, cmd, New String() {WaitFor}, sh)
    End Function

    Public Function ExtendedExpect(Host As KeyValuePair(Of String, String), cmd As String, WaitFor As String(), sh As ShellStream) As ActionResult
        Dim reader As StreamReader ' = Nothing
        Try
            reader = New StreamReader(sh)
            Dim writer As New StreamWriter(sh) With {.NewLine = vbLf, .AutoFlush = True}
            writer.WriteLine(cmd)
            Dim expiry As DateTime = Now.AddSeconds(Timeout)
            Dim ret As ActionResult = LookForExpect(Host, cmd, WaitFor, sh, reader, expiry)
            ret.Host = Host
            WriteToLog(LogSeverity.Debug, String.Format("action='{0}' waitfor='{1}' timeout={2} success='{3}' reply='{4}'", cmd, WaitFor, Timeout, ret.Success, ret.Summary))
            Return ret

        Catch ex As Exception
            WriteToLog(LogSeverity.Debug, String.Format("action='{0}' waitfor='{1}' timeout={2} result='error' error=''", cmd, WaitFor, Timeout, ex.Message))
            Return New ActionResult(Host, ex, "Unknown", Command, WaitFor)

        End Try

    End Function

    Private Function ExtendedExpect(Host As KeyValuePair(Of String, String), cmd As String, WaitFor As String, sh As ShellStream, reader As StreamReader) As ActionResult
        Return LookForExpect(Host, cmd, {WaitFor}, sh, reader, Now.AddSeconds(Timeout))
    End Function

    Private Function LookForExpect(Host As KeyValuePair(Of String, String), cmd As String, WaitFor As String(), sh As ShellStream, reader As StreamReader, expiry As DateTime) As ActionResult
        Return LookForExpect(Host, cmd, WaitFor, sh, reader, expiry, 0)
    End Function

    Private Function LookForExpect(Host As KeyValuePair(Of String, String), cmd As String, WaitFor As String(), sh As ShellStream, reader As StreamReader, expiry As DateTime, loops As Integer) As ActionResult
        loops += 1

        If Now > expiry Then Throw New TimeoutException

        'wait for something to read
        While (sh.Length = 0)
            Threading.Thread.Sleep(200)
            If Now > expiry Then Return New ActionResult(Host, New TimeoutException("Connection attempt timed out"), "Timeout", cmd, WaitFor)
        End While
        WriteToLog(LogSeverity.Debug, String.Format("waitfor='{0}' timeout_at='{1}' reply='no reply yet' result='waiting for reply' cycles={2}", WaitFor, expiry.ToString, loops))

        Dim ret As String = reader.ReadToEnd
        If ret IsNot Nothing Then
            For Each waitforthis As String In WaitFor
                If Regex.IsMatch(ret, waitforthis, RegexOptions.IgnoreCase + RegexOptions.Multiline) Then
                    WriteToLog(LogSeverity.Debug, String.Format("waitfor='{0}' reply='{1}' result='match found' cycles={2}", waitforthis, ret.Replace(cmd, ""), loops))
                    Return New ActionResult(Host, True, cmd, waitforthis, ret)
                End If
            Next

            Dim rslt As ActionResult = LookForExpect(Host, cmd, WaitFor, sh, reader, expiry, loops)
            If ret.Length > 1 Then ret = ret.Substring(0, ret.Length - 2)
            rslt.Reply = ret & rslt.Reply.Trim
            WriteToLog(LogSeverity.Debug, String.Format("waitfor='{0}' reply='{1}' result='waiting for value' cycles={2}", WaitFor, rslt.Reply.Replace(cmd, ""), loops))
            Return rslt
        Else
            WriteToLog(LogSeverity.Debug, String.Format("waitfor='{0}' reply='no reply yet' result='waiting for reply' cycles={1}", WaitFor, loops))
            Return LookForExpect(Host, cmd, WaitFor, sh, reader, expiry, loops)
        End If
        '
    End Function

#End Region

End Class
