
Imports System.IO
Imports System.IO.Compression
Public Class FileHelpers
    Public Shared Sub OpenFile(Filename As String)
        Try
            System.Diagnostics.Process.Start(Filename)
        Catch ex As Exception
            MsgBox("Unable to load the log file.")
        End Try
    End Sub
    Public Enum Overwrite
        Always
        WithNewer
        Never
        pu
    End Enum

    Public Shared Function MakeUniqueFilename(filename As String) As String

        If Not File.Exists(filename) Then Return filename

        Dim baseFilename As String = IO.Path.GetFileNameWithoutExtension(filename)
        Dim Extension As String = IO.Path.GetExtension(filename)

        Dim idx As Integer = 1

        Dim newfilename As String = String.Format("{0} ({1}){2}", baseFilename, idx, Extension)
        Do While File.Exists(newfilename)
            idx += 1
            newfilename = String.Format("{0} ({1}){2}", baseFilename, idx, Extension)
        Loop

        Return newfilename

    End Function





End Class
