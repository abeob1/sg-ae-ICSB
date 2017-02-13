<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="salsequote.aspx.vb" Inherits="WebApplication1.salsequote"
    MasterPageFile="~/Site.Master" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="forms">
        <div class="form-grids row widget-shadow" data-example-id="basic-forms">
            <div class="form-title">
                <div class="col-lg-5">
                    <h4>
                       Customer Contract :</h4>
                </div>
                <div class="btn-group pull-right" role="group" aria-label="Basic example">
                    <button type="button" class="btn btn-white">
                        <i class="fa fa-fast-backward"></i>
                    </button>
                    <button type="button" class="btn btn-white">
                        <i class="fa fa-arrow-left"></i>
                    </button>
                    <button type="button" class="btn btn-white">
                        <i class="fa fa-plus"></i>
                    </button>
                    <button type="button" class="btn btn-white">
                        <i class="fa fa-search"></i>
                    </button>
                    <button type="button" class="btn btn-white">
                        <i class="fa fa-arrow-right"></i>
                    </button>
                    <button type="button" class="btn btn-white">
                        <i class="fa fa-fast-forward"></i>
                    </button>
                </div>
            </div>
            <form id="ss" runat="server" >
            <div class="form-body">
                <div class="col-lg-6">
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Contract No
                        </label>
                        <div class="col-sm-8">
                            0012343
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Status
                        </label>
                        <div class="col-sm-8">
                            Open
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            User Name
                        </label>
                        <div class="col-sm-8">
                            
                            <asp:TextBox ID="username" runat="server" CssClass="form-control" placeholder="User Name"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Create Date
                        </label>
                        <div class="col-sm-8">
                            <input type="text" class="form-control datepicker" id="Email2" placeholder="Create Date">
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Customer Code
                        </label>
                        <div class="col-sm-8">
                            <input type="text" class="form-control" id="Email3" placeholder="Customer Code">
                        </div>
                        
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                           Customer Name
                        </label>
                        <div class="col-sm-8">
                            <input type="email" class="form-control" id="Email6" placeholder="Customer Name">
                        </div>
                    </div>
                    
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Contract Period
                        </label>
                        <div class="col-sm-4">
                            <input type="text" class="form-control datepicker" id="Email8" placeholder="Period From">
                        </div>
                        <div class="col-sm-4">
                            <input type="text" class="form-control datepicker" id="Email4" placeholder="Period To">
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Project Code
                        </label>
                        <div class="col-sm-8">
                            <input type="email" class="form-control" id="Email5" placeholder="Project Code">
                        </div>
                    </div>
                </div>
                <div class="col-lg-6">
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Address Name
                        </label>
                        <div class="col-sm-8">
                            <input type="email" class="form-control" id="Email18" placeholder="Address Name">
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Address Line1
                        </label>
                        <div class="col-sm-8">
                            <input type="email" class="form-control" id="Email19" placeholder="Address Line1">
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Address Line2
                        </label>
                        <div class="col-sm-8">
                            <input type="email" class="form-control" id="Email9" placeholder="Address Line2">
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Address Line3
                        </label>
                        <div class="col-sm-8">
                            <input type="email" class="form-control" id="Email10" placeholder="Address Line3">
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Address Line4
                        </label>
                        <div class="col-sm-8">
                            <input type="email" class="form-control" id="Email11" placeholder="Address Line4">
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Address Line5
                        </label>
                        <div class="col-sm-8">
                            <input type="email" class="form-control" id="Email13" placeholder="Address Line5">
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Address Line6
                        </label>
                        <div class="col-sm-8">
                            <input type="email" class="form-control" id="Email14" placeholder="Address Line6">
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Tel No
                        </label>
                        <div class="col-sm-8">
                            <input type="email" class="form-control" id="Email15" placeholder="Tel No">
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Fax No
                        </label>
                        <div class="col-sm-8">
                            <input type="email" class="form-control" id="Email17" placeholder="Fax No">
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Mobile No
                        </label>
                        <div class="col-sm-8">
                            <input type="email" class="form-control" id="Email12" placeholder="Mobile No.">
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Email
                        </label>
                        <div class="col-sm-8">
                            <input type="email" class="form-control" id="Email16" placeholder="Email">
                        </div>
                    </div>
                </div>
                <div class="row top-buffer">
                </div>
              
                <!-- tab start -->
                <div class="row">
                    <ul id="myTabs" class="nav nav-tabs" role="tablist">
                        <li role="presentation" class="active"><a href="#General" id="General-tab" role="tab"
                            data-toggle="tab" aria-controls="home" aria-expanded="false">General</a></li>
                        <li role="presentation" class=""><a href="#Addon" role="tab" id="Addon-tab" data-toggle="tab"
                            aria-controls="Addon" aria-expanded="true">Add-on</a></li>
                        <li role="presentation" class=""><a href="#Attachment" role="tab" id="Attachment-tab"
                            data-toggle="tab" aria-controls="Attachment" aria-expanded="true">Attachment</a></li>
                    </ul>
                    <div id="myTabContent" class="tab-content scrollbar1">
                        <div role="tabpanel" class="tab-pane fade active in" id="General" aria-labelledby="General-tab-tab">
                            <span class="top-buffer"></span>
                            <table class="table table-bordered">
                                <thead>
                                    <tr>
                                        <th>
                                            #
                                        </th>
                                        <th>
                                            Survey Type
                                        </th>
                                        <th>
                                            Continent
                                        </th>
                                        <th>
                                            Country
                                        </th>
                                        <th>
                                            City
                                        </th>
                                        <th>
                                            Equipment Group
                                        </th>
                                        <th>
                                            Currency
                                        </th>
                                        <th>
                                            Rate
                                        </th>
                                        <th>
                                            U.O.M
                                        </th>
                                        <th>
                                            GST
                                        </th>
                                        <th>
                                            Remarks
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <th scope="row">
                                            1
                                        </th>
                                        <td>
                                            ONH</td>
                                        <td>
                                            ASIS</td>
                                        <td>CHINA</td>
                                        <td>
                                            SHA
                                        </td>
                                        <td>
                                            GP</td>
                                        <td>
                                            USD</td>
										<td>7.00</td>
										<td>PER UNIT </td>
										<td>OS 0% </td>
										<td>&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <th scope="row">
                                            2
                                        </th>
                                        <td>ONH</td>
                                        <td>ASIS</td>
                                        <td>
                                            Table cell
                                        </td>
                                        <td>
                                            Table cell
                                        </td>
                                        <td>
                                            Table cell
                                        </td>
                                        <td>USD</td>
										<td>4.00</td>
										<td>PER UNIT </td>
										<td>OS 0%</td>
										<td>&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <th scope="row">
                                            3
                                        </th>
                                        <td>ONH</td>
                                        <td>ASIS</td>
                                        <td>
                                            Table cell
                                        </td>
                                        <td>
                                            Table cell
                                        </td>
                                        <td>
                                            Table cell
                                        </td>
                                        <td>USD</td>
										<td>7.00</td>
										<td>PER UNIT </td>
										<td>OS 0%                                        </td>
										<td>&nbsp;</td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <div role="tabpanel" class="tab-pane fade in" id="Addon" aria-labelledby="Addon-tab">
                             <span class="top-buffer"></span>
                                                        <table class="table table-bordered">
                                <thead>
                                    <tr>
                                        <th>
                                            #
                                        </th>
                                        <th>
                                            Charge Type
                                        </th>
                                        <th>
                                            Continent
                                        </th>
                                        <th>
                                            Country
                                        </th>
                                        <th>
                                            City
                                        </th>
                                        <th>
                                            Location
                                        </th>
                                        <th>
                                            Currency
                                        </th>
                                        <th>
                                            Rate
                                        </th>
                                        <th>
                                            U.O.M
                                        </th>
                                        <th>
                                            GST
                                        </th>
                                        <th>
                                            Remarks
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <th scope="row">
                                            1
                                        </th>
                                        <td>
                                            ONH</td>
                                        <td>
                                            ASIS</td>
                                        <td>CHINA</td>
                                        <td>
                                            SHA
                                        </td>
                                        <td>
                                            EI DEPOT</td>
                                        <td>
                                            USD</td>
										<td>7.00</td>
										<td>PER UNIT </td>
										<td>OS 0% </td>
										<td>&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <th scope="row">
                                            2
                                        </th>
                                        <td>ONH</td>
                                        <td>ASIS</td>
                                        <td>
                                            Table cell
                                        </td>
                                        <td>
                                            Table cell
                                        </td>
                                        <td>EI DEPOT</td>
                                        <td>USD</td>
										<td>4.00</td>
										<td>PER UNIT </td>
										<td>OS 0%</td>
										<td>&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <th scope="row">
                                            3
                                        </th>
                                        <td>ONH</td>
                                        <td>ASIS</td>
                                        <td>
                                            Table cell
                                        </td>
                                        <td>
                                            Table cell
                                        </td>
                                        <td>EI DEPOTl                                        </td>
                                        <td>USD</td>
										<td>7.00</td>
										<td>PER UNIT </td>
										<td>OS 0%                                        </td>
										<td>&nbsp;</td>
                                    </tr>
                                </tbody>
                            </table>

                        </div>
                        <div role="tabpanel" class="tab-pane fade" id="Attachment" aria-labelledby="Attachment-tab">
                          <span class="top-buffer"></span>
                                                        <table class="table table-bordered">
                                <thead>
                                    <tr>
                                        <th>
                                            #                                        </th>
                                        <th>Path</th>
                                        <th>File Name </th>
                                        <th>
                                            Attachment Date                                        </th>
                                        <th>
                                            Remarks                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <th scope="row">
                                            1                                        </th>
                                        <td>
                                            ONH</td>
                                        <td>
                                            ASIS</td>
                                        <td>CHINA</td>
                                        <td>&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <th scope="row">
                                            2                                        </th>
                                        <td>ONH</td>
                                        <td>ASIS</td>
                                        <td>
                                            Table cell                                        </td>
                                        <td>&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <th scope="row">
                                            3                                        </th>
                                        <td>ONH</td>
                                        <td>ASIS</td>
                                        <td>
                                            Table cell                                        </td>
                                        <td>&nbsp;</td>
                                    </tr>
                                </tbody>
                            </table>

                        </div>
                    </div>

                     <div class="row top-buffer">
                </div>
                <div class="row" align="right">
                    <span>
                        <button class="btn btn-success">
                            Save</button></span>
                             <span><button class="btn btn-primary">Print</button></span>
                           
                             <span><button class="btn btn-danger">Cancel</button></span></div>
                </div>
            </div>
            </form>
        </div>
    </div>
    <script type="text/javascript">        $('.datepicker').datepicker('autoclose', 'true')</script>
</asp:Content>
