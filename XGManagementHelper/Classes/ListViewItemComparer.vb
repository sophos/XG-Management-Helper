' Copyright 2020  Sophos Ltd.  All rights reserved.
' Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
' You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
' Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, 
' WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing 
' permissions and limitations under the License.

Public Class ListViewItemComparer

    Implements IComparer

    ReadOnly col As Integer
    ReadOnly order As SortOrder

    Public Sub New()
        col = 0
        order = SortOrder.Ascending
    End Sub

    Public Sub New(column As Integer, order As SortOrder)
        col = column
        Me.order = order
    End Sub

    Public Function Compare(x As Object, y As Object) As Integer Implements System.Collections.IComparer.Compare

        Dim returnVal As Integer '= -1

        Try

            ' Attempt to parse the two objects as DateTime
            Dim firstDate As System.DateTime = DateTime.Parse(CType(x, ListViewItem).SubItems(col).Text)
            Dim secondDate As System.DateTime = DateTime.Parse(CType(y, ListViewItem).SubItems(col).Text)

            ' Compare as date
            returnVal = DateTime.Compare(firstDate, secondDate)

        Catch ex As Exception

            ' If date parse failed then fall here to determine if objects are numeric
            If IsNumeric(CType(x, ListViewItem).SubItems(col).Text) And
                IsNumeric(CType(y, ListViewItem).SubItems(col).Text) Then

                ' Compare as numeric
                returnVal = Val(CType(x, ListViewItem).SubItems(col).Text).CompareTo(Val(CType(y, ListViewItem).SubItems(col).Text))

            Else
                ' If not numeric then compare as string
                returnVal = [String].Compare(CType(x, ListViewItem).SubItems(col).Text, CType(y, ListViewItem).SubItems(col).Text)
            End If

        End Try

        ' If order is descending then invert value
        If order = SortOrder.Descending Then
            returnVal *= -1
        End If

        Return returnVal

    End Function

End Class