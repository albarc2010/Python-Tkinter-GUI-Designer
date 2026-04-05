' PyForm Builder - مصمم واجهات بايثون
' Created by SharpDevelop 4.4
' Author: Ahmed — هذا الملف يُدار تلقائياً

Partial Class MainForm
    Inherits System.Windows.Forms.Form

    ' =============================================
    ' --- تعريفات العناصر ---
    ' =============================================

    Private WithEvents MainMenu As System.Windows.Forms.MenuStrip
    Private WithEvents MenuFile As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents MenuNew As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents MenuOpen As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents MenuSave As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents MenuExportPy As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents MenuSep1 As System.Windows.Forms.ToolStripSeparator
    Private WithEvents MenuExit As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents MenuEdit As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents MenuDelete As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents MenuSelectAll As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents MenuClearAll As System.Windows.Forms.ToolStripMenuItem

    Private WithEvents MainToolStrip As System.Windows.Forms.ToolStrip
    Private WithEvents TsBtnNew As System.Windows.Forms.ToolStripButton
    Private WithEvents TsBtnOpen As System.Windows.Forms.ToolStripButton
    Private WithEvents TsBtnSave As System.Windows.Forms.ToolStripButton
    Private WithEvents TsSep1 As System.Windows.Forms.ToolStripSeparator
    Private WithEvents TsBtnDelete As System.Windows.Forms.ToolStripButton
    Private WithEvents TsBtnClear As System.Windows.Forms.ToolStripButton
    Private WithEvents TsSep2 As System.Windows.Forms.ToolStripSeparator
    Private WithEvents TsBtnExport As System.Windows.Forms.ToolStripButton

    Private WithEvents AlignToolStrip As System.Windows.Forms.ToolStrip
    Private WithEvents TsLblAlign As System.Windows.Forms.ToolStripLabel
    Private WithEvents TsSepAlign As System.Windows.Forms.ToolStripSeparator
    Private WithEvents BtnAlignLeft As System.Windows.Forms.ToolStripButton
    Private WithEvents BtnAlignRight As System.Windows.Forms.ToolStripButton
    Private WithEvents BtnAlignTop As System.Windows.Forms.ToolStripButton
    Private WithEvents BtnAlignBottom As System.Windows.Forms.ToolStripButton
    Private WithEvents TsSepAlign2 As System.Windows.Forms.ToolStripSeparator
    Private WithEvents BtnAlignCenterH As System.Windows.Forms.ToolStripButton
    Private WithEvents BtnAlignCenterV As System.Windows.Forms.ToolStripButton
    Private WithEvents TsSepAlign3 As System.Windows.Forms.ToolStripSeparator
    Private WithEvents BtnEqualSpaceH As System.Windows.Forms.ToolStripButton
    Private WithEvents BtnEqualSpaceV As System.Windows.Forms.ToolStripButton

    Private WithEvents MainSplitter As System.Windows.Forms.SplitContainer
    Private WithEvents RightSplitter As System.Windows.Forms.SplitContainer

    ' --- الشريط الأيسر ---
    Private WithEvents PanelLeft As System.Windows.Forms.Panel
    Private WithEvents LblToolboxTitle As System.Windows.Forms.Label
    Private WithEvents WidgetListBox As System.Windows.Forms.ListBox
    Private WithEvents GrpFormSettings As System.Windows.Forms.GroupBox
    Private WithEvents LblFormTitle As System.Windows.Forms.Label
    Private WithEvents TxtFormTitle As System.Windows.Forms.TextBox
    Private WithEvents LblFormSize As System.Windows.Forms.Label
    Private WithEvents TxtFormWidth As System.Windows.Forms.TextBox
    Private WithEvents LblX As System.Windows.Forms.Label
    Private WithEvents TxtFormHeight As System.Windows.Forms.TextBox
    Private WithEvents LblFormBg As System.Windows.Forms.Label
    Private WithEvents PnlFormBg As System.Windows.Forms.Panel
    Private WithEvents LblFormBgHint As System.Windows.Forms.Label

    ' --- منطقة التصميم ---
    Private WithEvents PanelCenter As System.Windows.Forms.Panel
    Private WithEvents LblCanvasTitle As System.Windows.Forms.Label
    Private WithEvents DesignPanel As System.Windows.Forms.Panel

    ' --- الشريط الأيمن ---
    Private WithEvents PanelRight As System.Windows.Forms.Panel

    ' لوحة الخصائص الأساسية
    Private WithEvents GrpBasicProps As System.Windows.Forms.GroupBox
    Private WithEvents LblSelectedType As System.Windows.Forms.Label
    Private WithEvents LblPropName As System.Windows.Forms.Label
    Private WithEvents PropNameBox As System.Windows.Forms.TextBox
    Private WithEvents LblPropText As System.Windows.Forms.Label
    Private WithEvents PropTextBox As System.Windows.Forms.TextBox
    Private WithEvents LblPropX As System.Windows.Forms.Label
    Private WithEvents PropXBox As System.Windows.Forms.TextBox
    Private WithEvents LblPropY As System.Windows.Forms.Label
    Private WithEvents PropYBox As System.Windows.Forms.TextBox
    Private WithEvents LblPropWidth As System.Windows.Forms.Label
    Private WithEvents PropWidthBox As System.Windows.Forms.TextBox
    Private WithEvents LblPropHeight As System.Windows.Forms.Label
    Private WithEvents PropHeightBox As System.Windows.Forms.TextBox

    ' لوحة المظهر
    Private WithEvents GrpAppearance As System.Windows.Forms.GroupBox
    Private WithEvents LblFgColor As System.Windows.Forms.Label
    Private WithEvents PnlFgColor As System.Windows.Forms.Panel
    Private WithEvents LblFgHint As System.Windows.Forms.Label
    Private WithEvents LblBgColor As System.Windows.Forms.Label
    Private WithEvents PnlBgColor As System.Windows.Forms.Panel
    Private WithEvents LblBgHint As System.Windows.Forms.Label
    Private WithEvents LblFont As System.Windows.Forms.Label
    Private WithEvents CmbFontName As System.Windows.Forms.ComboBox
    Private WithEvents CmbFontSize As System.Windows.Forms.ComboBox
    Private WithEvents ChkFontBold As System.Windows.Forms.CheckBox
    Private WithEvents LblRelief As System.Windows.Forms.Label
    Private WithEvents CmbRelief As System.Windows.Forms.ComboBox
    Private WithEvents LblCommand As System.Windows.Forms.Label
    Private WithEvents PropCommandBox As System.Windows.Forms.TextBox
    Private WithEvents BtnChangeImage As System.Windows.Forms.Button

    ' لوحة Layout
    Private WithEvents GrpLayout As System.Windows.Forms.GroupBox
    Private WithEvents LblLayoutMode As System.Windows.Forms.Label
    Private WithEvents CmbLayoutMode As System.Windows.Forms.ComboBox
    Private WithEvents LblParentFrame As System.Windows.Forms.Label
    Private WithEvents CmbParentFrame As System.Windows.Forms.ComboBox

    ' خيارات place
    Private WithEvents PnlPlaceOptions As System.Windows.Forms.Panel
    Private WithEvents LblPlaceNote As System.Windows.Forms.Label

    ' خيارات pack
    Private WithEvents PnlPackOptions As System.Windows.Forms.Panel
    Private WithEvents LblPackSide As System.Windows.Forms.Label
    Private WithEvents CmbPackSide As System.Windows.Forms.ComboBox
    Private WithEvents LblPackFill As System.Windows.Forms.Label
    Private WithEvents CmbPackFill As System.Windows.Forms.ComboBox
    Private WithEvents ChkPackExpand As System.Windows.Forms.CheckBox

    ' خيارات grid
    Private WithEvents PnlGridOptions As System.Windows.Forms.Panel
    Private WithEvents LblGridRow As System.Windows.Forms.Label
    Private WithEvents TxtGridRow As System.Windows.Forms.TextBox
    Private WithEvents LblGridCol As System.Windows.Forms.Label
    Private WithEvents TxtGridCol As System.Windows.Forms.TextBox
    Private WithEvents LblGridSticky As System.Windows.Forms.Label
    Private WithEvents CmbGridSticky As System.Windows.Forms.ComboBox

    ' أزرار
    Private WithEvents BtnApplyProps As System.Windows.Forms.Button
    Private WithEvents BtnDelete As System.Windows.Forms.Button

    ' معاينة الكود
    Private WithEvents GrpCodePreview As System.Windows.Forms.GroupBox
    Private WithEvents CodePreviewBox As System.Windows.Forms.RichTextBox
    Private WithEvents BtnExport As System.Windows.Forms.Button
    Private WithEvents BtnSaveProject As System.Windows.Forms.Button
    Private WithEvents BtnOpenProject As System.Windows.Forms.Button
    Private WithEvents BtnClear As System.Windows.Forms.Button

    ' شريط الحالة
    Private WithEvents MainStatusStrip As System.Windows.Forms.StatusStrip
    Private WithEvents StatusLabel As System.Windows.Forms.ToolStripStatusLabel
    Private WithEvents LblPanelSize As System.Windows.Forms.ToolStripStatusLabel
    Private WithEvents LblControlCount As System.Windows.Forms.ToolStripStatusLabel

    ' =============================================
    ' --- InitializeComponent ---
    ' =============================================

    Private Sub InitializeComponent()
        Me.SuspendLayout()

        Me.MainMenu = New System.Windows.Forms.MenuStrip()
        Me.MenuFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuNew = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuOpen = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuSave = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuExportPy = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuSep1 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuExit = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuEdit = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuDelete = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuSelectAll = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuClearAll = New System.Windows.Forms.ToolStripMenuItem()
        Me.MainToolStrip = New System.Windows.Forms.ToolStrip()
        Me.TsBtnNew = New System.Windows.Forms.ToolStripButton()
        Me.TsBtnOpen = New System.Windows.Forms.ToolStripButton()
        Me.TsBtnSave = New System.Windows.Forms.ToolStripButton()
        Me.TsSep1 = New System.Windows.Forms.ToolStripSeparator()
        Me.TsBtnDelete = New System.Windows.Forms.ToolStripButton()
        Me.TsBtnClear = New System.Windows.Forms.ToolStripButton()
        Me.TsSep2 = New System.Windows.Forms.ToolStripSeparator()
        Me.TsBtnExport = New System.Windows.Forms.ToolStripButton()
        Me.AlignToolStrip = New System.Windows.Forms.ToolStrip()
        Me.TsLblAlign = New System.Windows.Forms.ToolStripLabel()
        Me.TsSepAlign = New System.Windows.Forms.ToolStripSeparator()
        Me.BtnAlignLeft = New System.Windows.Forms.ToolStripButton()
        Me.BtnAlignRight = New System.Windows.Forms.ToolStripButton()
        Me.BtnAlignTop = New System.Windows.Forms.ToolStripButton()
        Me.BtnAlignBottom = New System.Windows.Forms.ToolStripButton()
        Me.TsSepAlign2 = New System.Windows.Forms.ToolStripSeparator()
        Me.BtnAlignCenterH = New System.Windows.Forms.ToolStripButton()
        Me.BtnAlignCenterV = New System.Windows.Forms.ToolStripButton()
        Me.TsSepAlign3 = New System.Windows.Forms.ToolStripSeparator()
        Me.BtnEqualSpaceH = New System.Windows.Forms.ToolStripButton()
        Me.BtnEqualSpaceV = New System.Windows.Forms.ToolStripButton()
        Me.MainSplitter = New System.Windows.Forms.SplitContainer()
        Me.RightSplitter = New System.Windows.Forms.SplitContainer()
        Me.PanelLeft = New System.Windows.Forms.Panel()
        Me.LblToolboxTitle = New System.Windows.Forms.Label()
        Me.WidgetListBox = New System.Windows.Forms.ListBox()
        Me.GrpFormSettings = New System.Windows.Forms.GroupBox()
        Me.LblFormTitle = New System.Windows.Forms.Label()
        Me.TxtFormTitle = New System.Windows.Forms.TextBox()
        Me.LblFormSize = New System.Windows.Forms.Label()
        Me.TxtFormWidth = New System.Windows.Forms.TextBox()
        Me.LblX = New System.Windows.Forms.Label()
        Me.TxtFormHeight = New System.Windows.Forms.TextBox()
        Me.LblFormBg = New System.Windows.Forms.Label()
        Me.PnlFormBg = New System.Windows.Forms.Panel()
        Me.LblFormBgHint = New System.Windows.Forms.Label()
        Me.PanelCenter = New System.Windows.Forms.Panel()
        Me.LblCanvasTitle = New System.Windows.Forms.Label()
        Me.DesignPanel = New System.Windows.Forms.Panel()
        Me.PanelRight = New System.Windows.Forms.Panel()
        Me.GrpBasicProps = New System.Windows.Forms.GroupBox()
        Me.LblSelectedType = New System.Windows.Forms.Label()
        Me.LblPropName = New System.Windows.Forms.Label()
        Me.PropNameBox = New System.Windows.Forms.TextBox()
        Me.LblPropText = New System.Windows.Forms.Label()
        Me.PropTextBox = New System.Windows.Forms.TextBox()
        Me.LblPropX = New System.Windows.Forms.Label()
        Me.PropXBox = New System.Windows.Forms.TextBox()
        Me.LblPropY = New System.Windows.Forms.Label()
        Me.PropYBox = New System.Windows.Forms.TextBox()
        Me.LblPropWidth = New System.Windows.Forms.Label()
        Me.PropWidthBox = New System.Windows.Forms.TextBox()
        Me.LblPropHeight = New System.Windows.Forms.Label()
        Me.PropHeightBox = New System.Windows.Forms.TextBox()
        Me.GrpAppearance = New System.Windows.Forms.GroupBox()
        Me.LblFgColor = New System.Windows.Forms.Label()
        Me.PnlFgColor = New System.Windows.Forms.Panel()
        Me.LblFgHint = New System.Windows.Forms.Label()
        Me.LblBgColor = New System.Windows.Forms.Label()
        Me.PnlBgColor = New System.Windows.Forms.Panel()
        Me.LblBgHint = New System.Windows.Forms.Label()
        Me.LblFont = New System.Windows.Forms.Label()
        Me.CmbFontName = New System.Windows.Forms.ComboBox()
        Me.CmbFontSize = New System.Windows.Forms.ComboBox()
        Me.ChkFontBold = New System.Windows.Forms.CheckBox()
        Me.LblRelief = New System.Windows.Forms.Label()
        Me.CmbRelief = New System.Windows.Forms.ComboBox()
        Me.LblCommand = New System.Windows.Forms.Label()
        Me.PropCommandBox = New System.Windows.Forms.TextBox()
        Me.BtnChangeImage = New System.Windows.Forms.Button()
        Me.GrpLayout = New System.Windows.Forms.GroupBox()
        Me.LblLayoutMode = New System.Windows.Forms.Label()
        Me.CmbLayoutMode = New System.Windows.Forms.ComboBox()
        Me.LblParentFrame = New System.Windows.Forms.Label()
        Me.CmbParentFrame = New System.Windows.Forms.ComboBox()
        Me.PnlPlaceOptions = New System.Windows.Forms.Panel()
        Me.LblPlaceNote = New System.Windows.Forms.Label()
        Me.PnlPackOptions = New System.Windows.Forms.Panel()
        Me.LblPackSide = New System.Windows.Forms.Label()
        Me.CmbPackSide = New System.Windows.Forms.ComboBox()
        Me.LblPackFill = New System.Windows.Forms.Label()
        Me.CmbPackFill = New System.Windows.Forms.ComboBox()
        Me.ChkPackExpand = New System.Windows.Forms.CheckBox()
        Me.PnlGridOptions = New System.Windows.Forms.Panel()
        Me.LblGridRow = New System.Windows.Forms.Label()
        Me.TxtGridRow = New System.Windows.Forms.TextBox()
        Me.LblGridCol = New System.Windows.Forms.Label()
        Me.TxtGridCol = New System.Windows.Forms.TextBox()
        Me.LblGridSticky = New System.Windows.Forms.Label()
        Me.CmbGridSticky = New System.Windows.Forms.ComboBox()
        Me.BtnApplyProps = New System.Windows.Forms.Button()
        Me.BtnDelete = New System.Windows.Forms.Button()
        Me.GrpCodePreview = New System.Windows.Forms.GroupBox()
        Me.CodePreviewBox = New System.Windows.Forms.RichTextBox()
        Me.BtnExport = New System.Windows.Forms.Button()
        Me.BtnSaveProject = New System.Windows.Forms.Button()
        Me.BtnOpenProject = New System.Windows.Forms.Button()
        Me.BtnClear = New System.Windows.Forms.Button()
        Me.MainStatusStrip = New System.Windows.Forms.StatusStrip()
        Me.StatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.LblPanelSize = New System.Windows.Forms.ToolStripStatusLabel()
        Me.LblControlCount = New System.Windows.Forms.ToolStripStatusLabel()

        ' ======================
        ' MainMenu
        ' ======================
        Me.MainMenu.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.MainMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuFile, Me.MenuEdit})
        Me.MenuFile.Text = "ملف"
        Me.MenuFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {
            Me.MenuNew, Me.MenuOpen, Me.MenuSave, Me.MenuExportPy, Me.MenuSep1, Me.MenuExit})
        Me.MenuNew.Text = "مشروع جديد" : Me.MenuNew.ShortcutKeys = Keys.Control Or Keys.N
        Me.MenuOpen.Text = "فتح مشروع..." : Me.MenuOpen.ShortcutKeys = Keys.Control Or Keys.O
        Me.MenuSave.Text = "حفظ المشروع..." : Me.MenuSave.ShortcutKeys = Keys.Control Or Keys.S
        Me.MenuExportPy.Text = "تصدير .py..." : Me.MenuExportPy.ShortcutKeys = Keys.Control Or Keys.E
        Me.MenuExit.Text = "خروج"
        Me.MenuEdit.Text = "تحرير"
        Me.MenuEdit.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {
            Me.MenuDelete, Me.MenuSelectAll, Me.MenuClearAll})
        Me.MenuDelete.Text = "حذف المحدد" : Me.MenuDelete.ShortcutKeys = Keys.Delete
        Me.MenuSelectAll.Text = "تحديد الكل" : Me.MenuSelectAll.ShortcutKeys = Keys.Control Or Keys.A
        Me.MenuClearAll.Text = "مسح الكل"
        AddHandler Me.MenuNew.Click, Sub(s, ev) BtnClear_Click(s, ev)
        AddHandler Me.MenuOpen.Click, Sub(s, ev) BtnOpenProject_Click(s, ev)
        AddHandler Me.MenuSave.Click, Sub(s, ev) BtnSaveProject_Click(s, ev)
        AddHandler Me.MenuExportPy.Click, Sub(s, ev) BtnExport_Click(s, ev)
        AddHandler Me.MenuExit.Click, Sub(s, ev) Me.Close()
        AddHandler Me.MenuDelete.Click, Sub(s, ev) BtnDelete_Click(s, ev)
        AddHandler Me.MenuClearAll.Click, Sub(s, ev) BtnClear_Click(s, ev)

        ' ======================
        ' MainToolStrip
        ' ======================
        Me.MainToolStrip.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.MainToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {
            Me.TsBtnNew, Me.TsBtnOpen, Me.TsBtnSave, Me.TsSep1,
            Me.TsBtnDelete, Me.TsBtnClear, Me.TsSep2, Me.TsBtnExport})
        Me.TsBtnNew.Text = "جديد" : Me.TsBtnOpen.Text = "فتح"
        Me.TsBtnSave.Text = "حفظ" : Me.TsBtnDelete.Text = "حذف"
        Me.TsBtnClear.Text = "مسح الكل" : Me.TsBtnExport.Text = "⬇ تصدير .py"
        AddHandler Me.TsBtnNew.Click, Sub(s, ev) BtnClear_Click(s, ev)
        AddHandler Me.TsBtnOpen.Click, Sub(s, ev) BtnOpenProject_Click(s, ev)
        AddHandler Me.TsBtnSave.Click, Sub(s, ev) BtnSaveProject_Click(s, ev)
        AddHandler Me.TsBtnDelete.Click, Sub(s, ev) BtnDelete_Click(s, ev)
        AddHandler Me.TsBtnClear.Click, Sub(s, ev) BtnClear_Click(s, ev)
        AddHandler Me.TsBtnExport.Click, Sub(s, ev) BtnExport_Click(s, ev)

        ' ======================
        ' AlignToolStrip
        ' ======================
        Me.AlignToolStrip.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.AlignToolStrip.BackColor = System.Drawing.Color.FromArgb(240, 244, 250)
        Me.AlignToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {
            Me.TsLblAlign, Me.TsSepAlign,
            Me.BtnAlignLeft, Me.BtnAlignRight, Me.BtnAlignTop, Me.BtnAlignBottom,
            Me.TsSepAlign2, Me.BtnAlignCenterH, Me.BtnAlignCenterV,
            Me.TsSepAlign3, Me.BtnEqualSpaceH, Me.BtnEqualSpaceV})
        Me.TsLblAlign.Text = "محاذاة:"
        Me.TsLblAlign.Font = New System.Drawing.Font("Tahoma", 8.0F, System.Drawing.FontStyle.Bold)
        Me.TsLblAlign.ForeColor = System.Drawing.Color.FromArgb(24, 95, 165)
        Me.BtnAlignLeft.Text = "◧ يسار" : Me.BtnAlignLeft.Enabled = False
        Me.BtnAlignRight.Text = "◨ يمين" : Me.BtnAlignRight.Enabled = False
        Me.BtnAlignTop.Text = "⬆ أعلى" : Me.BtnAlignTop.Enabled = False
        Me.BtnAlignBottom.Text = "⬇ أسفل" : Me.BtnAlignBottom.Enabled = False
        Me.BtnAlignCenterH.Text = "↔ أفقي" : Me.BtnAlignCenterH.Enabled = False
        Me.BtnAlignCenterV.Text = "↕ عمودي" : Me.BtnAlignCenterV.Enabled = False
        Me.BtnEqualSpaceH.Text = "⇔ توزيع ↔" : Me.BtnEqualSpaceH.Enabled = False
        Me.BtnEqualSpaceV.Text = "⇕ توزيع ↕" : Me.BtnEqualSpaceV.Enabled = False

        ' ======================
        ' PanelLeft
        ' ======================
        Me.PanelLeft.Dock = DockStyle.Fill
        Me.PanelLeft.BackColor = System.Drawing.Color.FromArgb(245, 244, 240)
        Me.PanelLeft.Controls.Add(Me.GrpFormSettings)
        Me.PanelLeft.Controls.Add(Me.WidgetListBox)
        Me.PanelLeft.Controls.Add(Me.LblToolboxTitle)

        Me.LblToolboxTitle.Text = "  عناصر Tkinter"
        Me.LblToolboxTitle.Font = New System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Bold)
        Me.LblToolboxTitle.ForeColor = System.Drawing.Color.FromArgb(24, 95, 165)
        Me.LblToolboxTitle.BackColor = System.Drawing.Color.FromArgb(220, 232, 248)
        Me.LblToolboxTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.LblToolboxTitle.Dock = DockStyle.Top
        Me.LblToolboxTitle.Height = 28
        Me.LblToolboxTitle.BorderStyle = BorderStyle.FixedSingle

        Me.WidgetListBox.Dock = DockStyle.Top
        Me.WidgetListBox.Height = 238
        Me.WidgetListBox.Font = New System.Drawing.Font("Tahoma", 9.0F)
        Me.WidgetListBox.RightToLeft = RightToLeft.Yes
        Me.WidgetListBox.BorderStyle = BorderStyle.None
        Me.WidgetListBox.BackColor = System.Drawing.Color.FromArgb(250, 250, 248)
        Me.WidgetListBox.ItemHeight = 22
        Me.WidgetListBox.SelectionMode = SelectionMode.One

        Me.GrpFormSettings.Text = "إعدادات النافذة"
        Me.GrpFormSettings.Font = New System.Drawing.Font("Tahoma", 8.0F)
        Me.GrpFormSettings.RightToLeft = RightToLeft.Yes
        Me.GrpFormSettings.Dock = DockStyle.Top
        Me.GrpFormSettings.Height = 122

        Me.LblFormTitle.Text = "العنوان:"
        Me.LblFormTitle.Location = New System.Drawing.Point(6, 20)
        Me.LblFormTitle.Size = New System.Drawing.Size(48, 20)
        Me.LblFormTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.GrpFormSettings.Controls.Add(Me.LblFormTitle)

        Me.TxtFormTitle.Location = New System.Drawing.Point(56, 20)
        Me.TxtFormTitle.Size = New System.Drawing.Size(96, 21)
        Me.GrpFormSettings.Controls.Add(Me.TxtFormTitle)

        Me.LblFormSize.Text = "الحجم:"
        Me.LblFormSize.Location = New System.Drawing.Point(6, 46)
        Me.LblFormSize.Size = New System.Drawing.Size(48, 20)
        Me.LblFormSize.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.GrpFormSettings.Controls.Add(Me.LblFormSize)

        Me.TxtFormWidth.Location = New System.Drawing.Point(56, 46)
        Me.TxtFormWidth.Size = New System.Drawing.Size(38, 21)
        Me.GrpFormSettings.Controls.Add(Me.TxtFormWidth)

        Me.LblX.Text = "×"
        Me.LblX.Location = New System.Drawing.Point(96, 46)
        Me.LblX.Size = New System.Drawing.Size(12, 20)
        Me.LblX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.GrpFormSettings.Controls.Add(Me.LblX)

        Me.TxtFormHeight.Location = New System.Drawing.Point(110, 46)
        Me.TxtFormHeight.Size = New System.Drawing.Size(38, 21)
        Me.GrpFormSettings.Controls.Add(Me.TxtFormHeight)

        Me.LblFormBg.Text = "لون الواجهة:"
        Me.LblFormBg.Location = New System.Drawing.Point(6, 74)
        Me.LblFormBg.Size = New System.Drawing.Size(62, 20)
        Me.LblFormBg.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.GrpFormSettings.Controls.Add(Me.LblFormBg)

        Me.PnlFormBg.Location = New System.Drawing.Point(70, 74)
        Me.PnlFormBg.Size = New System.Drawing.Size(24, 20)
        Me.PnlFormBg.BackColor = System.Drawing.Color.FromArgb(245, 245, 240)
        Me.PnlFormBg.BorderStyle = BorderStyle.FixedSingle
        Me.PnlFormBg.Cursor = Cursors.Hand
        Me.GrpFormSettings.Controls.Add(Me.PnlFormBg)

        Me.LblFormBgHint.Text = "انقر للتغيير"
        Me.LblFormBgHint.Location = New System.Drawing.Point(96, 74)
        Me.LblFormBgHint.Size = New System.Drawing.Size(60, 20)
        Me.LblFormBgHint.ForeColor = System.Drawing.Color.Gray
        Me.LblFormBgHint.Font = New System.Drawing.Font("Tahoma", 7.5F)
        Me.LblFormBgHint.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.GrpFormSettings.Controls.Add(Me.LblFormBgHint)

        ' ======================
        ' PanelCenter
        ' ======================
        Me.PanelCenter.Dock = DockStyle.Fill
        Me.PanelCenter.BackColor = System.Drawing.Color.FromArgb(210, 208, 200)
        Me.PanelCenter.Controls.Add(Me.DesignPanel)
        Me.PanelCenter.Controls.Add(Me.LblCanvasTitle)

        Me.LblCanvasTitle.Text = "منطقة التصميم | Ctrl+Click تحديد متعدد | سحب الفأرة لتحديد منطقة | Del حذف | Ctrl+A تحديد الكل"
        Me.LblCanvasTitle.Font = New System.Drawing.Font("Tahoma", 7.0F)
        Me.LblCanvasTitle.ForeColor = System.Drawing.Color.FromArgb(70, 70, 70)
        Me.LblCanvasTitle.BackColor = System.Drawing.Color.FromArgb(200, 198, 192)
        Me.LblCanvasTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.LblCanvasTitle.Dock = DockStyle.Top
        Me.LblCanvasTitle.Height = 20
        Me.LblCanvasTitle.BorderStyle = BorderStyle.FixedSingle

        Me.DesignPanel.BackColor = System.Drawing.Color.FromArgb(245, 245, 240)
        Me.DesignPanel.BorderStyle = BorderStyle.FixedSingle
        Me.DesignPanel.AllowDrop = True
        Me.DesignPanel.Size = New System.Drawing.Size(600, 500)
        Me.DesignPanel.Location = New System.Drawing.Point(16, 30)
        Me.DesignPanel.Cursor = Cursors.Cross

        ' ======================
        ' PanelRight
        ' ======================
        Me.PanelRight.Dock = DockStyle.Fill
        Me.PanelRight.BackColor = System.Drawing.Color.FromArgb(250, 250, 248)
        Me.PanelRight.AutoScroll = True
        Me.PanelRight.Controls.Add(Me.GrpCodePreview)
        Me.PanelRight.Controls.Add(Me.BtnApplyProps)
        Me.PanelRight.Controls.Add(Me.BtnDelete)
        Me.PanelRight.Controls.Add(Me.GrpLayout)
        Me.PanelRight.Controls.Add(Me.GrpAppearance)
        Me.PanelRight.Controls.Add(Me.GrpBasicProps)

        ' ======================
        ' GrpBasicProps
        ' ======================
        Me.GrpBasicProps.Text = "الخصائص الأساسية"
        Me.GrpBasicProps.Font = New System.Drawing.Font("Tahoma", 8.0F)
        Me.GrpBasicProps.RightToLeft = RightToLeft.Yes
        Me.GrpBasicProps.Dock = DockStyle.Top
        Me.GrpBasicProps.Height = 186

        Me.LblSelectedType.Text = "لا يوجد تحديد"
        Me.LblSelectedType.Font = New System.Drawing.Font("Tahoma", 8.0F, System.Drawing.FontStyle.Italic)
        Me.LblSelectedType.ForeColor = System.Drawing.Color.FromArgb(24, 95, 165)
        Me.LblSelectedType.Location = New System.Drawing.Point(6, 14)
        Me.LblSelectedType.Size = New System.Drawing.Size(200, 16)
        Me.LblSelectedType.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.GrpBasicProps.Controls.Add(Me.LblSelectedType)

        Dim rows As Object(,) = {
            {"الاسم:", Me.LblPropName, Me.PropNameBox, 34},
            {"النص:", Me.LblPropText, Me.PropTextBox, 56},
            {"X:", Me.LblPropX, Me.PropXBox, 78},
            {"Y:", Me.LblPropY, Me.PropYBox, 100},
            {"العرض:", Me.LblPropWidth, Me.PropWidthBox, 122},
            {"الارتفاع:", Me.LblPropHeight, Me.PropHeightBox, 144}
        }
        For i As Integer = 0 To 5
            Dim lbl As Label = DirectCast(rows(i, 1), Label)
            Dim txt As TextBox = DirectCast(rows(i, 2), TextBox)
            Dim yy As Integer = CInt(rows(i, 3))
            lbl.Text = rows(i, 0).ToString()
            lbl.Location = New System.Drawing.Point(148, yy)
            lbl.Size = New System.Drawing.Size(56, 20)
            lbl.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.GrpBasicProps.Controls.Add(lbl)
            txt.Location = New System.Drawing.Point(6, yy)
            txt.Size = New System.Drawing.Size(140, 20)
            txt.Font = New System.Drawing.Font("Tahoma", 8.5F)
            Me.GrpBasicProps.Controls.Add(txt)
        Next

        ' ======================
        ' GrpAppearance
        ' ======================
        Me.GrpAppearance.Text = "المظهر"
        Me.GrpAppearance.Font = New System.Drawing.Font("Tahoma", 8.0F)
        Me.GrpAppearance.RightToLeft = RightToLeft.Yes
        Me.GrpAppearance.Dock = DockStyle.Top
        Me.GrpAppearance.Height = 192

        Me.LblFgColor.Text = "لون النص:"
        Me.LblFgColor.Location = New System.Drawing.Point(128, 18)
        Me.LblFgColor.Size = New System.Drawing.Size(70, 20)
        Me.LblFgColor.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.GrpAppearance.Controls.Add(Me.LblFgColor)

        Me.PnlFgColor.Location = New System.Drawing.Point(104, 18)
        Me.PnlFgColor.Size = New System.Drawing.Size(22, 20)
        Me.PnlFgColor.BackColor = System.Drawing.Color.Black
        Me.PnlFgColor.BorderStyle = BorderStyle.FixedSingle
        Me.PnlFgColor.Cursor = Cursors.Hand
        Me.GrpAppearance.Controls.Add(Me.PnlFgColor)

        Me.LblFgHint.Text = "انقر للتغيير"
        Me.LblFgHint.Location = New System.Drawing.Point(6, 18)
        Me.LblFgHint.Size = New System.Drawing.Size(96, 20)
        Me.LblFgHint.ForeColor = System.Drawing.Color.Gray
        Me.LblFgHint.Font = New System.Drawing.Font("Tahoma", 7.5F)
        Me.LblFgHint.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.GrpAppearance.Controls.Add(Me.LblFgHint)

        Me.LblBgColor.Text = "لون الخلفية:"
        Me.LblBgColor.Location = New System.Drawing.Point(122, 42)
        Me.LblBgColor.Size = New System.Drawing.Size(76, 20)
        Me.LblBgColor.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.GrpAppearance.Controls.Add(Me.LblBgColor)

        Me.PnlBgColor.Location = New System.Drawing.Point(104, 42)
        Me.PnlBgColor.Size = New System.Drawing.Size(22, 20)
        Me.PnlBgColor.BackColor = System.Drawing.Color.WhiteSmoke
        Me.PnlBgColor.BorderStyle = BorderStyle.FixedSingle
        Me.PnlBgColor.Cursor = Cursors.Hand
        Me.GrpAppearance.Controls.Add(Me.PnlBgColor)

        Me.LblBgHint.Text = "انقر للتغيير"
        Me.LblBgHint.Location = New System.Drawing.Point(6, 42)
        Me.LblBgHint.Size = New System.Drawing.Size(96, 20)
        Me.LblBgHint.ForeColor = System.Drawing.Color.Gray
        Me.LblBgHint.Font = New System.Drawing.Font("Tahoma", 7.5F)
        Me.LblBgHint.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.GrpAppearance.Controls.Add(Me.LblBgHint)

        Me.LblFont.Text = "الخط:"
        Me.LblFont.Location = New System.Drawing.Point(160, 66)
        Me.LblFont.Size = New System.Drawing.Size(38, 20)
        Me.LblFont.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.GrpAppearance.Controls.Add(Me.LblFont)

        Me.CmbFontName.Location = New System.Drawing.Point(6, 66)
        Me.CmbFontName.Size = New System.Drawing.Size(108, 21)
        Me.CmbFontName.DropDownStyle = ComboBoxStyle.DropDownList
        Me.CmbFontName.Font = New System.Drawing.Font("Tahoma", 8.0F)
        Me.GrpAppearance.Controls.Add(Me.CmbFontName)

        Me.CmbFontSize.Location = New System.Drawing.Point(116, 66)
        Me.CmbFontSize.Size = New System.Drawing.Size(42, 21)
        Me.CmbFontSize.DropDownStyle = ComboBoxStyle.DropDownList
        Me.CmbFontSize.Font = New System.Drawing.Font("Tahoma", 8.0F)
        Me.GrpAppearance.Controls.Add(Me.CmbFontSize)

        Me.ChkFontBold.Text = "عريض"
        Me.ChkFontBold.Location = New System.Drawing.Point(6, 90)
        Me.ChkFontBold.Size = New System.Drawing.Size(70, 20)
        Me.GrpAppearance.Controls.Add(Me.ChkFontBold)

        Me.LblRelief.Text = "Relief:"
        Me.LblRelief.Location = New System.Drawing.Point(160, 114)
        Me.LblRelief.Size = New System.Drawing.Size(38, 20)
        Me.LblRelief.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.GrpAppearance.Controls.Add(Me.LblRelief)

        Me.CmbRelief.Location = New System.Drawing.Point(6, 114)
        Me.CmbRelief.Size = New System.Drawing.Size(152, 21)
        Me.CmbRelief.DropDownStyle = ComboBoxStyle.DropDownList
        Me.GrpAppearance.Controls.Add(Me.CmbRelief)

        Me.LblCommand.Text = "command (اسم الدالة):"
        Me.LblCommand.Location = New System.Drawing.Point(6, 140)
        Me.LblCommand.Size = New System.Drawing.Size(192, 16)
        Me.LblCommand.Font = New System.Drawing.Font("Tahoma", 7.5F)
        Me.LblCommand.ForeColor = System.Drawing.Color.FromArgb(60, 60, 60)
        Me.LblCommand.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.GrpAppearance.Controls.Add(Me.LblCommand)

        Me.PropCommandBox.Location = New System.Drawing.Point(6, 158)
        Me.PropCommandBox.Size = New System.Drawing.Size(140, 20)
        Me.PropCommandBox.Font = New System.Drawing.Font("Courier New", 8.5F)
        Me.GrpAppearance.Controls.Add(Me.PropCommandBox)

        Me.BtnChangeImage.Text = "تغيير الصورة"
        Me.BtnChangeImage.Location = New System.Drawing.Point(148, 156)
        Me.BtnChangeImage.Size = New System.Drawing.Size(56, 22)
        Me.BtnChangeImage.Font = New System.Drawing.Font("Tahoma", 7.0F)
        Me.BtnChangeImage.BackColor = System.Drawing.Color.FromArgb(24, 95, 165)
        Me.BtnChangeImage.ForeColor = System.Drawing.Color.White
        Me.BtnChangeImage.FlatStyle = FlatStyle.Flat
        Me.BtnChangeImage.Visible = False
        Me.GrpAppearance.Controls.Add(Me.BtnChangeImage)

        ' ======================
        ' GrpLayout
        ' ======================
        Me.GrpLayout.Text = "Layout & الحاوية"
        Me.GrpLayout.Font = New System.Drawing.Font("Tahoma", 8.0F)
        Me.GrpLayout.RightToLeft = RightToLeft.Yes
        Me.GrpLayout.Dock = DockStyle.Top
        Me.GrpLayout.Height = 150

        Me.LblLayoutMode.Text = "نوع Layout:"
        Me.LblLayoutMode.Location = New System.Drawing.Point(128, 18)
        Me.LblLayoutMode.Size = New System.Drawing.Size(72, 20)
        Me.LblLayoutMode.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.GrpLayout.Controls.Add(Me.LblLayoutMode)

        Me.CmbLayoutMode.Location = New System.Drawing.Point(6, 18)
        Me.CmbLayoutMode.Size = New System.Drawing.Size(120, 21)
        Me.CmbLayoutMode.DropDownStyle = ComboBoxStyle.DropDownList
        Me.GrpLayout.Controls.Add(Me.CmbLayoutMode)

        Me.LblParentFrame.Text = "الحاوية (Frame):"
        Me.LblParentFrame.Location = New System.Drawing.Point(110, 44)
        Me.LblParentFrame.Size = New System.Drawing.Size(90, 20)
        Me.LblParentFrame.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.GrpLayout.Controls.Add(Me.LblParentFrame)

        Me.CmbParentFrame.Location = New System.Drawing.Point(6, 44)
        Me.CmbParentFrame.Size = New System.Drawing.Size(102, 21)
        Me.CmbParentFrame.DropDownStyle = ComboBoxStyle.DropDownList
        Me.GrpLayout.Controls.Add(Me.CmbParentFrame)

        ' --- PnlPlaceOptions ---
        Me.PnlPlaceOptions.Location = New System.Drawing.Point(6, 70)
        Me.PnlPlaceOptions.Size = New System.Drawing.Size(200, 22)
        Me.PnlPlaceOptions.Visible = True
        Me.GrpLayout.Controls.Add(Me.PnlPlaceOptions)

        Me.LblPlaceNote.Text = "الموقع من لوحة الخصائص (X, Y, W, H)"
        Me.LblPlaceNote.Location = New System.Drawing.Point(0, 0)
        Me.LblPlaceNote.Size = New System.Drawing.Size(200, 20)
        Me.LblPlaceNote.Font = New System.Drawing.Font("Tahoma", 7.5F)
        Me.LblPlaceNote.ForeColor = System.Drawing.Color.Gray
        Me.LblPlaceNote.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.PnlPlaceOptions.Controls.Add(Me.LblPlaceNote)

        ' --- PnlPackOptions ---
        Me.PnlPackOptions.Location = New System.Drawing.Point(6, 70)
        Me.PnlPackOptions.Size = New System.Drawing.Size(200, 70)
        Me.PnlPackOptions.Visible = False
        Me.GrpLayout.Controls.Add(Me.PnlPackOptions)

        Me.LblPackSide.Text = "side:"
        Me.LblPackSide.Location = New System.Drawing.Point(160, 2)
        Me.LblPackSide.Size = New System.Drawing.Size(36, 20)
        Me.LblPackSide.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.PnlPackOptions.Controls.Add(Me.LblPackSide)

        Me.CmbPackSide.Location = New System.Drawing.Point(80, 2)
        Me.CmbPackSide.Size = New System.Drawing.Size(78, 21)
        Me.CmbPackSide.DropDownStyle = ComboBoxStyle.DropDownList
        Me.PnlPackOptions.Controls.Add(Me.CmbPackSide)

        Me.LblPackFill.Text = "fill:"
        Me.LblPackFill.Location = New System.Drawing.Point(160, 26)
        Me.LblPackFill.Size = New System.Drawing.Size(36, 20)
        Me.LblPackFill.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.PnlPackOptions.Controls.Add(Me.LblPackFill)

        Me.CmbPackFill.Location = New System.Drawing.Point(80, 26)
        Me.CmbPackFill.Size = New System.Drawing.Size(78, 21)
        Me.CmbPackFill.DropDownStyle = ComboBoxStyle.DropDownList
        Me.PnlPackOptions.Controls.Add(Me.CmbPackFill)

        Me.ChkPackExpand.Text = "expand=True"
        Me.ChkPackExpand.Location = New System.Drawing.Point(80, 50)
        Me.ChkPackExpand.Size = New System.Drawing.Size(100, 20)
        Me.ChkPackExpand.Font = New System.Drawing.Font("Tahoma", 8.0F)
        Me.PnlPackOptions.Controls.Add(Me.ChkPackExpand)

        ' --- PnlGridOptions ---
        Me.PnlGridOptions.Location = New System.Drawing.Point(6, 70)
        Me.PnlGridOptions.Size = New System.Drawing.Size(200, 70)
        Me.PnlGridOptions.Visible = False
        Me.GrpLayout.Controls.Add(Me.PnlGridOptions)

        Me.LblGridRow.Text = "row:"
        Me.LblGridRow.Location = New System.Drawing.Point(162, 2)
        Me.LblGridRow.Size = New System.Drawing.Size(34, 20)
        Me.LblGridRow.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.PnlGridOptions.Controls.Add(Me.LblGridRow)

        Me.TxtGridRow.Location = New System.Drawing.Point(120, 2)
        Me.TxtGridRow.Size = New System.Drawing.Size(40, 20)
        Me.TxtGridRow.Text = "0"
        Me.PnlGridOptions.Controls.Add(Me.TxtGridRow)

        Me.LblGridCol.Text = "col:"
        Me.LblGridCol.Location = New System.Drawing.Point(114, 2)
        Me.LblGridCol.Size = New System.Drawing.Size(4, 20)

        Me.LblGridCol.Text = "column:"
        Me.LblGridCol.Location = New System.Drawing.Point(66, 2)
        Me.LblGridCol.Size = New System.Drawing.Size(52, 20)
        Me.LblGridCol.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.PnlGridOptions.Controls.Add(Me.LblGridCol)

        Me.TxtGridCol.Location = New System.Drawing.Point(20, 2)
        Me.TxtGridCol.Size = New System.Drawing.Size(44, 20)
        Me.TxtGridCol.Text = "0"
        Me.PnlGridOptions.Controls.Add(Me.TxtGridCol)

        Me.LblGridSticky.Text = "sticky:"
        Me.LblGridSticky.Location = New System.Drawing.Point(150, 28)
        Me.LblGridSticky.Size = New System.Drawing.Size(46, 20)
        Me.LblGridSticky.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.PnlGridOptions.Controls.Add(Me.LblGridSticky)

        Me.CmbGridSticky.Location = New System.Drawing.Point(6, 28)
        Me.CmbGridSticky.Size = New System.Drawing.Size(142, 21)
        Me.CmbGridSticky.DropDownStyle = ComboBoxStyle.DropDownList
        Me.PnlGridOptions.Controls.Add(Me.CmbGridSticky)

        ' ======================
        ' BtnApplyProps / BtnDelete
        ' ======================
        Me.BtnApplyProps.Text = "✓ تطبيق الخصائص"
        Me.BtnApplyProps.Dock = DockStyle.Top
        Me.BtnApplyProps.Height = 28
        Me.BtnApplyProps.Font = New System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Bold)
        Me.BtnApplyProps.BackColor = System.Drawing.Color.FromArgb(24, 95, 165)
        Me.BtnApplyProps.ForeColor = System.Drawing.Color.White
        Me.BtnApplyProps.FlatStyle = FlatStyle.Flat

        Me.BtnDelete.Text = "✕ حذف المحدد (Del)"
        Me.BtnDelete.Dock = DockStyle.Top
        Me.BtnDelete.Height = 26
        Me.BtnDelete.Font = New System.Drawing.Font("Tahoma", 8.0F)
        Me.BtnDelete.BackColor = System.Drawing.Color.FromArgb(163, 45, 45)
        Me.BtnDelete.ForeColor = System.Drawing.Color.White
        Me.BtnDelete.FlatStyle = FlatStyle.Flat

        ' ======================
        ' GrpCodePreview
        ' ======================
        Me.GrpCodePreview.Text = "معاينة الكود — Python"
        Me.GrpCodePreview.Font = New System.Drawing.Font("Tahoma", 8.0F)
        Me.GrpCodePreview.RightToLeft = RightToLeft.No
        Me.GrpCodePreview.Dock = DockStyle.Fill

        Me.CodePreviewBox.Dock = DockStyle.Fill
        Me.CodePreviewBox.Font = New System.Drawing.Font("Courier New", 8.5F)
        Me.CodePreviewBox.BackColor = System.Drawing.Color.FromArgb(28, 28, 28)
        Me.CodePreviewBox.ForeColor = System.Drawing.Color.FromArgb(200, 230, 200)
        Me.CodePreviewBox.ReadOnly = True
        Me.CodePreviewBox.BorderStyle = BorderStyle.None
        Me.CodePreviewBox.ScrollBars = RichTextBoxScrollBars.Both
        Me.CodePreviewBox.WordWrap = False
        Me.GrpCodePreview.Controls.Add(Me.CodePreviewBox)

        Me.BtnExport.Text = "⬇ تصدير كود .py"
        Me.BtnExport.Dock = DockStyle.Bottom
        Me.BtnExport.Height = 28
        Me.BtnExport.Font = New System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Bold)
        Me.BtnExport.BackColor = System.Drawing.Color.FromArgb(59, 109, 17)
        Me.BtnExport.ForeColor = System.Drawing.Color.White
        Me.BtnExport.FlatStyle = FlatStyle.Flat
        Me.GrpCodePreview.Controls.Add(Me.BtnExport)

        Me.BtnSaveProject.Text = "💾 حفظ المشروع (.pfb)"
        Me.BtnSaveProject.Dock = DockStyle.Bottom
        Me.BtnSaveProject.Height = 26
        Me.BtnSaveProject.Font = New System.Drawing.Font("Tahoma", 8.0F)
        Me.BtnSaveProject.BackColor = System.Drawing.Color.FromArgb(24, 95, 165)
        Me.BtnSaveProject.ForeColor = System.Drawing.Color.White
        Me.BtnSaveProject.FlatStyle = FlatStyle.Flat
        Me.GrpCodePreview.Controls.Add(Me.BtnSaveProject)

        Me.BtnOpenProject.Text = "📂 فتح مشروع (.pfb)"
        Me.BtnOpenProject.Dock = DockStyle.Bottom
        Me.BtnOpenProject.Height = 26
        Me.BtnOpenProject.Font = New System.Drawing.Font("Tahoma", 8.0F)
        Me.BtnOpenProject.BackColor = System.Drawing.Color.FromArgb(80, 80, 80)
        Me.BtnOpenProject.ForeColor = System.Drawing.Color.White
        Me.BtnOpenProject.FlatStyle = FlatStyle.Flat
        Me.GrpCodePreview.Controls.Add(Me.BtnOpenProject)

        Me.BtnClear.Text = "مسح اللوحة"
        Me.BtnClear.Dock = DockStyle.Bottom
        Me.BtnClear.Height = 24
        Me.BtnClear.Font = New System.Drawing.Font("Tahoma", 8.0F)
        Me.BtnClear.BackColor = System.Drawing.Color.FromArgb(130, 80, 10)
        Me.BtnClear.ForeColor = System.Drawing.Color.White
        Me.BtnClear.FlatStyle = FlatStyle.Flat
        Me.GrpCodePreview.Controls.Add(Me.BtnClear)

        ' ======================
        ' SplitContainers
        ' ======================
        Me.MainSplitter.Orientation = Orientation.Vertical
        Me.MainSplitter.Panel1.Controls.Add(Me.PanelLeft)
        Me.MainSplitter.Panel2.Controls.Add(Me.RightSplitter)
        Me.MainSplitter.Dock = DockStyle.Fill

        Me.RightSplitter.Orientation = Orientation.Vertical
        Me.RightSplitter.Panel1.Controls.Add(Me.PanelCenter)
        Me.RightSplitter.Panel2.Controls.Add(Me.PanelRight)
        Me.RightSplitter.Dock = DockStyle.Fill

        ' ======================
        ' StatusStrip
        ' ======================
        Me.MainStatusStrip.RightToLeft = RightToLeft.Yes
        Me.MainStatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {
            Me.StatusLabel, Me.LblPanelSize, Me.LblControlCount})
        Me.StatusLabel.Text = "جاهز"
        Me.StatusLabel.Spring = True
        Me.StatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.LblPanelSize.Text = "البانل: 600 × 500"
        Me.LblPanelSize.BorderSides = ToolStripStatusLabelBorderSides.Left
        Me.LblPanelSize.ForeColor = System.Drawing.Color.FromArgb(24, 95, 165)
        Me.LblControlCount.Text = "العناصر: 0"
        Me.LblControlCount.BorderSides = ToolStripStatusLabelBorderSides.Left

        ' ======================
        ' MainForm
        ' ======================
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0F, 13.0F)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1150, 730)
        Me.MinimumSize = New System.Drawing.Size(950, 640)
        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.RightToLeftLayout = True
        Me.Font = New System.Drawing.Font("Tahoma", 8.5F)
        Me.Text = "PyForm Builder — مصمم واجهات بايثون"
        Me.StartPosition = FormStartPosition.CenterScreen

        Me.Controls.Add(Me.MainSplitter)
        Me.Controls.Add(Me.AlignToolStrip)
        Me.Controls.Add(Me.MainToolStrip)
        Me.Controls.Add(Me.MainMenu)
        Me.Controls.Add(Me.MainStatusStrip)
        Me.MainMenuStrip = Me.MainMenu

        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

End Class