﻿<%@ Page Language="C#" MasterPageFile="~/MasterPages/NewChetana.master" AutoEventWireup="true" CodeFile="BankPayment.aspx.cs" Inherits="BankPayment" Title="Bank Payment" %>

<%@ Register src="UserControls/uc_BankPayment.ascx" tagname="uc_BankPayment" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <uc1:uc_BankPayment ID="uc_BankPayment1" runat="server" />
</asp:Content>

