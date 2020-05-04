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
        Me.MaxLabel = New System.Windows.Forms.Label()
        Me.MinLabel = New System.Windows.Forms.Label()
        Me.CloseLabel = New System.Windows.Forms.Label()
        Me.TitleLabel = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'MaxLabel
        '
        Me.MaxLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MaxLabel.AutoSize = True
        Me.MaxLabel.Font = New System.Drawing.Font("Wingdings", 12.0!)
        Me.MaxLabel.ForeColor = System.Drawing.Color.Silver
        Me.MaxLabel.Location = New System.Drawing.Point(624, 16)
        Me.MaxLabel.Name = "MaxLabel"
        Me.MaxLabel.Size = New System.Drawing.Size(22, 17)
        Me.MaxLabel.TabIndex = 7
        Me.MaxLabel.Text = "o"
        '
        'MinLabel
        '
        Me.MinLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MinLabel.AutoSize = True
        Me.MinLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MinLabel.ForeColor = System.Drawing.Color.Silver
        Me.MinLabel.Location = New System.Drawing.Point(597, 10)
        Me.MinLabel.Name = "MinLabel"
        Me.MinLabel.Size = New System.Drawing.Size(18, 20)
        Me.MinLabel.TabIndex = 5
        Me.MinLabel.Text = "_"
        '
        'CloseLabel
        '
        Me.CloseLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CloseLabel.AutoSize = True
        Me.CloseLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CloseLabel.ForeColor = System.Drawing.Color.Silver
        Me.CloseLabel.Location = New System.Drawing.Point(650, 16)
        Me.CloseLabel.Name = "CloseLabel"
        Me.CloseLabel.Size = New System.Drawing.Size(20, 20)
        Me.CloseLabel.TabIndex = 4
        Me.CloseLabel.Text = "X"
        '
        'TitleLabel
        '
        Me.TitleLabel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TitleLabel.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.TitleLabel.ForeColor = System.Drawing.Color.White
        Me.TitleLabel.Location = New System.Drawing.Point(0, 0)
        Me.TitleLabel.Name = "TitleLabel"
        Me.TitleLabel.Padding = New System.Windows.Forms.Padding(5, 0, 0, 0)
        Me.TitleLabel.Size = New System.Drawing.Size(680, 50)
        Me.TitleLabel.TabIndex = 6
        Me.TitleLabel.Text = "Window Title"
        Me.TitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'MyTitleBar
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(21, Byte), Integer), CType(CType(47, Byte), Integer), CType(CType(60, Byte), Integer))
        Me.Controls.Add(Me.MaxLabel)
        Me.Controls.Add(Me.MinLabel)
        Me.Controls.Add(Me.CloseLabel)
        Me.Controls.Add(Me.TitleLabel)
        Me.DoubleBuffered = True
        Me.Name = "MyTitleBar"
        Me.Size = New System.Drawing.Size(680, 50)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents MaxLabel As Label
    Friend WithEvents MinLabel As Label
    Friend WithEvents CloseLabel As Label
    Friend WithEvents TitleLabel As Label
End Class
