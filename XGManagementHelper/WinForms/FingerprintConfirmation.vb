Public Class FingerprintConfirmation

    Sub New(Fingerprint As String, Unexpected As Boolean, host As String)

        ' This call is required by the designer.
        InitializeComponent()
        Me.Text = host & " Fingerprint Confirm"
        ' Add any initialization after the InitializeComponent() call.
        FingerprintLabel.Text = Fingerprint
        If Unexpected Then
            FingerprintLabel.BackColor = Color.Red
            MessageLabel.Text = "The firewall's host key has changed from what is stored. Something may be intercepting your communication, and you should cancel immediately if this is not expected. Do you want to trust it?"
            CancelButton.BackColor = Color.Red
            CancelButton.ForeColor = Color.White
            OKButton.BackColor = Color.FromKnownColor(KnownColor.Control)
            OKButton.ForeColor = Color.FromArgb(25, 135, 203)
            FingerprintLabel.Text = Fingerprint

        Else
            FingerprintLabel.BackColor = Color.FromArgb(25, 135, 203)

        End If
    End Sub

    Private Sub FingerprintConfirmation_Activated(sender As Object, e As EventArgs) Handles Me.Activated
        If FingerprintLabel.BackColor = Color.Red Then
            CancelButton.Focus()
        Else
            OKButton.Focus()
        End If
    End Sub
End Class