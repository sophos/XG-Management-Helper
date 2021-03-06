﻿' Copyright 2020  Sophos Ltd.  All rights reserved.
' Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
' You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
' Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, 
' WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing 
' permissions and limitations under the License.

Public Class LogViewer

    Private IsLoading As Boolean
    ReadOnly Property LogLocation As String
    Sub New(LogLocation As String)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.        
        Me.LogLocation = LogLocation
        ListLogs()
    End Sub

    Private Sub OpenLog(filename As String)
        If Not filename.EndsWith(".enc", StringComparison.CurrentCultureIgnoreCase) Then filename &= ".enc"
        Dim filepath As String = filename
        If Not filename.StartsWith(LogLocation) Then filepath = IO.Path.Combine(LogLocation, filename)
        If Not IO.File.Exists(filepath) Then
            LogMessagesTextBox.Text = ""
            Exit Sub
        End If

        Dim CipherText As String = IO.File.ReadAllText(filepath)
        LogMessagesTextBox.Text = EncryptionHelper.EncryptedFileRead(filepath)
    End Sub

    Public Sub ShowLog(filename As String)
        IsLoading = True
        Dim filepath As String = filename

        If filepath.ToLower.EndsWith(".enc") Then
            LogsComboBox.Text = IO.Path.GetFileNameWithoutExtension(filename)
        Else
            LogsComboBox.Text = filename
        End If

        OpenLog(filepath & ".enc")
        IsLoading = False
    End Sub

    Private Sub ListLogs()
        Dim files As String() = System.IO.Directory.GetFiles(LogLocation, "*.enc")
        LogsComboBox.Items.Clear()

        For Each file As String In files
            LogsComboBox.Items.Add(IO.Path.GetFileNameWithoutExtension(file))
        Next

        If LogsComboBox.Items.Count > 0 Then
            LogsComboBox.Text = LogsComboBox.Items(LogsComboBox.Items.Count - 1)
        Else
            LogsComboBox.Text = "No password change logs found"
            LogMessagesTextBox.Clear()
        End If
    End Sub

    Private Sub LogsComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles LogsComboBox.SelectedIndexChanged
        If IsLoading Then Exit Sub
        OpenLog(LogsComboBox.Text)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Delete(LogsComboBox.Text)

    End Sub

    Private Sub Delete(ByVal filename As String)
        If Not filename.EndsWith(".enc", StringComparison.CurrentCultureIgnoreCase) Then filename &= ".enc"
        filename = IO.Path.Combine(LogLocation, filename)
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