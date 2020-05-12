' Copyright 2020  Sophos Ltd.  All rights reserved.
' Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
' You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
' Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, 
' WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing 
' permissions and limitations under the License.

Imports System
Imports System.Security.Cryptography
Imports System.Text

Public Class PBKDF2


    Public Shared Function CryptDeriveKey(Password As String, Salt As String, Iterations As Integer) As Byte()
        Dim PasswordBytes As Byte() = Encoding.Unicode.GetBytes(Password)
        Dim SaltBytes As Byte() = Convert.FromBase64String(Salt)
        Dim RetKey As Byte() = {}
        Try
            Dim PDB As Rfc2898DeriveBytes = New Rfc2898DeriveBytes(PasswordBytes, SaltBytes, Iterations)
            RetKey = PDB.GetBytes(32)
        Catch e As Exception
            Debug.Print(e.ToString)
            Return Nothing
        Finally
            ClearBytes(PasswordBytes)
            ClearBytes(SaltBytes)
        End Try
        Return RetKey
    End Function

    Public Shared Sub ClearBytes(ByVal buffer As Byte())
        If buffer Is Nothing Then
            Throw New ArgumentException("buffer")
        End If

        For x As Integer = 0 To buffer.Length - 1
            buffer(x) = 0
        Next
    End Sub

End Class
