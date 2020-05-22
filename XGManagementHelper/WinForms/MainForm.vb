' Copyright 2020  Sophos Ltd.  All rights reserved.
' Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
' You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
' Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, 
' WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing 
' permissions and limitations under the License.

Imports System.Management
Imports System.ComponentModel
Imports System.Security.AccessControl
Imports Microsoft.Win32
Imports XGManagementHelper.EncryptionHelper
Imports XGManagementHelper.Randomness
Imports System.Text.RegularExpressions

Public Class MainForm

    ReadOnly Property Workers As New List(Of BackgroundWorker)
    ReadOnly NewHosts As New List(Of KeyValuePair(Of String, String))
    ReadOnly IncrementalAutoSave As Boolean = True
    ReadOnly Hosts As New List(Of KeyValuePair(Of String, String))
    ReadOnly Shortcuts As New List(Of String)
    ReadOnly Property LogLocation As String = IO.Path.Combine(My.Computer.FileSystem.SpecialDirectories.MyDocuments, "XG Management Helper")
    ReadOnly VersionCheckURL As String = "https://raw.githubusercontent.com/sophos/XG-Management-Helper/master/VERSION.TXT"
    Private FormIsDirty As Boolean
    Private CentralUser As String = ""
    Private CentralPass As String = ""
    Private ShellCommonPass As String = ""
    Private LoadingBool As Boolean = False
    Private UserRequestedUpdateCheck As Boolean = False
    Private LastRefresh As DateTime = DateTime.MinValue

    Private WithEvents WGET As New HttpGet

#Region "Form Events"

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadingBool = True
        InitTableColumns()
        SetRegistryPermissions()
        LoadSavedValues()
        RefreshHostsList(False)

        XGShellConnection.LogFile = IO.Path.Combine(LogLocation, "application.log")
        If IO.File.Exists(IO.Path.Combine(LogLocation, "application.log")) Then
            If IO.File.Exists(IO.Path.Combine(LogLocation, "application.last.log")) Then IO.File.Delete(IO.Path.Combine(LogLocation, "application.last.log"))
            IO.File.Move(XGShellConnection.LogFile, IO.Path.Combine(LogLocation, "application.last.log"))
        End If

        Try
            If Not IO.Directory.Exists(LogLocation) Then IO.Directory.CreateDirectory(LogLocation)
        Catch ex As Exception

        End Try
        LoadingBool = False
        Me.MaximumSize = Screen.FromRectangle(Me.Bounds).WorkingArea.Size
        WritePasswordChangeLog()

    End Sub

    Private Sub MainForm_Move(sender As Object, e As EventArgs) Handles Me.Move
        Me.MaximumSize = Screen.FromRectangle(Me.Bounds).WorkingArea.Size
    End Sub

    Private Sub MainForm_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        SaveSettings(FormIsDirty)
        SaveHosts(FormIsDirty)
    End Sub

#End Region

#Region "Form Control Events"

    Private Function GetSet() As XGShellConnection.LogSeverity
        Dim LogLevel As XGShellConnection.LogSeverity = XGShellConnection.LogSeverity.Informational
        '
        If My.Computer.Keyboard.CtrlKeyDown Then
            LogLevel = XGShellConnection.LogSeverity.Debug
        End If
        '
        Dim sfcount As Integer = ResultsListView.CheckedItems.Count
        If sfcount = 0 Then sfcount = Hosts.Count
        If sfcount = 0 Then Return Nothing
        '
        StatusToolStripStatusLabel.Text = "Status: Getting ready"
        TopPanel.Enabled = False
        Return LogLevel
    End Function

    Private Function GetListViewItemForHost(host As KeyValuePair(Of String, String)) As ListViewItem
        For Each this As ListViewItem In ResultsListView.Items
            If this.Text = host.Key Then Return this
        Next
        Return Nothing
    End Function

    Private Sub CentralUserTextBox_TextChanged(sender As Object, e As EventArgs) Handles ShellPassTextBox.TextChanged
        If LoadingBool Then Exit Sub
        FormIsDirty = True
        ShellCommonPass = ShellPassTextBox.Text
        SaveSettings(IncrementalAutoSave)
        EnableDisable()

    End Sub

    Private Sub ResultsListView_ItemChecked(sender As Object, e As ItemCheckedEventArgs) Handles ResultsListView.ItemChecked
        EnableDisable()
    End Sub

    Private Sub AllCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles AllCheckBox.CheckedChanged

        For Each lvi As ListViewItem In ResultsListView.Items
            lvi.Checked = sender.checked
        Next
    End Sub

    Private Sub ToggleCheckButton_Click(sender As Object, e As EventArgs) Handles ToggleCheckButton.Click
        For Each lvi As ListViewItem In ResultsListView.Items
            lvi.Checked = Not lvi.Checked
        Next
    End Sub

    Private Sub ResultsListView_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles ResultsListView.MouseDoubleClick

        If EditHostToolStripMenuItem.Checked Then
            EditSelectedHost()
        Else
            OpenSelectedHostWebadmin()
        End If
    End Sub

    Private Sub ResultsListView_MouseClick(sender As Object, e As MouseEventArgs)
        If ResultsListView.SelectedItems.Count = 1 Then
            If e.Button = Windows.Forms.MouseButtons.Right Then
                FirewallsRightClickContextMenu.Show(CType(sender, Control), e.Location)
            End If
        End If
    End Sub

    Private Sub Worker_DoWork(sender As Object, e As DoWorkEventArgs) 'Handles worker.DoWork 
        Dim Request As XGShellConnection.ActionRequest = e.Argument
        Dim SSH As New XGShellConnection(TrustInitialSSHFingerprintToolStripMenuItem.Checked)
        Dim result As XGShellConnection.ActionResult
        Dim pass As String = Request.Host.Value
        If pass = "" Then pass = ShellCommonPass 'ShellPassTextBox.Text    
        '
        Select Case Request.Action
            Case "StatusCheck"
                result = SSH.StatusCheck(Request.Host, "admin", pass, Request.LogLevel)
                '
            Case "HotfixInstall"
                result = SSH.InstallHotfixes(Request.Host, "admin", pass, Request.LogLevel)
                '
            Case "EnableCentralManagementAndReporting", "EnableCentralManagement", "EnableCentralReporting"
                Dim CentralUser As String = Request.GetValue("CentralUser")
                Dim CentralPass As String = Request.GetValue("CentralPass")

                If Request.Action = "EnableCentralManagementAndReporting" Then
                    result = SSH.RegisterToCentral(Request.Host, "admin", pass, CentralUser, CentralPass, True, True, True, Request.LogLevel)
                ElseIf Request.Action = "EnableCentralManagement" Then
                    result = SSH.RegisterToCentral(Request.Host, "admin", pass, CentralUser, CentralPass, True, False, True, Request.LogLevel)
                Else
                    result = SSH.RegisterToCentral(Request.Host, "admin", pass, CentralUser, CentralPass, False, True, False, Request.LogLevel)
                End If
                '
            Case "DeRegisterFromCentral"
                result = SSH.DeRegisterFromCentral(Request.Host, "admin", pass, Request.LogLevel)
                '
            Case "SetAdminPassword"
                Using key As SecureRegistryKey = GetHostKey(Request.Host.Key)
                    Dim Generate As Boolean = Request.GetValue("Generate").Equals("True")
                    Dim Newpass As String = Request.GetValue("Newpass")
                    Dim EnforceComplexity As String = Request.GetValue("EnforceComplexity").Equals("True")

                    Dim loglines As String = ""
                    If Generate OrElse Not Newpass = pass Then
                        If Generate Then Newpass = GetRandomString(32, EnforceComplexity)
                        key.SetSecureValue("PasswordTry", Newpass)
                        key.SetValue("PasswordTryAt", Now.ToString)
                        Request.SetValue("Newpass", Newpass)
                        result = SSH.SetAdminPassword(Request.Host, "admin", pass, Newpass, Request.LogLevel)

                        If result.Success Then
                            key.SetSecureValue("PasswordSet", Newpass)
                            key.SetValue("PasswordSetAt", Now.ToString)
                        End If
                    Else
                        result = New XGShellConnection.ActionResult(Request.Host, True, "Change Password", "", "No change needed")
                    End If
                End Using
                '
            Case "CheckVPNCAPCHA"
                result = SSH.GetCaptchaSetting(Request.Host, "admin", pass, Request.LogLevel)
                '
            Case "EnableVPNCAPCHA"
                result = SSH.EnableCaptchaOnVPN(Request.Host, "admin", pass, Request.LogLevel)
                '
            Case "DisableVPNCAPCHA"
                result = SSH.DisableCaptchaOnVPN(Request.Host, "admin", pass, Request.LogLevel)
                '
            Case "CheckPasswordResetPopup"
                result = SSH.GetManditoryPasswordResetStatus(Request.Host, "admin", pass, Request.LogLevel)
                '
            Case "DisablePasswordResetPopup"
                result = SSH.DisableManditoryPasswordReset(Request.Host, "admin", pass, Request.LogLevel)
                '
            Case "CheckUnchangedUsers"
                result = SSH.CheckUnchangedUserPasswords(Request.Host, "admin", pass, Request.LogLevel)
                '
            Case "ChangeUserPasswords"
                result = SSH.ChangeUnchangedUserPasswords(Request.Host, "admin", pass, Request.GetValue("token"), Request.LogLevel)
                '
            Case Else
                result = New XGShellConnection.ActionResult(Request.Host, New NotImplementedException(Request.Action & "is not implemented yet"), Request.Action, "", "")
                '
        End Select
        result.Request = Request
        result.Action = Request.Action
        result.LogLevel = Request.LogLevel
        e.Result = result
        '
    End Sub

    Private Sub Worker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) 'Handles worker.RunWorkerCompleted

        If ResultsListView.InvokeRequired Then
            ResultsListView.Invoke(Sub()
                                       'MsgBox("Invoked")
                                       RunWorkerCompleted(sender, e)
                                   End Sub)
        Else
            'MsgBox("Not Invoked")
            RunWorkerCompleted(sender, e)
        End If

    End Sub

    Private Sub WGET_AsyncDownloadComplete(sender As Object, e As HttpGet.AsyncDownloadEventArgs) Handles WGET.AsyncDownloadComplete
        Dim m As Match = Regex.Match(e.Data, "Version\s(?<version>\d+\.\d+(\.\d+\.\d+)?)")

        If m.Success Then

            Dim thisv As String() = Split(Application.ProductVersion, ".")
            Dim WebVersion As String = m.Groups("version").Value
            'WebVersion = "Version 1.1.0.2" 'for testing. comment out before release!
            Dim webv As String() = Split(WebVersion, ".")

            Dim UpdateAvailable As Boolean = False
            Dim NewerThanWeb As Boolean = False

            If thisv.Count = 4 And webv.Count = 4 Then
                If webv(0) > thisv(0) Then
                    UpdateAvailable = True
                ElseIf webv(0) = thisv(0) Then
                    If webv(1) > thisv(1) Then
                        UpdateAvailable = True
                    ElseIf webv(1) = thisv(1) Then
                        If webv(2) > thisv(2) Then
                            UpdateAvailable = True
                        ElseIf webv(2) = thisv(2) Then
                            If webv(3) > thisv(3) Then
                                UpdateAvailable = True
                            ElseIf webv(3) = thisv(3) Then
                            Else
                                NewerThanWeb = True
                            End If
                        Else
                            NewerThanWeb = True
                        End If
                    Else
                        NewerThanWeb = True
                    End If
                Else
                    NewerThanWeb = True
                End If
            Else
                NewerThanWeb = True
            End If

            Using key As SecureRegistryKey = GetApplicationRootKey()
                VersionLabel.ForeColor = Color.FromKnownColor(KnownColor.ControlDarkDark)
                VersionLabel.Cursor = Cursors.Default
                If NewerThanWeb Then
                    VersionLabel.Text = String.Format("v{0} (Pre-release version)", Application.ProductVersion)
                    key.SetValue("LastVersionCheckResult", "(Pre-release version)")
                    If UserRequestedUpdateCheck Then MsgBox("You are running a pre-release version. No update is available.", MsgBoxStyle.Information, "Update Check")
                ElseIf UpdateAvailable Then
                    VersionLabel.Text = String.Format("v{0} (Update available)", Application.ProductVersion)
                    key.SetValue("LastVersionCheckResult", "(Update available)")
                    VersionLabel.ForeColor = Color.Red
                    VersionLabel.Cursor = Cursors.Hand
                    If UserRequestedUpdateCheck Then
                        If MsgBox("An update is available. Do you want to download it now?", MsgBoxStyle.Information + MsgBoxStyle.YesNo, "Update Check") = MsgBoxResult.Yes Then
                            Process.Start("https://github.com/sophos/XG-Management-Helper/raw/master/XGManagementHelperSetupLatest.zip")
                        End If
                    End If
                Else
                    VersionLabel.Text = String.Format("v{0} (Latest version)", Application.ProductVersion)
                    key.SetValue("LastVersionCheckResult", "(Latest version)")
                    If UserRequestedUpdateCheck Then MsgBox("You are running the latest version. No update is available.", MsgBoxStyle.Information, "Update Check")
                End If
            End Using

        Else
            If UserRequestedUpdateCheck Then MsgBox("Something went wrong while checking for an update.", MsgBoxStyle.Exclamation, "Update Check")
        End If
        UserRequestedUpdateCheck = False
    End Sub

    Private Sub UpdateListTimes_Tick(sender As Object, e As EventArgs) Handles UpdateListTimes.Tick
        For Each lvi As ListViewItem In ResultsListView.Items
            Dim host As KeyValuePair(Of String, String) = GetHostByName(lvi.Text)
            UpdateFirewallRow(host, True)
        Next
        If AutoCheckStatusToolStripMenuItem.Checked Then
            If Now.Subtract(LastRefresh).TotalMinutes >= 5 Then
                DoStatusCheckRefresh()
            End If
        End If

    End Sub

#End Region

#Region "Main Methods"

    Private Sub DoAction(ActionName As String, loglevel As XGShellConnection.LogSeverity, ConfirmDialog As Boolean, ParamArray Params() As KeyValuePair(Of String, String))
        Dim SelectedHosts() As KeyValuePair(Of String, String) = GetSelectedHosts()
        If ConfirmDialog Then
            Dim confirm As New ChangeConfirmation(SelectedHosts.Count, loglevel = XGShellConnection.LogSeverity.Debug)
            If confirm.ShowDialog = DialogResult.Cancel Then Exit Sub
            If confirm.DebugLogging Then loglevel = XGShellConnection.LogSeverity.Debug
        End If
        UpdateListTimes.Enabled = False
        ActionToolStripMenuItem.Enabled = False
        Shortcut1Button.Enabled = False
        Shortcut2Button.Enabled = False
        Shortcut3Button.Enabled = False
        Shortcut4Button.Enabled = False
        ToolStripProgressBar.Maximum = SelectedHosts.Count
        ToolStripProgressBar.Minimum = 0
        ToolStripProgressBar.Value = 0
        ToolStripProgressBar.Visible = True
        Dim count As Integer = 0
        RotateShortcuts(ActionName)
        'clear results window
        For Each Host As KeyValuePair(Of String, String) In SelectedHosts
            Using key As SecureRegistryKey = GetHostKey(Host.Key)
                key.SetValue("LastAction", ActionName)
            End Using
            count += 1
            ToolStripProgressBar.Value += 1
            StatusToolStripStatusLabel.Text = String.Format("Action:{2} - launching {0} of {1}", count, SelectedHosts.Count, ActionName)
            UpdateFirewallRow(Host, False, "wait", "Running...")
            Dim bw As New BackgroundWorker
            AddHandler bw.DoWork, AddressOf Worker_DoWork
            AddHandler bw.RunWorkerCompleted, AddressOf Worker_RunWorkerCompleted
            Workers.Add(bw)

            Dim args As New XGShellConnection.ActionRequest(Host, ActionName, loglevel, Params)
            bw.RunWorkerAsync(args)
        Next
    End Sub

    Private Sub DoAction(Action As String)
        Dim LogLevel As XGShellConnection.LogSeverity = GetSet()
        If LogLevel = Nothing Then Exit Sub
        Select Case Action
            Case "EnableCentralManagementAndReporting", "EnableCentralManagement", "EnableCentralReporting"
                DoAction(Action, LogLevel, True, New KeyValuePair(Of String, String)("Centraluser", CentralUser), New KeyValuePair(Of String, String)("Centralpass", CentralPass))
                '
            Case "DeRegisterFromCentral", "EnableVPNCAPCHA", "DisableVPNCAPCHA", "DisablePasswordResetPopup"
                DoAction(Action, LogLevel, True)
                '
            Case "SetAdminPassword"
                Dim np As New NewPassword
                Dim newpass As String
                Dim Generate As Boolean
                If np.ShowDialog = DialogResult.OK Then
                    Generate = np.GeneratePasswords
                    newpass = np.NewPassword
                Else
                    Exit Sub
                End If
                Dim ActionName As String = "SetAdminPassword"
                DoAction(ActionName, LogLevel, True, New KeyValuePair(Of String, String)("Generate", Generate),
                                                     New KeyValuePair(Of String, String)("Newpass", newpass),
                                                     New KeyValuePair(Of String, String)("EnforceComplexity", np.EnforceComplexity))

            Case "ChangeUserPasswords"
                Dim settoken As New SetToken
                If settoken.ShowDialog = DialogResult.OK Then
                    DoAction(Action, LogLevel, True, New KeyValuePair(Of String, String)("token", settoken.Token))
                End If

            Case Else 'StatusCheck, HotfixInstall, CheckVPNCAPCHA, CheckUnchangedUsers, CheckPasswordResetPopup
                DoAction(Action, LogLevel, False)

        End Select
        TopPanel.Enabled = True
    End Sub

#End Region

#Region "Private Methods"

#Region "Host and host list display functions"

    Private Sub AddResultsLog(Host As String, action As String, result As String, message As String)
        LogsLabel.Visible = True
        Splitter1.Visible = True
        LogListView.Visible = True
        Dim lvi As ListViewItem = LogListView.Items.Add(Now.ToString("yyyy-MM-dd h:mm:ss tt"))
        lvi.SubItems.Add(Host)
        lvi.SubItems.Add(action)
        lvi.SubItems.Add(message)
        lvi.ImageKey = result
        IO.File.AppendAllText(IO.Path.Combine(LogLocation, "application.log"), String.Format("{0}  - host=""{1}"" action=""{2}"" result=""{3}"" message=""{4}""", lvi.Text, Host, action, result, message) & vbNewLine)
        LogListView.Sorting = SortOrder.Descending
        LogListView.Sort()
        AutoResizeColumns(LogListView)

    End Sub

    Private Sub InitTableColumns()
        ResultsListView.Columns.Clear()
        ResultsListView.Columns.Add("Firewall")
        ResultsListView.Columns.Add("Creds")
        ResultsListView.Columns.Add("Model")
        ResultsListView.Columns.Add("Version")
        ResultsListView.Columns.Add("System Load")
        ResultsListView.Columns.Add("Last Response")
        ResultsListView.Columns.Add("Last Action")
        ResultsListView.Columns.Add("Result")
    End Sub

    Private Sub RefreshHostsList(Optional clear As Boolean = False)
        ResultsListView.BeginUpdate()

        For Each host As KeyValuePair(Of String, String) In Hosts
            UpdateFirewallRow(host, Not clear)
        Next

        Dim DeletedItems As New List(Of ListViewItem)
        For Each lvi As ListViewItem In ResultsListView.Items
            Dim Host As KeyValuePair(Of String, String) = GetHostByName(lvi.Text)
            If IsNothing(Host) Then DeletedItems.Add(lvi)
        Next

        For Each lvi As ListViewItem In DeletedItems
            ResultsListView.Items.Remove(lvi)
        Next

        ResultsListView.Sorting = SortOrder.Ascending
        Me.ResultsListView.ListViewItemSorter = New ListViewItemComparer(0, ResultsListView.Sorting)
        ResultsListView.Sort()
        AutoResizeColumns(ResultsListView)
        ResultsListView.EndUpdate()
    End Sub

    Private Function UpdateFirewallRow(Host As KeyValuePair(Of String, String), ShowLastResults As Boolean) As ListViewItem
        Return UpdateFirewallRow(Host, ShowLastResults, "", "")
    End Function

    Private Function UpdateFirewallRow(Host As KeyValuePair(Of String, String), ShowLastResults As Boolean, OverrideImageKey As String, OverrideResults As String) As ListViewItem
        Dim lvi As ListViewItem = GetListViewItemForHost(Host)
        If lvi Is Nothing Then lvi = ResultsListView.Items.Add(Host.Key)
        If OverrideImageKey.Length > 0 Then lvi.ImageKey = OverrideImageKey
        '
        lvi.SubItems.Clear()
        Using key As SecureRegistryKey = GetHostKey(Host.Key)
            lvi.Text = Host.Key
            lvi.SubItems.Add(If(Host.Value.Length > 0, "SET", ""))
            lvi.SubItems.Add(key.GetValue("Model", ""))
            lvi.SubItems.Add(key.GetValue("DisplayVersion", ""))
            lvi.SubItems.Add(key.GetValue("Loadavg15min", ""))
            lvi.SubItems.Add(Age(key.GetValue("LastUpdate", "")))
            If ShowLastResults Then
                lvi.SubItems.Add(key.GetValue("LastAction", ""))
                lvi.SubItems.Add(key.GetValue("LastResult", ""))
            ElseIf OverrideResults.Length > 0 Then
                lvi.SubItems.Add(key.GetValue("LastAction", ""))
                lvi.SubItems.Add(OverrideResults)
            End If
        End Using
        Return lvi
        '
    End Function

    Private Sub UpdateFirewallRow(result As XGShellConnection.ActionResult)
        UpdateFirewallRow(result, "", "")
    End Sub

    Private Sub UpdateFirewallRow(result As XGShellConnection.ActionResult, OverrideImageKey As String)
        UpdateFirewallRow(result, OverrideImageKey, "")
    End Sub

    Private Sub UpdateFirewallRow(result As XGShellConnection.ActionResult, OverrideImageKey As String, OverrideResults As String)
        Dim lvi As ListViewItem = UpdateFirewallRow(result.Host, False, "", "")

        If OverrideImageKey.Length > 0 Then lvi.ImageKey = OverrideImageKey
        If OverrideResults.Length = 0 Then
            lvi.SubItems.Add(result.Action)
            lvi.SubItems.Add(result.Summary)
        Else
            lvi.SubItems.Add(result.Action)
            lvi.SubItems.Add(OverrideResults)
        End If
        '
        If result IsNot Nothing Then
            If result.Success Then
                If OverrideImageKey.Length = 0 Then lvi.ImageKey = "green check"
                XGShellConnection.WriteToLog(result.Host.Key, XGShellConnection.LogSeverity.Informational, result.Summary, result.LogLevel)
                '
            Else
                If OverrideImageKey.Length = 0 Then
                    Select Case result.FailReason
                        Case "Timeout"
                            lvi.ImageKey = "timeout"
                        Case "Credentials"
                            lvi.ImageKey = "fail"
                        Case "Connection"
                            lvi.ImageKey = "fail"
                        Case Else
                            lvi.ImageKey = "fail"
                    End Select
                End If
                XGShellConnection.WriteToLog(result.Host.Key, XGShellConnection.LogSeverity.Error, result.Summary, result.LogLevel)
                '
            End If
            If Not IsNothing(result.LogLevel) Then AddResultsLog(result.Host.Key, result.Action, lvi.ImageKey, result.Summary)
            'AutoResizeColumns(ResultsListView)
            '
        End If
        '
    End Sub

    Private Sub AddOrUpdateHostRecordTransaction(newhost As KeyValuePair(Of String, String), Optional FinalizeTransaction As Boolean = False)
        If newhost.Key Is Nothing Then
            Exit Sub
        End If
        If newhost.Key.Trim.Length = 0 Then
            Exit Sub
        End If
        NewHosts.Add(newhost)
        If FinalizeTransaction Then FinalizeAddOrUpdateHostRecordTransaction()
    End Sub

    Private Sub FinalizeAddOrUpdateHostRecordTransaction()
        If NewHosts.Count = 0 Then Exit Sub
        Try
            'Update entry if duplicate host
            For Each ThisHost As KeyValuePair(Of String, String) In Hosts
                If ThisHost.Key IsNot Nothing Then
                    If Not NewHosts.Contains(ThisHost, New KeyComparer) Then
                        NewHosts.Add(ThisHost)
                    Else
                        'skip it
                    End If
                End If
            Next

            Hosts.Clear()
            Hosts.AddRange(NewHosts)
            NewHosts.Clear()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        SaveHosts(IncrementalAutoSave)
    End Sub

    Private Function WritePasswordChangeLog() As Integer
        Dim Logname As String = String.Format("PasswordChanges-{0}", Now.ToString("yyyy-MM-dd_h-mm_tt"))
        Dim loglines As String = ""
        Dim Logfilename As String = FileHelpers.MakeUniqueFilename(Logname & ".enc")
        Logfilename = IO.Path.Combine(LogLocation, Logfilename)
        Logname = IO.Path.GetFileNameWithoutExtension(Logfilename)

        Dim counter As Integer = 0
        Dim Newhosts As New List(Of KeyValuePair(Of String, String))
        For Each host As KeyValuePair(Of String, String) In Hosts
            Using key As SecureRegistryKey = GetHostKey(host.Key)
                Dim SetTo As String = key.GetSecureValue("PasswordSet", "")
                Dim SetAt As String = key.GetValue("PasswordSetAt")
                Dim Tried As String = key.GetSecureValue("PasswordTry", "")
                Dim TriedAt As String = key.GetValue("PasswordTryAt")
                If Tried.Length > 0 Then
                    counter += 1
                    loglines &= String.Format("tried=""{0}"", at=""{1}""", Tried, TriedAt) & vbNewLine
                    EncryptedFileWrite(Logfilename, loglines)
                    key.DeleteValue("PasswordTry")
                    key.DeleteValue("PasswordTryAt")

                    If SetTo.Length > 0 Then
                        loglines &= String.Format("set=""{0}"", at=""{1}"", results=""success""", SetTo, SetAt) & vbNewLine
                        EncryptedFileWrite(Logfilename, loglines)
                        key.DeleteValue("PasswordSet")
                        key.DeleteValue("PasswordSetAt")

                        Dim newhost As KeyValuePair(Of String, String)
                        If SetTo.Equals(ShellCommonPass) Then
                            newhost = New KeyValuePair(Of String, String)(host.Key, "")
                        Else
                            newhost = New KeyValuePair(Of String, String)(host.Key, SetTo)
                        End If
                        AddOrUpdateHostRecordTransaction(newhost)
                    Else
                        loglines &= String.Format("set=""{0}"", at=""unknown"", results=""UNKNOWN""", SetTo, SetAt) & vbNewLine
                        EncryptedFileWrite(Logfilename, loglines)
                    End If
                End If
            End Using
        Next
        FinalizeAddOrUpdateHostRecordTransaction()

        'End Using

        If counter > 0 Then
            Dim lv As New LogViewer(LogLocation)
            lv.ShowLog(Logname)
            lv.ShowDialog()
        End If
        Return counter

    End Function

    Private Sub RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs)
        Dim result As XGShellConnection.ActionResult = e.Result
        workers.Remove(sender)
        Using key As SecureRegistryKey = GetHostKey(result.Host.Key)
            key.SetValue("LastResult", result.Summary)
            '
            If workers.Count = 0 Then
                'finished. update status and clean up
                StatusToolStripStatusLabel.Text = "Finished"
                ToolStripProgressBar.Visible = False
                ActionToolStripMenuItem.Enabled = True
                Shortcut1Button.Enabled = True
                Shortcut2Button.Enabled = True
                Shortcut3Button.Enabled = True
                Shortcut4Button.Enabled = True
                UpdateListTimes.Enabled = True
            Else
                StatusToolStripStatusLabel.Text = String.Format("Action:{0} - workers remaining: {1}", result.Action, workers.Count)
            End If
            '
            Select Case result.Action
                Case "SetAdminPassword"
                    If result.Success Then
                        Dim newpass As String = result.Request.GetValue("NewPass")
                        Dim newhost As New KeyValuePair(Of String, String)(result.Host.Key, If(newpass.Equals(ShellCommonPass), "", newpass))
                        result.Host = newhost
                        AddOrUpdateHostRecordTransaction(newhost, True)
                    End If
                    '
                    UpdateFirewallRow(result)
                    If workers.Count = 0 Then WritePasswordChangeLog()
                    '
                Case "EnableCentralManagementAndReporting", "EnableCentralManagement", "EnableCentralReporting"
                    If result.Success Then
                        If result.Summary.Contains("Central Service(s) enabled") Then
                            UpdateFirewallRow(result, "gray check")
                        Else
                            UpdateFirewallRow(result, "pause")
                        End If
                    Else
                        UpdateFirewallRow(result)
                    End If
            End Select
            '
        End Using
        UpdateFirewallRow(result)
        If Workers.Count = 0 Then AutoResizeColumns(ResultsListView)

    End Sub

#End Region

    Public Shared Sub AutoResizeColumns(ByVal this As ListView)
        this.SuspendLayout()
        this.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)
        Dim CurrentCol As ListView.ColumnHeaderCollection = this.Columns

        For i As Integer = 0 To CurrentCol.Count - 1
            Dim colWidth As Integer = TextRenderer.MeasureText(CurrentCol(i).Text, this.Font).Width + 10
            If colWidth > CurrentCol(i).Width Then
                CurrentCol(i).Width = colWidth
            End If
        Next
        this.ResumeLayout()
    End Sub

    Private Function Age(TimeString As String) As String
        Try
            Dim dt As DateTime = Convert.ToDateTime(TimeString)
            Dim diff As TimeSpan = Now.Subtract(dt)
            'Debug.Print("checking '{0}' as '{1}' to now. total seconds: {2}", TimeString, dt.ToString, diff.TotalSeconds)

            Select Case diff.TotalSeconds
                Case 0 To 29 'seconds
                    Return "Just now"
                Case 30 To 59
                    Return "Moments ago"
                Case 60 To 119
                    Return "A minute ago"
                Case 120 To 3599 ' minutes
                    Return CInt(diff.TotalMinutes) & " minutes ago"
                Case 3600 To 86399 'hours
                    Return CInt(diff.TotalHours) & " hours ago"
                Case 86400 To 172799 ' one day
                    Return "Yesterday"
                Case 172800 To 604800
                    Return Now.AddDays(-CInt(diff.TotalDays)).ToString("dddd")

                Case 604800 To 129599
                    Return "A week ago"
                Case Else
                    Return CInt(diff.TotalDays / 7) & " weeks ago"
                    'weeks
            End Select
        Catch ex As Exception
            Return "Never"
        End Try
    End Function

    Private Function GetSelectedHosts() As KeyValuePair(Of String, String)()
        Dim Selection As IEnumerable
        Dim SelectedHosts() As KeyValuePair(Of String, String) = {}
        If ResultsListView.CheckedItems.Count > 0 Then
            Selection = ResultsListView.CheckedItems
        Else
            Selection = ResultsListView.Items
        End If

        For Each itm As ListViewItem In Selection
            Dim Host As KeyValuePair(Of String, String) = GetHostByName(itm.Text)
            If Not IsNothing(Host) Then
                ReDim Preserve SelectedHosts(SelectedHosts.Count)
                SelectedHosts(SelectedHosts.GetUpperBound(0)) = Host
            End If
        Next

        Return SelectedHosts

    End Function

    Private Sub DeleteCheckedHosts()
        Dim selected As KeyValuePair(Of String, String)() = GetSelectedHosts()
        Dim cleanedlist As New List(Of KeyValuePair(Of String, String))
        Dim deletelist As New List(Of KeyValuePair(Of String, String))
        '
        For Each host As KeyValuePair(Of String, String) In Hosts
            If Not selected.Contains(host) Then
                cleanedlist.Add(host)
            Else
                deletelist.Add(host)
            End If
        Next
        '
        Using HostsKey As SecureRegistryKey = GetHostsKey()
            For Each host As KeyValuePair(Of String, String) In deletelist
                HostsKey.DeleteValue(host.Key)
                HostsKey.DeleteSubKey("Host-" & host.Key)
                ResultsListView.Items.Remove(GetListViewItemForHost(host))
            Next
        End Using
        '
        Hosts.Clear()
        Hosts.AddRange(cleanedlist)

        'SaveHosts(IncrementalAutoSave)
        'RefreshHostsList()
        '
    End Sub

    Private Sub ImportFirewalls()
        Dim ofd As New OpenFileDialog With {.Filter = "Text files (*.txt)|*.txt"}
        If ofd.ShowDialog() = DialogResult.OK Then
            Try
                Dim ImportedList As String = IO.File.ReadAllText(ofd.FileName)
                Dim ImportedListArray As String() = Split(ImportedList.Replace(vbCrLf, vbLf), vbLf)
                For Each ListLine As String In ImportedListArray
                    Dim ImportedHostLine As String = ListLine.Trim
                    If Trim(ImportedHostLine).Length > 0 Then
                        If ImportedHostLine.Contains(" ") Then
                            Dim HostString As String = ImportedHostLine.Substring(0, ImportedHostLine.IndexOf(" ")).Trim
                            Dim Password As String = ImportedHostLine.Substring(ImportedHostLine.IndexOf(" ") + 1).Trim
                            Dim Host As New KeyValuePair(Of String, String)(HostString, Password)
                            AddOrUpdateHostRecordTransaction(Host)

                        Else
                            Dim Host As New KeyValuePair(Of String, String)(ImportedHostLine, "")
                            AddOrUpdateHostRecordTransaction(Host)

                        End If
                    End If
                Next

                'combine unique entries
                FinalizeAddOrUpdateHostRecordTransaction()
                SaveHosts(IncrementalAutoSave)
                RefreshHostsList(False)
            Catch ex As Exception
                Debug.Print("E04683" & ex.Message)
            End Try

        End If
    End Sub

    Private Sub SetRegistryPermissions()
        Try
            Dim user As String = Environment.UserDomainName + "\\" + Environment.UserName
            Dim rs As New RegistrySecurity
            rs.SetAccessRuleProtection(True, False)
            rs.AddAccessRule(New RegistryAccessRule(user, RegistryRights.FullControl, InheritanceFlags.None, PropagationFlags.None, AccessControlType.Allow))
            Using key As SecureRegistryKey = GetCurrentUserKey.CreateSubKey("Software\XGMigrationHelper", RegistryKeyPermissionCheck.Default, rs)
                key.SetAccessControl(rs)
            End Using
            Using key As SecureRegistryKey = GetCurrentUserKey.CreateSubKey("Software\XGMigrationHelper\Hosts")
                key.SetAccessControl(rs)
            End Using

        Catch ex As Exception

        End Try

    End Sub

    Private Sub BackupKey()
        Dim sfd As New SaveFileDialog With {.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments,
                                            .DefaultExt = ".recovery",
                                            .OverwritePrompt = True,
                                            .AddExtension = True,
                                            .FileName = "XGManagementHelper",
                                            .Filter = "XG Management Helper Recovery File|*.recovery"}
        If sfd.ShowDialog = DialogResult.OK Then
            Dim ekey As Byte() = Convert.FromBase64String(EncryptionHelper.DataKey)
            Dim iv As Byte() = Convert.FromBase64String(EncryptionHelper.DataIV)
            Dim output As Byte()
            ReDim output(ekey.Length + iv.Length - 1)
            ekey.CopyTo(output, 0)
            iv.CopyTo(output, ekey.Length)
            IO.File.WriteAllBytes(sfd.FileName, output)
        End If
    End Sub

    Private Sub CheckForUpdate(Optional Force As Boolean = False)
        Using key As SecureRegistryKey = GetApplicationRootKey()
            Dim LastVersion As String = key.GetValue("LastRunVersion", "")
            Dim LastVersionCheck As DateTime

            If LastVersion <> Application.ProductVersion Then
                LastVersionCheck = DateTime.MinValue
            Else
                LastVersionCheck = Convert.ToDateTime(key.GetValue("LastVersionCheck", DateTime.MinValue.ToString))
            End If
            If Force OrElse Now.Subtract(LastVersionCheck).TotalHours > 12 Then
                WGET.AsyncURLDownloadAsString(VersionCheckURL)
                VersionLabel.Text = "v" & Application.ProductVersion
            Else
                VersionLabel.Text = "v" & Application.ProductVersion & key.GetValue("LastVersionCheckResult", "Unknown")
            End If
            key.SetValue("LastRunVersion", Application.ProductVersion)
        End Using
    End Sub

    Private Sub LoadSavedValues()
        Using Insecure As SecureRegistryKey = GetApplicationRootKey() 'can't encrypt/decrypt yet. datakey not provided
            TrustInitialSSHFingerprintToolStripMenuItem.Checked = Insecure.GetValue("TrustInitialSSHFingerprint", True)
            If Insecure.GetValue("DoubleClick", "Edit").Equals("Edit") Then
                EditHostToolStripMenuItem.Checked = True
            Else
                OpenWebAdminAndCopyPasswordToClipboardToolStripMenuItem.Checked = True
            End If

            DataIV = Insecure.GetValue("Init3")
            If DataIV Is Nothing OrElse DataIV.Length < 16 Then
                DataIV = Convert.ToBase64String(GetRandomBytes(16))
                Insecure.SetValue("Init3", DataIV)
            End If
            Dim login As New Login
        End Using
        '
        If Login.ShowDialog() = DialogResult.OK Then
            EncryptionHelper.DataKey = Login.Key
            EncryptionHelper.DataIV = DataIV
            Using key As SecureRegistryKey = GetApplicationRootKey()
                If Login.NewPassword Then
                    If MsgBox("Do you want to backup your key, in case you ever lose the password?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Backup") = MsgBoxResult.Yes Then
                        BackupKey()
                    End If
                End If
                Login.Dispose()
                '
                CheckForUpdate()
                Try
                    ShellCommonPass = key.GetSecureValue("CommonShell", "")
                    CentralUser = key.GetValue("LastEmail", "")
                    CentralPass = key.GetSecureValue("LastEmailPass", "")
                    If key.GetValue("DoubleClick", "Edit") = "Edit" Then
                        EditHostToolStripMenuItem.Checked = True
                        OpenWebAdminAndCopyPasswordToClipboardToolStripMenuItem.Checked = False
                    Else
                        EditHostToolStripMenuItem.Checked = False
                        OpenWebAdminAndCopyPasswordToClipboardToolStripMenuItem.Checked = True
                    End If
                    '
                Catch ex As Exception
                    MsgBox(ex.ToString)
                End Try
                '
                ShellPassTextBox.Text = ShellCommonPass
                AutoCheckStatusToolStripMenuItem.Checked = key.GetValue("AutoCheck", True)
                Using hkey As SecureRegistryKey = GetHostsKey()
                    Dim reg_hosts As String() = hkey.GetValueNames()
                    Hosts.Clear()
                    For Each hostname As String In reg_hosts
                        If hostname.Length > 0 Then
                            Dim pass As String = hkey.GetSecureValue(hostname)
                            Dim host As New KeyValuePair(Of String, String)(hostname, pass)
                            If Not Hosts.Contains(host, New KeyComparer) Then Hosts.Add(host)
                        End If
                    Next
                End Using
            End Using
        Else
            Application.Exit()
        End If
        LoadingBool = False
        RefreshHostsList()
        EnableDisable()
        '
        If AutoCheckStatusToolStripMenuItem.Checked Then
            DoStatusCheckRefresh()
        End If
        '
    End Sub

    Private Sub SaveSettings(Optional Force As Boolean = False)
        If Force Then
            Dim key As SecureRegistryKey = GetApplicationRootKey()
            Try
                If CentralUser.Length Then
                    key.SetValue("LastEmail", CentralUser)
                Else
                    key.DeleteValue("LastEmail")
                End If
            Catch ex As Exception
                Debug.Print("LastEmail len {0} error {1}", CentralUser.Length, ex.Message)
            End Try

            Try
                If CentralPass.Length Then
                    key.SetSecureValue("LastEmailPass", CentralPass)
                Else
                    key.DeleteValue("LastEmailPass")
                End If
            Catch ex As Exception
                Debug.Print("CentralPass len {0} error {1}", CentralPass.Length, ex.Message)
            End Try

            Try
                If ShellCommonPass.Length Then
                    key.SetSecureValue("CommonShell", ShellCommonPass)
                Else
                    key.DeleteValue("CommonShell")
                End If
            Catch ex As Exception
                Debug.Print("CommonShell len {0} error {1}", ShellCommonPass.Length, ex.Message)
            End Try
        Else
            FormIsDirty = True
        End If
    End Sub

    Private Sub SaveHosts(Optional Force As Boolean = False)
        If Force Then
            Using key As SecureRegistryKey = GetHostsKey()
                Try
                    For Each hostname As String In key.GetSubKeyNames
                        key.DeleteValue(hostname)
                    Next
                Catch ex As Exception
                    Debug.Print("E082432:" & ex.Message)
                End Try
                Try
                    For Each host As KeyValuePair(Of String, String) In Hosts
                        key.SetSecureValue(host.Key, host.Value)
                    Next
                Catch ex As Exception
                    Debug.Print("E9348" & ex.Message)
                End Try
            End Using
        Else
            FormIsDirty = True
        End If
    End Sub

    Private Sub EnableDisable()
        Dim En = True

        If Hosts.Count = 0 Then En = False
        ActionToolStripMenuItem.Enabled = En

        If ResultsListView.CheckedItems.Count = 0 Then
            DeleteSelectedToolStripMenuItem.Enabled = False
            ActionToolStripMenuItem.Text = "Action (All Firewalls)"
        Else
            DeleteSelectedToolStripMenuItem.Enabled = True
            If ResultsListView.CheckedItems.Count = 1 Then
                ActionToolStripMenuItem.Text = String.Format("Action ({0} Firewall)", ResultsListView.CheckedItems.Count)
            Else
                ActionToolStripMenuItem.Text = String.Format("Action ({0} Firewalls)", ResultsListView.CheckedItems.Count)
            End If
        End If

    End Sub

    Private Sub AddOrUpdateostInteractively(Optional defaulthost As String = "", Optional defaultpass As String = "")
        Dim sshhost As String
        Dim sshpass As String
        Dim af As AddFirewalls
        Dim another As Boolean = False
        If defaultpass = Nothing Then defaultpass = ""
        Do
            af = New AddFirewalls(ShellCommonPass)
            If defaulthost.Length > 0 Then af.SSHHost = defaulthost
            If Not defaultpass.Equals("SET") Then af.SSHPass = defaultpass
            af.AddAnother = another
            Select Case af.ShowDialog
                Case DialogResult.OK
                    sshhost = af.SSHHost
                    sshpass = af.SSHPass
                    Dim host As New KeyValuePair(Of String, String)(sshhost, sshpass)
                    AddOrUpdateHostRecordTransaction(host)
                    '
                Case DialogResult.Cancel
                    Exit Do
                    '
                Case Else
                    Exit Do
                    '
            End Select
            '
            FinalizeAddOrUpdateHostRecordTransaction()
            another = af.AddAnother
            '
        Loop While another
        '
        EnableDisable()
        SaveHosts()
        RefreshHostsList(False)
    End Sub

    Private Function GetHostByName(Host As String) As KeyValuePair(Of String, String)
        For Each thishost As KeyValuePair(Of String, String) In Hosts
            If thishost.Key.Equals(Host) Then Return thishost
        Next
        Return Nothing
    End Function

    Private Sub SetCentralCreds()

        Dim cu As String = ""
        Dim cp As String = ""
        Dim l As New CentralLogin With {.CentralUser = CentralUser, .CentralPass = CentralPass}

        While cu.Length = 0 Or cp.Length = 0
            Select Case l.ShowDialog
                Case DialogResult.OK
                    cu = l.CentralUser
                    cp = l.CentralPass

                Case DialogResult.Abort
                    CentralUser = ""
                    CentralPass = ""
                    SaveSettings(IncrementalAutoSave)
                    Exit Sub

                Case DialogResult.Cancel

                    Exit Sub
            End Select
        End While
        CentralUser = cu
        CentralPass = cp
        SaveSettings(IncrementalAutoSave)
    End Sub

    Private Sub EditSelectedHost()
        If ResultsListView.SelectedItems.Count <> 1 Then Exit Sub
        Dim hostname As String = ResultsListView.SelectedItems(0).Text
        Dim host As KeyValuePair(Of String, String) = GetHostByName(hostname)
        AddOrUpdateostInteractively(host.Key, host.Value)
    End Sub

    Private Sub OpenSelectedHostWebadmin()
        If ResultsListView.SelectedItems.Count <> 1 Then Exit Sub
        Dim hostname As String = ResultsListView.SelectedItems(0).Text
        Dim host As KeyValuePair(Of String, String) = GetHostByName(hostname)
        Dim pass As String = ShellCommonPass
        If Not host.Value = "" Then pass = host.Value
        Clipboard.SetText(pass)
        Dim port As String
        Using key As SecureRegistryKey = GetHostKey(host.Key)
            port = key.GetValue("Webadmin", "4444")
        End Using
        Process.Start(String.Format("https://{0}:{1}/", host.Key, port))
    End Sub

    Private Sub DeleteSelectedHost()
        If ResultsListView.SelectedItems.Count <> 1 Then Exit Sub
        Dim hostname As String = ResultsListView.SelectedItems(0).Text
        Dim host As KeyValuePair(Of String, String) = GetHostByName(hostname)
        '
        If host.Key.Length > 0 Then
            If MsgBox("Are you sure you want to delete this host and its saved password?", MsgBoxStyle.Question + MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                Hosts.Remove(host)
                SaveHosts(IncrementalAutoSave)
                RefreshHostsList(False)
            End If
        End If

    End Sub

    Private Sub RotateShortcuts(LastAction As String)
        If Shortcuts.Contains(LastAction) Then Shortcuts.Remove(LastAction)
        Shortcuts.Add(LastAction)

        While Shortcuts.Count > 4
            Shortcuts.RemoveAt(0)
        End While
        ShowShortcuts()
    End Sub

    Private Sub ShowShortcuts()
        Using key As SecureRegistryKey = GetApplicationRootKey()
            Me.SuspendLayout()
            Shortcut1Button.Visible = False
            Shortcut2Button.Visible = False
            Shortcut3Button.Visible = False
            Shortcut4Button.Visible = False
            Select Case Shortcuts.Count
                Case 0
                Case 1
                    Shortcut1Button.Text = Shortcuts(0)
                    Shortcut2Button.Text = ""
                    Shortcut3Button.Text = ""
                    Shortcut4Button.Text = ""
                    Shortcut1Button.Visible = True
                Case 2
                    Shortcut1Button.Text = Shortcuts(0)
                    Shortcut2Button.Text = Shortcuts(1)
                    Shortcut3Button.Text = ""
                    Shortcut4Button.Text = ""
                    Shortcut1Button.Visible = True
                    Shortcut2Button.Visible = True
                Case 3
                    Shortcut1Button.Text = Shortcuts(0)
                    Shortcut2Button.Text = Shortcuts(1)
                    Shortcut3Button.Text = Shortcuts(2)
                    Shortcut4Button.Text = ""
                    Shortcut1Button.Visible = True
                    Shortcut2Button.Visible = True
                    Shortcut3Button.Visible = True
                Case 4
                    Shortcut1Button.Text = Shortcuts(0)
                    Shortcut2Button.Text = Shortcuts(1)
                    Shortcut3Button.Text = Shortcuts(2)
                    Shortcut4Button.Text = Shortcuts(3)
                    Shortcut1Button.Visible = True
                    Shortcut2Button.Visible = True
                    Shortcut3Button.Visible = True
                    Shortcut4Button.Visible = True
                Case Else
            End Select
            Shortcut1Button.Tag = Shortcut1Button.Text
            Shortcut2Button.Tag = Shortcut2Button.Text
            Shortcut3Button.Tag = Shortcut3Button.Text
            Shortcut4Button.Tag = Shortcut4Button.Text
            Me.ResumeLayout()

            key.SetValue("Shortcut1", Shortcut1Button.Text)
            key.SetValue("Shortcut2", Shortcut2Button.Text)
            key.SetValue("Shortcut3", Shortcut3Button.Text)
            key.SetValue("Shortcut4", Shortcut4Button.Text)

        End Using


    End Sub

    Private Sub DoStatusCheckRefresh()
        Dim sh As KeyValuePair(Of String, String)() = GetSelectedHosts()

        For Each lvi As ListViewItem In ResultsListView.Items
            If lvi.SubItems(2).Text.Length = 0 Then
                lvi.Checked = False
            Else
                lvi.Checked = True
            End If
        Next
        DoAction("StatusCheck")
        LastRefresh = Now
        For Each lvi As ListViewItem In ResultsListView.Items
            Dim host As KeyValuePair(Of String, String) = GetHostByName(lvi.Text)
            If sh.Contains(host) Then
                lvi.Checked = True
            Else
                lvi.Checked = False
            End If
        Next
        For Each host As KeyValuePair(Of String, String) In sh
            Dim lvi As ListViewItem = GetListViewItemForHost(host)
            lvi.Checked = True
        Next

    End Sub

#Region "File Menu"

    Private Sub ImportFirewallsListToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ImportFirewallsListToolStripMenuItem.Click
        ImportFirewalls()
    End Sub

    Private Sub BackupApplicationKeyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BackupApplicationKeyToolStripMenuItem.Click
        BackupKey()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Application.Exit()
    End Sub

#End Region

#Region "Edit Menu"

    Private Sub AddFirewallsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddFirewallsToolStripMenuItem.Click
        AddOrUpdateostInteractively()
    End Sub

    Private Sub DeleteSelectedToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteSelectedToolStripMenuItem.Click
        If MsgBox("Are you sure you want to delete these hosts?", MsgBoxStyle.YesNo + MsgBoxStyle.Exclamation, "Confirmation Required") = MsgBoxResult.Yes Then
            DeleteCheckedHosts()
        End If
    End Sub

    Private Sub ChangeCentralCredentialsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ChangeCentralCredentialsToolStripMenuItem.Click
        SetCentralCreds()
    End Sub

    Private Sub TrustInitialSSHFingerprintToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TrustInitialSSHFingerprintToolStripMenuItem.Click
        TrustInitialSSHFingerprintToolStripMenuItem.Checked = Not TrustInitialSSHFingerprintToolStripMenuItem.Checked
        Using key As SecureRegistryKey = GetApplicationRootKey()
            key.SetValue("TrustInitialSSHFingerprint", TrustInitialSSHFingerprintToolStripMenuItem.Checked)
        End Using

    End Sub

#End Region

#Region "View Menu"

    Private Sub PasswordChangeLogsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PasswordChangeLogsToolStripMenuItem.Click
        Dim lv As New LogViewer(LogLocation)
        lv.ShowDialog()
    End Sub

#End Region

#Region "Action Menu"

    Private Sub ActionMenuItem_Click(sender As Object, e As EventArgs) Handles PingCheckToolStripMenuItem.Click,
                                                                               InstallAnyAvailableHotfixesevenIfToolStripMenuItem.Click,
                                                                               RegisterEnableAllCentralServicesToolStripMenuItem.Click,
                                                                               EnableCentralManagementOnlyToolStripMenuItem.Click,
                                                                               EnableCentralReportingOnlyToolStripMenuItem.Click,
                                                                               BulkChangeadminPasswordToolStripMenuItem.Click,
                                                                               CheckStatusToolStripMenuItem1.Click,
                                                                               DisablePopUpToolStripMenuItem.Click,
                                                                               CkeckStatusToolStripMenuItem.Click,
                                                                               EnableCAPCHAToolStripMenuItem1.Click,
                                                                               DisableCAPCHAOnVPNZoneToolStripMenuItem.Click,
                                                                               CheckPasswordsOlderThanApr252020ToolStripMenuItem.Click,
                                                                               AddTokenToOlderUserPasswordsToolStripMenuItem.Click,
                                                                               DeregisterFromSophosCentralToolStripMenuItem1.Click,
                                                                               Shortcut1Button.Click, Shortcut2Button.Click, Shortcut3Button.Click, Shortcut4Button.Click
        DoAction(sender.tag)
    End Sub


#End Region

#Region "Help Menu"

    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        Dim a As New AboutBox1
        a.ShowDialog()
    End Sub

    Private Sub ReportAnIssueToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ReportAnIssueToolStripMenuItem.Click
        Process.Start("https://github.com/sophos/XG-Management-Helper/issues")
    End Sub

    Private Sub GetLatestVersionToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GetLatestVersionToolStripMenuItem.Click
        UserRequestedUpdateCheck = True
        CheckForUpdate(True)
    End Sub


#End Region

#Region "Context Menu"

    Private Sub EditFirewallToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EditFirewallToolStripMenuItem.Click
        EditSelectedHost()
    End Sub

    Private Sub OpenWebAdminCopyPasswordToClipboardToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenWebAdminCopyPasswordToClipboardToolStripMenuItem.Click
        OpenSelectedHostWebadmin()
    End Sub

    Private Sub OpenWebAdminAndCopyPasswordToClipboardToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenWebAdminAndCopyPasswordToClipboardToolStripMenuItem.Click, EditHostToolStripMenuItem.Click
        If LoadingBool Then Exit Sub
        sender.checked = True
        Using key As SecureRegistryKey = GetApplicationRootKey()
            If sender.name.Equals(OpenWebAdminAndCopyPasswordToClipboardToolStripMenuItem.Name) Then
                EditHostToolStripMenuItem.Checked = False
                key.SetValue("DoubleClick", "Edit")
            Else
                OpenWebAdminAndCopyPasswordToClipboardToolStripMenuItem.Checked = False
                key.SetValue("DoubleClick", "Open")
            End If
        End Using


    End Sub

    Private Sub DeleteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteToolStripMenuItem.Click
        DeleteSelectedHost()
    End Sub

    Private Sub ApplicationLogsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ApplicationLogsToolStripMenuItem.Click
        FileHelpers.OpenFileWithDefaultApplication(LogLocation)
    End Sub



    Private Sub AutoCheckStatusToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AutoCheckStatusToolStripMenuItem.Click
        If LoadingBool Then Exit Sub
        Using key As SecureRegistryKey = GetApplicationRootKey()
            key.SetValue("AutoCheck", AutoCheckStatusToolStripMenuItem.Checked)
        End Using
        AutoPingTimer.Enabled = AutoCheckStatusToolStripMenuItem.Checked
        '
    End Sub

    Private Sub TopMenuStrip_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles TopMenuStrip.ItemClicked

    End Sub

    Private Sub WGET_AsyncDownloadFail(sender As Object, e As HttpGet.AsyncDownloadEventArgs) Handles WGET.AsyncDownloadFail
        If UserRequestedUpdateCheck Then
            MsgBox("Unable to check for new version." & vbNewLine & "Error Message: " & e.Exception.Message, MsgBoxStyle.Exclamation, "Update Check Error")
        End If
    End Sub

    Private Sub VersionLabel_Click(sender As Object, e As EventArgs) Handles VersionLabel.Click
        If VersionLabel.ForeColor = Color.Red Then
            UserRequestedUpdateCheck = True
            CheckForUpdate()
        End If
    End Sub


#End Region

#End Region

End Class