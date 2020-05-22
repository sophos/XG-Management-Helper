Public Class SetToken
    Declare Auto Function SendMessage Lib "user32.dll" (ByVal hWnd As IntPtr, ByVal msg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
    Enum ProgressBarColor
        Green = &H1
        Red = &H2
        Yellow = &H3
    End Enum
    Private Shared Sub ChangeProgBarColor(ByVal ProgressBar_Name As Windows.Forms.ProgressBar, ByVal ProgressBar_Color As ProgressBarColor)
        SendMessage(ProgressBar_Name.Handle, &H410, ProgressBar_Color, 0)
    End Sub

    ReadOnly Property Token As String
        Get
            Return NewPasswordTextBox.Text
        End Get
    End Property
    ReadOnly pws As New PasswordMeter.PasswordStrength With {.MinimumLength = 4, .MinimumRequirements = 3}
    Private Sub SetProgressBar(Value As Integer)
        StrengthProgressBar.Value = Value
        Select Case StrengthProgressBar.Value
            Case < 20
                ChangeProgBarColor(StrengthProgressBar, ProgressBarColor.Red)
                StrengthLabel.ForeColor = Color.Red
            Case 20 To 39
                ChangeProgBarColor(StrengthProgressBar, ProgressBarColor.Yellow)
                StrengthLabel.ForeColor = Color.Orange
            Case Else
                ChangeProgBarColor(StrengthProgressBar, ProgressBarColor.Green)
                StrengthLabel.ForeColor = Color.Green
        End Select
    End Sub

    Private Sub NewPasswordTextBox_TextChanged(sender As Object, e As EventArgs) Handles NewPasswordTextBox.TextChanged
        pws.SetPassword(NewPasswordTextBox.Text)
        Dim strength As String = pws.GetPasswordStrength
        StrengthLabel.Text = strength
        StrengthLabel.Left = NewPasswordTextBox.Right - StrengthLabel.Width

        SetProgressBar(pws.GetPasswordScore())

        Dim message As String = String.Format("Dear user,
The password you use to login to the XG Firewal User Portal, or Sophos Connect VPN client has been changed, with a token value added to the end of it. Users will still login using the same password as before, but will now need to add the following token to the end of it:
""{0}""

For example, if your password now were: ""ABC123""
Then you will now need to login with: ""ABC123{0}"".

This is a temporary security measure, and you may change your password at any time, by logging into the XG Firewall user portal using this new password, selecting ""Personal"" on the left menu, then ""Change Password"". Follow the instructions shown, to choose a new password. Be sure to choose a new and secure password that is at least 
", NewPasswordTextBox.Text)

        If NewPasswordTextBox.Text.Length > 3 Then
            DialogOKButton.Enabled = True
            MessageTextBox.Text = message
        Else
            DialogOKButton.Enabled = False
            MessageTextBox.Text = ""
        End If
    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles CopyMessageButton.Click
        Clipboard.SetText(MessageTextBox.Text)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles MakeTokenButton.Click
        NewPasswordTextBox.Text = Randomness.GetRandomString(10, True)
    End Sub
End Class