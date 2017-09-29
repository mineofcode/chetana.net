﻿<%@ Page Language="C#" MasterPageFile="~/MasterPages/NewChetana.master" AutoEventWireup="true" CodeFile="Token_Register_Report.aspx.cs" Inherits="Token_Register_Report" Title="Token Register Report" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<CR:CrystalReportViewer ID="TokeRegisterReport" runat="server" AutoDataBind="true"
        DisplayGroupTree="false" EnableDatabaseLogonPrompt="false" EnableDrillDown="false"
        EnableParameterPrompt="false" EnableTheming="false" EnableToolTips="true" HasDrillUpButton="false"
        HasGotoPageButton="True" HasPageNavigationButtons="True" HasRefreshButton="true"
        HasSearchButton="True" HasToggleGroupTreeButton="false" HasViewList="false" HasZoomFactorList="false"
        Width="773px" ClientTarget="Auto" HasCrystalLogo="False" />
</asp:Content>


