' Copyright 2020  Sophos Ltd.  All rights reserved.
' Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
' You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
' Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, 
' WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing 
' permissions and limitations under the License.

Imports System.Security.AccessControl
Imports XGManagementHelper.Randomness

Public Class Login
    ReadOnly pws As New PasswordMeter.PasswordStrength
    ReadOnly Property Key As String
        Get
            Return DATAKEY
        End Get
    End Property

    Private PBKDF2IV As String
    Private PBKDF2SALT As String
    Private PBKDF2Key As String
    Private DATAKEY As String
    Private _NewPassword As Boolean = False

    Private recoveredKey As String
    Private recoveredIV As String

    ReadOnly Property NewPassword As Boolean
        Get
            Return _NewPassword
        End Get
    End Property

    Declare Auto Function SendMessage Lib "user32.dll" (ByVal hWnd As IntPtr, ByVal msg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
    Enum ProgressBarColor
        Green = &H1
        Red = &H2
        Yellow = &H3
    End Enum
    Private Shared Sub ChangeProgBarColor(ByVal ProgressBar_Name As Windows.Forms.ProgressBar, ByVal ProgressBar_Color As ProgressBarColor)
        SendMessage(ProgressBar_Name.Handle, &H410, ProgressBar_Color, 0)
    End Sub

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
        pws.MinimumLength = 8
        pws.MinimumRequirements = 4

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
            SetPasswordButton.Enabled = False
            ConfirmNewPasswordTextBox.Enabled = False

        ElseIf strength.ToLower.Contains("very weak") Then
            ConfirmNewPasswordTextBox.Enabled = False
            StatusLabel.Text = "PASSWORD IS " & strength.ToUpper
            StatusLabel.ForeColor = Color.Red
            SetPasswordButton.Enabled = False

        Else
            ConfirmNewPasswordTextBox.Enabled = True
            If NewPasswordTextBox.Text.Equals(ConfirmNewPasswordTextBox.Text) Then
                StatusLabel.Text = "PASSWORDS MATCH"
                StatusLabel.ForeColor = Color.Green
                SetPasswordButton.Enabled = True

            Else
                StatusLabel.Text = "PASSWORDS DO NOT MATCH"
                StatusLabel.ForeColor = Color.Red
                SetPasswordButton.Enabled = False

            End If
        End If


    End Sub



    Private Sub LoadSettings()

        Using key As Microsoft.Win32.RegistryKey = My.Computer.Registry.CurrentUser.CreateSubKey("Software\XGMigrationHelper")

            PBKDF2IV = key.GetValue("Init1", "NOT SET")
            PBKDF2SALT = key.GetValue("Init2", "NOT SET")
            Dim DATAKEY_CHECK As String = key.GetValue("datavalue", "NOT SET")

            Me.Height = MyTitleBar1.Height + SetPasswordPanel.Height

            If PBKDF2IV.Equals("NOT SET") Or PBKDF2SALT.Equals("NOT SET") Or DATAKEY_CHECK.Equals("NOT SET") Then
                SetPasswordPanel.Dock = DockStyle.Fill
                SetPasswordPanel.Visible = True
                LoginPanel.Visible = False
                _NewPassword = True
            Else
                LoginPanel.Dock = DockStyle.Fill
                LoginPanel.Visible = True
                SetPasswordPanel.Visible = False
                _NewPassword = False
            End If

        End Using
    End Sub

    Private Sub Login_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadSettings()
    End Sub

    Private Sub NewPasswordTextBox_TextChanged(sender As Object, e As EventArgs) Handles NewPasswordTextBox.TextChanged, ConfirmNewPasswordTextBox.TextChanged
        EnableDisable()
    End Sub

    Private Sub SetPasswordButton_Click(sender As Object, e As EventArgs) Handles SetPasswordButton.Click
        Using key As Microsoft.Win32.RegistryKey = My.Computer.Registry.CurrentUser.CreateSubKey("Software\XGMigrationHelper")
            PBKDF2IV = key.GetValue("Init1")
            If PBKDF2IV Is Nothing OrElse PBKDF2IV.Length < 16 Then
                PBKDF2IV = Convert.ToBase64String(GetRandomBytes(16))
                key.SetValue("Init1", PBKDF2IV)
                Debug.Print("Init1 set")
            End If

            PBKDF2SALT = key.GetValue("Init2")
            If PBKDF2SALT Is Nothing OrElse PBKDF2SALT.Length < 16 Then
                PBKDF2SALT = Convert.ToBase64String(GetRandomBytes(16))
                key.SetValue("Init2", PBKDF2SALT)
                Debug.Print("Init2 set")
            End If

            PBKDF2Key = Convert.ToBase64String(PBKDF2.CryptDeriveKey(ConfirmNewPasswordTextBox.Text, PBKDF2SALT, 100000))
            If recoveredKey IsNot Nothing Then
                DATAKEY = recoveredKey
                key.SetValue("Init3", recoveredIV)

            Else
                DATAKEY = Convert.ToBase64String(GetRandomBytes(32))
            End If

            Using AESWrapper As New AES256Wrapper(PBKDF2Key, PBKDF2IV)
                key.SetValue("datavalue", AESWrapper.Encrypt(DATAKEY))
            End Using

        End Using
    End Sub

    Private failcount As Integer = 0
    Private Sub LoginButton_Click(sender As Object, e As EventArgs) Handles LoginButton.Click
        PBKDF2Key = Convert.ToBase64String(PBKDF2.CryptDeriveKey(LoginTextBox.Text, PBKDF2SALT, 100000))
        Using key As Microsoft.Win32.RegistryKey = My.Computer.Registry.CurrentUser.CreateSubKey("Software\XGMigrationHelper")
            Using AESWrapper As New AES256Wrapper(PBKDF2Key, PBKDF2IV)
                DATAKEY = AESWrapper.Decrypt(key.GetValue("datavalue", ""))
                If DATAKEY.Length = 0 Then
                    ErrorLabel.Visible = True
                    LoginTextBox.Clear()
                    LoginTextBox.Focus()
                    failcount += 1
                    If failcount >= 3 Then
                        DialogResult = DialogResult.Cancel
                        Me.Close()
                    End If
                Else
                    ErrorLabel.Visible = False
                    DialogResult = DialogResult.OK
                    Me.Close()
                End If
            End Using

        End Using
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim ofd As New OpenFileDialog With {.Filter = "XG Management Helper Recovery|*.recovery", .InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments}
        If ofd.ShowDialog = DialogResult.OK Then
            Dim input As Byte() = IO.File.ReadAllBytes(ofd.FileName)
            If input.Length = 48 Then
                Dim k(31) As Byte
                Dim iv(15) As Byte
                For x As Integer = 0 To 31
                    k(x) = input(x)
                Next
                For x As Integer = 0 To 15
                    iv(x) = input(x + k.Length)
                Next
                recoveredKey = Convert.ToBase64String(k)
                recoveredIV = Convert.ToBase64String(iv)

                SetPasswordPanel.Dock = DockStyle.Fill
                SetPasswordPanel.Visible = True
                LoginPanel.Visible = False
                _NewPassword = True

            Else
                MsgBox("Recovery file is not valid")

            End If
        End If
    End Sub

    Private Sub LoginTextBox_KeyDown(sender As Object, e As KeyEventArgs) Handles LoginTextBox.KeyDown
        If e.KeyCode = Keys.Return Or e.KeyCode = Keys.Enter Then
            e.Handled = True
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub LoginTextBox_KeyUp(sender As Object, e As KeyEventArgs) Handles LoginTextBox.KeyUp
        If e.KeyCode = Keys.Return Or e.KeyCode = Keys.Enter Then
            e.Handled = True
            e.SuppressKeyPress = True
            LoginButton_Click(sender, e)
        End If
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        Select Case sender.Text
            Case "Show"
                LoginTextBox.PasswordChar = ""
                LoginTextBox.UseSystemPasswordChar = False
                sender.Text = "Hide"

            Case "Hide"
                LoginTextBox.PasswordChar = "X"
                LoginTextBox.UseSystemPasswordChar = True
                sender.Text = "Show"

        End Select
    End Sub
End Class