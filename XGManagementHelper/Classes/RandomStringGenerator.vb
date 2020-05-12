Imports System.Security.Cryptography

Public Class Randomness

    Private Const ExtendedAllowedChars As String = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-=+!@#$%^&*()"
    Private Const AllowedChars As String = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"

    Public Shared Function GetRandomString(Length As Integer, UseExtendedCharacters As Boolean) As String
        Dim randomess(Length - 1) As UInt64
        Dim UseChars As String = If(UseExtendedCharacters, ExtendedAllowedChars, AllowedChars)

        Dim rslt As String = ""
        For count As Integer = 0 To Length - 1

            Dim pos As Integer = GetRandomInteger(0, UseChars.Length - 1)
            rslt &= UseChars.Chars(pos)
        Next

        Return rslt
    End Function

    Public Shared Function GetRandomBytes(length As Integer) As Byte()
        Using CryptoRNG As RandomNumberGenerator = RandomNumberGenerator.Create()
            Dim retBytes(length - 1) As Byte
            CryptoRNG.GetNonZeroBytes(retBytes)
            Return retBytes
        End Using
    End Function

    Public Shared Function GetRandomInteger() As Integer
        'Dim retBytes(3) As Byte
        Using CryptoRNG As RandomNumberGenerator = RandomNumberGenerator.Create()
            Dim retBytes As Byte() = {0, 0, 0, 0}
            CryptoRNG.GetNonZeroBytes(retBytes)
            Return BitConverter.ToInt32(retBytes, 0)
        End Using

    End Function

    Public Shared Function GetRandomUInt32() As UInt32
        Using CryptoRNG As RandomNumberGenerator = RandomNumberGenerator.Create()
            Dim retBytes As Byte() = {0, 0, 0, 0}
            CryptoRNG.GetNonZeroBytes(retBytes)
            Return BitConverter.ToUInt32(retBytes, 0)
        End Using
    End Function

    Public Shared Function GetRandomInteger(FromInclusive As Integer, ToInclusive As Integer) As Integer
        'This function calculates the largest acceptable range of random numbers to accept, 
        'so that a (RandomInt Mod Range) does not favor any possible result. 
        '
        'It throws out any generated Random numbers outside of that range, that would introduce bias in the result.
        '
        'To minimize discards needed, the largest possible range is selected, based on a multiple of the size of the target range.
        'The calculation "Maxval - (Maxval Mod RangeOfPossibleResultsFromZero) - 1" ensures 

        If FromInclusive >= ToInclusive Then Throw New ArgumentException("FromInclusive must be less than ToInclusive")
        Dim RangeOfPossibleResultsFromZero As UInt32 = ToInclusive - FromInclusive + 1
        Dim Maxval As UInt32 = UInt32.MaxValue
        Dim AccepatbleRange As UInt32 = Maxval - (Maxval Mod RangeOfPossibleResultsFromZero) - 1
        Dim RandomInt As UInt32 = GetRandomUInt32()

        'get the next random number in 
        Dim counter As Integer = 0
        Do
            If RandomInt <= AccepatbleRange Then Exit Do
            Debug.Print("threw out {0}, is > {1}", RandomInt, AccepatbleRange)
            counter += 1
            If counter >= 1000 Then Throw New Exception("Something has gone seriously wrong")
        Loop
        Return (RandomInt Mod RangeOfPossibleResultsFromZero) + FromInclusive


    End Function

    Public Shared Sub TestRandomnessLoop(Max As Integer, iterations As Integer, loops As Integer)
        For x As Integer = 1 To loops
            Debug.Print(TestRandomness(Max, iterations))
        Next
    End Sub

    Public Shared Function TestRandomness(Max As Integer, iterations As Integer) As String
        'test results between 0 and max over 
        Dim ResultSummary(Max) As Double
        Dim ResultDetails(iterations - 1) As Double
        For x As Integer = 1 To iterations
            Dim val As Integer = GetRandomInteger(0, Max)
            ResultSummary(val) += 1
            ResultDetails(x - 1) = val
        Next

        Dim mincount As Integer = iterations
        Dim maxcount As Integer = 0
        Dim leastPopular As New List(Of Integer)
        Dim mostPopular As New List(Of Integer)
        For x As Integer = 0 To Max
            If ResultSummary(x) > maxcount Then
                maxcount = ResultSummary(x)
                mostPopular.Clear()
                mostPopular.Add(x)
            ElseIf ResultSummary(x) = maxcount Then
                mostPopular.Add(x)
            End If
            If ResultSummary(x) < mincount Then
                mincount = ResultSummary(x)
                leastPopular.Clear()
                leastPopular.Add(x)
            ElseIf ResultSummary(x) = mincount Then
                leastPopular.Add(x)
            End If

            'Debug.Print("""{0}"", ""{1}""", x, ResultSummary(x))
        Next

        'Standard Deviation
        Dim average As Double = ResultSummary.Average()
        Dim sd As Double = Math.Sqrt((ResultSummary.[Select](Function(val) (val - average) ^ 2).Sum()) / ResultSummary.Length)

        ''Birthday
        'For x As Integer = 0 To Max
        '    Dim Indexes As New List(Of Double)
        '    Dim lastindex As Integer = 0
        '    Dim idx As Integer = 0
        '    For idx = 0 To iterations - 1
        '        If ResultDetails(idx) = x Then
        '            Indexes.Add(idx - lastindex)
        '            lastindex = idx
        '        End If
        '    Next
        '    Dim freq As Double = Indexes.Average
        '    Dim freqsd As Double = Math.Sqrt((Indexes.[Select](Function(val) (val - freq) ^ 2).Sum()) / Indexes.Count)
        '    Debug.Print("Value: {0} Occurrences: {1} avg ocurence distance: {2}, occurrence std.dev.:{3}", x, Indexes.Count, freq, freqsd)
        'Next

        Return String.Format("Iterations: {6}, range 0-{7}, Lowest hit count: {0}, highest hit count: {1}, average: {2}, standard deviation: {3}, least popular: '{4}' most popular: '{5}'", mincount, maxcount, average, sd, String.Join(", ", leastPopular), String.Join(",", mostPopular), iterations, Max)

    End Function

End Class
