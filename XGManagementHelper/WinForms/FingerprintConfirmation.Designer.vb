<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FingerprintConfirmation
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
        Me.OKButton = New System.Windows.Forms.Button()
        Me.MessageLabel = New System.Windows.Forms.Label()
        Me.CancelButton = New System.Windows.Forms.Button()
        Me.FingerprintLabel = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.MyTitleBar1 = New XGManagementHelper.MyTitleBar()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'OKButton
        '
        Me.OKButton.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OKButton.BackColor = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(135, Byte), Integer), CType(CType(203, Byte), Integer))
        Me.OKButton.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.OKButton.FlatAppearance.BorderColor = System.Drawing.Color.SteelBlue
        Me.OKButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.OKButton.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.OKButton.ForeColor = System.Drawing.Color.White
        Me.OKButton.Location = New System.Drawing.Point(195, 133)
        Me.OKButton.Name = "OKButton"
        Me.OKButton.Size = New System.Drawing.Size(147, 31)
        Me.OKButton.TabIndex = 2
        Me.OKButton.Text = "Trust"
        Me.OKButton.UseVisualStyleBackColor = False
        '
        'MessageLabel
        '
        Me.MessageLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me.MessageLabel.Font = New System.Drawing.Font("Tahoma", 8.25!)
        Me.MessageLabel.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.MessageLabel.Location = New System.Drawing.Point(8, 0)
        Me.MessageLabel.Name = "MessageLabel"
        Me.MessageLabel.Size = New System.Drawing.Size(349, 49)
        Me.MessageLabel.TabIndex = 45
        Me.MessageLabel.Text = "The firewall's host key is not known. Do you want to trust it?"
        Me.MessageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'CancelButton
        '
        Me.CancelButton.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CancelButton.FlatAppearance.BorderSize = 0
        Me.CancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CancelButton.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.CancelButton.ForeColor = System.Drawing.Color.SteelBlue
        Me.CancelButton.Location = New System.Drawing.Point(23, 133)
        Me.CancelButton.Name = "CancelButton"
        Me.CancelButton.Size = New System.Drawing.Size(147, 31)
        Me.CancelButton.TabIndex = 1
        Me.CancelButton.Text = "Cancel"
        Me.CancelButton.UseVisualStyleBackColor = True
        '
        'FingerprintLabel
        '
        Me.FingerprintLabel.BackColor = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(135, Byte), Integer), CType(CType(203, Byte), Integer))
        Me.FingerprintLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me.FingerprintLabel.Font = New System.Drawing.Font("Courier New", 8.0!, System.Drawing.FontStyle.Bold)
        Me.FingerprintLabel.ForeColor = System.Drawing.Color.White
        Me.FingerprintLabel.Location = New System.Drawing.Point(8, 73)
        Me.FingerprintLabel.Name = "FingerprintLabel"
        Me.FingerprintLabel.Size = New System.Drawing.Size(349, 17)
        Me.FingerprintLabel.TabIndex = 47
        Me.FingerprintLabel.Text = "00-00-00-00-00-00-00-00-00-00"
        Me.FingerprintLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label2
        '
        Me.Label2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 8.25!)
        Me.Label2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label2.Location = New System.Drawing.Point(8, 49)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(349, 24)
        Me.Label2.TabIndex = 48
        Me.Label2.Text = "Firewall Fingerprint"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'MyTitleBar1
        '
        Me.MyTitleBar1.BackColor = System.Drawing.Color.FromArgb(CType(CType(21, Byte), Integer), CType(CType(47, Byte), Integer), CType(CType(60, Byte), Integer))
        Me.MyTitleBar1.Dock = System.Windows.Forms.DockStyle.Top
        Me.MyTitleBar1.Location = New System.Drawing.Point(2, 2)
        Me.MyTitleBar1.MaximizeBox = False
        Me.MyTitleBar1.MinimizeBox = True
        Me.MyTitleBar1.Name = "MyTitleBar1"
        Me.MyTitleBar1.Size = New System.Drawing.Size(365, 50)
        Me.MyTitleBar1.TabIndex = 49
        Me.MyTitleBar1.TabStop = False
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.CancelButton)
        Me.Panel1.Controls.Add(Me.FingerprintLabel)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.OKButton)
        Me.Panel1.Controls.Add(Me.MessageLabel)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(2, 52)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Padding = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.Panel1.Size = New System.Drawing.Size(365, 194)
        Me.Panel1.TabIndex = 50
        '
        'FingerprintConfirmation
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(369, 248)
        Me.ControlBox = False
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.MyTitleBar1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FingerprintConfirmation"
        Me.Padding = New System.Windows.Forms.Padding(2)
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Fingerprint Confirmation"
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents OKButton As Button
    Friend WithEvents MessageLabel As Label
    Friend WithEvents CancelButton As Button
    Friend WithEvents FingerprintLabel As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents MyTitleBar1 As MyTitleBar
    Friend WithEvents Panel1 As Panel
End Class
