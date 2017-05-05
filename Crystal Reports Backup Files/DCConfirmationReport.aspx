﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/NewChetana.master"
    AutoEventWireup="true" CodeFile="DCConfirmationReport.aspx.cs" Inherits="DCConfirmationReport" %>

<%@ Register TagPrefix="ajaxCt" Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
        <div class="section-header">
            <div class="title">
                <img src="Images/forms/ico-promotions.png" alt="Edit campaign details">
                DateWise Invoice <a href="Campaigns.aspx" title="back to campaign list"></a>
            </div>
            <div class="options">
            </div>
            <%-- <div id="filter" runat="server" style="float: right; width: 50%">
    <span>Filter Data:</span>
    <input name="filt" onkeyup="filter(this, 'sf', '<%=grdDetails.ClientID%>')" id="filterdata"
        type="text"></div>--%>
        </div>
        <asp:Panel ID="pnldt" CssClass="panelDetails" runat="server" Width="735px">
            <table>
                <tr>
                    <td>
                        <asp:Label ID="Label5" CssClass="lbl-form" runat="server" Text="Super Zone"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList CssClass="ddl-form" ID="DDLSuperZone" TabIndex="2" runat="server"
                            DataTextField="SuperZoneName" DataValueField="SuperZoneID" Width="200px" OnSelectedIndexChanged="DDLSuperZone_SelectedIndexChanged"
                            AutoPostBack="True">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="reqsuper" runat="server" ErrorMessage="Please select SuperZone"
                            InitialValue="0" ValidationGroup="AZone123" ControlToValidate="DDLSuperZone">.</asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <asp:Label ID="Label4" CssClass="lbl-form" runat="server" Text="Zone"></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:DropDownList ID="DDLZone" runat="server" TabIndex="3"  CssClass="ddl-form"
                            DataTextField="ZoneName" DataValueField="ZoneID" Width="200px" >
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td width="60px">
                        <asp:Label ID="Label1" runat="server" CssClass="lbl-form" Text="From Date"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtfromDate" CssClass="inp-form" TabIndex="1" Width="80px" runat="server"
                            Enabled="true"></asp:TextBox>
                        <ajaxCt:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtfromDate"
                            Format="dd/MM/yyyy" />
                        <ajaxCt:MaskedEditExtender ID="fromDate" runat="server" TargetControlID="txtfromDate"
                            MaskType="Date" Mask="99/99/9999" AcceptAMPM="true" MessageValidatorTip="false"
                            AutoComplete="true" CultureName="en-US" />
                        <asp:RequiredFieldValidator ID="Rfvfdt" runat="server" ErrorMessage="Require From Date"
                            ValidationGroup="bkwSummary" ControlToValidate="txtfromDate">.</asp:RequiredFieldValidator>
                    </td>
                    <td width="30px">
                    </td>
                    <td width="60px">
                        <asp:Label ID="Label2" runat="server" Text="To Date" CssClass="lbl-form"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txttoDate" CssClass="inp-form" TabIndex="2" Width="80px" runat="server"
                            Enabled="true"></asp:TextBox>
                        <ajaxCt:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txttoDate"
                            Format="dd/MM/yyyy" />
                        <ajaxCt:MaskedEditExtender ID="toDate" runat="server" TargetControlID="txttoDate"
                            MaskType="Date" Mask="99/99/9999" AcceptAMPM="true" MessageValidatorTip="false"
                            AutoComplete="true" CultureName="en-US" />
                        <asp:RequiredFieldValidator ID="Rfvtdt" runat="server" ErrorMessage="Require To Date"
                            ValidationGroup="bkwSummary" ControlToValidate="txttoDate">.</asp:RequiredFieldValidator>
                    </td>
                    <td width="30px">
                    </td>
                    <td>
                        <asp:Button ID="btnget" runat="server" Width="80px" TabIndex="3" Text="Get" CssClass="submitbtn"
                            ValidationGroup="bkwSummary" OnClick="btnget_Click" Style="height: 26px" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <br />
        <CR:CrystalReportViewer ID="a" runat="server" AutoDataBind="true"
            DisplayGroupTree="false" EnableDatabaseLogonPrompt="false" EnableDrillDown="false"
            EnableParameterPrompt="true" EnableTheming="false" EnableToolTips="false" HasDrillUpButton="false"
            HasGotoPageButton="True" HasPageNavigationButtons="True" HasRefreshButton="true"
            HasSearchButton="True" HasToggleGroupTreeButton="True" HasViewList="False" HasZoomFactorList="False"
            Height="1039px" Width="773px" />
        <asp:ValidationSummary ShowMessageBox="true" ShowSummary="false" ValidationGroup="bkwSummary"
            runat="server" ID="ss" />
    </div>
</asp:Content>
