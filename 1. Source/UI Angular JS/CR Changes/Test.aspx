﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="FileUpload.Test" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data" action="http://119.73.138.58:84/FileUpload.aspx">
    <div>
        <input type="file" runat="server" name="FileName" id="FileName" />
        <input type="submit" runat="server" />
    </div>
    </form>
</body>
</html>
