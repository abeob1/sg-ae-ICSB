<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="salsequote.aspx.vb" Inherits="WebApplication1.salsequote"
    MasterPageFile="~/Site.Master" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <script src="asset/js/services/SL_service.js"></script>
    <script src="asset/js/ctrl/SurveyList_ctrl.js"></script>
    <div class="forms" ng-controller="SurveyList_ctrl">
        <div class="form-grids row widget-shadow" data-example-id="basic-forms">
            <div class="form-title">
                <div class="col-lg-5">
                    <h4>
                        Survey List :</h4>
                </div>
            </div>
            <form id="ss" runat="server">
            <div class="form-body">
                <div class="col-lg-6">
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Survey No
                        </label>
                        <div class="col-sm-8">
                            <div class="input-group">
                                <input ng-disabled="updatedisabled" type="text" ng-model="U_SurvyNo" class="form-control"
                                    ng-keydown="CCselected=false" id="SurveyNo" placeholder="Survey No">
                                <span class="input-group-btn">
                                    <button class="btn btn-info" type="button" ng-disabled="updatedisabled" ng-click="getsurveyno(U_SurvyNo);">
                                        <i class="fa fa-search" aria-hidden="true"></i>
                                    </button>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Sales Order No
                        </label>
                        <div class="col-sm-8">
                            <div class="input-group">
                                <input type="text" ng-disabled="updatedisabled" ng-model="U_OrderNo" class="form-control"
                                    ng-keydown="CCselected=false" id="SalesOrderNo" placeholder="Sales Order No" />
                                <span class="input-group-btn">
                                    <button class="btn btn-info" type="button" ng-disabled="updatedisabled" ng-click="getSalesOrderNo(U_OrderNo);">
                                        <i class="fa fa-search" aria-hidden="true"></i>
                                    </button>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Customer Code
                        </label>
                        <div class="col-sm-8">
                            <div class="input-group">
                                <input type="text" ng-disabled="updatedisabled" ng-model="U_Ccode" class="form-control"
                                    ng-keydown="CCselected=false" id="CustomerCode" placeholder="Customer Code" />
                                <span class="input-group-btn">
                                    <button class="btn btn-info" type="button" ng-disabled="updatedisabled" ng-click="getcustomer('code');">
                                        <i class="fa fa-search" aria-hidden="true"></i>
                                    </button>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Customer Name
                        </label>
                        <div class="col-sm-8">
                            <div class="input-group">
                                <input ng-disabled="updatedisabled" type="text" ng-model="U_Cname" ng-keydown="CCselected=false"
                                    class="form-control" id="Email7" placeholder="Company Name">
                                <span class="input-group-btn">
                                    <button class="btn btn-info" type="button" ng-disabled="updatedisabled" ng-click="getcustomer('name');">
                                        <i class="fa fa-search" aria-hidden="true"></i>
                                    </button>
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-lg-6">
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Survey Date From
                        </label>
                        <div class="col-sm-8">
                            <input type="text" ng-model="U_SrvyDatFrm" ng-disabled="updatedisabled" class="form-control datepicker" placeholder="Survey Date From" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Survey Date To
                        </label>
                        <div class="col-sm-8">
                            <input type="text" ng-model="U_SrvyDatTo" ng-disabled="updatedisabled" class="form-control datepicker" placeholder="Survey Date To"/>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Container Number
                        </label>
                        <div class="col-sm-8">
                            <input type="text" ng-model="U_ContainerNo" ng-disabled="updatedisabled" class="form-control" placeholder="Container Number"/>
                        </div>
                    </div>
                    <div class="form-group"></div> 
                </div>
                <div class="row top-buffer">
                </div>
                <!-- table start -->
                <button type="button" class="btn-success btn" ng-click="findSL();">
                    Search</button>
                <div class="row">
                    <table width="100%" border="0" class="table table-bordered table-responsive" cellspacing="5"
                        cellpadding="5">
                        <tr>
                            <td>
                                Link
                            </td>
                            <td>
                                Survey No
                            </td>
                            <td>
                                Survey SAP Doc.No
                            </td>
                            <td>
                                Order No
                            </td>
                            <td>
                                Order SAP Doc.No
                            </td>
                            <td>
                                User Name
                            </td>
                            <td>
                                Survey Date
                            </td>
                            <td>
                                Customer Code
                            </td>
                            <td>
                                Customer Name
                            </td>
                            <td>
                                Survey Type
                            </td>
                            <td>
                                Location
                            </td>
                            <td>
                                Survey Result
                            </td>
                        </tr>
                        <tr ng-repeat="d in SL">
                            <td>
                                <i class="fa fa-arrow-right fa-2x" ng-click="gotoSurveyFromByLink(d.Survey_No);"
                                    style="color: #faba0a; cursor: pointer;" aria-hidden="true"></i>
                            </td>
                            <td>
                                {{d.Survey_No}}
                            </td>
                            <td>
                                {{d.DocNum}}
                            </td>
                            <td>
                                {{d.Order_No}}
                            </td>
                            <td>
                                {{d.SODocNum}}
                            </td>
                            <td>
                                {{d.User_Name}}
                            </td>
                            <td>
                                {{d.Survey_Date}}
                            </td>
                            <td>
                                {{d.Customer_Code}}
                            </td>
                            <td>
                                {{d.Customer_Name}}
                            </td>
                            <td>
                                {{d.Survey_Type}}
                            </td>
                            <td>
                                {{d.Location}}
                            </td>
                            <td>
                                {{d.Survey_Result}}
                            </td>
                        </tr>
                    </table>
                    <div align="center" class="text-danger" ng-show="!SL.length">
                        <strong>No Data...</strong></div>
                </div>
            </div>
            </form>
        </div>
    </div>
    <script type="text/javascript">        $('.datepicker').datepicker({ autoclose: true, format: 'dd/mm/yyyy' });</script>
    <script>

        function hoverdiv(e, divid) {

            var left = e.clientX + "px";
            var top = e.clientY + "px";

            var div = document.getElementById(divid);

            div.style.left = left;
            div.style.top = top;

            $("#" + divid).toggle();
            return false;
        }

        function toolshow(id) {

            $("#" + id).show("fast");
        }
        function toolhide(id) {
            $("#" + id).hide(500);
        }

    </script>
    <!--Sales Order No Modal -->
    <div id="myModalSONum" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-miss="modal">
                        &times;</button>
                    <h4 class="modal-title">
                        Select any one record</h4>
                </div>
                <div class="modal-body" style="overflow: auto; height: 400px">
                    <table width="100%" border="0" class="table table-bordered table-responsive" cellpadding="5"
                        cellspacing="5">
                        <tr>
                            <td>
                                Order No
                            </td>
                            <td>
                                Action
                            </td>
                        </tr>
                        <tr ng-repeat="d in U_OrderNo.ODLN">
                            <td>
                                {{d.U_OrderNo}}
                            </td>
                            <td>
                                <button type="button" ng-click="fillSalesOrderNo(d.U_OrderNo)" class="btn btn-sm btn-primary"
                                    data-dismiss="modal">
                                    Select</button>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">
                        Close</button>
                </div>
            </div>
        </div>
    </div>
    <!--Suvey No Modal -->
    <div id="myModalSuveyNo" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-miss="modal">
                        &times;</button>
                    <h4 class="modal-title">
                        Select any one record</h4>
                </div>
                <div class="modal-body" style="overflow: auto; height: 400px;">
                    <table width="100%" border="0" class="table table-bordered table-responsive" cellpadding="5"
                        cellspacing="5">
                        <tr>
                            <td>
                                Survey No
                            </td>
                            <td>
                                Action
                            </td>
                        </tr>
                        <tr ng-repeat="d
    in U_SurvyNo.ODLN">
                            <td>
                                {{d.U_SurvyNo}}
                            </td>
                            <td>
                                <button ng-click="FillSurveyNo(d.U_SurvyNo)" class="btn btn-sm btn-primary" data-dismiss="modal">
                                    Select</button>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="modal-footer">
                    <button class="btn btn-default" type="button" data-dismiss="modal">
                        Close</button>
                </div>
            </div>
        </div>
    </div>
    <!--Customer Code-->
    <div id="myModal" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
    <div class="modal-content">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal">
                &times;</button>
            <h4 class="modal-title">
                Select Any One Record</h4>
        </div>
        <div class="modal-body" style="overflow: auto; height: 400px;">
            <table width="100%" border="0" class="table table-bordered table-responsive" cellspacing="5"
                cellpadding="5">
                <tr>
                    <td>
                        Code
                    </td>
                    <td>
                        Name
                    </td>
                    <td>
                        Action
                    </td>
                </tr>
                <tr ng-repeat="d in customer.OCRD">
                    <td>
                        {{d.U_Ccode}}
                    </td>
                    <td>
                        {{d.U_Cname}}
                    </td>
                    <td>
                        <button ng-click="Fillcustomer(d.U_Ccode,d.U_Cname)" class="btn btn-sm btn-primary"
                            data-dismiss="modal">
                            Select</button>
                    </td>
                </tr>
            </table>
        </div>
        <div class="modal-footer">
            <button type="button" class="btn btn-default" data-dismiss="modal">
                Close</button>
        </div>
    </div>
    </div> </div>
</asp:Content>
