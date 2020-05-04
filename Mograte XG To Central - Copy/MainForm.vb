

Public Class MainForm
    <System.Runtime.InteropServices.DllImport("user32.dll")>
    Private Shared Function GetAsyncKeyState(vKey As Keys) As Short
    End Function

    Private ENC As New Hider(My.Computer.Name & "wertyuj3k4er8foji4rewfoyiczld4d53ads6assad7")
    Public host As KeyValuePair(Of String, String)

    Public Hosts() As KeyValuePair(Of String, String) = {}
    Private CentralUser As String = ""
    Private CentralPass As String = ""

    Private CM As Boolean = True
    Private CMBackup As Boolean = True
    Private CR As Boolean = True

    Private LoadingBool As Boolean = False

#Region "Form Control Events"
    Private Sub GoButton_Click(sender As Object, e As EventArgs) Handles GoButton.Click
        Dim LogLevel As XGShellConnection.LogSeverity = XGShellConnection.LogSeverity.Informational

        If Convert.ToBoolean(GetAsyncKeyState(Keys.Control)) Then
            MsgBox("Control Pressed")
            LogLevel = XGShellConnection.LogSeverity.Debug
        End If


        Select Case ActionComboBox.Text
            Case "Check Current Version"
                DoVersionCheck(LogLevel)

            Case "Asnarök Activity Check"
                DoAsnarökCheck(LogLevel)

            Case "Install Available Hotfix(es)"
                If MsgBox("Are you sure you want to trigger hotfix installation right now?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, "Please Confirm") = MsgBoxResult.Yes Then DoHotfixInstall(LogLevel)

            Case "Enable All Central Services",
                 "Enable Central Management",
                 "Enable Central Management + Backups",
                 "Enable Central Reporting",
                 "Disable All Central Services"
                If MsgBox("Are you sure you want to migrate these firewalls to Sophos Central?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Please Confirm") = MsgBoxResult.Yes Then DoMigration(LogLevel)

            Case "De-Register from Central"
                If MsgBox("Are you sure you want to De-register these firewalls from Sophos Central?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, "Please Confirm") = MsgBoxResult.Yes Then DoDeRegister(LogLevel)

        End Select

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
        SaveSettings()
        EnableDisable()


    End Sub

    Private Sub AddHostButton_Click(sender As Object, e As EventArgs) Handles AddHostButton.Click
        AddHost()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadingBool = True
        ActionComboBox.Text = ActionComboBox.Items(0)

        Dim key As Microsoft.Win32.RegistryKey
        key = My.Computer.Registry.CurrentUser.CreateSubKey("Software\Sophos\XGManagementMigration")
        CentralUser = key.GetValue("LastEmail", "")
        CentralPass = ENC.DecryptData(key.GetValue("LastEmailPass", ""))
        ShellPassTextBox.Text = ENC.DecryptData(key.GetValue("CommonShell", ""))

        key = key.CreateSubKey("Hosts")
        Dim reg_hosts As String() = key.GetValueNames()
        Hosts = {}
        For Each hostname As String In reg_hosts
            Dim pass As String = ENC.DecryptData(key.GetValue(hostname))
            Dim host As New KeyValuePair(Of String, String)(hostname, pass)
            ReDim Preserve Hosts(Hosts.Count)
            Hosts(Hosts.GetUpperBound(0)) = host
        Next
        ListHosts()
        EnableDisable()
        LoadingBool = False



    End Sub

    Private Sub CentralCredsButton_Click(sender As Object, e As EventArgs) Handles CentralCredsButton.Click
        SetCentralCreds()

    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ActionComboBox.SelectedIndexChanged
        Select Case ActionComboBox.Text
            Case "Enable All Central Services"
                CM = True
                CR = True
                CMBackup = True
            Case "Enable Central Management"
                CM = True
                CR = False
                CMBackup = False
            Case "Enable Central Management + Enable Backups"
                CM = True
                CR = False
                CMBackup = True
            Case "Enable Central Reporting"
                CM = False
                CR = True
                CMBackup = False
            Case "Disable All Central Services"
                CM = False
                CR = False
                CMBackup = False
        End Select
    End Sub

    Private Sub DeleteButton_Click(sender As Object, e As EventArgs) Handles DeleteButton.Click

        Dim selected() As KeyValuePair(Of String, String) = {}
        Debug.Print("checked listview items: {0}", ResultsListView.CheckedItems.Count)
        For Each lvi As ListViewItem In ResultsListView.CheckedItems

            For Each host As KeyValuePair(Of String, String) In Hosts
                If host.Key = lvi.Text Then
                    ReDim Preserve selected(selected.Count)
                    selected(selected.GetUpperBound(0)) = host
                    Debug.Print("Added {0} to 'selected'", host.Key)
                End If
            Next
        Next

        If selected.Count = 0 Then Exit Sub

        Dim cleaned() As KeyValuePair(Of String, String) = {}
        For Each host As KeyValuePair(Of String, String) In Hosts
            Debug.Print("{0} is in selected: {1}", host.Key, selected.Contains(host))
            If Not selected.Contains(host) Then
                ReDim Preserve cleaned(cleaned.Count)
                cleaned(cleaned.GetUpperBound(0)) = host
            End If
        Next
        Hosts = cleaned
        SaveHosts()
        ListHosts()


    End Sub

    Private Sub ResultsListView_ItemChecked(sender As Object, e As ItemCheckedEventArgs) Handles ResultsListView.ItemChecked

        EnableDisable()
    End Sub

    Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
        Dim ofd As New OpenFileDialog
        ofd.Filter = "Text files (*.txt)|*.txt"
        If ofd.ShowDialog() = DialogResult.OK Then
            Try
                Dim fwlist As String = IO.File.ReadAllText(ofd.FileName)
                Dim fwarray As String() = Split(fwlist.Replace(vbCrLf, vbLf), vbLf)
                'Dim newhosts() As KeyValuePair(Of String, String) = {}
                For Each HHHH As String In fwarray
                    Dim HHH As String = HHHH.Trim
                    If Trim(HHH).Length > 0 Then
                        If HHH.Contains(" ") Then
                            Dim HH As String = HHH.Substring(0, HHH.IndexOf(" ")).Trim
                            Dim PP As String = HHH.Substring(HHH.IndexOf(" ") + 1).Trim

                            Dim H As New KeyValuePair(Of String, String)(HH, PP)
                            'MsgBox("adding " & HH & vbNewLine & "Added " & H.Key,, "1")
                            AddNewHost(H)
                        Else

                            Dim H As New KeyValuePair(Of String, String)(HHH, "")
                            AddNewHost(H)
                            'MsgBox("adding " & HHH & vbNewLine & "Added " & H.Key,, "2")
                        End If
                    End If
                Next
                'combine unique entries
                FinishAddNewHosts()
                MsgBox("new host count: " & Hosts.Count)
                SaveHosts()
                ListHosts()
            Catch ex As Exception
                MsgBox(ex.Message)
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
            StatusToolStripStatusLabel.Text = String.Format("checking {0} of {1} - {2}", count, SelectedHosts.Count, Host)
            Dim lvi As ListViewItem = GetListViewItemForHost(Host)
            lvi.ImageKey = "wait"

            Dim SSH As New XGShellConnection
            Dim pass As String = Host.Value
            If pass = "" Then pass = ShellPassTextBox.Text

            Dim result As String = SSH.InstallHotfixes(Host.Key, "admin", pass, loglevel)

            lvi.SubItems.Add(result)
            lvi.SubItems.Add(Now.ToString("yyyy-MM-dd h:mm:ss tt"))
            If result.Contains("200") Then
                If result.Contains("remains") Then
                    lvi.ImageKey = "gray check"
                Else
                    lvi.ImageKey = "green check"
                End If
                lvi.Checked = False
                XGShellConnection.WriteToLog(XGShellConnection.LogSeverity.Informational, result, loglevel)
            Else
                lvi.ImageKey = "fail"
                lvi.Checked = True
                XGShellConnection.WriteToLog(XGShellConnection.LogSeverity.Error, result, loglevel)
            End If

            ResultsListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent) 'And ColumnHeaderAutoResizeStyle.HeaderSize)
        Next
        'finished. update status and clean up
        StatusToolStripStatusLabel.Text = "Finished"
        ToolStripProgressBar.Visible = False
        GoButton.Enabled = True
    End Sub

    Private Sub DoAsnarökCheck(loglevel As XGShellConnection.LogSeverity)

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
            StatusToolStripStatusLabel.Text = String.Format("checking {0} of {1} - {2}", count, SelectedHosts.Count, Host)
            Dim lvi As ListViewItem = GetListViewItemForHost(Host)
            lvi.ImageKey = "wait"

            Dim SSH As New XGShellConnection
            Dim pass As String = Host.Value
            If pass = "" Then pass = ShellPassTextBox.Text

            Dim result As String = SSH.CheckSQLiActivity(Host.Key, "admin", pass, loglevel)
            lvi.SubItems.Add(result)
            lvi.SubItems.Add(Now.ToString("yyyy-MM-dd h:mm:ss tt"))

            Select Case result
                Case "Hotfix applied: YES Affected: NO"
                    lvi.ImageKey = "green check"
                    XGShellConnection.WriteToLog(XGShellConnection.LogSeverity.Informational, result, loglevel)
                Case "Hotfix applied: YES Affected: YES"
                    lvi.ImageKey = "fail"
                    lvi.BackColor = Color.Red
                    lvi.ForeColor = Color.White
                    XGShellConnection.WriteToLog(XGShellConnection.LogSeverity.Critical, result, loglevel)
                Case "Hotfix applied: NO Affected: UNKNOWN"
                    lvi.ImageKey = "fail"
                    lvi.BackColor = Color.Red
                    lvi.ForeColor = Color.White
                    XGShellConnection.WriteToLog(XGShellConnection.LogSeverity.Critical, result, loglevel)
                Case Else
                    lvi.ImageKey = "fail"
                    lvi.BackColor = Color.Red
                    lvi.ForeColor = Color.White
                    XGShellConnection.WriteToLog(XGShellConnection.LogSeverity.Error, result, loglevel)
            End Select

            ResultsListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)

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
            StatusToolStripStatusLabel.Text = String.Format("checking {0} of {1} - {2}", count, SelectedHosts.Count, Host)
            Dim lvi As ListViewItem = GetListViewItemForHost(Host)
            lvi.ImageKey = "wait"

            Dim SSH As New XGShellConnection
            Dim pass As String = Host.Value
            If pass = "" Then pass = ShellPassTextBox.Text

            Dim result As String = SSH.CheckCurrentFirmwareVersion(Host.Key, "admin", pass, loglevel)

            lvi.SubItems.Add(result)
            lvi.SubItems.Add(Now.ToString("yyyy-MM-dd h:mm:ss tt"))
            If result.Contains("NO HOTFIX") Then
                lvi.ImageKey = "fail"
                lvi.Checked = True
                XGShellConnection.WriteToLog(XGShellConnection.LogSeverity.Informational, result, loglevel)
            Else
                lvi.ImageKey = ""
                lvi.Checked = False
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
            If pass = "" Then pass = ShellPassTextBox.Text
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
            StatusToolStripStatusLabel.Text = String.Format("checking {0} of {1} - {2}", count, SelectedHosts.Count, Host)
            Dim lvi As ListViewItem = GetListViewItemForHost(Host)
            lvi.ImageKey = "wait"

            Dim SSH As New XGShellConnection
            Dim pass As String = Host.Value
            If pass = "" Then pass = ShellPassTextBox.Text

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

#End Region

#Region "Private Methods"

    Private Sub EnableDisable()
        Dim En = True

        If Hosts.Count = 0 Then En = False
        GoButton.Enabled = En
        DeleteButton.Enabled = ResultsListView.CheckedItems.Count > 0
    End Sub

    Private Sub SaveSettings()
        Dim key As Microsoft.Win32.RegistryKey

        key = My.Computer.Registry.CurrentUser.CreateSubKey("Software\Sophos\XGManagementMigration")
        key.SetValue("LastEmail", CentralUser)
        key.SetValue("LastEmailPass", ENC.EncryptData(CentralPass))
        key.SetValue("CommonShell", ENC.EncryptData(ShellPassTextBox.Text))

    End Sub

    Private Sub SaveHosts()
        Dim key As Microsoft.Win32.RegistryKey
        Try
            My.Computer.Registry.CurrentUser.DeleteSubKeyTree("Software\Sophos\XGManagementMigration\Hosts")
        Catch ex As Exception

        End Try

        key = My.Computer.Registry.CurrentUser.CreateSubKey("Software\Sophos\XGManagementMigration\Hosts")

        For Each host As KeyValuePair(Of String, String) In Hosts
            key.SetValue(host.Key, ENC.EncryptData(host.Value))
        Next
    End Sub

    Private Sub AddHost(Optional defaulthost As String = "", Optional defaultpass As String = "")
        Dim sshhost As String
        Dim sshpass As String
        Dim af As New AddFirewalls
        Dim another As Boolean = False
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

                Case DialogResult.Cancel
                    Exit Do

                Case Else
                    Exit Do

            End Select

            FinishAddNewHosts()
            another = af.AddAnother

        Loop While another

        EnableDisable()
        SaveHosts()
        ListHosts()
    End Sub

    Private Function GetHostByName(Host As String) As KeyValuePair(Of String, String)
        For Each thishost As KeyValuePair(Of String, String) In Hosts
            If thishost.Key.Equals(Host) Then Return thishost
        Next
        Return Nothing
    End Function

    Dim newhosts() As KeyValuePair(Of String, String) = {}
    Private Sub AddNewHost(newhost As KeyValuePair(Of String, String))
        ReDim Preserve newhosts(newhosts.Count)
        newhosts(newhosts.GetUpperBound(0)) = newhost
    End Sub
    Private Sub FinishAddNewHosts()
        If newhosts.Count = 0 Then Exit Sub

        Try

            'Update entry if duplicate host
            For Each thishost As KeyValuePair(Of String, String) In Hosts
                If Not newhosts.Contains(thishost, New KeyComparer) Then
                    ReDim Preserve newhosts(newhosts.Count)
                    newhosts(newhosts.GetUpperBound(0)) = thishost
                Else
                    'skip it
                End If
            Next
            Hosts = newhosts
            newhosts = {}
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub ListHosts()
        ResultsListView.Items.Clear()

        For Each host As KeyValuePair(Of String, String) In Hosts
            Dim lvi As ListViewItem = ResultsListView.Items.Add(host.Key)
            lvi.SubItems.Add(If(host.Value = "", "*", "Set"))
            lvi.ImageKey = ""
        Next
        ResultsListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent And ColumnHeaderAutoResizeStyle.HeaderSize)



        ResultsListView.Sorting = SortOrder.Ascending
        'Set the ListviewItemSorter property to a new ListviewItemComparer object
        Me.ResultsListView.ListViewItemSorter = New ListViewItemComparer(0, ResultsListView.Sorting)

        ' Call the sort method to manually sort
        ResultsListView.Sort()



    End Sub

    Private Sub SetCentralCreds()

        Dim cu As String = ""
        Dim cp As String = ""
        Dim l As New CentralLogin
        l.CentralUser = CentralUser
        l.CentralPass = CentralPass

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

#End Region

End Class

Public Class KeyComparer
    Implements IEqualityComparer(Of KeyValuePair(Of String, String))

    Public Function Equals(x As KeyValuePair(Of String, String), y As KeyValuePair(Of String, String)) As Boolean Implements IEqualityComparer(Of KeyValuePair(Of String, String)).Equals
        Return x.Key.Equals(y.Key)
    End Function

    Public Function GetHashCode(obj As KeyValuePair(Of String, String)) As Integer Implements IEqualityComparer(Of KeyValuePair(Of String, String)).GetHashCode
        Return obj.GetHashCode
    End Function
End Class

Public Class ListViewItemComparer

    Implements IComparer

    Private col As Integer
    Private order As SortOrder

    Public Sub New()
        col = 0
        order = SortOrder.Ascending
    End Sub

    Public Sub New(column As Integer, order As SortOrder)
        col = column
        Me.order = order
    End Sub

    Public Function Compare(x As Object, y As Object) As Integer Implements System.Collections.IComparer.Compare

        Dim returnVal As Integer = -1

        Try

            ' Attempt to parse the two objects as DateTime
            Dim firstDate As System.DateTime = DateTime.Parse(CType(x, ListViewItem).SubItems(col).Text)
            Dim secondDate As System.DateTime = DateTime.Parse(CType(y, ListViewItem).SubItems(col).Text)

            ' Compare as date
            returnVal = DateTime.Compare(firstDate, secondDate)

        Catch ex As Exception

            ' If date parse failed then fall here to determine if objects are numeric
            If IsNumeric(CType(x, ListViewItem).SubItems(col).Text) And
                IsNumeric(CType(y, ListViewItem).SubItems(col).Text) Then

                ' Compare as numeric
                returnVal = Val(CType(x, ListViewItem).SubItems(col).Text).CompareTo(Val(CType(y, ListViewItem).SubItems(col).Text))

            Else
                ' If not numeric then compare as string
                returnVal = [String].Compare(CType(x, ListViewItem).SubItems(col).Text, CType(y, ListViewItem).SubItems(col).Text)
            End If

        End Try

        ' If order is descending then invert value
        If order = SortOrder.Descending Then
            returnVal *= -1
        End If

        Return returnVal

    End Function

End Class