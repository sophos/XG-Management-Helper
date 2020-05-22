' Copyright 2020  Alan Toews  All rights reserved.
' Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
' You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
' Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, 
' WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing 
' permissions and limitations under the License.

Imports System.Runtime.InteropServices

Public Class MyTitleBar

#Region "Windows API Calls"
    Private Const WM_NCLBUTTONDOWN As Integer = &HA1
    Private Const HTBORDER As Integer = 18
    Private Const HTBOTTOM As Integer = 15
    Private Const HTBOTTOMLEFT As Integer = 16
    Private Const HTBOTTOMRIGHT As Integer = 17
    Private Const HTCAPTION As Integer = 2
    Private Const HTLEFT As Integer = 10
    Private Const HTRIGHT As Integer = 11
    Private Const HTTOP As Integer = 12
    Private Const HTTOPLEFT As Integer = 13
    Private Const HTTOPRIGHT As Integer = 14
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

    Private Const BorderWidth As Integer = 6
    Private _resizeDir As ResizeDirection = ResizeDirection.None

    Private Enum ResizeDirection
        None = 0
        Left = 1
        TopLeft = 2
        Top = 3
        TopRight = 4
        Right = 5
        BottomRight = 6
        Bottom = 7
        BottomLeft = 8
    End Enum

    Private Property ResizeDir() As ResizeDirection
        Get
            Return _resizeDir
        End Get
        Set(ByVal value As ResizeDirection)
            _resizeDir = value
        End Set
    End Property

#Region "Titlebar Controls"
    Private EventSet As Boolean = False
    Private LastDown As DateTime = DateTime.MinValue
    Private WithEvents ParentF As Form


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

    Private Sub CloseLabel_Click(sender As Object, e As EventArgs) Handles ControlButton.Click
        Me.ParentForm.DialogResult = DialogResult.Cancel
        Me.ParentForm.Close()
    End Sub

    Private Sub MinLabel_Click(sender As Object, e As EventArgs) Handles MinimizeButton.Click
        Me.ParentForm.WindowState = FormWindowState.Minimized
    End Sub
    Private Sub MaxLabel_Click(sender As Object, e As EventArgs) Handles MaximizeButton.Click
        If Me.ParentForm.WindowState = FormWindowState.Maximized Then
            Me.ParentForm.WindowState = FormWindowState.Normal
        Else
            Me.ParentForm.WindowState = FormWindowState.Maximized
        End If

    End Sub

    Private Sub WindowActionLabels_MouseEnter(sender As Object, e As EventArgs)
        sender.forecolor = Color.White
    End Sub

    Private Sub MinLabel_MouseLeave(sender As Object, e As EventArgs)
        sender.forecolor = Color.Silver
    End Sub

    Private Sub MyTitleBar_ParentChanged(sender As Object, e As EventArgs) Handles Me.ParentChanged
        Try
            If Not Me.ParentForm Is Nothing Then
                Me.Dock = DockStyle.Top
                Me.SendToBack()
                Me.Text = Me.ParentForm.Text

                Me.ParentForm.Padding = New Padding(BorderWidth + 1, 0, BorderWidth + 1, BorderWidth + 1)
                'Me.MinimizeBox = Me.ParentForm.MinimizeBox
                'Me.MaximizeBox = Me.ParentForm.MaximizeBox
                'Me.ControlBox = Me.ParentForm.ControlBox

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
                    AddHandler Me.ParentForm.MouseDown, AddressOf ParentForm_MouseDown
                    AddHandler Me.ParentForm.MouseMove, AddressOf ParentForm_MouseMove
                    AddHandler Me.ParentForm.MouseLeave, AddressOf ParentForm_MouseLeave
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
                Return Me.MinimizeButton.Visible
            Catch ex As Exception
                Return True
            End Try
        End Get
        Set(value As Boolean)
            Me.MinimizeButton.Visible = value
        End Set
    End Property

    Public Property MaximizeBox As Boolean
        Get
            Try
                Return Me.MaximizeButton.Visible
            Catch ex As Exception
                Return True
            End Try
        End Get
        Set(value As Boolean)
            Me.MaximizeButton.Visible = value
            Me.MaximizeSpacerLabel.Visible = value
        End Set
    End Property

    Public Property ControlBox As Boolean
        Get
            Try
                Return Me.ControlButton.Visible
            Catch ex As Exception
                Return True
            End Try
        End Get
        Set(value As Boolean)
            Me.ControlButton.Visible = value
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
        If Me.ParentForm.WindowState = FormWindowState.Maximized Then
            MaximizeButton.BackgroundImage = XGManagementHelper.My.Resources.Resources.restore
        Else
            MaximizeButton.BackgroundImage = XGManagementHelper.My.Resources.Resources.maximize
        End If
        Me.ParentForm.Invalidate()

    End Sub

    Private Sub ParentForm_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        If e.Button = Windows.Forms.MouseButtons.Left And Me.ParentForm.WindowState <> FormWindowState.Maximized And MaximizeBox Then
            ResizeForm(ResizeDir)
        End If
    End Sub

    Private Sub ParentForm_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        'Calculate which direction to resize based on mouse position

        'Change cursor        

        If e.Location.X < BorderWidth And e.Location.Y < BorderWidth Then
            ResizeDir = ResizeDirection.TopLeft
            Me.ParentForm.Cursor = Cursors.SizeNWSE
            '
        ElseIf e.Location.X < BorderWidth And e.Location.Y > Me.ParentForm.Height - BorderWidth Then
            ResizeDir = ResizeDirection.BottomLeft
            Me.ParentForm.Cursor = Cursors.SizeNESW
            '
        ElseIf e.Location.X > Me.ParentForm.Width - BorderWidth And e.Location.Y > Me.ParentForm.Height - BorderWidth Then
            ResizeDir = ResizeDirection.BottomRight
            Me.ParentForm.Cursor = Cursors.SizeNWSE
            '
        ElseIf e.Location.X > Me.ParentForm.Width - BorderWidth And e.Location.Y < BorderWidth Then
            ResizeDir = ResizeDirection.TopRight
            Me.ParentForm.Cursor = Cursors.SizeNESW
            '
        ElseIf e.Location.X < BorderWidth Then
            ResizeDir = ResizeDirection.Left
            Me.ParentForm.Cursor = Cursors.SizeWE
            '
        ElseIf e.Location.X > Me.ParentForm.Width - BorderWidth Then
            ResizeDir = ResizeDirection.Right
            Me.ParentForm.Cursor = Cursors.SizeWE
            '
        ElseIf e.Location.Y < BorderWidth Then
            ResizeDir = ResizeDirection.Top
            Me.ParentForm.Cursor = Cursors.SizeNS
            '
        ElseIf e.Location.Y > Me.ParentForm.Height - BorderWidth Then
            ResizeDir = ResizeDirection.Bottom
            Me.ParentForm.Cursor = Cursors.SizeNS
            '
        Else
            ResizeDir = ResizeDirection.None
            Me.ParentForm.Cursor = Cursors.Default
            '
        End If
    End Sub

    Private Sub ResizeForm(ByVal direction As ResizeDirection)
        Dim dir As Integer = -1
        Select Case direction
            Case ResizeDirection.Left
                dir = HTLEFT
            Case ResizeDirection.TopLeft
                dir = HTTOPLEFT
            Case ResizeDirection.Top
                dir = HTTOP
            Case ResizeDirection.TopRight
                dir = HTTOPRIGHT
            Case ResizeDirection.Right
                dir = HTRIGHT
            Case ResizeDirection.BottomRight
                dir = HTBOTTOMRIGHT
            Case ResizeDirection.Bottom
                dir = HTBOTTOM
            Case ResizeDirection.BottomLeft
                dir = HTBOTTOMLEFT
        End Select

        If dir <> -1 Then
            ReleaseCapture()
            SendMessage(Me.ParentForm.Handle, WM_NCLBUTTONDOWN, dir, 0)
        End If
    End Sub


    Private Sub ParentForm_MouseLeave(sender As Object, e As EventArgs)
        Me.ParentForm.Cursor = Cursors.Default
    End Sub
End Class
