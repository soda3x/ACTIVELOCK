'*   ActiveLock
'*   Copyright 1998-2002 Nelson Ferraz
'*   Copyright 2003-2006 The ActiveLock Software Group (ASG)
'*   All material is the property of the contributing authors.
'*
'*   Redistribution and use in source and binary forms, with or without
'*   modification, are permitted provided that the following conditions are
'*   met:
'*
'*     [o] Redistributions of source code must retain the above copyright
'*         notice, this list of conditions and the following disclaimer.
'*
'*     [o] Redistributions in binary form must reproduce the above
'*         copyright notice, this list of conditions and the following
'*         disclaimer in the documentation and/or other materials provided
'*         with the distribution.
'*
'*   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
'*   "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
'*   LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
'*   A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
'*   OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
'*   SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
'*   LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
'*   DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
'*   THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
'*   (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
'*   OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
'*
'*
'Option Strict Off
Option Explicit On
Imports System.IO
Imports System.Text
Imports System.Windows.Forms
Imports ActiveLock3_5NET

Friend Class frmMain
  Inherits System.Windows.Forms.Form

#Region "Private variables "
  'listview sorter class
  Private lvwColumnSorter As ListViewColumnSorter

  'ActiveLock objects
  Public GeneratorInstance As _IALUGenerator
  Public ActiveLock As _IActiveLock
  Public fDisableNotifications As Boolean

  ' Hardware keys from the Installation Code
  Private MACaddress, ComputerName As String
  Private VolumeSerial, FirmwareSerial As String
  Private WindowsSerial, BIOSserial As String
  Private MotherboardSerial, IPaddress As String
  Private systemEvent As Boolean

  Private PROJECT_INI_FILENAME As String

  Public Shared resxList As New Collections.Specialized.ListDictionary

  Private printPreviewDialog1 As New PrintPreviewDialog

#End Region

#Region "Windows Form Designer generated code "
  Public Sub New()
    MyBase.New()
    'This call is required by the Windows Form Designer.
    InitializeComponent()

    ' Create an instance of a ListView column sorter and assign it 
    ' to the ListView control.
    lvwColumnSorter = New ListViewColumnSorter
    Me.lstvwProducts.ListViewItemSorter = lvwColumnSorter
  End Sub
  'Form overrides dispose to clean up the component list.
  Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
    If Disposing Then
      If Not components Is Nothing Then
        components.Dispose()
      End If
    End If
    MyBase.Dispose(Disposing)
  End Sub
  'Required by the Windows Form Designer
  Private components As System.ComponentModel.IContainer
  Friend ToolTip1 As System.Windows.Forms.ToolTip
  Friend WithEvents cmdCopyGCode As System.Windows.Forms.Button
  Friend WithEvents cmdCopyVCode As System.Windows.Forms.Button
  Friend WithEvents cmdCodeGen As System.Windows.Forms.Button
  Friend WithEvents txtName As System.Windows.Forms.TextBox
  Friend WithEvents txtVer As System.Windows.Forms.TextBox
  Friend WithEvents cmdAdd As System.Windows.Forms.Button
  Friend WithEvents Label2 As System.Windows.Forms.Label
  Friend WithEvents Label3 As System.Windows.Forms.Label
  Friend WithEvents fraProdNew As System.Windows.Forms.GroupBox
  Friend WithEvents cmdRemove As System.Windows.Forms.Button
  Friend WithEvents cmdValidate As System.Windows.Forms.Button
  Friend WithEvents _SSTab1_TabPage0 As System.Windows.Forms.TabPage
  Friend WithEvents Label8 As System.Windows.Forms.Label
  Friend WithEvents Label15 As System.Windows.Forms.Label
  Friend WithEvents chkLockIP As System.Windows.Forms.CheckBox
  Friend WithEvents chkLockMotherboard As System.Windows.Forms.CheckBox
  Friend WithEvents chkLockBIOS As System.Windows.Forms.CheckBox
  Friend WithEvents chkLockWindows As System.Windows.Forms.CheckBox
  Friend WithEvents chkLockHDfirmware As System.Windows.Forms.CheckBox
  Friend WithEvents chkLockHD As System.Windows.Forms.CheckBox
  Friend WithEvents chkLockComputer As System.Windows.Forms.CheckBox
  Friend WithEvents chkLockMACaddress As System.Windows.Forms.CheckBox
  Friend WithEvents chkItemData As System.Windows.Forms.CheckBox
  Friend WithEvents cmdCopy As System.Windows.Forms.Button
  Friend WithEvents cmdPaste As System.Windows.Forms.Button
  Friend WithEvents txtUser As System.Windows.Forms.TextBox
  Friend WithEvents cmdBrowse As System.Windows.Forms.Button
  Friend WithEvents cmdSave As System.Windows.Forms.Button
  Friend WithEvents txtDays As System.Windows.Forms.TextBox
  Friend WithEvents cmdKeyGen As System.Windows.Forms.Button
  Friend WithEvents Label11 As System.Windows.Forms.Label
  Friend WithEvents Label5 As System.Windows.Forms.Label
  Friend WithEvents lblExpiry As System.Windows.Forms.Label
  Friend WithEvents Label6 As System.Windows.Forms.Label
  Friend WithEvents Label7 As System.Windows.Forms.Label
  Friend WithEvents lblDays As System.Windows.Forms.Label
  Friend WithEvents frmKeyGen As System.Windows.Forms.Panel
  Friend WithEvents cmdViewArchive As System.Windows.Forms.Button
  Friend WithEvents _SSTab1_TabPage1 As System.Windows.Forms.TabPage
  Friend WithEvents SSTab1 As System.Windows.Forms.TabControl
  Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
  Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
  Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
  Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
  Friend WithEvents lstvwProducts As System.Windows.Forms.ListView
  Friend WithEvents sbStatus As System.Windows.Forms.StatusBar
  Friend WithEvents mainStatusBarPanel As System.Windows.Forms.StatusBarPanel
  Friend WithEvents saveDlg As System.Windows.Forms.SaveFileDialog
  Friend WithEvents cmdViewLevel As System.Windows.Forms.Button
  Friend WithEvents grpCodes As System.Windows.Forms.GroupBox
  Friend WithEvents grpProductsList As System.Windows.Forms.GroupBox
  Friend WithEvents picALBanner As System.Windows.Forms.PictureBox
  Friend WithEvents picALBanner2 As System.Windows.Forms.PictureBox
  'NOTE: The following procedure is required by the Windows Form Designer
  'It can be modified using the Windows Form Designer.
  'Do not modify it using the code editor.
  Friend WithEvents lblGCode As System.Windows.Forms.Label
  Friend WithEvents lblVCode As System.Windows.Forms.Label
  Friend WithEvents lblLicenseKey As System.Windows.Forms.Label
  Friend WithEvents dtpExpireDate As DateTimePicker
  Friend WithEvents txtVCode As System.Windows.Forms.TextBox
  Friend WithEvents txtGCode As System.Windows.Forms.TextBox
  Friend WithEvents txtInstallCode As System.Windows.Forms.TextBox
  Friend WithEvents txtLicenseKey As System.Windows.Forms.TextBox
  Friend WithEvents txtLicenseFile As System.Windows.Forms.TextBox
  Friend WithEvents cboProducts As System.Windows.Forms.ComboBox
  Friend WithEvents lnkActivelockSoftwareGroup As System.Windows.Forms.LinkLabel
  Friend WithEvents cmdPrintLicenseKey As System.Windows.Forms.Button
  Friend WithEvents cboRegisteredLevel As System.Windows.Forms.ComboBox
  Friend WithEvents cboLicType As System.Windows.Forms.ComboBox
  Friend WithEvents cmdEmailLicenseKey As System.Windows.Forms.Button
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Me.components = New System.ComponentModel.Container
    Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmMain))
    Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
    Me.txtVCode = New System.Windows.Forms.TextBox
    Me.cmdBrowse = New System.Windows.Forms.Button
    Me.cmdSave = New System.Windows.Forms.Button
    Me.cmdKeyGen = New System.Windows.Forms.Button
    Me.cmdViewArchive = New System.Windows.Forms.Button
    Me.lstvwProducts = New System.Windows.Forms.ListView
    Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
    Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
    Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
    Me.ColumnHeader4 = New System.Windows.Forms.ColumnHeader
    Me.picALBanner2 = New System.Windows.Forms.PictureBox
    Me.cmdCopyVCode = New System.Windows.Forms.Button
    Me.cmdCopyGCode = New System.Windows.Forms.Button
    Me.cmdCodeGen = New System.Windows.Forms.Button
    Me.txtName = New System.Windows.Forms.TextBox
    Me.txtVer = New System.Windows.Forms.TextBox
    Me.cmdAdd = New System.Windows.Forms.Button
    Me.cmdValidate = New System.Windows.Forms.Button
    Me.cmdRemove = New System.Windows.Forms.Button
    Me.picALBanner = New System.Windows.Forms.PictureBox
    Me.cmdCopy = New System.Windows.Forms.Button
    Me.cmdPaste = New System.Windows.Forms.Button
    Me.txtUser = New System.Windows.Forms.TextBox
    Me.txtLicenseFile = New System.Windows.Forms.TextBox
    Me.cboLicType = New System.Windows.Forms.ComboBox
    Me.txtInstallCode = New System.Windows.Forms.TextBox
    Me.txtLicenseKey = New System.Windows.Forms.TextBox
    Me.cboRegisteredLevel = New System.Windows.Forms.ComboBox
    Me.cboProducts = New System.Windows.Forms.ComboBox
    Me.cmdViewLevel = New System.Windows.Forms.Button
    Me.cmdPrintLicenseKey = New System.Windows.Forms.Button
    Me.cmdEmailLicenseKey = New System.Windows.Forms.Button
    Me.SSTab1 = New System.Windows.Forms.TabControl
    Me._SSTab1_TabPage0 = New System.Windows.Forms.TabPage
    Me.grpProductsList = New System.Windows.Forms.GroupBox
    Me.fraProdNew = New System.Windows.Forms.GroupBox
    Me.grpCodes = New System.Windows.Forms.GroupBox
    Me.txtGCode = New System.Windows.Forms.TextBox
    Me.lblGCode = New System.Windows.Forms.Label
    Me.lblVCode = New System.Windows.Forms.Label
    Me.Label2 = New System.Windows.Forms.Label
    Me.Label3 = New System.Windows.Forms.Label
    Me._SSTab1_TabPage1 = New System.Windows.Forms.TabPage
    Me.frmKeyGen = New System.Windows.Forms.Panel
    Me.dtpExpireDate = New System.Windows.Forms.DateTimePicker
    Me.chkLockIP = New System.Windows.Forms.CheckBox
    Me.chkLockMotherboard = New System.Windows.Forms.CheckBox
    Me.chkLockBIOS = New System.Windows.Forms.CheckBox
    Me.chkLockWindows = New System.Windows.Forms.CheckBox
    Me.chkLockHDfirmware = New System.Windows.Forms.CheckBox
    Me.chkLockHD = New System.Windows.Forms.CheckBox
    Me.chkLockComputer = New System.Windows.Forms.CheckBox
    Me.chkLockMACaddress = New System.Windows.Forms.CheckBox
    Me.chkItemData = New System.Windows.Forms.CheckBox
    Me.txtDays = New System.Windows.Forms.TextBox
    Me.Label11 = New System.Windows.Forms.Label
    Me.Label5 = New System.Windows.Forms.Label
    Me.lblExpiry = New System.Windows.Forms.Label
    Me.Label6 = New System.Windows.Forms.Label
    Me.Label7 = New System.Windows.Forms.Label
    Me.lblLicenseKey = New System.Windows.Forms.Label
    Me.lblDays = New System.Windows.Forms.Label
    Me.Label8 = New System.Windows.Forms.Label
    Me.Label15 = New System.Windows.Forms.Label
    Me.sbStatus = New System.Windows.Forms.StatusBar
    Me.mainStatusBarPanel = New System.Windows.Forms.StatusBarPanel
    Me.saveDlg = New System.Windows.Forms.SaveFileDialog
    Me.lnkActivelockSoftwareGroup = New System.Windows.Forms.LinkLabel
    Me.SSTab1.SuspendLayout()
    Me._SSTab1_TabPage0.SuspendLayout()
    Me.grpProductsList.SuspendLayout()
    Me.fraProdNew.SuspendLayout()
    Me.grpCodes.SuspendLayout()
    Me._SSTab1_TabPage1.SuspendLayout()
    Me.frmKeyGen.SuspendLayout()
    CType(Me.mainStatusBarPanel, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'txtVCode
    '
    Me.txtVCode.AcceptsReturn = True
    Me.txtVCode.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtVCode.AutoSize = False
    Me.txtVCode.BackColor = System.Drawing.SystemColors.Control
    Me.txtVCode.Cursor = System.Windows.Forms.Cursors.IBeam
    Me.txtVCode.ForeColor = System.Drawing.SystemColors.WindowText
    Me.txtVCode.Location = New System.Drawing.Point(84, 20)
    Me.txtVCode.MaxLength = 0
    Me.txtVCode.Multiline = True
    Me.txtVCode.Name = "txtVCode"
    Me.txtVCode.ReadOnly = True
    Me.txtVCode.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.txtVCode.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
    Me.txtVCode.Size = New System.Drawing.Size(466, 52)
    Me.txtVCode.TabIndex = 2
    Me.txtVCode.Text = ""
    Me.ToolTip1.SetToolTip(Me.txtVCode, "Use this code to set ActiveLock's SoftwareCode property within your application.")
    '
    'cmdBrowse
    '
    Me.cmdBrowse.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.cmdBrowse.BackColor = System.Drawing.SystemColors.Control
    Me.cmdBrowse.Cursor = System.Windows.Forms.Cursors.Default
    Me.cmdBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Popup
    Me.cmdBrowse.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.cmdBrowse.ForeColor = System.Drawing.SystemColors.ControlText
    Me.cmdBrowse.Location = New System.Drawing.Point(477, 474)
    Me.cmdBrowse.Name = "cmdBrowse"
    Me.cmdBrowse.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.cmdBrowse.Size = New System.Drawing.Size(21, 22)
    Me.cmdBrowse.TabIndex = 32
    Me.ToolTip1.SetToolTip(Me.cmdBrowse, "Browse for license file.")
    '
    'cmdSave
    '
    Me.cmdSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.cmdSave.BackColor = System.Drawing.SystemColors.Control
    Me.cmdSave.Cursor = System.Windows.Forms.Cursors.Default
    Me.cmdSave.Enabled = False
    Me.cmdSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup
    Me.cmdSave.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.cmdSave.ForeColor = System.Drawing.SystemColors.ControlText
    Me.cmdSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
    Me.cmdSave.Location = New System.Drawing.Point(499, 474)
    Me.cmdSave.Name = "cmdSave"
    Me.cmdSave.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.cmdSave.Size = New System.Drawing.Size(65, 22)
    Me.cmdSave.TabIndex = 33
    Me.cmdSave.Text = "&Save"
    Me.cmdSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
    Me.ToolTip1.SetToolTip(Me.cmdSave, "Save into file generated license key for the above request code (which should not" & _
    " be blank).")
    '
    'cmdKeyGen
    '
    Me.cmdKeyGen.BackColor = System.Drawing.SystemColors.Control
    Me.cmdKeyGen.Cursor = System.Windows.Forms.Cursors.Default
    Me.cmdKeyGen.Enabled = False
    Me.cmdKeyGen.FlatStyle = System.Windows.Forms.FlatStyle.Popup
    Me.cmdKeyGen.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.cmdKeyGen.ForeColor = System.Drawing.SystemColors.ControlText
    Me.cmdKeyGen.ImageAlign = System.Drawing.ContentAlignment.BottomLeft
    Me.cmdKeyGen.Location = New System.Drawing.Point(2, 322)
    Me.cmdKeyGen.Name = "cmdKeyGen"
    Me.cmdKeyGen.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.cmdKeyGen.Size = New System.Drawing.Size(80, 24)
    Me.cmdKeyGen.TabIndex = 26
    Me.cmdKeyGen.Text = "&Generate"
    Me.cmdKeyGen.TextAlign = System.Drawing.ContentAlignment.MiddleRight
    Me.ToolTip1.SetToolTip(Me.cmdKeyGen, "Generate license key for the above request code (which should not be blank).")
    '
    'cmdViewArchive
    '
    Me.cmdViewArchive.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
    Me.cmdViewArchive.BackColor = System.Drawing.SystemColors.Control
    Me.cmdViewArchive.Cursor = System.Windows.Forms.Cursors.Default
    Me.cmdViewArchive.FlatStyle = System.Windows.Forms.FlatStyle.Popup
    Me.cmdViewArchive.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.cmdViewArchive.ForeColor = System.Drawing.SystemColors.ControlText
    Me.cmdViewArchive.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
    Me.cmdViewArchive.Location = New System.Drawing.Point(3, 426)
    Me.cmdViewArchive.Name = "cmdViewArchive"
    Me.cmdViewArchive.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.cmdViewArchive.Size = New System.Drawing.Size(79, 44)
    Me.cmdViewArchive.TabIndex = 29
    Me.cmdViewArchive.Text = "&View License Database"
    Me.cmdViewArchive.TextAlign = System.Drawing.ContentAlignment.MiddleRight
    Me.ToolTip1.SetToolTip(Me.cmdViewArchive, "View License Archive")
    '
    'lstvwProducts
    '
    Me.lstvwProducts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
    Me.lstvwProducts.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4})
    Me.lstvwProducts.Dock = System.Windows.Forms.DockStyle.Fill
    Me.lstvwProducts.FullRowSelect = True
    Me.lstvwProducts.GridLines = True
    Me.lstvwProducts.HideSelection = False
    Me.lstvwProducts.Location = New System.Drawing.Point(3, 16)
    Me.lstvwProducts.MultiSelect = False
    Me.lstvwProducts.Name = "lstvwProducts"
    Me.lstvwProducts.Size = New System.Drawing.Size(558, 239)
    Me.lstvwProducts.Sorting = System.Windows.Forms.SortOrder.Ascending
    Me.lstvwProducts.TabIndex = 10
    Me.ToolTip1.SetToolTip(Me.lstvwProducts, "Products list")
    Me.lstvwProducts.View = System.Windows.Forms.View.Details
    '
    'ColumnHeader1
    '
    Me.ColumnHeader1.Text = "Name"
    Me.ColumnHeader1.Width = 140
    '
    'ColumnHeader2
    '
    Me.ColumnHeader2.Text = "Version"
    Me.ColumnHeader2.Width = 80
    '
    'ColumnHeader3
    '
    Me.ColumnHeader3.Text = "VCode"
    Me.ColumnHeader3.Width = 120
    '
    'ColumnHeader4
    '
    Me.ColumnHeader4.Text = "GCode"
    Me.ColumnHeader4.Width = 120
    '
    'picALBanner2
    '
    Me.picALBanner2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.picALBanner2.BackColor = System.Drawing.SystemColors.ActiveCaptionText
    Me.picALBanner2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
    Me.picALBanner2.Cursor = System.Windows.Forms.Cursors.Hand
    Me.picALBanner2.Location = New System.Drawing.Point(486, 24)
    Me.picALBanner2.Name = "picALBanner2"
    Me.picALBanner2.Size = New System.Drawing.Size(74, 38)
    Me.picALBanner2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
    Me.picALBanner2.TabIndex = 64
    Me.picALBanner2.TabStop = False
    Me.ToolTip1.SetToolTip(Me.picALBanner2, "www.activelocksoftware.com")
    '
    'cmdCopyVCode
    '
    Me.cmdCopyVCode.BackColor = System.Drawing.SystemColors.Control
    Me.cmdCopyVCode.Cursor = System.Windows.Forms.Cursors.Default
    Me.cmdCopyVCode.FlatStyle = System.Windows.Forms.FlatStyle.Popup
    Me.cmdCopyVCode.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.cmdCopyVCode.ForeColor = System.Drawing.SystemColors.ControlText
    Me.cmdCopyVCode.Location = New System.Drawing.Point(60, 48)
    Me.cmdCopyVCode.Name = "cmdCopyVCode"
    Me.cmdCopyVCode.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.cmdCopyVCode.Size = New System.Drawing.Size(23, 23)
    Me.cmdCopyVCode.TabIndex = 1
    Me.cmdCopyVCode.TextAlign = System.Drawing.ContentAlignment.BottomCenter
    Me.ToolTip1.SetToolTip(Me.cmdCopyVCode, "Copy VCode into clipboard")
    '
    'cmdCopyGCode
    '
    Me.cmdCopyGCode.BackColor = System.Drawing.SystemColors.Control
    Me.cmdCopyGCode.Cursor = System.Windows.Forms.Cursors.Default
    Me.cmdCopyGCode.FlatStyle = System.Windows.Forms.FlatStyle.Popup
    Me.cmdCopyGCode.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.cmdCopyGCode.ForeColor = System.Drawing.SystemColors.ControlText
    Me.cmdCopyGCode.Location = New System.Drawing.Point(60, 100)
    Me.cmdCopyGCode.Name = "cmdCopyGCode"
    Me.cmdCopyGCode.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.cmdCopyGCode.Size = New System.Drawing.Size(23, 23)
    Me.cmdCopyGCode.TabIndex = 4
    Me.cmdCopyGCode.TextAlign = System.Drawing.ContentAlignment.BottomCenter
    Me.ToolTip1.SetToolTip(Me.cmdCopyGCode, "Copy GCode into clipboard")
    '
    'cmdCodeGen
    '
    Me.cmdCodeGen.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.cmdCodeGen.BackColor = System.Drawing.SystemColors.Control
    Me.cmdCodeGen.Cursor = System.Windows.Forms.Cursors.Default
    Me.cmdCodeGen.Enabled = False
    Me.cmdCodeGen.FlatStyle = System.Windows.Forms.FlatStyle.Popup
    Me.cmdCodeGen.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.cmdCodeGen.ForeColor = System.Drawing.SystemColors.ControlText
    Me.cmdCodeGen.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
    Me.cmdCodeGen.Location = New System.Drawing.Point(192, 48)
    Me.cmdCodeGen.Name = "cmdCodeGen"
    Me.cmdCodeGen.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.cmdCodeGen.Size = New System.Drawing.Size(78, 23)
    Me.cmdCodeGen.TabIndex = 4
    Me.cmdCodeGen.Text = "&Generate"
    Me.cmdCodeGen.TextAlign = System.Drawing.ContentAlignment.MiddleRight
    Me.ToolTip1.SetToolTip(Me.cmdCodeGen, "Generate new codes")
    '
    'txtName
    '
    Me.txtName.AcceptsReturn = True
    Me.txtName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtName.AutoSize = False
    Me.txtName.BackColor = System.Drawing.SystemColors.Window
    Me.txtName.Cursor = System.Windows.Forms.Cursors.IBeam
    Me.txtName.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtName.ForeColor = System.Drawing.SystemColors.WindowText
    Me.txtName.Location = New System.Drawing.Point(88, 24)
    Me.txtName.MaxLength = 0
    Me.txtName.Name = "txtName"
    Me.txtName.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.txtName.Size = New System.Drawing.Size(181, 21)
    Me.txtName.TabIndex = 1
    Me.txtName.Text = ""
    Me.ToolTip1.SetToolTip(Me.txtName, "Product Name")
    '
    'txtVer
    '
    Me.txtVer.AcceptsReturn = True
    Me.txtVer.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtVer.AutoSize = False
    Me.txtVer.BackColor = System.Drawing.SystemColors.Window
    Me.txtVer.Cursor = System.Windows.Forms.Cursors.IBeam
    Me.txtVer.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtVer.ForeColor = System.Drawing.SystemColors.WindowText
    Me.txtVer.Location = New System.Drawing.Point(88, 48)
    Me.txtVer.MaxLength = 0
    Me.txtVer.Name = "txtVer"
    Me.txtVer.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.txtVer.Size = New System.Drawing.Size(100, 21)
    Me.txtVer.TabIndex = 3
    Me.txtVer.Text = ""
    Me.ToolTip1.SetToolTip(Me.txtVer, "Product Version")
    '
    'cmdAdd
    '
    Me.cmdAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
    Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
    Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
    Me.cmdAdd.Enabled = False
    Me.cmdAdd.FlatStyle = System.Windows.Forms.FlatStyle.Popup
    Me.cmdAdd.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
    Me.cmdAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
    Me.cmdAdd.Location = New System.Drawing.Point(7, 210)
    Me.cmdAdd.Name = "cmdAdd"
    Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.cmdAdd.Size = New System.Drawing.Size(128, 21)
    Me.cmdAdd.TabIndex = 7
    Me.cmdAdd.Text = "&Add to product list"
    Me.cmdAdd.TextAlign = System.Drawing.ContentAlignment.MiddleRight
    Me.ToolTip1.SetToolTip(Me.cmdAdd, "Add to product list")
    '
    'cmdValidate
    '
    Me.cmdValidate.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.cmdValidate.BackColor = System.Drawing.SystemColors.Control
    Me.cmdValidate.Cursor = System.Windows.Forms.Cursors.Default
    Me.cmdValidate.FlatStyle = System.Windows.Forms.FlatStyle.Popup
    Me.cmdValidate.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.cmdValidate.ForeColor = System.Drawing.SystemColors.ControlText
    Me.cmdValidate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
    Me.cmdValidate.Location = New System.Drawing.Point(272, 48)
    Me.cmdValidate.Name = "cmdValidate"
    Me.cmdValidate.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.cmdValidate.Size = New System.Drawing.Size(78, 23)
    Me.cmdValidate.TabIndex = 5
    Me.cmdValidate.Text = "&Validate"
    Me.cmdValidate.TextAlign = System.Drawing.ContentAlignment.MiddleRight
    Me.ToolTip1.SetToolTip(Me.cmdValidate, "Validate product codes")
    '
    'cmdRemove
    '
    Me.cmdRemove.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
    Me.cmdRemove.BackColor = System.Drawing.SystemColors.Control
    Me.cmdRemove.Enabled = False
    Me.cmdRemove.FlatStyle = System.Windows.Forms.FlatStyle.Popup
    Me.cmdRemove.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.cmdRemove.ForeColor = System.Drawing.SystemColors.ControlText
    Me.cmdRemove.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
    Me.cmdRemove.Location = New System.Drawing.Point(139, 210)
    Me.cmdRemove.Name = "cmdRemove"
    Me.cmdRemove.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.cmdRemove.Size = New System.Drawing.Size(154, 21)
    Me.cmdRemove.TabIndex = 8
    Me.cmdRemove.Text = "&Remove from product list"
    Me.cmdRemove.TextAlign = System.Drawing.ContentAlignment.MiddleRight
    Me.ToolTip1.SetToolTip(Me.cmdRemove, "Remove from product list")
    '
    'picALBanner
    '
    Me.picALBanner.BackColor = System.Drawing.SystemColors.ActiveCaptionText
    Me.picALBanner.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
    Me.picALBanner.Cursor = System.Windows.Forms.Cursors.Hand
    Me.picALBanner.Location = New System.Drawing.Point(4, 128)
    Me.picALBanner.Name = "picALBanner"
    Me.picALBanner.Size = New System.Drawing.Size(74, 38)
    Me.picALBanner.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
    Me.picALBanner.TabIndex = 63
    Me.picALBanner.TabStop = False
    Me.ToolTip1.SetToolTip(Me.picALBanner, "www.activelocksoftware.com")
    '
    'cmdCopy
    '
    Me.cmdCopy.BackColor = System.Drawing.SystemColors.Control
    Me.cmdCopy.Cursor = System.Windows.Forms.Cursors.Default
    Me.cmdCopy.FlatStyle = System.Windows.Forms.FlatStyle.Popup
    Me.cmdCopy.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.cmdCopy.ForeColor = System.Drawing.SystemColors.ControlText
    Me.cmdCopy.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
    Me.cmdCopy.Location = New System.Drawing.Point(2, 348)
    Me.cmdCopy.Name = "cmdCopy"
    Me.cmdCopy.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.cmdCopy.Size = New System.Drawing.Size(80, 24)
    Me.cmdCopy.TabIndex = 28
    Me.cmdCopy.Text = "Copy  key"
    Me.cmdCopy.TextAlign = System.Drawing.ContentAlignment.MiddleRight
    Me.ToolTip1.SetToolTip(Me.cmdCopy, "Copy License Key into clipboard")
    '
    'cmdPaste
    '
    Me.cmdPaste.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.cmdPaste.BackColor = System.Drawing.SystemColors.Control
    Me.cmdPaste.Cursor = System.Windows.Forms.Cursors.Default
    Me.cmdPaste.FlatStyle = System.Windows.Forms.FlatStyle.Popup
    Me.cmdPaste.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.cmdPaste.ForeColor = System.Drawing.SystemColors.ControlText
    Me.cmdPaste.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
    Me.cmdPaste.Location = New System.Drawing.Point(479, 74)
    Me.cmdPaste.Name = "cmdPaste"
    Me.cmdPaste.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.cmdPaste.Size = New System.Drawing.Size(86, 24)
    Me.cmdPaste.TabIndex = 14
    Me.cmdPaste.Text = "Paste code"
    Me.cmdPaste.TextAlign = System.Drawing.ContentAlignment.MiddleRight
    Me.ToolTip1.SetToolTip(Me.cmdPaste, "Paste installation code from clipboard")
    '
    'txtUser
    '
    Me.txtUser.AcceptsReturn = True
    Me.txtUser.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtUser.AutoSize = False
    Me.txtUser.BackColor = System.Drawing.SystemColors.Window
    Me.txtUser.Cursor = System.Windows.Forms.Cursors.IBeam
    Me.txtUser.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtUser.ForeColor = System.Drawing.SystemColors.WindowText
    Me.txtUser.Location = New System.Drawing.Point(88, 100)
    Me.txtUser.MaxLength = 0
    Me.txtUser.Name = "txtUser"
    Me.txtUser.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.txtUser.Size = New System.Drawing.Size(475, 21)
    Me.txtUser.TabIndex = 16
    Me.txtUser.Text = ""
    Me.ToolTip1.SetToolTip(Me.txtUser, "Here will apear user name based on the instalation code")
    '
    'txtLicenseFile
    '
    Me.txtLicenseFile.AcceptsReturn = True
    Me.txtLicenseFile.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtLicenseFile.AutoSize = False
    Me.txtLicenseFile.BackColor = System.Drawing.SystemColors.Window
    Me.txtLicenseFile.Cursor = System.Windows.Forms.Cursors.IBeam
    Me.txtLicenseFile.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtLicenseFile.ForeColor = System.Drawing.SystemColors.WindowText
    Me.txtLicenseFile.Location = New System.Drawing.Point(86, 475)
    Me.txtLicenseFile.MaxLength = 0
    Me.txtLicenseFile.Name = "txtLicenseFile"
    Me.txtLicenseFile.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.txtLicenseFile.Size = New System.Drawing.Size(388, 21)
    Me.txtLicenseFile.TabIndex = 31
    Me.txtLicenseFile.Text = ""
    Me.ToolTip1.SetToolTip(Me.txtLicenseFile, "Enter or select license file")
    '
    'cboLicType
    '
    Me.cboLicType.BackColor = System.Drawing.SystemColors.Window
    Me.cboLicType.Cursor = System.Windows.Forms.Cursors.Default
    Me.cboLicType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
    Me.cboLicType.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.cboLicType.ForeColor = System.Drawing.SystemColors.WindowText
    Me.cboLicType.Items.AddRange(New Object() {"Time Locked", "Periodic", "Permanent"})
    Me.cboLicType.Location = New System.Drawing.Point(88, 28)
    Me.cboLicType.Name = "cboLicType"
    Me.cboLicType.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.cboLicType.Size = New System.Drawing.Size(234, 22)
    Me.cboLicType.TabIndex = 6
    Me.ToolTip1.SetToolTip(Me.cboLicType, "Select license type")
    '
    'txtInstallCode
    '
    Me.txtInstallCode.AcceptsReturn = True
    Me.txtInstallCode.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtInstallCode.AutoSize = False
    Me.txtInstallCode.BackColor = System.Drawing.SystemColors.Window
    Me.txtInstallCode.Cursor = System.Windows.Forms.Cursors.IBeam
    Me.txtInstallCode.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtInstallCode.ForeColor = System.Drawing.SystemColors.WindowText
    Me.txtInstallCode.Location = New System.Drawing.Point(88, 76)
    Me.txtInstallCode.MaxLength = 0
    Me.txtInstallCode.Name = "txtInstallCode"
    Me.txtInstallCode.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.txtInstallCode.Size = New System.Drawing.Size(389, 21)
    Me.txtInstallCode.TabIndex = 13
    Me.txtInstallCode.Text = ""
    Me.ToolTip1.SetToolTip(Me.txtInstallCode, "Enter here installation code")
    '
    'txtLicenseKey
    '
    Me.txtLicenseKey.AcceptsReturn = True
    Me.txtLicenseKey.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtLicenseKey.AutoSize = False
    Me.txtLicenseKey.BackColor = System.Drawing.SystemColors.Control
    Me.txtLicenseKey.Cursor = System.Windows.Forms.Cursors.IBeam
    Me.txtLicenseKey.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtLicenseKey.ForeColor = System.Drawing.Color.Blue
    Me.txtLicenseKey.Location = New System.Drawing.Point(86, 272)
    Me.txtLicenseKey.MaxLength = 0
    Me.txtLicenseKey.Multiline = True
    Me.txtLicenseKey.Name = "txtLicenseKey"
    Me.txtLicenseKey.ReadOnly = True
    Me.txtLicenseKey.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.txtLicenseKey.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
    Me.txtLicenseKey.Size = New System.Drawing.Size(476, 200)
    Me.txtLicenseKey.TabIndex = 27
    Me.txtLicenseKey.Text = "1234567890123456789012345678901234567890123456789012345678901234" & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10)
    Me.ToolTip1.SetToolTip(Me.txtLicenseKey, "License Key")
    '
    'cboRegisteredLevel
    '
    Me.cboRegisteredLevel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.cboRegisteredLevel.BackColor = System.Drawing.SystemColors.Window
    Me.cboRegisteredLevel.Cursor = System.Windows.Forms.Cursors.Default
    Me.cboRegisteredLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
    Me.cboRegisteredLevel.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.cboRegisteredLevel.ForeColor = System.Drawing.SystemColors.WindowText
    Me.cboRegisteredLevel.Location = New System.Drawing.Point(418, 4)
    Me.cboRegisteredLevel.Name = "cboRegisteredLevel"
    Me.cboRegisteredLevel.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.cboRegisteredLevel.Size = New System.Drawing.Size(124, 20)
    Me.cboRegisteredLevel.TabIndex = 3
    Me.ToolTip1.SetToolTip(Me.cboRegisteredLevel, "Select desired registration level")
    '
    'cboProducts
    '
    Me.cboProducts.BackColor = System.Drawing.SystemColors.Window
    Me.cboProducts.Cursor = System.Windows.Forms.Cursors.Default
    Me.cboProducts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
    Me.cboProducts.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.cboProducts.ForeColor = System.Drawing.SystemColors.WindowText
    Me.cboProducts.Location = New System.Drawing.Point(88, 4)
    Me.cboProducts.Name = "cboProducts"
    Me.cboProducts.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.cboProducts.Size = New System.Drawing.Size(234, 22)
    Me.cboProducts.TabIndex = 1
    Me.ToolTip1.SetToolTip(Me.cboProducts, "Select product from list")
    '
    'cmdViewLevel
    '
    Me.cmdViewLevel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.cmdViewLevel.BackColor = System.Drawing.SystemColors.Control
    Me.cmdViewLevel.Cursor = System.Windows.Forms.Cursors.Default
    Me.cmdViewLevel.FlatStyle = System.Windows.Forms.FlatStyle.Popup
    Me.cmdViewLevel.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.cmdViewLevel.ForeColor = System.Drawing.SystemColors.ControlText
    Me.cmdViewLevel.Location = New System.Drawing.Point(544, 4)
    Me.cmdViewLevel.Name = "cmdViewLevel"
    Me.cmdViewLevel.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.cmdViewLevel.Size = New System.Drawing.Size(22, 22)
    Me.cmdViewLevel.TabIndex = 4
    Me.cmdViewLevel.TextAlign = System.Drawing.ContentAlignment.BottomCenter
    Me.ToolTip1.SetToolTip(Me.cmdViewLevel, "Manage registered levels")
    '
    'cmdPrintLicenseKey
    '
    Me.cmdPrintLicenseKey.BackColor = System.Drawing.SystemColors.Control
    Me.cmdPrintLicenseKey.Cursor = System.Windows.Forms.Cursors.Default
    Me.cmdPrintLicenseKey.FlatStyle = System.Windows.Forms.FlatStyle.Popup
    Me.cmdPrintLicenseKey.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.cmdPrintLicenseKey.ForeColor = System.Drawing.SystemColors.ControlText
    Me.cmdPrintLicenseKey.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
    Me.cmdPrintLicenseKey.Location = New System.Drawing.Point(2, 374)
    Me.cmdPrintLicenseKey.Name = "cmdPrintLicenseKey"
    Me.cmdPrintLicenseKey.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.cmdPrintLicenseKey.Size = New System.Drawing.Size(80, 24)
    Me.cmdPrintLicenseKey.TabIndex = 64
    Me.cmdPrintLicenseKey.Text = "Print  key"
    Me.cmdPrintLicenseKey.TextAlign = System.Drawing.ContentAlignment.MiddleRight
    Me.ToolTip1.SetToolTip(Me.cmdPrintLicenseKey, "Print License Key")
    '
    'cmdEmailLicenseKey
    '
    Me.cmdEmailLicenseKey.BackColor = System.Drawing.SystemColors.Control
    Me.cmdEmailLicenseKey.Cursor = System.Windows.Forms.Cursors.Default
    Me.cmdEmailLicenseKey.FlatStyle = System.Windows.Forms.FlatStyle.Popup
    Me.cmdEmailLicenseKey.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.cmdEmailLicenseKey.ForeColor = System.Drawing.SystemColors.ControlText
    Me.cmdEmailLicenseKey.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
    Me.cmdEmailLicenseKey.Location = New System.Drawing.Point(2, 400)
    Me.cmdEmailLicenseKey.Name = "cmdEmailLicenseKey"
    Me.cmdEmailLicenseKey.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.cmdEmailLicenseKey.Size = New System.Drawing.Size(80, 24)
    Me.cmdEmailLicenseKey.TabIndex = 65
    Me.cmdEmailLicenseKey.Text = "Email  key"
    Me.cmdEmailLicenseKey.TextAlign = System.Drawing.ContentAlignment.MiddleRight
    Me.ToolTip1.SetToolTip(Me.cmdEmailLicenseKey, "Email License Key")
    '
    'SSTab1
    '
    Me.SSTab1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.SSTab1.Controls.Add(Me._SSTab1_TabPage0)
    Me.SSTab1.Controls.Add(Me._SSTab1_TabPage1)
    Me.SSTab1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.SSTab1.ItemSize = New System.Drawing.Size(42, 18)
    Me.SSTab1.Location = New System.Drawing.Point(0, 0)
    Me.SSTab1.Name = "SSTab1"
    Me.SSTab1.SelectedIndex = 1
    Me.SSTab1.Size = New System.Drawing.Size(580, 526)
    Me.SSTab1.TabIndex = 0
    '
    '_SSTab1_TabPage0
    '
    Me._SSTab1_TabPage0.Controls.Add(Me.grpProductsList)
    Me._SSTab1_TabPage0.Controls.Add(Me.fraProdNew)
    Me._SSTab1_TabPage0.Location = New System.Drawing.Point(4, 22)
    Me._SSTab1_TabPage0.Name = "_SSTab1_TabPage0"
    Me._SSTab1_TabPage0.Size = New System.Drawing.Size(572, 500)
    Me._SSTab1_TabPage0.TabIndex = 0
    Me._SSTab1_TabPage0.Text = "Product Code Generator"
    '
    'grpProductsList
    '
    Me.grpProductsList.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.grpProductsList.Controls.Add(Me.lstvwProducts)
    Me.grpProductsList.Location = New System.Drawing.Point(4, 240)
    Me.grpProductsList.Name = "grpProductsList"
    Me.grpProductsList.Size = New System.Drawing.Size(564, 258)
    Me.grpProductsList.TabIndex = 1
    Me.grpProductsList.TabStop = False
    Me.grpProductsList.Text = " Products list "
    '
    'fraProdNew
    '
    Me.fraProdNew.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.fraProdNew.BackColor = System.Drawing.SystemColors.Control
    Me.fraProdNew.Controls.Add(Me.picALBanner2)
    Me.fraProdNew.Controls.Add(Me.grpCodes)
    Me.fraProdNew.Controls.Add(Me.cmdCodeGen)
    Me.fraProdNew.Controls.Add(Me.txtName)
    Me.fraProdNew.Controls.Add(Me.txtVer)
    Me.fraProdNew.Controls.Add(Me.cmdAdd)
    Me.fraProdNew.Controls.Add(Me.Label2)
    Me.fraProdNew.Controls.Add(Me.Label3)
    Me.fraProdNew.Controls.Add(Me.cmdValidate)
    Me.fraProdNew.Controls.Add(Me.cmdRemove)
    Me.fraProdNew.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.fraProdNew.ForeColor = System.Drawing.SystemColors.ControlText
    Me.fraProdNew.Location = New System.Drawing.Point(2, 0)
    Me.fraProdNew.Name = "fraProdNew"
    Me.fraProdNew.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.fraProdNew.Size = New System.Drawing.Size(567, 236)
    Me.fraProdNew.TabIndex = 0
    Me.fraProdNew.TabStop = False
    Me.fraProdNew.Text = " Product details "
    '
    'grpCodes
    '
    Me.grpCodes.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.grpCodes.Controls.Add(Me.txtGCode)
    Me.grpCodes.Controls.Add(Me.txtVCode)
    Me.grpCodes.Controls.Add(Me.lblGCode)
    Me.grpCodes.Controls.Add(Me.lblVCode)
    Me.grpCodes.Controls.Add(Me.cmdCopyVCode)
    Me.grpCodes.Controls.Add(Me.cmdCopyGCode)
    Me.grpCodes.Location = New System.Drawing.Point(4, 75)
    Me.grpCodes.Name = "grpCodes"
    Me.grpCodes.Size = New System.Drawing.Size(556, 129)
    Me.grpCodes.TabIndex = 6
    Me.grpCodes.TabStop = False
    Me.grpCodes.Text = " Codes "
    '
    'txtGCode
    '
    Me.txtGCode.AcceptsReturn = True
    Me.txtGCode.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtGCode.AutoSize = False
    Me.txtGCode.BackColor = System.Drawing.SystemColors.Control
    Me.txtGCode.Cursor = System.Windows.Forms.Cursors.IBeam
    Me.txtGCode.ForeColor = System.Drawing.SystemColors.WindowText
    Me.txtGCode.Location = New System.Drawing.Point(84, 74)
    Me.txtGCode.MaxLength = 0
    Me.txtGCode.Multiline = True
    Me.txtGCode.Name = "txtGCode"
    Me.txtGCode.ReadOnly = True
    Me.txtGCode.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.txtGCode.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
    Me.txtGCode.Size = New System.Drawing.Size(466, 50)
    Me.txtGCode.TabIndex = 5
    Me.txtGCode.Text = ""
    '
    'lblGCode
    '
    Me.lblGCode.BackColor = System.Drawing.SystemColors.Control
    Me.lblGCode.Cursor = System.Windows.Forms.Cursors.Default
    Me.lblGCode.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblGCode.ForeColor = System.Drawing.SystemColors.ControlText
    Me.lblGCode.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
    Me.lblGCode.Location = New System.Drawing.Point(6, 74)
    Me.lblGCode.Name = "lblGCode"
    Me.lblGCode.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.lblGCode.Size = New System.Drawing.Size(78, 26)
    Me.lblGCode.TabIndex = 3
    Me.lblGCode.Text = "GCode (PRV_KEY)"
    Me.lblGCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight
    '
    'lblVCode
    '
    Me.lblVCode.BackColor = System.Drawing.SystemColors.Control
    Me.lblVCode.Cursor = System.Windows.Forms.Cursors.Default
    Me.lblVCode.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblVCode.ForeColor = System.Drawing.SystemColors.ControlText
    Me.lblVCode.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
    Me.lblVCode.Location = New System.Drawing.Point(6, 20)
    Me.lblVCode.Name = "lblVCode"
    Me.lblVCode.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.lblVCode.Size = New System.Drawing.Size(78, 28)
    Me.lblVCode.TabIndex = 0
    Me.lblVCode.Text = "VCode (PUB_KEY)"
    Me.lblVCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight
    '
    'Label2
    '
    Me.Label2.BackColor = System.Drawing.SystemColors.Control
    Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
    Me.Label2.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
    Me.Label2.Location = New System.Drawing.Point(8, 24)
    Me.Label2.Name = "Label2"
    Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.Label2.Size = New System.Drawing.Size(65, 18)
    Me.Label2.TabIndex = 0
    Me.Label2.Text = "&Name:"
    '
    'Label3
    '
    Me.Label3.BackColor = System.Drawing.SystemColors.Control
    Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
    Me.Label3.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
    Me.Label3.Location = New System.Drawing.Point(8, 48)
    Me.Label3.Name = "Label3"
    Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.Label3.Size = New System.Drawing.Size(73, 18)
    Me.Label3.TabIndex = 2
    Me.Label3.Text = "V&ersion:"
    '
    '_SSTab1_TabPage1
    '
    Me._SSTab1_TabPage1.Controls.Add(Me.frmKeyGen)
    Me._SSTab1_TabPage1.Location = New System.Drawing.Point(4, 22)
    Me._SSTab1_TabPage1.Name = "_SSTab1_TabPage1"
    Me._SSTab1_TabPage1.Size = New System.Drawing.Size(572, 500)
    Me._SSTab1_TabPage1.TabIndex = 1
    Me._SSTab1_TabPage1.Text = "License Key Generator"
    '
    'frmKeyGen
    '
    Me.frmKeyGen.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.frmKeyGen.BackColor = System.Drawing.SystemColors.Control
    Me.frmKeyGen.Controls.Add(Me.cmdEmailLicenseKey)
    Me.frmKeyGen.Controls.Add(Me.cmdPrintLicenseKey)
    Me.frmKeyGen.Controls.Add(Me.dtpExpireDate)
    Me.frmKeyGen.Controls.Add(Me.picALBanner)
    Me.frmKeyGen.Controls.Add(Me.chkLockIP)
    Me.frmKeyGen.Controls.Add(Me.chkLockMotherboard)
    Me.frmKeyGen.Controls.Add(Me.chkLockBIOS)
    Me.frmKeyGen.Controls.Add(Me.chkLockWindows)
    Me.frmKeyGen.Controls.Add(Me.chkLockHDfirmware)
    Me.frmKeyGen.Controls.Add(Me.chkLockHD)
    Me.frmKeyGen.Controls.Add(Me.chkLockComputer)
    Me.frmKeyGen.Controls.Add(Me.chkLockMACaddress)
    Me.frmKeyGen.Controls.Add(Me.chkItemData)
    Me.frmKeyGen.Controls.Add(Me.cmdCopy)
    Me.frmKeyGen.Controls.Add(Me.cmdPaste)
    Me.frmKeyGen.Controls.Add(Me.txtUser)
    Me.frmKeyGen.Controls.Add(Me.cmdBrowse)
    Me.frmKeyGen.Controls.Add(Me.txtLicenseFile)
    Me.frmKeyGen.Controls.Add(Me.cmdSave)
    Me.frmKeyGen.Controls.Add(Me.cboLicType)
    Me.frmKeyGen.Controls.Add(Me.txtDays)
    Me.frmKeyGen.Controls.Add(Me.txtInstallCode)
    Me.frmKeyGen.Controls.Add(Me.txtLicenseKey)
    Me.frmKeyGen.Controls.Add(Me.cmdKeyGen)
    Me.frmKeyGen.Controls.Add(Me.Label11)
    Me.frmKeyGen.Controls.Add(Me.Label5)
    Me.frmKeyGen.Controls.Add(Me.lblExpiry)
    Me.frmKeyGen.Controls.Add(Me.Label6)
    Me.frmKeyGen.Controls.Add(Me.Label7)
    Me.frmKeyGen.Controls.Add(Me.lblLicenseKey)
    Me.frmKeyGen.Controls.Add(Me.lblDays)
    Me.frmKeyGen.Controls.Add(Me.cmdViewArchive)
    Me.frmKeyGen.Controls.Add(Me.Label8)
    Me.frmKeyGen.Controls.Add(Me.Label15)
    Me.frmKeyGen.Controls.Add(Me.cboRegisteredLevel)
    Me.frmKeyGen.Controls.Add(Me.cboProducts)
    Me.frmKeyGen.Controls.Add(Me.cmdViewLevel)
    Me.frmKeyGen.Cursor = System.Windows.Forms.Cursors.Default
    Me.frmKeyGen.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.frmKeyGen.ForeColor = System.Drawing.SystemColors.ControlText
    Me.frmKeyGen.Location = New System.Drawing.Point(2, 0)
    Me.frmKeyGen.Name = "frmKeyGen"
    Me.frmKeyGen.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.frmKeyGen.Size = New System.Drawing.Size(567, 498)
    Me.frmKeyGen.TabIndex = 0
    '
    'dtpExpireDate
    '
    Me.dtpExpireDate.CustomFormat = "yyyy/MM/dd"
    Me.dtpExpireDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
    Me.dtpExpireDate.Location = New System.Drawing.Point(88, 52)
    Me.dtpExpireDate.Name = "dtpExpireDate"
    Me.dtpExpireDate.Size = New System.Drawing.Size(118, 20)
    Me.dtpExpireDate.TabIndex = 10
    Me.dtpExpireDate.Value = New Date(2006, 5, 25, 20, 28, 52, 803)
    Me.dtpExpireDate.Visible = False
    '
    'chkLockIP
    '
    Me.chkLockIP.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.chkLockIP.BackColor = System.Drawing.SystemColors.Control
    Me.chkLockIP.Cursor = System.Windows.Forms.Cursors.Default
    Me.chkLockIP.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.chkLockIP.ForeColor = System.Drawing.SystemColors.ControlText
    Me.chkLockIP.Location = New System.Drawing.Point(87, 254)
    Me.chkLockIP.Name = "chkLockIP"
    Me.chkLockIP.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.chkLockIP.Size = New System.Drawing.Size(476, 13)
    Me.chkLockIP.TabIndex = 24
    Me.chkLockIP.Text = "Lock to IP Address"
    '
    'chkLockMotherboard
    '
    Me.chkLockMotherboard.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.chkLockMotherboard.BackColor = System.Drawing.SystemColors.Control
    Me.chkLockMotherboard.Cursor = System.Windows.Forms.Cursors.Default
    Me.chkLockMotherboard.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.chkLockMotherboard.ForeColor = System.Drawing.SystemColors.ControlText
    Me.chkLockMotherboard.Location = New System.Drawing.Point(87, 236)
    Me.chkLockMotherboard.Name = "chkLockMotherboard"
    Me.chkLockMotherboard.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.chkLockMotherboard.Size = New System.Drawing.Size(476, 13)
    Me.chkLockMotherboard.TabIndex = 23
    Me.chkLockMotherboard.Text = "Lock to Motherboard Serial"
    '
    'chkLockBIOS
    '
    Me.chkLockBIOS.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.chkLockBIOS.BackColor = System.Drawing.SystemColors.Control
    Me.chkLockBIOS.Cursor = System.Windows.Forms.Cursors.Default
    Me.chkLockBIOS.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.chkLockBIOS.ForeColor = System.Drawing.SystemColors.ControlText
    Me.chkLockBIOS.Location = New System.Drawing.Point(87, 218)
    Me.chkLockBIOS.Name = "chkLockBIOS"
    Me.chkLockBIOS.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.chkLockBIOS.Size = New System.Drawing.Size(476, 13)
    Me.chkLockBIOS.TabIndex = 22
    Me.chkLockBIOS.Text = "Lock to BIOS Version"
    '
    'chkLockWindows
    '
    Me.chkLockWindows.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.chkLockWindows.BackColor = System.Drawing.SystemColors.Control
    Me.chkLockWindows.Cursor = System.Windows.Forms.Cursors.Default
    Me.chkLockWindows.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.chkLockWindows.ForeColor = System.Drawing.SystemColors.ControlText
    Me.chkLockWindows.Location = New System.Drawing.Point(87, 200)
    Me.chkLockWindows.Name = "chkLockWindows"
    Me.chkLockWindows.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.chkLockWindows.Size = New System.Drawing.Size(476, 13)
    Me.chkLockWindows.TabIndex = 21
    Me.chkLockWindows.Text = "Lock to Windows Serial"
    '
    'chkLockHDfirmware
    '
    Me.chkLockHDfirmware.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.chkLockHDfirmware.BackColor = System.Drawing.SystemColors.Control
    Me.chkLockHDfirmware.Cursor = System.Windows.Forms.Cursors.Default
    Me.chkLockHDfirmware.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.chkLockHDfirmware.ForeColor = System.Drawing.SystemColors.ControlText
    Me.chkLockHDfirmware.Location = New System.Drawing.Point(87, 182)
    Me.chkLockHDfirmware.Name = "chkLockHDfirmware"
    Me.chkLockHDfirmware.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.chkLockHDfirmware.Size = New System.Drawing.Size(476, 13)
    Me.chkLockHDfirmware.TabIndex = 20
    Me.chkLockHDfirmware.Text = "Lock to HDD Firmware Serial"
    '
    'chkLockHD
    '
    Me.chkLockHD.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.chkLockHD.BackColor = System.Drawing.SystemColors.Control
    Me.chkLockHD.Cursor = System.Windows.Forms.Cursors.Default
    Me.chkLockHD.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.chkLockHD.ForeColor = System.Drawing.SystemColors.ControlText
    Me.chkLockHD.Location = New System.Drawing.Point(87, 164)
    Me.chkLockHD.Name = "chkLockHD"
    Me.chkLockHD.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.chkLockHD.Size = New System.Drawing.Size(476, 13)
    Me.chkLockHD.TabIndex = 19
    Me.chkLockHD.Text = "Lock to HDD Volume Serial"
    '
    'chkLockComputer
    '
    Me.chkLockComputer.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.chkLockComputer.BackColor = System.Drawing.SystemColors.Control
    Me.chkLockComputer.Cursor = System.Windows.Forms.Cursors.Default
    Me.chkLockComputer.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.chkLockComputer.ForeColor = System.Drawing.SystemColors.ControlText
    Me.chkLockComputer.Location = New System.Drawing.Point(87, 146)
    Me.chkLockComputer.Name = "chkLockComputer"
    Me.chkLockComputer.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.chkLockComputer.Size = New System.Drawing.Size(476, 13)
    Me.chkLockComputer.TabIndex = 18
    Me.chkLockComputer.Text = "Lock to Computer Name"
    '
    'chkLockMACaddress
    '
    Me.chkLockMACaddress.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.chkLockMACaddress.BackColor = System.Drawing.SystemColors.Control
    Me.chkLockMACaddress.Cursor = System.Windows.Forms.Cursors.Default
    Me.chkLockMACaddress.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.chkLockMACaddress.ForeColor = System.Drawing.SystemColors.ControlText
    Me.chkLockMACaddress.Location = New System.Drawing.Point(87, 128)
    Me.chkLockMACaddress.Name = "chkLockMACaddress"
    Me.chkLockMACaddress.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.chkLockMACaddress.Size = New System.Drawing.Size(476, 13)
    Me.chkLockMACaddress.TabIndex = 17
    Me.chkLockMACaddress.Text = "Lock to MAC Address"
    '
    'chkItemData
    '
    Me.chkItemData.BackColor = System.Drawing.SystemColors.Control
    Me.chkItemData.Cursor = System.Windows.Forms.Cursors.Default
    Me.chkItemData.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.chkItemData.ForeColor = System.Drawing.SystemColors.ControlText
    Me.chkItemData.Location = New System.Drawing.Point(418, 28)
    Me.chkItemData.Name = "chkItemData"
    Me.chkItemData.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.chkItemData.Size = New System.Drawing.Size(146, 22)
    Me.chkItemData.TabIndex = 7
    Me.chkItemData.Text = "Use Item Data for Code"
    '
    'txtDays
    '
    Me.txtDays.AcceptsReturn = True
    Me.txtDays.AutoSize = False
    Me.txtDays.BackColor = System.Drawing.SystemColors.Control
    Me.txtDays.Cursor = System.Windows.Forms.Cursors.IBeam
    Me.txtDays.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtDays.ForeColor = System.Drawing.SystemColors.WindowText
    Me.txtDays.Location = New System.Drawing.Point(88, 52)
    Me.txtDays.MaxLength = 0
    Me.txtDays.Name = "txtDays"
    Me.txtDays.ReadOnly = True
    Me.txtDays.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.txtDays.Size = New System.Drawing.Size(117, 21)
    Me.txtDays.TabIndex = 9
    Me.txtDays.Text = "30"
    Me.txtDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
    '
    'Label11
    '
    Me.Label11.BackColor = System.Drawing.SystemColors.Control
    Me.Label11.Cursor = System.Windows.Forms.Cursors.Default
    Me.Label11.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.Label11.ForeColor = System.Drawing.SystemColors.ControlText
    Me.Label11.Location = New System.Drawing.Point(2, 102)
    Me.Label11.Name = "Label11"
    Me.Label11.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.Label11.Size = New System.Drawing.Size(89, 17)
    Me.Label11.TabIndex = 15
    Me.Label11.Text = "User Name:"
    '
    'Label5
    '
    Me.Label5.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
    Me.Label5.BackColor = System.Drawing.SystemColors.Control
    Me.Label5.Cursor = System.Windows.Forms.Cursors.Default
    Me.Label5.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.Label5.ForeColor = System.Drawing.SystemColors.ControlText
    Me.Label5.Location = New System.Drawing.Point(2, 477)
    Me.Label5.Name = "Label5"
    Me.Label5.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.Label5.Size = New System.Drawing.Size(78, 17)
    Me.Label5.TabIndex = 30
    Me.Label5.Text = "License &file:"
    '
    'lblExpiry
    '
    Me.lblExpiry.BackColor = System.Drawing.SystemColors.Control
    Me.lblExpiry.Cursor = System.Windows.Forms.Cursors.Default
    Me.lblExpiry.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblExpiry.ForeColor = System.Drawing.SystemColors.ControlText
    Me.lblExpiry.Location = New System.Drawing.Point(2, 54)
    Me.lblExpiry.Name = "lblExpiry"
    Me.lblExpiry.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.lblExpiry.Size = New System.Drawing.Size(89, 17)
    Me.lblExpiry.TabIndex = 8
    Me.lblExpiry.Text = "&Expires After:"
    '
    'Label6
    '
    Me.Label6.BackColor = System.Drawing.SystemColors.Control
    Me.Label6.Cursor = System.Windows.Forms.Cursors.Default
    Me.Label6.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.Label6.ForeColor = System.Drawing.SystemColors.ControlText
    Me.Label6.Location = New System.Drawing.Point(2, 30)
    Me.Label6.Name = "Label6"
    Me.Label6.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.Label6.Size = New System.Drawing.Size(89, 17)
    Me.Label6.TabIndex = 5
    Me.Label6.Text = "License &Type:"
    '
    'Label7
    '
    Me.Label7.BackColor = System.Drawing.SystemColors.Control
    Me.Label7.Cursor = System.Windows.Forms.Cursors.Default
    Me.Label7.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.Label7.ForeColor = System.Drawing.SystemColors.ControlText
    Me.Label7.Location = New System.Drawing.Point(2, 78)
    Me.Label7.Name = "Label7"
    Me.Label7.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.Label7.Size = New System.Drawing.Size(89, 17)
    Me.Label7.TabIndex = 12
    Me.Label7.Text = "Installation C&ode:"
    '
    'lblLicenseKey
    '
    Me.lblLicenseKey.BackColor = System.Drawing.SystemColors.Control
    Me.lblLicenseKey.Cursor = System.Windows.Forms.Cursors.Default
    Me.lblLicenseKey.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblLicenseKey.ForeColor = System.Drawing.SystemColors.ControlText
    Me.lblLicenseKey.ImageAlign = System.Drawing.ContentAlignment.BottomRight
    Me.lblLicenseKey.Location = New System.Drawing.Point(2, 274)
    Me.lblLicenseKey.Name = "lblLicenseKey"
    Me.lblLicenseKey.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.lblLicenseKey.Size = New System.Drawing.Size(82, 44)
    Me.lblLicenseKey.TabIndex = 25
    Me.lblLicenseKey.Text = "License &Key:"
    '
    'lblDays
    '
    Me.lblDays.BackColor = System.Drawing.SystemColors.Control
    Me.lblDays.Cursor = System.Windows.Forms.Cursors.Default
    Me.lblDays.ForeColor = System.Drawing.SystemColors.ControlText
    Me.lblDays.Location = New System.Drawing.Point(208, 56)
    Me.lblDays.Name = "lblDays"
    Me.lblDays.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.lblDays.Size = New System.Drawing.Size(182, 17)
    Me.lblDays.TabIndex = 11
    Me.lblDays.Text = "days"
    '
    'Label8
    '
    Me.Label8.BackColor = System.Drawing.SystemColors.Control
    Me.Label8.Cursor = System.Windows.Forms.Cursors.Default
    Me.Label8.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.Label8.ForeColor = System.Drawing.SystemColors.ControlText
    Me.Label8.Location = New System.Drawing.Point(2, 6)
    Me.Label8.Name = "Label8"
    Me.Label8.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.Label8.Size = New System.Drawing.Size(65, 17)
    Me.Label8.TabIndex = 0
    Me.Label8.Text = "&Product:"
    '
    'Label15
    '
    Me.Label15.BackColor = System.Drawing.SystemColors.Control
    Me.Label15.Cursor = System.Windows.Forms.Cursors.Default
    Me.Label15.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.Label15.ForeColor = System.Drawing.SystemColors.ControlText
    Me.Label15.Location = New System.Drawing.Point(326, 6)
    Me.Label15.Name = "Label15"
    Me.Label15.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.Label15.Size = New System.Drawing.Size(90, 17)
    Me.Label15.TabIndex = 2
    Me.Label15.Text = "Registered Level:"
    '
    'sbStatus
    '
    Me.sbStatus.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.sbStatus.Location = New System.Drawing.Point(0, 527)
    Me.sbStatus.Name = "sbStatus"
    Me.sbStatus.Panels.AddRange(New System.Windows.Forms.StatusBarPanel() {Me.mainStatusBarPanel})
    Me.sbStatus.ShowPanels = True
    Me.sbStatus.Size = New System.Drawing.Size(580, 22)
    Me.sbStatus.TabIndex = 1
    Me.sbStatus.Text = "Ready ..."
    '
    'mainStatusBarPanel
    '
    Me.mainStatusBarPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring
    Me.mainStatusBarPanel.Text = "Ready ..."
    Me.mainStatusBarPanel.Width = 564
    '
    'lnkActivelockSoftwareGroup
    '
    Me.lnkActivelockSoftwareGroup.ActiveLinkColor = System.Drawing.Color.Green
    Me.lnkActivelockSoftwareGroup.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lnkActivelockSoftwareGroup.Location = New System.Drawing.Point(416, 4)
    Me.lnkActivelockSoftwareGroup.Name = "lnkActivelockSoftwareGroup"
    Me.lnkActivelockSoftwareGroup.Size = New System.Drawing.Size(156, 12)
    Me.lnkActivelockSoftwareGroup.TabIndex = 66
    Me.lnkActivelockSoftwareGroup.TabStop = True
    Me.lnkActivelockSoftwareGroup.Text = "www.activelocksoftware.com"
    Me.lnkActivelockSoftwareGroup.TextAlign = System.Drawing.ContentAlignment.MiddleRight
    Me.lnkActivelockSoftwareGroup.VisitedLinkColor = System.Drawing.Color.Blue
    '
    'frmMain
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
    Me.BackColor = System.Drawing.SystemColors.Control
    Me.ClientSize = New System.Drawing.Size(580, 549)
    Me.Controls.Add(Me.lnkActivelockSoftwareGroup)
    Me.Controls.Add(Me.sbStatus)
    Me.Controls.Add(Me.SSTab1)
    Me.Cursor = System.Windows.Forms.Cursors.Default
    Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.Location = New System.Drawing.Point(3, 22)
    Me.MinimumSize = New System.Drawing.Size(588, 576)
    Me.Name = "frmMain"
    Me.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
    Me.Text = "ALUGEN - ActiveLock3 Universal GENerator"
    Me.SSTab1.ResumeLayout(False)
    Me._SSTab1_TabPage0.ResumeLayout(False)
    Me.grpProductsList.ResumeLayout(False)
    Me.fraProdNew.ResumeLayout(False)
    Me.grpCodes.ResumeLayout(False)
    Me._SSTab1_TabPage1.ResumeLayout(False)
    Me.frmKeyGen.ResumeLayout(False)
    CType(Me.mainStatusBarPanel, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub
#End Region


#Region "Methods"

  Private Sub LoadResources()
    Dim resxReader As Resources.ResXResourceReader = New Resources.ResXResourceReader("Alugen3NET.resx")
    Dim resxDE As IDictionaryEnumerator = resxReader.GetEnumerator
    For Each dd As DictionaryEntry In resxReader
      resxList.Add(dd.Key, dd.Value)
    Next
  End Sub

  Private Sub LoadImages()
    'load buttons images
    cmdAdd.Image = CType(resxList("add.gif"), Image)
    cmdRemove.Image = CType(resxList("delete.gif"), Image)
    cmdCodeGen.Image = CType(resxList("generate.gif"), Image)
    cmdKeyGen.Image = CType(resxList("generate.gif"), Image)
    cmdValidate.Image = CType(resxList("validate.gif"), Image)
    cmdCopyVCode.Image = CType(resxList("copy.gif"), Image)
    cmdCopyGCode.Image = CType(resxList("copy.gif"), Image)
    cmdCopy.Image = CType(resxList("copy.gif"), Image)
    cmdViewLevel.Image = CType(resxList("preview.gif"), Image)
    cmdPaste.Image = CType(resxList("paste.gif"), Image)
    cmdSave.Image = CType(resxList("save.gif"), Image)
    cmdViewArchive.Image = CType(resxList("viewdatabase.gif"), Image)
    cmdBrowse.Image = CType(resxList("folder.gif"), Image)
    cmdPrintLicenseKey.Image = CType(resxList("print.gif"), Image)
    cmdEmailLicenseKey.Image = CType(resxList("email.gif"), Image)
    'lbl's
    lblVCode.Image = CType(resxList("keys.gif"), Image)
    lblGCode.Image = CType(resxList("keys.gif"), Image)
    lblLicenseKey.Image = CType(resxList("KeyLock.gif"), Image)
    'AL banners
    picALBanner.Image = CType(resxList("I_Trust_AL_small.gif"), Image)
    picALBanner2.Image = CType(resxList("I_Trust_AL_small.gif"), Image)
  End Sub

  Private Sub AppendLockString(ByRef strLock As String, ByVal newSubString As String)
    '===============================================================================
    ' Name: Sub AppendLockString
    ' Input:
    '   ByRef strLock As String - The lock string to be appended to, returns as an output
    '   ByVal newSubString As String - The string to be appended to the lock string if strLock is empty string
    ' Output:
    '   Appended lock string and installation code
    ' Purpose: Appends the lock string to the given installation code
    ' Remarks: None
    '===============================================================================

    If strLock = "" Then
      strLock = newSubString
    Else
      strLock = strLock & vbLf & newSubString
    End If
  End Sub

  Private Function ReconstructedInstallationCode() As String
    Dim strLock, strReq As String
    Dim noKey As String
    noKey = Chr(110) & Chr(111) & Chr(107) & Chr(101) & Chr(121)

    If Me.chkLockMACaddress.CheckState = CheckState.Checked Then
      AppendLockString(strLock, MACaddress)
    Else
      AppendLockString(strLock, noKey)
    End If
    If Me.chkLockComputer.CheckState = CheckState.Checked Then
      AppendLockString(strLock, ComputerName)
    Else
      AppendLockString(strLock, noKey)
    End If
    If Me.chkLockHD.CheckState = CheckState.Checked Then
      AppendLockString(strLock, VolumeSerial)
    Else
      AppendLockString(strLock, noKey)
    End If
    If Me.chkLockHDfirmware.CheckState = CheckState.Checked Then
      AppendLockString(strLock, FirmwareSerial)
    Else
      AppendLockString(strLock, noKey)
    End If
    If Me.chkLockWindows.CheckState = CheckState.Checked Then
      AppendLockString(strLock, WindowsSerial)
    Else
      AppendLockString(strLock, noKey)
    End If
    If Me.chkLockBIOS.CheckState = CheckState.Checked Then
      AppendLockString(strLock, BIOSserial)
    Else
      AppendLockString(strLock, noKey)
    End If
    If Me.chkLockMotherboard.CheckState = CheckState.Checked Then
      AppendLockString(strLock, MotherboardSerial)
    Else
      AppendLockString(strLock, noKey)
    End If
    If Me.chkLockIP.CheckState = CheckState.Checked Then
      AppendLockString(strLock, IPaddress)
    Else
      AppendLockString(strLock, noKey)
    End If

    If Not strLock Is Nothing _
      AndAlso strLock.Substring(0, 1) = vbLf Then
      strLock = strLock.Substring(2)
    End If

    Dim Index, i As Short
    Dim strInstCode As String
    strInstCode = ActiveLock3Globals_definst.Base64Decode(txtInstallCode.Text)

    If strInstCode Is Nothing Then Exit Function

    If Not strInstCode Is Nothing _
      AndAlso strInstCode.Substring(0, 1) = "+" Then
      strInstCode = strInstCode.Substring(2)
    End If
    Index = 0
    i = 1
    ' Get to the last vbLf, which denotes the ending of the lock code and beginning of user name.
    Do While i > 0
      i = strInstCode.IndexOf(vbLf, Index) 'InStr(Index + 1, strInstCode, vbLf)
      If i > 0 Then Index = i
    Loop
    ' user name starts from Index+1 to the end
    Dim user As String
    user = strInstCode.Substring(Index + 1)

    ' combine with user name
    strReq = strLock & vbLf & user

    ' base-64 encode the request
    Dim strReq2 As String
    strReq2 = modBase64.Base64_Encode("+" & strReq)
    ReconstructedInstallationCode = strReq2

  End Function

  Private Sub UpdateKeyGenButtonStatus()
    If txtInstallCode.Text = "" Then
      cmdKeyGen.Enabled = False
    Else
      If cboProducts.SelectedIndex >= 0 Then
        cmdKeyGen.Enabled = True
      End If
    End If
  End Sub

  Private Function Make64ByteChunks(ByRef strdata As String) As String
    ' Breaks a long string into chunks of 64-byte lines.
    Dim i As Integer
    Dim Count As Integer
    Dim strNew64Chunk As String
    Dim sResult As String = ""

    Count = strdata.Length
    For i = 0 To Count Step 64
      If i + 64 > Count Then
        strNew64Chunk = strdata.Substring(i)

      Else
        strNew64Chunk = strdata.Substring(i, 64)
      End If
      sResult = sResult & IIf(sResult.Length > 0, vbCrLf, "") & strNew64Chunk
    Next

    Make64ByteChunks = sResult
  End Function

  Private Function GetExpirationDate() As String
    If cboLicType.Text = "Time Locked" Then
      'GetExpirationDate = txtDays.Text
      GetExpirationDate = CType(dtpExpireDate.Value, DateTime).ToString("yyyy/MM/dd")
    Else
      GetExpirationDate = Now.UtcNow.AddDays(CShort(txtDays.Text)).ToString("yyyy/MM/dd")
    End If
  End Function

  Private Sub SaveLicenseKey(ByVal sLibKey As String, ByVal sFileName As String)
    Dim hFile As Integer
    hFile = FreeFile()
    FileOpen(hFile, sFileName, OpenMode.Output)
    PrintLine(hFile, sLibKey)
    FileClose(hFile)
  End Sub


  Private Sub UpdateStatus(ByRef Msg As String)
    'write status on fist status bar panel
    sbStatus.Panels(0).Text = Msg
  End Sub


  Function CheckForResources(ByVal ParamArray MyArray() As Object) As Boolean
    'MyArray is a list of things to check
    'These can be DLLs or OCXs

    'Files, by default, are searched for in the Windows System Directory
    'Exceptions;
    '   Begins with a # means it should be in the same directory with the executable
    '   Contains the full path (anything with a "\")

    'Typical names would be "#aaa.dll", "mydll.dll", "myocx.ocx", "comdlg32.ocx", "mscomctl.ocx", "msflxgrd.ocx"

    'If the file has no extension, we;
    '     assume it's a DLL, and if it still can't be found
    '     assume it's an OCX

    Try

      Dim foundIt As Boolean
      Dim Y As Object
      Dim i, j As Short
      Dim systemDir, s, pathName As String

      WhereIsDLL("") 'initialize

      systemDir = WinSysDir() 'Get the Windows system directory
      For Each Y In MyArray
        foundIt = False
        s = CStr(Y)

        If s.Substring(0, 1) = "#" Then
          pathName = Application.StartupPath
          s = s.Substring(2)
        ElseIf s.IndexOf("\") > 0 Then
          j = s.LastIndexOf("\") 'InStrRev(s, "\")
          pathName = s.Substring(s, j - 1)
          s = s.Substring(j + 1)
        Else
          pathName = systemDir
        End If

        If s.IndexOf(".") > 0 Then
          If File.Exists(pathName & "\" & s) Then foundIt = True
        ElseIf File.Exists(pathName & "\" & s & ".DLL") Then
          foundIt = True
        ElseIf File.Exists(pathName & "\" & s & ".OCX") Then
          foundIt = True
          s = s & ".OCX" 'this will make the softlocx check easier
        End If

        If Not foundIt Then
          MsgBox(s & " could not be found in " & pathName & vbCrLf & System.Reflection.Assembly.GetExecutingAssembly.GetName.Name & " cannot run without this library file!" & vbCrLf & vbCrLf & "Exiting!", MsgBoxStyle.Critical, "Missing Resource")
          End
        End If
      Next Y

      CheckForResources = True
    Catch ex As Exception
      MessageBox.Show(ex.Message, ACTIVELOCKSTRING, MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Try


  End Function

  Function WhereIsDLL(ByVal T As String) As String
    'Places where programs look for DLLs
    '   1 directory containing the EXE
    '   2 current directory
    '   3 32 bit system directory   possibly \Windows\system32
    '   4 16 bit system directory   possibly \Windows\system
    '   5 windows directory         possibly \Windows
    '   6 path

    'The current directory may be changed in the course of the program
    'but current directory -- when the program starts -- is what matters
    'so a call should be made to this function early on to "lock" the paths.

    'Add a call at the beginning of checkForResources

    Static a As Object
    Dim s, D As String
    Dim EnvString As String
    Dim Indx As Short ' Declare variables.
    Dim i As Short

    On Error Resume Next
    i = UBound(a)
    If i = 0 Then
      s = Environment.GetEnvironmentVariable("PATH") & ";" & CurDir() & ";"

      D = WinSysDir()
      s = s & D & ";"

      If D.Substring(D.Length - 2) = "32" Then 'I'm guessing at the name of the 16 bit windows directory (assuming it exists)
        i = D.Length
        s = s & D.Substring(D, i - 2) & ";"
      End If

      s = s & WinDir() & ";"
      Indx = 1 ' Initialize index to 1.
      Do
        EnvString = Environ(Indx) ' Get environment variable.
        If StrComp(EnvString.Substring(0, 5), "PATH=", CompareMethod.Text) = 0 Then ' Check PATH entry.
          s = s & EnvString.Substring(6)
          Exit Do
        End If
        Indx = Indx + 1
      Loop Until EnvString = ""
      a = Split(s, ";")
    End If

    T = Trim(T)
    If T = "" Then Exit Function
    If Not T.Substring(T.Length - 4).IndexOf(".") > 0 Then T = T & ".DLL" 'default extension
    For i = 0 To UBound(a)
      If File.Exists(a(i) & "\" & T) Then
        WhereIsDLL = a(i)
        Exit Function
      End If
    Next i

  End Function

  Private Sub AddRow(ByRef productName As String, ByRef productVer As String, ByRef productVCode As String, ByRef productGCode As String, Optional ByRef fUpdateStore As Boolean = True)
    ' Add a Product Row to the GUI.
    ' If fUpdateStore is True, then product info is also saved to the store.
    ' Call Activelock3.IALUGenerator to add product
    If fUpdateStore Then
      Dim ProdInfo As ProductInfo
      ProdInfo = ActiveLock3AlugenGlobals_definst.CreateProductInfo(productName, productVer, productVCode, productGCode)
      Call GeneratorInstance.SaveProduct(ProdInfo)
    End If
    ' Update the view
    Dim itemProductInfo As New ProductInfoItem(productName, productVer)
    cboProducts.Items.Add(itemProductInfo)
    Dim mainListItem As New ListViewItem(productName)
    mainListItem.SubItems.Add(productVer)
    mainListItem.SubItems.Add(productVCode)
    mainListItem.SubItems.Add(productGCode)
    lstvwProducts.BeginUpdate()
    lstvwProducts.Items.Add(mainListItem)
    lstvwProducts.EndUpdate()
    mainListItem.Selected = True
    cmdRemove.Enabled = True
  End Sub

  Private Sub UpdateCodeGenButtonStatus()
    If txtName.Text = "" Or txtVer.Text = "" Then
      cmdCodeGen.Enabled = False
    ElseIf CheckDuplicate(txtName.Text, txtVer.Text) Then
      cmdCodeGen.Enabled = False
    Else
      cmdCodeGen.Enabled = True
    End If
  End Sub

  Private Sub UpdateAddButtonStatus()
    If txtName.Text = "" Or txtVer.Text = "" Or txtVCode.Text = "" Then
      cmdAdd.Enabled = False
    ElseIf CheckDuplicate(txtName.Text, txtVer.Text) Then
      cmdAdd.Enabled = False
    Else
      cmdAdd.Enabled = True
    End If
  End Sub

  Private Function CheckDuplicate(ByRef productName As String, ByRef productVer As String) As Boolean
    CheckDuplicate = False
    Dim i As Short
    For i = 0 To lstvwProducts.Items.Count - 1
      If lstvwProducts.Items(i).Text = productName _
        AndAlso lstvwProducts.Items(i).SubItems(1).Text = productVer Then
        If Not fDisableNotifications Then
          txtVCode.Text = lstvwProducts.Items(i).SubItems(2).Text
          txtGCode.Text = lstvwProducts.Items(i).SubItems(3).Text
        End If
        CheckDuplicate = True
        Exit Function
      End If
    Next
    If Not fDisableNotifications Then
      txtVCode.Text = ""
      txtGCode.Text = ""
    End If
  End Function

  Private Sub InitUI()
    ' Initialize the GUI

    txtLicenseKey.Text = ""
    cboLicType.Text = "Periodic"

    cboProducts.DisplayMember = "ProductNameVersion"
    cboProducts.ValueMember = "ProductNameVersion"

    lstvwProducts.Items.Clear()
    ' Populate Product List on Product Code Generator tab
    ' and Key Gen tab with product info from products.ini
    Dim arrProdInfos() As Object 'ActiveLock3_4NET.ProductInfo
    arrProdInfos = GeneratorInstance.RetrieveProducts().Clone()

    If IsArrayEmpty(arrProdInfos) Then Exit Sub

    Dim i As Short
    For i = LBound(arrProdInfos) To UBound(arrProdInfos)
      PopulateUI(arrProdInfos(i))
    Next
    lstvwProducts.Items(0).Selected = True
  End Sub

  Private Function IsArrayEmpty(ByRef arrVar As Object) As Boolean
    IsArrayEmpty = True
    Try
      Dim lb As Integer
      lb = UBound(arrVar, 1) ' this will raise an error if the array is empty
      IsArrayEmpty = False ' If we managed to get to here, then it's not empty
    Catch ex As Exception
      MessageBox.Show(ex.Message, ACTIVELOCKSTRING, MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Try
  End Function

  Private Sub PopulateUI(ByRef ProdInfo As ProductInfo)
    With ProdInfo
      AddRow(.name, .Version, .VCode, .GCode, False)
    End With
  End Sub

  Private Function GetUserFromInstallCode(ByVal strInstCode As String) As String
    ' Retrieves lock string and user info from the request string
    '
    Dim a() As String
    Dim Index, i As Short
    Dim aString As String
    Dim usedLockNone As Boolean
    Dim noKey As String
    noKey = Chr(110) & Chr(111) & Chr(107) & Chr(101) & Chr(121)

    If strInstCode Is Nothing Or strInstCode = "" Then Exit Function
    strInstCode = ActiveLock3Globals_definst.Base64Decode(strInstCode)

    If Not strInstCode Is Nothing _
      AndAlso strInstCode.Substring(0, 1) = "+" Then
      strInstCode = strInstCode.Substring(1)
      usedLockNone = True
    End If

    systemEvent = True
    'clean checkboxes
    chkLockMACaddress.CheckState = CheckState.Unchecked
    chkLockMACaddress.Enabled = True
    chkLockComputer.CheckState = CheckState.Unchecked
    chkLockComputer.Enabled = True
    chkLockHD.CheckState = CheckState.Unchecked
    chkLockHD.Enabled = True
    chkLockHDfirmware.CheckState = CheckState.Unchecked
    chkLockHDfirmware.Enabled = True
    chkLockWindows.CheckState = CheckState.Unchecked
    chkLockWindows.Enabled = True
    chkLockBIOS.CheckState = CheckState.Unchecked
    chkLockBIOS.Enabled = True
    chkLockMotherboard.CheckState = CheckState.Unchecked
    chkLockMotherboard.Enabled = True
    chkLockIP.CheckState = CheckState.Unchecked
    chkLockIP.Enabled = True

    a = Split(strInstCode, vbLf)
    If usedLockNone = True Then
      For i = LBound(a) To UBound(a) - 1
        aString = a(i)
        If i = LBound(a) Then
          MACaddress = aString
          chkLockMACaddress.Text = "Lock to MAC Address:                           " & MACaddress
        ElseIf i = LBound(a) + 1 Then
          ComputerName = aString
          chkLockComputer.Text = "Lock to Computer Name:                       " & ComputerName
        ElseIf i = LBound(a) + 2 Then
          VolumeSerial = aString
          chkLockHD.Text = "Lock to HDD Volume Serial Number:     " & VolumeSerial
        ElseIf i = LBound(a) + 3 Then
          FirmwareSerial = aString
          chkLockHDfirmware.Text = "Lock to HDD Firmware Serial Number:  " & FirmwareSerial
        ElseIf i = LBound(a) + 4 Then
          WindowsSerial = aString
          chkLockWindows.Text = "Lock to Windows Serial Number:           " & WindowsSerial
        ElseIf i = LBound(a) + 5 Then
          BIOSserial = aString
          chkLockBIOS.Text = "Lock to BIOS Serial Number:                 " & BIOSserial
        ElseIf i = LBound(a) + 6 Then
          MotherboardSerial = aString
          chkLockMotherboard.Text = "Lock to Motherboard Serial Number:     " & MotherboardSerial
        ElseIf i = LBound(a) + 7 Then
          IPaddress = aString
          chkLockIP.Text = "Lock to IP Address:                               " & IPaddress
        End If
      Next i
    Else '"+" was not used, therefore one or more lockTypes were specified in the application
      chkLockMACaddress.Enabled = False
      chkLockHD.Enabled = False
      chkLockHDfirmware.Enabled = False
      chkLockWindows.Enabled = False
      chkLockComputer.Enabled = False
      chkLockBIOS.Enabled = False
      chkLockMotherboard.Enabled = False
      chkLockIP.Enabled = False

      For i = LBound(a) To UBound(a) - 1
        aString = a(i)
        If i = LBound(a) And aString <> noKey Then
          MACaddress = aString
          chkLockMACaddress.Text = "Lock to MAC Address:                           " & MACaddress
          chkLockMACaddress.CheckState = CheckState.Checked
        ElseIf i = (LBound(a) + 1) And aString <> noKey Then
          ComputerName = aString
          chkLockComputer.Text = "Lock to Computer Name:                       " & ComputerName
          chkLockComputer.CheckState = CheckState.Checked
        ElseIf i = (LBound(a) + 2) And aString <> noKey Then
          VolumeSerial = aString
          chkLockHD.Text = "Lock to HDD Volume Serial Number:     " & VolumeSerial
          chkLockHD.CheckState = CheckState.Checked
        ElseIf i = (LBound(a) + 3) And aString <> noKey Then
          FirmwareSerial = aString
          chkLockHDfirmware.Text = "Lock to HDD Firmware Serial Number:  " & FirmwareSerial
          chkLockHDfirmware.CheckState = CheckState.Checked
        ElseIf i = (LBound(a) + 4) And aString <> noKey Then
          WindowsSerial = aString
          chkLockWindows.Text = "Lock to Windows Serial Number:           " & WindowsSerial
          chkLockWindows.CheckState = CheckState.Checked
        ElseIf i = (LBound(a) + 5) And aString <> noKey Then
          BIOSserial = aString
          chkLockBIOS.Text = "Lock to BIOS Serial Number:                 " & BIOSserial
          chkLockBIOS.CheckState = CheckState.Checked
        ElseIf i = (LBound(a) + 6) And aString <> noKey Then
          MotherboardSerial = aString
          chkLockMotherboard.Text = "Lock to Motherboard Serial Number:     " & MotherboardSerial
          chkLockMotherboard.CheckState = CheckState.Checked
        ElseIf i = (LBound(a) + 7) And aString <> noKey Then
          IPaddress = aString
          chkLockIP.Text = "Lock to IP Address:                               " & IPaddress
          chkLockIP.CheckState = CheckState.Checked
        End If

      Next i
    End If

    If VolumeSerial = "Not Available" Or VolumeSerial = "0000-0000" Then
      chkLockHD.Enabled = False
      chkLockHD.CheckState = CheckState.Unchecked
    End If
    If MACaddress = "00 00 00 00 00 00" Or MACaddress = "00-00-00-00-00-00" Or MACaddress = "" Or MACaddress = "Not Available" Then
      chkLockMACaddress.Enabled = False
      chkLockMACaddress.CheckState = CheckState.Unchecked
    End If
    If FirmwareSerial = "Not Available" Then
      chkLockHDfirmware.Enabled = False
      chkLockHDfirmware.CheckState = CheckState.Unchecked
    End If
    If BIOSserial = "Not Available" Then
      chkLockBIOS.Enabled = False
      chkLockBIOS.CheckState = CheckState.Unchecked
    End If
    If MotherboardSerial = "Not Available" Then
      chkLockMotherboard.Enabled = False
      chkLockMotherboard.CheckState = CheckState.Unchecked
    End If
    If IPaddress = "Not Available" Then
      chkLockIP.Enabled = False
      chkLockIP.CheckState = CheckState.Unchecked
    End If

    GetUserFromInstallCode = a(a.Length - 1)
    systemEvent = False

  End Function

  Private Sub SelectOnEnterTextBox(ByRef sender As Object)
    'select all text in a textbox
    CType(sender, TextBox).SelectionStart = 0
    CType(sender, TextBox).SelectionLength = CType(sender, TextBox).Text.Length
  End Sub

#End Region



#Region "Events"

  Private Sub frmMain_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
    'load resources
    LoadResources()
    'load images for buttons, labels, pictureboxes
    LoadImages()

    'initialize RegisteredLevels
    strRegisteredLevelDBName = AddBackSlash(Application.StartupPath) & "RegisteredLevelDB.dat"

    ' Check the existence of necessary files to run this application
    Call CheckForResources("Alcrypto3.dll", "comdlg32.ocx", "msflxgrd.ocx", "comctl32.ocx", "tabctl32.ocx")

    'load RegisteredLevels
    If Not File.Exists(strRegisteredLevelDBName) Then

      With cboRegisteredLevel
        .Items.Clear()
        .Items.Add(New Mylist("Limited A", 0))
        .Items.Add(New Mylist("Limited B", 0))
        .Items.Add(New Mylist("Limited C", 0))
        .Items.Add(New Mylist("Limited D", 0))
        .Items.Add(New Mylist("Limited E", 0))
        .Items.Add(New Mylist("No Print/Save", 0))
        .Items.Add(New Mylist("Educational A", 0))
        .Items.Add(New Mylist("Educational B", 0))
        .Items.Add(New Mylist("Educational C", 0))
        .Items.Add(New Mylist("Educational D", 0))
        .Items.Add(New Mylist("Educational E", 0))
        .Items.Add(New Mylist("Level 1", 0))
        .Items.Add(New Mylist("Level 2", 0))
        .Items.Add(New Mylist("Level 3", 0))
        .Items.Add(New Mylist("Level 4", 0))
        .Items.Add(New Mylist("Light Version", 0))
        .Items.Add(New Mylist("Pro Version", 0))
        .Items.Add(New Mylist("Enterprise Version", 0))
        .Items.Add(New Mylist("Demo Only", 0))
        .Items.Add(New Mylist("Full Version-Europe", 0))
        .Items.Add(New Mylist("Full Version-Africa", 0))
        .Items.Add(New Mylist("Full Version-Asia", 0))
        .Items.Add(New Mylist("Full Version-USA", 0))
        .Items.Add(New Mylist("Full Version-International", 0))
        .SelectedIndex = 0
        SaveComboBox(strRegisteredLevelDBName, cboRegisteredLevel, True)
      End With
    Else
      LoadComboBox(strRegisteredLevelDBName, cboRegisteredLevel, True)
      'cboRegisteredLevel.SelectedIndex = 0
    End If

    ' Initialize AL
    ActiveLock = ActiveLock3Globals_definst.NewInstance()
    ActiveLock.KeyStoreType = IActiveLock.LicStoreType.alsFile

    Dim MyAL As New Globals_Renamed
    Dim MyGen As New AlugenGlobals

    'Use the following for ASP.NET applications
    'ActiveLock.Init(Application.StartupPath & "\bin")
    'Use the following for the VB.NET applications
    ActiveLock.Init(Application.StartupPath)

    GeneratorInstance = MyGen.GeneratorInstance()
    GeneratorInstance.StoragePath = AppPath() & "\products.ini"

    ' Initialize GUI
    InitUI()

    If File.Exists(AppPath() & "\products.ini") Then cboProducts.SelectedIndex = 0

    'Assume that the application LockType is not LOckNone only
    txtUser.Enabled = False
    txtUser.ReadOnly = True
    txtUser.BackColor = System.Drawing.ColorTranslator.FromOle(&H8000000F)

    'Read the program INI file to retrieve control settings
    PROJECT_INI_FILENAME = WinDir() & "\Alugen3_4NET.ini"

    SSTab1.SelectedIndex = Convert.ToInt32(ProfileString32(PROJECT_INI_FILENAME, "Startup Options", "TabNumber", CStr(0)))
    cboProducts.SelectedIndex = Convert.ToInt32(ProfileString32(PROJECT_INI_FILENAME, "Startup Options", "cboProducts", CStr(0)))
    cboLicType.SelectedIndex = Convert.ToInt32(ProfileString32(PROJECT_INI_FILENAME, "Startup Options", "cboLicType", CStr(1)))
    cboRegisteredLevel.SelectedIndex = Convert.ToInt32(ProfileString32(PROJECT_INI_FILENAME, "Startup Options", "cboRegisteredLevel", CStr(0)))
    chkItemData.CheckState = Convert.ToInt32(ProfileString32(PROJECT_INI_FILENAME, "Startup Options", "chkItemData", CStr(0)))

    Me.Text = "ALUGEN3.5NET - ActiveLock3 Universal GENerator - v3.5 for VB.NET"

  End Sub

  Private Sub frmMain_Closed(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Closed
    Dim mnReturnValue As Integer
    mnReturnValue = SetProfileString32(PROJECT_INI_FILENAME, "Startup Options", "TabNumber", CStr(SSTab1.SelectedIndex))
    mnReturnValue = SetProfileString32(PROJECT_INI_FILENAME, "Startup Options", "cboProducts", CStr(cboProducts.SelectedIndex))
    mnReturnValue = SetProfileString32(PROJECT_INI_FILENAME, "Startup Options", "cboLicType", CStr(cboLicType.SelectedIndex))
    mnReturnValue = SetProfileString32(PROJECT_INI_FILENAME, "Startup Options", "cboRegisteredLevel", CStr(cboRegisteredLevel.SelectedIndex))
    mnReturnValue = SetProfileString32(PROJECT_INI_FILENAME, "Startup Options", "chkItemData", chkItemData.CheckState)
    End
  End Sub

  Private Sub chkLockBIOS_CheckStateChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles chkLockBIOS.CheckStateChanged
    If systemEvent Then Exit Sub
    systemEvent = True
    txtInstallCode.Text = ReconstructedInstallationCode()
    systemEvent = False
  End Sub

  Private Sub chkLockComputer_CheckStateChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles chkLockComputer.CheckStateChanged
    If systemEvent Then Exit Sub
    systemEvent = True
    txtInstallCode.Text = ReconstructedInstallationCode()
    systemEvent = False
  End Sub

  Private Sub chkLockHD_CheckStateChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles chkLockHD.CheckStateChanged
    If systemEvent Then Exit Sub
    systemEvent = True
    txtInstallCode.Text = ReconstructedInstallationCode()
    systemEvent = False
  End Sub

  Private Sub chkLockHDfirmware_CheckStateChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles chkLockHDfirmware.CheckStateChanged
    If systemEvent Then Exit Sub
    systemEvent = True
    txtInstallCode.Text = ReconstructedInstallationCode()
    systemEvent = False
  End Sub

  Private Sub chkLockIP_CheckStateChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles chkLockIP.CheckStateChanged
    If systemEvent Then Exit Sub
    systemEvent = True
    txtInstallCode.Text = ReconstructedInstallationCode()
    systemEvent = False
  End Sub

  Private Sub chkLockMACaddress_CheckStateChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles chkLockMACaddress.CheckStateChanged
    If systemEvent Then Exit Sub
    systemEvent = True
    txtInstallCode.Text = ReconstructedInstallationCode()
    systemEvent = False
  End Sub

  Private Sub chkLockMotherboard_CheckStateChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles chkLockMotherboard.CheckStateChanged
    If systemEvent Then Exit Sub
    systemEvent = True
    txtInstallCode.Text = ReconstructedInstallationCode()
    systemEvent = False
  End Sub

  Private Sub chkLockWindows_CheckStateChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles chkLockWindows.CheckStateChanged
    If systemEvent Then Exit Sub
    systemEvent = True
    txtInstallCode.Text = ReconstructedInstallationCode()
    systemEvent = False
  End Sub

  Private Sub cboLicType_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cboLicType.SelectedIndexChanged
    ' enable the days edit box
    If cboLicType.Text = "Periodic" Or cboLicType.Text = "Time Locked" Then
      txtDays.ReadOnly = False
      txtDays.BackColor = System.Drawing.ColorTranslator.FromOle(&H80000005)
      txtDays.ForeColor = System.Drawing.Color.Black
    Else
      txtDays.ReadOnly = True
      txtDays.BackColor = System.Drawing.ColorTranslator.FromOle(&H8000000F)
      txtDays.ForeColor = System.Drawing.ColorTranslator.FromOle(&H8000000F)
    End If
    If cboLicType.Text = "Time Locked" Then
      lblExpiry.Text = "&Expires on Date:"
      txtDays.Text = Now.UtcNow.AddDays(30).ToString("yyyy/MM/dd")
      lblDays.Text = "YYYY/MM/DD"
      txtDays.Visible = False
      dtpExpireDate.Visible = True
      dtpExpireDate.Value = Now.UtcNow.AddDays(30)
    Else
      lblExpiry.Text = "&Expires after:"
      txtDays.Text = "30"
      lblDays.Text = "Day(s)"
      txtDays.Visible = True
      dtpExpireDate.Visible = False
    End If
  End Sub

  Private Sub cboProducts_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cboProducts.SelectedIndexChanged
    'product selected from products combo - update the controls
    UpdateKeyGenButtonStatus()
  End Sub

  Private Sub cmdAdd_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdAdd.Click
    If SSTab1.SelectedIndex <> 0 Then Exit Sub ' our tab not active - do nothing
    cmdAdd.Enabled = False ' disallow repeated clicking of Add button
    AddRow(txtName.Text, txtVer.Text, txtVCode.Text, txtGCode.Text)

    UpdateStatus(String.Format("Product '{0}({1})' was added succesfuly.", txtName.Text, txtVer.Text))
    txtName.Focus()
    cmdValidate.Enabled = True
  End Sub
  Private Sub cmdBrowse_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdBrowse.Click
    Dim itemProductInfo As ProductInfoItem = cboProducts.SelectedItem
    Dim strName As String = itemProductInfo.ProductName
    Try
      With saveDlg
        .InitialDirectory = Dir(txtLicenseFile.Text)
        .Filter = "ALL Files (*.ALL)|*.ALL"
        '!!!! TODO
        '.Flags = MSComDlg.FileOpenConstants.cdlOFNExplorer Or MSComDlg.FileOpenConstants.cdlOFNShareAware Or MSComDlg.FileOpenConstants.cdlOFNNoChangeDir
        '.CancelError = True
        .FileName = strName
        .ShowDialog()
        txtLicenseFile.Text = .FileName
      End With
    Catch ex As Exception
      MessageBox.Show(ex.Message, ACTIVELOCKSTRING, MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Try
  End Sub

  Private Sub cmdCodeGen_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCodeGen.Click
    If SSTab1.SelectedIndex <> 0 Then Exit Sub ' our tab not active - do nothing

    Cursor.Current = Cursors.WaitCursor
    UpdateStatus("Generating product codes! Please wait ...")
    fDisableNotifications = True
    txtVCode.Text = ""
    txtGCode.Text = ""
    fDisableNotifications = False
    Enabled = False

    Application.DoEvents()

    Try

      Dim KEY As RSAKey

      ReDim KEY.data(32)
      'Dim progress As ProgressType
      ' generate the key
      'VarPtr function is not supported in VB.NET
      'VB6 equivalent function is used instead - ialkan
      'Adding a delegate for AddressOf CryptoProgressUpdate did not work
      'for now. Modified alcrypto3NET.dll to create a second generate function
      'rsa_generate2 that does not deal with progress monitoring
      If modALUGEN.rsa_generate2(KEY, 1024) = RETVAL_ON_ERROR Then
        Err.Raise(ActiveLock3Globals_definst.ActiveLockErrCodeConstants.alerrRSAError, ACTIVELOCKSTRING, STRRSAERROR)
      End If

      ' extract private and public key blobs
      Dim strBlob As String
      Dim blobLen As Integer
      If rsa_public_key_blob(KEY, vbNullString, blobLen) = RETVAL_ON_ERROR Then
        Err.Raise(ActiveLock3Globals_definst.ActiveLockErrCodeConstants.alerrRSAError, ACTIVELOCKSTRING, STRRSAERROR)
      End If

      If blobLen > 0 Then
        strBlob = New String(Chr(0), blobLen)
        If rsa_public_key_blob(KEY, strBlob, blobLen) = RETVAL_ON_ERROR Then
          Err.Raise(ActiveLock3Globals_definst.ActiveLockErrCodeConstants.alerrRSAError, ACTIVELOCKSTRING, STRRSAERROR)
        End If

        System.Diagnostics.Debug.WriteLine("Public blob: " & strBlob)
        txtVCode.Text = strBlob
      End If

      'FUTURE RSA IMPLEMENTATION
      'Dim rsa As New RSACryptoServiceProvider
      'Dim xmlPublicKey As String
      'Dim xmlPrivateKey As String
      'xmlPublicKey = rsa.ToXmlString(False)
      'xmlPrivateKey = rsa.ToXmlString(True)
      'rsa.FromXmlString(xmlPublicKey)
      'txtVCode.Text = xmlPublicKey
      'txtGCode.Text = xmlPrivateKey

      If modALUGEN.rsa_private_key_blob(KEY, vbNullString, blobLen) = RETVAL_ON_ERROR Then
        Err.Raise(ActiveLock3Globals_definst.ActiveLockErrCodeConstants.alerrRSAError, ACTIVELOCKSTRING, STRRSAERROR)
      End If

      If blobLen > 0 Then
        strBlob = New String(Chr(0), blobLen)
        If modALUGEN.rsa_private_key_blob(KEY, strBlob, blobLen) = RETVAL_ON_ERROR Then
          Err.Raise(ActiveLock3Globals_definst.ActiveLockErrCodeConstants.alerrRSAError, ACTIVELOCKSTRING, STRRSAERROR)
        End If

        System.Diagnostics.Debug.WriteLine("Private blob: " & strBlob)
        txtGCode.Text = strBlob
      End If
      ' done with the key - throw it away
      If modALUGEN.rsa_freekey(KEY) = RETVAL_ON_ERROR Then
        Err.Raise(ActiveLock3Globals_definst.ActiveLockErrCodeConstants.alerrRSAError, ACTIVELOCKSTRING, STRRSAERROR)
      End If

      ' Test generated key for correctness by recreating it from the blobs
      ' Note:
      ' ====
      ' Due to an outstanding bug in ALCrypto.dll, sometimes this step will crash the app because
      ' the generated keyset is bad.
      ' The work-around for the time being is to keep regenerating the keyset until eventually
      ' you'll get a valid keyset that no longer crashes.
      Dim strdata As String : strdata = "This is a test string to be encrypted."
      If modALUGEN.rsa_createkey(txtVCode.Text, txtVCode.Text.Length, txtGCode.Text, txtGCode.Text.Length, KEY) = RETVAL_ON_ERROR Then
        Err.Raise(ActiveLock3Globals_definst.ActiveLockErrCodeConstants.alerrRSAError, ACTIVELOCKSTRING, STRRSAERROR)
      End If

      ' It worked! We're all set to go.
      If modALUGEN.rsa_freekey(KEY) = RETVAL_ON_ERROR Then
        Err.Raise(ActiveLock3Globals_definst.ActiveLockErrCodeConstants.alerrRSAError, ACTIVELOCKSTRING, STRRSAERROR)
      End If

    Catch ex As Exception
      MessageBox.Show(ex.Message, ACTIVELOCKSTRING, MessageBoxButtons.OK, MessageBoxIcon.Error)
    Finally
      'update controls
      fDisableNotifications = True
      UpdateAddButtonStatus()
      UpdateStatus("Product codes generation success.")
      Cursor.Current = Cursors.Default
      Enabled = True
    End Try
  End Sub

  Private Sub cmdCopy_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCopy.Click
    Dim aDataObject As New DataObject
    aDataObject.SetData(DataFormats.Text, txtLicenseKey.Text)
    Clipboard.SetDataObject(aDataObject)
  End Sub
  Private Sub cmdCopyGCode_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCopyGCode.Click
    Dim aDataObject As New DataObject
    aDataObject.SetData(DataFormats.Text, txtGCode.Text)
    Clipboard.SetDataObject(aDataObject)
  End Sub

  Private Sub cmdCopyVCode_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCopyVCode.Click
    Dim aDataObject As New DataObject
    aDataObject.SetData(DataFormats.Text, txtVCode.Text)
    Clipboard.SetDataObject(aDataObject)
  End Sub

  ' Generate license key
  Private Sub cmdKeyGen_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdKeyGen.Click
    If SSTab1.SelectedIndex <> 1 Then Exit Sub ' our tab not active - do nothing

    If chkLockMACaddress.CheckState = CheckState.Unchecked _
      And chkLockComputer.CheckState = CheckState.Unchecked _
      And chkLockHD.CheckState = CheckState.Unchecked _
      And chkLockHDfirmware.CheckState = CheckState.Unchecked _
      And chkLockWindows.CheckState = CheckState.Unchecked _
      And chkLockBIOS.CheckState = CheckState.Unchecked _
      And chkLockMotherboard.CheckState = CheckState.Unchecked _
      And chkLockIP.CheckState = CheckState.Unchecked Then
      MsgBox("Warning: You did not select any hardware keys to lock the license.", MsgBoxStyle.Exclamation)
    End If

    ' get product and version
    Cursor.Current = Cursors.WaitCursor
    UpdateStatus("Generating license key...")

    Try
      Dim itemProductInfo As ProductInfoItem = cboProducts.SelectedItem
      Dim strName, strVer As String
      strName = itemProductInfo.ProductName
      strVer = itemProductInfo.ProductVersion
      With ActiveLock
        .SoftwareName = strName
        .SoftwareVersion = strVer
      End With

      Dim varLicType As ProductLicense.ALLicType

      If cboLicType.Text = "Periodic" Then
        varLicType = ProductLicense.ALLicType.allicPeriodic
      ElseIf cboLicType.Text = "Permanent" Then
        varLicType = ProductLicense.ALLicType.allicPermanent
      ElseIf cboLicType.Text = "Time Locked" Then
        varLicType = ProductLicense.ALLicType.allicTimeLocked
      Else
        varLicType = ProductLicense.ALLicType.allicNone
      End If

      Dim strExpire As String
      strExpire = GetExpirationDate()
      Dim strRegDate As String
      strRegDate = Now.UtcNow.ToString("yyyy/MM/dd")

      Dim Lic As ProductLicense
      'Create a product license object without the product key or license key
      'Lic = ActiveLock3.CreateProductLicense(strName, "Code", strVer, alfSingle, varLicType, "Licensee", strExpire, "LicKey", "RegDate", "Hash1")
      'generate license object
      Dim selRegLevel As Mylist = cboRegisteredLevel.SelectedItem
      Lic = ActiveLock3Globals_definst.CreateProductLicense(strName, strVer, "", _
                ProductLicense.LicFlags.alfSingle, varLicType, "", _
                IIf(chkItemData.CheckState = CheckState.Unchecked, _
                  selRegLevel.Name, selRegLevel.ItemData), _
                strExpire, , strRegDate)

      Dim strLibKey As String
      ' Pass it to IALUGenerator to generate the key
      Dim selectedRegisteredLevel As String
      Dim mList As Mylist
      mList = cboRegisteredLevel.Items(cboRegisteredLevel.SelectedIndex)
      If chkItemData.CheckState = CheckState.Unchecked Then
        selectedRegisteredLevel = mList.Name
      Else
        selectedRegisteredLevel = mList.ItemData
      End If
      strLibKey = GeneratorInstance.GenKey(Lic, txtInstallCode.Text, selectedRegisteredLevel)
      'split license key into 64byte chunks
      txtLicenseKey.Text = Make64ByteChunks(strLibKey & "aLck" & txtInstallCode.Text)
      'update license file path
      txtLicenseFile.Text = Application.StartupPath & "\" & strName & ".all"

      Cursor.Current = Cursors.Default
      UpdateStatus("Ready...")

      'add license to database
      Dim lockTypesString As String
      Dim frmAlugenDatabase As New frmAlugenDb
      If MsgBox("Would you like to save the new license in the License Database?", MsgBoxStyle.YesNo + MsgBoxStyle.Question) = MsgBoxResult.Yes Then
        lockTypesString = ""
        If chkLockMACaddress.CheckState = CheckState.Checked Then
          lockTypesString = lockTypesString & "MAC Address"
        End If
        If chkLockComputer.CheckState = CheckState.Checked Then
          If lockTypesString <> "" Then lockTypesString = lockTypesString & "+"
          lockTypesString = lockTypesString & "Computer Name"
        End If
        If chkLockHD.CheckState = CheckState.Checked Then
          If lockTypesString <> "" Then lockTypesString = lockTypesString & "+"
          lockTypesString = lockTypesString & "HDD Volume Serial"
        End If
        If chkLockHDfirmware.CheckState = CheckState.Checked Then
          If lockTypesString <> "" Then lockTypesString = lockTypesString & "+"
          lockTypesString = lockTypesString & "HDD Firmware Serial"
        End If
        If chkLockWindows.CheckState = CheckState.Checked Then
          If lockTypesString <> "" Then lockTypesString = lockTypesString & "+"
          lockTypesString = lockTypesString & "Windows Serial"
        End If
        If chkLockBIOS.CheckState = CheckState.Checked Then
          If lockTypesString <> "" Then lockTypesString = lockTypesString & "+"
          lockTypesString = lockTypesString & "BIOS Serial"
        End If
        If chkLockMotherboard.CheckState = CheckState.Checked Then
          If lockTypesString <> "" Then lockTypesString = lockTypesString & "+"
          lockTypesString = lockTypesString & "Motherboard Serial"
        End If
        If chkLockIP.CheckState = CheckState.Checked Then
          If lockTypesString <> "" Then lockTypesString = lockTypesString & "+"
          lockTypesString = lockTypesString & "IP Address"
        End If

        'add license to database
        Call frmAlugenDatabase.ArchiveLicense(strName, strVer, txtUser.Text, strRegDate, strExpire, cboLicType.Text, lockTypesString, cboRegisteredLevel.Text, txtInstallCode.Text, txtLicenseKey.Text)

      End If
    Catch ex As Exception
      UpdateStatus("Error: " & ex.Message)
    Finally
      Cursor.Current = Cursors.Default
    End Try
  End Sub

  Private Sub cmdPaste_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdPaste.Click
    If Clipboard.GetDataObject.GetDataPresent(DataFormats.Text) Then
      txtInstallCode.Text = Clipboard.GetDataObject.GetData(DataFormats.Text)
    End If
  End Sub

  Private Sub cmdRemove_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdRemove.Click
    If SSTab1.SelectedIndex <> 0 Then Exit Sub ' our tab not active - do nothing
    Dim SelStart, SelEnd As Short
    Dim fEnableAdd As Boolean
    fEnableAdd = False

    Dim strName As String = lstvwProducts.SelectedItems(0).Text
    Dim strVer As String = lstvwProducts.SelectedItems(0).SubItems(1).Text
    Dim selItem As String = strName & " - " & strVer
    'delete from INI File
    GeneratorInstance.DeleteProduct(strName, strVer)
    'remove from products list
    lstvwProducts.SelectedItems(0).Remove()

    ' Enable Add button if we're removing the variable currently being edited.
    If fEnableAdd Then
      cmdAdd.Enabled = True
    End If
    If lstvwProducts.Items.Count = 0 Then
      ' no (selectable) rows left (just the header)
      cmdRemove.Enabled = False
    End If

    For Each itemProduct As ProductInfoItem In cboProducts.Items
      If itemProduct.ProductName = strName _
      AndAlso itemProduct.ProductVersion = strVer Then
        cboProducts.Items.Remove(itemProduct)
        Exit For
      End If
    Next

    txtVCode.Text = ""
    txtGCode.Text = ""
    cmdValidate.Enabled = True

  End Sub

  Private Sub cmdSave_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSave.Click
    UpdateStatus("Saving license key to file...")
    ' save the license key
    SaveLicenseKey(txtLicenseKey.Text, txtLicenseFile.Text)
    UpdateStatus("License key saved.")
  End Sub

  Private Sub cmdValidate_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdValidate.Click
    Cursor.Current = Cursors.WaitCursor
    If txtVCode.Text = "" And txtGCode.Text = "" Then
      UpdateStatus("GCode and VCode fields are blank.  Nothing to validate.")
      Exit Sub ' nothing to validate
    End If
    ' Validate to keyset to make sure it's valid.
    UpdateStatus("Validating keyset...")
    Dim KEY As RSAKey
    Dim strdata As String : strdata = "This is a test string to be signed."
    Dim strSig As String, rc As Integer
    rc = modALUGEN.rsa_createkey(txtVCode.Text, txtVCode.Text.Length, txtGCode.Text, txtGCode.Text.Length, KEY)
    If rc = RETVAL_ON_ERROR Then
      MessageBox.Show("Code not valid! " & vbCrLf & STRRSAERROR, ACTIVELOCKSTRING, MessageBoxButtons.OK, MessageBoxIcon.Error)
      'Err.Raise(ActiveLock3Globals_definst.ActiveLockErrCodeConstants.alerrRSAError, ACTIVELOCKSTRING, STRRSAERROR)
      UpdateStatus(txtName.Text & " (" & txtVer.Text & ") " & STRRSAERROR)
      Exit Sub
    End If

    ' sign it
    strSig = RSASign(txtVCode.Text, txtGCode.Text, strdata)
    rc = RSAVerify(txtVCode.Text, strdata, strSig)
    If rc = 0 Then
      UpdateStatus(txtName.Text & " (" & txtVer.Text & ") validated successfully.")
    Else
      UpdateStatus(txtName.Text & " (" & txtVer.Text & ") GCode-VCode mismatch!")
    End If
    ' It worked! We're all set to go.
    If modALUGEN.rsa_freekey(KEY) = RETVAL_ON_ERROR Then
      Err.Raise(ActiveLock3Globals_definst.ActiveLockErrCodeConstants.alerrRSAError, ACTIVELOCKSTRING, STRRSAERROR)
    End If

    Cursor.Current = Cursors.Default
  End Sub

  Private Sub cmdViewArchive_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdViewArchive.Click
    Dim lic As New frmAlugenDb
    lic.Show()
  End Sub

  Private Sub cmdViewLevel_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdViewLevel.Click
    Dim mfrmLevelManager As New frmLevelManager
    With mfrmLevelManager
      .ShowDialog()
      cboRegisteredLevel.Items.Clear()
      LoadComboBox(strRegisteredLevelDBName, cboRegisteredLevel, True)
      cboRegisteredLevel.SelectedIndex = 0
    End With
    mfrmLevelManager.Close()
    mfrmLevelManager = Nothing
  End Sub

  Private Sub lstvwProducts_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstvwProducts.SelectedIndexChanged
    If lstvwProducts.SelectedItems.Count > 0 Then
      Dim selListItem As ListViewItem = lstvwProducts.SelectedItems(0)
      txtName.Text = selListItem.Text
      txtVer.Text = selListItem.SubItems(1).Text

      txtVCode.Text = selListItem.SubItems(2).Text
      txtGCode.Text = selListItem.SubItems(3).Text
      'txtVCode.Text = ""
      'txtGCode.Text = ""

      cmdRemove.Enabled = True
      cmdValidate.Enabled = True
    End If
  End Sub

  Private Sub lstvwProducts_ColumnClick(ByVal sender As Object, ByVal e As ColumnClickEventArgs) Handles lstvwProducts.ColumnClick
    'Determine if the clicked column is already the column that is 
    ' being sorted.
    If (e.Column = lvwColumnSorter.SortColumn) Then
      ' Reverse the current sort direction for this column.
      If (lvwColumnSorter.Order = SortOrder.Ascending) Then
        lvwColumnSorter.Order = SortOrder.Descending
      Else
        lvwColumnSorter.Order = SortOrder.Ascending
      End If
    Else
      ' Set the column number that is to be sorted; default to ascending.
      lvwColumnSorter.SortColumn = e.Column
      lvwColumnSorter.Order = SortOrder.Ascending
    End If

    ' Perform the sort with these new sort options.
    Me.lstvwProducts.Sort()
  End Sub

  Private Sub picALBanner_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picALBanner.Click
    'navigate to www.activelocksoftware.com
    System.Diagnostics.Process.Start(ACTIVELOCKSOFTWAREWEB)
  End Sub

  Private Sub picALBanner2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picALBanner2.Click
    'navigate to www.activelocksoftware.com
    picALBanner_Click(sender, e)
  End Sub

  Private Sub txtLicenseKey_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtLicenseKey.TextChanged
    cmdSave.Enabled = CBool(txtLicenseKey.Text.Length > 0)
  End Sub

  Private Sub txtName_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtName.TextChanged
    fDisableNotifications = False
    UpdateCodeGenButtonStatus()
    UpdateAddButtonStatus()
  End Sub

  Private Sub txtVer_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtVer.TextChanged
    ' New product - will be processed by Add command
    fDisableNotifications = False
    UpdateCodeGenButtonStatus()
    UpdateAddButtonStatus()
  End Sub

  Private Sub txtUser_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtUser.TextChanged
    If fDisableNotifications Then Exit Sub
    fDisableNotifications = True
    txtInstallCode.Text = ActiveLock.InstallationCode(txtUser.Text)
    fDisableNotifications = False
  End Sub

  Private Sub txtInstallCode_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtInstallCode.TextChanged
    If txtInstallCode.Text.Length > 0 Then

      If systemEvent Then Exit Sub

      UpdateKeyGenButtonStatus()
      If fDisableNotifications Then Exit Sub

      fDisableNotifications = True
      txtUser.Text = GetUserFromInstallCode(txtInstallCode.Text)
      fDisableNotifications = False

      systemEvent = True
      If chkLockMACaddress.Enabled = True Then chkLockMACaddress.CheckState = CheckState.Checked
      If chkLockComputer.Enabled = True Then chkLockComputer.CheckState = CheckState.Checked
      If chkLockHD.Enabled = True Then chkLockHD.CheckState = CheckState.Checked
      If chkLockHDfirmware.Enabled = True Then chkLockHDfirmware.CheckState = CheckState.Checked
      If chkLockWindows.Enabled = True Then chkLockWindows.CheckState = CheckState.Checked
      If chkLockBIOS.Enabled = True Then chkLockBIOS.CheckState = CheckState.Checked
      If chkLockMotherboard.Enabled = True Then chkLockMotherboard.CheckState = CheckState.Checked
      If chkLockIP.Enabled = True Then chkLockIP.CheckState = CheckState.Checked
      systemEvent = False
    Else
      fDisableNotifications = True
      chkLockComputer.Enabled = True
      chkLockComputer.Text = "Lock to Computer Name"
      chkLockHD.Enabled = True
      chkLockHD.Text = "Lock to HDD Volume Serial"
      chkLockHDfirmware.Enabled = True
      chkLockHDfirmware.Text = "Lock to HDD Firmware Serial"
      chkLockMACaddress.Enabled = True
      chkLockMACaddress.Text = "Lock to MAC Address"
      chkLockWindows.Enabled = True
      chkLockWindows.Text = "Lock to Windows Serial"
      chkLockBIOS.Enabled = True
      chkLockBIOS.Text = "Lock to BIOS Serial"
      chkLockMotherboard.Enabled = True
      chkLockMotherboard.Text = "Lock to Motherboard Serial"
      chkLockIP.Enabled = True
      chkLockIP.Text = "Lock to IP Address"
      txtUser.Text = ""
      fDisableNotifications = False
    End If
  End Sub

  Private Sub txtDays_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDays.Enter
    SelectOnEnterTextBox(sender)
  End Sub

  Private Sub txtInstallCode_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtInstallCode.Enter
    SelectOnEnterTextBox(sender)
  End Sub

  Private Sub txtVCode_Enter(ByVal sender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtVCode.Enter
    SelectOnEnterTextBox(sender)
  End Sub

  Private Sub txtGCode_Enter(ByVal sender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtGCode.Enter
    SelectOnEnterTextBox(sender)
  End Sub

  Private Sub txtLicenseKey_Enter(ByVal sender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtLicenseKey.Enter
    SelectOnEnterTextBox(sender)
  End Sub

  Private Sub txtLicenseFile_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtLicenseFile.Enter
    SelectOnEnterTextBox(sender)
  End Sub

  Private Sub txtName_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtName.Enter
    SelectOnEnterTextBox(sender)
  End Sub

  Private Sub txtVer_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtVer.Enter
    SelectOnEnterTextBox(sender)
  End Sub

  Private Sub lnkActivelockSoftwareGroup_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkActivelockSoftwareGroup.LinkClicked
    picALBanner_Click(sender, e)
  End Sub

  Private Sub cmdPrintLicenseKey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPrintLicenseKey.Click
    Dim daPrintDocument As New daReport.DaPrintDocument
    Dim hashParameters As New Hashtable
    Dim selProduct As ProductInfoItem = cboProducts.SelectedItem
    Dim itemRegLevel As Mylist = cboRegisteredLevel.SelectedItem

    'set .xml file for printing
    daPrintDocument.setXML("reports\repLicenseKey.xml")

    'build parameters
    hashParameters.Add("pProductName", selProduct.ProductName)
    hashParameters.Add("pProductVersion", selProduct.ProductVersion)
    hashParameters.Add("pRegisteredLevel", itemRegLevel.Name)
    hashParameters.Add("pLicenseType", CType(cboLicType.SelectedItem, String))
    hashParameters.Add("pRegisteredDate", "")
    hashParameters.Add("pExpireDate", "")
    hashParameters.Add("pInstallCode", txtInstallCode.Text)
    hashParameters.Add("pUserName", txtUser.Text)
    hashParameters.Add("pLicenseKey", txtLicenseKey.Text)

    'setting parameters
    daPrintDocument.SetParameters(hashParameters)
    'daPrintDocument.DocumentName = "License key"

    'print preview
    printPreviewDialog1.Icon = CType(resxList("report.ico"), Icon)
    printPreviewDialog1.Text = daPrintDocument.DocumentName
    printPreviewDialog1.Document = daPrintDocument
    printPreviewDialog1.PrintPreviewControl.Zoom = 1.0
    printPreviewDialog1.WindowState = FormWindowState.Maximized
    printPreviewDialog1.ShowDialog(Me)
  End Sub

  Private Sub cmdEmailLicenseKey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEmailLicenseKey.Click
    Dim mailToString As String
    Dim emailAddress As String = "user@company.com"
    Dim strSubject As String
    Dim strBodyMessage As String
    Dim selProduct As ProductInfoItem = cboProducts.SelectedItem
    Dim itemRegLevel As Mylist = cboRegisteredLevel.SelectedItem
    Dim strNewLine As String = "%0D%0A"

    strSubject = String.Format("License key for application {0} ({1}), user [{2}]", selProduct.ProductName, selProduct.ProductVersion, txtUser.Text)
    strBodyMessage = strNewLine & String.Format("Install code:" & strNewLine & "{0}", txtInstallCode.Text)
    strBodyMessage = strBodyMessage & strNewLine & strNewLine & String.Format("License key:" & strNewLine & "{0}", txtLicenseKey.Text.Replace(Chr(13), strNewLine))

    'final constructor
    mailToString = String.Format("mailto:{0}?subject={1}&body={2}", emailAddress, strSubject, strBodyMessage)

    'launch default email client
    System.Diagnostics.Process.Start(mailToString)
  End Sub
#End Region


End Class