Public Class CentralLogin
    Public Property CentralUser As String
        Get
            Return CentralUserTextBox.Text
        End Get
        Set(value As String)
            CentralUserTextBox.Text = value
            EnableDisable()
        End Set
    End Property
    Public Property CentralPass As String
        Get
            Return CentralPassTextBox.Text
        End Get
        Set(value As String)
            CentralPassTextBox.Text = value
            EnableDisable()
        End Set
    End Property
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles DialogOKButton.Click
        DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles DialogCancelButton.Click
        DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub CentralUserTextBox_TextChanged(sender As Object, e As EventArgs) Handles CentralUserTextBox.TextChanged, CentralPassTextBox.TextAlignChanged
        EnableDisable()

    End Sub
    Private Sub EnableDisable()
        If CentralUser.Length < 7 Or CentralPass.Length < 8 Then
            DialogOKButton.Enabled = False
        Else
            DialogOKButton.Enabled = True
        End If
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles PasswordShowButton.Click
        Select Case PasswordShowButton.Text
            Case "Show"
                CentralPassTextBox.PasswordChar = ""
                CentralPassTextBox.UseSystemPasswordChar = False

                PasswordShowButton.Text = "Hide"
            Case "Hide"
                CentralPassTextBox.PasswordChar = "X"
                CentralPassTextBox.UseSystemPasswordChar = True
                PasswordShowButton.Text = "Show"
        End Select
    End Sub

    Private Sub ClearButton_Click(sender As Object, e As EventArgs) Handles ClearButton.Click
        DialogResult = DialogResult.Abort
        Close()

    End Sub

    Private Sub CentralPassTextBox_TextChanged(sender As Object, e As EventArgs) Handles CentralPassTextBox.TextChanged
        EnableDisable()
    End Sub
End Class