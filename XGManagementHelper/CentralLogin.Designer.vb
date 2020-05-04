<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CentralLogin
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
        Me.Label3 = New System.Windows.Forms.Label()
        Me.CentralPassTextBox = New System.Windows.Forms.TextBox()
        Me.CentralUserTextBox = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.DialogOKButton = New System.Windows.Forms.Button()
        Me.DialogCancelButton = New System.Windows.Forms.Button()
        Me.PasswordShowButton = New System.Windows.Forms.Button()
        Me.ClearButton = New System.Windows.Forms.Button()
        Me.MyTitleBar1 = New XGManagementHelper.MyTitleBar()
        Me.SuspendLayout()
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(69, 78)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(94, 13)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "EMAIL ADDRESS"
        '
        'CentralPassTextBox
        '
        Me.CentralPassTextBox.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.CentralPassTextBox.Location = New System.Drawing.Point(72, 150)
        Me.CentralPassTextBox.Name = "CentralPassTextBox"
        Me.CentralPassTextBox.PasswordChar = Global.Microsoft.VisualBasic.ChrW(88)
        Me.CentralPassTextBox.Size = New System.Drawing.Size(220, 26)
        Me.CentralPassTextBox.TabIndex = 9
        Me.CentralPassTextBox.UseSystemPasswordChar = True
        '
        'CentralUserTextBox
        '
        Me.CentralUserTextBox.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.CentralUserTextBox.Location = New System.Drawing.Point(72, 94)
        Me.CentralUserTextBox.Name = "CentralUserTextBox"
        Me.CentralUserTextBox.Size = New System.Drawing.Size(220, 26)
        Me.CentralUserTextBox.TabIndex = 8
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(69, 134)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(70, 13)
        Me.Label4.TabIndex = 11
        Me.Label4.Text = "PASSWORD"
        '
        'DialogOKButton
        '
        Me.DialogOKButton.BackColor = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(135, Byte), Integer), CType(CType(203, Byte), Integer))
        Me.DialogOKButton.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.DialogOKButton.FlatAppearance.BorderColor = System.Drawing.Color.SteelBlue
        Me.DialogOKButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.DialogOKButton.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.DialogOKButton.ForeColor = System.Drawing.Color.White
        Me.DialogOKButton.Location = New System.Drawing.Point(187, 220)
        Me.DialogOKButton.Name = "DialogOKButton"
        Me.DialogOKButton.Size = New System.Drawing.Size(105, 31)
        Me.DialogOKButton.TabIndex = 12
        Me.DialogOKButton.Text = "Continue"
        Me.DialogOKButton.UseVisualStyleBackColor = False
        '
        'DialogCancelButton
        '
        Me.DialogCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.DialogCancelButton.FlatAppearance.BorderSize = 0
        Me.DialogCancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.DialogCancelButton.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Underline)
        Me.DialogCancelButton.ForeColor = System.Drawing.Color.SteelBlue
        Me.DialogCancelButton.Location = New System.Drawing.Point(72, 220)
        Me.DialogCancelButton.Name = "DialogCancelButton"
        Me.DialogCancelButton.Size = New System.Drawing.Size(105, 31)
        Me.DialogCancelButton.TabIndex = 13
        Me.DialogCancelButton.Text = "Cancel"
        Me.DialogCancelButton.UseVisualStyleBackColor = True
        '
        'PasswordShowButton
        '
        Me.PasswordShowButton.FlatAppearance.BorderSize = 0
        Me.PasswordShowButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.PasswordShowButton.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.PasswordShowButton.ForeColor = System.Drawing.Color.SteelBlue
        Me.PasswordShowButton.Location = New System.Drawing.Point(296, 151)
        Me.PasswordShowButton.Name = "PasswordShowButton"
        Me.PasswordShowButton.Size = New System.Drawing.Size(62, 23)
        Me.PasswordShowButton.TabIndex = 14
        Me.PasswordShowButton.Text = "Show"
        Me.PasswordShowButton.UseVisualStyleBackColor = True
        '
        'ClearButton
        '
        Me.ClearButton.DialogResult = System.Windows.Forms.DialogResult.Abort
        Me.ClearButton.FlatAppearance.BorderSize = 0
        Me.ClearButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ClearButton.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Underline)
        Me.ClearButton.ForeColor = System.Drawing.Color.SteelBlue
        Me.ClearButton.Location = New System.Drawing.Point(5, 260)
        Me.ClearButton.Name = "ClearButton"
        Me.ClearButton.Size = New System.Drawing.Size(62, 31)
        Me.ClearButton.TabIndex = 15
        Me.ClearButton.Text = "Clear"
        Me.ClearButton.UseVisualStyleBackColor = True
        '
        'MyTitleBar1
        '
        Me.MyTitleBar1.BackColor = System.Drawing.Color.FromArgb(CType(CType(21, Byte), Integer), CType(CType(47, Byte), Integer), CType(CType(60, Byte), Integer))
        Me.MyTitleBar1.Dock = System.Windows.Forms.DockStyle.Top
        Me.MyTitleBar1.Location = New System.Drawing.Point(2, 2)
        Me.MyTitleBar1.MaximizeBox = True
        Me.MyTitleBar1.MinimizeBox = True
        Me.MyTitleBar1.Name = "MyTitleBar1"
        Me.MyTitleBar1.Size = New System.Drawing.Size(366, 50)
        Me.MyTitleBar1.TabIndex = 16
        '
        'CentralLogin
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(370, 296)
        Me.ControlBox = False
        Me.Controls.Add(Me.MyTitleBar1)
        Me.Controls.Add(Me.ClearButton)
        Me.Controls.Add(Me.PasswordShowButton)
        Me.Controls.Add(Me.DialogCancelButton)
        Me.Controls.Add(Me.DialogOKButton)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.CentralPassTextBox)
        Me.Controls.Add(Me.CentralUserTextBox)
        Me.Controls.Add(Me.Label4)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "CentralLogin"
        Me.Padding = New System.Windows.Forms.Padding(2)
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "CentralLogin"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label3 As Label
    Friend WithEvents CentralPassTextBox As TextBox
    Friend WithEvents CentralUserTextBox As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents DialogOKButton As Button
    Friend WithEvents DialogCancelButton As Button
    Friend WithEvents PasswordShowButton As Button
    Friend WithEvents ClearButton As Button
    Friend WithEvents MyTitleBar1 As XGManagementHelper.MyTitleBar
End Class
