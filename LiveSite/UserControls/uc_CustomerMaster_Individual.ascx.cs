﻿#region Namespace
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Idv.Chetana.BAL;
using System.Data.SqlClient;
using System.Text;
using System.Xml;
using System.IO;
using System.Data.OleDb;

#endregion


public partial class UserControls_uc_CustomerMaster_Individual : System.Web.UI.UserControl
{
    #region Variables
    int CID;
    string flag = "state";

    #endregion

    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {
        if (ChkBlacklist.Checked == true)
        {
            lblblkremark.Visible = true;
            LblblkDate.Visible = true;
            TxtblkRemark.Visible = true;
            TxtblkDate.Visible = true;
        }
        else
        {
            lblblkremark.Visible = false;
            LblblkDate.Visible = false;
            TxtblkRemark.Visible = false;
            TxtblkDate.Visible = false;

        }
        if (!Page.IsPostBack)
        {
            btnAddAccess.Enabled = false;
            txtBookType.Enabled = false;
            txtFromQty.Enabled = false;
            txtToQty.Enabled = false;
            txtDiscount.Enabled = false;

            DdlSection.Enabled = false;
            DdlSection.DataSource = SectionMaster.Get_SectionList();
            DdlSection.DataBind();
            LblCustId.Text = "0";
            fillzones();
            TxtCustCode.Focus();
            custRating();
            getDDLdata();
            getDDLarezo();
            getDDLarea();
            GetSBUCode();
            //FillBoard();
            btnSave.Visible = false;
            PnlAdd.Visible = false;
            SetView();
            Session["TempDataTable"] = null;
            Session["TempAssorted"] = null;
            checkAction.Checked = true;


        }

        if (ChkBlacklist.Checked == true)
        {
            lblblkremark.Visible = true;
            LblblkDate.Visible = true;
            TxtblkRemark.Visible = true;
            TxtblkDate.Visible = true;
        }
        else
        {
            lblblkremark.Visible = false;
            LblblkDate.Visible = false;
            TxtblkRemark.Visible = false;
            TxtblkDate.Visible = false;

        }
    }
    #endregion

    #region Excel Upload

    //private DataTable ConvertExcelToDataTable()
    //{
    //    try
    //    {

    //    }
    //    catch (Exception ex)
    //    {
    //        MessageBox(ex.ToString());
    //    }
    //}

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {
            if (FileUpload1.HasFile)
            {
               
                string connString = "";
                string fileName = System.IO.Path.GetFileName(FileUpload1.PostedFile.FileName);
                FileUpload1.PostedFile.SaveAs(Server.MapPath("~/Uploads/" + fileName));
                string strFileType = Path.GetExtension(FileUpload1.FileName).ToLower();
                string path = Server.MapPath("~/Uploads/" + fileName);
                if (strFileType.Trim() == ".xls")
                {
                    connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";ReadOnly=1;Extended Properties=\"Excel 8.0;HDR=NO;IMEX=2\"";
                }
                else if (strFileType.Trim() == ".xlsx")
                {
                    // connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";ReadOnly=1;Extended Properties=\"Excel 12.0;HDR=NO;IMEX=2\"";
                    connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + path + "';Extended Properties=\"Excel 12.0 Xml;HDR=NO;IMEX=1;\"";
                }
                System.Data.OleDb.OleDbConnection MyConnection;
                System.Data.DataSet DtSet;
                System.Data.OleDb.OleDbDataAdapter MyCommand;
                //MyConnection = new System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + path + "';ReadOnly=0;Extended Properties=Excel 8.0;");
                MyConnection = new System.Data.OleDb.OleDbConnection(connString);
                MyCommand = new System.Data.OleDb.OleDbDataAdapter("select * from [Sheet1$]", MyConnection);
               // MyCommand.TableMappings.Add("Table", "TestTable");
                DtSet = new System.Data.DataSet();
                MyCommand.Fill(DtSet);
                MyConnection.Close();
            }
        }
        catch (IOException ex)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw;
        }

    }

    #endregion

    private void AddOtherFields(string fieldname, string value, StringBuilder strbldr)
    {

        strbldr.Append("<" + fieldname + ">" + value + "</" + fieldname + ">");

    }

    #region Save Customer

    protected void btnSave_Click(object sender, EventArgs e)
    {

        // DdlSection.Text = "----";

        try
        {
            //string Area = TxtArea.Text.Split(':')[0].Trim();
            Other_Z.Customer_cs_rev _objCust = new Other_Z.Customer_cs_rev();
            _objCust.CustID = Convert.ToInt32(LblCustId.Text);
            _objCust.CustCode = TxtCustCode.Text.Trim();
            _objCust.ShortForm = TxtShortForm.Text.Trim();
            _objCust.FamilyCode = TxtFamilyCode.Text.Trim();
            _objCust.Address = TxtAddress.Text.Trim();
            _objCust.Zip = TxtZip.Text.Trim();
            _objCust.Phone1 = TxtPhone1.Text.Trim();
            _objCust.Phone2 = TxtPhone2.Text.Trim();
            _objCust.EmailID = TxtEmailID.Text.Trim();
            _objCust.CUSTOMERTYPE = TxtCustomerType.Text.Trim();
            _objCust.IsDeleted = false;
            _objCust.IsActive = true;
            _objCust.CreatedBy = Convert.ToString(Session["UserName"]);
            _objCust.CustName = TxtCustName.Text.Trim();
            _objCust.ZoneID = Convert.ToInt32(DDLzone.SelectedValue);
            _objCust.SuperZoneID = Convert.ToInt32(DDLsuperzone.SelectedValue);
            _objCust.AreaZoneID = Convert.ToInt32(DDLareazone.SelectedValue);
            _objCust.AreaID = Convert.ToInt32(DDLarea.SelectedValue);
            _objCust.DMID = Convert.ToInt32(ddLStates.SelectedValue);
            _objCust.City = Convert.ToInt32(ddlCity.SelectedValue);
            _objCust.CMID = Convert.ToInt32(DDLCC.SelectedValue);
            _objCust.CMIDsub = Convert.ToInt32(DDLCSC.SelectedValue);
            _objCust.SchAdditionalDis = txtSchAdditionalDis.Text.Trim();
            _objCust.TODValue1 = txtTODValue1.Text.Trim();
            _objCust.TODValue2 = txtTODValue2.Text.Trim();
            _objCust.TODValue3 = txtTODValue3.Text.Trim();
            _objCust.TODDisc1 = txtTODDisc1.Text.Trim();
            _objCust.TODDisc2 = txtTODDisc2.Text.Trim();
            _objCust.TODDisc3 = txtTODDisc3.Text.Trim();
            _objCust.CustRating = Convert.ToInt32(DdlCustRating.SelectedValue);
            _objCust.isSplit = chk_splitdc.Checked;
            //StringBuilder stbr = new StringBuilder("<Root>");
            //AddOtherFields("CUL", txtUpperlimit.Text, stbr);
            //AddOtherFields("CLL", txtLowerlimit.Text, stbr);
            //AddOtherFields("SBUCode", ddlSbucode.Text, stbr);
            //_objCust.OtherFields = stbr.Append("</Root>").ToString();

            #region XML Customer TOD Table Update
            XmlDocument doc = new XmlDocument();
            XmlNode node1 = doc.CreateElement("r");

            XmlNode nd = doc.CreateElement("CUL");
            nd.InnerText = txtUpperlimit.Text;
            node1.AppendChild(nd);

            nd = doc.CreateElement("CLL");
            nd.InnerText = txtLowerlimit.Text;
            node1.AppendChild(nd);

            nd = doc.CreateElement("SBUCode");
            nd.InnerText = ddlSbucode.Text;
            node1.AppendChild(nd);

            nd = doc.CreateElement("P");
            nd.InnerText = txtPAN.Text;
            node1.AppendChild(nd);

            XmlNode node = doc.CreateElement("r1");
            foreach (GridViewRow item in TODGridview.Rows)
            {
                XmlNode element = doc.CreateElement("t");

                nd = doc.CreateElement("i");
                nd.InnerText = ((TextBox)item.FindControl("lblTodAmount")).Text.Trim();
                element.AppendChild(nd);

                nd = doc.CreateElement("j");
                nd.InnerText = ((TextBox)item.FindControl("lblTodPercentage")).Text.Trim();
                element.AppendChild(nd);

                nd = doc.CreateElement("a");
                nd.InnerText = Convert.ToInt32(((CheckBox)item.FindControl("lblAction")).Checked).ToString();
                element.AppendChild(nd);

                nd = doc.CreateElement("f");
                nd.InnerText = Convert.ToInt32(Session["FY"]).ToString();
                element.AppendChild(nd);

                nd = doc.CreateElement("au");
                nd.InnerText = Convert.ToInt32(((Label)item.FindControl("lblAutoId")).Text).ToString();
                element.AppendChild(nd);

                node.AppendChild(element);

            }
            foreach (GridViewRow item in GridAssorted.Rows)
            {
                XmlNode element2 = doc.CreateElement("ad");

                nd = doc.CreateElement("b");
                nd.InnerText = ((Label)item.FindControl("bookId")).Text.Trim();
                element2.AppendChild(nd);

                nd = doc.CreateElement("fr");
                nd.InnerText = ((TextBox)item.FindControl("lblFromQty")).Text.Trim();
                element2.AppendChild(nd);

                nd = doc.CreateElement("to");
                nd.InnerText = ((TextBox)item.FindControl("lblToQty")).Text.Trim();
                element2.AppendChild(nd);

                nd = doc.CreateElement("di");
                nd.InnerText = ((TextBox)item.FindControl("lblDiscount")).Text.Trim();
                element2.AppendChild(nd);

                nd = doc.CreateElement("Fin");
                nd.InnerText = Convert.ToInt32(Session["FY"]).ToString();
                element2.AppendChild(nd);

                nd = doc.CreateElement("aid");
                nd.InnerText = Convert.ToInt32(((Label)item.FindControl("lblAutoIdAssored")).Text).ToString();
                element2.AppendChild(nd);

                node.AppendChild(element2);

            }
            node1.AppendChild(node);
            _objCust.OtherFields = node1.OuterXml.ToString();
            #endregion

            string TxtPrincipalDOB1 = "";
            string TxtKeyPersonDOB1 = "";

            try
            {
                TxtPrincipalDOB1 = TxtPrincipalDOB.Text.Split('/')[1] + "/" + TxtPrincipalDOB.Text.Split('/')[0] + "/" + TxtPrincipalDOB.Text.Split('/')[2];
                TxtKeyPersonDOB1 = TxtKeyPersonDOB.Text.Split('/')[1] + "/" + TxtKeyPersonDOB.Text.Split('/')[0] + "/" + TxtKeyPersonDOB.Text.Split('/')[2];
            }
            catch
            {


            }

            if (LblCustId.Text == "0")
            {
                _objCust.Save(out CID);
                // TxtCustId.Text = Convert.ToString(CID);
                string Medium = TxtMedium.Text.Split(':')[0].Trim();
                CustomerDetails _objCustDetails = new CustomerDetails();
                _objCustDetails.CustId = CID;
                _objCustDetails.CustCode = TxtCustCode.Text.Trim();
                _objCustDetails.CreditLimit = TxtCreditLimit.Text.Trim();
                // _objCustDetails.Creditdays = txtcreditdays.Text.Trim();
                _objCustDetails.BlackList = ChkBlacklist.Checked;
                _objCustDetails.CreatedBy = Convert.ToString(Session["UserName"]);
                _objCustDetails.PrincipalName = TxtPrincipalName.Text.Trim();
                _objCustDetails.PrincipalMobile = TxtPrincipalMobile.Text.Trim();
                _objCustDetails.PrincipalDOB = TxtPrincipalDOB1;
                _objCustDetails.KeyPersonName = TxtKeyPersonName.Text.Trim();
                _objCustDetails.KeyPersonMobile = TxtKeyPersonMobile.Text.Trim();
                _objCustDetails.KeyPersonDOB = TxtKeyPersonDOB.Text.Trim();
                _objCustDetails.AdditinalDis = TxtAdditinalDis.Text.Trim();
                _objCustDetails.VIPRemark = TxtVIPRemark.Text.Trim();
                _objCustDetails.SectionID = 0;
                //_objCustDetails.SectionID = Convert.ToInt32(DdlSection.SelectedValue);
                _objCustDetails.Medium = Medium;

                //_objCustDetails.BoardID = Convert.ToInt32(DDLBoard.SelectedValue.ToString());
                if (txtcgp.Text.Trim().ToString() != "")
                {
                    _objCustDetails.CGP = Convert.ToDecimal(txtcgp.Text.Trim().ToString());
                }
                else { _objCustDetails.CGP = 0; }

                if (txtbuisiness.Text.Trim().ToString() != "")
                {
                    _objCustDetails.Business_Potential = Convert.ToDecimal(txtbuisiness.Text.Trim().ToString());
                }
                else { _objCustDetails.Business_Potential = 0; }
                _objCustDetails.Association = txtassociation.Text.Trim().ToString();
                _objCustDetails.Payment_Track = "";

                _objCustDetails.Save();
                grdCustDetails.Visible = true;
                PnlDetails.Visible = true;
                MessageBox("Record saved successfully");
                if (btnSave.Text == "Update")
                {
                    PnlDetails.Visible = true;
                    PnlAdd.Visible = false;
                    btnSave.Visible = false;
                    filter.Visible = true;


                }
                else
                {
                    PnlDetails.Visible = false;
                    PnlAdd.Visible = true;
                    btnSave.Visible = true;

                }
            }
            else
            {
                string Medium = TxtMedium.Text.Split(':')[0].Trim();
                _objCust.CustID = Convert.ToInt32(LblCustId.Text);
                _objCust.CustDetailID = Convert.ToInt32(LblCustDetailID1.Text);
                if (TxtCreditLimit.Text == "")
                {
                    TxtCreditLimit.Text = "0";
                }
                _objCust.CreditLimit = Convert.ToInt32(TxtCreditLimit.Text.Trim());
                _objCust.CreditDays = txtcreditdays.Text.Trim();
                _objCust.BlackList = ChkBlacklist.Checked;
                _objCust.IsDeleted1 = false;
                _objCust.PrincipalName = TxtPrincipalName.Text.Trim();
                _objCust.PrincipalMobile = TxtPrincipalMobile.Text.Trim();
                _objCust.PrincipalDOB = TxtPrincipalDOB.Text.Trim();
                _objCust.KeyPersonName = TxtKeyPersonName.Text.Trim();
                _objCust.KeyPersonMobile = TxtKeyPersonMobile.Text.Trim();
                _objCust.KeyPersonDOB = TxtKeyPersonDOB.Text.Trim();
                _objCust.AdditinalDis = TxtAdditinalDis.Text.Trim();
                _objCust.VIPRemark = TxtVIPRemark.Text.Trim();
                _objCust.BlackListRemark = TxtblkRemark.Text.Trim();
                _objCust.BlackListDate = TxtblkDate.Text.Trim();

                //_objCust.SectionID = Convert.ToInt32(DdlSection.SelectedValue);
                _objCust.SectionID = 0;
                _objCust.Medium = Medium;

                //_objCust.BoardID = Convert.ToInt32(DDLBoard.SelectedValue.ToString());

                if (txtcgp.Text.Trim().ToString() != "")
                {
                    _objCust.CGP = Convert.ToDecimal(txtcgp.Text.Trim().ToString());
                }
                else { _objCust.CGP = 0; }

                if (txtbuisiness.Text.Trim().ToString() != "")
                {
                    _objCust.Business_Potential = Convert.ToDecimal(txtbuisiness.Text.Trim().ToString());
                }
                else { _objCust.Business_Potential = 0; }
                _objCust.Association = txtassociation.Text.Trim().ToString();
                _objCust.Payment_Track = "";

                _objCust.Update();
                //grdCustDetails.DataSource = BindGvCustDetail();
                //grdCustDetails.DataBind();

                MessageBox("Record updated successfully");
                TxtCustCode.Enabled = false;
                NewPanel.Visible = false;
                AssortedPanel.Visible = false;
                if (btnSave.Text.ToLower() == "update")
                {
                    // PnlDetails.Visible = true;
                    //PnlAdd.Visible = false;
                    btnSave.Visible = false;
                    // filter.Visible = true;
                    lblCustName.Text = "";
                    TxtCustCode.Focus();
                    TxtCustCode.Text = "";

                }
                else
                {
                    // PnlDetails.Visible = false;
                    // PnlAdd.Visible = true;
                    btnSave.Visible = true;

                }

            }
            TxtCustCode.Text = "";
            TxtShortForm.Text = "";
            TxtFamilyCode.Text = "";
            TxtAddress.Text = "";
            TxtZip.Text = "";
            TxtPhone1.Text = "";
            TxtPhone2.Text = "";
            TxtEmailID.Text = "";
            TxtCustName.Text = "";
            TxtCreditLimit.Text = "";
            TxtPrincipalName.Text = "";
            TxtPrincipalMobile.Text = "";
            TxtPrincipalDOB.Text = "";
            TxtKeyPersonName.Text = "";
            TxtKeyPersonMobile.Text = "";
            TxtKeyPersonDOB.Text = "";
            TxtAdditinalDis.Text = "";
            TxtVIPRemark.Text = "";
            TxtMedium.Text = "";
            TxtCustomerType.Text = "";
            DDLzone.SelectedValue = "0";
            DDLareazone.SelectedValue = "0";
            DDLarea.SelectedValue = "0";
            ddLStates.SelectedValue = "0";
            DDLsuperzone.SelectedValue = "0";
            DdlCustRating.SelectedValue = "0";

            txtFromQty.Text = "";
            txtDiscount.Text = "";
            txtToQty.Text = "";

            //DDLBoard.SelectedValue = "0";
            txtassociation.Text = "";
            txtbuisiness.Text = "";
            txtcgp.Text = "";

            btnSave.Visible = false;
            PnlAdd.Visible = false;
        }
        catch (SqlException ex)
        {
            Response.Write(ex.Message.ToString());
        }
        catch (Exception ex1)
        {
            Response.Write(ex1.Message.ToString());
        }

    }

    #endregion

    public void ClearAll()
    {
        TxtCustCode.Text = "";
        TxtShortForm.Text = "";
        TxtFamilyCode.Text = "";
        TxtAddress.Text = "";
        TxtZip.Text = "";
        TxtPhone1.Text = "";
        TxtPhone2.Text = "";
        TxtEmailID.Text = "";
        TxtCustName.Text = "";
        TxtCreditLimit.Text = "";
        TxtPrincipalName.Text = "";
        TxtPrincipalMobile.Text = "";
        TxtPrincipalDOB.Text = "";
        TxtKeyPersonName.Text = "";
        TxtKeyPersonMobile.Text = "";
        TxtKeyPersonDOB.Text = "";
        TxtAdditinalDis.Text = "";
        TxtVIPRemark.Text = "";
        TxtMedium.Text = "";
        TxtCustomerType.Text = "";
        DDLzone.SelectedIndex = 0;
        DDLareazone.SelectedIndex = 0;
        DDLarea.SelectedIndex = 0;
        ddLStates.SelectedIndex = 0;
        DDLsuperzone.SelectedIndex = 0;
        DdlCustRating.SelectedIndex = 0;
        ddlCity.SelectedIndex = 0;
        txtFromQty.Text = "";
        txtDiscount.Text = "";
        txtToQty.Text = "";
        btnSave.Visible = false;
        PnlAdd.Visible = false;
    }


    #region  Button Event
    protected void btncancel_Click(object sender, EventArgs e)
    {
        PnlAdd.Visible = false;
        PnlDetails.Visible = true;
    }
    protected void BtnAdd_Click(object sender, EventArgs e)
    {
        LblCustId.Text = "0";
        PnlDetails.Visible = false;
        PnlAdd.Visible = true;
    }

    #endregion

    #region MsgBox
    public void MessageBox(string msg)
    {
        string jv = "alert('" + msg + "');";
        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "msg", jv, true);
    }
    #endregion


    #region Binddata method

    public DataTable BindGvCustDetail(string CustCode)
    {
        DataTable dt = new DataTable();
        dt = Customer_cs.Get_CustList("", CustCode);
        return dt;
    }


    #endregion


    #region Gridview Events
    protected void grdCustDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        //bool _objct = new CustomerToTransport().isDelete("customer", (((Label)grdCustDetails.Rows[e.RowIndex].FindControl("lblCustID")).Text));

        //if (_objct)
        //{
        //    Customer_cs _objCM = new Customer_cs();
        //    _objCM.CustID = Convert.ToInt32(((Label)grdCustDetails.Rows[e.RowIndex].FindControl("lblCustID")).Text);

        //    _objCM.CustCode = ((Label)grdCustDetails.Rows[e.RowIndex].FindControl("lblCustCode")).Text;
        //    _objCM.CustName = ((Label)grdCustDetails.Rows[e.RowIndex].FindControl("lblCustName")).Text;

        //    _objCM.IsActive = Convert.ToBoolean(false);
        //    _objCM.IsDeleted = Convert.ToBoolean(true);
        //    _objCM.IsDeleted1 = Convert.ToBoolean(true);
        //    try
        //    {
        //        _objCM.Update();
        //        MessageBox("Your record is Deleted");
        //        PnlDetails.Visible = true;
        //        PnlAdd.Visible = false;
        //    }
        //    catch
        //    {

        //    }

        //}
        //else
        //{
        //    MessageBox("You Can not Delete This Customer");
        //}
    }
    protected void grdCustDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

        //grdCustDetails.PageIndex = e.NewPageIndex;
        //grdCustDetails.DataSource = BindGvCustDetail();
        //grdCustDetails.DataBind();
    }

    protected void grdCustDetails_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {
            PnlDetails.Visible = false;
            PnlAdd.Visible = true;
            grdCustDetails.Visible = true;
            PnlDetails.Visible = true;

            LblCustId.Text = ((Label)grdCustDetails.Rows[e.NewSelectedIndex].FindControl("lblCustID")).Text;

            TxtCustCode.Text = ((Label)grdCustDetails.Rows[e.NewSelectedIndex].FindControl("lblCustCode")).Text;
            TxtShortForm.Text = ((Label)grdCustDetails.Rows[e.NewSelectedIndex].FindControl("lblShortForm")).Text;
            TxtFamilyCode.Text = ((Label)grdCustDetails.Rows[e.NewSelectedIndex].FindControl("lblFamilyCode")).Text;
            TxtAddress.Text = ((Label)grdCustDetails.Rows[e.NewSelectedIndex].FindControl("lblAddress")).Text;
            //TxtCity.Text = ((Label)grdCustDetails.Rows[e.NewSelectedIndex].FindControl("lblCity ")).Text;
            TxtZip.Text = ((Label)grdCustDetails.Rows[e.NewSelectedIndex].FindControl("lblZip")).Text;
            TxtPhone1.Text = ((Label)grdCustDetails.Rows[e.NewSelectedIndex].FindControl("lblPhone1")).Text;
            TxtPhone2.Text = ((Label)grdCustDetails.Rows[e.NewSelectedIndex].FindControl("lblPhone2")).Text;
            TxtEmailID.Text = ((Label)grdCustDetails.Rows[e.NewSelectedIndex].FindControl("lblEmailID")).Text;
            TxtCustName.Text = ((Label)grdCustDetails.Rows[e.NewSelectedIndex].FindControl("lblCustName")).Text;
            TxtCreditLimit.Text = ((Label)grdCustDetails.Rows[e.NewSelectedIndex].FindControl("lblCreditLimit")).Text;
            TxtPrincipalName.Text = ((Label)grdCustDetails.Rows[e.NewSelectedIndex].FindControl("LblPrincipalName")).Text;
            TxtPrincipalMobile.Text = ((Label)grdCustDetails.Rows[e.NewSelectedIndex].FindControl("LblPrincipalMobile")).Text;
            TxtPrincipalDOB.Text = ((Label)grdCustDetails.Rows[e.NewSelectedIndex].FindControl("LblPrincipalDOB")).Text;
            TxtKeyPersonName.Text = ((Label)grdCustDetails.Rows[e.NewSelectedIndex].FindControl("LblKeyPersonName")).Text;
            TxtKeyPersonMobile.Text = ((Label)grdCustDetails.Rows[e.NewSelectedIndex].FindControl("LblKeyPersonMobile")).Text;
            TxtKeyPersonDOB.Text = ((Label)grdCustDetails.Rows[e.NewSelectedIndex].FindControl("LblKeyPersonDOB")).Text;
            TxtAdditinalDis.Text = ((Label)grdCustDetails.Rows[e.NewSelectedIndex].FindControl("LblAdditinalDis")).Text;
            TxtVIPRemark.Text = ((Label)grdCustDetails.Rows[e.NewSelectedIndex].FindControl("LblVIPRemark")).Text;
            //lblcreditdays
            txtcreditdays.Text = ((Label)grdCustDetails.Rows[e.NewSelectedIndex].FindControl("lblcreditdays")).Text;
            TxtMedium.Text = ((Label)grdCustDetails.Rows[e.NewSelectedIndex].FindControl("LblMedium")).Text;
            //TxtBookTypeDetaildis.Text = ((Label)grdCustDetails.Rows[e.NewSelectedIndex].FindControl("LblBookTypeDetaildis")).Text;
            ChkIsActive.Checked = ((CheckBox)grdCustDetails.Rows[e.NewSelectedIndex].FindControl("chkisActive")).Checked;
            ChkBlacklist.Checked = ((CheckBox)grdCustDetails.Rows[e.NewSelectedIndex].FindControl("ChKBList")).Checked;

        }
        catch
        {


        }

    }
    protected void grdCustDetails_RowEditing(object sender, GridViewEditEventArgs e)
    {



    }

    public void FillForm()
    {
        try
        {
            DDLareazone.Enabled = true;
            DDLarea.Enabled = true;
            DDLzone.Enabled = true;
            btnSave.Visible = true;
            PnlAdd.Visible = true;
            btnSave.Text = "Update";
            btnSave.Enabled = true;
            LblCustId.Text = ((Label)grdCustDetails.Rows[0].FindControl("lblCustID")).Text;
            TxtCustCode.Text = ((Label)grdCustDetails.Rows[0].FindControl("lblCustCode")).Text;
            TxtShortForm.Text = ((Label)grdCustDetails.Rows[0].FindControl("lblShortForm")).Text;
            TxtFamilyCode.Text = ((Label)grdCustDetails.Rows[0].FindControl("lblFamilyCode")).Text;
            TxtAddress.Text = ((Label)grdCustDetails.Rows[0].FindControl("lblAddress")).Text;
            TxtZip.Text = ((Label)grdCustDetails.Rows[0].FindControl("lblZip")).Text;
            TxtPhone1.Text = ((Label)grdCustDetails.Rows[0].FindControl("lblPhone1")).Text;
            TxtPhone2.Text = ((Label)grdCustDetails.Rows[0].FindControl("lblPhone2")).Text;
            TxtEmailID.Text = ((Label)grdCustDetails.Rows[0].FindControl("lblEmailID")).Text;
            TxtCustName.Text = ((Label)grdCustDetails.Rows[0].FindControl("lblCustName")).Text;
            TxtCreditLimit.Text = ((Label)grdCustDetails.Rows[0].FindControl("lblCreditLimit")).Text;
            TxtPrincipalName.Text = ((Label)grdCustDetails.Rows[0].FindControl("LblPrincipalName")).Text;
            TxtPrincipalMobile.Text = ((Label)grdCustDetails.Rows[0].FindControl("LblPrincipalMobile")).Text;
            TxtPrincipalDOB.Text = ((Label)grdCustDetails.Rows[0].FindControl("LblPrincipalDOB")).Text;
            TxtKeyPersonName.Text = ((Label)grdCustDetails.Rows[0].FindControl("LblKeyPersonName")).Text;
            TxtKeyPersonMobile.Text = ((Label)grdCustDetails.Rows[0].FindControl("LblKeyPersonMobile")).Text;
            TxtKeyPersonDOB.Text = ((Label)grdCustDetails.Rows[0].FindControl("LblKeyPersonDOB")).Text;
            TxtAdditinalDis.Text = ((Label)grdCustDetails.Rows[0].FindControl("LblAdditinalDis")).Text;
            TxtVIPRemark.Text = ((Label)grdCustDetails.Rows[0].FindControl("LblVIPRemark")).Text;
            TxtMedium.Text = ((Label)grdCustDetails.Rows[0].FindControl("LblMedium")).Text;
            ChkIsActive.Checked = ((CheckBox)grdCustDetails.Rows[0].FindControl("chkisActive")).Checked;
            LblSuperzone.Text = ((Label)grdCustDetails.Rows[0].FindControl("LblSuperZoneID")).Text;
            Lblzone.Text = ((Label)grdCustDetails.Rows[0].FindControl("LblZoneID")).Text;
            LblAreazone.Text = ((Label)grdCustDetails.Rows[0].FindControl("LblAreaZoneID")).Text;
            LblArea.Text = ((Label)grdCustDetails.Rows[0].FindControl("LblAreaID")).Text;
            LblCustDetailID1.Text = ((Label)grdCustDetails.Rows[0].FindControl("LblCustDetailID")).Text;
            TxtCustomerType.Text = ((Label)grdCustDetails.Rows[0].FindControl("lblctype")).Text;
            txtcreditdays.Text = ((Label)grdCustDetails.Rows[0].FindControl("lblcreditdays")).Text;
            lblrating.Text = ((Label)grdCustDetails.Rows[0].FindControl("lblcustrating")).Text;
            txtSchAdditionalDis.Text = ((Label)grdCustDetails.Rows[0].FindControl("lblSchAdditionalDis")).Text;
            txtTODValue1.Text = ((Label)grdCustDetails.Rows[0].FindControl("lblTODValue1")).Text;
            txtTODValue2.Text = ((Label)grdCustDetails.Rows[0].FindControl("lblTODValue2")).Text;
            txtTODValue3.Text = ((Label)grdCustDetails.Rows[0].FindControl("lblTODValue3")).Text;
            txtTODDisc1.Text = ((Label)grdCustDetails.Rows[0].FindControl("lblTODDisc1")).Text;
            txtTODDisc2.Text = ((Label)grdCustDetails.Rows[0].FindControl("lblTODDisc2")).Text;
            txtTODDisc3.Text = ((Label)grdCustDetails.Rows[0].FindControl("lblTODDisc3")).Text;
            TxtblkRemark.Text = ((Label)grdCustDetails.Rows[0].FindControl("Lblblkremark")).Text;
            TxtblkDate.Text = ((Label)grdCustDetails.Rows[0].FindControl("Lblblkdate")).Text;
            txtUpperlimit.Text = ((Label)grdCustDetails.Rows[0].FindControl("lblCUL")).Text;
            txtLowerlimit.Text = ((Label)grdCustDetails.Rows[0].FindControl("lblCLL")).Text;
            lblSBUCodeNone.Text = ((Label)grdCustDetails.Rows[0].FindControl("lblSUBCode")).Text;
            txtPAN.Text = ((Label)grdCustDetails.Rows[0].FindControl("lblPANNO")).Text;
            chk_splitdc.Checked = ((CheckBox)grdCustDetails.Rows[0].FindControl("chk_isSplit")).Checked;
            try
            {
                DDLsuperzone.SelectedValue = LblSuperzone.Text.Trim();
                DDLzone.DataSource = Masters.Get_AreaZone_Zone_SuperZone(Convert.ToInt32(LblSuperzone.Text), "Zone");
                DDLzone.DataBind();
                DDLzone.Items.Insert(0, new ListItem("--Select Zone--", "0"));
                DDLzone.SelectedValue = Lblzone.Text.Trim();

                DDLareazone.DataSource = Masters.Get_AreaZone_Zone_SuperZone(Convert.ToInt32(Lblzone.Text), "AreaZone");
                DDLareazone.DataBind();
                DDLareazone.Items.Insert(0, new ListItem("--Select AreaZone--", "0"));
                DDLareazone.SelectedValue = LblAreazone.Text.Trim();

                DDLarea.DataSource = Masters.Get_AreaZone_Zone_SuperZone(Convert.ToInt32(LblAreazone.Text), "Area");
                DDLarea.DataBind();
                DDLarea.Items.Insert(0, new ListItem("--Select Area--", "0"));
                DDLarea.SelectedValue = LblArea.Text.Trim();
            }
            catch { }



            ddLStates.DataSource = Destination.GetDestination(flag);
            ddLStates.DataBind();
            ddLStates.Items.Insert(0, new ListItem("--Select State--", "0"));
            ddLStates.SelectedValue = ((Label)grdCustDetails.Rows[0].FindControl("lblstate1")).Text.Trim();

            ddlCity.DataSource = Destination.GetDestination(Convert.ToString(ddLStates.SelectedValue));
            ddlCity.DataBind();
            ddlCity.Items.Insert(0, new ListItem("--Select City--", "0"));
            ddlCity.SelectedValue = ((Label)grdCustDetails.Rows[0].FindControl("lblCity")).Text.Trim();

            DataView dv1 = new DataView(Masterofmaster.Get_MasterOfMaster_ByGroup("CustRating").Tables[0]);
            DdlCustRating.DataSource = dv1;
            DdlCustRating.DataBind();
            DdlCustRating.Items.Insert(0, new ListItem("--Select Rating--", "0"));
            DdlCustRating.SelectedValue = lblrating.Text.Trim();
            ChkBlacklist.Checked = ((CheckBox)grdCustDetails.Rows[0].FindControl("ChKBList")).Checked;

            //lblboardid.Text = ((Label)grdCustDetails.Rows[0].FindControl("lblboardid")).Text;
            //DDLBoard.DataSource = Masterofmaster.Get_MasterOfMaster_ByGroup("Board").Tables[0];
            //DDLBoard.DataBind();
            //DDLBoard.Items.Insert(0, new ListItem("--Select Board--", "0"));
            //DDLBoard.SelectedValue = ((Label)grdCustDetails.Rows[0].FindControl("lblboardid")).Text;
            txtassociation.Text = ((Label)grdCustDetails.Rows[0].FindControl("lblassociation")).Text;
            txtcgp.Text = ((Label)grdCustDetails.Rows[0].FindControl("lblcgp")).Text;
            txtbuisiness.Text = ((Label)grdCustDetails.Rows[0].FindControl("lblbuisiness")).Text;
            //Customer Category
            DDLCC.DataSource = CustCategory.GetCustomerCategoryMaster(flag);
            DDLCC.DataBind();
            DDLCC.Items.Insert(0, new ListItem("--Select Category--", "0"));
            DDLCC.SelectedValue = ((Label)grdCustDetails.Rows[0].FindControl("lblCMID")).Text.Trim();
            if (DDLCC.SelectedValue == "8")
            {
                btnAddAccess.Enabled = true;
                txtBookType.Enabled = true;
                txtFromQty.Enabled = true;
                txtToQty.Enabled = true;
                txtDiscount.Enabled = true;
            }
            else
            {
                btnAddAccess.Enabled = false;
                txtBookType.Enabled = false;
                txtFromQty.Enabled = false;
                txtToQty.Enabled = false;
                txtDiscount.Enabled = false;
            }


            DDLCSC.DataSource = CustCategory.GetCustomerCategoryMaster(Convert.ToString(DDLCC.SelectedValue));
            DDLCSC.DataBind();
            DDLCSC.Items.Insert(0, new ListItem("--Select Sub Category--", "0"));
            DDLCSC.SelectedValue = ((Label)grdCustDetails.Rows[0].FindControl("lblCMIDSUB")).Text.Trim();



        }
        catch (Exception ex)
        {


        }
    }

    #endregion

    #region text changed
    protected void TxtCustomerType_TextChanged(object sender, EventArgs e)
    {
        if (TxtCustomerType.Text == "Other")
        {
            DdlSection.Enabled = false;
        }
        else
        {
            DdlSection.Enabled = true;
        }
    }
    protected void TxtCustCode_TextChanged(object sender, EventArgs e)
    {
        bool Auth = Masters.GetCodeValidation("Customer", TxtCustCode.Text.Trim());
        if (Auth)
        {
            MessageBox("Customer Code already exist");
            TxtCustCode.Text = "";
            TxtCustCode.Focus();
        }
        else
        {
            TxtCustName.Focus();
        }
    }
    #endregion

    #region Selected Index Changed
    public void getDDLdata()
    {
        DDLzone.DataSource = Masters.Get_AreaZone_Zone_SuperZone(Convert.ToInt32(DDLsuperzone.SelectedValue.ToString()), "Zone");
        DDLzone.DataBind();
        DDLzone.Items.Insert(0, new ListItem("--Select Zone--", "0"));
        DDLzone.Enabled = true;
        DDLzone.Focus();
    }
    public void getDDLarezo()
    {
        DDLareazone.DataSource = Masters.Get_AreaZone_Zone_SuperZone(Convert.ToInt32(DDLzone.SelectedValue.ToString()), "AreaZone");
        DDLareazone.DataBind();
        DDLareazone.Items.Insert(0, new ListItem("--Select AreaZone--", "0"));
        DDLareazone.Enabled = true;
        DDLareazone.Focus();
    }
    public void getDDLarea()
    {
        DDLarea.DataSource = Masters.Get_AreaZone_Zone_SuperZone(Convert.ToInt32(DDLareazone.SelectedValue.ToString()), "Area");
        DDLarea.DataBind();
        DDLarea.Items.Insert(0, new ListItem("--Select Area--", "0"));
        DDLarea.Enabled = true;
        DDLarea.Focus();
    }
    protected void DDLsuperzone_SelectedIndexChanged(object sender, EventArgs e)
    {
        getDDLdata();
    }

    protected void DDLzone_SelectedIndexChanged(object sender, EventArgs e)
    {
        getDDLarezo();
    }
    protected void DDLareazone_SelectedIndexChanged(object sender, EventArgs e)
    {
        getDDLarea();
    }
    private void GetSBUCode()
    {
        ddlSbucode.DataSource = Masterofmaster.Get_MasterOfMaster_ByGroup_ForDropdown("SBU", "DropDown");
        ddlSbucode.DataBind();
        ddlSbucode.Items.Insert(0, new ListItem("--Seelct SBU Code--", "0"));
    }


    protected void ddLStates_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddLStates.SelectedValue != "0")
        {
            ddlCity.DataSource = Destination.GetDestination(Convert.ToString(ddLStates.SelectedValue));
            ddlCity.DataBind();
            ddlCity.Items.Insert(0, new ListItem("--Select City--", "0"));
            if (ddlCity.Items.Count > 0)
            {
                ddlCity.Focus();
            }
            else
            {
                ddLStates.Focus();
            }
        }
        else
        {
            ddlCity.DataBind();
            ddlCity.Focus();
        }
    }

    private void fillzones()
    {
        DDLsuperzone.DataSource = SuperZone.GetSuperzonemaster();
        DDLsuperzone.DataBind();
        DDLsuperzone.Items.Insert(0, new ListItem("--Select SuperZone--", "0"));
        ddLStates.DataSource = Destination.GetDestination(flag);
        ddLStates.DataBind();
        ddLStates.Items.Insert(0, new ListItem("--Select State--", "0"));

        ddlCity.Items.Insert(0, new ListItem("--Select City--", "0"));
        DDLCC.DataSource = CustCategory.GetCustomerCategoryMaster(flag);
        DDLCC.DataBind();
        DDLCC.Items.Insert(0, new ListItem("--Select Category--", "0"));
        DDLCSC.Items.Insert(0, new ListItem("--Select Sub Category--", "0"));

    }
    protected void DDLarea_SelectedIndexChanged(object sender, EventArgs e)
    {
        TxtCreditLimit.Focus();
    }

    public void custRating()
    {
        DataView dv1 = new DataView(Masterofmaster.Get_MasterOfMaster_ByGroup("CustRating").Tables[0]);
        DdlCustRating.DataSource = dv1;
        DdlCustRating.DataBind();
        DdlCustRating.Items.Insert(0, new ListItem("--Select Rating--", "0"));
    }
    //public void FillBoard()
    //{
    //    //DDLBoard.DataSource = Masterofmaster.Get_MasterOfMaster_ByGroup("Board").Tables[0];
    //    //DDLBoard.DataBind();
    //    //DDLBoard.Items.Insert(0, new ListItem("--Select Board--", "0"));
    //}
    #endregion

    #region SetView

    public void SetView()
    {
        if (Request.QueryString["a"] != null)
        {
            if (Request.QueryString["a"] == "a")
            {
                LblCustId.Text = "0";
                PnlAdd.Visible = true;
                PnlDetails.Visible = false;
                btnSave.Visible = true;

                filter.Visible = false;
            }
            else
                if (Request.QueryString["a"] == "v")
                {
                    PnlDetails.Visible = true;
                    PnlAdd.Visible = false;
                    btnSave.Visible = false;
                    filter.Visible = true;
                }
        }
    }
    #endregion

    protected void btnGetData_Click(object sender, EventArgs e)
    {

        string CustCode = txtcustomer.Text.ToString().Split(':')[0].Trim();
        DataSet ds = new DataSet();
        ds = DCMaster.Get_Name(CustCode, "Customer!" + Session["FY"].ToString());
        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();
        dt = ds.Tables[0];
        dt1 = ds.Tables[1];//CustomerTod Table Get Value
        DataTable dtAssorted = ds.Tables[2];    //Customer Assorted Discount

        Session["TempDataTable"] = null;
        Session["TempAssorted"] = null;

        if (dt1.Rows.Count > 0)
        {
            checkAction.Checked = true;
            Session["TempDataTable"] = dt1;
            TODGridview.DataSource = dt1;
            TODGridview.DataBind();
        }
        else
        {
            TODGridview.DataSource = dt1;
            TODGridview.DataBind();
        }
        if (dtAssorted.Rows.Count > 0)
        {
            Session["TempAssorted"] = dtAssorted;
            GridAssorted.DataSource = dtAssorted;
            GridAssorted.DataBind();
        }
        else
        {
            GridAssorted.DataSource = dtAssorted;
            GridAssorted.DataBind();
        }

        if (dt.Rows.Count != 0)
        {
            NewPanel.Visible = true;
            AssortedPanel.Visible = true;
            txtcustomer.Text = CustCode;
            lblCustName.Text = Convert.ToString(dt.Rows[0]["CustName"]);

            grdCustDetails.DataSource = BindGvCustDetail(CustCode);
            grdCustDetails.DataBind();


            if (grdCustDetails.Rows.Count > 0)
                FillForm();
            if (ChkBlacklist.Checked == true)
            {
                lblblkremark.Visible = true;
                LblblkDate.Visible = true;
                TxtblkRemark.Visible = true;
                TxtblkDate.Visible = true;
            }
            else
            {
                lblblkremark.Visible = false;
                LblblkDate.Visible = false;
                TxtblkRemark.Visible = false;
                TxtblkDate.Visible = false;

            }
        }
        else
        {
            lblCustName.Text = "No such Customer code";
            NewPanel.Visible = false;
            AssortedPanel.Visible = false;
            txtcustomer.Focus();
            txtcustomer.Text = "";
            ClearAll();
        }
    }

    protected void DDLCC_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDLCC.SelectedValue != "0")
        {
            DDLCSC.DataSource = CustCategory.GetCustomerCategoryMaster(Convert.ToString(DDLCC.SelectedValue));
            DDLCSC.DataBind();
            DDLCSC.Items.Insert(0, new ListItem("Select Sub Category", "0"));


            if (DDLCSC.Items.Count > 0)
            {
                DDLCSC.Focus();
            }
            else
            {
                DDLCSC.Focus();
            }
        }
        else
        {
            DDLCSC.DataBind();
            DDLCSC.Focus();
        }
        if (DDLCC.SelectedValue == "8")
        {
            btnAddAccess.Enabled = true;
            txtBookType.Enabled = true;
            txtFromQty.Enabled = true;
            txtToQty.Enabled = true;
            txtDiscount.Enabled = true;
        }
        else
        {
            btnAddAccess.Enabled = false;
            txtBookType.Enabled = false;
            txtFromQty.Enabled = false;
            txtToQty.Enabled = false;
            txtDiscount.Enabled = false;
        }
    }

    protected void ChkBlacklist_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkBlacklist.Checked == true)
        {
            lblblkremark.Visible = true;
            LblblkDate.Visible = true;
            TxtblkRemark.Visible = true;
            TxtblkDate.Visible = true;
        }
        else
        {
            lblblkremark.Visible = false;
            LblblkDate.Visible = false;
            TxtblkRemark.Visible = false;
            TxtblkDate.Visible = false;

        }
    }

    #region TOD Add Button Click
    protected void btnAddTOD_Click(object sender, EventArgs e)
    {
        DataTable dt = GetDataTempTable();
        if (dt != null)
        {
            bool isflage = false;
            DataRow[] rows = dt.Select("TodAmount = " + txtTODAmount.Text + " And TodPercentage = " + txtTodDis.Text);
            DataRow dr = null;

            if (rows.Length > 0)
            {
                dr = rows[0];
                isflage = true;
                MessageBox("Already Add Tod Amount");
                txtTODAmount.Focus();
            }
            else
            {
                dt.DefaultView.Sort = "TodAmount ASC";
                dr = dt.NewRow();
            }
            dr["Autoid"] = Convert.ToInt32(lblAutoId.Text == "" ? 0 : Convert.ToInt32(lblAutoId.Text)).ToString();
            dr["TodAmount"] = Convert.ToSingle(txtTODAmount.Text.Trim());
            dr["TodPercentage"] = Convert.ToSingle(txtTodDis.Text.Trim());
            dr["IsAction"] = Convert.ToInt32(checkAction.Checked == true ? 1 : 0);



            if (!isflage)
            {
                AddRow(dr);
                txtTODAmount.Text = "";
                txtTodDis.Text = "";
            }
        }
        TODGridview.DataSource = dt;
        TODGridview.DataBind();
        TODGridview.Visible = true;
        txtTODAmount.Text = "";
        txtTodDis.Text = "";
        checkAction.Checked = true;
        txtTODAmount.Focus();


    }
    #endregion

    #region Create Temp DataTable
    private void CreateTempTable()
    {
        if (Session["TempDataTable"] == null)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Autoid", typeof(int));
            dt.Columns.Add("TodAmount", typeof(float));
            dt.Columns.Add("TodPercentage", typeof(float));
            dt.Columns.Add("IsAction", typeof(int));

            Session["TempDataTable"] = dt;

        }
    }
    #endregion

    #region Get Temp Table
    private DataTable GetDataTempTable()
    {
        //if (Session["TempDataTable"] == null)
        //{
        //    string CustCode = txtcustomer.Text.ToString().Split(':')[0].Trim();
        //    DataTable dt = DCMaster.Get_Name(CustCode, "Customer").Tables[1];
        //}
        ////if (dt.Rows.Count > 0)
        ////{
        //   // Session["TempDataTable"] = dt;
        ////}

        CreateTempTable();
        return (DataTable)Session["TempDataTable"];
    }
    #endregion

    #region Add Row
    private void AddRow(DataRow dr)
    {
        CreateTempTable();
        if (Session["TempDataTable"] != null)
        {
            DataTable dt = (DataTable)Session["TempDataTable"];
            dt.Rows.Add(dr);
            Session["TempDataTable"] = dt;
        }
    }
    #endregion

    protected void btn_Delete_Click(object sender, ImageClickEventArgs e)
    {
        try
        {

            Other_Z.Customer_cs_rev Objcust = new Other_Z.Customer_cs_rev();
            Objcust.TODValue1 = "DEL";
            Objcust.OtherFields = "<r></r>";
            Objcust.Fyfrom = Convert.ToInt32(Session["FY"]);
            ImageButton btn = ((ImageButton)sender);
            DataTable dt = (DataTable)Session["TempDataTable"];
            Objcust.CustID = Convert.ToInt32(((Label)btn.FindControl("lblAutoId")).Text);
            Objcust.Save(out CID);
            if (dt != null)
            {
                DataRow[] row = dt.Select("AutoId=" + Objcust.CustID);
                DataRow dr = null;
                if (row.Length > 0)
                {
                    dr = row[0];
                    dr.Delete();

                }
            }
            DataView dv = new DataView(dt);
            Session["TempDataTable"] = dv.ToTable();
            TODGridview.DataSource = dv.ToTable();
            TODGridview.DataBind();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    protected void TODGridview_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }

    #region Assorted Discount All Details CRUD Operation

    #region Book Type Text Change Event
    protected void txtBookType_TextChange(object sender, EventArgs e)
    {
        txtFromQty.Focus();
    }
    #endregion

    #region Add Button Click Event Assorted
    protected void btnAddAccess_Click(object sender, EventArgs e)
    {
        Other_Z.OtherBAL ObjBAL = new Other_Z.OtherBAL();
        string BookCode = txtBookType.Text.Split(':')[0].Trim();
        DataSet ds = ObjBAL.GetBookIdWithBookType(BookCode, Convert.ToInt32(Session["FY"]), "booktype");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataTable dt = GetDataTempTableAssorted();
            if (dt != null)
            {
                if (Convert.ToInt32(txtDiscount.Text) <= 0 || Convert.ToInt32(txtDiscount.Text) >= 100)
                {
                    MessageBox("discount greater then 0 and less then 100");
                    txtDiscount.Focus();
                    return;

                }
                if (Convert.ToInt32(txtFromQty.Text) > Convert.ToInt32(txtToQty.Text))
                {
                    MessageBox("From Qty shold be less than To Qty");
                    txtFromQty.Focus();
                    return;
                }
                else
                {
                    // string CustCode = txtBookType.Text.Split(':')[0].Trim();
                    bool isflage = false;

                    //DataRow[] rows1 = dt.Select("BookType = '" + BookCode + "' And ToQty > " + txtFromQty.Text);
                    //if (rows1.Length > 0)
                    //{
                    //    MessageBox("Wrong from quantity and to quantity");
                    //    txtFromQty.Focus();
                    //    return;
                    //}

                    DataRow[] rows = dt.Select("BookType = '" + BookCode + "' And FromQty =" + txtFromQty.Text + " And ToQty = " + txtToQty.Text + "And Discount=" + txtDiscount.Text + "");

                    DataRow dr = null;
                    if (rows.Length > 0)
                    {
                        dr = rows[0];
                        isflage = true;
                        MessageBox("Already Add Assorted Discount");
                        txtBookType.Text = "";
                        txtBookType.Focus();
                        return;

                    }
                    else
                    {
                        dt.DefaultView.Sort = "BookType ASC";
                        dr = dt.NewRow();
                    }
                    dr["Autoid"] = Convert.ToInt32(lblautoIdAssorteddiscount.Text == "" ? 0 : Convert.ToInt32(lblautoIdAssorteddiscount.Text)).ToString();
                    dr["BookType"] = BookCode;
                    dr["Bookid"] = ds.Tables[0].Rows[0][0].ToString();
                    dr["FromQty"] = Convert.ToInt32(txtFromQty.Text.Trim());
                    dr["ToQty"] = Convert.ToInt32(txtToQty.Text.Trim());
                    dr["Discount"] = Convert.ToInt32(txtDiscount.Text.Trim());

                    if (!isflage)
                    {
                        AddRowAssorted(dr);
                        txtTODAmount.Text = "";
                        txtTodDis.Text = "";
                    }
                }
                GridAssorted.DataSource = dt;
                GridAssorted.DataBind();
                GridAssorted.Visible = true;
                txtBookType.Text = "";
                txtFromQty.Text = "";
                txtToQty.Text = "";
                txtDiscount.Text = "";
                txtBookType.Focus();
            }
        }
        else
        {
            MessageBox("Invalid book name please try again");
            txtBookType.Text = "";
            txtBookType.Focus();
            return;
        }

    }
    #endregion

    #region Create Temp DataTable Assorted
    private void CreateTempTableAssorted()
    {
        if (Session["TempAssorted"] == null)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Autoid", typeof(string));
            dt.Columns.Add("BookType", typeof(string));
            dt.Columns.Add("Bookid", typeof(int));
            dt.Columns.Add("FromQty", typeof(int));
            dt.Columns.Add("ToQty", typeof(int));
            dt.Columns.Add("Discount", typeof(int));
            Session["TempAssorted"] = dt;
        }
    }
    #endregion

    #region Get Temp Table
    private DataTable GetDataTempTableAssorted()
    {
        CreateTempTableAssorted();
        return (DataTable)Session["TempAssorted"];
    }
    #endregion

    #region Add Row Assorted
    private void AddRowAssorted(DataRow dr)
    {
        CreateTempTableAssorted();
        if (Session["TempAssorted"] != null)
        {
            DataTable dt = (DataTable)Session["TempAssorted"];
            dt.Rows.Add(dr);
            Session["TempAssorted"] = dt;
        }
    }
    #endregion

    #region Delete From Assoretd Descount
    protected void btn_DeleteAssorted_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Other_Z.Customer_cs_rev Objcust = new Other_Z.Customer_cs_rev();
            Objcust.TODValue1 = "delete";
            Objcust.OtherFields = "<r></r>";
            Objcust.Fyfrom = Convert.ToInt32(Session["FY"]);
            ImageButton btn = ((ImageButton)sender);
            DataTable dt = (DataTable)Session["TempAssorted"];
            Objcust.CustID = Convert.ToInt32(((Label)btn.FindControl("lblAutoIdAssored")).Text);
            Objcust.Save(out CID);
            if (dt != null)
            {
                DataRow[] row = dt.Select("AutoId=" + Objcust.CustID);
                DataRow dr = null;
                if (row.Length > 0)
                {
                    dr = row[0];
                    dr.Delete();

                }
            }
            DataView dv = new DataView(dt);
            Session["TempAssorted"] = dv.ToTable();
            GridAssorted.DataSource = dv.ToTable();
            GridAssorted.DataBind();
        }
        catch (Exception ex)
        {

            throw;
        }
    }
    #endregion

    #region DataTable Delete Rows In Temp Table
    protected void GridAssorted_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        //try
        //{
        //    if (Session["TempAssorted"] != null)
        //    {
        //        DataTable dt = new DataTable();
        //        dt = (DataTable)Session["TempAssorted"];
        //        dt.Rows[e.RowIndex].Delete();
        //        DataView dv = new DataView(dt);
        //        GridAssorted.DataSource = dv.ToTable();
        //        GridAssorted.DataBind();
        //        Session["TempAssorted"] = dv.ToTable();
        //    }
        //}
        //catch (Exception)
        //{

        //    throw;
        //}

    }
    #endregion

    #region Grid View Book Type Change Event
    protected void lblBookType_TextChanged(object sender, EventArgs e)
    {
        TextBox txtbooktype = (TextBox)sender;
        GridViewRow gvRow = (GridViewRow)txtbooktype.Parent.Parent;
        Label lblTotalRs = (Label)gvRow.FindControl("bookId");
        TextBox txtFromQty = (TextBox)gvRow.FindControl("lblFromQty");
        string BookCode = txtbooktype.Text.Split(':')[0].Trim();
        txtbooktype.Text = BookCode;
        if (txtbooktype.Text != "")
        {
            Other_Z.OtherBAL ObjBAL = new Other_Z.OtherBAL();
            DataSet ds = ObjBAL.GetBookIdWithBookType(BookCode, Convert.ToInt32(Session["FY"]), "booktype");
            if (ds.Tables[0].Rows.Count > 0)
            {
                lblTotalRs.Text = ds.Tables[0].Rows[0][0].ToString();
                txtFromQty.Focus();
            }
            else
            {
                MessageBox("Invalid book name please try again");
                txtbooktype.Text = "";
                txtbooktype.Focus();
                return;
            }
        }
        else
        {
            MessageBox("Please enter book code");
            txtbooktype.Text = "";
            txtbooktype.Focus();
            return;
        }
    }
    #endregion

    #region From Quntity Text Change Event In GridView
    protected void txtFromQty_TextChanged(object sender, EventArgs e)
    {
        TextBox txtFromQt = (TextBox)sender;
        GridViewRow gvRow = (GridViewRow)txtFromQt.Parent.Parent;
        TextBox txtToQ = (TextBox)gvRow.FindControl("lblToQty");
        if (txtFromQt.Text == "0" || txtFromQt.Text == "")
        {
            MessageBox("Please enter the from quntity");
            txtFromQt.Focus();
            return;
        }
        else if (Convert.ToInt32(txtFromQt.Text) > Convert.ToInt32(txtToQ.Text))
        {
            MessageBox("From Qty shold be less than To Qty");
            txtFromQt.Focus();
            return;
        }
        else
        {
            txtToQ.Focus();
        }

    }
    #endregion

    #region To Quntity Text Change Event In GridView
    protected void txtToQty_TextChanged(object sender, EventArgs e)
    {
        TextBox txtToQty = (TextBox)sender;
        GridViewRow gvRow = (GridViewRow)txtToQty.Parent.Parent;
        TextBox txtDis = (TextBox)gvRow.FindControl("lblDiscount");
        if (txtToQty.Text == "0" || txtToQty.Text == "")
        {
            MessageBox("Please enter the to quntity");
            txtToQty.Focus();
            return;
        }
        else
        {
            txtDis.Focus();
        }
    }
    #endregion

    #region Discount Text Change Event  In Gridview
    protected void txtDiscount_TextChanged(object sender, EventArgs e)
    {
        TextBox txtDisc = (TextBox)sender;
        GridViewRow gvRow = (GridViewRow)txtDisc.Parent.Parent;
        if (txtDisc.Text == "0" || txtDisc.Text == "")
        {
            MessageBox("Please enter the Discount");
            txtDisc.Focus();
            return;
        }
        else if (Convert.ToInt32(txtDisc.Text) <= 0 || Convert.ToInt32(txtDisc.Text) >= 100)
        {
            MessageBox("Discount greater then 0 and less then 100");
            txtDisc.Focus();
            return;
        }
        else
        {
            txtBookType.Focus();
        }
    }
    #endregion
    #endregion
}