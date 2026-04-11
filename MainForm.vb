' PyForm Builder - مصمم واجهات بايثون
' Created by SharpDevelop 4.4
' Author: Ahmed
' Date: 03/04/2026

Imports System.Windows.Forms
Imports System.Drawing
Imports System.IO
Imports System.Text

Partial Class MainForm
    Inherits System.Windows.Forms.Form

    ' =============================================
    ' --- هيكل بيانات العنصر ---
    ' =============================================

    Public Class WidgetData
        Public Property WidgetType As String
        Public Property Name As String
        Public Property Text As String
        Public Property X As Integer
        Public Property Y As Integer
        Public Property W As Integer
        Public Property H As Integer
        Public Property FgColor As String
        Public Property BgColor As String
        Public Property FontName As String
        Public Property FontSize As Integer
        Public Property FontBold As Boolean
        Public Property Relief As String
        Public Property CommandName As String
        Public Property LayoutMode As String   ' place / pack / grid
        Public Property PackSide As String     ' left / right / top / bottom
        Public Property PackFill As String     ' none / x / y / both
        Public Property PackExpand As Boolean
        Public Property GridRow As Integer
        Public Property GridCol As Integer
        Public Property GridSticky As String   ' n/s/e/w/nsew...
        Public Property ParentFrame As String  ' اسم الـ Frame الأب (فارغ = root)
        Public Property ImagePath As String    ' مسار الصورة إن وجدت
    End Class

    ' =============================================
    ' --- تعداد مقابض تغيير الحجم ---
    ' =============================================

    Private Enum ResizeHandle
        None
        TopLeft
        TopRight
        BottomLeft
        BottomRight
        Right
        Bottom
    End Enum

    ' =============================================
    ' --- المتغيرات العامة ---
    ' =============================================

    Private selectedControl As Control = Nothing
    Private selectedControls As New List(Of Control)
    Private dragOffset As Point
    Private isDragging As Boolean = False
    Private isResizing As Boolean = False
    Private activeHandle As ResizeHandle = ResizeHandle.None
    Private resizeStartBounds As Rectangle
    Private resizeStartMouse As Point
    Private Const HANDLE_SIZE As Integer = 7
    Private isRubberBanding As Boolean = False
    Private rubberBandStart As Point
    Private rubberBandRect As Rectangle
    Private controlCounters As New Dictionary(Of String, Integer)
    Private Const GRID_SIZE As Integer = 8

    ' بيانات إضافية لكل عنصر
    Private widgetFgColor As New Dictionary(Of String, Color)
    Private widgetBgColor As New Dictionary(Of String, Color)
    Private widgetRelief As New Dictionary(Of String, String)
    Private widgetCommand As New Dictionary(Of String, String)
    Private widgetLayout As New Dictionary(Of String, String)    ' place/pack/grid
    Private widgetPackSide As New Dictionary(Of String, String)
    Private widgetPackFill As New Dictionary(Of String, String)
    Private widgetPackExpand As New Dictionary(Of String, Boolean)
    Private widgetGridRow As New Dictionary(Of String, Integer)
    Private widgetGridCol As New Dictionary(Of String, Integer)
    Private widgetGridSticky As New Dictionary(Of String, String)
    Private widgetParent As New Dictionary(Of String, String)    ' اسم الـ Frame الأب
    Private widgetImage As New Dictionary(Of String, String)     ' مسار الصورة

    Private currentProjectPath As String = ""
    Private formBgColor As Color = Color.FromArgb(245, 245, 240)

    ' Clipboard
    Private clipboardItems As New List(Of WidgetData)

    ' Zoom
    Private zoomFactor As Single = 1.0F
    Private Const ZOOM_MIN As Single = 0.5F
    Private Const ZOOM_MAX As Single = 3.0F

    ' Undo/Redo
    Private undoStack As New Stack(Of String)
    Private redoStack As New Stack(Of String)

    ' =============================================
    ' --- التهيئة ---
    ' =============================================

    Public Sub New()
        Me.InitializeComponent()
        Me.SetupDesignCanvas()
        Me.SetupWidgetList()
        Me.SetupCombos()
    End Sub

    Private Sub SetupDesignCanvas()
        DesignPanel.AllowDrop = True
        AddHandler DesignPanel.Paint, AddressOf DesignPanel_Paint
        AddHandler DesignPanel.MouseDown, AddressOf DesignPanel_MouseDown
        AddHandler DesignPanel.MouseMove, AddressOf DesignPanel_MouseMove
        AddHandler DesignPanel.MouseUp, AddressOf DesignPanel_MouseUp
        AddHandler DesignPanel.DragEnter, AddressOf DesignPanel_DragEnter
        AddHandler DesignPanel.DragDrop, AddressOf DesignPanel_DragDrop
    End Sub

    Private Sub SetupWidgetList()
        WidgetListBox.Items.Clear()
        For Each w As String In {"Label", "Button", "Entry", "Text", "Checkbutton",
                                  "Radiobutton", "Listbox", "Combobox", "Frame",
                                  "Scale", "Spinbox", "Canvas", "Image"}
            WidgetListBox.Items.Add(w)
        Next
    End Sub

    Private Sub SetupCombos()
        ' Relief
        CmbRelief.Items.Clear()
        For Each r As String In {"flat", "raised", "sunken", "groove", "ridge", "solid"}
            CmbRelief.Items.Add(r)
        Next
        CmbRelief.SelectedIndex = 0

        ' Font
        CmbFontName.Items.Clear()
        For Each f As String In {"Tahoma", "Arial", "Courier New", "Times New Roman",
                                  "Verdana", "Cairo", "Segoe UI", "Helvetica"}
            CmbFontName.Items.Add(f)
        Next
        CmbFontName.SelectedIndex = 0
        CmbFontSize.Items.Clear()
        For Each s As String In {"8", "9", "10", "11", "12", "14", "16", "18", "20", "24", "28", "32"}
            CmbFontSize.Items.Add(s)
        Next
        If CmbFontSize.Items.Contains("10") Then CmbFontSize.SelectedItem = "10"

        ' Layout Mode
        CmbLayoutMode.Items.Clear()
        CmbLayoutMode.Items.Add("place")
        CmbLayoutMode.Items.Add("pack")
        CmbLayoutMode.Items.Add("grid")
        CmbLayoutMode.SelectedIndex = 0

        ' Pack Side
        CmbPackSide.Items.Clear()
        For Each s As String In {"top", "bottom", "left", "right"}
            CmbPackSide.Items.Add(s)
        Next
        CmbPackSide.SelectedIndex = 0

        ' Pack Fill
        CmbPackFill.Items.Clear()
        For Each s As String In {"none", "x", "y", "both"}
            CmbPackFill.Items.Add(s)
        Next
        CmbPackFill.SelectedIndex = 0

        ' Grid Sticky
        CmbGridSticky.Items.Clear()
        For Each s As String In {"", "n", "s", "e", "w", "ns", "ew", "nsew", "ne", "nw", "se", "sw"}
            CmbGridSticky.Items.Add(s)
        Next
        CmbGridSticky.SelectedIndex = 0

        ' Parent Frame
        UpdateParentFrameCombo()
    End Sub

    ''' <summary>تحديث قائمة الـ Frames المتاحة كآباء</summary>
    Private Sub UpdateParentFrameCombo()
        Dim current As String = If(CmbParentFrame.SelectedItem IsNot Nothing,
                                   CmbParentFrame.SelectedItem.ToString(), "root")
        CmbParentFrame.Items.Clear()
        CmbParentFrame.Items.Add("root")
        For Each ctrl As Control In DesignPanel.Controls
            If ctrl.Tag IsNot Nothing AndAlso ctrl.Tag.ToString() = "Frame" Then
                CmbParentFrame.Items.Add(ctrl.Name)
            End If
        Next
        If CmbParentFrame.Items.Contains(current) Then
            CmbParentFrame.SelectedItem = current
        Else
            CmbParentFrame.SelectedIndex = 0
        End If
    End Sub

    ' =============================================
    ' --- تغيير وضع Layout في الـ UI ---
    ' =============================================

    Private Sub CmbLayoutMode_SelectedIndexChanged(sender As Object, e As EventArgs) _
        Handles CmbLayoutMode.SelectedIndexChanged
        Dim mode As String = If(CmbLayoutMode.SelectedItem IsNot Nothing,
                                CmbLayoutMode.SelectedItem.ToString(), "place")
        ' إظهار/إخفاء الخيارات المناسبة
        PnlPlaceOptions.Visible = (mode = "place")
        PnlPackOptions.Visible = (mode = "pack")
        PnlGridOptions.Visible = (mode = "grid")
    End Sub

    ' =============================================
    ' --- إضافة العناصر ---
    ' =============================================

    Private Sub WidgetListBox_DoubleClick(sender As Object, e As EventArgs) _
        Handles WidgetListBox.DoubleClick
        If WidgetListBox.SelectedItem Is Nothing Then Return
        Dim wType As String = WidgetListBox.SelectedItem.ToString()
        If wType = "Image" Then
            AddImageWidget(New Point(20, 20))
        Else
            AddWidgetToCanvas(wType, New Point(20, 20))
        End If
    End Sub

    Private Sub WidgetListBox_MouseMove(sender As Object, e As MouseEventArgs) _
        Handles WidgetListBox.MouseMove
        If e.Button = MouseButtons.Left AndAlso WidgetListBox.SelectedItem IsNot Nothing Then
            WidgetListBox.DoDragDrop(WidgetListBox.SelectedItem.ToString(), DragDropEffects.Copy)
        End If
    End Sub

    Private Sub DesignPanel_DragEnter(sender As Object, e As DragEventArgs)
        If e.Data.GetDataPresent(GetType(String)) Then e.Effect = DragDropEffects.Copy
    End Sub

    Private Sub DesignPanel_DragDrop(sender As Object, e As DragEventArgs)
        Dim wType As String = e.Data.GetData(GetType(String)).ToString()
        Dim pt As Point = SnapToGrid(DesignPanel.PointToClient(New Point(e.X, e.Y)))
        If wType = "Image" Then
            AddImageWidget(pt)
        Else
            AddWidgetToCanvas(wType, pt)
        End If
    End Sub

    ' =============================================
    ' --- إضافة عنصر صورة ---
    ' =============================================

    Private Sub AddImageWidget(location As Point, Optional data As WidgetData = Nothing)
        Dim imagePath As String = ""

        If data IsNot Nothing AndAlso data.ImagePath <> "" Then
            imagePath = data.ImagePath
        Else
            Using ofd As New OpenFileDialog()
                ofd.Title = "اختر صورة"
                ofd.Filter = "ملفات الصور (*.png;*.jpg;*.jpeg;*.gif;*.bmp)|*.png;*.jpg;*.jpeg;*.gif;*.bmp"
                If ofd.ShowDialog() <> DialogResult.OK Then Return
                imagePath = ofd.FileName
            End Using
        End If

        If Not controlCounters.ContainsKey("Image") Then controlCounters("Image") = 0
        controlCounters("Image") += 1
        Dim idx As Integer = controlCounters("Image")
        Dim ctrlName As String = If(data IsNot Nothing AndAlso data.Name <> "",
                                    data.Name, "image_" & idx)

        ' نستخدم PictureBox لعرض الصورة في منطقة التصميم
        Dim pb As New PictureBox()
        pb.Name = ctrlName
        pb.Tag = "Image"
        pb.SizeMode = PictureBoxSizeMode.StretchImage
        pb.Size = If(data IsNot Nothing, New Size(data.W, data.H), New Size(120, 90))
        pb.Location = location
        pb.BorderStyle = BorderStyle.FixedSingle

        Try
            pb.Image = Image.FromFile(imagePath)
        Catch
            ' صورة placeholder إذا فشل التحميل
            Dim bmp As New Bitmap(120, 90)
            Using g As Graphics = Graphics.FromImage(bmp)
                g.Clear(Color.LightGray)
                g.DrawString("صورة", New Font("Tahoma", 9), Brushes.Gray, 35, 35)
            End Using
            pb.Image = bmp
        End Try

        widgetFgColor(ctrlName) = Color.Black
        widgetBgColor(ctrlName) = Color.White
        widgetRelief(ctrlName) = "flat"
        widgetCommand(ctrlName) = ""
        widgetLayout(ctrlName) = If(data IsNot Nothing, data.LayoutMode, "place")
        widgetPackSide(ctrlName) = If(data IsNot Nothing, data.PackSide, "top")
        widgetPackFill(ctrlName) = If(data IsNot Nothing, data.PackFill, "none")
        widgetPackExpand(ctrlName) = If(data IsNot Nothing, data.PackExpand, False)
        widgetGridRow(ctrlName) = If(data IsNot Nothing, data.GridRow, 0)
        widgetGridCol(ctrlName) = If(data IsNot Nothing, data.GridCol, 0)
        widgetGridSticky(ctrlName) = If(data IsNot Nothing, data.GridSticky, "")
        widgetParent(ctrlName) = If(data IsNot Nothing, data.ParentFrame, "root")
        widgetImage(ctrlName) = imagePath

        AddHandler pb.MouseDown, AddressOf Widget_MouseDown
        AddHandler pb.MouseMove, AddressOf Widget_MouseMove
        AddHandler pb.MouseUp, AddressOf Widget_MouseUp

        DesignPanel.Controls.Add(pb)
        pb.BringToFront()
        SelectSingle(pb)
        UpdateParentFrameCombo()
        UpdateStatusBar("تمت إضافة صورة: " & ctrlName)
        UpdateCodePreview()
    End Sub

    ' =============================================
    ' --- إضافة عنصر عادي ---
    ' =============================================

    Private Sub AddWidgetToCanvas(widgetType As String, location As Point,
                                   Optional data As WidgetData = Nothing)
        If Not controlCounters.ContainsKey(widgetType) Then controlCounters(widgetType) = 0
        controlCounters(widgetType) += 1
        Dim idx As Integer = controlCounters(widgetType)

        Dim ctrl As Control = Nothing
        Dim defText As String = ""
        Dim defSize As New Size(100, 28)

        Select Case widgetType
            Case "Label"
                ctrl = New Label() : defText = "label_" & idx : defSize = New Size(80, 22)
                DirectCast(ctrl, Label).AutoSize = False
                DirectCast(ctrl, Label).BorderStyle = BorderStyle.FixedSingle
            Case "Button"
                ctrl = New Button() : defText = "زر " & idx : defSize = New Size(90, 28)
            Case "Entry"
                ctrl = New TextBox() : defSize = New Size(120, 24)
            Case "Text"
                ctrl = New RichTextBox() : defSize = New Size(160, 80)
            Case "Checkbutton"
                Dim chkPanel As New Panel()
                chkPanel.BackColor = formBgColor
                ctrl = chkPanel
                defText = "خيار " & idx
                defSize = New Size(120, 28)
            Case "Radiobutton"
                Dim rdPanel As New Panel()
                rdPanel.BackColor = formBgColor
                ctrl = rdPanel
                defText = "خيار " & idx
                defSize = New Size(120, 28)
            Case "Listbox"
                ctrl = New ListBox() : defSize = New Size(120, 80)
                DirectCast(ctrl, ListBox).Items.Add("عنصر 1")
                DirectCast(ctrl, ListBox).Items.Add("عنصر 2")
            Case "Combobox"
                ' نستخدم Panel مع Label داخله لمحاكاة ComboBox ويقبل أي ارتفاع
                Dim cmbPanel As New Panel()
                cmbPanel.BackColor = Color.White
                DirectCast(cmbPanel, Panel).BorderStyle = BorderStyle.FixedSingle
                Dim cmbLbl As New Label()
                cmbLbl.Text = "اختر..."
                cmbLbl.Dock = DockStyle.Fill
                cmbLbl.TextAlign = ContentAlignment.MiddleLeft
                cmbLbl.Padding = New Padding(3, 0, 20, 0)
                cmbPanel.Controls.Add(cmbLbl)
                ctrl = cmbPanel
                defText = "اختر..."
                defSize = New Size(120, 28)
            Case "Frame"
                ctrl = New GroupBox() : defText = "إطار " & idx : defSize = New Size(180, 120)
            Case "Scale"
                ctrl = New TrackBar() : defSize = New Size(150, 30)
                DirectCast(ctrl, TrackBar).Minimum = 0
                DirectCast(ctrl, TrackBar).Maximum = 100
            Case "Spinbox"
                ctrl = New NumericUpDown() : defSize = New Size(80, 24)
            Case "Canvas"
                ctrl = New Panel() : defSize = New Size(200, 150)
                ctrl.BackColor = Color.White
                DirectCast(ctrl, Panel).BorderStyle = BorderStyle.FixedSingle
            Case Else
                Return
        End Select

        Dim ctrlName As String = If(data IsNot Nothing AndAlso data.Name <> "",
                                    data.Name, widgetType.ToLower() & "_" & idx)
        ctrl.Name = ctrlName
        ctrl.Text = If(data IsNot Nothing, data.Text, defText)
        ctrl.Size = If(data IsNot Nothing, New Size(data.W, data.H), defSize)
        ctrl.Location = location
        ctrl.Tag = widgetType

        Dim fg As Color = Color.Black
        Dim bg As Color = ctrl.BackColor
        If data IsNot Nothing Then
            Try
                If data.FgColor <> "" Then fg = ColorTranslator.FromHtml(data.FgColor)
                If data.BgColor <> "" Then bg = ColorTranslator.FromHtml(data.BgColor)
            Catch : End Try
        End If
        ctrl.ForeColor = fg : ctrl.BackColor = bg

        Dim fName As String = If(data IsNot Nothing AndAlso data.FontName <> "", data.FontName, "Tahoma")
        Dim fSize As Integer = If(data IsNot Nothing AndAlso data.FontSize > 0, data.FontSize, 9)
        Dim fBold As Boolean = If(data IsNot Nothing, data.FontBold, False)
        Try
            ctrl.Font = New Font(fName, fSize, If(fBold, FontStyle.Bold, FontStyle.Regular))
        Catch : ctrl.Font = New Font("Tahoma", 9) : End Try

        widgetFgColor(ctrlName) = fg
        widgetBgColor(ctrlName) = bg
        widgetRelief(ctrlName) = If(data IsNot Nothing AndAlso data.Relief <> "", data.Relief, "flat")
        widgetCommand(ctrlName) = If(data IsNot Nothing, data.CommandName, "")
        widgetLayout(ctrlName) = If(data IsNot Nothing AndAlso data.LayoutMode <> "", data.LayoutMode, "place")
        widgetPackSide(ctrlName) = If(data IsNot Nothing AndAlso data.PackSide <> "", data.PackSide, "top")
        widgetPackFill(ctrlName) = If(data IsNot Nothing AndAlso data.PackFill <> "", data.PackFill, "none")
        widgetPackExpand(ctrlName) = If(data IsNot Nothing, data.PackExpand, False)
        widgetGridRow(ctrlName) = If(data IsNot Nothing, data.GridRow, 0)
        widgetGridCol(ctrlName) = If(data IsNot Nothing, data.GridCol, 0)
        widgetGridSticky(ctrlName) = If(data IsNot Nothing, data.GridSticky, "")
        widgetParent(ctrlName) = If(data IsNot Nothing, data.ParentFrame, "root")
        widgetImage(ctrlName) = ""

        AddHandler ctrl.MouseDown, AddressOf Widget_MouseDown
        AddHandler ctrl.MouseMove, AddressOf Widget_MouseMove
        AddHandler ctrl.MouseUp, AddressOf Widget_MouseUp

        ' رسم مخصص للعناصر المستبدلة بـ Panel
        If widgetType = "Checkbutton" OrElse widgetType = "Radiobutton" OrElse widgetType = "Combobox" Then
            AddHandler ctrl.Paint, AddressOf CustomWidget_Paint
        End If

        SaveUndoState()
        DesignPanel.Controls.Add(ctrl)
        ctrl.BringToFront()

        ' إذا كان Frame — نرسم حدوداً مميزة
        If widgetType = "Frame" Then
            ctrl.BackColor = Color.FromArgb(230, 238, 250)
            UpdateParentFrameCombo()
        End If

        SelectSingle(ctrl)
        UpdateStatusBar("تمت إضافة: " & ctrlName)
        UpdateCodePreview()
    End Sub

    ' =============================================
    ' --- تغيير الصورة ---
    ' =============================================

    Private Sub BtnChangeImage_Click(sender As Object, e As EventArgs) Handles BtnChangeImage.Click
        If selectedControl Is Nothing OrElse selectedControl.Tag.ToString() <> "Image" Then
            MessageBox.Show("حدد عنصر صورة أولاً.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        Using ofd As New OpenFileDialog()
            ofd.Filter = "ملفات الصور (*.png;*.jpg;*.jpeg;*.gif;*.bmp)|*.png;*.jpg;*.jpeg;*.gif;*.bmp"
            If ofd.ShowDialog() = DialogResult.OK Then
                Try
                    Dim pb As PictureBox = DirectCast(selectedControl, PictureBox)
                    pb.Image = Image.FromFile(ofd.FileName)
                    widgetImage(selectedControl.Name) = ofd.FileName
                    UpdateStatusBar("تم تغيير الصورة: " & IO.Path.GetFileName(ofd.FileName))
                    UpdateCodePreview()
                Catch ex As Exception
                    MessageBox.Show("خطأ: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        End Using
    End Sub

    ' =============================================
    ' --- منطق التحديد ---
    ' =============================================

    Private Sub SelectSingle(ctrl As Control)
        If selectedControl IsNot Nothing AndAlso ctrl IsNot selectedControl Then
            AutoSaveCurrentProps()
        End If
        selectedControls.Clear()
        selectedControl = ctrl
        If ctrl IsNot Nothing Then
            selectedControls.Add(ctrl)
            LblSelectedType.Text = "نوع: " & ctrl.Tag.ToString()
            UpdatePropertyFields()
        Else
            LblSelectedType.Text = "لا يوجد تحديد"
            ClearPropertyFields()
        End If
        UpdateAlignButtons()
        DesignPanel.Invalidate()
    End Sub

    Private Sub ToggleSelection(ctrl As Control)
        If selectedControl IsNot Nothing Then AutoSaveCurrentProps()
        If selectedControls.Contains(ctrl) Then
            selectedControls.Remove(ctrl)
            selectedControl = If(selectedControls.Count > 0, selectedControls(selectedControls.Count - 1), Nothing)
        Else
            selectedControls.Add(ctrl)
            selectedControl = ctrl
        End If
        If selectedControls.Count = 1 Then
            LblSelectedType.Text = "نوع: " & selectedControls(0).Tag.ToString()
            UpdatePropertyFields()
        ElseIf selectedControls.Count > 1 Then
            LblSelectedType.Text = "محدد: " & selectedControls.Count & " عناصر"
            ClearPropertyFields()
        Else
            LblSelectedType.Text = "لا يوجد تحديد"
            ClearPropertyFields()
        End If
        UpdateAlignButtons()
        DesignPanel.Invalidate()
    End Sub

    Private Sub UpdateAlignButtons()
        Dim hasMulti As Boolean = selectedControls.Count >= 2
        BtnAlignLeft.Enabled = hasMulti : BtnAlignRight.Enabled = hasMulti
        BtnAlignTop.Enabled = hasMulti : BtnAlignBottom.Enabled = hasMulti
        BtnAlignCenterH.Enabled = hasMulti : BtnAlignCenterV.Enabled = hasMulti
        BtnEqualSpaceH.Enabled = selectedControls.Count >= 3
        BtnEqualSpaceV.Enabled = selectedControls.Count >= 3
    End Sub

    Private Sub AutoSaveCurrentProps()
        If selectedControl Is Nothing Then Return
        Try
            Dim n As String = selectedControl.Name
            selectedControl.Text = PropTextBox.Text
            ' إعادة رسم العناصر المخصصة عند تغيير النص
            Dim ctrlTagA As String = If(selectedControl.Tag IsNot Nothing, selectedControl.Tag.ToString(), "")
            If ctrlTagA = "Checkbutton" OrElse ctrlTagA = "Radiobutton" OrElse ctrlTagA = "Combobox" Then
                selectedControl.Invalidate()
            End If
            Dim xv, yv, wv, hv As Integer
            If Integer.TryParse(PropXBox.Text, xv) AndAlso xv >= 0 Then selectedControl.Left = xv
            If Integer.TryParse(PropYBox.Text, yv) AndAlso yv >= 0 Then selectedControl.Top = yv
            If Integer.TryParse(PropWidthBox.Text, wv) AndAlso wv > 0 Then selectedControl.Width = wv
            If Integer.TryParse(PropHeightBox.Text, hv) AndAlso hv > 0 Then selectedControl.Height = hv
            selectedControl.ForeColor = PnlFgColor.BackColor
            selectedControl.BackColor = PnlBgColor.BackColor
            widgetFgColor(n) = PnlFgColor.BackColor
            widgetBgColor(n) = PnlBgColor.BackColor
            If CmbFontName.SelectedItem IsNot Nothing AndAlso CmbFontSize.SelectedItem IsNot Nothing Then
                Try
                    selectedControl.Font = New Font(CmbFontName.SelectedItem.ToString(),
                        Single.Parse(CmbFontSize.SelectedItem.ToString()),
                        If(ChkFontBold.Checked, FontStyle.Bold, FontStyle.Regular))
                Catch : End Try
            End If
            If CmbRelief.SelectedItem IsNot Nothing Then widgetRelief(n) = CmbRelief.SelectedItem.ToString()
            widgetCommand(n) = PropCommandBox.Text.Trim()
            ' Layout
            If CmbLayoutMode.SelectedItem IsNot Nothing Then widgetLayout(n) = CmbLayoutMode.SelectedItem.ToString()
            If CmbPackSide.SelectedItem IsNot Nothing Then widgetPackSide(n) = CmbPackSide.SelectedItem.ToString()
            If CmbPackFill.SelectedItem IsNot Nothing Then widgetPackFill(n) = CmbPackFill.SelectedItem.ToString()
            widgetPackExpand(n) = ChkPackExpand.Checked
            Dim gr, gc As Integer
            If Integer.TryParse(TxtGridRow.Text, gr) Then widgetGridRow(n) = gr
            If Integer.TryParse(TxtGridCol.Text, gc) Then widgetGridCol(n) = gc
            If CmbGridSticky.SelectedItem IsNot Nothing Then widgetGridSticky(n) = CmbGridSticky.SelectedItem.ToString()
            ' Parent Frame
            If CmbParentFrame.SelectedItem IsNot Nothing Then
                widgetParent(n) = CmbParentFrame.SelectedItem.ToString()
            End If
            DesignPanel.Invalidate()
        Catch : End Try
    End Sub

    ' =============================================
    ' --- أحداث الفأرة على العناصر ---
    ' =============================================

    Private Sub Widget_MouseDown(sender As Object, e As MouseEventArgs)
        If e.Button = MouseButtons.Left Then
            Dim ctrl As Control = DirectCast(sender, Control)
            If Control.ModifierKeys = Keys.Control Then
                ToggleSelection(ctrl) : Return
            End If
            If selectedControls.Count = 1 AndAlso ctrl Is selectedControl Then
                Dim mp As Point = DesignPanel.PointToClient(ctrl.PointToScreen(e.Location))
                activeHandle = GetHandleAtPoint(mp)
                If activeHandle <> ResizeHandle.None Then
                    isResizing = True
                    resizeStartBounds = ctrl.Bounds
                    resizeStartMouse = mp
                    DesignPanel.Capture = True
                    Return
                End If
            End If
            If Not selectedControls.Contains(ctrl) Then SelectSingle(ctrl)
            dragOffset = e.Location
            isDragging = True
        End If
    End Sub

    Private Sub Widget_MouseMove(sender As Object, e As MouseEventArgs)
        Dim ctrl As Control = DirectCast(sender, Control)
        If Not isDragging AndAlso Not isResizing AndAlso
           selectedControls.Count = 1 AndAlso ctrl Is selectedControl Then
            Dim mp As Point = DesignPanel.PointToClient(ctrl.PointToScreen(e.Location))
            ctrl.Cursor = GetCursorForHandle(GetHandleAtPoint(mp))
        End If
        If isDragging AndAlso e.Button = MouseButtons.Left Then
            Dim dx As Integer = e.X - dragOffset.X
            Dim dy As Integer = e.Y - dragOffset.Y
            For Each c As Control In selectedControls
                Dim nx As Integer = Math.Max(0, CInt(Math.Round((c.Left + dx) / GRID_SIZE) * GRID_SIZE))
                Dim ny As Integer = Math.Max(0, CInt(Math.Round((c.Top + dy) / GRID_SIZE) * GRID_SIZE))
                c.Location = New Point(nx, ny)
            Next
            If selectedControls.Count = 1 Then UpdatePropertyFields()
            UpdateStatusBar("الموقع: (" & selectedControl.Left & ", " & selectedControl.Top & ")")
            DesignPanel.Invalidate()
        End If
    End Sub

    Private Sub Widget_MouseUp(sender As Object, e As MouseEventArgs)
        If isDragging OrElse isResizing Then SaveUndoState()
        isDragging = False : isResizing = False
        activeHandle = ResizeHandle.None
        DesignPanel.Capture = False
        UpdateCodePreview()
        DesignPanel.Invalidate()
    End Sub

    ' =============================================
    ' --- أحداث الفأرة على اللوحة ---
    ' =============================================

    Private Sub DesignPanel_MouseDown(sender As Object, e As MouseEventArgs)
        If e.Button = MouseButtons.Left Then
            If selectedControls.Count = 1 AndAlso selectedControl IsNot Nothing Then
                activeHandle = GetHandleAtPoint(e.Location)
                If activeHandle <> ResizeHandle.None Then
                    isResizing = True
                    resizeStartBounds = selectedControl.Bounds
                    resizeStartMouse = e.Location
                    Return
                End If
            End If
            If Control.ModifierKeys <> Keys.Control Then SelectSingle(Nothing)
            isRubberBanding = True
            rubberBandStart = e.Location
            rubberBandRect = New Rectangle(e.Location, Size.Empty)
        End If
    End Sub

    Private Sub DesignPanel_MouseMove(sender As Object, e As MouseEventArgs)
        If isResizing AndAlso selectedControl IsNot Nothing Then
            PerformResize(e.Location) : Return
        End If
        If isRubberBanding Then
            rubberBandRect = New Rectangle(
                Math.Min(e.X, rubberBandStart.X), Math.Min(e.Y, rubberBandStart.Y),
                Math.Abs(e.X - rubberBandStart.X), Math.Abs(e.Y - rubberBandStart.Y))
            DesignPanel.Invalidate() : Return
        End If
        Dim s As Point = SnapToGrid(e.Location)
        UpdateStatusBar("الموقع: (" & s.X & ", " & s.Y & ")")
    End Sub

    Private Sub DesignPanel_MouseUp(sender As Object, e As MouseEventArgs)
        If isResizing Then
            isResizing = False : activeHandle = ResizeHandle.None
            UpdateCodePreview() : DesignPanel.Invalidate() : Return
        End If
        If isRubberBanding Then
            isRubberBanding = False
            If rubberBandRect.Width > 4 AndAlso rubberBandRect.Height > 4 Then
                selectedControls.Clear() : selectedControl = Nothing
                For Each ctrl As Control In DesignPanel.Controls
                    If rubberBandRect.IntersectsWith(ctrl.Bounds) Then
                        selectedControls.Add(ctrl) : selectedControl = ctrl
                    End If
                Next
                If selectedControls.Count = 1 Then
                    LblSelectedType.Text = "نوع: " & selectedControls(0).Tag.ToString()
                    UpdatePropertyFields()
                ElseIf selectedControls.Count > 1 Then
                    LblSelectedType.Text = "محدد: " & selectedControls.Count & " عناصر"
                    ClearPropertyFields()
                End If
                UpdateAlignButtons()
            End If
            rubberBandRect = Rectangle.Empty
            DesignPanel.Invalidate()
        End If
    End Sub

    ' =============================================
    ' --- مقابض تغيير الحجم ---
    ' =============================================

    Private Function GetHandleAtPoint(pt As Point) As ResizeHandle
        If selectedControl Is Nothing Then Return ResizeHandle.None
        Dim b As Rectangle = selectedControl.Bounds
        Dim hs As Integer = HANDLE_SIZE + 2
        If New Rectangle(b.Left - hs, b.Top - hs, hs * 2, hs * 2).Contains(pt) Then
            Return ResizeHandle.TopLeft
        End If
        If New Rectangle(b.Right - hs, b.Top - hs, hs * 2, hs * 2).Contains(pt) Then
            Return ResizeHandle.TopRight
        End If
        If New Rectangle(b.Left - hs, b.Bottom - hs, hs * 2, hs * 2).Contains(pt) Then
            Return ResizeHandle.BottomLeft
        End If
        If New Rectangle(b.Right - hs, b.Bottom - hs, hs * 2, hs * 2).Contains(pt) Then
            Return ResizeHandle.BottomRight
        End If
        If New Rectangle(b.Right - hs, b.Top + hs, hs * 2, b.Height - hs * 2).Contains(pt) Then
            Return ResizeHandle.Right
        End If
        If New Rectangle(b.Left, b.Bottom - hs, b.Width, hs * 2).Contains(pt) Then
            Return ResizeHandle.Bottom
        End If
        Return ResizeHandle.None
    End Function

    Private Sub PerformResize(currentMouse As Point)
        Dim dx As Integer = currentMouse.X - resizeStartMouse.X
        Dim dy As Integer = currentMouse.Y - resizeStartMouse.Y
        Dim minW As Integer = 20 : Dim minH As Integer = 16
        Dim nx As Integer = resizeStartBounds.X
        Dim ny As Integer = resizeStartBounds.Y
        Dim nw As Integer = resizeStartBounds.Width
        Dim nh As Integer = resizeStartBounds.Height
        Select Case activeHandle
            Case ResizeHandle.BottomRight
                nw = Math.Max(minW, SnapVal(resizeStartBounds.Width + dx))
                nh = Math.Max(minH, SnapVal(resizeStartBounds.Height + dy))
            Case ResizeHandle.Right
                nw = Math.Max(minW, SnapVal(resizeStartBounds.Width + dx))
            Case ResizeHandle.Bottom
                nh = Math.Max(minH, SnapVal(resizeStartBounds.Height + dy))
            Case ResizeHandle.BottomLeft
                Dim nr As Integer = resizeStartBounds.Right
                nx = Math.Min(nr - minW, SnapVal(resizeStartBounds.X + dx))
                nw = nr - nx
                nh = Math.Max(minH, SnapVal(resizeStartBounds.Height + dy))
            Case ResizeHandle.TopRight
                Dim nb As Integer = resizeStartBounds.Bottom
                ny = Math.Min(nb - minH, SnapVal(resizeStartBounds.Y + dy))
                nh = nb - ny
                nw = Math.Max(minW, SnapVal(resizeStartBounds.Width + dx))
            Case ResizeHandle.TopLeft
                Dim nr As Integer = resizeStartBounds.Right
                Dim nb As Integer = resizeStartBounds.Bottom
                nx = Math.Min(nr - minW, SnapVal(resizeStartBounds.X + dx))
                ny = Math.Min(nb - minH, SnapVal(resizeStartBounds.Y + dy))
                nw = nr - nx
                nh = nb - ny
        End Select
        selectedControl.SetBounds(nx, ny, nw, nh)
        UpdatePropertyFields()
        UpdateStatusBar("الحجم: " & nw & " × " & nh)
        DesignPanel.Invalidate()
    End Sub

    Private Function GetCursorForHandle(h As ResizeHandle) As Cursor
        Select Case h
            Case ResizeHandle.TopLeft, ResizeHandle.BottomRight
                Return Cursors.SizeNWSE
            Case ResizeHandle.TopRight, ResizeHandle.BottomLeft
                Return Cursors.SizeNESW
            Case ResizeHandle.Right
                Return Cursors.SizeWE
            Case ResizeHandle.Bottom
                Return Cursors.SizeNS
            Case Else
                Return Cursors.SizeAll
        End Select
    End Function

    Private Function SnapVal(v As Integer) As Integer
        Return CInt(Math.Round(v / GRID_SIZE) * GRID_SIZE)
    End Function

    ' =============================================
    ' --- رسم اللوحة ---
    ' =============================================

    ''' <summary>رسم مخصص لعناصر Combobox و Checkbutton و Radiobutton</summary>
    Private Sub CustomWidget_Paint(sender As Object, e As PaintEventArgs)
        Dim ctrl As Control = DirectCast(sender, Control)
        Dim wType As String = If(ctrl.Tag IsNot Nothing, ctrl.Tag.ToString(), "")
        Dim g As Graphics = e.Graphics
        Dim r As Rectangle = ctrl.ClientRectangle
        Dim fg As Color = ctrl.ForeColor
        Dim bg As Color = ctrl.BackColor
        Dim txt As String = ctrl.Text

        g.Clear(bg)

        Select Case wType
            Case "Combobox"
                ' رسم حقل النص
                Dim textRect As New Rectangle(2, 2, r.Width - 22, r.Height - 4)
                Using fgBrush As New SolidBrush(fg)
                    Dim sf As New StringFormat()
                    sf.Alignment = StringAlignment.Near
                    sf.LineAlignment = StringAlignment.Center
                    g.DrawString(txt, ctrl.Font, fgBrush, textRect, sf)
                End Using
                ' رسم زر السهم
                Dim arrowRect As New Rectangle(r.Width - 20, 0, 20, r.Height)
                Using arrowBg As New SolidBrush(Color.FromArgb(220, 220, 220))
                    g.FillRectangle(arrowBg, arrowRect)
                End Using
                ' خط الفاصل
                Using sepPen As New Pen(Color.FromArgb(150, 150, 150), 1)
                    g.DrawLine(sepPen, r.Width - 21, 1, r.Width - 21, r.Height - 2)
                End Using
                ' رسم السهم ▼
                Dim cx As Integer = r.Width - 10
                Dim cy As Integer = r.Height \ 2
                Dim arrowPts() As Point = {
                    New Point(cx - 4, cy - 2),
                    New Point(cx + 4, cy - 2),
                    New Point(cx, cy + 3)
                }
                Using arrowBrush As New SolidBrush(Color.FromArgb(60, 60, 60))
                    g.FillPolygon(arrowBrush, arrowPts)
                End Using
                ' الحدود
                Using borderPen As New Pen(Color.FromArgb(120, 120, 120), 1)
                    g.DrawRectangle(borderPen, 0, 0, r.Width - 1, r.Height - 1)
                End Using

            Case "Checkbutton"
                ' رسم مربع الاختيار
                Dim boxSize As Integer = Math.Min(14, r.Height - 4)
                Dim boxY As Integer = (r.Height - boxSize) \ 2
                Dim boxRect As New Rectangle(3, boxY, boxSize, boxSize)
                Using boxBg As New SolidBrush(Color.White)
                    g.FillRectangle(boxBg, boxRect)
                End Using
                Using boxPen As New Pen(Color.FromArgb(100, 100, 100), 1)
                    g.DrawRectangle(boxPen, boxRect)
                End Using
                ' النص
                Dim txtRect As New Rectangle(boxSize + 8, 0, r.Width - boxSize - 10, r.Height)
                Using fgBrush As New SolidBrush(fg)
                    Dim sf As New StringFormat()
                    sf.LineAlignment = StringAlignment.Center
                    g.DrawString(txt, ctrl.Font, fgBrush, txtRect, sf)
                End Using

            Case "Radiobutton"
                ' رسم دائرة الراديو
                Dim circSize As Integer = Math.Min(14, r.Height - 4)
                Dim circY As Integer = (r.Height - circSize) \ 2
                Dim circRect As New Rectangle(3, circY, circSize, circSize)
                Using circBg As New SolidBrush(Color.White)
                    g.FillEllipse(circBg, circRect)
                End Using
                Using circPen As New Pen(Color.FromArgb(100, 100, 100), 1)
                    g.DrawEllipse(circPen, circRect)
                End Using
                ' النص
                Dim txtRect2 As New Rectangle(circSize + 8, 0, r.Width - circSize - 10, r.Height)
                Using fgBrush As New SolidBrush(fg)
                    Dim sf As New StringFormat()
                    sf.LineAlignment = StringAlignment.Center
                    g.DrawString(txt, ctrl.Font, fgBrush, txtRect2, sf)
                End Using
        End Select
    End Sub

    Private Sub DesignPanel_Paint(sender As Object, e As PaintEventArgs)
        Dim g As Graphics = e.Graphics
        Dim dotBrush As New SolidBrush(Color.FromArgb(160, 160, 150))
        Dim x As Integer = 0
        While x < DesignPanel.Width
            Dim y As Integer = 0
            While y < DesignPanel.Height
                g.FillRectangle(dotBrush, x, y, 1, 1)
                y += GRID_SIZE
            End While
            x += GRID_SIZE
        End While
        dotBrush.Dispose()

        ' رسم تسمية اسم الـ Frame فوقه
        For Each ctrl As Control In DesignPanel.Controls
            If ctrl.Tag IsNot Nothing AndAlso ctrl.Tag.ToString() = "Frame" Then
                Dim frameLabelBrush As New SolidBrush(Color.FromArgb(24, 95, 165))
                g.DrawString("[" & ctrl.Name & "]",
                    New Font("Tahoma", 7, FontStyle.Bold),
                    frameLabelBrush,
                    ctrl.Left + 2, ctrl.Top - 14)
                frameLabelBrush.Dispose()
            End If
        Next

        ' إطارات التحديد
        For Each ctrl As Control In selectedControls
            Dim isPrimary As Boolean = (ctrl Is selectedControl)
            Dim selPen As New Pen(If(isPrimary, Color.FromArgb(24, 95, 165), Color.FromArgb(100, 160, 220)),
                                  If(isPrimary, 1.5F, 1.0F))
            selPen.DashStyle = Drawing2D.DashStyle.Dash
            g.DrawRectangle(selPen, ctrl.Left - 2, ctrl.Top - 2, ctrl.Width + 3, ctrl.Height + 3)
            selPen.Dispose()
        Next

        ' مقابض تغيير الحجم
        If selectedControls.Count = 1 AndAlso selectedControl IsNot Nothing Then
            Dim b As Rectangle = selectedControl.Bounds
            Dim hBrush As New SolidBrush(Color.FromArgb(24, 95, 165))
            Dim hPen As New Pen(Color.White, 1)
            Dim hs As Integer = HANDLE_SIZE
            Dim pts() As Point = {
                New Point(b.Left - hs \ 2, b.Top - hs \ 2),
                New Point(b.Right - hs \ 2, b.Top - hs \ 2),
                New Point(b.Left - hs \ 2, b.Bottom - hs \ 2),
                New Point(b.Right - hs \ 2, b.Bottom - hs \ 2),
                New Point(b.Right - hs \ 2, b.Top + b.Height \ 2 - hs \ 2),
                New Point(b.Left + b.Width \ 2 - hs \ 2, b.Bottom - hs \ 2)
            }
            For Each hp As Point In pts
                g.FillRectangle(hBrush, hp.X, hp.Y, hs, hs)
                g.DrawRectangle(hPen, hp.X, hp.Y, hs, hs)
            Next
            hBrush.Dispose() : hPen.Dispose()
        End If

        ' مستطيل التحديد المطاطي
        If isRubberBanding AndAlso rubberBandRect.Width > 2 Then
            Dim rbPen As New Pen(Color.FromArgb(24, 95, 165), 1)
            rbPen.DashStyle = Drawing2D.DashStyle.Dot
            Dim rbBrush As New SolidBrush(Color.FromArgb(30, 24, 95, 165))
            g.FillRectangle(rbBrush, rubberBandRect)
            g.DrawRectangle(rbPen, rubberBandRect)
            rbPen.Dispose() : rbBrush.Dispose()
        End If
    End Sub

    ' =============================================
    ' --- أدوات المحاذاة ---
    ' =============================================

    Private Sub BtnAlignLeft_Click(sender As Object, e As EventArgs) Handles BtnAlignLeft.Click
        If selectedControls.Count < 2 Then Return
        Dim minX As Integer = Integer.MaxValue
        For Each c As Control In selectedControls
            If c.Left < minX Then minX = c.Left
        Next
        For Each c As Control In selectedControls
            c.Left = minX
        Next
        DesignPanel.Invalidate() : UpdateCodePreview()
        UpdateStatusBar("محاذاة يسار: X = " & minX)
    End Sub

    Private Sub BtnAlignRight_Click(sender As Object, e As EventArgs) Handles BtnAlignRight.Click
        If selectedControls.Count < 2 Then Return
        Dim maxR As Integer = Integer.MinValue
        For Each c As Control In selectedControls
            If c.Right > maxR Then maxR = c.Right
        Next
        For Each c As Control In selectedControls
            c.Left = maxR - c.Width
        Next
        DesignPanel.Invalidate() : UpdateCodePreview()
    End Sub

    Private Sub BtnAlignTop_Click(sender As Object, e As EventArgs) Handles BtnAlignTop.Click
        If selectedControls.Count < 2 Then Return
        Dim minY As Integer = Integer.MaxValue
        For Each c As Control In selectedControls
            If c.Top < minY Then minY = c.Top
        Next
        For Each c As Control In selectedControls
            c.Top = minY
        Next
        DesignPanel.Invalidate() : UpdateCodePreview()
    End Sub

    Private Sub BtnAlignBottom_Click(sender As Object, e As EventArgs) Handles BtnAlignBottom.Click
        If selectedControls.Count < 2 Then Return
        Dim maxB As Integer = Integer.MinValue
        For Each c As Control In selectedControls
            If c.Bottom > maxB Then maxB = c.Bottom
        Next
        For Each c As Control In selectedControls
            c.Top = maxB - c.Height
        Next
        DesignPanel.Invalidate() : UpdateCodePreview()
    End Sub

    Private Sub BtnAlignCenterH_Click(sender As Object, e As EventArgs) Handles BtnAlignCenterH.Click
        If selectedControls.Count < 2 OrElse selectedControl Is Nothing Then Return
        Dim cy As Integer = selectedControl.Top + selectedControl.Height \ 2
        For Each c As Control In selectedControls
            c.Top = cy - c.Height \ 2
        Next
        DesignPanel.Invalidate() : UpdateCodePreview()
    End Sub

    Private Sub BtnAlignCenterV_Click(sender As Object, e As EventArgs) Handles BtnAlignCenterV.Click
        If selectedControls.Count < 2 OrElse selectedControl Is Nothing Then Return
        Dim cx As Integer = selectedControl.Left + selectedControl.Width \ 2
        For Each c As Control In selectedControls
            c.Left = cx - c.Width \ 2
        Next
        DesignPanel.Invalidate() : UpdateCodePreview()
    End Sub

    Private Sub BtnEqualSpaceH_Click(sender As Object, e As EventArgs) Handles BtnEqualSpaceH.Click
        If selectedControls.Count < 3 Then Return
        Dim sorted As New List(Of Control)(selectedControls)
        sorted.Sort(Function(a, b) a.Left.CompareTo(b.Left))
        Dim totalSpace As Integer = sorted(sorted.Count - 1).Left - sorted(0).Right
        Dim spacing As Integer = totalSpace \ (sorted.Count - 1)
        Dim cx As Integer = sorted(0).Right
        For i As Integer = 1 To sorted.Count - 2
            sorted(i).Left = cx + spacing : cx = sorted(i).Right
        Next
        DesignPanel.Invalidate() : UpdateCodePreview()
    End Sub

    Private Sub BtnEqualSpaceV_Click(sender As Object, e As EventArgs) Handles BtnEqualSpaceV.Click
        If selectedControls.Count < 3 Then Return
        Dim sorted As New List(Of Control)(selectedControls)
        sorted.Sort(Function(a, b) a.Top.CompareTo(b.Top))
        Dim totalSpace As Integer = sorted(sorted.Count - 1).Top - sorted(0).Bottom
        Dim spacing As Integer = totalSpace \ (sorted.Count - 1)
        Dim cy As Integer = sorted(0).Bottom
        For i As Integer = 1 To sorted.Count - 2
            sorted(i).Top = cy + spacing : cy = sorted(i).Bottom
        Next
        DesignPanel.Invalidate() : UpdateCodePreview()
    End Sub

    ' =============================================
    ' --- لوحة الخصائص ---
    ' =============================================

    Private Sub UpdatePropertyFields()
        If selectedControl Is Nothing Then Return
        Dim n As String = selectedControl.Name
        PropNameBox.Text = n
        PropTextBox.Text = selectedControl.Text
        PropXBox.Text = selectedControl.Left.ToString()
        PropYBox.Text = selectedControl.Top.ToString()
        PropWidthBox.Text = selectedControl.Width.ToString()
        PropHeightBox.Text = selectedControl.Height.ToString()
        PnlFgColor.BackColor = If(widgetFgColor.ContainsKey(n), widgetFgColor(n), Color.Black)
        PnlBgColor.BackColor = If(widgetBgColor.ContainsKey(n), widgetBgColor(n), selectedControl.BackColor)
        Dim f As Font = selectedControl.Font
        If CmbFontName.Items.Contains(f.Name) Then CmbFontName.SelectedItem = f.Name Else CmbFontName.SelectedIndex = 0
        Dim sz As String = CInt(f.Size).ToString()
        If CmbFontSize.Items.Contains(sz) Then CmbFontSize.SelectedItem = sz
        ChkFontBold.Checked = f.Bold
        Dim rel As String = If(widgetRelief.ContainsKey(n), widgetRelief(n), "flat")
        If CmbRelief.Items.Contains(rel) Then CmbRelief.SelectedItem = rel
        PropCommandBox.Text = If(widgetCommand.ContainsKey(n), widgetCommand(n), "")

        ' Layout
        Dim lm As String = If(widgetLayout.ContainsKey(n), widgetLayout(n), "place")
        If CmbLayoutMode.Items.Contains(lm) Then CmbLayoutMode.SelectedItem = lm
        Dim ps As String = If(widgetPackSide.ContainsKey(n), widgetPackSide(n), "top")
        If CmbPackSide.Items.Contains(ps) Then CmbPackSide.SelectedItem = ps
        Dim pf As String = If(widgetPackFill.ContainsKey(n), widgetPackFill(n), "none")
        If CmbPackFill.Items.Contains(pf) Then CmbPackFill.SelectedItem = pf
        ChkPackExpand.Checked = If(widgetPackExpand.ContainsKey(n), widgetPackExpand(n), False)
        TxtGridRow.Text = If(widgetGridRow.ContainsKey(n), widgetGridRow(n).ToString(), "0")
        TxtGridCol.Text = If(widgetGridCol.ContainsKey(n), widgetGridCol(n).ToString(), "0")
        Dim gs As String = If(widgetGridSticky.ContainsKey(n), widgetGridSticky(n), "")
        If CmbGridSticky.Items.Contains(gs) Then CmbGridSticky.SelectedItem = gs

        ' Parent Frame
        UpdateParentFrameCombo()
        Dim par As String = If(widgetParent.ContainsKey(n), widgetParent(n), "root")
        If CmbParentFrame.Items.Contains(par) Then CmbParentFrame.SelectedItem = par

        ' إظهار/إخفاء زر تغيير الصورة
        BtnChangeImage.Visible = (selectedControl.Tag IsNot Nothing AndAlso
                                  selectedControl.Tag.ToString() = "Image")
        ' إظهار خيارات الـ Layout الصحيحة
        PnlPlaceOptions.Visible = (lm = "place")
        PnlPackOptions.Visible = (lm = "pack")
        PnlGridOptions.Visible = (lm = "grid")
    End Sub

    Private Sub ClearPropertyFields()
        PropNameBox.Text = "" : PropTextBox.Text = ""
        PropXBox.Text = "" : PropYBox.Text = ""
        PropWidthBox.Text = "" : PropHeightBox.Text = ""
        PropCommandBox.Text = ""
        PnlFgColor.BackColor = Color.Black
        PnlBgColor.BackColor = Color.WhiteSmoke
        If CmbRelief.Items.Count > 0 Then CmbRelief.SelectedIndex = 0
        If CmbLayoutMode.Items.Count > 0 Then CmbLayoutMode.SelectedIndex = 0
        BtnChangeImage.Visible = False
    End Sub

    Private Sub BtnApplyProps_Click(sender As Object, e As EventArgs) Handles BtnApplyProps.Click
        If selectedControl Is Nothing Then
            MessageBox.Show("لم يتم تحديد أي عنصر.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        Try
            Dim oldName As String = selectedControl.Name
            Dim newName As String = PropNameBox.Text.Trim()
            If newName <> "" AndAlso newName <> oldName Then
                RenameInDict(widgetFgColor, oldName, newName)
                RenameInDict(widgetBgColor, oldName, newName)
                RenameInDict(widgetRelief, oldName, newName)
                RenameInDict(widgetCommand, oldName, newName)
                RenameInDict(widgetLayout, oldName, newName)
                RenameInDict(widgetPackSide, oldName, newName)
                RenameInDict(widgetPackFill, oldName, newName)
                RenameInDict(widgetPackExpand, oldName, newName)
                RenameInDict(widgetGridRow, oldName, newName)
                RenameInDict(widgetGridCol, oldName, newName)
                RenameInDict(widgetGridSticky, oldName, newName)
                RenameInDict(widgetParent, oldName, newName)
                RenameInDict(widgetImage, oldName, newName)
                selectedControl.Name = newName
                UpdateParentFrameCombo()
            End If
            AutoSaveCurrentProps()
            UpdateCodePreview()
            UpdateStatusBar("تم تطبيق الخصائص على: " & selectedControl.Name)
        Catch ex As Exception
            MessageBox.Show("خطأ: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub RenameInDict(Of T)(dict As Dictionary(Of String, T), oldKey As String, newKey As String)
        If dict.ContainsKey(oldKey) Then
            dict(newKey) = dict(oldKey) : dict.Remove(oldKey)
        End If
    End Sub

    Private Sub PnlFgColor_Click(sender As Object, e As EventArgs) Handles PnlFgColor.Click
        Using cd As New ColorDialog()
            cd.Color = PnlFgColor.BackColor
            If cd.ShowDialog() = DialogResult.OK Then PnlFgColor.BackColor = cd.Color
        End Using
    End Sub

    Private Sub PnlBgColor_Click(sender As Object, e As EventArgs) Handles PnlBgColor.Click
        Using cd As New ColorDialog()
            cd.Color = PnlBgColor.BackColor
            If cd.ShowDialog() = DialogResult.OK Then PnlBgColor.BackColor = cd.Color
        End Using
    End Sub

    Private Sub PnlFormBg_Click(sender As Object, e As EventArgs) Handles PnlFormBg.Click
        Using cd As New ColorDialog()
            cd.Color = formBgColor : cd.FullOpen = True
            If cd.ShowDialog() = DialogResult.OK Then
                formBgColor = cd.Color
                PnlFormBg.BackColor = cd.Color
                DesignPanel.BackColor = cd.Color
                UpdateStatusBar("لون الواجهة: " & ColorToHex(cd.Color))
            End If
        End Using
    End Sub

    ' =============================================
    ' --- حذف ---
    ' =============================================

    Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles BtnDelete.Click
        If selectedControls.Count = 0 Then Return
        For Each ctrl As Control In selectedControls
            Dim n As String = ctrl.Name
            widgetFgColor.Remove(n) : widgetBgColor.Remove(n)
            widgetRelief.Remove(n) : widgetCommand.Remove(n)
            widgetLayout.Remove(n) : widgetPackSide.Remove(n)
            widgetPackFill.Remove(n) : widgetPackExpand.Remove(n)
            widgetGridRow.Remove(n) : widgetGridCol.Remove(n)
            widgetGridSticky.Remove(n) : widgetParent.Remove(n)
            widgetImage.Remove(n)
            DesignPanel.Controls.Remove(ctrl)
        Next
        Dim cnt As Integer = selectedControls.Count
        selectedControls.Clear()
        SelectSingle(Nothing)
        UpdateParentFrameCombo()
        UpdateCodePreview()
        UpdateStatusBar("تم حذف " & cnt & " عنصر")
    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
        If keyData = Keys.Delete AndAlso selectedControls.Count > 0 Then
            SaveUndoState()
            BtnDelete_Click(Nothing, Nothing)
            Return True
        End If
        If keyData = (Keys.Control Or Keys.A) Then
            selectedControls.Clear()
            For Each ctrl As Control In DesignPanel.Controls
                selectedControls.Add(ctrl)
                selectedControl = ctrl
            Next
            If selectedControls.Count > 0 Then
                LblSelectedType.Text = "محدد: " & selectedControls.Count & " عناصر"
                ClearPropertyFields()
            End If
            UpdateAlignButtons()
            DesignPanel.Invalidate()
            Return True
        End If
        If keyData = (Keys.Control Or Keys.C) AndAlso selectedControls.Count > 0 Then
            CopySelectedControls()
            Return True
        End If
        If keyData = (Keys.Control Or Keys.V) AndAlso clipboardItems.Count > 0 Then
            SaveUndoState()
            PasteControls()
            Return True
        End If
        If keyData = (Keys.Control Or Keys.Z) Then
            PerformUndo()
            Return True
        End If
        If keyData = (Keys.Control Or Keys.Y) Then
            PerformRedo()
            Return True
        End If
        If keyData = (Keys.Control Or Keys.Oemplus) OrElse keyData = (Keys.Control Or Keys.Add) Then
            ZoomIn()
            Return True
        End If
        If keyData = (Keys.Control Or Keys.OemMinus) OrElse keyData = (Keys.Control Or Keys.Subtract) Then
            ZoomOut()
            Return True
        End If
        If keyData = (Keys.Control Or Keys.D0) OrElse keyData = (Keys.Control Or Keys.NumPad0) Then
            ZoomReset()
            Return True
        End If
        Return MyBase.ProcessCmdKey(msg, keyData)
    End Function


    ' =============================================
    ' --- نسخ ولصق ---
    ' =============================================

    Private Sub CopySelectedControls()
        clipboardItems.Clear()
        For Each ctrl As Control In selectedControls
            Dim n As String = ctrl.Name
            Dim wd As New WidgetData()
            wd.WidgetType = If(ctrl.Tag IsNot Nothing, ctrl.Tag.ToString(), "")
            wd.Name = n
            wd.Text = ctrl.Text
            wd.X = ctrl.Left : wd.Y = ctrl.Top
            wd.W = ctrl.Width : wd.H = ctrl.Height
            wd.FgColor = ColorToHex(ctrl.ForeColor)
            wd.BgColor = ColorToHex(ctrl.BackColor)
            wd.FontName = ctrl.Font.Name
            wd.FontSize = CInt(ctrl.Font.Size)
            wd.FontBold = ctrl.Font.Bold
            wd.Relief = If(widgetRelief.ContainsKey(n), widgetRelief(n), "flat")
            wd.CommandName = If(widgetCommand.ContainsKey(n), widgetCommand(n), "")
            wd.LayoutMode = If(widgetLayout.ContainsKey(n), widgetLayout(n), "place")
            wd.PackSide = If(widgetPackSide.ContainsKey(n), widgetPackSide(n), "top")
            wd.PackFill = If(widgetPackFill.ContainsKey(n), widgetPackFill(n), "none")
            wd.PackExpand = If(widgetPackExpand.ContainsKey(n), widgetPackExpand(n), False)
            wd.GridRow = If(widgetGridRow.ContainsKey(n), widgetGridRow(n), 0)
            wd.GridCol = If(widgetGridCol.ContainsKey(n), widgetGridCol(n), 0)
            wd.GridSticky = If(widgetGridSticky.ContainsKey(n), widgetGridSticky(n), "")
            wd.ParentFrame = If(widgetParent.ContainsKey(n), widgetParent(n), "root")
            wd.ImagePath = If(widgetImage.ContainsKey(n), widgetImage(n), "")
            clipboardItems.Add(wd)
        Next
        UpdateStatusBar("تم نسخ " & clipboardItems.Count & " عنصر — Ctrl+V للصق")
    End Sub

    Private Sub PasteControls()
        Dim offset As Integer = 16
        selectedControls.Clear()
        selectedControl = Nothing
        For Each wd As WidgetData In clipboardItems
            If Not controlCounters.ContainsKey(wd.WidgetType) Then
                controlCounters(wd.WidgetType) = 0
            End If
            Dim newWd As New WidgetData()
            newWd.WidgetType = wd.WidgetType
            newWd.Text = wd.Text
            newWd.X = Math.Min(wd.X + offset, DesignPanel.Width - wd.W)
            newWd.Y = Math.Min(wd.Y + offset, DesignPanel.Height - wd.H)
            newWd.W = wd.W : newWd.H = wd.H
            newWd.FgColor = wd.FgColor : newWd.BgColor = wd.BgColor
            newWd.FontName = wd.FontName : newWd.FontSize = wd.FontSize
            newWd.FontBold = wd.FontBold : newWd.Relief = wd.Relief
            newWd.CommandName = wd.CommandName : newWd.LayoutMode = wd.LayoutMode
            newWd.PackSide = wd.PackSide : newWd.PackFill = wd.PackFill
            newWd.PackExpand = wd.PackExpand : newWd.GridRow = wd.GridRow
            newWd.GridCol = wd.GridCol : newWd.GridSticky = wd.GridSticky
            newWd.ParentFrame = wd.ParentFrame : newWd.ImagePath = wd.ImagePath
            ' Name يُولد داخل AddWidgetToCanvas
            newWd.Name = ""
            controlCounters(wd.WidgetType) -= 1
            If newWd.WidgetType = "Image" Then
                AddImageWidget(New Point(newWd.X, newWd.Y), newWd)
            Else
                AddWidgetToCanvas(newWd.WidgetType, New Point(newWd.X, newWd.Y), newWd)
            End If
            If DesignPanel.Controls.Count > 0 Then
                Dim pasted As Control = DesignPanel.Controls(DesignPanel.Controls.Count - 1)
                If Not selectedControls.Contains(pasted) Then
                    selectedControls.Add(pasted)
                    selectedControl = pasted
                End If
            End If
        Next
        LblSelectedType.Text = "محدد: " & selectedControls.Count & " عناصر ملصقة"
        UpdateAlignButtons()
        DesignPanel.Invalidate()
        UpdateCodePreview()
        UpdateStatusBar("تم لصق " & clipboardItems.Count & " عنصر")
    End Sub

    ' =============================================
    ' --- Undo / Redo ---
    ' =============================================

    Private Sub SaveUndoState()
        undoStack.Push(SerializeCurrentState())
        redoStack.Clear()
        UpdateUndoRedoButtons()
    End Sub

    Private Sub PerformUndo()
        If undoStack.Count = 0 Then
            UpdateStatusBar("لا يوجد ما يمكن التراجع عنه")
            Return
        End If
        redoStack.Push(SerializeCurrentState())
        RestoreState(undoStack.Pop())
        UpdateUndoRedoButtons()
        UpdateStatusBar("تم التراجع | Ctrl+Y للإعادة")
    End Sub

    Private Sub PerformRedo()
        If redoStack.Count = 0 Then
            UpdateStatusBar("لا يوجد ما يمكن إعادته")
            Return
        End If
        undoStack.Push(SerializeCurrentState())
        RestoreState(redoStack.Pop())
        UpdateUndoRedoButtons()
        UpdateStatusBar("تم الإعادة")
    End Sub

    Private Sub UpdateUndoRedoButtons()
        BtnUndo.Enabled = undoStack.Count > 0
        BtnRedo.Enabled = redoStack.Count > 0
        BtnUndo.ToolTipText = "تراجع (" & undoStack.Count & ") Ctrl+Z"
        BtnRedo.ToolTipText = "إعادة (" & redoStack.Count & ") Ctrl+Y"
    End Sub

    Private Function SerializeCurrentState() As String
        Dim sb As New StringBuilder()
        sb.Append("{""w"":[")
        Dim ctrls As New List(Of Control)()
        For Each c As Control In DesignPanel.Controls
            ctrls.Add(c)
        Next
        For i As Integer = 0 To ctrls.Count - 1
            Dim ctrl As Control = ctrls(i)
            Dim n As String = ctrl.Name
            Dim wt As String = If(ctrl.Tag IsNot Nothing, ctrl.Tag.ToString(), "")
            If i > 0 Then sb.Append(",")
            sb.Append("{")
            sb.Append(Chr(34) & "t" & Chr(34) & ":" & Chr(34) & EscapeJson(wt) & Chr(34) & ",")
            sb.Append(Chr(34) & "n" & Chr(34) & ":" & Chr(34) & EscapeJson(n) & Chr(34) & ",")
            sb.Append(Chr(34) & "x" & Chr(34) & ":" & Chr(34) & EscapeJson(ctrl.Text) & Chr(34) & ",")
            sb.Append(Chr(34) & "l" & Chr(34) & ":" & ctrl.Left & ",")
            sb.Append(Chr(34) & "p" & Chr(34) & ":" & ctrl.Top & ",")
            sb.Append(Chr(34) & "ww" & Chr(34) & ":" & ctrl.Width & ",")
            sb.Append(Chr(34) & "hh" & Chr(34) & ":" & ctrl.Height & ",")
            sb.Append(Chr(34) & "fg" & Chr(34) & ":" & Chr(34) & ColorToHex(ctrl.ForeColor) & Chr(34) & ",")
            sb.Append(Chr(34) & "bg" & Chr(34) & ":" & Chr(34) & ColorToHex(ctrl.BackColor) & Chr(34) & ",")
            sb.Append(Chr(34) & "fn" & Chr(34) & ":" & Chr(34) & EscapeJson(ctrl.Font.Name) & Chr(34) & ",")
            sb.Append(Chr(34) & "fs" & Chr(34) & ":" & CInt(ctrl.Font.Size) & ",")
            sb.Append(Chr(34) & "fb" & Chr(34) & ":" & If(ctrl.Font.Bold, "true", "false") & ",")
            sb.Append(Chr(34) & "r" & Chr(34) & ":" & Chr(34) & EscapeJson(If(widgetRelief.ContainsKey(n), widgetRelief(n), "flat")) & Chr(34) & ",")
            sb.Append(Chr(34) & "cm" & Chr(34) & ":" & Chr(34) & EscapeJson(If(widgetCommand.ContainsKey(n), widgetCommand(n), "")) & Chr(34) & ",")
            sb.Append(Chr(34) & "lm" & Chr(34) & ":" & Chr(34) & EscapeJson(If(widgetLayout.ContainsKey(n), widgetLayout(n), "place")) & Chr(34) & ",")
            sb.Append(Chr(34) & "ps" & Chr(34) & ":" & Chr(34) & EscapeJson(If(widgetPackSide.ContainsKey(n), widgetPackSide(n), "top")) & Chr(34) & ",")
            sb.Append(Chr(34) & "pf" & Chr(34) & ":" & Chr(34) & EscapeJson(If(widgetPackFill.ContainsKey(n), widgetPackFill(n), "none")) & Chr(34) & ",")
            sb.Append(Chr(34) & "pe" & Chr(34) & ":" & If(widgetPackExpand.ContainsKey(n) AndAlso widgetPackExpand(n), "true", "false") & ",")
            sb.Append(Chr(34) & "gr" & Chr(34) & ":" & If(widgetGridRow.ContainsKey(n), widgetGridRow(n), 0) & ",")
            sb.Append(Chr(34) & "gc" & Chr(34) & ":" & If(widgetGridCol.ContainsKey(n), widgetGridCol(n), 0) & ",")
            sb.Append(Chr(34) & "gs" & Chr(34) & ":" & Chr(34) & EscapeJson(If(widgetGridSticky.ContainsKey(n), widgetGridSticky(n), "")) & Chr(34) & ",")
            sb.Append(Chr(34) & "par" & Chr(34) & ":" & Chr(34) & EscapeJson(If(widgetParent.ContainsKey(n), widgetParent(n), "root")) & Chr(34) & ",")
            sb.Append(Chr(34) & "img" & Chr(34) & ":" & Chr(34) & EscapeJson(If(widgetImage.ContainsKey(n), widgetImage(n), "")) & Chr(34))
            sb.Append("}")
        Next
        sb.Append("]}")
        Return sb.ToString()
    End Function

    Private Sub RestoreState(json As String)
        DesignPanel.Controls.Clear()
        controlCounters.Clear()
        widgetFgColor.Clear() : widgetBgColor.Clear() : widgetRelief.Clear()
        widgetCommand.Clear() : widgetLayout.Clear() : widgetPackSide.Clear()
        widgetPackFill.Clear() : widgetPackExpand.Clear() : widgetGridRow.Clear()
        widgetGridCol.Clear() : widgetGridSticky.Clear() : widgetParent.Clear()
        widgetImage.Clear()
        For Each block As String In SplitJsonObjects(ExtractJsonArray(json, "w"))
            Dim wd As New WidgetData()
            wd.WidgetType = ParseJsonString(block, "t")
            wd.Name = ParseJsonString(block, "n")
            wd.Text = ParseJsonString(block, "x")
            wd.X = ParseJsonInt(block, "l", 0)
            wd.Y = ParseJsonInt(block, "p", 0)
            wd.W = ParseJsonInt(block, "ww", 100)
            wd.H = ParseJsonInt(block, "hh", 28)
            wd.FgColor = ParseJsonString(block, "fg")
            wd.BgColor = ParseJsonString(block, "bg")
            wd.FontName = ParseJsonString(block, "fn")
            wd.FontSize = ParseJsonInt(block, "fs", 9)
            wd.FontBold = ParseJsonBool(block, "fb")
            wd.Relief = ParseJsonString(block, "r")
            wd.CommandName = ParseJsonString(block, "cm")
            wd.LayoutMode = ParseJsonString(block, "lm")
            wd.PackSide = ParseJsonString(block, "ps")
            wd.PackFill = ParseJsonString(block, "pf")
            wd.PackExpand = ParseJsonBool(block, "pe")
            wd.GridRow = ParseJsonInt(block, "gr", 0)
            wd.GridCol = ParseJsonInt(block, "gc", 0)
            wd.GridSticky = ParseJsonString(block, "gs")
            wd.ParentFrame = ParseJsonString(block, "par")
            wd.ImagePath = ParseJsonString(block, "img")
            If wd.WidgetType <> "" Then
                If Not controlCounters.ContainsKey(wd.WidgetType) Then
                    controlCounters(wd.WidgetType) = 0
                End If
                controlCounters(wd.WidgetType) -= 1
                If wd.WidgetType = "Image" Then
                    AddImageWidget(New Point(wd.X, wd.Y), wd)
                Else
                    AddWidgetToCanvas(wd.WidgetType, New Point(wd.X, wd.Y), wd)
                End If
            End If
        Next
        SelectSingle(Nothing)
        UpdateParentFrameCombo()
        UpdateCodePreview()
        DesignPanel.Invalidate()
    End Sub

    ' =============================================
    ' --- Zoom ---
    ' =============================================

    Private Sub ZoomIn()
        If zoomFactor < ZOOM_MAX Then
            zoomFactor = Math.Min(ZOOM_MAX, zoomFactor + 0.25F)
            ApplyZoom()
        End If
    End Sub

    Private Sub ZoomOut()
        If zoomFactor > ZOOM_MIN Then
            zoomFactor = Math.Max(ZOOM_MIN, zoomFactor - 0.25F)
            ApplyZoom()
        End If
    End Sub

    Private Sub ZoomReset()
        zoomFactor = 1.0F
        ApplyZoom()
    End Sub

    Private Sub ApplyZoom()
        Dim baseW As Integer = 600
        Dim baseH As Integer = 500
        Integer.TryParse(TxtFormWidth.Text, baseW)
        Integer.TryParse(TxtFormHeight.Text, baseH)
        DesignPanel.Size = New Size(CInt(baseW * zoomFactor), CInt(baseH * zoomFactor))
        LblZoom.Text = CInt(zoomFactor * 100) & "%"
        DesignPanel.Invalidate()
        UpdateStatusBar("تكبير: " & CInt(zoomFactor * 100) & "% | Ctrl+[+/-/0]")
    End Sub

    Private Sub BtnZoomIn_Click(sender As Object, e As EventArgs) Handles BtnZoomIn.Click
        ZoomIn()
    End Sub

    Private Sub BtnZoomOut_Click(sender As Object, e As EventArgs) Handles BtnZoomOut.Click
        ZoomOut()
    End Sub

    Private Sub BtnZoomReset_Click(sender As Object, e As EventArgs) Handles BtnZoomReset.Click
        ZoomReset()
    End Sub

    Private Sub BtnUndo_Click(sender As Object, e As EventArgs) Handles BtnUndo.Click
        PerformUndo()
    End Sub

    Private Sub BtnRedo_Click(sender As Object, e As EventArgs) Handles BtnRedo.Click
        PerformRedo()
    End Sub

    ' =============================================
    ' --- توليد كود بايثون (OOP + place/pack/grid + Frame + Image) ---
    ' =============================================

    Private Sub UpdateCodePreview()
        CodePreviewBox.Text = GeneratePythonCode()
    End Sub

    Private Function GeneratePythonCode() As String
        Dim sb As New StringBuilder()
        Dim formTitle As String = If(TxtFormTitle.Text.Trim() = "", "واجهتي", TxtFormTitle.Text.Trim())
        Dim formW As String = If(TxtFormWidth.Text.Trim() = "", "600", TxtFormWidth.Text.Trim())
        Dim formH As String = If(TxtFormHeight.Text.Trim() = "", "500", TxtFormHeight.Text.Trim())
        Dim fgBg As String = "bg='" & ColorToHex(formBgColor) & "'"

        sb.AppendLine("# -*- coding: utf-8 -*-")
        sb.AppendLine("# تم إنشاء هذا الكود بواسطة PyForm Builder")
        sb.AppendLine("import tkinter as tk")
        sb.AppendLine("from tkinter import ttk")

        ' فحص إذا كانت هناك صور
        Dim hasImages As Boolean = False
        For Each ctrl As Control In DesignPanel.Controls
            If ctrl.Tag IsNot Nothing AndAlso ctrl.Tag.ToString() = "Image" Then
                hasImages = True : Exit For
            End If
        Next
        If hasImages Then sb.AppendLine("from PIL import Image, ImageTk  # pip install Pillow")

        sb.AppendLine("")
        sb.AppendLine("")
        sb.AppendLine("class MainApp(tk.Tk):")
        sb.AppendLine("    def __init__(self):")
        sb.AppendLine("        super().__init__()")
        sb.AppendLine("        self.title('" & formTitle.Replace("'", "\'") & "')")
        sb.AppendLine("        self.geometry('" & formW & "x" & formH & "')")
        sb.AppendLine("        self.configure(" & fgBg & ")")
        sb.AppendLine("        self.resizable(True, True)")
        sb.AppendLine("        self._build_ui()")
        sb.AppendLine("")
        sb.AppendLine("    def _build_ui(self):")

        If DesignPanel.Controls.Count = 0 Then
            sb.AppendLine("        pass")
            sb.AppendLine("")
        End If

        Dim commandDefs As New List(Of String)
        If hasImages Then sb.AppendLine("        self._images = []  # للحفاظ على مراجع الصور")

        ' أولاً: توليد الـ Frames لأنها آباء
        For Each ctrl As Control In DesignPanel.Controls
            If ctrl.Tag IsNot Nothing AndAlso ctrl.Tag.ToString() = "Frame" Then
                GenerateWidgetCode(sb, ctrl, commandDefs)
            End If
        Next

        ' ثانياً: بقية العناصر
        For Each ctrl As Control In DesignPanel.Controls
            If ctrl.Tag Is Nothing OrElse ctrl.Tag.ToString() <> "Frame" Then
                GenerateWidgetCode(sb, ctrl, commandDefs)
            End If
        Next

        ' دوال الأحداث
        If commandDefs.Count > 0 Then
            sb.AppendLine("")
            sb.AppendLine("    # --- دوال الأحداث ---")
            For Each cmd As String In commandDefs
                sb.AppendLine("    def " & cmd & "(self):")
                sb.AppendLine("        pass  # اكتب الكود هنا")
                sb.AppendLine("")
            Next
        End If

        sb.AppendLine("")
        sb.AppendLine("if __name__ == '__main__':")
        sb.AppendLine("    app = MainApp()")
        sb.AppendLine("    app.mainloop()")
        Return sb.ToString()
    End Function

    Private Sub GenerateWidgetCode(sb As StringBuilder, ctrl As Control, commandDefs As List(Of String))
        Dim wType As String = If(ctrl.Tag IsNot Nothing, ctrl.Tag.ToString(), "")
        Dim n As String = ctrl.Name
        Dim txt As String = ctrl.Text.Replace("'", "\'")
        Dim x As Integer = ctrl.Left : Dim y As Integer = ctrl.Top
        Dim w As Integer = ctrl.Width : Dim h As Integer = ctrl.Height

        ' تحديد الأب
        Dim parentName As String = If(widgetParent.ContainsKey(n), widgetParent(n), "root")
        Dim parentRef As String = If(parentName = "root", "self", "self." & parentName)

        ' الخصائص المشتركة
        Dim fg As String = ColorToHex(If(widgetFgColor.ContainsKey(n), widgetFgColor(n), ctrl.ForeColor))
        Dim bg As String = ColorToHex(If(widgetBgColor.ContainsKey(n), widgetBgColor(n), ctrl.BackColor))
        Dim fBoldStr As String = If(ctrl.Font.Bold, ", 'bold'", "")
        Dim fontStr As String = "('" & ctrl.Font.Name & "', " & CInt(ctrl.Font.Size) & fBoldStr & ")"
        Dim relief As String = If(widgetRelief.ContainsKey(n), widgetRelief(n), "flat")
        Dim cmd As String = If(widgetCommand.ContainsKey(n), widgetCommand(n), "")
        Dim cmdStr As String = If(cmd <> "", ", command=self." & cmd, "")
        If cmd <> "" AndAlso Not commandDefs.Contains(cmd) Then commandDefs.Add(cmd)
        Dim props As String = "fg='" & fg & "', bg='" & bg & "', font=" & fontStr & ", relief='" & relief & "'"

        ' توليد تعريف العنصر
        Select Case wType
            Case "Label"
                sb.AppendLine("        self." & n & " = tk.Label(" & parentRef & ", text='" & txt & "', " & props & ")")
            Case "Button"
                sb.AppendLine("        self." & n & " = tk.Button(" & parentRef & ", text='" & txt & "', " & props & cmdStr & ")")
            Case "Entry"
                sb.AppendLine("        self.var_" & n & " = tk.StringVar()")
                sb.AppendLine("        self." & n & " = tk.Entry(" & parentRef & ", textvariable=self.var_" & n & ", " & props & ")")
            Case "Text"
                sb.AppendLine("        self." & n & " = tk.Text(" & parentRef & ", width=" & CInt(w / 8) & ", height=" & CInt(h / 16) & ", wrap=tk.WORD, " & props & ")")
            Case "Checkbutton"
                sb.AppendLine("        self.var_" & n & " = tk.BooleanVar()")
                sb.AppendLine("        self." & n & " = tk.Checkbutton(" & parentRef & ", text='" & txt & "', variable=self.var_" & n & ", " & props & ")")
            Case "Radiobutton"
                sb.AppendLine("        self." & n & " = tk.Radiobutton(" & parentRef & ", text='" & txt & "', value='" & n & "', " & props & ")")
            Case "Listbox"
                sb.AppendLine("        self." & n & " = tk.Listbox(" & parentRef & ", width=" & CInt(w / 8) & ", height=" & CInt(h / 16) & ", " & props & ")")
            Case "Combobox"
                sb.AppendLine("        self.var_" & n & " = tk.StringVar()")
                sb.AppendLine("        self." & n & " = ttk.Combobox(" & parentRef & ", textvariable=self.var_" & n & ", width=" & CInt(w / 8) & ")")
            Case "Frame"
                sb.AppendLine("        self." & n & " = tk.LabelFrame(" & parentRef & ", text='" & txt & "', width=" & w & ", height=" & h & ", " & props & ")")
                sb.AppendLine("        self." & n & ".pack_propagate(False)")
            Case "Scale"
                sb.AppendLine("        self." & n & " = tk.Scale(" & parentRef & ", from_=0, to=100, orient=tk.HORIZONTAL, length=" & w & ", " & props & ")")
            Case "Spinbox"
                sb.AppendLine("        self.var_" & n & " = tk.StringVar()")
                sb.AppendLine("        self." & n & " = tk.Spinbox(" & parentRef & ", from_=0, to=100, textvariable=self.var_" & n & ", width=" & CInt(w / 8) & ")")
            Case "Canvas"
                sb.AppendLine("        self." & n & " = tk.Canvas(" & parentRef & ", width=" & w & ", height=" & h & ", bg='white')")
            Case "Image"
                Dim imgPath As String = If(widgetImage.ContainsKey(n), widgetImage(n), "")
                Dim imgFile As String = IO.Path.GetFileName(imgPath).Replace("\", "/")
                sb.AppendLine("        _img_" & n & " = ImageTk.PhotoImage(Image.open('" & imgFile & "').resize((" & w & ", " & h & ")))")
                sb.AppendLine("        self._images.append(_img_" & n & ")")
                sb.AppendLine("        self." & n & " = tk.Label(" & parentRef & ", image=_img_" & n & ", relief='" & relief & "')")
        End Select

        If wType = "" Then Return

        ' توليد Layout
        Dim lm As String = If(widgetLayout.ContainsKey(n), widgetLayout(n), "place")
        Select Case lm
            Case "place"
                sb.AppendLine("        self." & n & ".place(x=" & x & ", y=" & y & ", width=" & w & ", height=" & h & ")")
            Case "pack"
                Dim side As String = If(widgetPackSide.ContainsKey(n), widgetPackSide(n), "top")
                Dim fill As String = If(widgetPackFill.ContainsKey(n), widgetPackFill(n), "none")
                Dim expand As Boolean = If(widgetPackExpand.ContainsKey(n), widgetPackExpand(n), False)
                Dim expandStr As String = If(expand, "True", "False")
                sb.AppendLine("        self." & n & ".pack(side='" & side & "', fill='" & fill & "', expand=" & expandStr & ", padx=2, pady=2)")
            Case "grid"
                Dim row As Integer = If(widgetGridRow.ContainsKey(n), widgetGridRow(n), 0)
                Dim col As Integer = If(widgetGridCol.ContainsKey(n), widgetGridCol(n), 0)
                Dim sticky As String = If(widgetGridSticky.ContainsKey(n), widgetGridSticky(n), "")
                Dim stickyStr As String = If(sticky <> "", ", sticky='" & sticky & "'", "")
                sb.AppendLine("        self." & n & ".grid(row=" & row & ", column=" & col & ", padx=4, pady=4" & stickyStr & ")")
        End Select

        sb.AppendLine("")
    End Sub

    Private Function ColorToHex(c As Color) As String
        Return String.Format("#{0:X2}{1:X2}{2:X2}", c.R, c.G, c.B)
    End Function

    ' =============================================
    ' --- تصدير / حفظ / فتح ---
    ' =============================================

    Private Sub BtnExport_Click(sender As Object, e As EventArgs) Handles BtnExport.Click
        Using sfd As New SaveFileDialog()
            sfd.Filter = "ملفات بايثون (*.py)|*.py|كل الملفات (*.*)|*.*"
            sfd.FileName = "my_gui.py"
            If sfd.ShowDialog() = DialogResult.OK Then
                Try
                    File.WriteAllText(sfd.FileName, GeneratePythonCode(), Encoding.UTF8)
                    MessageBox.Show("تم الحفظ: " & sfd.FileName, "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Catch ex As Exception
                    MessageBox.Show("خطأ: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        End Using
    End Sub

    Private Sub BtnSaveProject_Click(sender As Object, e As EventArgs) Handles BtnSaveProject.Click
        Using sfd As New SaveFileDialog()
            sfd.Filter = "مشروع PyForm (*.pfb)|*.pfb|كل الملفات (*.*)|*.*"
            sfd.FileName = If(currentProjectPath <> "", IO.Path.GetFileName(currentProjectPath), "project.pfb")
            If currentProjectPath <> "" Then sfd.InitialDirectory = IO.Path.GetDirectoryName(currentProjectPath)
            If sfd.ShowDialog() = DialogResult.OK Then SaveProject(sfd.FileName)
        End Using
    End Sub

    Private Sub SaveProject(path As String)
        Try
            Dim fw As Integer = 600 : Dim fh As Integer = 500
            Integer.TryParse(TxtFormWidth.Text, fw)
            Integer.TryParse(TxtFormHeight.Text, fh)
            Dim sb As New StringBuilder()
            sb.AppendLine("{")
            sb.AppendLine("  ""FormTitle"": """ & EscapeJson(TxtFormTitle.Text) & """,")
            sb.AppendLine("  ""FormWidth"": " & fw & ",")
            sb.AppendLine("  ""FormHeight"": " & fh & ",")
            sb.AppendLine("  ""FormBgColor"": """ & ColorToHex(formBgColor) & """,")
            sb.AppendLine("  ""Widgets"": [")
            Dim ctrls As New List(Of Control)()
            For Each c As Control In DesignPanel.Controls
                ctrls.Add(c)
            Next
            For i As Integer = 0 To ctrls.Count - 1
                Dim ctrl As Control = ctrls(i)
                Dim n As String = ctrl.Name
                Dim comma As String = If(i < ctrls.Count - 1, ",", "")
                sb.AppendLine("    {")
                sb.AppendLine("      ""WidgetType"": """ & EscapeJson(If(ctrl.Tag IsNot Nothing, ctrl.Tag.ToString(), "")) & """,")
                sb.AppendLine("      ""Name"": """ & EscapeJson(n) & """,")
                sb.AppendLine("      ""Text"": """ & EscapeJson(ctrl.Text) & """,")
                sb.AppendLine("      ""X"": " & ctrl.Left & ",")
                sb.AppendLine("      ""Y"": " & ctrl.Top & ",")
                sb.AppendLine("      ""W"": " & ctrl.Width & ",")
                sb.AppendLine("      ""H"": " & ctrl.Height & ",")
                sb.AppendLine("      ""FgColor"": """ & ColorToHex(ctrl.ForeColor) & """,")
                sb.AppendLine("      ""BgColor"": """ & ColorToHex(ctrl.BackColor) & """,")
                sb.AppendLine("      ""FontName"": """ & EscapeJson(ctrl.Font.Name) & """,")
                sb.AppendLine("      ""FontSize"": " & CInt(ctrl.Font.Size) & ",")
                sb.AppendLine("      ""FontBold"": " & If(ctrl.Font.Bold, "true", "false") & ",")
                sb.AppendLine("      ""Relief"": """ & EscapeJson(If(widgetRelief.ContainsKey(n), widgetRelief(n), "flat")) & """,")
                sb.AppendLine("      ""CommandName"": """ & EscapeJson(If(widgetCommand.ContainsKey(n), widgetCommand(n), "")) & """,")
                sb.AppendLine("      ""LayoutMode"": """ & EscapeJson(If(widgetLayout.ContainsKey(n), widgetLayout(n), "place")) & """,")
                sb.AppendLine("      ""PackSide"": """ & EscapeJson(If(widgetPackSide.ContainsKey(n), widgetPackSide(n), "top")) & """,")
                sb.AppendLine("      ""PackFill"": """ & EscapeJson(If(widgetPackFill.ContainsKey(n), widgetPackFill(n), "none")) & """,")
                sb.AppendLine("      ""PackExpand"": " & If(widgetPackExpand.ContainsKey(n) AndAlso widgetPackExpand(n), "true", "false") & ",")
                sb.AppendLine("      ""GridRow"": " & If(widgetGridRow.ContainsKey(n), widgetGridRow(n), 0) & ",")
                sb.AppendLine("      ""GridCol"": " & If(widgetGridCol.ContainsKey(n), widgetGridCol(n), 0) & ",")
                sb.AppendLine("      ""GridSticky"": """ & EscapeJson(If(widgetGridSticky.ContainsKey(n), widgetGridSticky(n), "")) & """,")
                sb.AppendLine("      ""ParentFrame"": """ & EscapeJson(If(widgetParent.ContainsKey(n), widgetParent(n), "root")) & """,")
                sb.AppendLine("      ""ImagePath"": """ & EscapeJson(If(widgetImage.ContainsKey(n), widgetImage(n), "")) & """")
                sb.AppendLine("    }" & comma)
            Next
            sb.AppendLine("  ]") : sb.AppendLine("}")
            File.WriteAllText(path, sb.ToString(), Encoding.UTF8)
            currentProjectPath = path
            Me.Text = "PyForm Builder — " & IO.Path.GetFileName(path)
            UpdateStatusBar("تم حفظ المشروع: " & IO.Path.GetFileName(path))
            MessageBox.Show("تم حفظ المشروع بنجاح!", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show("خطأ: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub BtnOpenProject_Click(sender As Object, e As EventArgs) Handles BtnOpenProject.Click
        Using ofd As New OpenFileDialog()
            ofd.Filter = "مشروع PyForm (*.pfb)|*.pfb|كل الملفات (*.*)|*.*"
            If ofd.ShowDialog() = DialogResult.OK Then LoadProject(ofd.FileName)
        End Using
    End Sub

    Private Sub LoadProject(path As String)
        Try
            Dim json As String = File.ReadAllText(path, Encoding.UTF8)
            DesignPanel.Controls.Clear()
            controlCounters.Clear()
            widgetFgColor.Clear() : widgetBgColor.Clear() : widgetRelief.Clear()
            widgetCommand.Clear() : widgetLayout.Clear() : widgetPackSide.Clear()
            widgetPackFill.Clear() : widgetPackExpand.Clear() : widgetGridRow.Clear()
            widgetGridCol.Clear() : widgetGridSticky.Clear() : widgetParent.Clear()
            widgetImage.Clear()

            TxtFormTitle.Text = ParseJsonString(json, "FormTitle")
            TxtFormWidth.Text = ParseJsonInt(json, "FormWidth", 600).ToString()
            TxtFormHeight.Text = ParseJsonInt(json, "FormHeight", 500).ToString()

            Dim savedBg As String = ParseJsonString(json, "FormBgColor")
            If savedBg <> "" Then
                Try
                    formBgColor = ColorTranslator.FromHtml(savedBg)
                    PnlFormBg.BackColor = formBgColor
                    DesignPanel.BackColor = formBgColor
                Catch : End Try
            End If
            UpdateDesignPanelSize()

            ' تحميل Frames أولاً لأنها آباء
            For Each block As String In SplitJsonObjects(ExtractJsonArray(json, "Widgets"))
                If ParseJsonString(block, "WidgetType") = "Frame" Then
                    LoadWidget(block)
                End If
            Next
            ' ثم بقية العناصر
            For Each block As String In SplitJsonObjects(ExtractJsonArray(json, "Widgets"))
                If ParseJsonString(block, "WidgetType") <> "Frame" Then
                    LoadWidget(block)
                End If
            Next

            currentProjectPath = path
            Me.Text = "PyForm Builder — " & IO.Path.GetFileName(path)
            SelectSingle(Nothing)
            UpdateParentFrameCombo()
            UpdateCodePreview()
            UpdateStatusBar("تم فتح المشروع: " & IO.Path.GetFileName(path))
        Catch ex As Exception
            MessageBox.Show("خطأ: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LoadWidget(block As String)
        Dim wd As New WidgetData()
        wd.WidgetType = ParseJsonString(block, "WidgetType")
        wd.Name = ParseJsonString(block, "Name")
        wd.Text = ParseJsonString(block, "Text")
        wd.X = ParseJsonInt(block, "X", 0) : wd.Y = ParseJsonInt(block, "Y", 0)
        wd.W = ParseJsonInt(block, "W", 100) : wd.H = ParseJsonInt(block, "H", 28)
        wd.FgColor = ParseJsonString(block, "FgColor")
        wd.BgColor = ParseJsonString(block, "BgColor")
        wd.FontName = ParseJsonString(block, "FontName")
        wd.FontSize = ParseJsonInt(block, "FontSize", 9)
        wd.FontBold = ParseJsonBool(block, "FontBold")
        wd.Relief = ParseJsonString(block, "Relief")
        wd.CommandName = ParseJsonString(block, "CommandName")
        wd.LayoutMode = ParseJsonString(block, "LayoutMode")
        wd.PackSide = ParseJsonString(block, "PackSide")
        wd.PackFill = ParseJsonString(block, "PackFill")
        wd.PackExpand = ParseJsonBool(block, "PackExpand")
        wd.GridRow = ParseJsonInt(block, "GridRow", 0)
        wd.GridCol = ParseJsonInt(block, "GridCol", 0)
        wd.GridSticky = ParseJsonString(block, "GridSticky")
        wd.ParentFrame = ParseJsonString(block, "ParentFrame")
        wd.ImagePath = ParseJsonString(block, "ImagePath")
        If wd.WidgetType = "" Then Return
        If Not controlCounters.ContainsKey(wd.WidgetType) Then controlCounters(wd.WidgetType) = 0
        controlCounters(wd.WidgetType) -= 1
        If wd.WidgetType = "Image" Then
            AddImageWidget(New Point(wd.X, wd.Y), wd)
        Else
            AddWidgetToCanvas(wd.WidgetType, New Point(wd.X, wd.Y), wd)
        End If
    End Sub

    ' =============================================
    ' --- JSON helpers ---
    ' =============================================

    Private Function EscapeJson(s As String) As String
        If s Is Nothing Then Return ""
        Return s.Replace("\", "\\").Replace("""", "\""").Replace(Chr(13), "").Replace(Chr(10), "\n")
    End Function

    Private Function ParseJsonString(json As String, key As String) As String
        Dim pattern As String = """" & key & """: """
        Dim si As Integer = json.IndexOf(pattern)
        If si < 0 Then Return ""
        si += pattern.Length
        Dim ei As Integer = si
        While ei < json.Length
            If json(ei) = """"c AndAlso (ei = 0 OrElse json(ei - 1) <> "\"c) Then Exit While
            ei += 1
        End While
        Return json.Substring(si, ei - si).Replace("\""", """").Replace("\n", Environment.NewLine)
    End Function

    Private Function ParseJsonInt(json As String, key As String, def As Integer) As Integer
        Dim pattern As String = """" & key & """: "
        Dim si As Integer = json.IndexOf(pattern)
        If si < 0 Then Return def
        si += pattern.Length
        Dim ei As Integer = si
        While ei < json.Length AndAlso (Char.IsDigit(json(ei)) OrElse json(ei) = "-"c) : ei += 1 : End While
        Dim r As Integer
        If Integer.TryParse(json.Substring(si, ei - si), r) Then Return r
        Return def
    End Function

    Private Function ParseJsonBool(json As String, key As String) As Boolean
        Dim pattern As String = """" & key & """: "
        Dim si As Integer = json.IndexOf(pattern)
        If si < 0 Then Return False
        si += pattern.Length
        If si + 4 <= json.Length Then Return json.Substring(si, 4) = "true"
        Return False
    End Function

    Private Function ExtractJsonArray(json As String, key As String) As String
        Dim pattern As String = """" & key & """: ["
        Dim si As Integer = json.IndexOf(pattern)
        If si < 0 Then Return ""
        si += pattern.Length - 1
        Dim depth As Integer = 0 : Dim ei As Integer = si
        While ei < json.Length
            If json(ei) = "["c Then depth += 1
            If json(ei) = "]"c Then
                depth -= 1
                If depth = 0 Then Return json.Substring(si + 1, ei - si - 1)
            End If
            ei += 1
        End While
        Return ""
    End Function

    Private Function SplitJsonObjects(json As String) As List(Of String)
        Dim result As New List(Of String)()
        Dim depth As Integer = 0 : Dim start As Integer = -1
        For i As Integer = 0 To json.Length - 1
            If json(i) = "{"c Then
                If depth = 0 Then start = i
                depth += 1
            ElseIf json(i) = "}"c Then
                depth -= 1
                If depth = 0 AndAlso start >= 0 Then
                    result.Add(json.Substring(start, i - start + 1))
                    start = -1
                End If
            End If
        Next
        Return result
    End Function

    ' =============================================
    ' --- مسح اللوحة ---
    ' =============================================

    Private Sub BtnClear_Click(sender As Object, e As EventArgs) Handles BtnClear.Click
        If MessageBox.Show("هل تريد مسح جميع العناصر؟", "تأكيد",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
            DesignPanel.Controls.Clear()
            controlCounters.Clear()
            widgetFgColor.Clear() : widgetBgColor.Clear() : widgetRelief.Clear()
            widgetCommand.Clear() : widgetLayout.Clear() : widgetPackSide.Clear()
            widgetPackFill.Clear() : widgetPackExpand.Clear() : widgetGridRow.Clear()
            widgetGridCol.Clear() : widgetGridSticky.Clear() : widgetParent.Clear()
            widgetImage.Clear()
            selectedControls.Clear()
            SelectSingle(Nothing)
            UpdateParentFrameCombo()
            UpdateCodePreview()
            UpdateStatusBar("تم مسح اللوحة")
        End If
    End Sub

    ' =============================================
    ' --- أدوات مساعدة ---
    ' =============================================

    Private Function SnapToGrid(pt As Point) As Point
        Return New Point(CInt(Math.Round(pt.X / GRID_SIZE) * GRID_SIZE),
                         CInt(Math.Round(pt.Y / GRID_SIZE) * GRID_SIZE))
    End Function

    Private Sub UpdateStatusBar(msg As String)
        StatusLabel.Text = msg
        LblControlCount.Text = "العناصر: " & DesignPanel.Controls.Count.ToString()
        LblPanelSize.Text = "البانل: " & DesignPanel.Width & " × " & DesignPanel.Height
    End Sub

    Private Sub UpdateDesignPanelSize()
        Dim w As Integer = 600 : Dim h As Integer = 500
        Integer.TryParse(TxtFormWidth.Text.Trim(), w)
        Integer.TryParse(TxtFormHeight.Text.Trim(), h)
        w = Math.Max(200, Math.Min(w, 2000))
        h = Math.Max(150, Math.Min(h, 2000))
        DesignPanel.Size = New Size(w, h)
        UpdateStatusBar("البانل: " & w & " × " & h)
        UpdateCodePreview()
    End Sub

    ' =============================================
    ' --- تحميل النموذج ---
    ' =============================================

    Private Sub MainFormLoad(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "PyForm Builder — مصمم واجهات بايثون"
        TxtFormTitle.Text = "واجهتي"
        TxtFormWidth.Text = "600"
        TxtFormHeight.Text = "500"
        PnlFormBg.BackColor = formBgColor
        AddHandler PnlFormBg.Click, AddressOf PnlFormBg_Click
        AddHandler TxtFormWidth.TextChanged, AddressOf FormSize_TextChanged
        AddHandler TxtFormHeight.TextChanged, AddressOf FormSize_TextChanged
        AddHandler TxtFormTitle.TextChanged, AddressOf FormTitle_TextChanged
        UpdateAlignButtons()
        UpdateCodePreview()
    End Sub

    Private Sub MainFormShown(sender As Object, e As EventArgs) Handles MyBase.Shown
        Try
            MainSplitter.SplitterDistance = 165
            RightSplitter.SplitterDistance = CInt(RightSplitter.Width * 0.74)
        Catch : End Try
    End Sub

    Private Sub FormSize_TextChanged(sender As Object, e As EventArgs)
        UpdateDesignPanelSize()
    End Sub

    Private Sub FormTitle_TextChanged(sender As Object, e As EventArgs)
        UpdateCodePreview()
    End Sub

End Class