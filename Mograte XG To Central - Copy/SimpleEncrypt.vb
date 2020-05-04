Imports System.Security.Cryptography

Public NotInheritable Class Hider
    Private Key As String

    Private Function TruncateHash(ByVal key As String, ByVal length As Integer) As Byte()

        Dim sha1 As New SHA1CryptoServiceProvider

        ' Hash the key.
        Dim keyBytes() As Byte =
        System.Text.Encoding.Unicode.GetBytes(key)
        Dim hash() As Byte = sha1.ComputeHash(keyBytes)

        ' Truncate or pad the hash.
        ReDim Preserve hash(length - 1)
        Return hash
    End Function

    Sub New(ByVal key As String)
        Me.Key = key
        ' Initialize the crypto provider.
        TripleDes.Key = TruncateHash(key, TripleDes.KeySize \ 8)
        TripleDes.IV = TruncateHash("", TripleDes.BlockSize \ 8)
    End Sub

    Private TripleDes As New TripleDESCryptoServiceProvider
    'Public Function EncryptData(ByVal plaintext As String) As String
    '    Try


    '        ' Convert the plaintext string to a byte array.
    '        Dim plaintextBytes() As Byte = System.Text.Encoding.Unicode.GetBytes(plaintext)

    '        ' Create the stream.
    '        Dim ms As New System.IO.MemoryStream
    '        ' Create the encoder to write to the stream.
    '        Dim encStream As New CryptoStream(ms, TripleDes.CreateEncryptor(), System.Security.Cryptography.CryptoStreamMode.Write)

    '        ' Use the crypto stream to write the byte array to the stream.
    '        encStream.Write(plaintextBytes, 0, plaintextBytes.Length)
    '        encStream.FlushFinalBlock()

    '        ' Convert the encrypted stream to a printable string.
    '        Return Convert.ToBase64String(ms.ToArray)
    '    Catch ex As Exception
    '        Return ""
    '    End Try
    'End Function
    'Public Function DecryptData(ByVal encryptedtext As String) As String
    '    If encryptedtext.Length = 0 Then Return ""
    '    Try

    '        Dim TripleDes As New TripleDESCryptoServiceProvider
    '        ' Convert the encrypted text string to a byte array.
    '        Dim encryptedBytes() As Byte = Convert.FromBase64String(encryptedtext)

    '        ' Create the stream.
    '        Dim ms As New System.IO.MemoryStream
    '        ' Create the decoder to write to the stream.
    '        Dim decStream As New CryptoStream(ms, TripleDes.CreateDecryptor(), System.Security.Cryptography.CryptoStreamMode.Write)

    '        ' Use the crypto stream to write the byte array to the stream.
    '        decStream.Write(encryptedBytes, 0, encryptedBytes.Length)
    '        decStream.FlushFinalBlock()

    '        ' Convert the plaintext stream to a string.
    '        Return System.Text.Encoding.Unicode.GetString(ms.ToArray)
    '    Catch ex As Exception
    '        Return ""
    '    End Try
    'End Function

    Public Function EncryptData(ByVal input As String) As String
        Dim AES As New System.Security.Cryptography.RijndaelManaged
        Dim Hash_AES As New System.Security.Cryptography.MD5CryptoServiceProvider
        Dim encrypted As String = ""
        Try
            Dim hash(31) As Byte
            Dim temp As Byte() = Hash_AES.ComputeHash(System.Text.ASCIIEncoding.ASCII.GetBytes(Key))
            Array.Copy(temp, 0, hash, 0, 16)
            Array.Copy(temp, 0, hash, 15, 16)
            AES.Key = hash
            AES.Mode = Security.Cryptography.CipherMode.ECB
            Dim DESEncrypter As System.Security.Cryptography.ICryptoTransform = AES.CreateEncryptor
            Dim Buffer As Byte() = System.Text.ASCIIEncoding.ASCII.GetBytes(input)
            encrypted = Convert.ToBase64String(DESEncrypter.TransformFinalBlock(Buffer, 0, Buffer.Length))
            Return encrypted
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Public Function DecryptData(ByVal input As String) As String
        Dim AES As New System.Security.Cryptography.RijndaelManaged
        Dim Hash_AES As New System.Security.Cryptography.MD5CryptoServiceProvider
        Dim decrypted As String = ""
        Try
            Dim hash(31) As Byte
            Dim temp As Byte() = Hash_AES.ComputeHash(System.Text.ASCIIEncoding.ASCII.GetBytes(Key))
            Array.Copy(temp, 0, hash, 0, 16)
            Array.Copy(temp, 0, hash, 15, 16)
            AES.Key = hash
            AES.Mode = Security.Cryptography.CipherMode.ECB
            Dim DESDecrypter As System.Security.Cryptography.ICryptoTransform = AES.CreateDecryptor
            Dim Buffer As Byte() = Convert.FromBase64String(input)
            decrypted = System.Text.ASCIIEncoding.ASCII.GetString(DESDecrypter.TransformFinalBlock(Buffer, 0, Buffer.Length))
            Return decrypted
        Catch ex As Exception
            Return ""
        End Try
    End Function

End Class
