<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="salsequote.aspx.vb" Inherits="WebApplication1.salsequote"
    MasterPageFile="~/Site.Master" %>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <script src="asset/js/services/oss_service.js" ></script>
    <script src="asset/js/ctrl/otherSurvey_ctrl.js"></script>

    <div class="forms" ng-controller="otherSurvey_ctrl">
        <div class="form-grids row widget-shadow" data-example-id="basic-forms">
            <div class="form-title">
                <div class="col-lg-5">
                    <h4>
                        Any other survey :</h4>
                </div>
                <div class="btn-group pull-right" role="group"  aria-label="Basic example">
                <button type="button" class="btn btn-white"  ng-click="backToSO();">
                        <i class="fa fa-fast-backward"></i> Back to Sales Order
                    </button>
                    <button type="button" class="btn btn-white"  ng-click="SurveyList();">
                        <i class="fa fa-th-list"></i> Back to Survey List
                    </button>
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
                            <span >{{data.ODLN[0].U_SurvyNo}}</span>
                           
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                           SAP Document.No
                        </label>
                        <div class="col-sm-8">
                            <span >{{data.ODLN[0].DocNum}}</span>
                           
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Status
                        </label>
                        <div class="col-sm-8">
                            {{data.ODLN[0].U_Status}}
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            User Name
                        </label>
                        <div class="col-sm-8">
                            
                                <input type="text" ng-model="data.ODLN[0].U_UName" ng-disabled="true" class="form-control"
                                id="username" value="{{UserData[0].Name}}" placeholder="">
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Create Date
                        </label>
                        <div class="col-sm-8">
                            <input type="text" ng-model="data.ODLN[0].U_Cdate" class="form-control datepickersub"
                                id="Email2" placeholder="Create Date">
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                           Survey Date
                        </label>
                        <div class="col-sm-8">
                            <input type="text" ng-model="data.ODLN[0].DocDate" ng-disabled="whilefind" class="form-control datepickersub"
                                id="Email3" placeholder="Order Date">
                        </div>
                        
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Company Code
                        </label>
                        <div class="col-sm-8">
                          <div class="input-group">
                            <input type="text" ng-model="data.ODLN[0].CardCode" ng-disabled="true" class="form-control" id="Email6"
                                placeholder="Company Code">
                                <span class="input-group-btn">
        <button class="btn btn-info" type="button" ng-disabled="true" ng-click="getcustomer();"><i class="fa fa-search" aria-hidden="true"></i></button>
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
                         <input type="text" ng-model="data.ODLN[0].CardName" ng-disabled="true"  class="form-control" id="Email7"
                                placeholder="Customer Name">
      <span class="input-group-btn">
        <button class="btn btn-info" type="button" ng-disabled="true" ng-click="getcustomer();"><i class="fa fa-search" aria-hidden="true"></i></button>
      </span>

                           
                                </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Surveyor ID
                        </label>
                         <div class="col-sm-8">
                          <div class="input-group">
                            <input type="text" ng-disabled="false" ng-model="data.ODLN[0].U_SurveyorID" class="form-control" id="Text1"
                                placeholder="Surveyor ID">
                                <span class="input-group-btn">
        <button class="btn btn-info" type="button" ng-disabled="false" ><i class="fa fa-search" aria-hidden="true" ng-click="getsurveyorID();"></i></button>
      </span>
      </div>
                        </div>
                        
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Surveyor Name
                        </label>
                         <div class="col-sm-8">
                          <div class="input-group">
                           {{getSurveyorName(data.ODLN[0].U_SurveyorID)}}
      </div>
                        </div>
                        
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Customer Reference
                        </label>
                        <div class="col-sm-8">
                            <input type="text" capitalize ng-disabled="false" ng-model="data.ODLN[0].NumAtCard" class="form-control" id="Email5"
                                placeholder="Customer Reference">
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Web Link
                        </label>
                        <div class="col-sm-8">
                        <a target="_blank" ng-model="HlinkBef" href={{data.ODLN[0].U_HLinkBef}}>{{data.ODLN[0].U_HLinkBef}}</a>
                        <a target="_blank" ng-model="HlinkAft" href={{data.ODLN[0].U_HLinkAft}}>{{data.ODLN[0].U_HLinkAft}}</a>
                        </div>
                    </div>
                </div>
                <div class="col-lg-6">
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Survey Type
                        </label>
                        <div class="col-sm-8">
                       
                         <select class="form-control" ng-disabled="true" ng-options="item.U_Stype as item.U_StypeName for item in stype" ng-model="data.ODLN[0].U_STypeCode" ></select>
                            
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Country 
                        </label>
                        <div class="col-sm-8">
                             <select class="form-control" ng-disabled="true" ng-options="item.U_Country as item.U_CountryName for item in CountryMaster" ng-model="data.ODLN[0].U_Country" ng-change="getcity(d.U_Country);"></select>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3"  class="col-sm-4 control-label">
                            City
                        </label>
                        <div class="col-sm-8">
                            <select class="form-control" ng-disabled="true" ng-options="item.U_City as item.U_CityName for item in city" ng-model="data.ODLN[0].U_City" "></select>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Location
                        </label>
                        <div class="col-sm-8">
                             <select class="form-control" ng-disabled="true" ng-options="item.U_Loc as item.U_LocName for item in location" ng-model="data.ODLN[0].U_Loc"></select>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                           Project Code
                        </label>
                        <div class="col-sm-8">
                       
                        <input type="text" capitalize class="form-control" ng-model="data.ODLN[0].Project" ng-disabled="true"/>
                            
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Agent Company
                        </label>
                         <div class="col-sm-8">
                          <div class="input-group">
                            <input type="text" ng-disabled="false" ng-model="data.ODLN[0].U_SuvExAgent" class="form-control" id="Text2"
                                placeholder="Agent Company">
                                <span class="input-group-btn">
        <button class="btn btn-info" type="button" ng-disabled="false" ><i class="fa fa-search" aria-hidden="true" ng-click="getsurveyAgent();"></i></button>
      </span>
      </div>
                        </div>
                        
                    </div>
                    
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                           Equipment Type
                        </label>
                        <div class="col-sm-8">
                       
                         <select class="form-control" ng-disabled="true" ng-options="item.U_EQTYPECODE as item.U_EQTYPENAME for item in etype" ng-model="data.ODLN[0].U_Eqtype" ></select>
                            
                        </div>
                    </div>

                     <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                             Survey Criteria
                        </label>
                        <div class="col-sm-8">
                           <select class="form-control" ng-disabled="false" ng-options="item.U_SURCRTACODE as item.U_SURCRTANAME for item in SurveyCriteria" ng-model="data.ODLN[0].U_SCriteria" ></select>
                        </div>
                    </div>


                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Equipment Number
                        </label>
                        <div class="col-sm-8">
                            <input type="text" ng-model="data.ODLN[0].U_EqNo" capitalize  ng-blur="getDamageHistory(data.ODLN[0].U_EqNo);" class="form-control" id="Text5"
                                placeholder="Equipment No">
                        </div>
                    </div>

                   

                    
                    

                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Survey Result
                        </label>
                        <div class="col-sm-8">
                       
                         <select class="form-control"  ng-model="data.ODLN[0].U_SResult" >
                         <option value="Accepted">Accepted</option>
                         <option value="Rejected">Rejected</option></select>
                            
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Number of Photos
                        </label>
                        <div class="col-sm-8">
                            <input type="text" ng-model="data.ODLN[0].U_NoPh" class="form-control" id="Text6"
                                placeholder="No of Photos">
                        </div>
                    </div>
                    

                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Remarks
                        </label>
                        <div class="col-sm-8">
                        <textarea class="form-control" capitalize ng-model="data.ODLN[0].Comments"></textarea>
                           
                        </div>
                    </div>
                    
                </div>
                <div class="row top-buffer">
                </div>
                <!-- table start -->
                <%--<h3>Damage History</h3>--%>
                <%--<div class="row">
                <table class="table table-bordered table-striped">
                                <thead>
                                    <tr style="background-color:#00a7db;color:#fff">
                                        <th>
                                            Survey Date
                                        </th>
                                        <th>
                                            Surveyor ID
                                        </th>
                                        <th>
                                            Surveyor type
                                        </th>
                                       
                                        <th>
                                            Country</th>
                                        <th>
                                            City
                                        </th>
                                        <th>
                                            Location</th>
                                        
                                        <th>
                                            Survey No
                                        </th>
                                        <th >
                                            Customer Referene
                                        </th>
                                        <th >
                                            Survey Result
                                        </th>
                                        
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr  ng-repeat="d in DamageHistory.ODLN">
                                        <td>
                                            {{d.DocDate}}
                                        </td>
                                        <td>
                                           {{d.U_SurveyorID}}
                                        </td>
                                        <td>
                                            {{d.U_SURVEYTYPE}}
                                        </td>
                             <td>
                                            {{d.U_Country}}</td>
                                        <td>
                                            {{d.U_City}}
                                        </td>
                                        <td>
                                            {{d.U_Loc}}</td>
                                        
                                        <td>
                                            {{d.U_SURVYNO}}
                                        </td>
                                        <td >
                                            {{d.CUST_REF}}
                                        </td>
                                        <td >
                                            {{d.U_SResult}}
                                        </td>
                                        
                                    </tr>
                            
                                   
                                </tbody>
                            </table></div>--%>
                            <div class="row" align="right">
                          <span>
                                    <button type="button" class="btn btn-success" ng-click="save();" ng-model="savebtn" ng-show="Showsavebtn" >
                                        {{savelable}}</button> <button type="button" class="btn btn-success" ng-click="update();" ng-model="updatebtn" ng-show="updatebtn" >
                                        Update</button></span> <span>
                                            <button class="btn btn-primary"  ng-disabled="btndisable">
                                               Print</button></span> <span>
                                                            <button type="button" class="btn btn-danger" ng-click="resetall();">
                                                                Cancel</button></span></div>
                
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
<script type="text/javascript">    $('.datepickersub').datepicker({ autoclose: true, format: 'dd/mm/yyyy',todayHighlight: true });</script>

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
    <td><button ng-click="findone($index)" class="btn btn-sm btn-primary" data-dismiss="modal">Select</button></td>
  </tr>
</table>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
      </div>
    </div>

  </div>
</div>

 <!-- Modal2 -->
<div id="myModal2" class="modal fade" role="dialog">
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
  <tr ng-repeat="d in surveyor.SUID">
    <td>{{d.U_SCode}}</td>
    <td>{{d.U_SName}}</td>
    <td><button ng-click="Fillsurveyor(d.U_SCode,d.U_SName)" class="btn btn-sm btn-primary" data-dismiss="modal">Select</button></td>
  </tr>
</table>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
      </div>
    </div>

  </div>
</div>

<!-- Modal3 -->
<div id="Modal3" class="modal fade" role="dialog">
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
    <td>Name</td>
    <td>Action</td>
  </tr>
  <tr ng-repeat="d in surveyorAgent.EXAGENT">
    <td>{{d.U_ANAME}}</td>
    <td><button ng-click="FillsurveyorAgent(d.U_ANAME)" class="btn btn-sm btn-primary" data-dismiss="modal">Select</button></td>
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
    <td>Company Code</td>
    <td>Customer Name</td>
    <td>Project Code</td>
    <td>Tel No</td>
    <td>Fax No</td>
    <td>Mobile No</td>
    <td>Email</td>
    <td></td>
    
    
  </tr>
  <tr ng-repeat="d in findresult.ODLN">
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
