Imports System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar

Public Class Form1

    Public Structure MainValues
        ' SET_VALUE
        Public Set_SV As Double
        Public Set_Door1 As Boolean
        Public Set_Door2 As Boolean
        Public Set_Crush1 As Boolean
        Public Set_Crush2 As Boolean
        Public Set_Val1 As Boolean
        Public Set_Val2 As Boolean
        ' GET_VALUE
        Public Get_PV As Double
        Public Get_DrPosUp As Integer
        Public Get_DrPosDw As Integer
        ' PROCESS_VALUE
        Public Now_TotalTime As TimeSpan
        Public Now_CurrentTime As TimeSpan
        Public Now_node(,) As Double
        Public OpenVA As TimeSpan
        Public OpenVB As TimeSpan
        Public CloseVA As TimeSpan
        Public CloseVB As TimeSpan
    End Structure
    Public MValues As New MainValues()

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetupTest()
        newChartLog()
    End Sub


    '---------------------------------------------------------------------------------------
#Region "Tester"
    Private Sub SetupTest()
        MValues.OpenVA = MValues.OpenVA.Add(TimeSpan.FromMinutes(20))
        MValues.CloseVA = MValues.CloseVA.Add(TimeSpan.FromMinutes(200))
        MValues.OpenVA = MValues.OpenVA.Add(TimeSpan.FromMinutes(20))
        MValues.CloseVB = MValues.CloseVB.Add(TimeSpan.FromMinutes(290))

        MValues.Now_node = New Double(1, 10) {} ' Initialize the 2D array
        For x As Integer = 0 To 9
            MValues.Now_node(0, x) = x * 2
            MValues.Now_node(1, x) = x * x
        Next
        MValues.Now_TotalTime = MValues.Now_TotalTime.Add(TimeSpan.FromMinutes(300))
        Timer1.Interval = 100 '' Interval 100mm as 1 minute
        Timer1.Start()
    End Sub
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If MValues.Now_CurrentTime > MValues.Now_TotalTime Then
            Timer1.Stop()
        End If
        MValues.Now_CurrentTime = MValues.Now_CurrentTime.Add(TimeSpan.FromMinutes(1))
        MValues.Get_PV = SmoothRandom(200, 900, 0.1)
    End Sub

    ' Declare the Random object
    Private rand As New Random()
    ' Function to generate a smooth random value
    Private Function SmoothRandom(min As Integer, max As Integer, smoothingFactor As Double) As Double
        ' Generate two random values
        Dim value1 As Double = rand.Next(min, max)
        Dim value2 As Double = rand.Next(min, max)

        ' Interpolate between the two values
        Return (1 - smoothingFactor) * value1 + smoothingFactor * value2
    End Function
#End Region
    '---------------------------------------------------------------------------------------

    Private Sub newChartLog()
        Dim CHRT As New CLS_CHART(Me)
        CHRT.Location = New Point(10, 10)
        CHRT.Width = 600
        CHRT.Height = 600
        Me.Controls.Add(CHRT)
    End Sub



End Class
