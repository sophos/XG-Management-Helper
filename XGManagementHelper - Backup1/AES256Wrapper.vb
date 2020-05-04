Imports System
Imports System.IO
Imports System.Security.Cryptography
Imports System.Text


'Largely driven by example at https://www.codeproject.com/tips/1156169/encrypt-strings-with-passwords-aes-sha
'written by APE-Germany
'License: CPOL https://www.codeproject.com/info/cpol10.aspx
Class AES256Wrapper
#Disable Warning IDE0044 ' Add readonly modifier
    Private Key As String
#Enable Warning IDE0044 ' Add readonly modifier

    Sub New(key As String)
        If key Is Nothing Then Throw New ArgumentNullException("Key is required")
        If key.Length = 0 Then Throw New ArgumentNullException("Key cannot be blank")
        Me.Key = key
    End Sub

    Public Function Encrypt(data As String) As String
        Return Encrypt(Key, data)
    End Function

    Public Function Decrypt(data As String) As String
        If Key Is Nothing Then Throw New ArgumentNullException("Must set key at instantiation to use this method")
        Return Decrypt(Key, data)
    End Function

    Public Function Encrypt(ByVal key As String, ByVal data As String) As String
        Dim EncryptedData As String
        Dim keys As Byte()() = GetHashKeys(key)
        Try
            EncryptedData = AESEncryptBytes(data, keys(0), keys(1))
        Catch ex As Exception
            'failures should be handled by throwing away the data, and returning an empty string       
            Return ""
        End Try

        Return EncryptedData
    End Function

    Public Function Decrypt(ByVal key As String, ByVal data As String) As String
        Dim DecryptedData As String
        Dim Keys As Byte()() = GetHashKeys(key)
        If data Is Nothing Then Return ""
        Try
            DecryptedData = AESDecryptBytes(data, keys(0), keys(1))
        Catch ex As Exception
            'failures should be handled by throwing away the data, and returning an empty string
            'could happen if registry data is transported from one computer to another
            Return ""
        End Try

        Return DecryptedData
    End Function

    Private Shared Function AESEncryptBytes(ByVal plainText As String, ByVal Key As Byte(), ByVal IV As Byte()) As String
        If plainText Is Nothing OrElse plainText.Length <= 0 Then Throw New ArgumentNullException("plainText")
        If Key Is Nothing OrElse Key.Length <= 0 Then Throw New ArgumentNullException("Key")
        If IV Is Nothing OrElse IV.Length <= 0 Then Throw New ArgumentNullException("IV")
        Dim EncryptedBytes As Byte()
        '
        Using MyAES As AesManaged = New AesManaged() With {.Key = Key, .IV = IV}
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
            If EncryptedBytes Is Nothing OrElse EncryptedBytes.Length <= 0 Then Throw New ArgumentNullException("EncryptedBytes")
            If Key Is Nothing OrElse Key.Length <= 0 Then Throw New ArgumentNullException("Key")
            If IV Is Nothing OrElse IV.Length <= 0 Then Throw New ArgumentNullException("IV")
            Dim plaintext As String = Nothing
            '
            Using MyAES As Aes = Aes.Create()
                MyAES.Key = Key
                MyAES.IV = IV
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
            'MsgBox(ex.ToString)
        End Try
    End Function

    Private Function GetHashKeys(ByVal key As String) As Byte()()
        Dim Result As Byte()() = New Byte(1)() {}
        Dim Hasher As SHA256 = New SHA256CryptoServiceProvider()
        Dim Encoder As Encoding = Encoding.UTF8
        Dim IVBytes As Byte() = Encoder.GetBytes(key)
        Dim KeyBytes As Byte() = Encoder.GetBytes(key)
        Dim HashedIV As Byte() = Hasher.ComputeHash(IVBytes)
        Dim HashedKey As Byte() = Hasher.ComputeHash(KeyBytes)
        '
        Array.Resize(HashedIV, 16)
        Result(0) = HashedKey
        Result(1) = HashedIV
        '
        Return Result
    End Function

End Class
