Option Explicit On 
Imports System.IO
Imports ActiveLock3_5NET
Imports MagicAjax


Public Class ASPNETAlugen3
  Inherits System.Web.UI.Page

  Public ActiveLock3AlugenGlobals_definst As New AlugenGlobals
  Public ActiveLock3Globals_definst As New Globals_Renamed
  Public GeneratorInstance As _IALUGenerator
  Public ActiveLock As _IActiveLock
  Public dt As New DataTable
  Protected WithEvents cmdSaveLicenseFile As System.Web.UI.WebControls.ImageButton
  Private msgPleaseWait As String = "(generating codes - please wait)"


#Region " Web Form Designer Generated Code "

  'This call is required by the Web Form Designer.
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

  End Sub
  Protected WithEvents cmdProducts As System.Web.UI.WebControls.Button
  Protected WithEvents cmdLicenses As System.Web.UI.WebControls.Button
  Protected WithEvents pnlMagicAjax As MagicAjax.UI.Controls.AjaxPanel
  Protected WithEvents pnlProducts As System.Web.UI.WebControls.Panel
  Protected WithEvents txtProductName As System.Web.UI.WebControls.TextBox
  Protected WithEvents txtProductVersion As System.Web.UI.WebControls.TextBox
  Protected WithEvents cmdGenerateCode As msWebControlsLibrary.ExImageButton
  Protected WithEvents cmdValidateCode As msWebControlsLibrary.ExImageButton
  Protected WithEvents txtVCode As System.Web.UI.WebControls.TextBox
  Protected WithEvents txtGCode As System.Web.UI.WebControls.TextBox
  Protected WithEvents imgVCode As System.Web.UI.WebControls.Image
  Protected WithEvents imgGCode As System.Web.UI.WebControls.Image
  Protected WithEvents cmdAddProduct As msWebControlsLibrary.ExImageButton
  Protected WithEvents cmdRemoveProduct As msWebControlsLibrary.ExImageButton
  Protected WithEvents pnlLicenses As System.Web.UI.WebControls.Panel
  Protected WithEvents grdProducts As System.Web.UI.WebControls.DataGrid
  Protected WithEvents plhSay As System.Web.UI.WebControls.PlaceHolder
  Protected WithEvents ltlAlert As System.Web.UI.WebControls.Literal
  Protected WithEvents sortExpression As System.Web.UI.HtmlControls.HtmlInputHidden
  Protected WithEvents cmdCopyVCode As System.Web.UI.HtmlControls.HtmlImage
  Protected WithEvents cmdCopyGCode As System.Web.UI.HtmlControls.HtmlImage
  Protected WithEvents sortOrder As System.Web.UI.HtmlControls.HtmlInputHidden
  Protected WithEvents cboProduct As System.Web.UI.WebControls.DropDownList
  Protected WithEvents cboRegLevel As System.Web.UI.WebControls.DropDownList
  Protected WithEvents cboLicenseType As System.Web.UI.WebControls.DropDownList
  Protected WithEvents chkUseItemData As System.Web.UI.WebControls.CheckBox
  Protected WithEvents txtDays As System.Web.UI.WebControls.TextBox
  Protected WithEvents lblExpiry As System.Web.UI.WebControls.Label
  Protected WithEvents txtInstallCode As System.Web.UI.WebControls.TextBox
  Protected WithEvents cmdPasteInstallCode As System.Web.UI.HtmlControls.HtmlImage
  Protected WithEvents txtUserName As System.Web.UI.WebControls.TextBox
  Protected WithEvents txtLicenseKey As System.Web.UI.WebControls.TextBox
  Protected WithEvents cmdGenerateLicenseKey As msWebControlsLibrary.ExImageButton
  Protected WithEvents cmdCopyLicenseKey As System.Web.UI.HtmlControls.HtmlImage
  Protected WithEvents cmdPrintLicenseKey As msWebControlsLibrary.ExImageButton
  Protected WithEvents cmdEmailLicenseKey As System.Web.UI.WebControls.ImageButton
  Protected WithEvents cmdSelectExpireDate As System.Web.UI.WebControls.ImageButton
  Protected WithEvents Calendar1 As System.Web.UI.WebControls.Calendar
  Protected WithEvents plhDate As System.Web.UI.WebControls.PlaceHolder
  Protected WithEvents myCalendar As System.Web.UI.HtmlControls.HtmlGenericControl

  'NOTE: The following placeholder declaration is required by the Web Form Designer.
  'Do not delete or move it.
  Private designerPlaceholderDeclaration As System.Object

  Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
    'CODEGEN: This method call is required by the Web Form Designer
    'Do not modify it using the code editor.
    InitializeComponent()

  End Sub

#End Region

  Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    'Put user code to initialize the page here


    ' Populate Product List on Product Code Generator tab
    ' and Key Gen tab with product info from products.ini
    Dim i As Short
    Dim arrProdInfos() As ProductInfo
    Dim MyGen As New AlugenGlobals
    GeneratorInstance = MyGen.GeneratorInstance()
    GeneratorInstance.StoragePath = AppPath() & "\products.ini"

    arrProdInfos = GeneratorInstance.RetrieveProducts()
    If arrProdInfos.Length = 0 Then Exit Sub

    'Populate the DataGrid control

    'dt = New DataTable
    Dim dc As New DataColumn("Name", System.Type.GetType("System.String"))
    dt.Columns.Add(dc)
    dc = New DataColumn("Version", System.Type.GetType("System.String"))
    dt.Columns.Add(dc)
    dc = New DataColumn("VCode", System.Type.GetType("System.String"))
    dt.Columns.Add(dc)
    dc = New DataColumn("GCode", System.Type.GetType("System.String"))
    dt.Columns.Add(dc)

    'If MagicAjaxContext.Current.IsAjaxCallForPage(Me.Page) Then
    'If MagicAjaxContext.Current.IsAjaxCall Then
    'If Not IsPostBack Then
    '  cboProduct.Items.Clear()
    'End If
    If Not MagicAjax.MagicAjaxContext.Current.IsAjaxCall Then
      cboProduct.Items.Clear()
    End If

    For i = LBound(arrProdInfos) To UBound(arrProdInfos)
      PopulateUI(arrProdInfos(i))
    Next

    grdProducts.AutoGenerateColumns = False

    Dim dgc As New LimitColumn ' BoundColumn
    dgc.HeaderText = "Product"
    dgc.DataField = "Name"
    dgc.ItemStyle.Width = Unit.Percentage(35)
    dgc.SortExpression = "Name"
    grdProducts.Columns.Add(dgc)

    dgc = New LimitColumn 'BoundColumn
    dgc.HeaderText = "Version"
    dgc.DataField = "Version"
    dgc.ItemStyle.Wrap = True
    dgc.ItemStyle.Width = Unit.Percentage(15)
    dgc.SortExpression = "Version"
    grdProducts.Columns.Add(dgc)

    dgc = New LimitColumn 'BoundColumn
    dgc.HeaderText = "VCode"
    dgc.DataField = "VCode"
    dgc.ItemStyle.Wrap = True
    dgc.ItemStyle.Width = Unit.Percentage(25)
    dgc.SortExpression = "VCode"
    dgc.CharacterLimit = 25
    grdProducts.Columns.Add(dgc)

    dgc = New LimitColumn 'BoundColumn
    dgc.HeaderText = "GCode"
    dgc.DataField = "GCode"
    dgc.ItemStyle.Wrap = True
    dgc.ItemStyle.Width = Unit.Percentage(25)
    dgc.SortExpression = "GCode"
    dgc.CharacterLimit = 25
    grdProducts.Columns.Add(dgc)

    ' Create a DataView from the DataTable.
    Dim dv As DataView = New DataView(dt)
    dv.Sort = IIf(sortExpression.Value.Length > 0, sortExpression.Value & " " & sortOrder.Value, "")

    grdProducts.DataSource = dv
    grdProducts.DataBind()

    'For Each col As DataGridColumn In grdProducts.Columns
    '  col.ItemStyle.Wrap = False
    '  col.ItemStyle.Width = Unit.Pixel(100)
    'Next

    ''inject JS call into body load
    'Dim strInit As String = "document.getElementById('" & "" & "')"
    'Dim mbody As HtmlGenericControl = CType(Page.FindControl("body"), HtmlGenericControl)
    'If Not mbody.Attributes("onload") Is Nothing Then
    '  mbody.Attributes("onload") += ";" & strInit & ";"
    'Else
    '  mbody.Attributes.Add("onload", ";" & strInit & ";")
    'End If


    '!!!!!!!!!!!!111
    If Not IsPostBack Then

      'initialize RegisteredLevels
      strRegisteredLevelDBName = AddBackSlash(AppPath) & "RegisteredLevelDB.dat"
      If Not MagicAjax.MagicAjaxContext.Current.IsAjaxCall Then
        loadRegisteredLevels()
      End If


      grdProducts.SelectedIndex = dt.Rows.Count - 1
      'grdProducts.SelectedItemStyle.BackColor = Color.FromArgb(200, 220, 240) '
      txtProductName.Text = arrProdInfos(i - 1).name
      txtProductVersion.Text = arrProdInfos(i - 1).Version
      txtVCode.Text = arrProdInfos(i - 1).VCode
      txtGCode.Text = arrProdInfos(i - 1).GCode

      ' The following code is for forcing a postback when the contents of a 
      ' textbox gets changed and it loses focus
      'Dim js As String
      'js = "javascript:" & Page.GetPostBackEventReference(Me, "@@@@@buttonPostBack") & ";"
      'txtName.Attributes.Add("onchange", js)
      'txtVer.Attributes.Add("onchange", js)

      ' The following Javascript KeyUp code will allow us disable the
      ' Generate button when there's no Product Name or Version entered

      'txtProductName.Attributes.Add("OnKeyUp", "document.getElementById('" & cmdGenerateCode.ClientID & "').disabled=(this.value=='')")
      'txtProductVersion.Attributes.Add("OnKeyUp", "document.getElementById('" & cmdGenerateCode.ClientID & "').disabled=(this.value=='')")

      cmdGenerateCode.Attributes.Add("OnClick", "document.getElementById('" & txtVCode.ClientID & "').value='" & msgPleaseWait & "'; document.getElementById('" & txtGCode.ClientID & "').value='" & msgPleaseWait & "'")
      cmdCopyVCode.Attributes.Add("OnClick", "CopyToClipboard('" & txtVCode.ClientID & "');")
      cmdCopyGCode.Attributes.Add("OnClick", "CopyToClipboard('" & txtGCode.ClientID & "');")
      cmdCopyLicenseKey.Attributes.Add("OnClick", "CopyToClipboard('" & txtLicenseKey.ClientID & "');")
      cmdPasteInstallCode.Attributes.Add("OnClick", "PasteFromClipboard('" & txtInstallCode.ClientID & "');")

      grdProducts.SelectedItem.Attributes.Item("onmouseover") = "this.style.cursor='hand'"
      grdProducts.SelectedItem.Attributes.Remove("onmouseout")

      pnlProducts.Visible = True
      pnlLicenses.Visible = False
    End If


    If CheckDuplicate(txtProductName.Text, txtProductVersion.Text) Then
      cmdGenerateCode.Enabled = False
      cmdAddProduct.Enabled = False
      cmdRemoveProduct.Enabled = True
    Else
      cmdGenerateCode.Enabled = True
      cmdAddProduct.Enabled = True
      cmdRemoveProduct.Enabled = False
    End If

  End Sub

  Private Sub cmdProducts_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdProducts.Click
    ResetCalendarControls()
    pnlProducts.Visible = True
    pnlLicenses.Visible = False
  End Sub

  Private Sub cmdLicenses_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdLicenses.Click
    ResetCalendarControls()
    pnlProducts.Visible = False
    pnlLicenses.Visible = True
  End Sub





  Public Function AppPath() As String
    Dim siteNameUrl As String
    AppPath = System.IO.Path.GetDirectoryName(Server.MapPath("ASPNETAlugen3_2.aspx"))
  End Function

  Function CreateDataSource(ByVal Name As String, ByVal Ver As String, ByVal Code1 As String, ByVal Code2 As String) As ICollection

    Dim dr As DataRow = dt.NewRow()

    'Create new row and add data to it
    dr(0) = Name
    dr(1) = Ver
    dr(2) = Code1
    dr(3) = Code2

    'add the row to the datatable
    dt.Rows.Add(dr)

  End Function

  Private Sub grdProducts_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdProducts.ItemCreated
    If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.SelectedItem Then
      If e.Item.ItemIndex <> grdProducts.SelectedIndex Then
        e.Item.Attributes.Add("onmouseover", "this.style.backgroundColor='#F0F0F0';this.style.cursor='pointer'")
        e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor='#FFFFFF';this.style.color='black'")
        e.Item.Attributes.Add("onclick", "javascript:__doPostBack('" & "grdProducts$" & "_ctl" & (e.Item.ItemIndex + 2) & "$_ctl0','')")
      Else
        e.Item.Attributes.Item("onmouseover") = "this.style.cursor='hand'"
        e.Item.Attributes.Remove("onmouseout")
      End If
    End If
  End Sub

  Private Sub grdProducts_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdProducts.SelectedIndexChanged

    grdProducts.SelectedItem.Attributes.Item("onmouseover") = "this.style.cursor='hand'"
    grdProducts.SelectedItem.Attributes.Remove("onmouseout")

    FillDataFromSelectedRow()

  End Sub

  Private Sub FillDataFromSelectedRow()
    txtProductName.Text = grdProducts.SelectedItem.Cells(1).Text
    txtProductVersion.Text = grdProducts.SelectedItem.Cells(2).Text

    Dim arrProdInfos() As ProductInfo
    Dim MyGen As New AlugenGlobals
    GeneratorInstance = MyGen.GeneratorInstance()
    GeneratorInstance.StoragePath = AppPath() & "\products.ini"
    arrProdInfos = GeneratorInstance.RetrieveProducts()

    For i As Short = LBound(arrProdInfos) To UBound(arrProdInfos)
      If arrProdInfos(i).name = grdProducts.SelectedItem.Cells(1).Text _
      AndAlso arrProdInfos(i).Version = grdProducts.SelectedItem.Cells(2).Text Then
        'txtVCode.Text = grdProducts.SelectedItem.Cells(3).Text
        'txtGCode.Text = grdProducts.SelectedItem.Cells(4).Text
        txtVCode.Text = arrProdInfos(i).VCode
        txtGCode.Text = arrProdInfos(i).GCode
        Exit For
      End If
    Next
  End Sub

  Private Sub Say(ByVal Message As String)
    'work only in postback scenario
    ' Format string properly
    Message = Message.Replace("'", "\'")
    Message = Message.Replace(Convert.ToChar(10), "\n")
    Message = Message.Replace(Convert.ToChar(13), "")
    ' Display as JavaScript alert
    ltlAlert.Text = "alert('" & Message & "')"
  End Sub

  Private Sub SayAjax(ByVal Message As String)
    'for ajax purpose
    ' Format string properly
    Message = Message.Replace("'", "\'")
    Message = Message.Replace(Convert.ToChar(10), "\n")
    Message = Message.Replace(Convert.ToChar(13), "")

    '
    plhSay.Controls.Clear()
    Dim myLiteral As New WebControls.Literal
    myLiteral.Text = Message
    Dim myDIV As New HtmlGenericControl("div")
    myDIV.Attributes.Add("style", "background-color: #F0F0F0; border:solid 1px red; padding:3px; position: absolute; top: 10; right: 10")
    myDIV.Controls.Add(myLiteral)
    plhSay.Controls.Add(myDIV)

  End Sub





  Private Sub cmdAddProduct_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAddProduct.Click
    If txtVCode.Text.Trim.Length = 0 Then
      Say("New product VCode is missing.")
      Exit Sub
    ElseIf txtGCode.Text.Trim.Length = 0 Then
      Say("New product GCode is missing.")
      Exit Sub
    End If
    If CheckDuplicate(txtProductName.Text, txtProductVersion.Text) Then
      Say("Selected Product Name and Version already exist in the product list.")
      Exit Sub
    End If
    'This button add the new product into the list and into the INI file
    AddRow(txtProductName.Text, txtProductVersion.Text, txtVCode.Text, txtGCode.Text, True)
    'txtVCode.Text = ""
    'txtGCode.Text = ""
    cmdRemoveProduct.Enabled = True

    SayAjax(String.Format("Product {0} ({1}) added successfuly.", txtProductName.Text, txtProductVersion.Text))
  End Sub

  Private Sub cmdRemoveProduct_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRemoveProduct.Click
    Dim dr As DataRow = dt.Rows(grdProducts.SelectedItem.ItemIndex)
    dt.Rows.Remove(dr)
    Dim strName, strVer As String
    strName = grdProducts.SelectedItem.Cells(1).Text
    strVer = grdProducts.SelectedItem.Cells(2).Text

    GeneratorInstance.DeleteProduct(strName, strVer)
    grdProducts.DataSource = dt
    grdProducts.DataBind()

    grdProducts.SelectedIndex = dt.Rows.Count - 1
    FillDataFromSelectedRow()

    SayAjax(String.Format("Product {0} ({1}) removed successfuly.", strName, strVer))
  End Sub

  Private Sub grdProducts_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles grdProducts.SortCommand
    ' Retrieve the data source from session state.
    Dim mydt As DataTable = dt 'CType(Session("Source"), DataTable)

    ' Create a DataView from the DataTable.
    Dim dv As DataView = New DataView(mydt)

    ' The DataView provides an easy way to sort. Simply set the
    ' Sort property with the name of the field to sort by.
    If sortExpression.Value <> e.SortExpression Then
      sortExpression.Value = e.SortExpression
      sortOrder.Value = ""
    End If
    sortOrder.Value = IIf(sortOrder.Value = "ASC", "DESC", "ASC")
    dv.Sort = sortExpression.Value & " " & sortOrder.Value

    ' Re-bind the data source and specify that it should be sorted
    ' by the field specified in the SortExpression property.
    grdProducts.DataSource = dv
    grdProducts.DataBind()

  End Sub

  Private Sub cmdGenerateCode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdGenerateCode.Click

    'txtVCode.Text = ""
    'txtGCode.Text = ""

    If txtProductName.Text = "" Then
      SayAjax("Product Name is empty.")
      Exit Sub
    ElseIf txtProductVersion.Text = "" Then
      SayAjax("Product Version is empty.")
      Exit Sub
    ElseIf CheckDuplicate(txtProductName.Text, txtProductVersion.Text) Then
      SayAjax("Selected Product Name and Version already exist in the products list.")
      Exit Sub
    End If

    Try

      Dim Key As New RSAKey
      ReDim Key.data(32)
      'Adding a delegate for AddressOf CryptoProgressUpdate did not work
      'for now. Modified alcrypto3NET.dll to create a second generate function
      'rsa_generate2 that does not deal with progress monitoring - ialkan
      modALUGEN.rsa_generate2(Key, 1024)

      ' extract private and public key blobs
      Dim strBlob As String
      Dim blobLen As Integer
      rsa_public_key_blob(Key, vbNullString, blobLen)
      If blobLen > 0 Then
        strBlob = New String(Chr(0), blobLen)
        rsa_public_key_blob(Key, strBlob, blobLen)
        'System.Diagnostics.Debug.WriteLine("Public blob: " & strBlob)
        txtVCode.Text = strBlob
      End If

      'FUTURE RSA IMPLEMENTATION USING NATIVE VB.NET FUNCTIONS
      'Dim rsa As New RSACryptoServiceProvider
      'Dim xmlPublicKey As String
      'Dim xmlPrivateKey As String
      'xmlPublicKey = rsa.ToXmlString(False)
      'xmlPrivateKey = rsa.ToXmlString(True)
      'rsa.FromXmlString(xmlPublicKey)
      'txtCode1.Text = xmlPublicKey
      'txtCode2.Text = xmlPrivateKey

      modALUGEN.rsa_private_key_blob(Key, vbNullString, blobLen)
      If blobLen > 0 Then
        strBlob = New String(Chr(0), blobLen)
        modALUGEN.rsa_private_key_blob(Key, strBlob, blobLen)
        'System.Diagnostics.Debug.WriteLine("Private blob: " & strBlob)
        txtGCode.Text = strBlob
      End If

      ' done with the key - throw it away
      modALUGEN.rsa_freekey(Key)

      ' Test generated key for correctness by recreating it from the blobs
      ' Note:
      ' ====
      ' Due to an outstanding bug in ALCrypto3NET.dll, sometimes this step will crash the app because
      ' the generated keyset is bad.
      ' The work-around for the time being is to keep regenerating the keyset until eventually
      ' you'll get a valid keyset that no longer crashes.
      Dim strdata As String : strdata = "This is a test string to be encrypted."
      modALUGEN.rsa_createkey(txtVCode.Text, txtVCode.Text.Length, txtGCode.Text, txtGCode.Text.Length, Key)
      ' It worked! We're all set to go.
      modALUGEN.rsa_freekey(Key)

      cmdGenerateCode.Enabled = False

      SayAjax("Code generation successful!")
    Catch ex As Exception
      SayAjax("Error!")
    End Try
  End Sub

  ' Validate and enable Add/Change button as appropriate
  Private Function CheckDuplicate(ByRef Name As String, ByRef Ver As String) As Boolean
    CheckDuplicate = False
    Dim dr As DataRow
    'dr = dt.Rows(grdProducts.SelectedIndex)
    Dim i As Short
    For i = 0 To dt.Rows.Count - 1
      dr = dt.Rows(i)
      If dr(0) = Name Then
        If dr(1) = Ver Then
          CheckDuplicate = True
          Exit Function
        End If
      End If
    Next
  End Function

  Private Sub cmdValidateCode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdValidateCode.Click
    If txtVCode.Text = "" Then
      SayAjax("Product VCode field is empty.")
      Exit Sub
    ElseIf txtGCode.Text = "" Then
      SayAjax("Product GCode field is empty.")
      Exit Sub
    ElseIf txtVCode.Text = "" And txtGCode.Text = "" Then
      SayAjax("Product VCode and GCode fields are blank. Nothing to validate.")
      Exit Sub ' nothing to validate
    End If

    Try
      ' Validate to keyset to make sure it's valid.
      SayAjax("Validating keyset...")
      Dim Key As RSAKey
      ReDim Key.data(32)
      Dim strdata As String
      strdata = "This is a test string to be signed."
      Dim strSig As String
      Dim txt1 As String, txt2 As String
      Dim len1 As Integer, len2 As Integer
      txt1 = txtVCode.Text
      len1 = txtVCode.Text.Length
      txt2 = txtGCode.Text
      len2 = txtGCode.Text.Length
      modALUGEN.rsa_createkey(txt1, len1, txt2, len2, Key)
      ' sign it
      strSig = RSASign(txtVCode.Text, txtGCode.Text, strdata)
      Dim rc As Integer
      rc = RSAVerify(txtVCode.Text, strdata, strSig)
      Dim dr As DataRow
      dr = dt.Rows(grdProducts.SelectedIndex)
      If rc = 0 Then
        SayAjax(dr(0) & " (" & dr(1) & ") validated successfully.")
      Else
        SayAjax(dr(0) & " (" & dr(1) & ") GCode-VCode mismatch!")
      End If
      ' It worked! We're all set to go.
      modALUGEN.rsa_freekey(Key)
    Catch ex As Exception
      SayAjax(ex.Message & ex.StackTrace)
    End Try

  End Sub

  Private Sub cboLicenseType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboLicenseType.SelectedIndexChanged
    ResetCalendarControls()
    Select Case cboLicenseType.SelectedValue
      Case 0 'time locked
        lblExpiry.Text = "Expire on date"
        cmdSelectExpireDate.Visible = True
      Case 1 'periodic
        lblExpiry.Text = "Expire after"
        cmdSelectExpireDate.Visible = False
      Case 2 'permanent
        lblExpiry.Text = "Not expire"
        cmdSelectExpireDate.Visible = False
    End Select
  End Sub

  Private Sub cmdSelectExpireDate_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdSelectExpireDate.Click
    If myCalendar.Visible = False Then
      Calendar1.Visible = True
      myCalendar.Visible = True

      Dim myIframe As New HtmlGenericControl("IFRAME")
      myIframe.Attributes.Add("style", "position: absolute;z-index:-1;border: none;margin: 0px 0px 0px 0px; background-color:Transparent; height: 180px; width: 200px;")
      myIframe.Attributes.Add("src", "javascript:false;")
      myIframe.Attributes.Add("frameBorder", "0")
      myIframe.Attributes.Add("scrolling", "no")

      myCalendar.Attributes.Add("style", "background-color: white; border:none; padding:0px; position: absolute; top: 158px; left: 308px; z-Index: 100;  height: 180px; width: 200px;")
      Calendar1.Attributes.Add("style", "background-color: white; border:solid 1px #90b0E0; padding:0px; position: absolute; top: 0px; left: 0px; z-Index: 100;")

      myIframe.Visible = True
      myCalendar.Controls.Add(myIframe)
    Else
      Calendar1.Visible = False
      myCalendar.Visible = False
    End If
    'AjaxCallHelper.WriteAlert("popup Calendar")

  End Sub

  Private Sub Calendar1_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Calendar1.SelectionChanged
    txtDays.Text = Calendar1.SelectedDate.ToString("yyyy/MM/dd")
    Calendar1.Visible = False
    myCalendar.Visible = False
  End Sub

  Private Sub Calendar1_VisibleMonthChanged(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MonthChangedEventArgs) Handles Calendar1.VisibleMonthChanged
    Calendar1.Visible = True
    myCalendar.Visible = True

    Dim myIframe As New HtmlGenericControl("IFRAME")
    myIframe.Attributes.Add("style", "position: absolute;z-index:-1;border: none;margin: 0px 0px 0px 0px; background-color:Transparent; height: 180px; width: 200px;")
    myIframe.Attributes.Add("src", "javascript:false;")
    myIframe.Attributes.Add("frameBorder", "0")
    myIframe.Attributes.Add("scrolling", "no")

    myCalendar.Controls.Add(myIframe)

  End Sub


  Private Sub ResetCalendarControls()
    Calendar1.Visible = False
    myCalendar.Visible = False
  End Sub

  Private Sub cboProduct_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboProduct.SelectedIndexChanged
    ResetCalendarControls()
  End Sub

  Private Sub cboRegLevel_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboRegLevel.SelectedIndexChanged
    ResetCalendarControls()
  End Sub



  Private Sub InitUI()
    ' Init Default license class
    cboLicenseType.SelectedIndex = 0
    Dim arrProdInfos() As ProductInfo
    Dim GeneratorInstance As _IALUGenerator
    Dim MyGen As New AlugenGlobals
    GeneratorInstance = MyGen.GeneratorInstance()
    GeneratorInstance.StoragePath = AppPath() & "\products.ini"

    arrProdInfos = GeneratorInstance.RetrieveProducts()
    If arrProdInfos.Length = 0 Then Exit Sub

    Dim i As Short
    For i = LBound(arrProdInfos) To UBound(arrProdInfos)
      PopulateUI(arrProdInfos(i))
    Next
  End Sub

  Private Sub PopulateUI(ByVal ProdInfo As ProductInfo)
    With ProdInfo
      AddRow(.name, .Version, .VCode, .GCode, False)
    End With
  End Sub

  Private Sub AddRow(ByRef Name As String, ByRef Ver As String, ByRef Code1 As String, ByRef Code2 As String, Optional ByRef fUpdateStore As Boolean = True)
    ' Add a Product Row to the GUI.
    ' If fUpdateStore is True, then product info is also saved to the store.

    ' Update the view
    Call CreateDataSource(Name, Ver, Code1, Code2)

    Dim ProdInfo As ProductInfo
    ProdInfo = ActiveLock3AlugenGlobals_definst.CreateProductInfo(Name, Ver, Code1, Code2)
    If fUpdateStore Then
      Call GeneratorInstance.SaveProduct(ProdInfo)
      grdProducts.DataSource = dt
      grdProducts.DataBind()
      grdProducts.SelectedIndex = dt.Rows.Count - 1
    End If
    If Not MagicAjax.MagicAjaxContext.Current.IsAjaxCall Then
      cboProduct.Items.Add(New ListItem(Name & " - " & Ver, Name & "|" & Ver))
    End If
  End Sub

  Private Sub txtProductVersion_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtProductVersion.TextChanged
    '  
  End Sub

  Private Sub txtProductName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtProductName.TextChanged
    '
  End Sub

  Private Sub txtInstallCode_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtInstallCode.TextChanged
    'extract username
    If txtInstallCode.Text.Length > 0 Then
      txtUserName.Text = GetUserFromInstallCode(txtInstallCode.Text)
      cmdGenerateLicenseKey.Enabled = True
    Else
      txtUserName.Text = ""
      cmdGenerateLicenseKey.Enabled = False
    End If
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

    a = Split(strInstCode, vbLf)
    GetUserFromInstallCode = a(a.Length - 1)

  End Function

  Private Sub loadRegisteredLevels()
    'load RegisteredLevels
    If Not File.Exists(strRegisteredLevelDBName) Then
      With cboRegLevel
        .Items.Clear()
        .Items.Add(New ListItem("Limited A", "0"))
        .Items.Add(New ListItem("Limited B", "1"))
        .Items.Add(New ListItem("Limited C", "2"))
        .Items.Add(New ListItem("Limited D", "3"))
        .Items.Add(New ListItem("Limited E", "4"))
        .Items.Add(New ListItem("No Print/Save", "5"))
        .Items.Add(New ListItem("Educational A", "6"))
        .Items.Add(New ListItem("Educational B", "7"))
        .Items.Add(New ListItem("Educational C", "8"))
        .Items.Add(New ListItem("Educational D", "9"))
        .Items.Add(New ListItem("Educational E", "10"))
        .Items.Add(New ListItem("Level 1", "11"))
        .Items.Add(New ListItem("Level 2", "12"))
        .Items.Add(New ListItem("Level 3", "13"))
        .Items.Add(New ListItem("Level 4", "14"))
        .Items.Add(New ListItem("Light Version", "15"))
        .Items.Add(New ListItem("Pro Version", "16"))
        .Items.Add(New ListItem("Enterprise Version", "17"))
        .Items.Add(New ListItem("Demo Only", "18"))
        .Items.Add(New ListItem("Full Version-Europe", "19"))
        .Items.Add(New ListItem("Full Version-Africa", "20"))
        .Items.Add(New ListItem("Full Version-Asia", "21"))
        .Items.Add(New ListItem("Full Version-USA", "22"))
        .Items.Add(New ListItem("Full Version-International", "23"))
        .SelectedIndex = 0
        SaveComboBox(strRegisteredLevelDBName, cboRegLevel, True)
      End With
    Else
      LoadComboBox(strRegisteredLevelDBName, cboRegLevel, True)
    End If
  End Sub

  Private Sub cmdGenerateLicenseKey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdGenerateLicenseKey.Click
    'generate license key
    Try
      Dim strName, strVer As String
      Dim itemProduct As ListItem = cboProduct.SelectedItem
      Dim a As String()

      a = itemProduct.Value.Split("|")
      strName = a(0)
      strVer = a(1)

      ActiveLock = ActiveLock3Globals_definst.NewInstance()
      With ActiveLock
        .SoftwareName = strName
        .SoftwareVersion = strVer
      End With

      Dim Generator As New AlugenGlobals
      GeneratorInstance = Generator.GeneratorInstance()
      GeneratorInstance.StoragePath = AppPath() & "\products.ini"

      Dim varLicType As ProductLicense.ALLicType

      If cboLicenseType.SelectedValue = "1" Then 'Periodic
        varLicType = ProductLicense.ALLicType.allicPeriodic
      ElseIf cboLicenseType.SelectedValue = "2" Then 'Permanent
        varLicType = ProductLicense.ALLicType.allicPermanent
      ElseIf cboLicenseType.SelectedValue = "0" Then 'Time Locked
        varLicType = ProductLicense.ALLicType.allicTimeLocked
      Else
        varLicType = ProductLicense.ALLicType.allicNone
      End If

      Dim strExpire As String
      strExpire = GetExpirationDate()
      Dim strRegDate As String
      strRegDate = Now.UtcNow.ToString("yyyy/MM/dd")

      Dim Lic As ProductLicense
      'generate license object
      Dim selRegLevel As ListItem = cboRegLevel.SelectedItem
      Lic = ActiveLock3Globals_definst.CreateProductLicense(strName, strVer, "", _
                ProductLicense.LicFlags.alfSingle, varLicType, "", _
                IIf(chkUseItemData.Checked, _
                  selRegLevel.Value, selRegLevel.Text), _
                strExpire, , strRegDate)

      Dim strLibKey As String
      ' Pass it to IALUGenerator to generate the key
      Dim selectedRegisteredLevel As String
      Dim mList As ListItem
      mList = cboRegLevel.SelectedItem
      If chkUseItemData.Checked Then
        selectedRegisteredLevel = mList.Value
      Else
        selectedRegisteredLevel = mList.Text
      End If

      strLibKey = GeneratorInstance.GenKey(Lic, txtInstallCode.Text, selectedRegisteredLevel)
      'split license key into 64byte chunks
      txtLicenseKey.Text = Make64ByteChunks(strLibKey & "aLck" & txtInstallCode.Text)
      'save the license to local server file - for use in save
      SaveLicenseKey(txtLicenseKey.Text, Session.SessionID.ToString & ".all") '&& "ASPNETAlugen3.all"

      SayAjax("License code generated successfuly!")
    Catch ex As Exception
      SayAjax("Error: " & ex.Message)
    Finally
    End Try
  End Sub

  Private Function GetExpirationDate() As String
    If cboLicenseType.SelectedValue = "0" Then 'Time Locked
      If txtDays.Text.Trim.Length = 0 Then txtDays.Text = Now.UtcNow.AddDays(30).ToString("yyyy/MM/dd")
      GetExpirationDate = CType(txtDays.Text, DateTime).ToString("yyyy/MM/dd")
    Else
      If txtDays.Text.Trim.Length = 0 Then txtDays.Text = "30"
      GetExpirationDate = Now.UtcNow.AddDays(CShort(txtDays.Text)).ToString("yyyy/MM/dd")
    End If
  End Function

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

  Private Sub cmdEmailLicenseKey_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdEmailLicenseKey.Click
    Dim mailToString As String
    Dim emailAddress As String = "user@company.com"
    Dim strSubject As String
    Dim strBodyMessage As String
    Dim strNewLine As String = "%0D%0A"

    strSubject = String.Format("License key for application {0}, user [{1}]", cboProduct.SelectedItem.Text, txtUserName.Text)
    strBodyMessage = strNewLine & String.Format("Install code:" & strNewLine & "{0}", txtInstallCode.Text)
    strBodyMessage = strBodyMessage & strNewLine & strNewLine & String.Format("License key:" & strNewLine & "{0}", txtLicenseKey.Text.Replace(Chr(13), strNewLine).Replace(Chr(10), ""))

    'final constructor
    mailToString = String.Format("mailto:{0}?subject={1}&body={2}", emailAddress, strSubject, strBodyMessage)

    AjaxCallHelper.Redirect(mailToString)
  End Sub

  Private Sub cmdSaveLicenseFile_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdSaveLicenseFile.Click

    DownloadFile(AppPath() & "\" & Session.SessionID.ToString & ".all", True)  '"\ASPNETAlugen3.all"

  End Sub

  Private Sub DownloadFile(ByVal fname As String, ByVal forceDownload As Boolean)
    Dim path As Path
    Dim fullpath = path.GetFullPath(fname)
    Dim name = path.GetFileName(fullpath)
    Dim ext = path.GetExtension(fullpath)
    Dim type As String = ""

    If Not IsDBNull(ext) Then
      ext = LCase(ext)
    End If

    Select Case ext
      Case ".htm", ".html"
        type = "text/HTML"
      Case ".txt"
        type = "text/plain"
      Case ".doc", ".rtf"
        type = "Application/msword"
      Case ".csv", ".xls"
        type = "Application/x-msexcel"
      Case Else
        type = "text/plain"
    End Select

    HttpContext.Current.Response.Clear()
    HttpContext.Current.Response.ClearHeaders()
    HttpContext.Current.Response.ClearContent()

    Response.ContentType = "application\octet-stream" '"application/zip"

    'If type <> "" Then
    '  Response.ContentType = type
    'End If
    If (forceDownload) Then
      Response.AppendHeader("content-disposition", _
      "attachment; filename=ASPNETAlugen3.all") ' + name
    End If

    Response.WriteFile(fullpath)
    Response.End()
  End Sub

  Private Sub SaveLicenseKey(ByVal sLibKey As String, ByVal sFileName As String)
    Dim fp As StreamWriter
    Try
      fp = File.CreateText(Server.MapPath(".\") & sFileName)
      fp.WriteLine(sLibKey)
      fp.Close()
    Catch err As Exception
      SayAjax(err.Message)
    Finally
    End Try
  End Sub

End Class
