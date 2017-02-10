<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="salsequote.aspx.vb" Inherits="WebApplication1.salsequote"
    MasterPageFile="~/Site.Master" %>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <script src="asset/js/services/SO_service.js" ></script>
    <script src="asset/js/ctrl/salseorder_ctrl.js"></script>

    <div class="forms" ng-controller="salseorder_ctrl">
        <div class="form-grids row widget-shadow" data-example-id="basic-forms">
            <div class="form-title">
                <div class="col-lg-5">
                    <h4>
                        Sales Order :</h4>
                </div>
                <div class="btn-group pull-right" role="group"  aria-label="Basic example">
                    <button type="button" class="btn btn-white"  ng-click="FirstRecord();" title="First Data Record">
                        <i class="fa fa-fast-backward"></i>
                    </button>
                    <button type="button" class="btn btn-white" ng-click="PreviousRecord();" title="Previous Record">
                        <i class="fa fa-arrow-left"></i>
                    </button>
                    <button type="button" class="btn btn-white" ng-click="addclick();" title="Add">
                        <i class="fa fa-plus"></i>
                    </button>
                    <button type="button" class="btn btn-white" ng-click="findclick();" title="Find" >
                        <i class="fa fa-search"></i>
                    </button>
                    <button type="button" class="btn btn-white" ng-click="NextRecord();" title="Next Record">
                        <i class="fa fa-arrow-right"></i>
                    </button>
                    <button type="button" class="btn btn-white" ng-click="LastRecord();" title="Last Data Record">
                        <i class="fa fa-fast-forward"></i>
                    </button>
                </div>
            </div>
           
            <form id="ss" runat="server">
            <div class="form-body">
                <div class="col-lg-6">
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Order Number
                        </label>
                        <div class="col-sm-8">
                            <span ng-hide="findbtn" >{{data.ORDR[0].U_OrderNo}}</span>
                            <span ng-show="findbtn"><input type="text" ng-model="data.ORDR[0].U_Qno" class="form-control"
                                id="Text22"></span>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Status
                        </label>
                        <div class="col-sm-8">
                            {{data.ORDR[0].U_Status}}
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            User Name
                        </label>
                        <div class="col-sm-8">
                            
                                <input type="text" ng-model="data.ORDR[0].U_UName" ng-disabled="true" class="form-control"
                                id="username" value="" placeholder="">
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Create Date
                        </label>
                        <div class="col-sm-8">
                            <input type="text" ng-model="data.ORDR[0].U_Cdate" ng-disabled="updatedisabled" class="form-control datepicker"
                                id="Email2" placeholder="Create Date">
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Order Date
                        </label>
                        <div class="col-sm-8">
                            <input type="text" ng-model="data.ORDR[0].DocDate" ng-disabled="updatedisabled" class="form-control datepicker"
                                id="Email3" placeholder="Order Date">
                        </div>
                        
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Company Code
                        </label>
                        <div class="col-sm-8">
                          <div class="input-group">
                            <input ng-disabled="updatedisabled" type="text" ng-model="data.ORDR[0].CardCode" ng-keydown="CCselected=false" class="form-control" id="Email6"
                                placeholder="Company Code">
                                <span class="input-group-btn">
        <button class="btn btn-info" type="button" ng-disabled="updatedisabled" ng-click="getcustomer('code');"><i class="fa fa-search" aria-hidden="true"></i></button>
      </span>
      </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Company Name
                        </label>
                        <div class="col-sm-8">
                        <div class="input-group">
                         <input ng-disabled="updatedisabled" type="text" ng-model="data.ORDR[0].CardName" ng-keydown="CCselected=false"  class="form-control" id="Email7"
                                placeholder="Company Name">
      <span class="input-group-btn">
        <button class="btn btn-info" type="button" ng-disabled="updatedisabled" ng-click="getcustomer('name');"><i class="fa fa-search" aria-hidden="true"></i></button>
      </span>

                           
                                </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Surveyor ID
                        </label>
                         <div class="col-sm-8">
                          <div class="input-group" >
                            <input ng-disabled="updatedisabled" type="text" ng-model="data.ORDR[0].U_SurveyorID" ng-keydown="SIselected=false" class="form-control" id="Text1"
                                placeholder="Surveyor ID">
                                <span class="input-group-btn">
        <button class="btn btn-info" ng-disabled="updatedisabled" type="button"  ng-click="getsurveyorID();" ><i class="fa fa-search" aria-hidden="true"></i></button>
      </span>
      </div>
                        </div>
                        
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Surveyor Name
                        </label>
                         <div class="col-sm-8">
                          {{getSurveyorName(data.ORDR[0].U_SurveyorID);}}

                        </div>
                        
                    </div>

                    
                </div>
                <div class="col-lg-6">
                <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Customer Reference
                        </label>
                        <div class="col-sm-8">
                            <input type="text" ng-model="data.ORDR[0].NumAtCard" capitalize class="form-control" id="Email5"
                                placeholder="Customer Reference">
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Survey Type
                        </label>
                        <div class="col-sm-8">
                       
                         <select class="form-control" ng-disabled="updatedisabled" ng-options="item.U_Stype as item.U_StypeName for item in stype" ng-model="data.ORDR[0].U_STypeCode" ></select>
                            
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Country 
                        </label>
                        <div class="col-sm-8">
                             <select class="form-control" ng-options="item.U_Country as item.U_CountryName for item in CountryMaster" ng-disabled="updatedisabled" ng-model="data.ORDR[0].U_Country" ng-change="getcity(data.ORDR[0].U_Country);"></select>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            City
                        </label>
                        <div class="col-sm-8">
                            <select ng-disabled="updatedisabled" class="form-control" ng-options="item.U_City as item.U_CityName for item in city" ng-model="data.ORDR[0].U_City" ng-change="getlocation(data.ORDR[0].U_City);"></select>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Location
                        </label>
                        <div class="col-sm-8">
                             <select ng-disabled="updatedisabled" class="form-control" ng-options="item.U_Loc as item.U_LocName for item in location" ng-model="data.ORDR[0].U_Loc"></select>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            IMO
                        </label>
                        <div class="col-sm-8">
                             <select ng-disabled="updatedisabled" class="form-control" ng-options="item.Name as item.Name for item in IMO" ng-model="data.ORDR[0].U_IMO"></select>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Project Code
                        </label>
                         <div class="col-sm-8">
                          <div class="input-group">
                            <input ng-disabled="updatedisabled" capitalize type="text" ng-model="data.ORDR[0].U_Ccode" class="form-control" id="Text3"
                                placeholder="Project Code">
                                <span class="input-group-btn">
        <button class="btn btn-info" ng-disabled="updatedisabled" type="button" ><i class="fa fa-search" aria-hidden="true"></i></button>
      </span>
      </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Remarks
                        </label>
                        <div class="col-sm-8">
                        <textarea class="form-control" capitalize ng-model="data.ORDR[0].Comments"></textarea>
                           
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
                                            Quantity
                                        </th>
                                        <th>
                                            Planned Date
                                        </th>
                                        <th>
                                            <span>Equipment Type</span></th>
                                        <th>
                                            <span>Survey Criteria</span></th>
                                        <th>
                                            <span>Survey Details</span>
                                        </th>
                                        <th>
                                            <span>Line Status</span></th>
                                        <th>
                                            <span>Open quantity</span></th>
                                        
                                        
                                        <th colspan="3">
                                            Action
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr ng-repeat="d in data.SQTOGEN">
                                        <th scope="row">
                                            {{$index+1}}
                                        </th>
                                        <td>
                                            <span ng-hide="d.edit">{{d.Quantity}}</span> <span ng-show="d.edit">
                                                
                                              <input value"" type="number" ng-model="d.Quantity" class="form-control" /></select>
                                                
                                               
                                            </span>
                                        </td>
                                        <td>
                                            <span ng-hide="d.edit">{{d.U_PDate}}</span> <span ng-show="d.edit">
                                           <input value"" type="text" ng-model="d.U_PDate" class="form-control datepicker" />
                                           
                                              
                                        </td>
                                        <td>
                                            <span ng-hide="d.edit">{{getegroupName(d.U_EQType)}}</span> <span ng-show="d.edit">
                                           <select class="form-control" ng-disabled="updatedisabled" ng-options="item.U_EQTYPECODE as item.U_EQTYPENAME for item in etype" ng-model="d.U_EQType" ></select>
                                           </span>
                                        </td>
                                        <td>
                                            <span ng-hide="d.edit">{{geteSCname(d.U_SCriteria)}}</span> <span ng-show="d.edit">
                                           <select class="form-control" ng-disabled="updatedisabled" ng-options="item.U_SURCRTACODE as item.U_SURCRTANAME for item in SurveyCriteria" ng-model="d.U_SCriteria" ></select>
                                         
                                        </td>
                                        <td>
                                           <a  href="#" ng-click="checksurvey();"> <i class="fa fa-arrow-right fa-2x" style="color:#faba0a" aria-hidden="true"></i></a></td>
                                        <td>
                                              <input ng-model="d.LineStatus" type="text" ng-disabled="true" class="form-control" />
                                            
                                        </td>

                                        <td>
                                            <input ng-model="d.OpenQty" type="text" ng-disabled="true" class="form-control" />
                                           
                                        </td>
                                      
                                      
                                        <td>
                                            <span ng-hide="d.edit" class="btn btn-success btn-sm" ng-click="g_edit($index);"><i
                                                class="fa fa-pencil"></i></span><span ng-show="d.edit" class="btn btn-primary btn-sm"
                                                    ng-click="g_ok($index);"><i class="fa fa-plus"></i></span>
                                        </td>
                                        <td>
                                            <span class="btn btn-danger btn-sm" ng-click="g_del($index);"><i class="fa fa-trash-o">
                                            </i></span>
                                        </td>
										 <td>
                                            <span class="btn btn-info btn-sm" ng-click="g_copy($index);"><i class="fa fa-copy"></i></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th scope="row">
                                        </th>
                                        <td>
                                        <input value"" type="number" ng-model="Quantity" class="form-control" /></select>
                                      
                                        </td>
                                        <td>
                                        <input value"" type="text" ng-model="U_PDate" class="form-control datepicker"  />
                                        
                                        </td>
                                        <td>
                                         <select class="form-control"  ng-options="item.U_EQTYPECODE as item.U_EQTYPENAME for item in etype" ng-model="U_EQType" ></select>
                                         
                                        </td>
                                        <td>
                                         <select class="form-control" ng-options="item.U_SURCRTACODE as item.U_SURCRTANAME for item in SurveyCriteria"  ng-model="U_SCriteria" ></select>
                                           
                                        </td>
                                        <td align="center">
                                       <i class="fa fa-arrow-right fa-2x" style="color:#faba0a" aria-hidden="true"></i>

                                           
                                        </td>
                                        <td>
                                         <input ng-model="g_rewmark" type="text" ng-disabled="true" class="form-control" />
                                            
                                        </td>
                                        <td>
                                         <input ng-model="Open_rewmark" type="text" ng-disabled="true" class="form-control" />
                                            
                                        </td>
                                      
                                       
                                        <td>
                                            <span class="btn btn-primary btn-sm" ng-click="g_add();"><i class="fa fa-plus"></i>
                                            </span>
                                        </td>
                                        <td colspan="2" >
                                            <span class="btn btn-danger btn-sm" ng-click="g_reset();"><i class="fa fa-times"></i>
                                            </span>
                                        </td>
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
                                        <th>Charge Type</th>
                                       
                                        <th>
                                            Date
                                        </th>
                                        <th>
                                            Line Status
                                        </th>
                                        
                                        <th colspan="3">
                                            Action
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr ng-repeat="d in data.SQTOADD">
                                        <th scope="row">
                                            {{$index+1}}
                                        </th>
                                        <td>
                                            <span ng-hide="d.edit">{{d.U_ChrgName}}</span> <span ng-show="d.edit">
                                             <select class="form-control" ng-options="item as item.U_StypeName for item in ChargeType" ng-model="d.U_Ctype" ></select>
                                                                                            
                                            </span>
                                        </td>
                                       
                                        <td>
                                            <span ng-hide="d.edit">{{d.U_PDate}}</span> <span ng-show="d.edit">
                                           <input value"" type="text" ng-model="d.U_PDate" class="form-control datepicker" />
                                           </span>
                                        </td>
                                        <td>
                                            <span ng-hide="d.edit">{{d.LineStatus}}</span> <span ng-show="d.edit">
                                             <input value"" type="text" ng-model="d.LineStatus" class="form-control" />
                                           </span>
                                            
                                         
                                        </td>
                                       
                                      
                                        <td>
                                           
                                        </td>
										 <td>
                                          </td>
                                    </tr>
                                    <tr>
                                        <th scope="row">
                                        </th>
                                        <td>                                        
                                         <select class="form-control" ng-options="item as item.U_StypeName for item in ChargeType" ng-model="a_Ctype" ></select>
                                           
                                        </td>
                                         <td>
                                       <input value"" type="text" ng-model="U_PDate" class="form-control datepicker" />
                                         
                                        </td>
                                        <td>
                                         <input ng-model="g_rewmark" type="text" ng-disabled="true" class="form-control" />
                                         
                                        </td>
                                       
                                      
                                        <td>
                                            <span class="btn btn-primary btn-sm" ng-click="a_add();"><i class="fa fa-plus"></i>
                                            </span>
                                        </td>
                                        <td colspan="2">
                                            <span class="btn btn-danger btn-sm" ng-click="a_reset();"><i class="fa fa-times"></i>
                                            </span>
                                        </td>
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
                                            #
                                        </th>
                                        <th>
                                            Path
                                        </th>
                                        <th>
                                            File Name
                                        </th>
                                        <th>
                                            Attachment Date
                                        </th>
                                        <th>Action</th>
                                        
                                    </tr>
                                     <tr  ng-repeat="d in data.ATTACHMENT" >
                                        <td>
                                            {{$index+1}} 
                                        </td>
                                        <td>
                                            {{d.U_FilePath}}
                                        </td>
                                        <td>
                                            {{d.U_FileName}}
                                        </td>
                                        <td>
                                            {{d.U_Date}}
                                        </td>
                                        <td><a href="{{fileBurl+d.U_FileName}}" download><button type="button" class="btn btn-info"><i class="fa fa-download" aria-hidden="true"></i></button></a></td>
                                        
                                    </tr>
                                    
                                </thead>
                                
                            </table>
                             <div class="form-body">
                <div class="col-lg-12" ng-show="Fileloading">
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                           Select files to attach
                        </label>
                        <div class="col-sm-8">
                            
                           <div class="input-group">
                           <form method="post" id="fileinfo" name="fileinfo">
                              <input type="file" id="fileopen" file-model="myFile" class="form-control"/>
                            
                <label class="input-group-btn">
                    <span class="btn btn-primary" ng-click="uploadFile();" >
                        Upload&hellip; 
                    </span>
                    </form>
                </label>
             
            </div>
                        </div>
                    </div>
                  
                   
                </div>
                <div class="col-lg-12" ng-hide="Fileloading"><div class="progress" style="height:40px;">
    <div class="progress-bar progress-bar-striped active" role="progressbar" aria-valuenow="100" aria-valuemin="0" aria-valuemax="100" style="width:100%;font-size:20px;line-height:25px;">
      Loading...
    </div>
  </div></div>
                </div>
                        </div>
                    </div>
                    <div class="row top-buffer">
                    </div>
                    <div class="row" align="right">
                        <span>
                            <button class="btn btn-success" type="button" ng-click="save();"  ng-disabled="loading" ng-show="savebtn">
                                {{savelable}}</button>
                                
                                <button type="button" class="btn btn-primary" ng-click="FindRecord();"  ng-show="findbtn">
                                Find</button>

                                 
                                <button type="button" class="btn btn-success" ng-disabled="data.ORDR[0].U_Status=='Closed'" ng-click="UpdateRecord();"  ng-show="updatebtn">
                                Update</button>
                                
                               
                                                            <button type="button" class="btn btn-danger" ng-click="resetall();">
                                                                Cancel</button></span></div>
                </div>
            </div>
            </form>
        </div>
    </div>
    <script type="text/javascript">        $('.datepicker').datepicker({ autoclose: true, format: 'dd/mm/yyyy',todayHighlight: true });</script>
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
    <td><button ng-click="Fillcustomer(d.U_Ccode,d.U_Cname)" class="btn btn-sm btn-primary" data-dismiss="modal">Select</button></td>
  </tr>
</table>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
      </div>
    </div>

  </div>
</div>

 <!-- CheckSModal -->
<div id="checkSModal" class="modal fade" role="dialog">
  <div class="modal-dialog" style="width:90%">

    <!-- Modal content-->
    <div class="modal-content" >
      <div class="modal-header">        
        <button type="button" class="close" data-dismiss="modal">&times;</button>
        <h4 class="modal-title">Select Any One Record</h4>
      </div>
      <div class="modal-body" style="overflow:auto;height:400px;">
      <div>Sales Order No : {{mySdata.OCRD[0].DocEntry}}<br />Open Quantity : {{mySdata.OCRD[0].OpenQty}}</div>
      <div align="center"><button type="button" class="btn-info btn" ng-disabled="mySdata.OCRD[0].OpenQty==0" ng-click="gotoSurveyFrom(mySdata.OCRD[0].FormName);">New Survey</button></div><br />
        <table width="100%" border="0" class="table table-bordered table-responsive" cellspacing="5" cellpadding="5">
  <tr>
    <td>Liink</td>
    <td>Survey No</td>
    <td>User Name</td>
    <td>Survey Date</td>
    <td>Company Code</td>
    <td>Company Name</td>
    <td>Survey Type</td>
    <td>Location</td>
    <td>Survey Result</td>
  </tr>
  <tr ng-repeat="d in mySdata.DETAILS">
    <td><i class="fa fa-arrow-right fa-2x" ng-click="gotoSurveyFromByLink(d.Survey_No);" style="color:#faba0a;cursor:pointer;" aria-hidden="true"></i></td>
    <td>{{d.Survey_No}}</td>
    <td>{{d.User_Name}}</td>
    <td>{{d.Survey_Date}}</td>
    <td>{{d.Customer_Code}}</td>
    <td>{{d.Customer_Name}}</td>
    <td>{{d.Survey_Type}}</td>
    <td>{{d.Location}}</td>
    <td>{{d.Survey_Result}}</td>
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
    <td>Order No</td>
    <td>Status</td>
    <td>User Name</td>
    <td>Creation Date</td>
    <td>Company Code</td>
    <td>Company Name</td>
    <td>Project Code</td>
    <td>City</td>
    <td>Country</td>
    <td>Surveyor ID</td>
    <td>Email</td>
    <td></td>
    
    
  </tr>
  <tr ng-repeat="d in findresult.ORDR">
   <td>{{d.U_OrderNo}}</td>
    <td>{{d.U_Status}}</td>
    <td>{{d.U_UName}}</td>
    <td>{{d.U_Cdate}}</td>
    <td>{{d.CardCode}}</td>
    <td>{{d.CardName}}</td>
    <td>{{d.Project}}</td>
    <td>{{d.U_City}}</td>
    <td>{{d.U_Country}}</td>
    <td>{{d.U_SurveyorID}}</td>
    <td><button ng-click="SetRecord(d);" class="btn btn-sm btn-primary" data-dismiss="modal">Select</button></td>
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
