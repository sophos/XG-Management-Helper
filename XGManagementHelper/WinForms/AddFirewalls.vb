Public Class AddFirewalls
    Public Property SSHHost As String
        Get
            Return SSHHostTextBox.Text
        End Get
        Set(value As String)
            SSHHostTextBox.Text = value
            EnableDisable()
        End Set
    End Property
    Public Property SSHPass As String
        Get
            Return SSHPassTextBox.Text
        End Get
        Set(value As String)
            SSHPassTextBox.Text = value
            EnableDisable()
        End Set
    End Property
    Public Property AddAnother As Boolean
        Get
            Return AddAnotherCheckBox.Checked
        End Get
        Set(value As Boolean)
            AddAnotherCheckBox.Checked = value
        End Set
    End Property


    Private Sub CentralUserTextBox_KeyDown(sender As Object, e As KeyEventArgs) Handles SSHHostTextBox.KeyDown, SSHPassTextBox.KeyDown
        Select Case e.KeyCode
            Case Keys.Return, Keys.Enter
                e.SuppressKeyPress = True
                e.Handled = True
                Me.DialogResult = DialogResult.OK
            Case Else
        End Select
    End Sub

    Private Sub CentralUserTextBox_KeyUp(sender As Object, e As KeyEventArgs) Handles SSHHostTextBox.KeyUp, SSHPassTextBox.KeyUp
        Select Case e.KeyCode
            Case Keys.Return, Keys.Enter
                'AddHost()
                e.SuppressKeyPress = True
                e.Handled = True
            Case Else
        End Select
    End Sub

    Private Sub PasswordShowButton_Click(sender As Object, e As EventArgs) Handles PasswordShowButton.Click
        Select Case PasswordShowButton.Text
            Case "Show"
                SSHPassTextBox.PasswordChar = ""
                SSHPassTextBox.UseSystemPasswordChar = False

                PasswordShowButton.Text = "Hide"
            Case "Hide"
                SSHPassTextBox.PasswordChar = "X"
                SSHPassTextBox.UseSystemPasswordChar = True
                PasswordShowButton.Text = "Show"
        End Select
    End Sub
    Private LoadingBool As Boolean = False
    Private Sub CommonPassCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles CommonPassCheckBox.CheckedChanged
        If LoadingBool Then Exit Sub
        If CommonPassCheckBox.Checked Then
            SSHPassTextBox.Text = ""
        Else
            'leave it alone
        End If
        EnableDisable()
    End Sub

    Private Sub SSHPassTextBox_TextChanged(sender As Object, e As EventArgs) Handles SSHPassTextBox.TextChanged
        LoadingBool = True
        CommonPassCheckBox.Checked = SSHPassTextBox.Text.Length = 0
        LoadingBool = False
    End Sub

    Private Sub SSHHostTextBox_TextChanged(sender As Object, e As EventArgs) Handles SSHHostTextBox.TextChanged, SSHPassTextBox.TextChanged
        EnableDisable()
    End Sub

    Private Sub EnableDisable()
        If SSHHost.Length = 0 Then
            DialogOKButton.Enabled = False
        Else
            If SSHPass.Length > 0 Or CommonPassCheckBox.Checked Then
                DialogOKButton.Enabled = True
            Else
                DialogOKButton.Enabled = False
            End If
        End If
    End Sub


    Private Sub AddFirewalls_GotFocus(sender As Object, e As EventArgs) Handles Me.GotFocus
        SSHHostTextBox.Focus()
    End Sub

    Private Sub AddFirewalls_Activated(sender As Object, e As EventArgs) Handles Me.Activated
        SSHHostTextBox.Focus()
    End Sub

    Private Sub AddFirewalls_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub SSHPassTextBox_GotFocus(sender As Object, e As EventArgs) Handles SSHPassTextBox.GotFocus
        SSHPassTextBox.SelectAll()
    End Sub
End Class