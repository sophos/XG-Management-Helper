' Copyright 2020  Sophos Ltd.  All rights reserved.
' Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
' You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
' Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, 
' WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing 
' permissions and limitations under the License.

Imports System
Imports System.IO
Imports System.Security.Cryptography
Imports System.Text


'Largely driven by example at https://www.codeproject.com/tips/1156169/encrypt-strings-with-passwords-aes-sha
'written by APE-Germany
'License: CPOL https://www.codeproject.com/info/cpol10.aspx
Public Class AES256Wrapper
    Implements IDisposable

#Disable Warning IDE0044 ' Add readonly modifier
    Private Key As Byte()
    Private IV As Byte()
#Enable Warning IDE0044 ' Add readonly modifier
    Sub New(key As Byte(), IV As Byte())
        If key Is Nothing Then Throw New Exception("Key is required")
        If key.Length = 0 Then Throw New ArgumentNullException("Key cannot be blank")
        Me.Key = key
        Me.IV = IV
    End Sub

    Sub New(key As String, IV As String)
        If key Is Nothing Then Throw New Exception("Key is required")
        If key.Length = 0 Then Throw New ArgumentNullException("Key cannot be blank")
        Me.Key = Convert.FromBase64String(key)
        Me.IV = Convert.FromBase64String(IV)
    End Sub

    Public Function Encrypt(data As String) As String
        Return Encrypt(Key, IV, data)
    End Function

    Public Function Decrypt(data As String) As String
        If Key Is Nothing Then Throw New ArgumentNullException("Must set key at instantiation to use this method")
        Return Decrypt(Key, IV, data)
    End Function

    Public Function Encrypt(ByVal Key As Byte(), ByVal IV As Byte(), ByVal data As String) As String
        Dim EncryptedData As String

        Try
            EncryptedData = AESEncryptBytes(data, Key, IV)
            Return EncryptedData
        Catch ex As Exception
            'failures should be handled by throwing away the data, and returning an empty string       
            Return ""
        End Try

        Return ""
    End Function

    Public Function Decrypt(ByVal key As Byte(), IV As Byte(), ByVal data As String) As String
        Dim DecryptedData As String
        If data Is Nothing Then Return ""

        Try
            DecryptedData = AESDecryptBytes(data, key, IV)
            Return DecryptedData
        Catch ex As Exception
            'failures should be handled by throwing away the data, and returning an empty string
            'could happen if registry data is transported from one computer to another
            Return ""
        End Try

    End Function

    Private Shared Function AESEncryptBytes(ByVal plainText As String, ByVal Key As Byte(), ByVal IV As Byte()) As String
        If plainText Is Nothing OrElse plainText.Length <= 0 Then Return "" 'Throw New ArgumentNullException("plainText")
        If Key Is Nothing OrElse Key.Length <= 0 Then Throw New ArgumentNullException("Key")
        If IV Is Nothing OrElse IV.Length <= 0 Then Throw New ArgumentNullException("IV")
        Dim EncryptedBytes As Byte()
        '
        Using MyAES As AesManaged = New AesManaged() With {.Key = Key, .IV = IV, .Mode = CipherMode.CBC, .Padding = PaddingMode.PKCS7}
            Dim MyCryptoTransform As ICryptoTransform = MyAES.CreateEncryptor(MyAES.Key, MyAES.IV)
            Using MyMemoryStream As MemoryStream = New MemoryStream()
                Using MyCryptoStream As CryptoStream = New CryptoStream(MyMemoryStream, MyCryptoTransform, CryptoStreamMode.Write)
                    Using MyMemoryWriter As StreamWriter = New StreamWriter(MyCryptoStream)
                        MyMemoryWriter.Write(plainText)
                    End Using
                    EncryptedBytes = MyMemoryStream.ToArray()
                End Using
            End Using
        End Using
        '
        Return Convert.ToBase64String(EncryptedBytes)
    End Function



    Private Shared Function AESDecryptBytes(ByVal cipherTextString As String, ByVal Key As Byte(), ByVal IV As Byte()) As String
        Try
            Dim EncryptedBytes As Byte() = Convert.FromBase64String(cipherTextString)
            If EncryptedBytes Is Nothing OrElse EncryptedBytes.Length <= 0 Then Return "" 'Throw New ArgumentNullException("EncryptedBytes")
            If Key Is Nothing OrElse Key.Length <= 0 Then Throw New ArgumentNullException("Key")
            If IV Is Nothing OrElse IV.Length <= 0 Then Throw New ArgumentNullException("IV")
            Dim plaintext As String = Nothing
            '
            Using MyAES As Aes = Aes.Create()
                MyAES.Key = Key
                MyAES.IV = IV
                MyAES.Mode = CipherMode.CBC
                MyAES.Padding = PaddingMode.PKCS7

                Dim MyDecryptor As ICryptoTransform = MyAES.CreateDecryptor(MyAES.Key, MyAES.IV)
                Using MyMemoryStream As MemoryStream = New MemoryStream(EncryptedBytes)
                    Using MyCryptoStream As CryptoStream = New CryptoStream(MyMemoryStream, MyDecryptor, CryptoStreamMode.Read)
                        Using MyStreamReader As StreamReader = New StreamReader(MyCryptoStream)
                            plaintext = MyStreamReader.ReadToEnd()
                        End Using
                    End Using
                End Using
            End Using
            '
            Return plaintext
        Catch ex As Exception
            'MsgBox(ex.ToString & vbNewLine & vbNewLine & cipherTextString.Length)
            Return ""
        End Try
    End Function



#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        ' TODO: uncomment the following line if Finalize() is overridden above.
        ' GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
