Public Class KeyComparer
    Implements IEqualityComparer(Of KeyValuePair(Of String, String))

    Public Function Equals(x As KeyValuePair(Of String, String), y As KeyValuePair(Of String, String)) As Boolean Implements IEqualityComparer(Of KeyValuePair(Of String, String)).Equals
        Return x.Key.Equals(y.Key)
    End Function

    Public Function GetHashCode(obj As KeyValuePair(Of String, String)) As Integer Implements IEqualityComparer(Of KeyValuePair(Of String, String)).GetHashCode
        Return obj.GetHashCode
    End Function
End Class