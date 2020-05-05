Imports System.Management
Imports System.Security.Cryptography
Imports System.IO.Compression

Public Class MainForm


    'temporary quick encryption, replaced now
    'ReadOnly ENC As New Hider(My.Computer.Name & "wertyuj3k4er8foji4rewfoyiczld4d53ads6assad7")

    'AES256 SHA2
    ReadOnly Key1 As String = "9P5xruz;=FH""Q^Xzj'1[#nbKY7vwgGVl7Q$(=N?Pyw\gnM,])2*7jJmPK[qdMJu"
    ReadOnly key2 As String = My.Computer.Name & SerialNumbers()
    Private key3 As String
    Private AESWrapper As AES256Wrapper

    'Public host As KeyValuePair(Of String, String)

    Public Hosts As New List(Of KeyValuePair(Of String, String))
    ReadOnly NewHosts As New List(Of KeyValuePair(Of String, String))
    Private CentralUser As String = ""
    Private CentralPass As String = ""
    Private ShellCommonPass As String = ""
    Private CM As Boolean = True
    Private CMBackup As Boolean = True
    Private CR As Boolean = True
    Private LoadingBool As Boolean = False

    Private Enum ActionOptions
        'Asnarök_Activity_Check
        Change_admin_password
        Check_Current_Version
        Install_Available_Hotfixes
        Enable_All_Central_Services
        Disable_All_Central_Services
        Enable_Central_Management
        Enable_Central_Management_and_Backups
        Enable_Central_Reporting
        Deregister_from_Central
    End Enum

#Region "Form Control Events"

    Private Sub GoButton_Click(sender As Object, e As EventArgs) Handles GoButton.Click

        Dim LogLevel As XGShellConnection.LogSeverity = XGShellConnection.LogSeverity.Informational
        '
        If My.Computer.Keyboard.CtrlKeyDown Then
            MsgBox("Debug logging enabled", MsgBoxStyle.Information)
            LogLevel = XGShellConnection.LogSeverity.Debug
        End If
        '
        Dim sfcount As Integer = ResultsListView.CheckedItems.Count
        If sfcount = 0 Then sfcount = Hosts.Count
        If sfcount = 0 Then Exit Sub
        '
        StatusToolStripStatusLabel.Text = "Status: Getting ready"
        TopPanel.Enabled = False
        Dim DoMigrate As Boolean = False
        Select Case ActionComboBox.SelectedIndex
            Case ActionOptions.Check_Current_Version '"Check Current Version"
                DoVersionCheck(LogLevel)

            Case ActionOptions.Install_Available_Hotfixes '"Install Available Hotfix(es)"
                If MsgBox("Are you sure you want to trigger hotfix installation on " & sfcount & " firewall(s)?",
                          MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, "Please Confirm") = MsgBoxResult.Yes Then
                    DoHotfixInstall(LogLevel)
                End If

            Case ActionOptions.Enable_All_Central_Services '"Enable All Central Services"
                CM = True : CR = True : CMBackup = True
                DoMigrate = True

            Case ActionOptions.Enable_Central_Management '"Enable Central Management"
                CM = True : CR = False : CMBackup = False
                DoMigrate = True

            Case ActionOptions.Enable_Central_Management_and_Backups ' "Enable Central Management + Backups"
                CM = True : CR = False : CMBackup = True
                DoMigrate = True

            Case ActionOptions.Enable_Central_Reporting '"Enable Central Reporting"
                CM = False : CR = True : CMBackup = False
                DoMigrate = True

            Case ActionOptions.Disable_All_Central_Services '"Disable All Central Services"
                CM = False : CR = False : CMBackup = False
                DoMigrate = True

            Case ActionOptions.Deregister_from_Central '"De-Register from Central"
                If MsgBox("Are you sure you want to De-register " & sfcount & " firewall(s) from Sophos Central?",
                          MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, "Please Confirm") = MsgBoxResult.Yes Then
                    DoDeRegister(LogLevel)
                End If
            Case ActionOptions.Change_admin_password
                If MsgBox("Are you sure you want to change the 'admin' account password on " & sfcount & " firewall(s)?",
                          MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, "Please Confirm") = MsgBoxResult.Yes Then
                    DoSetAdminPassword(LogLevel)
                End If
        End Select

        If DoMigrate Then
            If MsgBox("Are you sure you want to migrate " & sfcount & " firewall(s) to Sophos Central?",
                      MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Please Confirm") = MsgBoxResult.Yes Then
                DoMigration(LogLevel)
            End If
        End If

        TopPanel.Enabled = True
    End Sub

    Private Function GetListViewItemForHost(host As KeyValuePair(Of String, String)) As ListViewItem
        For Each this As ListViewItem In ResultsListView.Items
            If this.Text = host.Key Then Return this
        Next
        Return Nothing
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

    Private Sub CentralUserTextBox_TextChanged(sender As Object, e As EventArgs) Handles ShellPassTextBox.TextChanged
        If LoadingBool Then Exit Sub
        ShellCommonPass = ShellPassTextBox.Text
        SaveSettings()
        EnableDisable()

    End Sub

    Private Sub AddHostButton_Click(sender As Object, e As EventArgs) Handles AddHostButton.Click
        AddHost()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadingBool = True
        Dim MyActionOption As ActionOptions = 0
        ActionComboBox.Items.Clear()
        Do
            Dim ThisAction As String = MyActionOption.ToString.Replace("_", " ")
            If IsNumeric(ThisAction) Then Exit Do
            ActionComboBox.Items.Add(ThisAction)
            MyActionOption += 1
        Loop
        ActionComboBox.Text = ActionComboBox.Items(0)
        SerialNumbers()
        LoadSavedValues()
        '
    End Sub

    Private Sub CentralCredsButton_Click(sender As Object, e As EventArgs) Handles CentralCredsButton.Click
        SetCentralCreds()

    End Sub

    Private Sub DeleteButton_Click(sender As Object, e As EventArgs) Handles DeleteButton.Click
        Dim selected As KeyValuePair(Of String, String)() = GetSelectedHosts()
        Dim removedlist As New List(Of KeyValuePair(Of String, String))
        '
        For Each host As KeyValuePair(Of String, String) In Hosts
            If Not selected.Contains(host) Then removedlist.Add(host)
        Next
        '
        Hosts.Clear()
        Hosts.AddRange(removedlist)
        SaveHosts()
        UpdateHostsList()
        '
    End Sub

    Private Sub ResultsListView_ItemChecked(sender As Object, e As ItemCheckedEventArgs) Handles ResultsListView.ItemChecked
        EnableDisable()
    End Sub

    Private Sub BrowseButton_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
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
                SaveHosts()
                UpdateHostsList()
            Catch ex As Exception
                Debug.Print("E04683" & ex.Message)
            End Try

        End If
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

    Private Sub DoHotfixInstall(loglevel As XGShellConnection.LogSeverity)

        Dim SelectedHosts() As KeyValuePair(Of String, String) = GetSelectedHosts()
        GoButton.Enabled = False
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

            Dim SSH As New XGShellConnection
            Dim pass As String = Host.Value
            If pass = "" Then pass = ShellCommonPass 'ShellPassTextBox.Text

            Dim result As String = SSH.InstallHotfixes(Host.Key, "admin", pass, loglevel)

            lvi.SubItems.Add(result)
            lvi.SubItems.Add(Now.ToString("yyyy-MM-dd h:mm:ss tt"))
            If result.Contains("200") Then
                If result.Contains("remains") Then
                    lvi.ImageKey = "gray check"
                Else
                    lvi.ImageKey = "green check"
                End If

                XGShellConnection.WriteToLog(XGShellConnection.LogSeverity.Informational, result, loglevel)
            Else
                lvi.ImageKey = "fail"

                XGShellConnection.WriteToLog(XGShellConnection.LogSeverity.Error, result, loglevel)
            End If

            ResultsListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent) 'And ColumnHeaderAutoResizeStyle.HeaderSize)
        Next
        'finished. update status and clean up
        StatusToolStripStatusLabel.Text = "Finished"
        ToolStripProgressBar.Visible = False
        GoButton.Enabled = True
    End Sub

    Private Sub DoVersionCheck(loglevel As XGShellConnection.LogSeverity)

        Dim SelectedHosts() As KeyValuePair(Of String, String) = GetSelectedHosts()
        GoButton.Enabled = False
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

            Dim SSH As New XGShellConnection
            Dim pass As String = Host.Value
            If pass = "" Then pass = ShellCommonPass 'ShellPassTextBox.Text

            Dim result As String = SSH.CheckCurrentFirmwareVersion(Host.Key, "admin", pass, loglevel)

            lvi.SubItems.Add(result)
            lvi.SubItems.Add(Now.ToString("yyyy-MM-dd h:mm:ss tt"))
            If result.Contains("NO HOTFIX") Then
                lvi.ImageKey = "fail"
                XGShellConnection.WriteToLog(XGShellConnection.LogSeverity.Informational, result, loglevel)
            Else
                lvi.ImageKey = ""
                XGShellConnection.WriteToLog(XGShellConnection.LogSeverity.Informational, result, loglevel)
            End If

            ResultsListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent) 'And ColumnHeaderAutoResizeStyle.HeaderSize)
        Next
        'finished. update status and clean up
        StatusToolStripStatusLabel.Text = "Finished"
        ToolStripProgressBar.Visible = False
        GoButton.Enabled = True
    End Sub

    Private Sub DoMigration(loglevel As XGShellConnection.LogSeverity)
        If CentralUser.Length = 0 Or CentralPass.Length = 0 Then SetCentralCreds()
        If CentralUser.Length = 0 Or CentralPass.Length = 0 Then
            MsgBox("Cannot proceed without Sophos Central credentals.", MsgBoxStyle.Exclamation, "Credentials needed")
            Exit Sub
        End If

        Dim Selection As IEnumerable = ResultsListView.Items

        If ResultsListView.CheckedItems.Count > 0 Then Selection = ResultsListView.CheckedItems

        Dim SelectedHosts() As KeyValuePair(Of String, String) = GetSelectedHosts()
        GoButton.Enabled = False
        ToolStripProgressBar.Maximum = SelectedHosts.Count
        ToolStripProgressBar.Minimum = 0
        ToolStripProgressBar.Value = 0
        ToolStripProgressBar.Visible = True
        Dim count As Integer = 0
        ListHosts()

        For Each Host As KeyValuePair(Of String, String) In SelectedHosts

            Dim SSH As New XGShellConnection
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
            Dim result As String = SSH.RegisterToCentral(Host.Key, "admin", pass, CentralUser, CentralPass, CM, CR, CMBackup, loglevel)

            'interpret results and update the line item accordingly
            lvi.SubItems.Add(result)
            lvi.SubItems.Add(Now.ToString("yyyy-MM-dd h:mm:ss tt"))

            If result.Contains("Approval Pending") Then
                lvi.ImageKey = "pause"
                XGShellConnection.WriteToLog(XGShellConnection.LogSeverity.Informational, result, loglevel)
            ElseIf result.Contains("Timeout") Then
                lvi.ImageKey = "timeout"
                XGShellConnection.WriteToLog(XGShellConnection.LogSeverity.Error, result, loglevel)
            ElseIf result.Contains("Central Service(s) enabled") Then
                lvi.ImageKey = "gray check"
                XGShellConnection.WriteToLog(XGShellConnection.LogSeverity.Informational, result, loglevel)
            Else
                lvi.ImageKey = "fail"
                XGShellConnection.WriteToLog(XGShellConnection.LogSeverity.Error, result, loglevel)
            End If
            ResultsListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent) 'And ColumnHeaderAutoResizeStyle.HeaderSize)
        Next

        'finished. update status and clean up
        StatusToolStripStatusLabel.Text = "Finished"
        ToolStripProgressBar.Visible = False
        GoButton.Enabled = True
    End Sub

    Private Sub DoDeRegister(loglevel As XGShellConnection.LogSeverity)
        'Throw New NotImplementedException

        Dim SelectedHosts() As KeyValuePair(Of String, String) = GetSelectedHosts()
        GoButton.Enabled = False
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

            Dim SSH As New XGShellConnection
            Dim pass As String = Host.Value
            If pass = "" Then pass = ShellCommonPass 'ShellPassTextBox.Text

            Dim result As String = SSH.DeRegisterFromCentral(Host.Key, "admin", pass, loglevel)
            lvi.SubItems.Add(result)
            lvi.SubItems.Add(Now.ToString("yyyy-MM-dd h:mm:ss tt"))

            Select Case result
                Case "Registration cleared successfully"
                    lvi.ImageKey = "success"

                Case "Not registered"
                    lvi.ImageKey = "gray check"

                Case "Timeout"
                    lvi.ImageKey = "timeout"

                Case Else
                    lvi.ImageKey = "fail"

            End Select
            ResultsListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)

        Next
        'finished. update status and clean up
        StatusToolStripStatusLabel.Text = "Finished"
        ToolStripProgressBar.Visible = False
        GoButton.Enabled = True
    End Sub

    Private Sub DoSetAdminPassword(loglevel As XGShellConnection.LogSeverity)
        Dim SelectedHosts() As KeyValuePair(Of String, String) = GetSelectedHosts()
        GoButton.Enabled = False
        ToolStripProgressBar.Maximum = SelectedHosts.Count
        ToolStripProgressBar.Minimum = 0
        ToolStripProgressBar.Value = 0
        ToolStripProgressBar.Visible = True
        Dim count As Integer = 0

        Dim np As New NewPassword
        Dim newpass As String
        Dim Generate As Boolean
        If np.ShowDialog = DialogResult.OK Then
            Generate = np.GeneratePasswords
            newpass = np.NewPassword
        Else
            Exit Sub
        End If

        'clear results window
        ListHosts()
        Dim Logname As String = String.Format("PasswordChanges-{0}", Now.ToString("yyyy-MM-dd_h-mm_tt"))
        Dim loglines As String = ""
        For Each Host As KeyValuePair(Of String, String) In SelectedHosts
            count += 1
            ToolStripProgressBar.Value += 1
            StatusToolStripStatusLabel.Text = String.Format("checking {0} of {1} - {2}", count, SelectedHosts.Count, Host.Key)
            Dim lvi As ListViewItem = GetListViewItemForHost(Host)
            lvi.ImageKey = "wait"

            Dim SSH As New XGShellConnection
            Dim pass As String = Host.Value
            If pass = "" Then pass = ShellCommonPass 'ShellPassTextBox.Text

            If Generate OrElse Not newpass = pass Then
                If Generate Then newpass = RandomString(32)
                Dim result As XGShellConnection.ExpectResult = SSH.SetAdminPassword(Host.Key, "admin", pass, newpass, loglevel)

                loglines &= String.Format("""{0}"",""{1}"",""{2}"",""{3}""{4}", Now.ToString("yyyy-MM-dd h:mm:ss tt"), Host.Key, newpass, result.Summary, vbNewLine)
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
                ElseIf result.Exception IsNot Nothing Then

                    If result.Exception.Message.Contains("time") Then
                        lvi.ImageKey = "timeout"

                    Else
                        lvi.ImageKey = "fail"

                    End If
                End If
            Else
                lvi.SubItems.Add("No change needed")
                lvi.SubItems.Add(Now.ToString("yyyy-MM-dd h:mm:ss tt"))
                lvi.ImageKey = "gray check"

            End If
            ResultsListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)
            '
        Next

        Dim Logfilename As String = FileHelpers.MakeUniqueFilename(Logname & ".enc")
        Logname = IO.Path.GetFileNameWithoutExtension(Logfilename)

        System.IO.File.AppendAllText(Logfilename, AESWrapper.Encrypt(loglines))

        Dim lv As New LogViewer(key2 & Key1 & key3)
        lv.ShowLog(Logname)
        lv.ShowDialog()

        FinishAddNewHosts()
        SaveHosts()
        UpdateHostsList()

        If Not Generate Then
            If MsgBox("Update complete. do you want to update your common firewall password to match the psasword you just set?", MsgBoxStyle.YesNo + MsgBoxStyle.Question) = MsgBoxResult.Yes Then
                ShellCommonPass = newpass
                ShellPassTextBox.Text = newpass
                SaveSettings()
            End If
        End If
        'finished. update status and clean up
        StatusToolStripStatusLabel.Text = "Finished"
        ToolStripProgressBar.Visible = False
        GoButton.Enabled = True
    End Sub
#End Region

#Region "Private Methods"

    Private Sub LoadSavedValues()
        Using key As Microsoft.Win32.RegistryKey = My.Computer.Registry.CurrentUser.CreateSubKey("Software\Sophos\XGManagementMigration")
            key3 = key.GetValue("telemetry", Guid.NewGuid.ToString)
            key.SetValue("telemetry", key3)
            '
            AESWrapper = New AES256Wrapper(key2 & Key1 & key3)
            '
            Try
                ShellCommonPass = AESWrapper.Decrypt(key.GetValue("CommonShell"))
                CentralUser = key.GetValue("LastEmail")
                CentralPass = AESWrapper.Decrypt(key.GetValue("LastEmailPass"))
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
                        Debug.Print("Loading {0} - epass:'{1}' dpass:'{2}'", hostname, rawpass, pass)
                        Hosts.Add(host)
                    End If
                Next
            End Using
        End Using
        ListHosts()
        EnableDisable()
        LoadingBool = False
    End Sub

    Private Sub SaveSettings()
        Dim key As Microsoft.Win32.RegistryKey
        key = My.Computer.Registry.CurrentUser.CreateSubKey("Software\Sophos\XGManagementMigration")
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

    End Sub

    Private Sub SaveHosts()
        Dim key As Microsoft.Win32.RegistryKey
        Try
            My.Computer.Registry.CurrentUser.DeleteSubKeyTree("Software\Sophos\XGManagementMigration\Hosts")
        Catch ex As Exception
            Debug.Print("E082432:" & ex.Message)
        End Try
        Try
            key = My.Computer.Registry.CurrentUser.CreateSubKey("Software\Sophos\XGManagementMigration\Hosts")
            For Each host As KeyValuePair(Of String, String) In Hosts
                key.SetValue(host.Key, AESWrapper.Encrypt(host.Value))
            Next
        Catch ex As Exception
            Debug.Print("E9348" & ex.Message)
        End Try

    End Sub

    Private Sub EnableDisable()
        Dim En = True

        If Hosts.Count = 0 Then En = False
        GoButton.Enabled = En
        DeleteButton.Enabled = ResultsListView.CheckedItems.Count > 0
    End Sub

    Private Sub AddHost(Optional defaulthost As String = "", Optional defaultpass As String = "")
        Dim sshhost As String
        Dim sshpass As String
        Dim af As AddFirewalls
        Dim another As Boolean = False
        If defaultpass = Nothing Then defaultpass = ""
        Do
            af = New AddFirewalls
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
            'MsgBox("newhost is nothing")
            Exit Sub
        End If
        If newhost.Key.Trim.Length = 0 Then
            'MsgBox("newhost length is zero")
            Exit Sub
        End If
        NewHosts.Add(newhost)
    End Sub

    Private Function SerialNumbers() As String
        Dim q As New SelectQuery("Win32_bios")
        Dim search As New ManagementObjectSearcher(q)
        Dim info As New ManagementObject

        Dim ret As String = ""
        For Each info In search.Get
            ret &= info("serialnumber").ToString & info("version").ToString
        Next
        Return ret
    End Function

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
                    SaveSettings()
                    Exit Sub

                Case DialogResult.Cancel

                    Exit Sub
            End Select
        End While
        CentralUser = cu
        CentralPass = cp
        SaveSettings()
    End Sub

    ReadOnly CryptoRandom As RandomNumberGenerator = RandomNumberGenerator.Create()
    'Private Const allowedchars As String = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-=+!@#$%^&*()"
    Private Const allowedchars As String = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"
    Private Function RandomString(Length As Integer) As String
        Dim randomess(Length - 1) As UInt64
        For count As Integer = 0 To Length - 1
            Dim bytes(7) As Byte
            CryptoRandom.GetNonZeroBytes(bytes)
            randomess(count) = BitConverter.ToUInt64(bytes, 0)
        Next

        'minimze randomness skew by modding uint64 values
        Dim rslt As String = ""
        For Each number As UInt64 In randomess
            Dim pos As Integer = number Mod (allowedchars.Length)
            rslt &= allowedchars.Chars(pos)
        Next

        Return rslt
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim lv As New LogViewer(key2 & Key1 & key3)
        lv.ShowDialog()

    End Sub


#End Region

End Class