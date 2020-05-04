<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LogViewer
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
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.LogsComboBox = New System.Windows.Forms.ComboBox()
        Me.LogMessagesTextBox = New System.Windows.Forms.TextBox()
        Me.MyTitleBar1 = New XGManagementHelper.MyTitleBar()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.LogsComboBox)
        Me.Panel1.Controls.Add(Me.Button1)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(2, 52)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(796, 26)
        Me.Panel1.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Left
        Me.Label1.Location = New System.Drawing.Point(0, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(132, 26)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Password Change Logs"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'LogsComboBox
        '
        Me.LogsComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.LogsComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.LogsComboBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LogsComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.LogsComboBox.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.LogsComboBox.FormattingEnabled = True
        Me.LogsComboBox.Location = New System.Drawing.Point(132, 0)
        Me.LogsComboBox.Name = "LogsComboBox"
        Me.LogsComboBox.Size = New System.Drawing.Size(518, 26)
        Me.LogsComboBox.TabIndex = 0
        '
        'LogMessagesTextBox
        '
        Me.LogMessagesTextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LogMessagesTextBox.Font = New System.Drawing.Font("Courier New", 12.0!)
        Me.LogMessagesTextBox.Location = New System.Drawing.Point(2, 78)
        Me.LogMessagesTextBox.Multiline = True
        Me.LogMessagesTextBox.Name = "LogMessagesTextBox"
        Me.LogMessagesTextBox.ReadOnly = True
        Me.LogMessagesTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.LogMessagesTextBox.Size = New System.Drawing.Size(796, 370)
        Me.LogMessagesTextBox.TabIndex = 1
        '
        'MyTitleBar1
        '
        Me.MyTitleBar1.BackColor = System.Drawing.Color.FromArgb(CType(CType(21, Byte), Integer), CType(CType(47, Byte), Integer), CType(CType(60, Byte), Integer))
        Me.MyTitleBar1.Dock = System.Windows.Forms.DockStyle.Top
        Me.MyTitleBar1.Location = New System.Drawing.Point(2, 2)
        Me.MyTitleBar1.MaximizeBox = True
        Me.MyTitleBar1.MinimizeBox = True
        Me.MyTitleBar1.Name = "MyTitleBar1"
        Me.MyTitleBar1.Size = New System.Drawing.Size(796, 50)
        Me.MyTitleBar1.TabIndex = 2
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(135, Byte), Integer), CType(CType(203, Byte), Integer))
        Me.Button1.Dock = System.Windows.Forms.DockStyle.Right
        Me.Button1.FlatAppearance.BorderSize = 0
        Me.Button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button1.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.Button1.ForeColor = System.Drawing.Color.White
        Me.Button1.Location = New System.Drawing.Point(650, 0)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(146, 26)
        Me.Button1.TabIndex = 2
        Me.Button1.Text = "Delete"
        Me.Button1.UseVisualStyleBackColor = False
        '
        'LogViewer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.ControlBox = False
        Me.Controls.Add(Me.LogMessagesTextBox)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.MyTitleBar1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "LogViewer"
        Me.Padding = New System.Windows.Forms.Padding(2)
        Me.Text = "LogViewer"
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label1 As Label
    Friend WithEvents LogsComboBox As ComboBox
    Friend WithEvents LogMessagesTextBox As TextBox
    Friend WithEvents MyTitleBar1 As MyTitleBar
    Friend WithEvents Button1 As Button
End Class
