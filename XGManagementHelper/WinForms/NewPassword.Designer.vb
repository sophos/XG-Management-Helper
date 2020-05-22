<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class NewPassword
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
        Me.MyTitleBar1 = New XGManagementHelper.MyTitleBar()
        Me.PasswordShowButton = New System.Windows.Forms.Button()
        Me.DialogCancelButton = New System.Windows.Forms.Button()
        Me.DialogOKButton = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.ConfirmNewPasswordTextBox = New System.Windows.Forms.TextBox()
        Me.NewPasswordTextBox = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.NewPasswordShowButton = New System.Windows.Forms.Button()
        Me.StatusLabel = New System.Windows.Forms.Label()
        Me.StrengthProgressBar = New System.Windows.Forms.ProgressBar()
        Me.GenerateCheckBox = New System.Windows.Forms.CheckBox()
        Me.CheckBoxLabel = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.StrengthLabel = New System.Windows.Forms.Label()
        Me.ComplexityCheckBox = New System.Windows.Forms.CheckBox()
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
        Me.MyTitleBar1.Size = New System.Drawing.Size(645, 50)
        Me.MyTitleBar1.TabIndex = 0
        '
        'PasswordShowButton
        '
        Me.PasswordShowButton.Enabled = False
        Me.PasswordShowButton.FlatAppearance.BorderSize = 0
        Me.PasswordShowButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.PasswordShowButton.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.PasswordShowButton.ForeColor = System.Drawing.Color.SteelBlue
        Me.PasswordShowButton.Location = New System.Drawing.Point(581, 160)
        Me.PasswordShowButton.Name = "PasswordShowButton"
        Me.PasswordShowButton.Size = New System.Drawing.Size(62, 23)
        Me.PasswordShowButton.TabIndex = 21
        Me.PasswordShowButton.Text = "Show"
        Me.PasswordShowButton.UseVisualStyleBackColor = True
        '
        'DialogCancelButton
        '
        Me.DialogCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.DialogCancelButton.FlatAppearance.BorderSize = 0
        Me.DialogCancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.DialogCancelButton.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Underline)
        Me.DialogCancelButton.ForeColor = System.Drawing.Color.SteelBlue
        Me.DialogCancelButton.Location = New System.Drawing.Point(37, 234)
        Me.DialogCancelButton.Name = "DialogCancelButton"
        Me.DialogCancelButton.Size = New System.Drawing.Size(105, 31)
        Me.DialogCancelButton.TabIndex = 20
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
        Me.DialogOKButton.Location = New System.Drawing.Point(152, 234)
        Me.DialogOKButton.Name = "DialogOKButton"
        Me.DialogOKButton.Size = New System.Drawing.Size(105, 31)
        Me.DialogOKButton.TabIndex = 19
        Me.DialogOKButton.Text = "Continue"
        Me.DialogOKButton.UseVisualStyleBackColor = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label3.Location = New System.Drawing.Point(354, 87)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(99, 13)
        Me.Label3.TabIndex = 17
        Me.Label3.Text = "NEW PASSWORD"
        '
        'ConfirmNewPasswordTextBox
        '
        Me.ConfirmNewPasswordTextBox.Enabled = False
        Me.ConfirmNewPasswordTextBox.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.ConfirmNewPasswordTextBox.Location = New System.Drawing.Point(357, 159)
        Me.ConfirmNewPasswordTextBox.Name = "ConfirmNewPasswordTextBox"
        Me.ConfirmNewPasswordTextBox.PasswordChar = Global.Microsoft.VisualBasic.ChrW(88)
        Me.ConfirmNewPasswordTextBox.Size = New System.Drawing.Size(220, 26)
        Me.ConfirmNewPasswordTextBox.TabIndex = 16
        Me.ConfirmNewPasswordTextBox.UseSystemPasswordChar = True
        '
        'NewPasswordTextBox
        '
        Me.NewPasswordTextBox.Enabled = False
        Me.NewPasswordTextBox.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.NewPasswordTextBox.Location = New System.Drawing.Point(357, 103)
        Me.NewPasswordTextBox.Name = "NewPasswordTextBox"
        Me.NewPasswordTextBox.Size = New System.Drawing.Size(220, 26)
        Me.NewPasswordTextBox.TabIndex = 15
        Me.NewPasswordTextBox.UseSystemPasswordChar = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label4.Location = New System.Drawing.Point(354, 143)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(157, 13)
        Me.Label4.TabIndex = 18
        Me.Label4.Text = "RE-ENTER NEW PASSWORD"
        '
        'NewPasswordShowButton
        '
        Me.NewPasswordShowButton.Enabled = False
        Me.NewPasswordShowButton.FlatAppearance.BorderSize = 0
        Me.NewPasswordShowButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.NewPasswordShowButton.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.NewPasswordShowButton.ForeColor = System.Drawing.Color.SteelBlue
        Me.NewPasswordShowButton.Location = New System.Drawing.Point(583, 104)
        Me.NewPasswordShowButton.Name = "NewPasswordShowButton"
        Me.NewPasswordShowButton.Size = New System.Drawing.Size(62, 23)
        Me.NewPasswordShowButton.TabIndex = 22
        Me.NewPasswordShowButton.Text = "Show"
        Me.NewPasswordShowButton.UseVisualStyleBackColor = True
        '
        'StatusLabel
        '
        Me.StatusLabel.AutoSize = True
        Me.StatusLabel.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.StatusLabel.Location = New System.Drawing.Point(354, 200)
        Me.StatusLabel.Name = "StatusLabel"
        Me.StatusLabel.Size = New System.Drawing.Size(216, 13)
        Me.StatusLabel.TabIndex = 23
        Me.StatusLabel.Text = "ENTER NEW PASSWORD TO CONTINUE"
        '
        'StrengthProgressBar
        '
        Me.StrengthProgressBar.BackColor = System.Drawing.SystemColors.Control
        Me.StrengthProgressBar.ForeColor = System.Drawing.Color.Red
        Me.StrengthProgressBar.Location = New System.Drawing.Point(357, 124)
        Me.StrengthProgressBar.Name = "StrengthProgressBar"
        Me.StrengthProgressBar.Size = New System.Drawing.Size(220, 10)
        Me.StrengthProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.StrengthProgressBar.TabIndex = 24
        '
        'GenerateCheckBox
        '
        Me.GenerateCheckBox.AutoSize = True
        Me.GenerateCheckBox.Checked = True
        Me.GenerateCheckBox.CheckState = System.Windows.Forms.CheckState.Checked
        Me.GenerateCheckBox.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GenerateCheckBox.Location = New System.Drawing.Point(26, 87)
        Me.GenerateCheckBox.Name = "GenerateCheckBox"
        Me.GenerateCheckBox.Size = New System.Drawing.Size(15, 14)
        Me.GenerateCheckBox.TabIndex = 25
        Me.GenerateCheckBox.UseVisualStyleBackColor = True
        '
        'CheckBoxLabel
        '
        Me.CheckBoxLabel.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.CheckBoxLabel.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.CheckBoxLabel.Location = New System.Drawing.Point(47, 83)
        Me.CheckBoxLabel.Name = "CheckBoxLabel"
        Me.CheckBoxLabel.Size = New System.Drawing.Size(210, 44)
        Me.CheckBoxLabel.TabIndex = 26
        Me.CheckBoxLabel.Text = "Generate a unique password for each firewall"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.Label2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label2.Location = New System.Drawing.Point(290, 138)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(25, 18)
        Me.Label2.TabIndex = 27
        Me.Label2.Text = "Or"
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Arial", 10.0!)
        Me.Label5.ForeColor = System.Drawing.Color.Gray
        Me.Label5.Location = New System.Drawing.Point(23, 129)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(261, 104)
        Me.Label5.TabIndex = 28
        Me.Label5.Text = "Passwords will be saved for each firewall after it is updated successfully. " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "R" &
    "esults may be exported for your own records."
        '
        'StrengthLabel
        '
        Me.StrengthLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.StrengthLabel.AutoSize = True
        Me.StrengthLabel.Font = New System.Drawing.Font("Arial", 6.0!)
        Me.StrengthLabel.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.StrengthLabel.Location = New System.Drawing.Point(527, 91)
        Me.StrengthLabel.Name = "StrengthLabel"
        Me.StrengthLabel.Size = New System.Drawing.Size(50, 10)
        Me.StrengthLabel.TabIndex = 30
        Me.StrengthLabel.Text = "VERY WEAK"
        Me.StrengthLabel.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'ComplexityCheckBox
        '
        Me.ComplexityCheckBox.AutoSize = True
        Me.ComplexityCheckBox.Checked = True
        Me.ComplexityCheckBox.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ComplexityCheckBox.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.ComplexityCheckBox.Location = New System.Drawing.Point(357, 243)
        Me.ComplexityCheckBox.Name = "ComplexityCheckBox"
        Me.ComplexityCheckBox.Size = New System.Drawing.Size(213, 17)
        Me.ComplexityCheckBox.TabIndex = 31
        Me.ComplexityCheckBox.Text = "Enforce default complexity requirements"
        Me.ComplexityCheckBox.UseVisualStyleBackColor = True
        '
        'NewPassword
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(649, 300)
        Me.ControlBox = False
        Me.Controls.Add(Me.NewPasswordTextBox)
        Me.Controls.Add(Me.ComplexityCheckBox)
        Me.Controls.Add(Me.StrengthLabel)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.CheckBoxLabel)
        Me.Controls.Add(Me.GenerateCheckBox)
        Me.Controls.Add(Me.StrengthProgressBar)
        Me.Controls.Add(Me.StatusLabel)
        Me.Controls.Add(Me.NewPasswordShowButton)
        Me.Controls.Add(Me.PasswordShowButton)
        Me.Controls.Add(Me.DialogCancelButton)
        Me.Controls.Add(Me.DialogOKButton)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.ConfirmNewPasswordTextBox)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.MyTitleBar1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "NewPassword"
        Me.Padding = New System.Windows.Forms.Padding(2)
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "New Admin Password"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents MyTitleBar1 As MyTitleBar
    Friend WithEvents PasswordShowButton As Button
    Friend WithEvents DialogCancelButton As Button
    Friend WithEvents DialogOKButton As Button
    Friend WithEvents Label3 As Label
    Friend WithEvents ConfirmNewPasswordTextBox As TextBox
    Friend WithEvents NewPasswordTextBox As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents NewPasswordShowButton As Button
    Friend WithEvents StatusLabel As Label
    Friend WithEvents StrengthProgressBar As ProgressBar
    Friend WithEvents GenerateCheckBox As CheckBox
    Friend WithEvents CheckBoxLabel As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents StrengthLabel As Label
    Friend WithEvents ComplexityCheckBox As CheckBox
End Class
