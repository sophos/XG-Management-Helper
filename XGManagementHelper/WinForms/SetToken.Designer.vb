<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SetToken
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.MyTitleBar1 = New XGManagementHelper.MyTitleBar()
        Me.DialogCancelButton = New System.Windows.Forms.Button()
        Me.DialogOKButton = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.NewPasswordTextBox = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.MessageTextBox = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.CopyMessageButton = New System.Windows.Forms.Button()
        Me.MakeTokenButton = New System.Windows.Forms.Button()
        Me.StrengthProgressBar = New System.Windows.Forms.ProgressBar()
        Me.StrengthLabel = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'MyTitleBar1
        '
        Me.MyTitleBar1.BackColor = System.Drawing.Color.FromArgb(CType(CType(21, Byte), Integer), CType(CType(47, Byte), Integer), CType(CType(60, Byte), Integer))
        Me.MyTitleBar1.ControlBox = False
        Me.MyTitleBar1.Dock = System.Windows.Forms.DockStyle.Top
        Me.MyTitleBar1.Location = New System.Drawing.Point(2, 2)
        Me.MyTitleBar1.MaximizeBox = False
        Me.MyTitleBar1.MinimizeBox = False
        Me.MyTitleBar1.Name = "MyTitleBar1"
        Me.MyTitleBar1.Size = New System.Drawing.Size(599, 50)
        Me.MyTitleBar1.TabIndex = 0
        '
        'DialogCancelButton
        '
        Me.DialogCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.DialogCancelButton.FlatAppearance.BorderSize = 0
        Me.DialogCancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.DialogCancelButton.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Underline)
        Me.DialogCancelButton.ForeColor = System.Drawing.Color.SteelBlue
        Me.DialogCancelButton.Location = New System.Drawing.Point(176, 398)
        Me.DialogCancelButton.Name = "DialogCancelButton"
        Me.DialogCancelButton.Size = New System.Drawing.Size(105, 31)
        Me.DialogCancelButton.TabIndex = 24
        Me.DialogCancelButton.Text = "Cancel"
        Me.DialogCancelButton.UseVisualStyleBackColor = True
        '
        'DialogOKButton
        '
        Me.DialogOKButton.BackColor = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(135, Byte), Integer), CType(CType(203, Byte), Integer))
        Me.DialogOKButton.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.DialogOKButton.Enabled = False
        Me.DialogOKButton.FlatAppearance.BorderColor = System.Drawing.Color.SteelBlue
        Me.DialogOKButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.DialogOKButton.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.DialogOKButton.ForeColor = System.Drawing.Color.White
        Me.DialogOKButton.Location = New System.Drawing.Point(291, 398)
        Me.DialogOKButton.Name = "DialogOKButton"
        Me.DialogOKButton.Size = New System.Drawing.Size(105, 31)
        Me.DialogOKButton.TabIndex = 23
        Me.DialogOKButton.Text = "Continue"
        Me.DialogOKButton.UseVisualStyleBackColor = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.Label3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label3.Location = New System.Drawing.Point(56, 188)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(142, 18)
        Me.Label3.TabIndex = 22
        Me.Label3.Text = "NEW PASSWORD"
        '
        'NewPasswordTextBox
        '
        Me.NewPasswordTextBox.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.NewPasswordTextBox.Location = New System.Drawing.Point(423, 185)
        Me.NewPasswordTextBox.Name = "NewPasswordTextBox"
        Me.NewPasswordTextBox.Size = New System.Drawing.Size(130, 26)
        Me.NewPasswordTextBox.TabIndex = 21
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label1.Location = New System.Drawing.Point(208, 188)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(199, 18)
        Me.Label1.TabIndex = 25
        Me.Label1.Text = "<CURRENT PASSWORD>"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.Label2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label2.Location = New System.Drawing.Point(403, 188)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(17, 18)
        Me.Label2.TabIndex = 26
        Me.Label2.Text = "+"
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Arial", 10.0!)
        Me.Label5.ForeColor = System.Drawing.Color.DarkRed
        Me.Label5.Location = New System.Drawing.Point(56, 65)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(497, 40)
        Me.Label5.TabIndex = 29
        Me.Label5.Text = "This action will modify local user accounts that have not changed their passwword" &
    " since April 25, 2020, 22:00 UTC. " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label4.Location = New System.Drawing.Point(420, 169)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(82, 13)
        Me.Label4.TabIndex = 30
        Me.Label4.Text = "TOKEN VALUE"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.Label6.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label6.Location = New System.Drawing.Point(195, 188)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(17, 18)
        Me.Label6.TabIndex = 31
        Me.Label6.Text = "="
        '
        'Label7
        '
        Me.Label7.Font = New System.Drawing.Font("Arial", 10.0!)
        Me.Label7.ForeColor = System.Drawing.Color.DarkRed
        Me.Label7.Location = New System.Drawing.Point(56, 219)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(497, 33)
        Me.Label7.TabIndex = 32
        Me.Label7.Text = "Be sure to notify affected users of this change. " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "A template message is provided" &
    " if you wish to use it:"
        '
        'MessageTextBox
        '
        Me.MessageTextBox.Location = New System.Drawing.Point(59, 255)
        Me.MessageTextBox.Multiline = True
        Me.MessageTextBox.Name = "MessageTextBox"
        Me.MessageTextBox.Size = New System.Drawing.Size(494, 125)
        Me.MessageTextBox.TabIndex = 33
        '
        'Label8
        '
        Me.Label8.Font = New System.Drawing.Font("Arial", 10.0!)
        Me.Label8.ForeColor = System.Drawing.Color.Gray
        Me.Label8.Location = New System.Drawing.Point(79, 105)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(474, 40)
        Me.Label8.TabIndex = 34
        Me.Label8.Text = "Users authenticated by Active Directory, RADIUS, LDAP, TACACS+, and eDirectory ac" &
    "counts will not be affected. "
        '
        'Label9
        '
        Me.Label9.Font = New System.Drawing.Font("Arial", 10.0!)
        Me.Label9.ForeColor = System.Drawing.Color.Gray
        Me.Label9.Location = New System.Drawing.Point(79, 142)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(474, 40)
        Me.Label9.TabIndex = 35
        Me.Label9.Text = "Each users password will remain the same, but have a token value appended to the " &
    "end of it. "
        '
        'CopyMessageButton
        '
        Me.CopyMessageButton.FlatAppearance.BorderSize = 0
        Me.CopyMessageButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CopyMessageButton.Font = New System.Drawing.Font("Arial", 8.0!)
        Me.CopyMessageButton.ForeColor = System.Drawing.Color.SteelBlue
        Me.CopyMessageButton.Location = New System.Drawing.Point(518, 231)
        Me.CopyMessageButton.Name = "CopyMessageButton"
        Me.CopyMessageButton.Size = New System.Drawing.Size(43, 21)
        Me.CopyMessageButton.TabIndex = 36
        Me.CopyMessageButton.Text = "Copy"
        Me.CopyMessageButton.UseVisualStyleBackColor = True
        '
        'MakeTokenButton
        '
        Me.MakeTokenButton.FlatAppearance.BorderSize = 0
        Me.MakeTokenButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.MakeTokenButton.Font = New System.Drawing.Font("Arial", 8.0!)
        Me.MakeTokenButton.ForeColor = System.Drawing.Color.SteelBlue
        Me.MakeTokenButton.Location = New System.Drawing.Point(458, 214)
        Me.MakeTokenButton.Name = "MakeTokenButton"
        Me.MakeTokenButton.Size = New System.Drawing.Size(95, 21)
        Me.MakeTokenButton.TabIndex = 37
        Me.MakeTokenButton.Text = "Make one for me"
        Me.MakeTokenButton.UseVisualStyleBackColor = True
        '
        'StrengthProgressBar
        '
        Me.StrengthProgressBar.BackColor = System.Drawing.SystemColors.Control
        Me.StrengthProgressBar.ForeColor = System.Drawing.Color.Red
        Me.StrengthProgressBar.Location = New System.Drawing.Point(423, 205)
        Me.StrengthProgressBar.Name = "StrengthProgressBar"
        Me.StrengthProgressBar.Size = New System.Drawing.Size(130, 10)
        Me.StrengthProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.StrengthProgressBar.TabIndex = 38
        '
        'StrengthLabel
        '
        Me.StrengthLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.StrengthLabel.AutoSize = True
        Me.StrengthLabel.Font = New System.Drawing.Font("Arial", 6.0!)
        Me.StrengthLabel.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.StrengthLabel.Location = New System.Drawing.Point(503, 174)
        Me.StrengthLabel.Name = "StrengthLabel"
        Me.StrengthLabel.Size = New System.Drawing.Size(50, 10)
        Me.StrengthLabel.TabIndex = 39
        Me.StrengthLabel.Text = "VERY WEAK"
        Me.StrengthLabel.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'SetToken
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(603, 450)
        Me.ControlBox = False
        Me.Controls.Add(Me.StrengthLabel)
        Me.Controls.Add(Me.NewPasswordTextBox)
        Me.Controls.Add(Me.StrengthProgressBar)
        Me.Controls.Add(Me.MakeTokenButton)
        Me.Controls.Add(Me.CopyMessageButton)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.MessageTextBox)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.DialogCancelButton)
        Me.Controls.Add(Me.DialogOKButton)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.MyTitleBar1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SetToken"
        Me.Padding = New System.Windows.Forms.Padding(2)
        Me.Text = "Set Password Token"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents MyTitleBar1 As MyTitleBar
    Friend WithEvents DialogCancelButton As Button
    Friend WithEvents DialogOKButton As Button
    Friend WithEvents Label3 As Label
    Friend WithEvents NewPasswordTextBox As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents MessageTextBox As TextBox
    Friend WithEvents Label8 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents CopyMessageButton As Button
    Friend WithEvents MakeTokenButton As Button
    Friend WithEvents StrengthProgressBar As ProgressBar
    Friend WithEvents StrengthLabel As Label
End Class
