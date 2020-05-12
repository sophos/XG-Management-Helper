<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Login
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
        Me.StrengthLabel = New System.Windows.Forms.Label()
        Me.StrengthProgressBar = New System.Windows.Forms.ProgressBar()
        Me.StatusLabel = New System.Windows.Forms.Label()
        Me.NewPasswordShowButton = New System.Windows.Forms.Button()
        Me.PasswordShowButton = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.ConfirmNewPasswordTextBox = New System.Windows.Forms.TextBox()
        Me.NewPasswordTextBox = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.SetPasswordPanel = New System.Windows.Forms.Panel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.SetPasswordButton = New System.Windows.Forms.Button()
        Me.LoginPanel = New System.Windows.Forms.Panel()
        Me.ErrorLabel = New System.Windows.Forms.Label()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.LoginButton = New System.Windows.Forms.Button()
        Me.LoginTextBox = New System.Windows.Forms.TextBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.MyTitleBar1 = New XGManagementHelper.MyTitleBar()
        Me.SetPasswordPanel.SuspendLayout()
        Me.LoginPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'StrengthLabel
        '
        Me.StrengthLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.StrengthLabel.AutoSize = True
        Me.StrengthLabel.Font = New System.Drawing.Font("Arial", 6.0!)
        Me.StrengthLabel.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.StrengthLabel.Location = New System.Drawing.Point(226, 71)
        Me.StrengthLabel.Name = "StrengthLabel"
        Me.StrengthLabel.Size = New System.Drawing.Size(50, 10)
        Me.StrengthLabel.TabIndex = 39
        Me.StrengthLabel.Text = "VERY WEAK"
        Me.StrengthLabel.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'StrengthProgressBar
        '
        Me.StrengthProgressBar.BackColor = System.Drawing.SystemColors.Control
        Me.StrengthProgressBar.ForeColor = System.Drawing.Color.Red
        Me.StrengthProgressBar.Location = New System.Drawing.Point(35, 109)
        Me.StrengthProgressBar.Name = "StrengthProgressBar"
        Me.StrengthProgressBar.Size = New System.Drawing.Size(220, 10)
        Me.StrengthProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.StrengthProgressBar.TabIndex = 38
        '
        'StatusLabel
        '
        Me.StatusLabel.AutoSize = True
        Me.StatusLabel.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.StatusLabel.Location = New System.Drawing.Point(32, 180)
        Me.StatusLabel.Name = "StatusLabel"
        Me.StatusLabel.Size = New System.Drawing.Size(216, 13)
        Me.StatusLabel.TabIndex = 37
        Me.StatusLabel.Text = "ENTER NEW PASSWORD TO CONTINUE"
        '
        'NewPasswordShowButton
        '
        Me.NewPasswordShowButton.Enabled = False
        Me.NewPasswordShowButton.FlatAppearance.BorderSize = 0
        Me.NewPasswordShowButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.NewPasswordShowButton.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.NewPasswordShowButton.ForeColor = System.Drawing.Color.SteelBlue
        Me.NewPasswordShowButton.Location = New System.Drawing.Point(261, 84)
        Me.NewPasswordShowButton.Name = "NewPasswordShowButton"
        Me.NewPasswordShowButton.Size = New System.Drawing.Size(62, 23)
        Me.NewPasswordShowButton.TabIndex = 36
        Me.NewPasswordShowButton.Text = "Show"
        Me.NewPasswordShowButton.UseVisualStyleBackColor = True
        '
        'PasswordShowButton
        '
        Me.PasswordShowButton.Enabled = False
        Me.PasswordShowButton.FlatAppearance.BorderSize = 0
        Me.PasswordShowButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.PasswordShowButton.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.PasswordShowButton.ForeColor = System.Drawing.Color.SteelBlue
        Me.PasswordShowButton.Location = New System.Drawing.Point(259, 140)
        Me.PasswordShowButton.Name = "PasswordShowButton"
        Me.PasswordShowButton.Size = New System.Drawing.Size(62, 23)
        Me.PasswordShowButton.TabIndex = 35
        Me.PasswordShowButton.Text = "Show"
        Me.PasswordShowButton.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label3.Location = New System.Drawing.Point(32, 67)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(99, 13)
        Me.Label3.TabIndex = 33
        Me.Label3.Text = "NEW PASSWORD"
        '
        'ConfirmNewPasswordTextBox
        '
        Me.ConfirmNewPasswordTextBox.Enabled = False
        Me.ConfirmNewPasswordTextBox.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.ConfirmNewPasswordTextBox.Location = New System.Drawing.Point(35, 139)
        Me.ConfirmNewPasswordTextBox.Name = "ConfirmNewPasswordTextBox"
        Me.ConfirmNewPasswordTextBox.PasswordChar = Global.Microsoft.VisualBasic.ChrW(88)
        Me.ConfirmNewPasswordTextBox.Size = New System.Drawing.Size(220, 26)
        Me.ConfirmNewPasswordTextBox.TabIndex = 32
        Me.ConfirmNewPasswordTextBox.UseSystemPasswordChar = True
        '
        'NewPasswordTextBox
        '
        Me.NewPasswordTextBox.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.NewPasswordTextBox.Location = New System.Drawing.Point(35, 83)
        Me.NewPasswordTextBox.Name = "NewPasswordTextBox"
        Me.NewPasswordTextBox.Size = New System.Drawing.Size(220, 26)
        Me.NewPasswordTextBox.TabIndex = 31
        Me.NewPasswordTextBox.UseSystemPasswordChar = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label4.Location = New System.Drawing.Point(32, 123)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(157, 13)
        Me.Label4.TabIndex = 34
        Me.Label4.Text = "RE-ENTER NEW PASSWORD"
        '
        'SetPasswordPanel
        '
        Me.SetPasswordPanel.Controls.Add(Me.Label2)
        Me.SetPasswordPanel.Controls.Add(Me.SetPasswordButton)
        Me.SetPasswordPanel.Controls.Add(Me.Label3)
        Me.SetPasswordPanel.Controls.Add(Me.StrengthLabel)
        Me.SetPasswordPanel.Controls.Add(Me.Label4)
        Me.SetPasswordPanel.Controls.Add(Me.StrengthProgressBar)
        Me.SetPasswordPanel.Controls.Add(Me.NewPasswordTextBox)
        Me.SetPasswordPanel.Controls.Add(Me.StatusLabel)
        Me.SetPasswordPanel.Controls.Add(Me.ConfirmNewPasswordTextBox)
        Me.SetPasswordPanel.Controls.Add(Me.NewPasswordShowButton)
        Me.SetPasswordPanel.Controls.Add(Me.PasswordShowButton)
        Me.SetPasswordPanel.Dock = System.Windows.Forms.DockStyle.Top
        Me.SetPasswordPanel.Location = New System.Drawing.Point(2, 52)
        Me.SetPasswordPanel.Name = "SetPasswordPanel"
        Me.SetPasswordPanel.Size = New System.Drawing.Size(374, 260)
        Me.SetPasswordPanel.TabIndex = 40
        '
        'Label2
        '
        Me.Label2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label2.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.Label2.ForeColor = System.Drawing.Color.Red
        Me.Label2.Location = New System.Drawing.Point(0, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(374, 56)
        Me.Label2.TabIndex = 44
        Me.Label2.Text = "This application stores sensitive password information. Please choose a strong pa" &
    "ssword to protect it."
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'SetPasswordButton
        '
        Me.SetPasswordButton.BackColor = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(135, Byte), Integer), CType(CType(203, Byte), Integer))
        Me.SetPasswordButton.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.SetPasswordButton.Enabled = False
        Me.SetPasswordButton.FlatAppearance.BorderColor = System.Drawing.Color.SteelBlue
        Me.SetPasswordButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.SetPasswordButton.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.SetPasswordButton.ForeColor = System.Drawing.Color.White
        Me.SetPasswordButton.Location = New System.Drawing.Point(35, 210)
        Me.SetPasswordButton.Name = "SetPasswordButton"
        Me.SetPasswordButton.Size = New System.Drawing.Size(220, 31)
        Me.SetPasswordButton.TabIndex = 43
        Me.SetPasswordButton.Text = "Set Password"
        Me.SetPasswordButton.UseVisualStyleBackColor = False
        '
        'LoginPanel
        '
        Me.LoginPanel.Controls.Add(Me.ErrorLabel)
        Me.LoginPanel.Controls.Add(Me.Button2)
        Me.LoginPanel.Controls.Add(Me.Label1)
        Me.LoginPanel.Controls.Add(Me.LoginButton)
        Me.LoginPanel.Controls.Add(Me.LoginTextBox)
        Me.LoginPanel.Controls.Add(Me.Button1)
        Me.LoginPanel.Location = New System.Drawing.Point(5, 318)
        Me.LoginPanel.Name = "LoginPanel"
        Me.LoginPanel.Size = New System.Drawing.Size(353, 222)
        Me.LoginPanel.TabIndex = 41
        Me.LoginPanel.Visible = False
        '
        'ErrorLabel
        '
        Me.ErrorLabel.AutoSize = True
        Me.ErrorLabel.ForeColor = System.Drawing.Color.Red
        Me.ErrorLabel.Location = New System.Drawing.Point(30, 99)
        Me.ErrorLabel.Name = "ErrorLabel"
        Me.ErrorLabel.Size = New System.Drawing.Size(80, 13)
        Me.ErrorLabel.TabIndex = 45
        Me.ErrorLabel.Text = "LOGIN FAILED"
        Me.ErrorLabel.Visible = False
        '
        'Button2
        '
        Me.Button2.FlatAppearance.BorderSize = 0
        Me.Button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button2.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.Button2.ForeColor = System.Drawing.Color.SteelBlue
        Me.Button2.Location = New System.Drawing.Point(33, 192)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(220, 27)
        Me.Button2.TabIndex = 44
        Me.Button2.Text = "Forgot Password"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label1.Location = New System.Drawing.Point(30, 53)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(183, 13)
        Me.Label1.TabIndex = 43
        Me.Label1.Text = "ENTER APPLICATION PASSWORD"
        '
        'LoginButton
        '
        Me.LoginButton.BackColor = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(135, Byte), Integer), CType(CType(203, Byte), Integer))
        Me.LoginButton.FlatAppearance.BorderColor = System.Drawing.Color.SteelBlue
        Me.LoginButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.LoginButton.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.LoginButton.ForeColor = System.Drawing.Color.White
        Me.LoginButton.Location = New System.Drawing.Point(33, 141)
        Me.LoginButton.Name = "LoginButton"
        Me.LoginButton.Size = New System.Drawing.Size(220, 31)
        Me.LoginButton.TabIndex = 42
        Me.LoginButton.Text = "Login"
        Me.LoginButton.UseVisualStyleBackColor = False
        '
        'LoginTextBox
        '
        Me.LoginTextBox.AcceptsReturn = True
        Me.LoginTextBox.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.LoginTextBox.Location = New System.Drawing.Point(33, 70)
        Me.LoginTextBox.Name = "LoginTextBox"
        Me.LoginTextBox.PasswordChar = Global.Microsoft.VisualBasic.ChrW(88)
        Me.LoginTextBox.Size = New System.Drawing.Size(220, 26)
        Me.LoginTextBox.TabIndex = 36
        Me.LoginTextBox.UseSystemPasswordChar = True
        '
        'Button1
        '
        Me.Button1.FlatAppearance.BorderSize = 0
        Me.Button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button1.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.Button1.ForeColor = System.Drawing.Color.SteelBlue
        Me.Button1.Location = New System.Drawing.Point(257, 71)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(62, 23)
        Me.Button1.TabIndex = 37
        Me.Button1.Text = "Show"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'MyTitleBar1
        '
        Me.MyTitleBar1.BackColor = System.Drawing.Color.FromArgb(CType(CType(21, Byte), Integer), CType(CType(47, Byte), Integer), CType(CType(60, Byte), Integer))
        Me.MyTitleBar1.Dock = System.Windows.Forms.DockStyle.Top
        Me.MyTitleBar1.Location = New System.Drawing.Point(2, 2)
        Me.MyTitleBar1.MaximizeBox = False
        Me.MyTitleBar1.MinimizeBox = False
        Me.MyTitleBar1.Name = "MyTitleBar1"
        Me.MyTitleBar1.Size = New System.Drawing.Size(374, 50)
        Me.MyTitleBar1.TabIndex = 42
        '
        'Login
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(378, 559)
        Me.ControlBox = False
        Me.Controls.Add(Me.LoginPanel)
        Me.Controls.Add(Me.SetPasswordPanel)
        Me.Controls.Add(Me.MyTitleBar1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Login"
        Me.Padding = New System.Windows.Forms.Padding(2)
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Login"
        Me.SetPasswordPanel.ResumeLayout(False)
        Me.SetPasswordPanel.PerformLayout()
        Me.LoginPanel.ResumeLayout(False)
        Me.LoginPanel.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents StrengthLabel As Label
    Friend WithEvents StrengthProgressBar As ProgressBar
    Friend WithEvents StatusLabel As Label
    Friend WithEvents NewPasswordShowButton As Button
    Friend WithEvents PasswordShowButton As Button
    Friend WithEvents Label3 As Label
    Friend WithEvents ConfirmNewPasswordTextBox As TextBox
    Friend WithEvents NewPasswordTextBox As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents SetPasswordPanel As Panel
    Friend WithEvents LoginPanel As Panel
    Friend WithEvents LoginTextBox As TextBox
    Friend WithEvents Button1 As Button
    Friend WithEvents SetPasswordButton As Button
    Friend WithEvents LoginButton As Button
    Friend WithEvents MyTitleBar1 As MyTitleBar
    Friend WithEvents Label1 As Label
    Friend WithEvents Button2 As Button
    Friend WithEvents ErrorLabel As Label
    Friend WithEvents Label2 As Label
End Class
