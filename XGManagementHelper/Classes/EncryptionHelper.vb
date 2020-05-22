' Copyright 2020  Sophos Ltd.  All rights reserved.
' Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
' You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
' Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, 
' WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing 
' permissions and limitations under the License.

Imports Microsoft.Win32

Public Class EncryptionHelper
    Partial Public Class SecureRegistryKey
        Implements IDisposable
        ReadOnly Property ThisKey As RegistryKey
        ReadOnly Property AESWrapper As AES256Wrapper

        ReadOnly Property CanEncrypt As Boolean
            Get
                Return AESWrapper IsNot Nothing
            End Get
        End Property

        Sub New(RegistryKey As RegistryKey, AESWrapper As AES256Wrapper)
            Me.ThisKey = RegistryKey
            Me.AESWrapper = AESWrapper
        End Sub

#Region "Properties"
        ReadOnly Property SubKeyCount As Integer
            Get
                Return ThisKey.SubKeyCount
            End Get
        End Property
        ReadOnly Property ValueCount As Integer
            Get
                Return ThisKey.ValueCount
            End Get
        End Property
        ReadOnly Property Name As String
            Get
                Return ThisKey.Name
            End Get
        End Property
        ReadOnly Property View As RegistryView
            Get
                Return ThisKey.View
            End Get
        End Property
#End Region

#Region "Gets"
        Public Function GetValue(name As String) As Object
            Return ThisKey.GetValue(name)
        End Function
        Public Function GetValue(name As String, defaultValue As Object) As Object
            Return ThisKey.GetValue(name, defaultValue)
        End Function
        Public Function GetValue(name As String, defaultValue As Object, options As RegistryOptions) As Object
            Return ThisKey.GetValue(name, defaultValue, options)
        End Function

        Public Function GetSecureValue(name As String) As Object
            If Not CanEncrypt Then Throw New Exception("Cannot use secure functions without key data")
            Return AESWrapper.Decrypt(ThisKey.GetValue(name))
        End Function
        Public Function GetSecureValue(name As String, defaultValue As Object) As Object
            If Not CanEncrypt Then Throw New Exception("Cannot use secure functions without key data")
            If ThisKey.GetValueNames.Contains(name) Then
                Return AESWrapper.Decrypt(ThisKey.GetValue(name))
            End If
            Return defaultValue
        End Function
        Public Function GetSecureValue(name As String, Key As String, IV As String) As Object
            Dim AESWrapper As New AES256Wrapper(Key, IV)
            Return AESWrapper.Decrypt(ThisKey.GetValue(name))
        End Function
        Public Function GetSecureValue(name As String, defaultValue As Object, Key As String, IV As String) As Object
            Dim AESWrapper As New AES256Wrapper(Key, IV)
            If ThisKey.GetValueNames.Contains(name) Then
                Return AESWrapper.Decrypt(ThisKey.GetValue(name))
            End If
            Return defaultValue
        End Function
        Public Function GetValueKind(name As String) As RegistryValueKind
            Return ThisKey.GetValueKind(name)
        End Function

        Public Function GetValueNames() As String()
            Return ThisKey.GetValueNames
        End Function

        Public Function GetSubKeyNames() As String()
            Return ThisKey.GetSubKeyNames
        End Function

        Public Function GetAccessControl() As Security.AccessControl.RegistrySecurity
            Return ThisKey.GetAccessControl()
        End Function
        Public Function GetAccessControl(includeSections As Security.AccessControl.AccessControlSections) As Security.AccessControl.RegistrySecurity
            Return ThisKey.GetAccessControl(includeSections)
        End Function
#End Region

#Region "sets"
        Public Sub SetValue(name As String, value As Object)
            ThisKey.SetValue(name, value)
        End Sub
        Public Sub SetValue(name As String, value As Object, valueKind As RegistryValueKind)
            ThisKey.SetValue(name, value, valueKind)
        End Sub

        Public Sub SetSecureValue(name As String, value As Object)
            If Not CanEncrypt Then Throw New Exception("Cannot use secure functions without key data")
            ThisKey.SetValue(name, AESWrapper.Encrypt(value))
        End Sub
        Public Sub SetSecureValue(name As String, value As Object, Key As String, IV As String)
            Dim AESWrapper As New AES256Wrapper(Key, IV)
            ThisKey.SetValue(name, AESWrapper.Encrypt(value))
        End Sub

        Public Sub SetAccessControl(registrySecurity As Security.AccessControl.RegistrySecurity)
            ThisKey.SetAccessControl(registrySecurity)
        End Sub

#End Region

#Region "Deletes"
        Public Sub DeleteValue(name As String)
            Try
                ThisKey.DeleteValue(name)
            Catch ex As Exception

            End Try

        End Sub
        Public Sub DeleteValue(name As String, throwOnMissingValue As Boolean)
            ThisKey.DeleteValue(name, throwOnMissingValue)
        End Sub

        Public Sub DeleteSubKey(name As String)
            ThisKey.DeleteSubKey(name)
        End Sub
        Public Sub DeleteSubKey(name As String, throwOnMissingValue As Boolean)
            ThisKey.DeleteSubKey(name, throwOnMissingValue)
        End Sub

        Public Sub DeleteSubKeyTree(name As String)
            ThisKey.DeleteSubKeyTree(name)
        End Sub
        Public Sub DeleteSubKeyTree(name As String, throwOnMissingValue As Boolean)
            ThisKey.DeleteSubKeyTree(name, throwOnMissingValue)
        End Sub
#End Region

#Region "Open/Close"
        Public Function OpenSubKey(subKey As String) As SecureRegistryKey
            Return New SecureRegistryKey(ThisKey.OpenSubKey(subKey), AESWrapper)
        End Function
        Public Function OpenSubKey(subKey As String, writable As Boolean) As SecureRegistryKey
            Return New SecureRegistryKey(ThisKey.OpenSubKey(subKey, writable), AESWrapper)
        End Function
        Public Function OpenSubKey(subKey As String, permissionCheck As RegistryKeyPermissionCheck) As SecureRegistryKey
            Return New SecureRegistryKey(ThisKey.OpenSubKey(subKey, permissionCheck), AESWrapper)
        End Function
        Public Function OpenSubKey(subKey As String, rights As Security.AccessControl.RegistryRights) As SecureRegistryKey
            Return New SecureRegistryKey(ThisKey.OpenSubKey(subKey, rights), AESWrapper)
        End Function
        Public Function OpenSubKey(subKey As String, permissionCheck As RegistryKeyPermissionCheck, rights As Security.AccessControl.RegistryRights) As SecureRegistryKey
            Return New SecureRegistryKey(ThisKey.OpenSubKey(subKey, permissionCheck, rights), AESWrapper)
        End Function
        Public Sub Close()
            ThisKey.Close()
        End Sub
#End Region

        Public Overloads Function Equals(obj As SecureRegistryKey) As Boolean
            Return Me.Equals(obj)
        End Function
        Public Overrides Function Equals(obj As Object) As Boolean
            Return ThisKey.Equals(obj)
        End Function

#Region "create"
        Public Function CreateSubKey(subKey As String) As SecureRegistryKey
            Return New SecureRegistryKey(ThisKey.CreateSubKey(subKey), AESWrapper)
        End Function
        Public Function CreateSubKey(subKey As String, writable As Boolean) As SecureRegistryKey
            Return New SecureRegistryKey(ThisKey.CreateSubKey(subKey, writable), AESWrapper)
        End Function
        Public Function CreateSubKey(subKey As String, permissionCheck As RegistryKeyPermissionCheck) As SecureRegistryKey
            Return New SecureRegistryKey(ThisKey.CreateSubKey(subKey, permissionCheck), AESWrapper)
        End Function
        Public Function CreateSubKey(subKey As String, writable As Boolean, options As RegistryOptions) As SecureRegistryKey
            Return New SecureRegistryKey(ThisKey.CreateSubKey(subKey, writable, options), AESWrapper)
        End Function
        Public Function CreateSubKey(subKey As String, permissionCheck As RegistryKeyPermissionCheck, options As RegistryOptions) As SecureRegistryKey
            Return New SecureRegistryKey(ThisKey.CreateSubKey(subKey, permissionCheck, options), AESWrapper)
        End Function
        Public Function CreateSubKey(subKey As String, permissionCheck As RegistryKeyPermissionCheck, registrySecurity As Security.AccessControl.RegistrySecurity) As SecureRegistryKey
            Return New SecureRegistryKey(ThisKey.CreateSubKey(subKey, permissionCheck, registrySecurity), AESWrapper)
        End Function
        Public Function CreateSubKey(subKey As String, permissionCheck As RegistryKeyPermissionCheck, registryOptions As RegistryOptions, registrySecurity As Security.AccessControl.RegistrySecurity) As SecureRegistryKey
            Return New SecureRegistryKey(ThisKey.CreateSubKey(subKey, permissionCheck, registryOptions, registrySecurity), AESWrapper)
        End Function

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects).
                End If

                ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                ' TODO: set large fields to null.
            End If
            disposedValue = True
        End Sub

        ' TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
        'Protected Overrides Sub Finalize()
        '    ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            ' TODO: uncomment the following line if Finalize() is overridden above.
            ' GC.SuppressFinalize(Me)
        End Sub
#End Region


#End Region

    End Class

    Public Shared Property RootKey As RegistryKey = My.Computer.Registry.CurrentUser
    Public Shared Property BasePath As String = "Software\XGMigrationHelper"
    Public Shared DataKey As String
    Public Shared DataIV As String

#Region "Get keys"

    Public Shared Function GetCurrentUserKey() As SecureRegistryKey
        Dim wrapper As AES256Wrapper = Nothing
        Try
            wrapper = New AES256Wrapper(DataKey, DataIV)
        Catch ex As Exception

        End Try
        Return New SecureRegistryKey(RootKey, wrapper)
    End Function

    Public Shared Function GetApplicationRootKey() As SecureRegistryKey

        Return GetCurrentUserKey.CreateSubKey(BasePath)
    End Function

    Public Shared Function GetHostsKey() As SecureRegistryKey
        Return GetApplicationRootKey.CreateSubKey("Hosts")
    End Function

    Public Shared Function GetHostKey(host As String) As SecureRegistryKey
        If host.Length > 0 Then
            Return GetHostsKey.CreateSubKey("Host-" & host)
        Else
            Return Nothing
        End If

    End Function

    Public Shared Sub EncryptedFileWrite(filename As String, text As String)
        Try
            System.IO.File.WriteAllText(filename, New AES256Wrapper(DataKey, DataIV).Encrypt(text))
        Catch ex As Exception
            Debug.Print(ex.ToString)
        End Try

    End Sub

    Public Shared Function EncryptedFileRead(filename As String) As String
        Try
            Return New AES256Wrapper(DataKey, DataIV).Decrypt(System.IO.File.ReadAllText(filename))
        Catch ex As Exception
            Debug.Print(ex.ToString)
        End Try
        Return ""
    End Function

#End Region


End Class
