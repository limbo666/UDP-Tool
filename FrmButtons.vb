Public Class FrmButtons
    Dim TextToSend1 As String
    Dim TextToSend2 As String
    Dim TextToSend3 As String
    Dim TextToSend4 As String
    Dim TextToSend5 As String
    Dim TextToSend6 As String



    Sub aling()
        Me.Top = FrmMain.Top
        If OS = "WIN10" Then

            If DisplayRatio = "4/3" Then
                Me.Left = FrmMain.Left + FrmMain.Width + 6
            ElseIf DisplayRatio = "16/9" Then
                Me.Left = FrmMain.Left + FrmMain.Width + 15
            Else
                Me.Left = FrmMain.Left + FrmMain.Width + 1
            End If

        Else
            Me.Left = FrmMain.Left + FrmMain.Width + 1
        End If
    End Sub
    Private Sub FrmButtons_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Timer1.Enabled = True

        aling()

        Dim button1text As String = INIRead(Application.StartupPath & "\buttons.ini", "Labels", "Button_1", "")

            If button1text.Length > 0 Then
                Button1.Enabled = True
                Button1.Text = button1text
            Else
                Button1.Text = "Empty"
                Button1.Enabled = False
            End If

            Dim button2text As String = INIRead(Application.StartupPath & "\buttons.ini", "Labels", "Button_2", "")
            If button2text.Length > 0 Then
                Button2.Enabled = True
                Button2.Text = button2text
            Else
                Button2.Text = "Empty"
                Button2.Enabled = False

            End If

            Dim button3text As String = INIRead(Application.StartupPath & "\buttons.ini", "Labels", "Button_3", "")
            If button3text.Length > 0 Then
                Button3.Enabled = True
                Button3.Text = button3text
            Else
                Button3.Text = "Empty"
                Button3.Enabled = False
            End If

            Dim button4text As String = INIRead(Application.StartupPath & "\buttons.ini", "Labels", "Button_4", "")
            If button4text.Length > 0 Then
                Button4.Enabled = True
                Button4.Text = button4text
            Else
                Button4.Text = "Empty"
                Button4.Enabled = False
            End If

            Dim button5text As String = INIRead(Application.StartupPath & "\buttons.ini", "Labels", "Button_5", "")
        If button5text.Length > 0 Then
            Button5.Enabled = True
            Button5.Text = button5text
        Else
            Button5.Text = "Empty"
            Button5.Enabled = False
        End If

        Dim button6text As String = INIRead(Application.StartupPath & "\buttons.ini", "Labels", "Button_6", "")
        If button6text.Length > 0 Then
            Button7.Enabled = True
            Button7.Text = button6text
        Else
            Button7.Text = "Empty"
            Button7.Enabled = False
        End If

        TextToSend1 = INIRead(Application.StartupPath & "\buttons.ini", "Commands", "Button_1", "Test Text 1")
            TextToSend2 = INIRead(Application.StartupPath & "\buttons.ini", "Commands", "Button_2", "Test Text 2")
            TextToSend3 = INIRead(Application.StartupPath & "\buttons.ini", "Commands", "Button_3", "Test Text 3")
            TextToSend4 = INIRead(Application.StartupPath & "\buttons.ini", "Commands", "Button_4", "Test Text 4")
            TextToSend5 = INIRead(Application.StartupPath & "\buttons.ini", "Commands", "Button_5", "Test Text 5")
        TextToSend6 = INIRead(Application.StartupPath & "\buttons.ini", "Commands", "Button_6", "Test Text 6")

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        aling()

    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Try
            Dim filepath As String = Application.StartupPath & "\buttons.ini"
            If Not System.IO.File.Exists(filepath) Then
                System.IO.File.Create(filepath).Dispose()

                Dim objWriter As New System.IO.StreamWriter(filepath)

                objWriter.Write(TextBox1.Text)
                objWriter.Close()

            End If
            Process.Start(Application.StartupPath & "\buttons.ini")
            Me.Close()
            FrmMain.ChkShowButtons.Checked = False

        Catch ex As Exception

        End Try

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        FrmMain.UDPSend(TextToSend1)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        FrmMain.UDPSend(TextToSend2)
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        FrmMain.UDPSend(TextToSend3)
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        FrmMain.UDPSend(TextToSend4)
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        FrmMain.UDPSend(TextToSend5)
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        FrmMain.UDPSend(TextToSend6)
    End Sub
End Class