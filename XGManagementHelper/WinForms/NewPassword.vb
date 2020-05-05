Public Class NewPassword
    Declare Auto Function SendMessage Lib "user32.dll" (ByVal hWnd As IntPtr, ByVal msg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
    Enum ProgressBarColor
        Green = &H1
        Red = &H2
        Yellow = &H3
    End Enum
    Private Shared Sub ChangeProgBarColor(ByVal ProgressBar_Name As Windows.Forms.ProgressBar, ByVal ProgressBar_Color As ProgressBarColor)
        SendMessage(ProgressBar_Name.Handle, &H410, ProgressBar_Color, 0)
    End Sub
    ReadOnly Property GeneratePasswords As Boolean
        Get
            Return GenerateCheckBox.Checked
        End Get
    End Property

    ReadOnly Property NewPassword As String
        Get
            Return NewPasswordTextBox.Text
        End Get
    End Property

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles NewPasswordShowButton.Click
        Select Case sender.Text
            Case "Show"
                NewPasswordTextBox.PasswordChar = ""
                NewPasswordTextBox.UseSystemPasswordChar = False
                sender.Text = "Hide"

            Case "Hide"
                NewPasswordTextBox.PasswordChar = "X"
                NewPasswordTextBox.UseSystemPasswordChar = True
                sender.Text = "Show"

        End Select
    End Sub

    Private Sub PasswordShowButton_Click(sender As Object, e As EventArgs) Handles PasswordShowButton.Click
        Select Case PasswordShowButton.Text
            Case "Show"
                ConfirmNewPasswordTextBox.PasswordChar = ""
                ConfirmNewPasswordTextBox.UseSystemPasswordChar = False
                sender.Text = "Hide"

            Case "Hide"
                ConfirmNewPasswordTextBox.PasswordChar = "X"
                ConfirmNewPasswordTextBox.UseSystemPasswordChar = True
                sender.Text = "Show"

        End Select
    End Sub

    ReadOnly pws As New PasswordMeter.PasswordStrength
    Private Sub ConfirmNewPasswordTextBox_TextChanged(sender As Object, e As EventArgs) Handles ConfirmNewPasswordTextBox.TextChanged, NewPasswordTextBox.TextChanged
        EnableDisable()
    End Sub

    Private Sub UniqueCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles GenerateCheckBox.CheckedChanged
        EnableDisable()
    End Sub

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

    Private Sub EnableDisable()
        If GenerateCheckBox.Checked Then
            NewPasswordTextBox.Enabled = False
            ConfirmNewPasswordTextBox.Enabled = False
            NewPasswordShowButton.Enabled = False
            PasswordShowButton.Enabled = False
            StatusLabel.Visible = False
            DialogOKButton.Enabled = True

        Else
            NewPasswordTextBox.Enabled = True
            ConfirmNewPasswordTextBox.Enabled = True
            NewPasswordShowButton.Enabled = True
            PasswordShowButton.Enabled = True
            StatusLabel.Visible = True

            pws.SetPassword(NewPasswordTextBox.Text)
            Dim strength As String = pws.GetPasswordStrength
            StrengthLabel.Text = strength
            StrengthLabel.Left = NewPasswordTextBox.Right - StrengthLabel.Width

            SetProgressBar(pws.GetPasswordScore())

            If NewPasswordTextBox.Text.Length = 0 Then
                StatusLabel.Text = "CHOOSE A STRONG PASSWORD"
                StatusLabel.ForeColor = Color.Red
                DialogOKButton.Enabled = False
                ConfirmNewPasswordTextBox.Enabled = False

            ElseIf strength.ToLower.Contains("very weak") Then
                ConfirmNewPasswordTextBox.Enabled = False
                StatusLabel.Text = "PASSWORD IS " & strength.ToUpper
                StatusLabel.ForeColor = Color.Red
                DialogOKButton.Enabled = False

            Else
                ConfirmNewPasswordTextBox.Enabled = True
                If NewPasswordTextBox.Text.Equals(ConfirmNewPasswordTextBox.Text) Then
                    StatusLabel.Text = "PASSWORDS MATCH"
                    StatusLabel.ForeColor = Color.Green
                    DialogOKButton.Enabled = True

                Else
                    StatusLabel.Text = "PASSWORDS DO NOT MATCH"
                    StatusLabel.ForeColor = Color.Red
                    DialogOKButton.Enabled = False

                End If
            End If

        End If

    End Sub
End Class