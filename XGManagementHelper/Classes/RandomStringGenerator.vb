Imports System.Security.Cryptography

Public Class RandomStringGenerator
    Shared ReadOnly CryptoRandom As RandomNumberGenerator = RandomNumberGenerator.Create()
    Private Const ExtendedAllowedChars As String = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-=+!@#$%^&*()"
    Private Const AllowedChars As String = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"
    Public Shared Function RandomString(Length As Integer, UseExtendedCharacters As Boolean) As String
        Dim randomess(Length - 1) As UInt64
        Dim UseChars As String = If(UseExtendedCharacters, ExtendedAllowedChars, AllowedChars)

        For count As Integer = 0 To Length - 1
            Dim bytes(7) As Byte
            CryptoRandom.GetNonZeroBytes(bytes)
            randomess(count) = BitConverter.ToUInt64(bytes, 0)
        Next

        'minimze randomness skew by modding uint64 values
        Dim rslt As String = ""
        For Each number As UInt64 In randomess
            Dim pos As Integer = number Mod (UseChars.Length)
            rslt &= UseChars.Chars(pos)
        Next

        Return rslt
    End Function
End Class
