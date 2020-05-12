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

    Public Shared Function CreateRandomSalt(ByVal length As Integer) As Byte()
        Dim randBytes As Byte()

        If length >= 1 Then
            randBytes = New Byte(length - 1) {}
        Else
            randBytes = New Byte(0) {}
        End If

        Dim rand As RNGCryptoServiceProvider = New RNGCryptoServiceProvider()
        rand.GetBytes(randBytes)
        Return randBytes
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
