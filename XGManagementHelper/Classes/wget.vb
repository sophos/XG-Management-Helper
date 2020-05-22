Imports System.Net.Http
Public Class HttpGet
    Public Event AsyncDownloadComplete(sender As Object, e As AsyncDownloadEventArgs)
    Public Event AsyncDownloadFail(sender As Object, e As AsyncDownloadEventArgs)
    Public Class AsyncDownloadEventArgs
        Inherits EventArgs
        Public Property URL As String
        Public Property Data As String
        Public Property Exception As Exception
        Sub New(url As String, Data As String)
            Me.URL = url
            Me.Data = Data
        End Sub
        Sub New(url As String, Exception As Exception)
            Me.URL = url
            Me.Exception = Exception
        End Sub


    End Class

    Async Sub AsyncURLDownloadAsString(URL As String)
        Using client As HttpClient = New HttpClient()
            Try

                Using response As HttpResponseMessage = Await client.GetAsync(URL)
                    Using content As HttpContent = response.Content
                        ' Get contents of page as a String.
                        Dim result As String = Await content.ReadAsStringAsync()
                        ' If data exists, print a substring.

                        If result IsNot Nothing Then
                            RaiseEvent AsyncDownloadComplete(Me, New AsyncDownloadEventArgs(URL, result))
                        End If
                    End Using
                End Using
            Catch ex As Exception
                RaiseEvent AsyncDownloadFail(Me, New AsyncDownloadEventArgs(URL, ex))
            End Try
        End Using
    End Sub


End Class
