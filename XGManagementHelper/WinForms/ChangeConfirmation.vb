' Copyright 2020  Sophos Ltd.  All rights reserved.
' Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
' You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
' Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, 
' WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing 
' permissions and limitations under the License.

Public Class ChangeConfirmation

    Public Sub New(AffectedFirewallCount As Integer, DebugLogging As Boolean)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.AffectedFirewallCount = AffectedFirewallCount
        Me.DebugLogging = DebugLogging
    End Sub

    Public Property AffectedFirewallCount As Integer
        Get
            Return CountLabel.Text
        End Get
        Set(value As Integer)
            CountLabel.Text = value
            If value = 1 Then
                FirewallsLabel.Text = "Firewall"
            Else
                FirewallsLabel.Text = "Firewalls"
            End If
        End Set
    End Property

    Public Property DebugLogging As Boolean
        Get
            Return DebugCheckBox.Checked
        End Get
        Set(value As Boolean)
            DebugCheckBox.Checked = value
        End Set
    End Property

End Class