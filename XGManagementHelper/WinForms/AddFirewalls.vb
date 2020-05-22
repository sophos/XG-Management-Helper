' Copyright 2020  Sophos Ltd.  All rights reserved.
' Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
' You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
' Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, 
' WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing 
' permissions and limitations under the License.
Imports XGManagementHelper.EncryptionHelper
Public Class AddFirewalls
    Private Property CommonPassword As String
    Private IsLoading As Boolean = False
    Sub New(CommonPassword As String)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.CommonPassword = CommonPassword
    End Sub
    Public Property SSHHost As String
        Get
            Return SSHHostTextBox.Text
        End Get
        Set(value As String)
            SSHHostTextBox.Text = value
            SSHHostTextBox.ReadOnly = True
            Me.Text = "Edit Firewall"
            AddAnotherCheckBox.Visible = False
            DialogOKButton.Text = "Save"
            IsLoading = True
            Using key As SecureRegistryKey = GetHostKey(value)
                Dim ExpectedFingerprint As String = key.GetSecureValue("Fingerprint")
                If ExpectedFingerprint IsNot Nothing Then
                    If ExpectedFingerprint.StartsWith(SSHHostTextBox.Text) Then
                        ExpectedFingerprint = ExpectedFingerprint.Substring(SSHHostTextBox.Text.Length)
                    Else
                        ExpectedFingerprint = "<No Fingerprint>"
                    End If
                End If
                FingerprintTextBox.Text = ExpectedFingerprint
                WebadminPortTextBox.Text = key.GetValue("Webadmin", "4444")
            End Using
            IsLoading = False
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
        If SSHHostTextBox.Text.Length = 0 Then

            FingerprintTextBox.Text = "<No Fingerprint>"
            WebadminPortTextBox.Text = "4444"
            Exit Sub
        End If

        Using key As SecureRegistryKey = GetHostKey(SSHHostTextBox.Text)
            Dim ExpectedFingerprint As String = key.GetSecureValue("Fingerprint")
            If ExpectedFingerprint IsNot Nothing Then
                If ExpectedFingerprint.StartsWith(SSHHostTextBox.Text) Then
                    ExpectedFingerprint = ExpectedFingerprint.Substring(SSHHostTextBox.Text.Length)
                Else
                    ExpectedFingerprint = "<No Fingerprint>"
                End If
            End If
            FingerprintTextBox.Text = ExpectedFingerprint
            WebadminPortTextBox.Text = key.GetValue("Webadmin", "4444")

        End Using
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

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Clipboard.SetText(SSHPassTextBox.Text)
        Beep()
    End Sub


    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim xg As New XGShellConnection(True)
        Dim pass As String = If(SSHPassTextBox.Text.Length > 0, SSHPassTextBox.Text, CommonPassword)
        Dim result As XGShellConnection.ActionResult = xg.CheckCurrentFirmwareVersion(New KeyValuePair(Of String, String)(SSHHostTextBox.Text, pass), "admin", pass, XGShellConnection.LogSeverity.Critical)
        If result.Success Then
            Using key As SecureRegistryKey = GetHostKey(SSHHostTextBox.Text)
                Dim ExpectedFingerprint As String = key.GetSecureValue("Fingerprint")
                If ExpectedFingerprint IsNot Nothing Then
                    If ExpectedFingerprint.StartsWith(SSHHostTextBox.Text) Then
                        ExpectedFingerprint = ExpectedFingerprint.Substring(SSHHostTextBox.Text.Length)
                    Else
                        ExpectedFingerprint = "<No Fingerprint>"
                    End If
                End If
                FingerprintTextBox.Text = ExpectedFingerprint
            End Using
            MsgBox("Connected Successfully", MsgBoxStyle.Information)
        Else
            MsgBox("Connection Failed: " & vbNewLine & result.Summary, MsgBoxStyle.Exclamation)

        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Clipboard.SetText(SSHPassTextBox.Text)
        Process.Start(String.Format("https://{0}:{1}/", SSHHostTextBox.Text, WebadminPortTextBox.Text))
    End Sub

    Private Sub WebadminPortTextBox_TextChanged(sender As Object, e As EventArgs) Handles WebadminPortTextBox.TextChanged
        If isloading Then Exit Sub
        If WebadminPortTextBox.Text.Length = 0 Then
            WebadminPortTextBox.Text = "4444"
            WebadminPortTextBox.SelectAll()
        ElseIf Not IsNumeric(WebadminPortTextBox.Text) Then
            WebadminPortTextBox.Text = "4444"
            WebadminPortTextBox.SelectAll()
        Else

        End If
        Using key As SecureRegistryKey = GetApplicationRootKey()
            key.SetValue("WebAdmin", WebadminPortTextBox.Text)
        End Using

    End Sub


    Private Sub WebadminPortTextBox_KeyUp(sender As Object, e As KeyEventArgs) Handles WebadminPortTextBox.KeyUp, WebadminPortTextBox.KeyDown
        Select Case e.KeyCode
            Case Keys.D0 To Keys.D9
            Case Keys.Left, Keys.Right, Keys.Back, Keys.Delete
            Case Else
                e.SuppressKeyPress = True
                e.Handled = True
        End Select
    End Sub

End Class