<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.ResultsListView = New System.Windows.Forms.ListView()
        Me.HostColumnHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.PassColumnHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ResultColumnHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.TimeColumnHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.StatusToolStripStatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripProgressBar = New System.Windows.Forms.ToolStripProgressBar()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.ShellPassTextBox = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TopPanel = New System.Windows.Forms.Panel()
        Me.AllCheckBox = New System.Windows.Forms.CheckBox()
        Me.ToggleCheckButton = New System.Windows.Forms.Button()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ImportFirewallsListToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.BackupApplicationKeyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FirewallsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddFirewallsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ChangeCentralCredentialsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TrustInitialSSHFingerprintToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DoubleClickOpensWebadminToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenWebAdminAndCopyPasswordToClipboardToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EditHostToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem3 = New System.Windows.Forms.ToolStripSeparator()
        Me.DeleteSelectedToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ViewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PasswordChangeLogsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ActionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SophosCentralToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RegisterEnableAllCentralServicesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RegisterAndToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EnableCentralManagementOnlyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EnableCentralReportingOnlyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem5 = New System.Windows.Forms.ToolStripSeparator()
        Me.DeregisterFromSophosCentralToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem4 = New System.Windows.Forms.ToolStripSeparator()
        Me.OtherToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BulkChangeadminPasswordToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CheckCurrentFirmwareVersionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.InstallAnyAvailableHotfixesevenIfToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem7 = New System.Windows.Forms.ToolStripSeparator()
        Me.CheckStatusToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CkeckStatusToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EnableCAPCHAToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.DisableCAPCHAOnVPNZoneToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LogListView = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.LogsLabel = New System.Windows.Forms.Label()
        Me.Splitter1 = New System.Windows.Forms.Splitter()
        Me.FirewallsRightClickContextMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ActionsForFirewallToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EditFirewallToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenWebAdminCopyPasswordToClipboardToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem6 = New System.Windows.Forms.ToolStripSeparator()
        Me.DeleteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MyTitleBar1 = New XGManagementHelper.MyTitleBar()
        Me.ManditoryPasswordResetToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CheckStatusToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.DisablePopUpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StatusStrip1.SuspendLayout()
        Me.TopPanel.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.FirewallsRightClickContextMenu.SuspendLayout()
        Me.SuspendLayout()
        '
        'ResultsListView
        '
        Me.ResultsListView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.ResultsListView.CheckBoxes = True
        Me.ResultsListView.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.HostColumnHeader, Me.PassColumnHeader, Me.ResultColumnHeader, Me.TimeColumnHeader})
        Me.ResultsListView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ResultsListView.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.ResultsListView.FullRowSelect = True
        Me.ResultsListView.HideSelection = False
        Me.ResultsListView.Location = New System.Drawing.Point(2, 103)
        Me.ResultsListView.Name = "ResultsListView"
        Me.ResultsListView.Size = New System.Drawing.Size(958, 320)
        Me.ResultsListView.SmallImageList = Me.ImageList1
        Me.ResultsListView.TabIndex = 13
        Me.ResultsListView.UseCompatibleStateImageBehavior = False
        Me.ResultsListView.View = System.Windows.Forms.View.Details
        '
        'HostColumnHeader
        '
        Me.HostColumnHeader.Text = "Host"
        Me.HostColumnHeader.Width = 170
        '
        'PassColumnHeader
        '
        Me.PassColumnHeader.Text = "Password"
        Me.PassColumnHeader.Width = 90
        '
        'ResultColumnHeader
        '
        Me.ResultColumnHeader.Text = "Result"
        Me.ResultColumnHeader.Width = 73
        '
        'TimeColumnHeader
        '
        Me.TimeColumnHeader.Text = "Time"
        Me.TimeColumnHeader.Width = 55
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "check")
        Me.ImageList1.Images.SetKeyName(1, "fail")
        Me.ImageList1.Images.SetKeyName(2, "pause")
        Me.ImageList1.Images.SetKeyName(3, "wait")
        Me.ImageList1.Images.SetKeyName(4, "timeout")
        Me.ImageList1.Images.SetKeyName(5, "gray check")
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1, Me.StatusToolStripStatusLabel, Me.ToolStripProgressBar})
        Me.StatusStrip1.Location = New System.Drawing.Point(2, 532)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(958, 22)
        Me.StatusStrip1.TabIndex = 14
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(42, 17)
        Me.ToolStripStatusLabel1.Text = "Status:"
        '
        'StatusToolStripStatusLabel
        '
        Me.StatusToolStripStatusLabel.Name = "StatusToolStripStatusLabel"
        Me.StatusToolStripStatusLabel.Size = New System.Drawing.Size(26, 17)
        Me.StatusToolStripStatusLabel.Text = "Idle"
        '
        'ToolStripProgressBar
        '
        Me.ToolStripProgressBar.Name = "ToolStripProgressBar"
        Me.ToolStripProgressBar.Size = New System.Drawing.Size(100, 16)
        Me.ToolStripProgressBar.Visible = False
        '
        'ShellPassTextBox
        '
        Me.ShellPassTextBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ShellPassTextBox.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.ShellPassTextBox.Location = New System.Drawing.Point(753, 1)
        Me.ShellPassTextBox.Name = "ShellPassTextBox"
        Me.ShellPassTextBox.PasswordChar = Global.Microsoft.VisualBasic.ChrW(88)
        Me.ShellPassTextBox.Size = New System.Drawing.Size(202, 26)
        Me.ShellPassTextBox.TabIndex = 3
        Me.ToolTip1.SetToolTip(Me.ShellPassTextBox, "If no password is supplied for an individual firewall, this password will be atte" &
        "mpted.")
        Me.ShellPassTextBox.UseSystemPasswordChar = True
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.Label2.Location = New System.Drawing.Point(563, 3)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(184, 18)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Common Shell Password"
        '
        'TopPanel
        '
        Me.TopPanel.Controls.Add(Me.AllCheckBox)
        Me.TopPanel.Controls.Add(Me.ToggleCheckButton)
        Me.TopPanel.Controls.Add(Me.Label2)
        Me.TopPanel.Controls.Add(Me.ShellPassTextBox)
        Me.TopPanel.Controls.Add(Me.MenuStrip1)
        Me.TopPanel.Dock = System.Windows.Forms.DockStyle.Top
        Me.TopPanel.Location = New System.Drawing.Point(2, 52)
        Me.TopPanel.Name = "TopPanel"
        Me.TopPanel.Size = New System.Drawing.Size(958, 51)
        Me.TopPanel.TabIndex = 9
        '
        'AllCheckBox
        '
        Me.AllCheckBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.AllCheckBox.AutoSize = True
        Me.AllCheckBox.Font = New System.Drawing.Font("Arial", 8.0!)
        Me.AllCheckBox.ForeColor = System.Drawing.Color.SteelBlue
        Me.AllCheckBox.Location = New System.Drawing.Point(4, 30)
        Me.AllCheckBox.Name = "AllCheckBox"
        Me.AllCheckBox.Size = New System.Drawing.Size(38, 18)
        Me.AllCheckBox.TabIndex = 50
        Me.AllCheckBox.Text = "All"
        Me.AllCheckBox.UseVisualStyleBackColor = True
        '
        'ToggleCheckButton
        '
        Me.ToggleCheckButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ToggleCheckButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ToggleCheckButton.FlatAppearance.BorderSize = 0
        Me.ToggleCheckButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ToggleCheckButton.Font = New System.Drawing.Font("Arial", 8.0!)
        Me.ToggleCheckButton.ForeColor = System.Drawing.Color.SteelBlue
        Me.ToggleCheckButton.Location = New System.Drawing.Point(36, 27)
        Me.ToggleCheckButton.Name = "ToggleCheckButton"
        Me.ToggleCheckButton.Size = New System.Drawing.Size(51, 23)
        Me.ToggleCheckButton.TabIndex = 45
        Me.ToggleCheckButton.Text = "Toggle"
        Me.ToggleCheckButton.UseVisualStyleBackColor = True
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.FirewallsToolStripMenuItem, Me.ViewToolStripMenuItem, Me.ActionToolStripMenuItem, Me.HelpToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(958, 26)
        Me.MenuStrip1.TabIndex = 49
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ImportFirewallsListToolStripMenuItem, Me.ToolStripMenuItem1, Me.BackupApplicationKeyToolStripMenuItem, Me.ToolStripMenuItem2, Me.ExitToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(46, 22)
        Me.FileToolStripMenuItem.Text = "&File"
        '
        'ImportFirewallsListToolStripMenuItem
        '
        Me.ImportFirewallsListToolStripMenuItem.Name = "ImportFirewallsListToolStripMenuItem"
        Me.ImportFirewallsListToolStripMenuItem.Size = New System.Drawing.Size(241, 22)
        Me.ImportFirewallsListToolStripMenuItem.Text = "&Import Firewall List"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(238, 6)
        '
        'BackupApplicationKeyToolStripMenuItem
        '
        Me.BackupApplicationKeyToolStripMenuItem.Name = "BackupApplicationKeyToolStripMenuItem"
        Me.BackupApplicationKeyToolStripMenuItem.Size = New System.Drawing.Size(241, 22)
        Me.BackupApplicationKeyToolStripMenuItem.Text = "&Backup Application Key"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(238, 6)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(241, 22)
        Me.ExitToolStripMenuItem.Text = "E&xit"
        '
        'FirewallsToolStripMenuItem
        '
        Me.FirewallsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddFirewallsToolStripMenuItem, Me.ChangeCentralCredentialsToolStripMenuItem, Me.TrustInitialSSHFingerprintToolStripMenuItem, Me.DoubleClickOpensWebadminToolStripMenuItem, Me.ToolStripMenuItem3, Me.DeleteSelectedToolStripMenuItem})
        Me.FirewallsToolStripMenuItem.Name = "FirewallsToolStripMenuItem"
        Me.FirewallsToolStripMenuItem.Size = New System.Drawing.Size(48, 22)
        Me.FirewallsToolStripMenuItem.Text = "&Edit"
        '
        'AddFirewallsToolStripMenuItem
        '
        Me.AddFirewallsToolStripMenuItem.Name = "AddFirewallsToolStripMenuItem"
        Me.AddFirewallsToolStripMenuItem.Size = New System.Drawing.Size(296, 22)
        Me.AddFirewallsToolStripMenuItem.Text = "&Add Firewall(s)"
        '
        'ChangeCentralCredentialsToolStripMenuItem
        '
        Me.ChangeCentralCredentialsToolStripMenuItem.Name = "ChangeCentralCredentialsToolStripMenuItem"
        Me.ChangeCentralCredentialsToolStripMenuItem.Size = New System.Drawing.Size(296, 22)
        Me.ChangeCentralCredentialsToolStripMenuItem.Text = "Set Central Credentials"
        '
        'TrustInitialSSHFingerprintToolStripMenuItem
        '
        Me.TrustInitialSSHFingerprintToolStripMenuItem.Checked = True
        Me.TrustInitialSSHFingerprintToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.TrustInitialSSHFingerprintToolStripMenuItem.Name = "TrustInitialSSHFingerprintToolStripMenuItem"
        Me.TrustInitialSSHFingerprintToolStripMenuItem.Size = New System.Drawing.Size(296, 22)
        Me.TrustInitialSSHFingerprintToolStripMenuItem.Text = "Trust Initial SSH Fingerprint"
        '
        'DoubleClickOpensWebadminToolStripMenuItem
        '
        Me.DoubleClickOpensWebadminToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OpenWebAdminAndCopyPasswordToClipboardToolStripMenuItem, Me.EditHostToolStripMenuItem})
        Me.DoubleClickOpensWebadminToolStripMenuItem.Name = "DoubleClickOpensWebadminToolStripMenuItem"
        Me.DoubleClickOpensWebadminToolStripMenuItem.Size = New System.Drawing.Size(296, 22)
        Me.DoubleClickOpensWebadminToolStripMenuItem.Text = "Double-Click Opens Webadmin"
        '
        'OpenWebAdminAndCopyPasswordToClipboardToolStripMenuItem
        '
        Me.OpenWebAdminAndCopyPasswordToClipboardToolStripMenuItem.Name = "OpenWebAdminAndCopyPasswordToClipboardToolStripMenuItem"
        Me.OpenWebAdminAndCopyPasswordToClipboardToolStripMenuItem.Size = New System.Drawing.Size(431, 22)
        Me.OpenWebAdminAndCopyPasswordToClipboardToolStripMenuItem.Text = "Open WebAdmin (and copy password to clipboard)"
        '
        'EditHostToolStripMenuItem
        '
        Me.EditHostToolStripMenuItem.Checked = True
        Me.EditHostToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.EditHostToolStripMenuItem.Name = "EditHostToolStripMenuItem"
        Me.EditHostToolStripMenuItem.Size = New System.Drawing.Size(431, 22)
        Me.EditHostToolStripMenuItem.Text = "Edit Firewall Settings"
        '
        'ToolStripMenuItem3
        '
        Me.ToolStripMenuItem3.Name = "ToolStripMenuItem3"
        Me.ToolStripMenuItem3.Size = New System.Drawing.Size(293, 6)
        '
        'DeleteSelectedToolStripMenuItem
        '
        Me.DeleteSelectedToolStripMenuItem.Enabled = False
        Me.DeleteSelectedToolStripMenuItem.Name = "DeleteSelectedToolStripMenuItem"
        Me.DeleteSelectedToolStripMenuItem.Size = New System.Drawing.Size(296, 22)
        Me.DeleteSelectedToolStripMenuItem.Text = "&Delete Selected"
        '
        'ViewToolStripMenuItem
        '
        Me.ViewToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PasswordChangeLogsToolStripMenuItem})
        Me.ViewToolStripMenuItem.Name = "ViewToolStripMenuItem"
        Me.ViewToolStripMenuItem.Size = New System.Drawing.Size(55, 22)
        Me.ViewToolStripMenuItem.Text = "&View"
        '
        'PasswordChangeLogsToolStripMenuItem
        '
        Me.PasswordChangeLogsToolStripMenuItem.Name = "PasswordChangeLogsToolStripMenuItem"
        Me.PasswordChangeLogsToolStripMenuItem.Size = New System.Drawing.Size(244, 22)
        Me.PasswordChangeLogsToolStripMenuItem.Text = "Password Change Logs"
        '
        'ActionToolStripMenuItem
        '
        Me.ActionToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SophosCentralToolStripMenuItem, Me.RegisterEnableAllCentralServicesToolStripMenuItem, Me.RegisterAndToolStripMenuItem, Me.ToolStripMenuItem5, Me.DeregisterFromSophosCentralToolStripMenuItem, Me.ToolStripMenuItem4, Me.OtherToolStripMenuItem, Me.BulkChangeadminPasswordToolStripMenuItem, Me.CheckCurrentFirmwareVersionToolStripMenuItem, Me.InstallAnyAvailableHotfixesevenIfToolStripMenuItem, Me.ToolStripMenuItem7, Me.CheckStatusToolStripMenuItem, Me.ManditoryPasswordResetToolStripMenuItem})
        Me.ActionToolStripMenuItem.Name = "ActionToolStripMenuItem"
        Me.ActionToolStripMenuItem.Size = New System.Drawing.Size(161, 22)
        Me.ActionToolStripMenuItem.Text = "Action (All Firewalls)"
        '
        'SophosCentralToolStripMenuItem
        '
        Me.SophosCentralToolStripMenuItem.Enabled = False
        Me.SophosCentralToolStripMenuItem.Name = "SophosCentralToolStripMenuItem"
        Me.SophosCentralToolStripMenuItem.Size = New System.Drawing.Size(504, 22)
        Me.SophosCentralToolStripMenuItem.Text = "Migrate Management to Sophos Central"
        '
        'RegisterEnableAllCentralServicesToolStripMenuItem
        '
        Me.RegisterEnableAllCentralServicesToolStripMenuItem.Name = "RegisterEnableAllCentralServicesToolStripMenuItem"
        Me.RegisterEnableAllCentralServicesToolStripMenuItem.Size = New System.Drawing.Size(504, 22)
        Me.RegisterEnableAllCentralServicesToolStripMenuItem.Text = "Register to Central && Enable CM + CFR"
        '
        'RegisterAndToolStripMenuItem
        '
        Me.RegisterAndToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.EnableCentralManagementOnlyToolStripMenuItem, Me.EnableCentralReportingOnlyToolStripMenuItem})
        Me.RegisterAndToolStripMenuItem.Name = "RegisterAndToolStripMenuItem"
        Me.RegisterAndToolStripMenuItem.Size = New System.Drawing.Size(504, 22)
        Me.RegisterAndToolStripMenuItem.Text = "Register to Central and..."
        '
        'EnableCentralManagementOnlyToolStripMenuItem
        '
        Me.EnableCentralManagementOnlyToolStripMenuItem.Name = "EnableCentralManagementOnlyToolStripMenuItem"
        Me.EnableCentralManagementOnlyToolStripMenuItem.Size = New System.Drawing.Size(343, 22)
        Me.EnableCentralManagementOnlyToolStripMenuItem.Text = "Enable Central Management Only"
        '
        'EnableCentralReportingOnlyToolStripMenuItem
        '
        Me.EnableCentralReportingOnlyToolStripMenuItem.Name = "EnableCentralReportingOnlyToolStripMenuItem"
        Me.EnableCentralReportingOnlyToolStripMenuItem.Size = New System.Drawing.Size(343, 22)
        Me.EnableCentralReportingOnlyToolStripMenuItem.Text = "Enable Central Firewall Reporting Only"
        '
        'ToolStripMenuItem5
        '
        Me.ToolStripMenuItem5.Name = "ToolStripMenuItem5"
        Me.ToolStripMenuItem5.Size = New System.Drawing.Size(501, 6)
        '
        'DeregisterFromSophosCentralToolStripMenuItem
        '
        Me.DeregisterFromSophosCentralToolStripMenuItem.Name = "DeregisterFromSophosCentralToolStripMenuItem"
        Me.DeregisterFromSophosCentralToolStripMenuItem.Size = New System.Drawing.Size(504, 22)
        Me.DeregisterFromSophosCentralToolStripMenuItem.Text = "De-register from Sophos Central"
        '
        'ToolStripMenuItem4
        '
        Me.ToolStripMenuItem4.Name = "ToolStripMenuItem4"
        Me.ToolStripMenuItem4.Size = New System.Drawing.Size(501, 6)
        '
        'OtherToolStripMenuItem
        '
        Me.OtherToolStripMenuItem.Enabled = False
        Me.OtherToolStripMenuItem.Name = "OtherToolStripMenuItem"
        Me.OtherToolStripMenuItem.Size = New System.Drawing.Size(504, 22)
        Me.OtherToolStripMenuItem.Text = "Other Actions"
        '
        'BulkChangeadminPasswordToolStripMenuItem
        '
        Me.BulkChangeadminPasswordToolStripMenuItem.Name = "BulkChangeadminPasswordToolStripMenuItem"
        Me.BulkChangeadminPasswordToolStripMenuItem.Size = New System.Drawing.Size(504, 22)
        Me.BulkChangeadminPasswordToolStripMenuItem.Text = "Bulk Change ""admin"" Password"
        '
        'CheckCurrentFirmwareVersionToolStripMenuItem
        '
        Me.CheckCurrentFirmwareVersionToolStripMenuItem.Name = "CheckCurrentFirmwareVersionToolStripMenuItem"
        Me.CheckCurrentFirmwareVersionToolStripMenuItem.Size = New System.Drawing.Size(504, 22)
        Me.CheckCurrentFirmwareVersionToolStripMenuItem.Text = "Check Current Firmware Version"
        '
        'InstallAnyAvailableHotfixesevenIfToolStripMenuItem
        '
        Me.InstallAnyAvailableHotfixesevenIfToolStripMenuItem.Name = "InstallAnyAvailableHotfixesevenIfToolStripMenuItem"
        Me.InstallAnyAvailableHotfixesevenIfToolStripMenuItem.Size = New System.Drawing.Size(504, 22)
        Me.InstallAnyAvailableHotfixesevenIfToolStripMenuItem.Text = "Install Any Available Hotfixes (If automatic installation disabled)"
        '
        'ToolStripMenuItem7
        '
        Me.ToolStripMenuItem7.Name = "ToolStripMenuItem7"
        Me.ToolStripMenuItem7.Size = New System.Drawing.Size(501, 6)
        '
        'CheckStatusToolStripMenuItem
        '
        Me.CheckStatusToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CkeckStatusToolStripMenuItem, Me.EnableCAPCHAToolStripMenuItem1, Me.DisableCAPCHAOnVPNZoneToolStripMenuItem})
        Me.CheckStatusToolStripMenuItem.Name = "CheckStatusToolStripMenuItem"
        Me.CheckStatusToolStripMenuItem.Size = New System.Drawing.Size(504, 22)
        Me.CheckStatusToolStripMenuItem.Text = "CAPCHA on VPN Zone"
        '
        'CkeckStatusToolStripMenuItem
        '
        Me.CkeckStatusToolStripMenuItem.Name = "CkeckStatusToolStripMenuItem"
        Me.CkeckStatusToolStripMenuItem.Size = New System.Drawing.Size(298, 22)
        Me.CkeckStatusToolStripMenuItem.Text = "Ckeck Status"
        '
        'EnableCAPCHAToolStripMenuItem1
        '
        Me.EnableCAPCHAToolStripMenuItem1.Name = "EnableCAPCHAToolStripMenuItem1"
        Me.EnableCAPCHAToolStripMenuItem1.Size = New System.Drawing.Size(180, 22)
        Me.EnableCAPCHAToolStripMenuItem1.Text = "Enable"
        '
        'DisableCAPCHAOnVPNZoneToolStripMenuItem
        '
        Me.DisableCAPCHAOnVPNZoneToolStripMenuItem.Name = "DisableCAPCHAOnVPNZoneToolStripMenuItem"
        Me.DisableCAPCHAOnVPNZoneToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.DisableCAPCHAOnVPNZoneToolStripMenuItem.Text = "Disable"
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AboutToolStripMenuItem})
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(52, 22)
        Me.HelpToolStripMenuItem.Text = "Help"
        '
        'AboutToolStripMenuItem
        '
        Me.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        Me.AboutToolStripMenuItem.Size = New System.Drawing.Size(231, 22)
        Me.AboutToolStripMenuItem.Text = "About This Application"
        '
        'LogListView
        '
        Me.LogListView.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader3, Me.ColumnHeader2})
        Me.LogListView.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.LogListView.HideSelection = False
        Me.LogListView.Location = New System.Drawing.Point(2, 436)
        Me.LogListView.Name = "LogListView"
        Me.LogListView.Size = New System.Drawing.Size(958, 96)
        Me.LogListView.SmallImageList = Me.ImageList1
        Me.LogListView.TabIndex = 45
        Me.LogListView.UseCompatibleStateImageBehavior = False
        Me.LogListView.View = System.Windows.Forms.View.Details
        Me.LogListView.Visible = False
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Timestamp"
        Me.ColumnHeader1.Width = 86
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Host"
        Me.ColumnHeader3.Width = 109
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Message"
        Me.ColumnHeader2.Width = 850
        '
        'LogsLabel
        '
        Me.LogsLabel.AutoSize = True
        Me.LogsLabel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.LogsLabel.Location = New System.Drawing.Point(2, 423)
        Me.LogsLabel.Name = "LogsLabel"
        Me.LogsLabel.Size = New System.Drawing.Size(72, 13)
        Me.LogsLabel.TabIndex = 46
        Me.LogsLabel.Text = "Result History"
        Me.LogsLabel.Visible = False
        '
        'Splitter1
        '
        Me.Splitter1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Splitter1.Location = New System.Drawing.Point(2, 420)
        Me.Splitter1.Name = "Splitter1"
        Me.Splitter1.Size = New System.Drawing.Size(958, 3)
        Me.Splitter1.TabIndex = 47
        Me.Splitter1.TabStop = False
        Me.Splitter1.Visible = False
        '
        'FirewallsRightClickContextMenu
        '
        Me.FirewallsRightClickContextMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ActionsForFirewallToolStripMenuItem, Me.EditFirewallToolStripMenuItem, Me.OpenWebAdminCopyPasswordToClipboardToolStripMenuItem, Me.ToolStripMenuItem6, Me.DeleteToolStripMenuItem})
        Me.FirewallsRightClickContextMenu.Name = "ContextMenuStrip1"
        Me.FirewallsRightClickContextMenu.Size = New System.Drawing.Size(326, 98)
        '
        'ActionsForFirewallToolStripMenuItem
        '
        Me.ActionsForFirewallToolStripMenuItem.Enabled = False
        Me.ActionsForFirewallToolStripMenuItem.Name = "ActionsForFirewallToolStripMenuItem"
        Me.ActionsForFirewallToolStripMenuItem.Size = New System.Drawing.Size(325, 22)
        Me.ActionsForFirewallToolStripMenuItem.Text = "Actions for Firewall"
        '
        'EditFirewallToolStripMenuItem
        '
        Me.EditFirewallToolStripMenuItem.Name = "EditFirewallToolStripMenuItem"
        Me.EditFirewallToolStripMenuItem.Size = New System.Drawing.Size(325, 22)
        Me.EditFirewallToolStripMenuItem.Text = "&Edit Firewall"
        '
        'OpenWebAdminCopyPasswordToClipboardToolStripMenuItem
        '
        Me.OpenWebAdminCopyPasswordToClipboardToolStripMenuItem.Name = "OpenWebAdminCopyPasswordToClipboardToolStripMenuItem"
        Me.OpenWebAdminCopyPasswordToClipboardToolStripMenuItem.Size = New System.Drawing.Size(325, 22)
        Me.OpenWebAdminCopyPasswordToClipboardToolStripMenuItem.Text = "&Open WebAdmin (Copy password to clipboard)"
        '
        'ToolStripMenuItem6
        '
        Me.ToolStripMenuItem6.Name = "ToolStripMenuItem6"
        Me.ToolStripMenuItem6.Size = New System.Drawing.Size(322, 6)
        '
        'DeleteToolStripMenuItem
        '
        Me.DeleteToolStripMenuItem.Name = "DeleteToolStripMenuItem"
        Me.DeleteToolStripMenuItem.Size = New System.Drawing.Size(325, 22)
        Me.DeleteToolStripMenuItem.Text = "Delete"
        '
        'MyTitleBar1
        '
        Me.MyTitleBar1.BackColor = System.Drawing.Color.FromArgb(CType(CType(21, Byte), Integer), CType(CType(47, Byte), Integer), CType(CType(60, Byte), Integer))
        Me.MyTitleBar1.Dock = System.Windows.Forms.DockStyle.Top
        Me.MyTitleBar1.Location = New System.Drawing.Point(2, 2)
        Me.MyTitleBar1.MaximizeBox = True
        Me.MyTitleBar1.MinimizeBox = True
        Me.MyTitleBar1.Name = "MyTitleBar1"
        Me.MyTitleBar1.Size = New System.Drawing.Size(958, 50)
        Me.MyTitleBar1.TabIndex = 44
        '
        'ManditoryPasswordResetToolStripMenuItem
        '
        Me.ManditoryPasswordResetToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CheckStatusToolStripMenuItem1, Me.DisablePopUpToolStripMenuItem})
        Me.ManditoryPasswordResetToolStripMenuItem.Name = "ManditoryPasswordResetToolStripMenuItem"
        Me.ManditoryPasswordResetToolStripMenuItem.Size = New System.Drawing.Size(504, 22)
        Me.ManditoryPasswordResetToolStripMenuItem.Text = "Manditory Password Reset"
        '
        'CheckStatusToolStripMenuItem1
        '
        Me.CheckStatusToolStripMenuItem1.Name = "CheckStatusToolStripMenuItem1"
        Me.CheckStatusToolStripMenuItem1.Size = New System.Drawing.Size(188, 22)
        Me.CheckStatusToolStripMenuItem1.Text = "Check Status"
        '
        'DisablePopUpToolStripMenuItem
        '
        Me.DisablePopUpToolStripMenuItem.Name = "DisablePopUpToolStripMenuItem"
        Me.DisablePopUpToolStripMenuItem.Size = New System.Drawing.Size(188, 22)
        Me.DisablePopUpToolStripMenuItem.Text = "Disable Pop-Up"
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(962, 556)
        Me.ControlBox = False
        Me.Controls.Add(Me.Splitter1)
        Me.Controls.Add(Me.ResultsListView)
        Me.Controls.Add(Me.LogsLabel)
        Me.Controls.Add(Me.LogListView)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.TopPanel)
        Me.Controls.Add(Me.MyTitleBar1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(962, 300)
        Me.Name = "MainForm"
        Me.Padding = New System.Windows.Forms.Padding(2)
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "XG Firewall Management Helper"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.TopPanel.ResumeLayout(False)
        Me.TopPanel.PerformLayout()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.FirewallsRightClickContextMenu.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ResultsListView As ListView
    Friend WithEvents ImageList1 As ImageList
    Friend WithEvents TimeColumnHeader As ColumnHeader
    Friend WithEvents HostColumnHeader As ColumnHeader
    Friend WithEvents ResultColumnHeader As ColumnHeader
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents ToolStripStatusLabel1 As ToolStripStatusLabel
    Friend WithEvents StatusToolStripStatusLabel As ToolStripStatusLabel
    Friend WithEvents ToolStripProgressBar As ToolStripProgressBar
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents PassColumnHeader As ColumnHeader
    Friend WithEvents Label2 As Label
    Friend WithEvents ShellPassTextBox As TextBox
    Friend WithEvents TopPanel As Panel
    Friend WithEvents MyTitleBar1 As MyTitleBar
    Friend WithEvents ToggleCheckButton As Button
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents FileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ImportFirewallsListToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ViewToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents BackupApplicationKeyToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItem2 As ToolStripSeparator
    Friend WithEvents ExitToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PasswordChangeLogsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents FirewallsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AddFirewallsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ChangeCentralCredentialsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem3 As ToolStripSeparator
    Friend WithEvents DeleteSelectedToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ActionToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SophosCentralToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RegisterEnableAllCentralServicesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RegisterAndToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents EnableCentralManagementOnlyToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents EnableCentralReportingOnlyToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DeregisterFromSophosCentralToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem4 As ToolStripSeparator
    Friend WithEvents BulkChangeadminPasswordToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OtherToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CheckCurrentFirmwareVersionToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents InstallAnyAvailableHotfixesevenIfToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AllCheckBox As CheckBox
    Friend WithEvents ToolStripMenuItem5 As ToolStripSeparator
    Friend WithEvents LogListView As ListView
    Friend WithEvents ColumnHeader1 As ColumnHeader
    Friend WithEvents ColumnHeader2 As ColumnHeader
    Friend WithEvents LogsLabel As Label
    Friend WithEvents Splitter1 As Splitter
    Friend WithEvents ColumnHeader3 As ColumnHeader
    Friend WithEvents TrustInitialSSHFingerprintToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents HelpToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AboutToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents FirewallsRightClickContextMenu As ContextMenuStrip
    Friend WithEvents EditFirewallToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OpenWebAdminCopyPasswordToClipboardToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DoubleClickOpensWebadminToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OpenWebAdminAndCopyPasswordToClipboardToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents EditHostToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ActionsForFirewallToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem6 As ToolStripSeparator
    Friend WithEvents DeleteToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem7 As ToolStripSeparator
    Friend WithEvents CheckStatusToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CkeckStatusToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents EnableCAPCHAToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents DisableCAPCHAOnVPNZoneToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ManditoryPasswordResetToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CheckStatusToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents DisablePopUpToolStripMenuItem As ToolStripMenuItem
End Class
