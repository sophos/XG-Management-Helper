' Copyright 2020  Sophos Ltd.  All rights reserved.
' Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
' You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
' Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, 
' WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing 
' permissions and limitations under the License.

Imports System.Management

Public Class SecurityCheck
    Class AntiVirusProduct
        Public instanceGuid As String
        Public displayName As String
        Public pathToSignedProductExe As String
        Public pathToSignedReportingExe As String
        Public productState As String
        Public timestamp As String

        'Works on:
        'Windows 10	Yes
        'Windows 8	Yes
        'Windows 7	Yes
        'Windows Vista	Yes
        'Windows XP	Yes
        'Windows Server 2012	No
        'Windows Server 2012 R2	No
        'Windows Server 2008 R2	No
        'Windows Server 2008	No
        'Windows Server 2003	No
        'Windows Server 2016	No
        'Windows 2000	No

        Public Function RealtimeProtectionON() As Boolean
            Select Case productState.Substring(2, 2)
                Case "00" 'RealTimProtectionStates.OFF

                    Return False
                Case "01" 'RealTimProtectionStates.EXPIRED
                    Return False

                Case "10" 'RealTimProtectionStates.On
                    Return True

                Case "11" ' RealTimProtectionStates.EXPIRED
                    Return False

                Case Else 'RealTimProtectionStates.UNKNOWN
                    Return False

            End Select
        End Function

        Public Function SignaturesUpToDate() As Boolean
            Select Case productState.Substring(4, 2)
                Case "00"
                    'up to date
                    Return True
                Case "10"
                    'out of date
                    Return False
                Case Else
                    'unknown
                    Return False
            End Select
        End Function

        Sub New(viruschecker As ManagementObject)
            Me.instanceGuid = viruschecker("instanceGuid")
            Me.displayName = viruschecker("displayName")
            Me.pathToSignedProductExe = viruschecker("pathToSignedProductExe")
            Me.pathToSignedReportingExe = viruschecker("pathToSignedReportingExe")
            Dim ps As UInt32 = viruschecker("productState")
            Me.productState = Convert.ToString(ps, 16).PadLeft(6, "0")
            Me.timestamp = viruschecker("timestamp")
        End Sub

        Public Function AllGood() As Boolean
            Return RealtimeProtectionON() And SignaturesUpToDate()
        End Function

    End Class

    Public Shared Function GetInstalledAV() As List(Of AntiVirusProduct)
        '// SELECT * FROM AntiVirusProduct
        '// SELECT * FROM FirewallProduct
        '// SELECT * FROM AntiSpywareProduct
        Dim wmiData As New ManagementObjectSearcher("root\SecurityCenter2", "SELECT * FROM AntiVirusProduct")
        Dim data As ManagementObjectCollection = wmiData.Get()

        Dim ret As New List(Of AntiVirusProduct)
        For Each virusChecker As ManagementObject In data
            ret.Add(New AntiVirusProduct(virusChecker))
        Next
        Return ret
    End Function
    Public Function CanCheckProtection() As Boolean
        Return Not My.Computer.Info.OSFullName.Contains("Server")
    End Function
    Public Shared Function IsProtected() As Boolean
        Dim ret As List(Of SecurityCheck.AntiVirusProduct) = SecurityCheck.GetInstalledAV

        Dim ProtectionFound As Boolean = False
        For Each av As SecurityCheck.AntiVirusProduct In ret
            If av.AllGood Then ProtectionFound = True
            Debug.Print("{0} is installed and {1}. {2}, {3}.",
                                              av.displayName,
                                              If(av.AllGood, "all is good", "there is a problem"),
                                              If(av.RealtimeProtectionON, "Real-time protection is enabled", "Real-time protection is DISABLED"),
                                              If(av.SignaturesUpToDate, "Signatures are up to date", "signatures are NOT up to date"),
                                              vbNewLine)

        Next

        Return ProtectionFound
    End Function
End Class
