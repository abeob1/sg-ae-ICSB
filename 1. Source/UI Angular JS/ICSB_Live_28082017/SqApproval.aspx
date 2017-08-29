<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="salsequote.aspx.vb" Inherits="WebApplication1.salsequote"
    MasterPageFile="~/Site.Master" %>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <script src="asset/js/services/SQA_service.js" ></script>
    <script src="asset/js/ctrl/SqApproval_ctrl.js"></script>

    <div class="forms" ng-controller="SqApproval_ctrl">
        <div class="form-grids row widget-shadow" data-example-id="basic-forms">
            <div class="form-title">
                <div class="col-lg-5">
                    <h4>
                        Sales Quotation 
                        Approval :</h4>
                </div>
                <div class="btn-group pull-right" ng-hide="true" role="group" aria-label="Basic example">
                    <button type="button" class="btn btn-white"  ng-click="FirstRecord();">
                        <i class="fa fa-fast-backward"></i>
                    </button>
                    <button type="button" class="btn btn-white" ng-click="PreviousRecord();">
                        <i class="fa fa-arrow-left"></i>
                    </button>
                    <button type="button" class="btn btn-white" ng-click="addclick();">
                        <i class="fa fa-plus"></i>
                    </button>
                    <button type="button" class="btn btn-white" ng-click="findclick();" >
                        <i class="fa fa-search"></i>
                    </button>
                    <button type="button" class="btn btn-white" ng-click="NextRecord();">
                        <i class="fa fa-arrow-right"></i>
                    </button>
                    <button type="button" class="btn btn-white" ng-click="LastRecord();">
                        <i class="fa fa-fast-forward"></i>
                    </button>
                </div>
            </div>
            
          

            <form id="ss" runat="server">
            <div class="form-body">
           
                <div class="col-lg-6">                   
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Create Date
                        </label>
                        <div class="col-sm-4">
                            <input type="text" ng-model="CdateFrom" class="form-control datepicker"
                                id="Email2" placeholder="Create Date">
                        </div>

                        <div class="col-sm-4">
                            <input type="text" ng-model="CdateTo" class="form-control datepicker"
                                id="Text1" placeholder="Create Date">
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Customer Code
                        </label>
                        <div class="col-sm-4">
                        <div class="input-group">
                         <input type="text" ng-model="FromCode"  class="form-control" id="Text2"
                                placeholder="Code From">
      <span class="input-group-btn">
        <button class="btn btn-info" type="button" ng-click="getcustomer();"><i class="fa fa-search" aria-hidden="true"></i></button>
      </span>
      </div>
                           
                                </div>
                        <div class="col-sm-4">
                            <div class="input-group">
                         <input type="text" ng-model="ToCode"  class="form-control" id="Text3"
                                placeholder="Code To">
      <span class="input-group-btn">
        <button class="btn btn-info" type="button" ng-click="getcustomerTo();"><i class="fa fa-search" aria-hidden="true"></i></button>
      </span>
      </div>
                        </div>
                    </div>
                    
                    
                    
                    
                </div>
                <div class="col-lg-6">
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Created by</label><div class="col-sm-8">
                        <select class="form-control"  ng-options="item.Name as item.Name for item in CreatedBy" ng-model="CreateBy" ng-change="setaddress(U_AddrN);" ></select>
                        
                            
                        </div>
                    </div>

                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Approved Status</label><div class="col-sm-8">
                        <select class="form-control"  ng-options="item.Name as item.Name for item in Astatus" ng-model="ApprovalSt"  ></select>
                        
                            
                        </div>
                    </div>
                   
                </div>
               
                <div class="row pull-right"><button type="button" ng-click="search();" class="btn btn-info"><i class="fa fa-search"></i> Search</button></div>
                <div class="row top-buffer">
                </div>
                <!-- tab start -->

                
                <div class="row">
                    <table class="table table-bordered table-striped">
                                <thead>
                                    <tr style="background-color:#00a7db;color:#fff">
                                        <th>
                                            So. No
                                        </th>
                                        <th>
                                            Link
                                        </th>
                                        <th>
                                            Created Date
                                        </th>
                                        <th>
                                            Created By
                                        </th>
                                        <th>
                                            Customer Code
                                        </th>
                                        <th>
                                            Customer Name
                                        </th>
                                        <th>
                                            Validity From
                                        </th>
                                        <th style="width: 25px">
                                            Validity To
                                        </th>
                                        <th>
                                            Approval Status
                                        </th>
                                        <th>
                                            Approved By
                                        </th>
                                        <th>
                                            Approved On
                                        </th>
                                         <th>
                                            Remark
                                        </th>
                                        <th colspan="3" ng-inti="filter=false">
                                            <button data-toggle="tooltip" data-placement="bottom" title="Filter"  ng-click="filter= !filter" class="btn-sm btn-white btn" type="button" ><i class="fa fa-filter" aria-hidden="true"></i> Filter</button>
                                        </th>
                                    </tr>
                                </thead>
                                <thead>
                                    <tr ng-show="filter">
                                        <th>
                                             <input ng-model="searchText.DocNum" class="form-control">
                                        </th>
                                        <th>
                                          
                                        </th>
                                        <th>
                                            <input ng-model="searchText.U_Cdate" class="form-control datepicker">
                                        </th>
                                        <th>
                                            <input ng-model="searchText.U_Uname" class="form-control">
                                        </th>
                                        <th>
                                            <input ng-model="searchText.U_Ccode" class="form-control">
                                        </th>
                                        <th>
                                            <input ng-model="searchText.U_Cname" class="form-control">
                                        </th>
                                        <th>
                                            <input ng-model="searchText.U_Qdate1" class="form-control datepicker">
                                        </th>
                                        <th style="width: 25px">
                                           <input ng-model="searchText.U_Qdate2" class="form-control datepicker">
                                        </th>
                                        <th>
                                          <input ng-model="searchText.U_Status" class="form-control">
                                        </th>
                                        <th>
                                             <input ng-model="searchText.U_ApprovedBy" class="form-control">
                                        </th>
                                        <th>
                                           <input ng-model="searchText.U_ApprovedDt" class="form-control">
                                        </th>
                                         <th>
                                           <input ng-model="s" class="form-control">
                                        </th>
                                        <th colspan="3">
                                          <span class="btn btn-danger btn-sm" ng-click="searchText=''"><i class="fa fa-times"></i> 
                                        </th>
                                    </tr>
                                </thead>

                                <tbody>
                                    <tr ng-repeat="d in data.APPROVAL | filter:searchText" >
                                        <td>
                                           <strong>{{d.DocNum}}</strong>
                                        </td>
                                        <td>
                                            
                                      <a href="Sales_Quotation.aspx?docno={{d.DocNum}}&sqapp=true" <i class="fa fa-arrow-right fa-2x" style="color:#faba0a" aria-hidden="true"></i>

                                           
                                        </td>
                                        <td>
                                            {{d.U_Cdate}}
                                        </td>
                                        <td>
                                            {{d.U_Uname}}
                                        </td>
                                        <td>
                                           {{d.U_Ccode}}
                                        </td>
                                        <td>
                                            {{d.U_Cname}}
                                        </td>
                                        <td>
                                            {{d.U_Qdate1}}
                                        </td>
                                        <td >
                                            {{d.U_Qdate2}}
                                        </td>
                                        <td>
                                            {{d.U_Status}}
                                        </td>
                                        <td>
                                           {{d.U_ApprovedBy}}
                                        </td>
                                        <td>
                                            {{d.U_ApprovedDt}}
                                        </td>
                                         <td>
                                            Remark
                                        </td>

                                          <td>
                                            
                                        </td>
                                       
                                       
                                    </tr>


                                    
                                </tbody>
                            </table>
                    
                    <div class="row top-buffer">
                    </div>
                    
                </div>
            </div>
            </form>
        </div>
    </div>
    <script type="text/javascript">        $('.datepicker').datepicker({ autoclose: true, format: 'dd/mm/yyyy' });

        
        </script>
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

    <!-- Modal -->
<div id="myModal" class="modal fade" role="dialog">
  <div class="modal-dialog">

    <!-- Modal content-->
    <div class="modal-content" >
      <div class="modal-header">        
        <button type="button" class="close" data-dismiss="modal">&times;</button>
        <h4 class="modal-title">Select Any One Record</h4>
      </div>
      <div class="modal-body" style="overflow:auto;height:400px;">
        <table width="100%" border="0" class="table table-bordered table-responsive" cellspacing="5" cellpadding="5">
  <tr>
    <td>Code</td>
    <td>Name</td>
    <td>Action</td>
  </tr>
  <tr ng-repeat="d in customer.OCRD">
    <td>{{d.U_Ccode}}</td>
    <td>{{d.U_Cname}}</td>
    <td><button ng-click="setc1(d.U_Ccode)" class="btn btn-sm btn-primary" data-dismiss="modal">Select</button></td>
  </tr>
</table>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
      </div>
    </div>

  </div>
</div>




    <!-- Modal -->
<div id="myModalTo" class="modal fade" role="dialog">
  <div class="modal-dialog">

    <!-- Modal content-->
    <div class="modal-content" >
      <div class="modal-header">        
        <button type="button" class="close" data-dismiss="modal">&times;</button>
        <h4 class="modal-title">Select Any One Record</h4>
      </div>
      <div class="modal-body" style="overflow:auto;height:400px;">
        <table width="100%" border="0" class="table table-bordered table-responsive" cellspacing="5" cellpadding="5">
  <tr>
    <td>Code</td>
    <td>Name</td>
    <td>Action</td>
  </tr>
  <tr ng-repeat="d in customerTO.OCRD">
    <td>{{d.U_Ccode}}</td>
    <td>{{d.U_Cname}}</td>
    <td><button ng-click="setc2(d.U_Ccode)" class="btn btn-sm btn-primary" data-dismiss="modal">Select</button></td>
  </tr>
</table>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
      </div>
    </div>

  </div>
</div>


<!-- Find Modal -->
<div id="findmodal" class="modal fade" role="dialog">
  <div class="modal-dialog" style="width: 1250px;">

    <!-- Modal content-->
    <div class="modal-content" >
      <div class="modal-header">        
        <button type="button" class="close" data-dismiss="modal">&times;</button>
        <h4 class="modal-title">Select Any One Record</h4>
      </div>
      <div class="modal-body" style="overflow:auto;height:400px;">
      
        <table width="100%" border="0" class="table table-bordered table-responsive" cellspacing="5" cellpadding="5">
  <tr>
    <td>Quotation No</td>
    <td>Status</td>
    <td>User Name</td>
    <td>Creation Date</td>
    <td>Quotation Valid</td>
    <td>Customer Code</td>
    <td>Customer Name</td>
    <td>Project Code</td>
    <td>Tel No</td>
    <td>Fax No</td>
    <td>Mobile No</td>
    <td>Email</td>
    <td></td>
    
    
  </tr>
  <tr ng-repeat="d in findresult.SQTO">
   <td>{{d.U_Qno}}</td>
    <td>{{d.U_Status}}</td>
    <td>{{d.U_Uname}}</td>
    <td>{{d.U_Cdate}}</td>
    <td>{{d.U_Qdate1}} - {{d.U_Qdate2}}</td>
    <td>{{d.U_Ccode}}</td>
    <td>{{d.U_Cname}}</td>
    <td>{{d.U_Pcode}}</td>
    <td>{{d.U_TelNo}}</td>
    <td>{{d.U_FaxNo}}</td>
    <td>{{d.U_Mno}}</td>
    <td>{{d.U_Email}}</td>
    <td><button ng-click="FindoneRecord(d.U_Qno);" class="btn btn-sm btn-primary" data-dismiss="modal">Select</button></td>
  </tr>

  <tr  class="text-danger" ng-if="findresult.VALIDATE[0].Status =='False'"> <td colspan="13" align="center" > No Record Found..</td></tr>
</table>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
      </div>
    </div>

  </div>
</div>






</asp:Content>
