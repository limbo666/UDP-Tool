Imports System.Net
Imports System.Text.Encoding
Imports System.IO
Imports System.Text
Imports System.Threading

Public Class FrmMain
    Public SendPort As Integer = 8266

    Dim LastText As String
    Dim HeadText As String
    Dim TailText As String
    Dim TargetIp As String
    Dim TargetPort As String
    Dim LocalServerPort As String

    Dim HeadAndTail As Boolean
    Dim round As Integer = 0

    Dim UDPmsg1 As String = ""
    Dim UDPmsg2 As String = ""
    Dim UDPmsg3 As String = ""
    Dim UDPmsg4 As String = ""
    Dim UDPmsg5 As String = ""
    Dim UDPmsg6 As String = ""
    Dim UDPmsg7 As String = ""
    Dim UDPmsg8 As String = ""
    Dim UDPmsg9 As String = ""
    Dim UDPmsg10 As String = ""

    Dim SelectedSound As Integer = 1
    Private trdSound As Thread


#Region "UDP Stuff"
    Dim publisher As New Sockets.UdpClient(0)
    Dim subscriber As New Sockets.UdpClient(0)
    'Dim subscriber As New Sockets.UdpClient(39712)
    Dim IndicatorToggle As Integer = 0
    Dim ReceivedText As String
#End Region

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        SaveSetting("UDP Tool", "Settings", "Top", Me.Top)
        SaveSetting("UDP Tool", "Settings", "Left", Me.Left)
        SaveSetting("UDP Tool", "Settings", "Target IP", TargetIp)
        SaveSetting("UDP Tool", "Settings", "Target Port", TargetPort)
        SaveSetting("UDP Tool", "Settings", "Server Port", LocalServerPort)
        SaveSetting("UDP Tool", "Settings", "Last Text", LastText)
        SaveSetting("UDP Tool", "Settings", "Head Text", HeadText)
        SaveSetting("UDP Tool", "Settings", "Tail Text", TailText)
        SaveSetting("UDP Tool", "Settings", "Top Most", ChkStayOnTop.Checked)
        SaveSetting("UDP Tool", "Settings", "Beep On Receive", CheckBox1.Checked)
        SaveSetting("UDP Tool", "Settings", "Log Messages", CheckBox2.Checked)
        SaveSetting("UDP Tool", "Settings", "Log Messages", CheckBox3.Checked)
        SaveSetting("UDP Tool", "Settings", "Enable Receiver", CheckBox4.Checked)
        SaveSetting("UDP Tool", "Settings", "Show Received Messages", ChkShowMessages.Checked)
        SaveSetting("UDP Tool", "Settings", "Show Buttons", ChkShowButtons.Checked)
        SaveSetting("UDP Tool", "Settings", "Head And Tail", HeadAndTail)
        Try
            subscriber.Close()
        Catch ex As Exception
        End Try
    End Sub



    Sub DetectOS()
        Try
            Dim osVer As Version = Environment.OSVersion.Version
            If osVer.Major = 6 And osVer.Minor = 1 Then
                'Windows 7
                OS = "WIN7"
            ElseIf osVer.Major = 6 And osVer.Minor = 2 Then
                'Windows 10
                OS = "WIN10"
            Else
                OS = "UNKNOWN"
            End If
        Catch ex As Exception
        End Try
    End Sub

    Sub DetectScreen()
        Dim screenWidth As Integer = Screen.PrimaryScreen.Bounds.Width
        Dim screenHeight As Integer = Screen.PrimaryScreen.Bounds.Height
        Dim resolution As String = screenWidth & "x" & screenHeight
        Select Case resolution
            Case "1920x1080" '16/9
                DisplayRatio = "16/9"
            Case "1366x768" '16/9
                DisplayRatio = "16/9"
            Case "1280x720" '16/9
                DisplayRatio = "16/9"
            Case "1280x1024" '4/3
                DisplayRatio = "4/3"
            Case "1280x800" '16/9
                DisplayRatio = "16/9"
            Case "1024x768" '4/3
                DisplayRatio = "4/3"
            Case "800x600" '4/3
                DisplayRatio = "4/3"
            Case Else

        End Select
    End Sub


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Button18.Text = INIRead(Application.StartupPath & "\buttons.ini", "Labels", "PlusButton_1", "")

        Me.MaximumSize = Me.Size
        Me.MinimumSize = Me.Size

        Me.Top = GetSetting("UDP Tool", "Settings", "Top", 100)
        Me.Left = GetSetting("UDP Tool", "Settings", "Left", 100)


        TargetIp = GetSetting("UDP Tool", "Settings", "Target IP", "192.168.1.255")
        TargetPort = GetSetting("UDP Tool", "Settings", "Target Port", "8266")
        LocalServerPort = GetSetting("UDP Tool", "Settings", "Server Port", "8266")
        LastText = GetSetting("UDP Tool", "Settings", "Last Text", "")
        HeadText = GetSetting("UDP Tool", "Settings", "Head Text", "")
        TailText = GetSetting("UDP Tool", "Settings", "Tail Text", "")
        ChkStayOnTop.Checked = GetSetting("UDP Tool", "Settings", "Top Most", False)
        CheckBox1.Checked = GetSetting("UDP Tool", "Settings", "Beep On Receive", False)
        CheckBox2.Checked = GetSetting("UDP Tool", "Settings", "Log Messages", False)
        CheckBox4.Checked = GetSetting("UDP Tool", "Settings", "Enable Receiver", True)
        ChkShowMessages.Checked = GetSetting("UDP Tool", "Settings", "Show Received Messages", False)
        ChkShowButtons.Checked = GetSetting("UDP Tool", "Settings", "Show Buttons", False)
        HeadAndTail = GetSetting("UDP Tool", "Settings", "Head And Tail", False)

        CheckBox5.Checked = HeadAndTail

        TextBox1.Text = LastText
        TextBox3.Text = TargetIp
        TextBox2.Text = TargetPort
        TextBox4.Text = LocalServerPort



        TextBox5.Text = HeadText
        TextBox6.Text = TailText
        DetectOS()
        DetectScreen()


    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Label9.Text = ""
        If TextBox1.Text.Length > 0 Then
            If HeadAndTail = True Then
                UDPSend(HeadText & LastText & TailText)
            Else
                UDPSend(LastText)
            End If
            If CheckBox3.Checked = True Then
                TextBox1.Text = Nothing
                TextBox1.Focus()
            End If
        Else
            TextBox1.BackColor = Color.Red
            TextBox1.Focus()
            TmrClear.Enabled = False
            TmrClear.Enabled = True
        End If


    End Sub


    Sub UDPSend(TextToSend)
        On Error GoTo ErrHandler
        Dim e2 As Encoding = Encoding.GetEncoding(28597) ' greek enconding 
        '  Dim sendbytes() As Byte = ASCII.GetBytes(TextToSend)
        '   Dim sendbytes() As Byte = System.Text.Encoding.Unicode.GetBytes(TextToSend)
        Dim sendbytes() As Byte = e2.GetBytes(TextToSend)
        publisher.Connect(TargetIp, TargetPort)
        publisher.Send(sendbytes, sendbytes.Length)
        Label9.Text = TextToSend
        Exit Sub
ErrHandler:
        Resume Next
    End Sub


    Private Sub TmrUDPRCV_Tick(sender As Object, e As EventArgs) Handles TmrUDPRCV.Tick


        'this is a UDP receiver

        If IndicatorToggle < 10 Then
            IndicatorToggle += 1
        Else
            IndicatorToggle = 0
        End If

        If IndicatorToggle < 5 Then
            LblIND.BackColor = Color.Lime
        Else
            LblIND.BackColor = Color.Green
        End If

        Try
            Dim ep As IPEndPoint = New IPEndPoint(IPAddress.Any, 0)
            Dim rcvbytes() As Byte = subscriber.Receive(ep)
            Dim e2 As Encoding = Encoding.GetEncoding(28597) ' greek enconding 
            ReceivedText = e2.GetString(rcvbytes)
            '     ReceivedText = ASCII.GetString(rcvbytes)
            TmrClear.Enabled = True
            If CheckBox1.Checked = True Then
                ' Beep()


                trdSound = New Thread(AddressOf SoundNow)
                    trdSound.IsBackground = True
                    trdSound.Start()


            End If

                If CheckBox2.Checked = True Then
                Log(ReceivedText, "UDP tool message log.txt")

            End If

            If ChkShowMessages.Checked = True Then
                Dim time As DateTime = DateTime.Now
                Dim format As String = "HH:mm:ss"
                ' Console.WriteLine(time.ToString(format))
                If FrmMessages.CheckBox1.Checked = True Then

                    FrmMessages.RichTextBox1.SelectionStart = FrmMessages.RichTextBox1.Text.Length
                    Dim oldcolor = FrmMessages.RichTextBox1.SelectionColor
                    FrmMessages.RichTextBox1.SelectionColor = Color.Red
                    FrmMessages.RichTextBox1.AppendText(time.ToString(format) & " ")
                    FrmMessages.RichTextBox1.SelectionColor = oldcolor
                    ' FrmMessages.RichTextBox1.AppendText(ReceivedText & vbNewLine)
                Else
                End If

                FrmMessages.RichTextBox1.AppendText(ReceivedText & vbNewLine)

                FrmMessages.RichTextBox1.ScrollToCaret()
                FrmMessages.RichTextBox1.SelectionStart = 0
            Else


            End If

            TmrClear.Enabled = False
            TmrClear.Enabled = True



        Catch ex As Exception
        End Try

        LblInText.Text = ReceivedText

    End Sub








    Public Shared Sub Log(logMessage As String, filepath As String)
        Dim fs As New FileStream(filepath, FileMode.Append, FileAccess.Write)
        Dim s As New StreamWriter(fs, System.Text.Encoding.Default)
        s.BaseStream.Seek(0, SeekOrigin.End)
        s.WriteLine(logMessage)
        s.Close()

    End Sub





    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged

        TargetPort = TextBox2.Text
    End Sub

    Private Sub TextBox3_TextChanged(sender As Object, e As EventArgs) Handles TextBox3.TextChanged

        TargetIp = TextBox3.Text
    End Sub


    Private Sub TmrClear_Tick(sender As Object, e As EventArgs) Handles TmrClear.Tick
        TmrClear.Enabled = False
        LblInText.Text = ""
        ReceivedText = ""
        If TextBox1.Text.Length > 0 Then
            TextBox1.BackColor = Color.FromKnownColor(KnownColor.Window)
        End If

    End Sub


    Private Sub TextBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox1.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
        End If

    End Sub

    Private Sub TextBox1_KeyUp(sender As Object, e As KeyEventArgs) Handles TextBox1.KeyUp

        If e.KeyCode.Equals(Keys.Enter) Then
            e.Handled = True
            Button1_Click(Nothing, Nothing)

        End If

    End Sub


    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged

        LastText = TextBox1.Text
        TextBox1.BackColor = Color.FromKnownColor(KnownColor.Window)
    End Sub

    Private Sub ChkStayOnTop_CheckedChanged(sender As Object, e As EventArgs) Handles ChkStayOnTop.CheckedChanged
        If ChkStayOnTop.Checked = True Then
            Me.TopMost = True
            FrmMessages.TopMost = True
            FrmButtons.TopMost = True
        Else
            Me.TopMost = False
            FrmMessages.TopMost = False
            FrmButtons.TopMost = False
        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        SelectedSound = INIRead(Application.StartupPath & "\buttons.ini", "Sound", "SelectedSound", 1)
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub

    Private Sub CheckBox4_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox4.CheckedChanged
        If CheckBox4.Checked = True Then
            TextBox4.Enabled = False

            Try

                subscriber = New Sockets.UdpClient(CInt(LocalServerPort))
                TmrUDPRCV.Enabled = True
                subscriber.Client.ReceiveTimeout = 100
                subscriber.Client.Blocking = False
            Catch ex As Exception
                '  MsgBox(Exception)
                MsgBox(ex.Message)
                CheckBox4.Checked = False

            End Try

        Else
            TextBox4.Enabled = True
            subscriber.Close()
            TmrUDPRCV.Enabled = False
            LblIND.BackColor = Color.FromArgb(64, 64, 64)

        End If
    End Sub

    Private Sub LblInText_Click(sender As Object, e As EventArgs) Handles LblInText.Click

    End Sub

    Private Sub LlAbout_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LlAbout.LinkClicked
        If Me.TopMost = True Then
            Me.TopMost = False
            WasTopMost = True

        End If
        FrmAbout.ShowDialog()
    End Sub

    Private Sub TextBox4_TextChanged(sender As Object, e As EventArgs) Handles TextBox4.TextChanged
        LocalServerPort = TextBox4.Text
    End Sub

    Private Sub ChkShowMessages_CheckedChanged(sender As Object, e As EventArgs) Handles ChkShowMessages.CheckedChanged
        If ChkShowMessages.Checked = True Then
            FrmMessages.Show()
        Else
            FrmMessages.Close()
        End If
    End Sub

    Private Sub ChkShowButtons_CheckedChanged(sender As Object, e As EventArgs) Handles ChkShowButtons.CheckedChanged
        If ChkShowButtons.Checked = True Then
            FrmButtons.Show()
        Else
            FrmButtons.Close()
        End If
    End Sub


    Private Sub CheckBox5_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox5.CheckedChanged
        If CheckBox5.Checked = True Then
            TextBox5.Visible = True
            TextBox6.Visible = True
            HeadAndTail = True

            TextBox1.Width = 330
            TextBox1.Left = TextBox5.Left + TextBox5.Width + 2
            TextBox5.Top = TextBox1.Top
            TextBox6.Top = TextBox1.Top

            Label7.Visible = True
            Label8.Visible = True


        Else
            TextBox5.Visible = False
            TextBox6.Visible = False
            HeadAndTail = False

            TextBox1.Width = 394
            TextBox1.Left = TextBox5.Left

            Label7.Visible = False
            Label8.Visible = False
        End If

        Label1.Left = TextBox1.Left

    End Sub

    Private Sub TextBox5_TextChanged(sender As Object, e As EventArgs) Handles TextBox5.TextChanged
        HeadText = TextBox5.Text
    End Sub

    Private Sub TextBox6_TextChanged(sender As Object, e As EventArgs) Handles TextBox6.TextChanged
        TailText = TextBox6.Text
    End Sub

    Private Sub TmrSendRepeat_Tick(sender As Object, e As EventArgs) Handles TmrSendRepeat.Tick
        Label9.Text = ""
        If round < 10 Then
            round = round + 1
        Else
            round = 1
        End If
        Select Case round
            Case 1
                If UDPmsg1 <> "" Then
                    UDPSend(UDPmsg1)
                End If

            Case 2
                If UDPmsg2 <> "" Then
                    UDPSend(UDPmsg2)
                End If
            Case 3
                If UDPmsg3 <> "" Then
                    UDPSend(UDPmsg3)
                End If
            Case 4
                If UDPmsg4 <> "" Then
                    UDPSend(UDPmsg4)
                End If
            Case 5
                If UDPmsg5 <> "" Then
                    UDPSend(UDPmsg5)
                End If
            Case 6
                If UDPmsg6 <> "" Then
                    UDPSend(UDPmsg6)
                End If
            Case 7
                If UDPmsg7 <> "" Then
                    UDPSend(UDPmsg7)
                End If
            Case 8
                If UDPmsg8 <> "" Then
                    UDPSend(UDPmsg8)
                End If
            Case 9
                If UDPmsg9 <> "" Then
                    UDPSend(UDPmsg9)
                End If
            Case 10
                If UDPmsg10 <> "" Then
                    UDPSend(UDPmsg10)
                End If
            Case Else
                '  UDPSend("OFF")
        End Select

    End Sub

    Private Sub CheckBox6_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox6.CheckedChanged
        Dim tmrinterval As Integer = 500
        Try
            tmrinterval = INIRead(Application.StartupPath & "\buttons.ini", "RepeatMessages", "Interval", 500)
        Catch ex As Exception

        End Try

        TmrSendRepeat.Interval = tmrinterval

        UDPmsg1 = INIRead(Application.StartupPath & "\buttons.ini", "RepeatMessages", "UDPmsg1", "")
        UDPmsg2 = INIRead(Application.StartupPath & "\buttons.ini", "RepeatMessages", "UDPmsg2", "")
        UDPmsg3 = INIRead(Application.StartupPath & "\buttons.ini", "RepeatMessages", "UDPmsg3", "")
        UDPmsg4 = INIRead(Application.StartupPath & "\buttons.ini", "RepeatMessages", "UDPmsg4", "")
        UDPmsg5 = INIRead(Application.StartupPath & "\buttons.ini", "RepeatMessages", "UDPmsg5", "")
        UDPmsg6 = INIRead(Application.StartupPath & "\buttons.ini", "RepeatMessages", "UDPmsg6", "")
        UDPmsg7 = INIRead(Application.StartupPath & "\buttons.ini", "RepeatMessages", "UDPmsg7", "")
        UDPmsg8 = INIRead(Application.StartupPath & "\buttons.ini", "RepeatMessages", "UDPmsg8", "")
        UDPmsg9 = INIRead(Application.StartupPath & "\buttons.ini", "RepeatMessages", "UDPmsg9", "")
        UDPmsg10 = INIRead(Application.StartupPath & "\buttons.ini", "RepeatMessages", "UDPmsg10", "")


        TmrSendRepeat.Enabled = CheckBox6.Checked
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
            '       Me.Close()

        Catch ex As Exception

        End Try
    End Sub
    Sub SoundNow()

        Try
            My.Computer.Audio.Stop()
        Catch ex As Exception

        End Try
        Select Case SelectedSound
            Case 1
                My.Computer.Audio.Play(My.Resources.Notify__1_, AudioPlayMode.WaitToComplete)
            Case 2
                My.Computer.Audio.Play(My.Resources.Notify__2_, AudioPlayMode.WaitToComplete)
            Case 3
                My.Computer.Audio.Play(My.Resources.Notify__3_, AudioPlayMode.WaitToComplete)
            Case 4
                My.Computer.Audio.Play(My.Resources.Notify__4_, AudioPlayMode.WaitToComplete)
            Case 5
                My.Computer.Audio.Play(My.Resources.Notify__5_, AudioPlayMode.WaitToComplete)
            Case 6
                My.Computer.Audio.Play(My.Resources.Notify__6_, AudioPlayMode.WaitToComplete)
            Case 7
                My.Computer.Audio.Play(My.Resources.Notify__7_, AudioPlayMode.WaitToComplete)
            Case 8
                My.Computer.Audio.Play(My.Resources.Notify__8_, AudioPlayMode.WaitToComplete)
            Case Else
                My.Computer.Audio.Play(My.Resources.Notify__1_, AudioPlayMode.WaitToComplete)

        End Select

    End Sub
    Private Sub Panel2_Paint(sender As Object, e As PaintEventArgs) Handles Panel2.Paint

    End Sub
End Class
