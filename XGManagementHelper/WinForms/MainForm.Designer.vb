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
        Me.GoButton = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.AddHostButton = New System.Windows.Forms.Button()
        Me.CentralCredsButton = New System.Windows.Forms.Button()
        Me.ActionComboBox = New System.Windows.Forms.ComboBox()
        Me.TopPanel = New System.Windows.Forms.Panel()
        Me.CheckNoneButton = New System.Windows.Forms.Button()
        Me.CheckAllButton = New System.Windows.Forms.Button()
        Me.ToggleCheckButton = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.DeleteButton = New System.Windows.Forms.Button()
        Me.btnBrowse = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.MyTitleBar1 = New XGManagementHelper.MyTitleBar()
        Me.StatusStrip1.SuspendLayout()
        Me.TopPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'ResultsListView
        '
        Me.ResultsListView.CheckBoxes = True
        Me.ResultsListView.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.HostColumnHeader, Me.PassColumnHeader, Me.ResultColumnHeader, Me.TimeColumnHeader})
        Me.ResultsListView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ResultsListView.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.ResultsListView.FullRowSelect = True
        Me.ResultsListView.HideSelection = False
        Me.ResultsListView.Location = New System.Drawing.Point(2, 127)
        Me.ResultsListView.Name = "ResultsListView"
        Me.ResultsListView.Size = New System.Drawing.Size(958, 405)
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
        Me.ShellPassTextBox.Location = New System.Drawing.Point(626, 43)
        Me.ShellPassTextBox.Name = "ShellPassTextBox"
        Me.ShellPassTextBox.PasswordChar = Global.Microsoft.VisualBasic.ChrW(88)
        Me.ShellPassTextBox.Size = New System.Drawing.Size(91, 26)
        Me.ShellPassTextBox.TabIndex = 3
        Me.ToolTip1.SetToolTip(Me.ShellPassTextBox, "If no password is supplied for an individual firewall, this password will be atte" &
        "mpted.")
        Me.ShellPassTextBox.UseSystemPasswordChar = True
        '
        'GoButton
        '
        Me.GoButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GoButton.BackColor = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(135, Byte), Integer), CType(CType(203, Byte), Integer))
        Me.GoButton.Enabled = False
        Me.GoButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(135, Byte), Integer), CType(CType(203, Byte), Integer))
        Me.GoButton.FlatAppearance.BorderSize = 0
        Me.GoButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.GoButton.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.GoButton.ForeColor = System.Drawing.Color.White
        Me.GoButton.Location = New System.Drawing.Point(907, 12)
        Me.GoButton.Name = "GoButton"
        Me.GoButton.Size = New System.Drawing.Size(48, 26)
        Me.GoButton.TabIndex = 20
        Me.GoButton.Text = "Go"
        Me.GoButton.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.Label2.Location = New System.Drawing.Point(436, 49)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(184, 18)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Common Shell Password"
        '
        'AddHostButton
        '
        Me.AddHostButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.AddHostButton.BackColor = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(135, Byte), Integer), CType(CType(203, Byte), Integer))
        Me.AddHostButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(135, Byte), Integer), CType(CType(203, Byte), Integer))
        Me.AddHostButton.FlatAppearance.BorderSize = 0
        Me.AddHostButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.AddHostButton.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.AddHostButton.ForeColor = System.Drawing.Color.White
        Me.AddHostButton.Location = New System.Drawing.Point(3, 9)
        Me.AddHostButton.Name = "AddHostButton"
        Me.AddHostButton.Size = New System.Drawing.Size(129, 26)
        Me.AddHostButton.TabIndex = 38
        Me.AddHostButton.Text = "Add Firewall(s)"
        Me.AddHostButton.UseVisualStyleBackColor = False
        '
        'CentralCredsButton
        '
        Me.CentralCredsButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CentralCredsButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CentralCredsButton.FlatAppearance.BorderSize = 0
        Me.CentralCredsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CentralCredsButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CentralCredsButton.ForeColor = System.Drawing.Color.SteelBlue
        Me.CentralCredsButton.Location = New System.Drawing.Point(835, 48)
        Me.CentralCredsButton.Name = "CentralCredsButton"
        Me.CentralCredsButton.Size = New System.Drawing.Size(123, 23)
        Me.CentralCredsButton.TabIndex = 39
        Me.CentralCredsButton.Text = "Set Central Credentials"
        Me.CentralCredsButton.UseVisualStyleBackColor = True
        '
        'ActionComboBox
        '
        Me.ActionComboBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ActionComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.ActionComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.ActionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ActionComboBox.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.ActionComboBox.FormattingEnabled = True
        Me.ActionComboBox.Location = New System.Drawing.Point(626, 12)
        Me.ActionComboBox.Name = "ActionComboBox"
        Me.ActionComboBox.Size = New System.Drawing.Size(275, 26)
        Me.ActionComboBox.TabIndex = 40
        '
        'TopPanel
        '
        Me.TopPanel.Controls.Add(Me.CheckNoneButton)
        Me.TopPanel.Controls.Add(Me.CheckAllButton)
        Me.TopPanel.Controls.Add(Me.ToggleCheckButton)
        Me.TopPanel.Controls.Add(Me.Button1)
        Me.TopPanel.Controls.Add(Me.GoButton)
        Me.TopPanel.Controls.Add(Me.Label2)
        Me.TopPanel.Controls.Add(Me.ShellPassTextBox)
        Me.TopPanel.Controls.Add(Me.AddHostButton)
        Me.TopPanel.Controls.Add(Me.CentralCredsButton)
        Me.TopPanel.Controls.Add(Me.ActionComboBox)
        Me.TopPanel.Controls.Add(Me.DeleteButton)
        Me.TopPanel.Controls.Add(Me.btnBrowse)
        Me.TopPanel.Controls.Add(Me.Label3)
        Me.TopPanel.Dock = System.Windows.Forms.DockStyle.Top
        Me.TopPanel.Location = New System.Drawing.Point(2, 52)
        Me.TopPanel.Name = "TopPanel"
        Me.TopPanel.Size = New System.Drawing.Size(958, 75)
        Me.TopPanel.TabIndex = 9
        '
        'CheckNoneButton
        '
        Me.CheckNoneButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CheckNoneButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CheckNoneButton.FlatAppearance.BorderSize = 0
        Me.CheckNoneButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckNoneButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckNoneButton.ForeColor = System.Drawing.Color.SteelBlue
        Me.CheckNoneButton.Location = New System.Drawing.Point(41, 48)
        Me.CheckNoneButton.Name = "CheckNoneButton"
        Me.CheckNoneButton.Size = New System.Drawing.Size(42, 23)
        Me.CheckNoneButton.TabIndex = 47
        Me.CheckNoneButton.Text = "None"
        Me.CheckNoneButton.UseVisualStyleBackColor = True
        '
        'CheckAllButton
        '
        Me.CheckAllButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CheckAllButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CheckAllButton.FlatAppearance.BorderSize = 0
        Me.CheckAllButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckAllButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckAllButton.ForeColor = System.Drawing.Color.SteelBlue
        Me.CheckAllButton.Location = New System.Drawing.Point(3, 48)
        Me.CheckAllButton.Name = "CheckAllButton"
        Me.CheckAllButton.Size = New System.Drawing.Size(32, 23)
        Me.CheckAllButton.TabIndex = 46
        Me.CheckAllButton.Text = "All"
        Me.CheckAllButton.UseVisualStyleBackColor = True
        '
        'ToggleCheckButton
        '
        Me.ToggleCheckButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ToggleCheckButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ToggleCheckButton.FlatAppearance.BorderSize = 0
        Me.ToggleCheckButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ToggleCheckButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ToggleCheckButton.ForeColor = System.Drawing.Color.SteelBlue
        Me.ToggleCheckButton.Location = New System.Drawing.Point(81, 48)
        Me.ToggleCheckButton.Name = "ToggleCheckButton"
        Me.ToggleCheckButton.Size = New System.Drawing.Size(51, 23)
        Me.ToggleCheckButton.TabIndex = 45
        Me.ToggleCheckButton.Text = "Toggle"
        Me.ToggleCheckButton.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button1.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button1.FlatAppearance.BorderSize = 0
        Me.Button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.ForeColor = System.Drawing.Color.SteelBlue
        Me.Button1.Location = New System.Drawing.Point(723, 48)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(117, 23)
        Me.Button1.TabIndex = 44
        Me.Button1.Text = "View Password Logs"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'DeleteButton
        '
        Me.DeleteButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.DeleteButton.BackColor = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(135, Byte), Integer), CType(CType(203, Byte), Integer))
        Me.DeleteButton.Enabled = False
        Me.DeleteButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(135, Byte), Integer), CType(CType(203, Byte), Integer))
        Me.DeleteButton.FlatAppearance.BorderSize = 0
        Me.DeleteButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.DeleteButton.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.DeleteButton.ForeColor = System.Drawing.Color.White
        Me.DeleteButton.Location = New System.Drawing.Point(138, 9)
        Me.DeleteButton.Name = "DeleteButton"
        Me.DeleteButton.Size = New System.Drawing.Size(129, 26)
        Me.DeleteButton.TabIndex = 41
        Me.DeleteButton.Text = "Delete Selected"
        Me.DeleteButton.UseVisualStyleBackColor = False
        '
        'btnBrowse
        '
        Me.btnBrowse.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnBrowse.BackColor = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(135, Byte), Integer), CType(CType(203, Byte), Integer))
        Me.btnBrowse.FlatAppearance.BorderSize = 0
        Me.btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnBrowse.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBrowse.ForeColor = System.Drawing.Color.White
        Me.btnBrowse.Location = New System.Drawing.Point(273, 9)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(129, 26)
        Me.btnBrowse.TabIndex = 42
        Me.btnBrowse.Text = "Import"
        Me.btnBrowse.UseVisualStyleBackColor = False
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.Label3.Location = New System.Drawing.Point(558, 17)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(52, 18)
        Me.Label3.TabIndex = 43
        Me.Label3.Text = "Action"
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
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(962, 556)
        Me.ControlBox = False
        Me.Controls.Add(Me.ResultsListView)
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
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.TopPanel.ResumeLayout(False)
        Me.TopPanel.PerformLayout()
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
    Friend WithEvents GoButton As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents ShellPassTextBox As TextBox
    Friend WithEvents AddHostButton As Button
    Friend WithEvents CentralCredsButton As Button
    Friend WithEvents ActionComboBox As ComboBox
    Friend WithEvents TopPanel As Panel
    Friend WithEvents DeleteButton As Button
    Friend WithEvents btnBrowse As Button
    Friend WithEvents Label3 As Label
    Friend WithEvents MyTitleBar1 As MyTitleBar
    Friend WithEvents Button1 As Button
    Friend WithEvents CheckNoneButton As Button
    Friend WithEvents CheckAllButton As Button
    Friend WithEvents ToggleCheckButton As Button
End Class
