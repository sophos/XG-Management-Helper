Imports System.Runtime.InteropServices

Public Class MyTitleBar

#Region "Windows API Calls"
    Public Const WM_NCLBUTTONDOWN As Integer = &HA1
    Public Const HT_CAPTION As Integer = &H2
    <DllImportAttribute("user32.dll")>
    Private Shared Function GetForegroundWindow() As Long
    End Function

    <DllImportAttribute("user32.dll")>
    Private Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
    End Function

    <DllImportAttribute("user32.dll")>
    Private Shared Function ReleaseCapture() As Boolean
    End Function
#End Region

#Region "Titlebar Controls"
    Private EventSet As Boolean = False
    Private LastDown As DateTime = DateTime.MinValue

    Private Sub HandleMouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown, TitleLabel.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Left Then
            If Now.Subtract(LastDown).TotalMilliseconds > SystemInformation.DoubleClickTime Then
                ReleaseCapture()
                SendMessage(Me.ParentForm.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0)
                LastDown = Now
            Else
                If e.Button = Windows.Forms.MouseButtons.Left Then
                    If Me.ParentForm.WindowState = FormWindowState.Maximized Then
                        Me.ParentForm.WindowState = FormWindowState.Normal
                    Else
                        Me.ParentForm.WindowState = FormWindowState.Maximized
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub CloseLabel_Click(sender As Object, e As EventArgs) Handles CloseLabel.Click
        Me.ParentForm.Close()
    End Sub

    Private Sub MinLabel_Click(sender As Object, e As EventArgs) Handles MinLabel.Click
        Me.ParentForm.WindowState = FormWindowState.Minimized
    End Sub
    Private Sub MaxLabel_Click(sender As Object, e As EventArgs) Handles MaxLabel.Click
        Me.ParentForm.WindowState = FormWindowState.Maximized
    End Sub

    Private Sub WindowActionLabels_MouseEnter(sender As Object, e As EventArgs) Handles CloseLabel.MouseEnter, MinLabel.MouseEnter
        sender.forecolor = Color.White
    End Sub

    Private Sub MinLabel_MouseLeave(sender As Object, e As EventArgs) Handles MinLabel.MouseLeave
        sender.forecolor = Color.Silver
    End Sub

    Private Sub MyTitleBar_ParentChanged(sender As Object, e As EventArgs) Handles Me.ParentChanged
        Try
            If Not Me.ParentForm Is Nothing Then
                Me.Dock = DockStyle.Top
                Me.SendToBack()
                Me.MinimizeBox = Me.ParentForm.MinimizeBox
                Me.Text = Me.ParentForm.Text

                Me.ParentForm.Padding = New Padding(2, 2, 2, 2)
                Me.ParentForm.MinimizeBox = False
                Me.ParentForm.MaximizeBox = False
                Me.ParentForm.ControlBox = False
                Me.ParentForm.FormBorderStyle = FormBorderStyle.None
                If Not EventSet Then
                    AddHandler Me.ParentForm.TextChanged, AddressOf TextChangeHandler
                    AddHandler Me.ParentForm.Paint, AddressOf ParentForm_Paint
                    AddHandler Me.ParentForm.Activated, AddressOf ParentForm_Redraw
                    AddHandler Me.ParentForm.Deactivate, AddressOf ParentForm_Redraw
                    AddHandler Me.ParentForm.Resize, AddressOf ParentForm_Redraw
                    EventSet = True
                End If
            End If
        Catch ex As Exception
        End Try

    End Sub

    Private Sub TextChangeHandler(sender As Object, e As EventArgs)
        Me.Text = sender.text
    End Sub

#End Region

    Public Overrides Property Text As String
        Get
            Try
                Return TitleLabel.Text
            Catch ex As Exception
                Return ""
            End Try
        End Get
        Set(value As String)
            Try
                TitleLabel.Text = value
            Catch ex As Exception
                TitleLabel.Text = ""
            End Try
        End Set
    End Property

    Public Property MinimizeBox As Boolean
        Get
            Try
                Return Me.MinLabel.Visible
            Catch ex As Exception
                Return True
            End Try
        End Get
        Set(value As Boolean)
            Me.MinLabel.Visible = value
        End Set
    End Property

    Public Property MaximizeBox As Boolean
        Get
            Try
                Return Me.MaxLabel.Visible
            Catch ex As Exception
                Return True
            End Try
        End Get
        Set(value As Boolean)
            Me.MaxLabel.Visible = value
        End Set
    End Property
    Private Sub ParentForm_Paint(sender As Object, e As PaintEventArgs)
        Dim activecolor As Color = Color.FromArgb(25, 135, 203)
        Dim inactivecolor As Color = Color.Gray
        Dim thiscolor As Color = inactivecolor
        Try
            Dim Fhandle As Long = GetForegroundWindow()
            If Me.ParentForm.Handle.ToInt64.Equals(Fhandle) Then thiscolor = activecolor
            e.Graphics.DrawRectangle(New Pen(New SolidBrush(thiscolor), 1), New Rectangle(1, 1, Me.ParentForm.Width - 2, Me.ParentForm.Height - 2))
        Catch ex As Exception

        End Try

    End Sub
    Private Sub ParentForm_Redraw(sender As Object, e As EventArgs)
        Me.ParentForm.Invalidate()
    End Sub


End Class
