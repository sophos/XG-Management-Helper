﻿' Copyright 2020  Sophos Ltd.  All rights reserved.
' Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
' You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
' Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, 
' WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing 
' permissions and limitations under the License.

Imports System.IO
Imports System.IO.Compression
Public Class FileHelpers
    Public Shared Sub OpenFileWithDefaultApplication(Filename As String)
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
