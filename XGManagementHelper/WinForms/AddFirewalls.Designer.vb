<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class AddFirewalls
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
        Me.CommonPassCheckBox = New System.Windows.Forms.CheckBox()
        Me.PasswordShowButton = New System.Windows.Forms.Button()
        Me.DialogCancelButton = New System.Windows.Forms.Button()
        Me.DialogOKButton = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.SSHPassTextBox = New System.Windows.Forms.TextBox()
        Me.SSHHostTextBox = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.AddAnotherCheckBox = New System.Windows.Forms.CheckBox()
        Me.MyTitleBar1 = New XGManagementHelper.MyTitleBar()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.FingerprintTextBox = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.WebadminPortTextBox = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'CommonPassCheckBox
        '
        Me.CommonPassCheckBox.AutoSize = True
        Me.CommonPassCheckBox.Checked = True
        Me.CommonPassCheckBox.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CommonPassCheckBox.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.CommonPassCheckBox.Location = New System.Drawing.Point(51, 164)
        Me.CommonPassCheckBox.Name = "CommonPassCheckBox"
        Me.CommonPassCheckBox.Size = New System.Drawing.Size(235, 22)
        Me.CommonPassCheckBox.TabIndex = 3
        Me.CommonPassCheckBox.Text = "Use Common Shell Password"
        Me.CommonPassCheckBox.UseVisualStyleBackColor = True
        '
        'PasswordShowButton
        '
        Me.PasswordShowButton.FlatAppearance.BorderSize = 0
        Me.PasswordShowButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.PasswordShowButton.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.PasswordShowButton.ForeColor = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(135, Byte), Integer), CType(CType(203, Byte), Integer))
        Me.PasswordShowButton.Location = New System.Drawing.Point(340, 133)
        Me.PasswordShowButton.Name = "PasswordShowButton"
        Me.PasswordShowButton.Size = New System.Drawing.Size(56, 23)
        Me.PasswordShowButton.TabIndex = 6
        Me.PasswordShowButton.Text = "Show"
        Me.PasswordShowButton.UseVisualStyleBackColor = True
        '
        'DialogCancelButton
        '
        Me.DialogCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.DialogCancelButton.FlatAppearance.BorderSize = 0
        Me.DialogCancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.DialogCancelButton.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Underline)
        Me.DialogCancelButton.ForeColor = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(135, Byte), Integer), CType(CType(203, Byte), Integer))
        Me.DialogCancelButton.Location = New System.Drawing.Point(51, 349)
        Me.DialogCancelButton.Name = "DialogCancelButton"
        Me.DialogCancelButton.Size = New System.Drawing.Size(78, 33)
        Me.DialogCancelButton.TabIndex = 5
        Me.DialogCancelButton.Text = "Cancel"
        Me.DialogCancelButton.UseVisualStyleBackColor = True
        '
        'DialogOKButton
        '
        Me.DialogOKButton.BackColor = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(135, Byte), Integer), CType(CType(203, Byte), Integer))
        Me.DialogOKButton.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.DialogOKButton.FlatAppearance.BorderColor = System.Drawing.Color.SteelBlue
        Me.DialogOKButton.FlatAppearance.BorderSize = 0
        Me.DialogOKButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.DialogOKButton.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.DialogOKButton.ForeColor = System.Drawing.Color.White
        Me.DialogOKButton.Location = New System.Drawing.Point(229, 349)
        Me.DialogOKButton.Name = "DialogOKButton"
        Me.DialogOKButton.Size = New System.Drawing.Size(105, 33)
        Me.DialogOKButton.TabIndex = 4
        Me.DialogOKButton.Text = "Add"
        Me.DialogOKButton.UseVisualStyleBackColor = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(48, 60)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(144, 13)
        Me.Label3.TabIndex = 46
        Me.Label3.Text = "HOSTNAME / IP ADDRESS"
        '
        'SSHPassTextBox
        '
        Me.SSHPassTextBox.AcceptsReturn = True
        Me.SSHPassTextBox.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.SSHPassTextBox.Location = New System.Drawing.Point(51, 132)
        Me.SSHPassTextBox.Name = "SSHPassTextBox"
        Me.SSHPassTextBox.PasswordChar = Global.Microsoft.VisualBasic.ChrW(88)
        Me.SSHPassTextBox.Size = New System.Drawing.Size(283, 26)
        Me.SSHPassTextBox.TabIndex = 2
        Me.SSHPassTextBox.UseSystemPasswordChar = True
        '
        'SSHHostTextBox
        '
        Me.SSHHostTextBox.AcceptsReturn = True
        Me.SSHHostTextBox.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.SSHHostTextBox.Location = New System.Drawing.Point(51, 76)
        Me.SSHHostTextBox.Name = "SSHHostTextBox"
        Me.SSHHostTextBox.Size = New System.Drawing.Size(283, 26)
        Me.SSHHostTextBox.TabIndex = 1
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(48, 116)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(95, 13)
        Me.Label4.TabIndex = 47
        Me.Label4.Text = "SSH PASSWORD"
        '
        'AddAnotherCheckBox
        '
        Me.AddAnotherCheckBox.AutoSize = True
        Me.AddAnotherCheckBox.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.AddAnotherCheckBox.Location = New System.Drawing.Point(229, 322)
        Me.AddAnotherCheckBox.Name = "AddAnotherCheckBox"
        Me.AddAnotherCheckBox.Size = New System.Drawing.Size(113, 22)
        Me.AddAnotherCheckBox.TabIndex = 52
        Me.AddAnotherCheckBox.Text = "Add Another"
        Me.AddAnotherCheckBox.UseVisualStyleBackColor = True
        '
        'MyTitleBar1
        '
        Me.MyTitleBar1.BackColor = System.Drawing.Color.FromArgb(CType(CType(21, Byte), Integer), CType(CType(47, Byte), Integer), CType(CType(60, Byte), Integer))
        Me.MyTitleBar1.Dock = System.Windows.Forms.DockStyle.Top
        Me.MyTitleBar1.Location = New System.Drawing.Point(2, 2)
        Me.MyTitleBar1.MaximizeBox = True
        Me.MyTitleBar1.MinimizeBox = True
        Me.MyTitleBar1.Name = "MyTitleBar1"
        Me.MyTitleBar1.Size = New System.Drawing.Size(431, 50)
        Me.MyTitleBar1.TabIndex = 53
        '
        'Button1
        '
        Me.Button1.FlatAppearance.BorderSize = 0
        Me.Button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button1.Font = New System.Drawing.Font("Arial", 8.0!)
        Me.Button1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(135, Byte), Integer), CType(CType(203, Byte), Integer))
        Me.Button1.Location = New System.Drawing.Point(139, 110)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(42, 22)
        Me.Button1.TabIndex = 54
        Me.Button1.Text = "Copy"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.FlatAppearance.BorderSize = 0
        Me.Button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button2.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.Button2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(135, Byte), Integer), CType(CType(203, Byte), Integer))
        Me.Button2.Location = New System.Drawing.Point(340, 77)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(60, 23)
        Me.Button2.TabIndex = 55
        Me.Button2.Text = "Test"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'FingerprintTextBox
        '
        Me.FingerprintTextBox.AcceptsReturn = True
        Me.FingerprintTextBox.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.FingerprintTextBox.Location = New System.Drawing.Point(51, 206)
        Me.FingerprintTextBox.Multiline = True
        Me.FingerprintTextBox.Name = "FingerprintTextBox"
        Me.FingerprintTextBox.PasswordChar = Global.Microsoft.VisualBasic.ChrW(88)
        Me.FingerprintTextBox.ReadOnly = True
        Me.FingerprintTextBox.Size = New System.Drawing.Size(283, 49)
        Me.FingerprintTextBox.TabIndex = 56
        Me.FingerprintTextBox.UseSystemPasswordChar = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(48, 190)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(81, 13)
        Me.Label1.TabIndex = 57
        Me.Label1.Text = "SSH Fingerprint"
        '
        'WebadminPortTextBox
        '
        Me.WebadminPortTextBox.AcceptsReturn = True
        Me.WebadminPortTextBox.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.WebadminPortTextBox.Location = New System.Drawing.Point(51, 282)
        Me.WebadminPortTextBox.Name = "WebadminPortTextBox"
        Me.WebadminPortTextBox.Size = New System.Drawing.Size(66, 26)
        Me.WebadminPortTextBox.TabIndex = 58
        Me.WebadminPortTextBox.Text = "4444"
        Me.WebadminPortTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(48, 266)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(81, 13)
        Me.Label2.TabIndex = 59
        Me.Label2.Text = "WebAdmin Port"
        '
        'Button3
        '
        Me.Button3.FlatAppearance.BorderSize = 0
        Me.Button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button3.Font = New System.Drawing.Font("Arial", 10.0!)
        Me.Button3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(135, Byte), Integer), CType(CType(203, Byte), Integer))
        Me.Button3.Location = New System.Drawing.Point(119, 282)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(230, 26)
        Me.Button3.TabIndex = 60
        Me.Button3.Text = "Open (and copy pwd to clipboard)"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'AddFirewalls
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(435, 406)
        Me.ControlBox = False
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.WebadminPortTextBox)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.FingerprintTextBox)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.MyTitleBar1)
        Me.Controls.Add(Me.AddAnotherCheckBox)
        Me.Controls.Add(Me.PasswordShowButton)
        Me.Controls.Add(Me.DialogCancelButton)
        Me.Controls.Add(Me.DialogOKButton)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.SSHPassTextBox)
        Me.Controls.Add(Me.SSHHostTextBox)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.CommonPassCheckBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "AddFirewalls"
        Me.Padding = New System.Windows.Forms.Padding(2)
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Add Firewall"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents CommonPassCheckBox As CheckBox
    Friend WithEvents PasswordShowButton As Button
    Friend WithEvents DialogCancelButton As Button
    Friend WithEvents DialogOKButton As Button
    Friend WithEvents Label3 As Label
    Friend WithEvents SSHPassTextBox As TextBox
    Friend WithEvents SSHHostTextBox As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents AddAnotherCheckBox As CheckBox
    Friend WithEvents MyTitleBar1 As MyTitleBar
    Friend WithEvents Button1 As Button
    Friend WithEvents Button2 As Button
    Friend WithEvents FingerprintTextBox As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents WebadminPortTextBox As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Button3 As Button
End Class
