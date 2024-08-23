' This class is used to monitor: Now_node(,), Now_TotalTime, Now_CurrentTime, and Now_PV
' It starts scanning by itself
' Call Example:
' Private Sub newChartLog()
'     Dim CHRT As New CLS_CHART(Me)
'     CHRT.Location = New Point(10, 10)
'     CHRT.Width = 600
'     CHRT.Height = 600
'     Me.Controls.Add(CHRT)
' End Sub
' Class completed on 16 Aug 2024 by Esper.
Imports System.Windows.Forms
Imports System.Drawing
Imports ScottPlot
Imports ScottPlot.WinForms
Imports ScottPlot.Colormaps
Imports ScottPlot.Stylers

Public Class CLS_CHART
    Inherits Panel

    ' ScottPlot control for plotting data
    Private ReadOnly iChart As New FormsPlot

    ' UI elements
    Private WithEvents MainGroup As New System.Windows.Forms.GroupBox()
    Private WithEvents mainTimer As New Timer
    Private WithEvents StatusPanel As New Panel
    Private WithEvents Lbl_Info As New System.Windows.Forms.Label

    ' Reference to the main form
    Private mainForm As Form1

    ' Constructor to initialize the chart with the main form
    Public Sub New(Form As Form1)
        mainForm = Form ' Copy main form reference

        ' Setup the main group box
        MainGroup.Width = 600
        MainGroup.Height = 260
        MainGroup.Text = "Data log"
        Me.Controls.Add(MainGroup)

        ' Setup the ScottPlot control
        iChart.Width = 550
        iChart.Height = 230
        iChart.Location = New Point(10, 30)
        iChart.Plot.XLabel("Time (min)")
        iChart.Plot.YLabel("Temperature (°C)")
        iChart.BackColor = System.Drawing.Color.FromArgb(255, 255, 255)
        MainGroup.Controls.Add(iChart)

        ' Setup the status panel for blinking status indicator
        StatusPanel.Location = New Point(585, 20)
        StatusPanel.Width = 10
        StatusPanel.Height = 10
        StatusPanel.BackColor = System.Drawing.Color.White
        MainGroup.Controls.Add(StatusPanel)

        ' Setup the timer
        mainTimer.Interval = 100
        mainTimer.Start()

        ' Setup the label for displaying information
        Lbl_Info.Location = New Point(0, 300)
        Lbl_Info.Width = 300
        Lbl_Info.Height = 250
        Me.Controls.Add(Lbl_Info)
    End Sub

    ' Variable to toggle status logic
    Private StatusLogic As Boolean

    ' Timer tick event to update the plot
    Private Sub myTimer_Tick(sender As Object, e As EventArgs) Handles mainTimer.Tick
        UpdatePlot()
    End Sub

    ' Initialize arrays for X and Y values
    Private xValues(9) As Double
    Private yValues(9) As Double

    ' Declare lists to store timeline and PV on-time data
    Private TimeLine As New List(Of Double)()
    Private PVOntime As New List(Of Double)()

    ' Method to update the plot with new data
    Private Sub UpdatePlot()
        ' Add data to the lists if the current time is less than the total time
        If mainForm.MValues.Now_CurrentTime < mainForm.MValues.Now_TotalTime Then
            TimeLine.Add(mainForm.MValues.Now_CurrentTime.TotalMinutes)
            PVOntime.Add(mainForm.MValues.Get_PV)
        End If

        ' Convert lists to arrays
        Dim TimeLineArray() As Double = TimeLine.ToArray()
        Dim PVOntimeArray() As Double = PVOntime.ToArray()

        ' Toggle status logic
        StatusLogic = Not StatusLogic

        ' Check if invoke is required for thread safety
        If Me.InvokeRequired Then
            Me.Invoke(New Action(AddressOf UpdatePlot))
        Else
            ' Clear the plot
            iChart.Plot.Clear()
            ValveHighlight()
            ' Update label text and fill arrays with random values for demonstration
            Lbl_Info.Text = ""
            For i As Integer = 0 To 9
                xValues(i) = mainForm.MValues.Now_node(0, i)
                yValues(i) = (New Random()).Next(0, 100)
                Lbl_Info.Text &= $"{xValues(i)}, {yValues(i)}{vbLf}"
            Next

            ' Add data to the plot
            iChart.Plot.Add.Scatter(TimeLineArray, PVOntimeArray)
            iChart.Plot.Add.Scatter(xValues, yValues)

            ' Auto scale the axes
            iChart.Plot.Axes.AutoScale()
            iChart.Refresh()

            ' Update status panel color
            StatusPanel.BackColor = If(StatusLogic, System.Drawing.Color.Green, System.Drawing.Color.White)
        End If
    End Sub
    Private Sub ValveHighlight()
        Dim vSpan As New Plot
        iChart.Plot.Add.HorizontalSpan(mainForm.MValues.OpenVA.TotalMinutes, mainForm.MValues.CloseVA.TotalMinutes)
        iChart.Plot.Add.HorizontalSpan(mainForm.MValues.OpenVB.TotalMinutes, mainForm.MValues.CloseVB.TotalMinutes)
        ' Refresh the plot to apply changes
        iChart.Refresh()

    End Sub

    Private Sub SetFontForControl(ctrl As System.Windows.Forms.Control, fontName As String, fontSize As Single, fontStyle As System.Drawing.FontStyle)
        Dim newFont As New System.Drawing.Font(fontName, fontSize, fontStyle)
        ctrl.Font = newFont
    End Sub


End Class
