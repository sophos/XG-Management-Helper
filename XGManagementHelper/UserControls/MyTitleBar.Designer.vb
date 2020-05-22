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
        Me.TitleLabel = New System.Windows.Forms.Label()
        Me.MinimizeButton = New System.Windows.Forms.Button()
        Me.MaximizeButton = New System.Windows.Forms.Button()
        Me.ControlButton = New System.Windows.Forms.Button()
        Me.MaximizeSpacerLabel = New System.Windows.Forms.Label()
        Me.MinimizeSpacerLabel = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
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
        'MinimizeButton
        '
        Me.MinimizeButton.BackgroundImage = Global.XGManagementHelper.My.Resources.Resources.minimize
        Me.MinimizeButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.MinimizeButton.Dock = System.Windows.Forms.DockStyle.Right
        Me.MinimizeButton.FlatAppearance.BorderSize = 0
        Me.MinimizeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.MinimizeButton.ImageKey = "close"
        Me.MinimizeButton.Location = New System.Drawing.Point(586, 0)
        Me.MinimizeButton.Name = "MinimizeButton"
        Me.MinimizeButton.Size = New System.Drawing.Size(16, 50)
        Me.MinimizeButton.TabIndex = 10
        Me.MinimizeButton.UseVisualStyleBackColor = True
        '
        'MaximizeButton
        '
        Me.MaximizeButton.BackgroundImage = Global.XGManagementHelper.My.Resources.Resources.maximize
        Me.MaximizeButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.MaximizeButton.Dock = System.Windows.Forms.DockStyle.Right
        Me.MaximizeButton.FlatAppearance.BorderSize = 0
        Me.MaximizeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.MaximizeButton.ImageKey = "close"
        Me.MaximizeButton.Location = New System.Drawing.Point(612, 0)
        Me.MaximizeButton.Name = "MaximizeButton"
        Me.MaximizeButton.Size = New System.Drawing.Size(16, 50)
        Me.MaximizeButton.TabIndex = 9
        Me.MaximizeButton.UseVisualStyleBackColor = True
        '
        'ControlButton
        '
        Me.ControlButton.BackgroundImage = Global.XGManagementHelper.My.Resources.Resources.close
        Me.ControlButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.ControlButton.Dock = System.Windows.Forms.DockStyle.Right
        Me.ControlButton.FlatAppearance.BorderSize = 0
        Me.ControlButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ControlButton.ImageKey = "close"
        Me.ControlButton.Location = New System.Drawing.Point(638, 0)
        Me.ControlButton.Name = "ControlButton"
        Me.ControlButton.Size = New System.Drawing.Size(16, 50)
        Me.ControlButton.TabIndex = 8
        Me.ControlButton.UseVisualStyleBackColor = True
        '
        'MaximizeSpacerLabel
        '
        Me.MaximizeSpacerLabel.Dock = System.Windows.Forms.DockStyle.Right
        Me.MaximizeSpacerLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MaximizeSpacerLabel.ForeColor = System.Drawing.Color.Silver
        Me.MaximizeSpacerLabel.ImageKey = "close"
        Me.MaximizeSpacerLabel.Location = New System.Drawing.Point(628, 0)
        Me.MaximizeSpacerLabel.Margin = New System.Windows.Forms.Padding(0)
        Me.MaximizeSpacerLabel.Name = "MaximizeSpacerLabel"
        Me.MaximizeSpacerLabel.Size = New System.Drawing.Size(10, 50)
        Me.MaximizeSpacerLabel.TabIndex = 11
        '
        'MinimizeSpacerLabel
        '
        Me.MinimizeSpacerLabel.Dock = System.Windows.Forms.DockStyle.Right
        Me.MinimizeSpacerLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MinimizeSpacerLabel.ForeColor = System.Drawing.Color.Silver
        Me.MinimizeSpacerLabel.ImageKey = "close"
        Me.MinimizeSpacerLabel.Location = New System.Drawing.Point(602, 0)
        Me.MinimizeSpacerLabel.Margin = New System.Windows.Forms.Padding(0)
        Me.MinimizeSpacerLabel.Name = "MinimizeSpacerLabel"
        Me.MinimizeSpacerLabel.Size = New System.Drawing.Size(10, 50)
        Me.MinimizeSpacerLabel.TabIndex = 12
        '
        'Label3
        '
        Me.Label3.Dock = System.Windows.Forms.DockStyle.Right
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Silver
        Me.Label3.ImageKey = "close"
        Me.Label3.Location = New System.Drawing.Point(654, 0)
        Me.Label3.Margin = New System.Windows.Forms.Padding(0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(26, 50)
        Me.Label3.TabIndex = 13
        '
        'MyTitleBar
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(21, Byte), Integer), CType(CType(47, Byte), Integer), CType(CType(60, Byte), Integer))
        Me.Controls.Add(Me.MinimizeButton)
        Me.Controls.Add(Me.MinimizeSpacerLabel)
        Me.Controls.Add(Me.MaximizeButton)
        Me.Controls.Add(Me.MaximizeSpacerLabel)
        Me.Controls.Add(Me.ControlButton)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.TitleLabel)
        Me.DoubleBuffered = True
        Me.Name = "MyTitleBar"
        Me.Size = New System.Drawing.Size(680, 50)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TitleLabel As Label
    Friend WithEvents ControlButton As Button
    Friend WithEvents MaximizeButton As Button
    Friend WithEvents MinimizeButton As Button
    Friend WithEvents MaximizeSpacerLabel As Label
    Friend WithEvents MinimizeSpacerLabel As Label
    Friend WithEvents Label3 As Label
End Class
