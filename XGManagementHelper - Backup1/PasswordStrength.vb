Imports System
Imports System.Collections
Imports System.Data


'Method taken from https://www.codeproject.com/Articles/59186/Password-Strength-Control-2
'written by Peter Tewkesbury
'Licensed under CPOL: https://www.codeproject.com/info/cpol10.aspx

Namespace PasswordMeter
    Public Class PasswordStrength
        Private dtDetails As DataTable
        Private PreviousPassword As String = ""

        Public Sub SetPassword(ByVal pwd As String)
            If PreviousPassword <> pwd Then
                PreviousPassword = pwd
                CheckPasswordWithDetails(PreviousPassword)
            End If
        End Sub

        Public Function GetPasswordScore() As Integer
            If dtDetails IsNot Nothing Then
                Return CInt(dtDetails.Rows(0)(5))
            Else
                Return 0
            End If
        End Function

        Public Function GetPasswordStrength() As String
            If dtDetails IsNot Nothing Then
                Return CStr(dtDetails.Rows(0)(3))
            Else
                Return "Unknown"
            End If
        End Function

        Public Function GetStrengthDetails() As DataTable
            Dim ret As New DataTable
            If dtDetails Is Nothing Then Return ret

            Dim filter As String = "Bonus <> 0"
            Dim drs As DataRow() = dtDetails.Select(filter)
            If drs Is Nothing Then Return ret

            For Each dr As DataRow In drs
                ret.ImportRow(dr)
            Next



            Return ret
        End Function

        Private Sub CheckPasswordWithDetails(ByVal pwd As String)
            Dim nScore As Integer '= 0
            Dim sComplexity As String = ""
            Dim iUpperCase As Integer = 0
            Dim iLowerCase As Integer = 0
            Dim iDigit As Integer = 0
            Dim iSymbol As Integer = 0
            Dim iRepeated As Integer = 1
            Dim htRepeated As Hashtable = New Hashtable()
            Dim iMiddle As Integer = 0
            Dim iMiddleEx As Integer = 1
            Dim ConsecutiveMode As Integer = 0
            Dim iConsecutiveUpper As Integer = 0
            Dim iConsecutiveLower As Integer = 0
            Dim iConsecutiveDigit As Integer = 0
            Dim iLevel As Integer = 0
            Dim sAlphas As String = "abcdefghijklmnopqrstuvwxyz"
            Dim sNumerics As String = "01234567890"
            Dim nSeqAlpha As Integer = 0
            Dim nSeqChar As Integer = 0
            Dim nSeqNumber As Integer = 0
            CreateDetailsTable()
            Dim drScore As DataRow = AddDetailsRow(Math.Min(System.Threading.Interlocked.Increment(iLevel), iLevel - 1), "Score", "", "", 0, 0)

            For Each ch As Char In pwd.ToCharArray()

                If Char.IsDigit(ch) Then
                    iDigit += 1
                    If ConsecutiveMode = 3 Then iConsecutiveDigit += 1
                    ConsecutiveMode = 3
                End If

                If Char.IsUpper(ch) Then
                    iUpperCase += 1
                    If ConsecutiveMode = 1 Then iConsecutiveUpper += 1
                    ConsecutiveMode = 1
                End If

                If Char.IsLower(ch) Then
                    iLowerCase += 1
                    If ConsecutiveMode = 2 Then iConsecutiveLower += 1
                    ConsecutiveMode = 2
                End If

                If Char.IsSymbol(ch) OrElse Char.IsPunctuation(ch) Then
                    iSymbol += 1
                    ConsecutiveMode = 0
                End If

                If Char.IsLetter(ch) Then

                    If htRepeated.Contains(Char.ToLower(ch)) Then
                        iRepeated += 1
                    Else
                        htRepeated.Add(Char.ToLower(ch), 0)
                    End If

                    If iMiddleEx > 1 Then iMiddle = iMiddleEx - 1
                End If

                If iUpperCase > 0 OrElse iLowerCase > 0 Then
                    If Char.IsDigit(ch) OrElse Char.IsSymbol(ch) Then iMiddleEx += 1
                End If
            Next

            For s As Integer = 0 To 23 - 1
                Dim sFwd As String = sAlphas.Substring(s, 3)
                Dim sRev As String = strReverse(sFwd)

                If pwd.ToLower().IndexOf(sFwd) <> -1 OrElse pwd.ToLower().IndexOf(sRev) <> -1 Then
                    nSeqAlpha += 1
                    nSeqChar += 1
                End If
            Next

            For s As Integer = 0 To 8 - 1
                Dim sFwd As String = sNumerics.Substring(s, 3)
                Dim sRev As String = strReverse(sFwd)

                If pwd.ToLower().IndexOf(sFwd) <> -1 OrElse pwd.ToLower().IndexOf(sRev) <> -1 Then
                    nSeqNumber += 1
                    nSeqChar += 1
                End If
            Next

            AddDetailsRow(Math.Min(System.Threading.Interlocked.Increment(iLevel), iLevel - 1), "Additions", "", "", 0, 0)
            nScore = 4 * pwd.Length
            AddDetailsRow(Math.Min(System.Threading.Interlocked.Increment(iLevel), iLevel - 1), "Password Length", "Flat", "(n*4)", pwd.Length, pwd.Length * 4)

            If iUpperCase > 0 Then
                nScore += ((pwd.Length - iUpperCase) * 2)
                AddDetailsRow(Math.Min(System.Threading.Interlocked.Increment(iLevel), iLevel - 1), "Uppercase Letters", "Cond/Incr", "+((len-n)*2)", iUpperCase, ((pwd.Length - iUpperCase) * 2))
            Else
                AddDetailsRow(Math.Min(System.Threading.Interlocked.Increment(iLevel), iLevel - 1), "Uppercase Letters", "Cond/Incr", "+((len-n)*2)", iUpperCase, 0)
            End If

            If iLowerCase > 0 Then
                nScore += ((pwd.Length - iLowerCase) * 2)
                AddDetailsRow(Math.Min(System.Threading.Interlocked.Increment(iLevel), iLevel - 1), "Lowercase Letters", "Cond/Incr", "+((len-n)*2)", iLowerCase, ((pwd.Length - iLowerCase) * 2))
            Else
                AddDetailsRow(Math.Min(System.Threading.Interlocked.Increment(iLevel), iLevel - 1), "Lowercase Letters", "Cond/Incr", "+((len-n)*2)", iLowerCase, 0)
            End If

            nScore += (iDigit * 4)
            AddDetailsRow(Math.Min(System.Threading.Interlocked.Increment(iLevel), iLevel - 1), "Numbers", "Cond", "+(n*4)", iDigit, (iDigit * 4))
            nScore += (iSymbol * 6)
            AddDetailsRow(Math.Min(System.Threading.Interlocked.Increment(iLevel), iLevel - 1), "Symbols", "Flat", "+(n*6)", iSymbol, (iSymbol * 6))
            nScore += (iMiddle * 2)
            AddDetailsRow(Math.Min(System.Threading.Interlocked.Increment(iLevel), iLevel - 1), "Middle Numbers or Symbols", "Flat", "+(n*2)", iMiddle, (iMiddle * 2))
            Dim requirments As Integer = 0
            If pwd.Length >= 8 Then requirments += 1
            If iUpperCase > 0 Then requirments += 1
            If iLowerCase > 0 Then requirments += 1
            If iDigit > 0 Then requirments += 1
            If iSymbol > 0 Then requirments += 1

            If requirments > 3 Then
                nScore += (requirments * 2)
                AddDetailsRow(Math.Min(System.Threading.Interlocked.Increment(iLevel), iLevel - 1), "Requirments", "Flat", "+(n*2)", requirments, (requirments * 2))
            Else
                AddDetailsRow(Math.Min(System.Threading.Interlocked.Increment(iLevel), iLevel - 1), "Requirments", "Flat", "+(n*2)", requirments, 0)
            End If

            AddDetailsRow(Math.Min(System.Threading.Interlocked.Increment(iLevel), iLevel - 1), "Deductions", "", "", 0, 0)

            If iDigit = 0 AndAlso iSymbol = 0 Then
                nScore -= pwd.Length
                AddDetailsRow(Math.Min(System.Threading.Interlocked.Increment(iLevel), iLevel - 1), "Letters only", "Flat", "-n", pwd.Length, -pwd.Length)
            Else
                AddDetailsRow(Math.Min(System.Threading.Interlocked.Increment(iLevel), iLevel - 1), "Letters only", "Flat", "-n", 0, 0)
            End If

            If iDigit = pwd.Length Then
                nScore -= pwd.Length
                AddDetailsRow(Math.Min(System.Threading.Interlocked.Increment(iLevel), iLevel - 1), "Numbers only", "Flat", "-n", pwd.Length, -pwd.Length)
            Else
                AddDetailsRow(Math.Min(System.Threading.Interlocked.Increment(iLevel), iLevel - 1), "Numbers only", "Flat", "-n", 0, 0)
            End If

            If iRepeated > 1 Then
                nScore -= (iRepeated * (iRepeated - 1))
                AddDetailsRow(Math.Min(System.Threading.Interlocked.Increment(iLevel), iLevel - 1), "Repeat Characters (Case Insensitive)", "Incr", "-(n(n-1))", iRepeated, -(iRepeated * (iRepeated - 1)))
            End If

            nScore -= (iConsecutiveUpper * 2)
            AddDetailsRow(Math.Min(System.Threading.Interlocked.Increment(iLevel), iLevel - 1), "Consecutive Uppercase Letters", "Flat", "-(n*2)", iConsecutiveUpper, -(iConsecutiveUpper * 2))
            nScore -= (iConsecutiveLower * 2)
            AddDetailsRow(Math.Min(System.Threading.Interlocked.Increment(iLevel), iLevel - 1), "Consecutive Lowercase Letters", "Flat", "-(n*2)", iConsecutiveLower, -(iConsecutiveLower * 2))
            nScore -= (iConsecutiveDigit * 2)
            AddDetailsRow(Math.Min(System.Threading.Interlocked.Increment(iLevel), iLevel - 1), "Consecutive Numbers", "Flat", "-(n*2)", iConsecutiveDigit, -(iConsecutiveDigit * 2))
            nScore -= (nSeqAlpha * 3)
            AddDetailsRow(Math.Min(System.Threading.Interlocked.Increment(iLevel), iLevel - 1), "Sequential Letters (3+)", "Flat", "-(n*3)", nSeqAlpha, -(nSeqAlpha * 3))
            nScore -= (nSeqNumber * 3)
            AddDetailsRow(Math.Min(System.Threading.Interlocked.Increment(iLevel), iLevel - 1), "Sequential Numbers (3+)", "Flat", "-(n*3)", nSeqNumber, -(nSeqNumber * 3))

            If nScore > 100 Then
                nScore = 100
            ElseIf nScore < 0 Then
                nScore = 0
            End If

            If nScore >= 0 AndAlso nScore < 20 Then
                sComplexity = "Very Weak"
            ElseIf nScore >= 20 AndAlso nScore < 40 Then
                sComplexity = "Weak"
            ElseIf nScore >= 40 AndAlso nScore < 60 Then
                sComplexity = "Good"
            ElseIf nScore >= 60 AndAlso nScore < 80 Then
                sComplexity = "Strong"
            ElseIf nScore >= 80 AndAlso nScore <= 100 Then
                sComplexity = "Very Strong"
            End If

            drScore(5) = nScore
            drScore(3) = sComplexity
            dtDetails.AcceptChanges()
        End Sub

        Private Sub CreateDetailsTable()
            dtDetails = New DataTable("PasswordDetails")
            dtDetails.Columns.Add(New DataColumn("Level", GetType(Int32)))
            dtDetails.Columns.Add(New DataColumn("Description", GetType(String)))
            dtDetails.Columns.Add(New DataColumn("Type", GetType(String)))
            dtDetails.Columns.Add(New DataColumn("Rate", GetType(String)))
            dtDetails.Columns.Add(New DataColumn("Count", GetType(Int32)))
            dtDetails.Columns.Add(New DataColumn("Bonus", GetType(Int32)))
        End Sub

        Private Function AddDetailsRow(ByVal Id As Integer, ByVal Description As String, ByVal Type As String, ByVal Rate As String, ByVal Count As Integer, ByVal Bouns As Integer) As DataRow
            Dim dr As DataRow = dtDetails.NewRow()
            dr(0) = Id
            dr(1) = Description
            dr(2) = Type
            dr(3) = Rate
            dr(4) = Count
            dr(5) = Bouns
            dtDetails.Rows.Add(dr)
            dtDetails.AcceptChanges()
            Return dr
        End Function

        'Private Function sReverse(ByVal str As String) As String
        '    Dim newstring As String = ""

        '    For s As Integer = 0 To str.Length - 1
        '        newstring = str(s) & newstring
        '    Next

        '    Return newstring
        'End Function
    End Class
End Namespace
