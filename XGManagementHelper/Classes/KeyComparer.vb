' Copyright 2020  Sophos Ltd.  All rights reserved.
' Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
' You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
' Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, 
' WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing 
' permissions and limitations under the License.

Public Class KeyComparer
    Implements IEqualityComparer(Of KeyValuePair(Of String, String))

    Public Function Equals(x As KeyValuePair(Of String, String), y As KeyValuePair(Of String, String)) As Boolean Implements IEqualityComparer(Of KeyValuePair(Of String, String)).Equals
        Return x.Key.Equals(y.Key)
    End Function

    Public Function GetHashCode(obj As KeyValuePair(Of String, String)) As Integer Implements IEqualityComparer(Of KeyValuePair(Of String, String)).GetHashCode
        Return obj.GetHashCode
    End Function
End Class