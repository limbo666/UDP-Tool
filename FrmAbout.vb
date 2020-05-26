Public Class FrmAbout

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub

    Private Sub FrmAbout_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
     
        If WasTopMost = True Then
            FrmMain.TopMost = True
            WasTopMost = False
        End If

      
    End Sub

    Private Sub FrmAbout_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LblVer.Text = Application.ProductVersion
        Me.Top = FrmMain.Top + ((FrmMain.Height - Me.Height) / 2)
        Me.Left = FrmMain.Left + ((FrmMain.Width - Me.Width) / 2)
    End Sub
End Class