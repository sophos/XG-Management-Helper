<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MyTitleBar
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.lblMax = New System.Windows.Forms.Label()
        Me.lblMin = New System.Windows.Forms.Label()
        Me.lblClose = New System.Windows.Forms.Label()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'lblMax
        '
        Me.lblMax.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblMax.AutoSize = True
        Me.lblMax.Font = New System.Drawing.Font("Wingdings", 12.0!)
        Me.lblMax.ForeColor = System.Drawing.Color.Silver
        Me.lblMax.Location = New System.Drawing.Point(624, 16)
        Me.lblMax.Name = "lblMax"
        Me.lblMax.Size = New System.Drawing.Size(22, 17)
        Me.lblMax.TabIndex = 7
        Me.lblMax.Text = "o"
        '
        'lblMin
        '
        Me.lblMin.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblMin.AutoSize = True
        Me.lblMin.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMin.ForeColor = System.Drawing.Color.Silver
        Me.lblMin.Location = New System.Drawing.Point(597, 10)
        Me.lblMin.Name = "lblMin"
        Me.lblMin.Size = New System.Drawing.Size(18, 20)
        Me.lblMin.TabIndex = 5
        Me.lblMin.Text = "_"
        '
        'lblClose
        '
        Me.lblClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblClose.AutoSize = True
        Me.lblClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClose.ForeColor = System.Drawing.Color.Silver
        Me.lblClose.Location = New System.Drawing.Point(650, 16)
        Me.lblClose.Name = "lblClose"
        Me.lblClose.Size = New System.Drawing.Size(20, 20)
        Me.lblClose.TabIndex = 4
        Me.lblClose.Text = "X"
        '
        'lblTitle
        '
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTitle.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.lblTitle.ForeColor = System.Drawing.Color.White
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Padding = New System.Windows.Forms.Padding(5, 0, 0, 0)
        Me.lblTitle.Size = New System.Drawing.Size(680, 50)
        Me.lblTitle.TabIndex = 6
        Me.lblTitle.Text = "Window Title"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'MyTitleBar
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(21, Byte), Integer), CType(CType(47, Byte), Integer), CType(CType(60, Byte), Integer))
        Me.Controls.Add(Me.lblMax)
        Me.Controls.Add(Me.lblMin)
        Me.Controls.Add(Me.lblClose)
        Me.Controls.Add(Me.lblTitle)
        Me.DoubleBuffered = True
        Me.Name = "MyTitleBar"
        Me.Size = New System.Drawing.Size(680, 50)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblMax As Label
    Friend WithEvents lblMin As Label
    Friend WithEvents lblClose As Label
    Friend WithEvents lblTitle As Label
End Class
