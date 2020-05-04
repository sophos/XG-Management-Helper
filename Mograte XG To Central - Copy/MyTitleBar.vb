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

    Private Sub HandleMouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown, lblTitle.MouseDown
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

    Private Sub lblClose_Click(sender As Object, e As EventArgs) Handles lblClose.Click
        Me.ParentForm.Close()
    End Sub

    Private Sub lblMin_Click(sender As Object, e As EventArgs) Handles lblMin.Click
        Me.ParentForm.WindowState = FormWindowState.Minimized
    End Sub
    Private Sub lblMax_Click(sender As Object, e As EventArgs) Handles lblMax.Click
        Me.ParentForm.WindowState = FormWindowState.Maximized
    End Sub

    Private Sub lblClose_MouseEnter(sender As Object, e As EventArgs) Handles lblClose.MouseEnter, lblMin.MouseEnter
        sender.forecolor = Color.White
    End Sub

    Private Sub lblMin_MouseLeave(sender As Object, e As EventArgs) Handles lblMin.MouseLeave
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
                Return lblTitle.Text
            Catch ex As Exception
                Return ""
            End Try
        End Get
        Set(value As String)
            Try
                lblTitle.Text = value
            Catch ex As Exception
                lblTitle.Text = ""
            End Try
        End Set
    End Property

    Public Property MinimizeBox As Boolean
        Get
            Try
                Return Me.lblMin.Visible
            Catch ex As Exception
                Return True
            End Try
        End Get
        Set(value As Boolean)
            Me.lblMin.Visible = value
        End Set
    End Property

    Public Property MaximizeBox As Boolean
        Get
            Try
                Return Me.lblMax.Visible
            Catch ex As Exception
                Return True
            End Try
        End Get
        Set(value As Boolean)
            Me.lblMax.Visible = value
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

    Private Sub lblTitle_Click(sender As Object, e As EventArgs) Handles lblTitle.Click

    End Sub
    'Public Property MaximizeBox As Boolean


End Class
