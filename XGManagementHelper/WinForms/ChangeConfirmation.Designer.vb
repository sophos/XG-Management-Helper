<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ChangeConfirmation
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
        Me.MessageLabel = New System.Windows.Forms.Label()
        Me.CountLabel = New System.Windows.Forms.Label()
        Me.FirewallsLabel = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.DialogCancelButton = New System.Windows.Forms.Button()
        Me.DialogOKButton = New System.Windows.Forms.Button()
        Me.MyTitleBar1 = New XGManagementHelper.MyTitleBar()
        Me.DebugCheckBox = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'MessageLabel
        '
        Me.MessageLabel.AutoSize = True
        Me.MessageLabel.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.MessageLabel.Location = New System.Drawing.Point(59, 73)
        Me.MessageLabel.Name = "MessageLabel"
        Me.MessageLabel.Size = New System.Drawing.Size(251, 18)
        Me.MessageLabel.TabIndex = 0
        Me.MessageLabel.Text = "You are about to make changes on "
        '
        'CountLabel
        '
        Me.CountLabel.Font = New System.Drawing.Font("Arial", 24.0!, System.Drawing.FontStyle.Bold)
        Me.CountLabel.ForeColor = System.Drawing.Color.Red
        Me.CountLabel.Location = New System.Drawing.Point(62, 96)
        Me.CountLabel.Name = "CountLabel"
        Me.CountLabel.Size = New System.Drawing.Size(79, 36)
        Me.CountLabel.TabIndex = 1
        Me.CountLabel.Text = "0"
        Me.CountLabel.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'FirewallsLabel
        '
        Me.FirewallsLabel.AutoSize = True
        Me.FirewallsLabel.Font = New System.Drawing.Font("Arial", 24.0!, System.Drawing.FontStyle.Bold)
        Me.FirewallsLabel.ForeColor = System.Drawing.Color.Red
        Me.FirewallsLabel.Location = New System.Drawing.Point(147, 96)
        Me.FirewallsLabel.Name = "FirewallsLabel"
        Me.FirewallsLabel.Size = New System.Drawing.Size(159, 37)
        Me.FirewallsLabel.TabIndex = 2
        Me.FirewallsLabel.Text = "Firewalls"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.Label4.Location = New System.Drawing.Point(59, 143)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(289, 18)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Please confirm you are ready to proceed"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.Label1.Location = New System.Drawing.Point(59, 237)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(289, 18)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Please confirm you are ready to proceed"
        '
        'DialogCancelButton
        '
        Me.DialogCancelButton.BackColor = System.Drawing.Color.Red
        Me.DialogCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.DialogCancelButton.FlatAppearance.BorderSize = 0
        Me.DialogCancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.DialogCancelButton.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold)
        Me.DialogCancelButton.ForeColor = System.Drawing.Color.White
        Me.DialogCancelButton.Location = New System.Drawing.Point(62, 182)
        Me.DialogCancelButton.Name = "DialogCancelButton"
        Me.DialogCancelButton.Size = New System.Drawing.Size(105, 31)
        Me.DialogCancelButton.TabIndex = 22
        Me.DialogCancelButton.Text = "Cancel"
        Me.DialogCancelButton.UseVisualStyleBackColor = False
        '
        'DialogOKButton
        '
        Me.DialogOKButton.BackColor = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(135, Byte), Integer), CType(CType(203, Byte), Integer))
        Me.DialogOKButton.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.DialogOKButton.FlatAppearance.BorderColor = System.Drawing.Color.SteelBlue
        Me.DialogOKButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.DialogOKButton.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold)
        Me.DialogOKButton.ForeColor = System.Drawing.Color.White
        Me.DialogOKButton.Location = New System.Drawing.Point(234, 182)
        Me.DialogOKButton.Name = "DialogOKButton"
        Me.DialogOKButton.Size = New System.Drawing.Size(105, 31)
        Me.DialogOKButton.TabIndex = 21
        Me.DialogOKButton.Text = "CONTINUE"
        Me.DialogOKButton.UseVisualStyleBackColor = False
        '
        'MyTitleBar1
        '
        Me.MyTitleBar1.BackColor = System.Drawing.Color.FromArgb(CType(CType(21, Byte), Integer), CType(CType(47, Byte), Integer), CType(CType(60, Byte), Integer))
        Me.MyTitleBar1.ControlBox = True
        Me.MyTitleBar1.Dock = System.Windows.Forms.DockStyle.Top
        Me.MyTitleBar1.Location = New System.Drawing.Point(2, 2)
        Me.MyTitleBar1.MaximizeBox = False
        Me.MyTitleBar1.MinimizeBox = False
        Me.MyTitleBar1.Name = "MyTitleBar1"
        Me.MyTitleBar1.Size = New System.Drawing.Size(396, 50)
        Me.MyTitleBar1.TabIndex = 23
        '
        'DebugCheckBox
        '
        Me.DebugCheckBox.AutoSize = True
        Me.DebugCheckBox.Location = New System.Drawing.Point(264, 288)
        Me.DebugCheckBox.Name = "DebugCheckBox"
        Me.DebugCheckBox.Size = New System.Drawing.Size(131, 17)
        Me.DebugCheckBox.TabIndex = 24
        Me.DebugCheckBox.Text = "Enable Debug logging"
        Me.DebugCheckBox.UseVisualStyleBackColor = True
        '
        'ChangeConfirmation
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(400, 310)
        Me.ControlBox = False
        Me.Controls.Add(Me.DebugCheckBox)
        Me.Controls.Add(Me.MyTitleBar1)
        Me.Controls.Add(Me.DialogCancelButton)
        Me.Controls.Add(Me.DialogOKButton)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.FirewallsLabel)
        Me.Controls.Add(Me.CountLabel)
        Me.Controls.Add(Me.MessageLabel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ChangeConfirmation"
        Me.Padding = New System.Windows.Forms.Padding(2)
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Change Confirmation"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents MessageLabel As Label
    Friend WithEvents CountLabel As Label
    Friend WithEvents FirewallsLabel As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents DialogCancelButton As Button
    Friend WithEvents DialogOKButton As Button
    Friend WithEvents MyTitleBar1 As MyTitleBar
    Friend WithEvents DebugCheckBox As CheckBox
End Class
