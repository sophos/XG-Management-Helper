Public Class LogViewer

    Sub New(Key As String)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        AESWrapper = New AES256Wrapper(Key)
        ListLogs()
    End Sub

    ReadOnly AESWrapper As AES256Wrapper
    Private IsLoading As Boolean

    Private Sub OpenLog(filename As String)

        If Not filename.EndsWith(".enc", StringComparison.CurrentCultureIgnoreCase) Then filename &= ".enc"
        If Not IO.File.Exists(filename) Then
            LogMessagesTextBox.Text = ""
            Exit Sub
        End If

        'IO.Directory.SetCurrentDirectory(Application.StartupPath)
        Dim CipherText As String = IO.File.ReadAllText(filename)
        LogMessagesTextBox.Text = AESWrapper.Decrypt(CipherText)
    End Sub

    Public Sub ShowLog(logname As String)
        IsLoading = True
        If logname.ToLower.EndsWith(".enc") Then
            LogsComboBox.Text = IO.Path.GetFileNameWithoutExtension(logname)
        Else
            LogsComboBox.Text = logname
        End If

        OpenLog(logname & ".enc")
        IsLoading = False
    End Sub
    Private Sub ListLogs()
        Dim files As String() = System.IO.Directory.GetFiles(".\", "*.enc")
        LogsComboBox.Items.Clear()

        For Each file As String In files
            LogsComboBox.Items.Add(IO.Path.GetFileNameWithoutExtension(file))
        Next

        If LogsComboBox.Items.Count > 0 Then
            LogsComboBox.Text = LogsComboBox.Items(LogsComboBox.Items.Count - 1)
        Else
            LogsComboBox.Text = "No password change logs found"
        End If
    End Sub

    Private Sub LogsComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles LogsComboBox.SelectedIndexChanged
        If IsLoading Then Exit Sub
        OpenLog(LogsComboBox.Text)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Delete(LogsComboBox.Text)

    End Sub
    Private Sub Delete(filename As String)
        If Not filename.EndsWith(".enc", StringComparison.CurrentCultureIgnoreCase) Then filename &= ".enc"

        If MsgBox(
        "Be sure to record any passwords stored in this file that may still be in use before deleting.
Thet cannot be recovered later!

Are you sure you want to delete this file now?", MsgBoxStyle.YesNo + MsgBoxStyle.Exclamation, "Delete Confirmation") = MsgBoxResult.Yes Then

            Try
                IO.File.Delete(filename)

            Catch ex As Exception
                MsgBox("Error deleting file: & " & ex.Message, MsgBoxStyle.Critical)
            End Try
        End If
        ListLogs()

    End Sub
End Class