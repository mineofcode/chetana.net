﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MonthwiseReconsiledData.aspx.cs" Inherits="MonthwiseReconsiledData" %>

<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
         
        <CR:CrystalReportViewer ID="crvReport" runat="server" 
            AutoDataBind="true" />
        <br />
        <CR:CrystalReportSource ID="CrystalReportSource1" runat="server">
            <report filename="Report\ReconsiledMonthWise.rpt">
            </report>
        </CR:CrystalReportSource>
         
    </div>
    </form>
</body>
</html>
