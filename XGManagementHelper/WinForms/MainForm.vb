
' Copyright 2020  Sophos Ltd.  All rights reserved.
' Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
' You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
' Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, 
' WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing 
' permissions and limitations under the License.

Imports System.Management
Imports System.Security.Cryptography
Imports System.IO.Compression
Imports System.ComponentModel
Imports XGManagementHelper.Randomness
Imports System.Security.AccessControl
Imports System.Security.Principal
Imports Microsoft.Win32

Public Class MainForm

    Private DataKey As String
    Private DataIV As String
    Private AESWrapper As AES256Wrapper
    ReadOnly NewHosts As New List(Of KeyValuePair(Of String, String))
    ReadOnly IncrementalAutoSave As Boolean = True
    Private FormIsDirty As Boolean
    ReadOnly Hosts As New List(Of KeyValuePair(Of String, String))
    Private CentralUser As String = ""
    Private CentralPass As String = ""
    Private ShellCommonPass As String = ""
    Private LoadingBool As Boolean = False

#Region "Form Events"

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadingBool = True
        SetRegistryPermissions()
        LoadSavedValues()
        UpdateHostsList()
        LoadingBool = False
        Me.MaximumSize = Screen.FromRectangle(Me.Bounds).WorkingArea.Size
        '
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
        If ResultsListView.SelectedItems.Count = 1 Then
            Dim h As String = ResultsListView.SelectedItems(0).Text
            For Each host As KeyValuePair(Of String, String) In Hosts
                If host.Key.Equals(h) Then
                    AddHost(host.Key, host.Value)
                    Exit Sub
                End If
            Next

        End If

    End Sub

#End Region

#Region "Main Methods"
    Private Sub AddResultsLog(Host As String, result As String, message As String)
        Splitter1.Visible = True
        LogsLabel.Visible = True
        LogListView.Visible = True
        Dim lvi As ListViewItem = LogListView.Items.Add(Now.ToString("yyyy-MM-dd h:mm:ss tt"))
        lvi.SubItems.Add(Host)
        lvi.SubItems.Add(message)
        lvi.ImageKey = result
        LogListView.Sorting = SortOrder.Descending
        LogListView.Sort()
        LogListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)
    End Sub

    Private Sub DoHotfixInstall(loglevel As XGShellConnection.LogSeverity)

        Dim SelectedHosts() As KeyValuePair(Of String, String) = GetSelectedHosts()
        Dim confirm As New ChangeConfirmation(SelectedHosts.Count, loglevel = XGShellConnection.LogSeverity.Debug)
        If confirm.ShowDialog = DialogResult.Cancel Then Exit Sub
        If confirm.DebugLogging Then loglevel = XGShellConnection.LogSeverity.Debug

        ActionToolStripMenuItem.Enabled = False
        ToolStripProgressBar.Maximum = SelectedHosts.Count
        ToolStripProgressBar.Minimum = 0
        ToolStripProgressBar.Value = 0
        ToolStripProgressBar.Visible = True
        Dim count As Integer = 0

        'clear results window
        ListHosts()
        For Each Host As KeyValuePair(Of String, String) In SelectedHosts
            count += 1
            ToolStripProgressBar.Value += 1
            StatusToolStripStatusLabel.Text = String.Format("checking {0} of {1} - {2}", count, SelectedHosts.Count, Host.Key)
            Dim lvi As ListViewItem = GetListViewItemForHost(Host)
            lvi.ImageKey = "wait"

            Dim SSH As New XGShellConnection(TrustInitialSSHFingerprintToolStripMenuItem.Checked, DataKey, DataIV)
            Dim pass As String = Host.Value
            If pass = "" Then pass = ShellCommonPass 'ShellPassTextBox.Text

            Dim result As XGShellConnection.ExpectResult = SSH.InstallHotfixes(Host.Key, "admin", pass, loglevel)

            lvi.SubItems.Add(result.Summary)
            lvi.SubItems.Add(Now.ToString("yyyy-MM-dd h:mm:ss tt"))
            If result.Success Then
                If result.Reply.Contains("remains") Then
                    lvi.ImageKey = "gray check"
                Else
                    lvi.ImageKey = "green check"
                End If
                XGShellConnection.WriteToLog(XGShellConnection.LogSeverity.Informational, result.Summary, loglevel)
            Else
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
                XGShellConnection.WriteToLog(XGShellConnection.LogSeverity.Error, result.Summary, loglevel)
            End If
            AddResultsLog(Host.Key, lvi.ImageKey, result.Summary)

            ResultsListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent) 'And ColumnHeaderAutoResizeStyle.HeaderSize)
        Next
        'finished. update status and clean up
        StatusToolStripStatusLabel.Text = "Finished"
        ToolStripProgressBar.Visible = False
        ActionToolStripMenuItem.Enabled = True
    End Sub

    Private Sub DoVersionCheck(loglevel As XGShellConnection.LogSeverity)

        Dim SelectedHosts() As KeyValuePair(Of String, String) = GetSelectedHosts()
        ActionToolStripMenuItem.Enabled = False
        ToolStripProgressBar.Maximum = SelectedHosts.Count
        ToolStripProgressBar.Minimum = 0
        ToolStripProgressBar.Value = 0
        ToolStripProgressBar.Visible = True
        Dim count As Integer = 0
        'clear results window
        ListHosts()
        For Each Host As KeyValuePair(Of String, String) In SelectedHosts
            'prep everything for this host. update progress bar, status, find the right entry in the results list to update,...
            count += 1
            ToolStripProgressBar.Value += 1
            StatusToolStripStatusLabel.Text = String.Format("checking {0} of {1} - {2}", count, SelectedHosts.Count, Host.Key)
            Dim lvi As ListViewItem = GetListViewItemForHost(Host)
            lvi.ImageKey = "wait"

            Dim SSH As New XGShellConnection(TrustInitialSSHFingerprintToolStripMenuItem.Checked, DataKey, DataIV)
            Dim pass As String = Host.Value
            If pass = "" Then pass = ShellCommonPass 'ShellPassTextBox.Text

            Dim result As XGShellConnection.ExpectResult = SSH.CheckCurrentFirmwareVersion(Host.Key, "admin", pass, loglevel)
            lvi.SubItems.Add(result.Summary)
            lvi.SubItems.Add(Now.ToString("yyyy-MM-dd h:mm:ss tt"))

            If result.Success Then
                lvi.ImageKey = "gray check"
                XGShellConnection.WriteToLog(XGShellConnection.LogSeverity.Informational, result.Summary, loglevel)
            Else
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
                XGShellConnection.WriteToLog(XGShellConnection.LogSeverity.Error, result.Summary, loglevel)
            End If
            AddResultsLog(Host.Key, lvi.ImageKey, result.Summary)

            ResultsListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent) 'And ColumnHeaderAutoResizeStyle.HeaderSize)
        Next
        'finished. update status and clean up
        StatusToolStripStatusLabel.Text = "Finished"
        ToolStripProgressBar.Visible = False
        ActionToolStripMenuItem.Enabled = True
    End Sub

    Private Sub DoMigration(loglevel As XGShellConnection.LogSeverity, CM As Boolean, CMBackup As Boolean, CR As Boolean)
        If CentralUser.Length = 0 Or CentralPass.Length = 0 Then SetCentralCreds()
        If CentralUser.Length = 0 Or CentralPass.Length = 0 Then
            MsgBox("Cannot proceed without Sophos Central credentals.", MsgBoxStyle.Exclamation, "Credentials needed")
            Exit Sub
        End If

        Dim Selection As IEnumerable = ResultsListView.Items
        Dim SelectedHosts() As KeyValuePair(Of String, String) = GetSelectedHosts()

        Dim confirm As New ChangeConfirmation(SelectedHosts.Count, loglevel = XGShellConnection.LogSeverity.Debug)
        If confirm.ShowDialog = DialogResult.Cancel Then Exit Sub
        If confirm.DebugLogging Then loglevel = XGShellConnection.LogSeverity.Debug
        If ResultsListView.CheckedItems.Count > 0 Then Selection = ResultsListView.CheckedItems

        ActionToolStripMenuItem.Enabled = False
        ToolStripProgressBar.Maximum = SelectedHosts.Count
        ToolStripProgressBar.Minimum = 0
        ToolStripProgressBar.Value = 0
        ToolStripProgressBar.Visible = True
        Dim count As Integer = 0
        ListHosts()

        For Each Host As KeyValuePair(Of String, String) In SelectedHosts

            Dim SSH As New XGShellConnection(TrustInitialSSHFingerprintToolStripMenuItem.Checked, DataKey, DataIV)
            'prep everything for this host. update progress bar, status, find the right entry in the results list to update,...
            count += 1
            ToolStripProgressBar.Value += 1
            StatusToolStripStatusLabel.Text = String.Format("checking {0} of {1} - {2}", count, SelectedHosts.Count, Host.Key)

            Dim lvi As ListViewItem = GetListViewItemForHost(Host)
            Dim pass As String = Host.Value
            If pass = "" Then pass = ShellCommonPass 'ShellPassTextBox.Text
            If lvi Is Nothing Then Throw New Exception("A firewall has been removed from the hosts list unexpectedly!")

            lvi.ImageKey = "wait"
            Application.DoEvents()

            'now we can talk to the firewall..
            Dim result As XGShellConnection.ExpectResult = SSH.RegisterToCentral(Host.Key, "admin", pass, CentralUser, CentralPass, CM, CR, CMBackup, loglevel)

            'interpret results and update the line item accordingly
            lvi.SubItems.Add(result.Summary)
            lvi.SubItems.Add(Now.ToString("yyyy-MM-dd h:mm:ss tt"))

            If result.Success Then
                lvi.ImageKey = "gray check"
                XGShellConnection.WriteToLog(XGShellConnection.LogSeverity.Informational, result.Summary, loglevel)
            Else

                XGShellConnection.WriteToLog(XGShellConnection.LogSeverity.Error, result.Summary, loglevel)
            End If


            If result.Success Then
                If result.Summary.Contains("Central Service(s) enabled") Then
                    lvi.ImageKey = "gray check"
                Else
                    lvi.ImageKey = "pause"
                End If
                XGShellConnection.WriteToLog(XGShellConnection.LogSeverity.Informational, result.Summary, loglevel)
            ElseIf result.Summary.Contains("Timeout") Then
                lvi.ImageKey = "timeout"
                XGShellConnection.WriteToLog(XGShellConnection.LogSeverity.Error, result.Summary, loglevel)
            Else
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
                XGShellConnection.WriteToLog(XGShellConnection.LogSeverity.Error, result.Summary, loglevel)
            End If
            AddResultsLog(Host.Key, lvi.ImageKey, result.Summary)
            ResultsListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent) 'And ColumnHeaderAutoResizeStyle.HeaderSize)
        Next

        'finished. update status and clean up
        StatusToolStripStatusLabel.Text = "Finished"
        ToolStripProgressBar.Visible = False
        ActionToolStripMenuItem.Enabled = True
    End Sub

    Private Sub DoDeRegister(loglevel As XGShellConnection.LogSeverity)
        'Throw New NotImplementedException

        Dim SelectedHosts() As KeyValuePair(Of String, String) = GetSelectedHosts()
        Dim confirm As New ChangeConfirmation(SelectedHosts.Count, loglevel = XGShellConnection.LogSeverity.Debug)
        If confirm.ShowDialog = DialogResult.Cancel Then Exit Sub
        If confirm.DebugLogging Then loglevel = XGShellConnection.LogSeverity.Debug
        If confirm.ShowDialog = DialogResult.Cancel Then Exit Sub

        ActionToolStripMenuItem.Enabled = False
        ToolStripProgressBar.Maximum = SelectedHosts.Count
        ToolStripProgressBar.Minimum = 0
        ToolStripProgressBar.Value = 0
        ToolStripProgressBar.Visible = True
        Dim count As Integer = 0

        'clear results window
        ListHosts()
        For Each Host As KeyValuePair(Of String, String) In SelectedHosts
            count += 1
            ToolStripProgressBar.Value += 1
            StatusToolStripStatusLabel.Text = String.Format("checking {0} of {1} - {2}", count, SelectedHosts.Count, Host.Key)
            Dim lvi As ListViewItem = GetListViewItemForHost(Host)
            lvi.ImageKey = "wait"

            Dim SSH As New XGShellConnection(TrustInitialSSHFingerprintToolStripMenuItem.Checked, DataKey, DataIV)
            Dim pass As String = Host.Value
            If pass = "" Then pass = ShellCommonPass 'ShellPassTextBox.Text

            Dim result As XGShellConnection.ExpectResult = SSH.DeRegisterFromCentral(Host.Key, "admin", pass, loglevel)
            lvi.SubItems.Add(result.Summary)
            lvi.SubItems.Add(Now.ToString("yyyy-MM-dd h:mm:ss tt"))

            Select Case result.Summary
                Case "SUCCESS: Registration cleared successfully"
                    lvi.ImageKey = "success"

                Case "FAIL: Not registered"
                    lvi.ImageKey = "gray check"

                Case "FAIL: Timeout"
                    lvi.ImageKey = "timeout"

                Case Else
                    lvi.ImageKey = "fail"

            End Select
            AddResultsLog(Host.Key, lvi.ImageKey, result.Summary)
            ResultsListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)

        Next
        'finished. update status and clean up
        StatusToolStripStatusLabel.Text = "Finished"
        ToolStripProgressBar.Visible = False
        ActionToolStripMenuItem.Enabled = True
    End Sub

    Private Sub DoSetAdminPassword(loglevel As XGShellConnection.LogSeverity)

        Dim np As New NewPassword
        Dim newpass As String
        Dim Generate As Boolean
        If np.ShowDialog = DialogResult.OK Then
            Generate = np.GeneratePasswords
            newpass = np.NewPassword
        Else
            Exit Sub
        End If
        Dim SelectedHosts() As KeyValuePair(Of String, String) = GetSelectedHosts()
        Dim confirm As New ChangeConfirmation(SelectedHosts.Count, loglevel = XGShellConnection.LogSeverity.Debug)
        If confirm.ShowDialog = DialogResult.Cancel Then Exit Sub
        If confirm.DebugLogging Then loglevel = XGShellConnection.LogSeverity.Debug

        ActionToolStripMenuItem.Enabled = False
        ToolStripProgressBar.Maximum = SelectedHosts.Count
        ToolStripProgressBar.Minimum = 0
        ToolStripProgressBar.Value = 0
        ToolStripProgressBar.Visible = True
        Dim count As Integer = 0

        'clear results window
        ListHosts()
        Dim Logname As String = String.Format("PasswordChanges-{0}", Now.ToString("yyyy-MM-dd_h-mm_tt"))
        Dim loglines As String = ""
        Dim Logfilename As String = FileHelpers.MakeUniqueFilename(Logname & ".enc")
        Logname = IO.Path.GetFileNameWithoutExtension(Logfilename)
        For Each Host As KeyValuePair(Of String, String) In SelectedHosts
            count += 1
            ToolStripProgressBar.Value += 1
            StatusToolStripStatusLabel.Text = String.Format("checking {0} of {1} - {2}", count, SelectedHosts.Count, Host.Key)
            Dim lvi As ListViewItem = GetListViewItemForHost(Host)
            lvi.ImageKey = "wait"

            Dim SSH As New XGShellConnection(TrustInitialSSHFingerprintToolStripMenuItem.Checked, DataKey, DataIV)
            Dim pass As String = Host.Value
            If pass = "" Then pass = ShellCommonPass 'ShellPassTextBox.Text

            If Generate OrElse Not newpass = pass Then
                If Generate Then newpass = GetRandomString(32, np.EnforceComplexity)
                loglines &= String.Format("""{0}"",""{1}"",""{2}"",""{3}""{4}", Now.ToString("yyyy-MM-dd h:mm:ss tt"), Host.Key, newpass, "Setting password", vbNewLine)
                System.IO.File.WriteAllText(Logfilename, AESWrapper.Encrypt(loglines))
                Dim result As XGShellConnection.ExpectResult = SSH.SetAdminPassword(Host.Key, "admin", pass, newpass, loglevel)
                loglines &= String.Format("""{0}"",""{1}"",""{2}"",""{3}""{4}", Now.ToString("yyyy-MM-dd h:mm:ss tt"), Host.Key, newpass, result.Summary, vbNewLine)
                System.IO.File.WriteAllText(Logfilename, AESWrapper.Encrypt(loglines))

                lvi.SubItems.Add(result.Summary)
                lvi.SubItems.Add(Now.ToString("yyyy-MM-dd h:mm:ss tt"))

                If result.Success Then
                    lvi.ImageKey = "success"
                    Dim newhost As KeyValuePair(Of String, String)
                    If newpass.Equals(ShellCommonPass) Then
                        newhost = New KeyValuePair(Of String, String)(Host.Key, "")
                    Else
                        newhost = New KeyValuePair(Of String, String)(Host.Key, newpass)
                    End If
                    AddNewHost(newhost)
                    FinishAddNewHosts()
                    SaveHosts(IncrementalAutoSave)
                    UpdateHostsList()
                Else
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

                    AddResultsLog(Host.Key, lvi.ImageKey, result.Summary)
                    XGShellConnection.WriteToLog(XGShellConnection.LogSeverity.Error, result.Summary, loglevel)
                End If
            Else
                lvi.SubItems.Add("No change needed")
                lvi.SubItems.Add(Now.ToString("yyyy-MM-dd h:mm:ss tt"))
                lvi.ImageKey = "gray check"

            End If
            ResultsListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)
            '
        Next
        Dim lv As New LogViewer(DataKey, DataIV)
        lv.ShowLog(Logname)
        lv.ShowDialog()

        FinishAddNewHosts()
        SaveHosts(IncrementalAutoSave)
        UpdateHostsList()

        If Not Generate Then
            If Not newpass.Equals(ShellCommonPass) Then
                If MsgBox("Update complete. do you want to update your common firewall password to match the psasword you just set?", MsgBoxStyle.YesNo + MsgBoxStyle.Question) = MsgBoxResult.Yes Then
                    ShellCommonPass = newpass
                    ShellPassTextBox.Text = newpass
                    SaveSettings(IncrementalAutoSave)
                End If
            End If
        End If

        'finished. update status and clean up
        StatusToolStripStatusLabel.Text = "Finished"
        ToolStripProgressBar.Visible = False
        ActionToolStripMenuItem.Enabled = True
    End Sub

#End Region

#Region "Private Methods"

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

    Private Sub DeleteSelected()
        Dim selected As KeyValuePair(Of String, String)() = GetSelectedHosts()
        Dim removedlist As New List(Of KeyValuePair(Of String, String))
        '
        For Each host As KeyValuePair(Of String, String) In Hosts
            If Not selected.Contains(host) Then removedlist.Add(host)
        Next
        '
        Hosts.Clear()
        Hosts.AddRange(removedlist)
        SaveHosts(IncrementalAutoSave)
        UpdateHostsList()
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
                            AddNewHost(Host)

                        Else
                            Dim Host As New KeyValuePair(Of String, String)(ImportedHostLine, "")
                            AddNewHost(Host)

                        End If
                    End If
                Next

                'combine unique entries
                FinishAddNewHosts()
                SaveHosts(IncrementalAutoSave)
                UpdateHostsList()
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
            Using key As Microsoft.Win32.RegistryKey = My.Computer.Registry.CurrentUser.CreateSubKey("Software\XGMigrationHelper", RegistryKeyPermissionCheck.Default, rs)
                key.SetAccessControl(rs)
            End Using
            Using key As Microsoft.Win32.RegistryKey = My.Computer.Registry.CurrentUser.CreateSubKey("Software\XGMigrationHelper\Hosts")
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
            Dim ekey As Byte() = Convert.FromBase64String(DataKey)
            Dim iv As Byte() = Convert.FromBase64String(DataIV)
            Dim output As Byte()
            ReDim output(ekey.Length + iv.Length - 1)
            ekey.CopyTo(output, 0)
            iv.CopyTo(output, ekey.Length)
            IO.File.WriteAllBytes(sfd.FileName, output)
        End If
    End Sub

    Private Sub LoadSavedValues()
        Using key As Microsoft.Win32.RegistryKey = My.Computer.Registry.CurrentUser.CreateSubKey("Software\XGMigrationHelper")
            TrustInitialSSHFingerprintToolStripMenuItem.Checked = key.GetValue("TrustInitialSSHFingerprint", True)

            DataIV = key.GetValue("Init3")
            If DataIV Is Nothing OrElse DataIV.Length < 16 Then
                DataIV = Convert.ToBase64String(GetRandomBytes(16))
                key.SetValue("Init3", DataIV)
            End If
            Dim login As New Login

            If login.ShowDialog() = DialogResult.OK Then
                DataKey = login.Key
                If login.NewPassword Then
                    If MsgBox("Do you want to backup your key, in case you ever lose the password?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Backup") = MsgBoxResult.Yes Then
                        BackupKey()

                    End If
                End If
                login.Dispose()
                'If DataKey Is Nothing OrElse DataKey.Length = 0 Then Exit Sub
                AESWrapper = New AES256Wrapper(DataKey, DataIV)
                '
                Try
                    ShellCommonPass = AESWrapper.Decrypt(key.GetValue("CommonShell", ""))
                    CentralUser = key.GetValue("LastEmail", "")
                    CentralPass = AESWrapper.Decrypt(key.GetValue("LastEmailPass", ""))
                Catch ex As Exception
                    MsgBox(ex.ToString)
                End Try
                '
                ShellPassTextBox.Text = ShellCommonPass
                '
                Using hostskey = key.CreateSubKey("Hosts")
                    Dim reg_hosts As String() = hostskey.GetValueNames()
                    Hosts.Clear()
                    For Each hostname As String In reg_hosts
                        If hostname.Length > 0 Then
                            Dim rawpass As String = hostskey.GetValue(hostname)
                            Dim pass As String = AESWrapper.Decrypt(rawpass)
                            Dim host As New KeyValuePair(Of String, String)(hostname, pass)
                            Hosts.Add(host)
                        End If
                    Next
                End Using
            Else
                Application.Exit()
            End If
            LoadingBool = False
        End Using
        ListHosts()
        EnableDisable()
    End Sub

    Private Sub SaveSettings(Optional Force As Boolean = False)
        If Force Then
            Dim key As Microsoft.Win32.RegistryKey
            key = My.Computer.Registry.CurrentUser.CreateSubKey("Software\XGMigrationHelper")
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
                    key.SetValue("LastEmailPass", AESWrapper.Encrypt(CentralPass))
                Else
                    key.DeleteValue("LastEmailPass")
                End If
            Catch ex As Exception
                Debug.Print("CentralPass len {0} error {1}", CentralPass.Length, ex.Message)
            End Try

            Try
                If ShellCommonPass.Length Then
                    key.SetValue("CommonShell", AESWrapper.Encrypt(ShellCommonPass))
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
            Using key As Microsoft.Win32.RegistryKey = My.Computer.Registry.CurrentUser.CreateSubKey("Software\XGMigrationHelper\Hosts")
                Try
                    For Each hostname As String In key.GetSubKeyNames
                        key.DeleteValue(hostname)
                    Next
                Catch ex As Exception
                    Debug.Print("E082432:" & ex.Message)
                End Try
                Try
                    For Each host As KeyValuePair(Of String, String) In Hosts
                        key.SetValue(host.Key, AESWrapper.Encrypt(host.Value))
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

    Private Sub AddHost(Optional defaulthost As String = "", Optional defaultpass As String = "")
        Dim sshhost As String
        Dim sshpass As String
        Dim af As AddFirewalls
        Dim another As Boolean = False
        If defaultpass = Nothing Then defaultpass = ""
        Do
            af = New AddFirewalls(DataKey, DataIV) With {.CommonPassword = ShellCommonPass}
            If defaulthost.Length > 0 Then af.SSHHost = defaulthost
            If Not defaultpass.Equals("*") Then af.SSHPass = defaultpass
            af.AddAnother = another
            Select Case af.ShowDialog
                Case DialogResult.OK
                    sshhost = af.SSHHost
                    sshpass = af.SSHPass
                    Dim host As New KeyValuePair(Of String, String)(sshhost, sshpass)
                    AddNewHost(host)
                    '
                Case DialogResult.Cancel
                    Exit Do
                    '
                Case Else
                    Exit Do
                    '
            End Select
            '
            FinishAddNewHosts()
            another = af.AddAnother
            '
        Loop While another
        '
        EnableDisable()
        SaveHosts()
        UpdateHostsList()
    End Sub

    Private Function GetHostByName(Host As String) As KeyValuePair(Of String, String)
        For Each thishost As KeyValuePair(Of String, String) In Hosts
            If thishost.Key.Equals(Host) Then Return thishost
        Next
        Return Nothing
    End Function

    Private Sub AddNewHost(newhost As KeyValuePair(Of String, String))
        If newhost.Key Is Nothing Then
            Exit Sub
        End If
        If newhost.Key.Trim.Length = 0 Then
            Exit Sub
        End If
        NewHosts.Add(newhost)
    End Sub

    Private Sub FinishAddNewHosts()
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
    End Sub

    Private Sub ListHosts(Optional clear As Boolean = True)
        Dim results As New List(Of String)
        Dim timestamps As New List(Of String)

        Dim selectedhosts As KeyValuePair(Of String, String)() = {}
        If ResultsListView.CheckedItems.Count > 0 Then selectedhosts = GetSelectedHosts()
        Try
            For Each lvi As ListViewItem In ResultsListView.Items
                results.Add(lvi.SubItems(2).Text)
                timestamps.Add(lvi.SubItems(3).Text)
            Next
        Catch ex As Exception
            clear = True
        End Try

        ResultsListView.Items.Clear()
        Dim count As Integer = 0
        For Each host As KeyValuePair(Of String, String) In Hosts
            Dim lvi As ListViewItem = ResultsListView.Items.Add(host.Key)
            lvi.SubItems.Add(If(host.Value = "", "-", "*****"))
            lvi.ImageKey = ""
            lvi.Checked = selectedhosts.Contains(host, New KeyComparer)
            If Not clear Then
                lvi.SubItems.Add(results.Item(count))
                lvi.SubItems.Add(timestamps.Item(count))
            End If
            count += 1
        Next

        ResultsListView.Sorting = SortOrder.Ascending
        'Set the ListviewItemSorter property to a new ListviewItemComparer object
        Me.ResultsListView.ListViewItemSorter = New ListViewItemComparer(0, ResultsListView.Sorting)

        ' Call the sort method to manually sort
        ResultsListView.Sort()
        ResultsListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent And ColumnHeaderAutoResizeStyle.HeaderSize)

    End Sub

    Private Sub UpdateHostsList()

        Dim DeletedItems As New List(Of ListViewItem)
        For Each lvi As ListViewItem In ResultsListView.Items
            Dim Host As KeyValuePair(Of String, String) = GetHostByName(lvi.Text)
            If IsNothing(Host) Then
                DeletedItems.Add(lvi)
            Else
                lvi.SubItems(1).Text = If(Host.Value = "", "-", "*****")
            End If
        Next
        '
        For Each host As KeyValuePair(Of String, String) In Hosts
            If GetListViewItemForHost(host) Is Nothing Then
                Dim lvi As ListViewItem = ResultsListView.Items.Add(host.Key)
                lvi.SubItems.Add(If(host.Value = "", "-", "*****"))
                lvi.ImageKey = ""
            End If
        Next
        '
        For Each lvi As ListViewItem In DeletedItems
            ResultsListView.Items.Remove(lvi)
        Next

    End Sub

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
        AddHost()
    End Sub

    Private Sub DeleteSelectedToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteSelectedToolStripMenuItem.Click
        DeleteSelected()
    End Sub

    Private Sub ChangeCentralCredentialsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ChangeCentralCredentialsToolStripMenuItem.Click
        SetCentralCreds()
    End Sub

#End Region

#Region "View Menu"

    Private Sub PasswordChangeLogsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PasswordChangeLogsToolStripMenuItem.Click
        Dim lv As New LogViewer(DataKey, DataIV)
        lv.ShowDialog()
    End Sub

#End Region

#Region "Action Menu"

    Private Sub RegisterEnableAllCentralServicesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RegisterEnableAllCentralServicesToolStripMenuItem.Click
        Dim LogLevel As XGShellConnection.LogSeverity = GetSet()
        If LogLevel = Nothing Then Exit Sub
        DoMigration(LogLevel, True, True, True)
        TopPanel.Enabled = True
    End Sub


    Private Sub EnableCentralManagementOnlyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EnableCentralManagementOnlyToolStripMenuItem.Click
        Dim LogLevel As XGShellConnection.LogSeverity = GetSet()
        If LogLevel = Nothing Then Exit Sub
        DoMigration(LogLevel, True, True, False)
        TopPanel.Enabled = True
    End Sub

    Private Sub EnableCentralReportingOnlyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EnableCentralReportingOnlyToolStripMenuItem.Click
        Dim LogLevel As XGShellConnection.LogSeverity = GetSet()
        If LogLevel = Nothing Then Exit Sub
        DoMigration(LogLevel, False, False, True)
        TopPanel.Enabled = True
    End Sub

    Private Sub DeregisterFromSophosCentralToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeregisterFromSophosCentralToolStripMenuItem.Click
        Dim LogLevel As XGShellConnection.LogSeverity = GetSet()
        If LogLevel = Nothing Then Exit Sub
        DoDeRegister(LogLevel)
        TopPanel.Enabled = True
    End Sub

    Private Sub BulkChangeadminPasswordToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BulkChangeadminPasswordToolStripMenuItem.Click
        Dim LogLevel As XGShellConnection.LogSeverity = GetSet()
        If LogLevel = Nothing Then Exit Sub
        DoSetAdminPassword(LogLevel)
        TopPanel.Enabled = True
    End Sub

    Private Sub CheckCurrentFirmwareVersionToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CheckCurrentFirmwareVersionToolStripMenuItem.Click
        Dim LogLevel As XGShellConnection.LogSeverity = GetSet()
        If LogLevel = Nothing Then Exit Sub
        DoVersionCheck(LogLevel)
        TopPanel.Enabled = True

    End Sub

    Private Sub InstallAnyAvailableHotfixesevenIfToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles InstallAnyAvailableHotfixesevenIfToolStripMenuItem.Click
        Dim LogLevel As XGShellConnection.LogSeverity = GetSet()
        If LogLevel = Nothing Then Exit Sub
        DoHotfixInstall(LogLevel)
        TopPanel.Enabled = True
    End Sub

    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click

    End Sub

    Private Sub TrustInitialSSHFingerprintToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TrustInitialSSHFingerprintToolStripMenuItem.Click
        TrustInitialSSHFingerprintToolStripMenuItem.Checked = Not TrustInitialSSHFingerprintToolStripMenuItem.Checked
        Using key As Microsoft.Win32.RegistryKey = My.Computer.Registry.CurrentUser.CreateSubKey("Software\XGMigrationHelper")
            key.SetValue("TrustInitialSSHFingerprint", TrustInitialSSHFingerprintToolStripMenuItem.Checked)
        End Using

    End Sub


#End Region


#End Region

End Class