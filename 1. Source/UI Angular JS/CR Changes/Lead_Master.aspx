<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="salsequote.aspx.vb" Inherits="WebApplication1.salsequote"
    MasterPageFile="~/Site.Master" %>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <script src="asset/js/ctrl/leadmaster_ctrl.js" ></script>
    <div class="forms" ng-controller="leadmaster_ctrl">
        <div class="form-grids row widget-shadow" data-example-id="basic-forms">
            <div class="form-title">
                <div class="col-lg-5">
                    <h4>
                        Lead Master:</h4>
                </div>
                <div class="btn-group pull-right" role="group" aria-label="Basic example">
                    <button type="button" class="btn btn-white" ng-click="LeadFirstRecord();" title="First Data Record">
                        <i class="fa fa-fast-backward"></i>
                    </button>
                    <button type="button" class="btn btn-white" ng-click="LeadPreviousRecord();" title="Previous Record">
                        <i class="fa fa-arrow-left"></i>
                    </button>
                    <button type="button" class="btn btn-white" ng-click="reset();" title="Add">
                        <i class="fa fa-plus"></i>
                    </button>
                    <button type="button" class="btn btn-white" ng-click="reset();findbtn=true;saveb=false;update=false;code = false;"  title="Find">
                        <i class="fa fa-search"></i>
                    </button>
                    <button type="button" class="btn btn-white" ng-click="LeadNextRecord();" title="Next Record">
                        <i class="fa fa-arrow-right"></i>
                    </button>
                    <button type="button" class="btn btn-white" ng-click="LeadLastRecord();" title="Last Data Record">
                        <i class="fa fa-fast-forward"></i>
                    </button>
                </div>
            </div>
            <div class="form-body">
                <div class="col-lg-6">
                  <div class="form-group">
                        <label for="inputEmail3" class="col-sm-3 control-label">
                            Company Code</label>
                        <div class="col-sm-8">
                            <input type="text" tabindex="1" ng-model="data.LEADM[0].Code" class="form-control" id="Email1"  ng-disabled="code" placeholder="Company Code">
                        </div>
                    </div>
                </div>
                <div class="col-lg-6">
                  <div class="form-group">
                        <label for="inputEmail3" class="col-sm-3 control-label">
                            Address Line1</label>
                        <div class="col-sm-8">
                            <input type="text" tabindex="7" ng-model="data.LEADM[0].U_Addr1" class="form-control" id="Text1" placeholder="Address Line1">
                        </div>
                    </div>
                </div>
                
                <div class="row top-buffer"></div>
                <div class="col-lg-6">
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-3 control-label">
                            Company  Name</label>
                        <div class="col-sm-8">
                            <input type="text" tabindex="2" ng-model="data.LEADM[0].Name" class="form-control" id="inputEmail3" placeholder="Company Name">
                        </div>
                    </div>
                </div>
                <div class="col-lg-6">
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-3 control-label">
                           Address Line2</label>
                        <div class="col-sm-8">
                            <input type="text" ng-model="data.LEADM[0].U_Addr2" tabindex="8" class="form-control" id="Text2" placeholder="Address Line2">
                        </div>
                    </div>
                </div>
                
                <div class="row top-buffer"></div>
                <div class="col-lg-6">
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-3 control-label">
                            Tel Number</label>
                        <div class="col-sm-8">
                            <input type="text" tabindex="3" ng-model="data.LEADM[0].U_TelNo" class="form-control" id="Email3" placeholder="Phone">
                        </div>
                    </div>
                </div>
                <div class="col-lg-6">
                  <div class="form-group">
                        <label for="inputEmail3" class="col-sm-3 control-label">
                            Address Line3</label>
                        <div class="col-sm-8">
                            <input type="text" tabindex="9" ng-model="data.LEADM[0].U_Addr3" class="form-control" id="Text3" placeholder="Address Line3">
                        </div>
                    </div>
                </div>
                
                <div class="row top-buffer"></div>
                <div class="col-lg-6">
                  <div class="form-group">
                        <label for="inputEmail3" class="col-sm-3 control-label">
                            Fax Number</label>
                        <div class="col-sm-8">
                            <input type="text" tabindex="4" ng-model="data.LEADM[0].U_FaxNo" class="form-control" id="Email4" placeholder="Fax">
                        </div>
                    </div>
                </div>
                <div class="col-lg-6">
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-3 control-label">
                           Address Line4</label>
                        <div class="col-sm-8">
                            <input type="text" tabindex="10" ng-model="data.LEADM[0].U_Addr4" class="form-control" id="Text4" placeholder="Address Line4">
                        </div>
                    </div>
                </div>
                <div class="row top-buffer"></div>
                <div class="col-lg-6">
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-3 control-label">
                            Mobile Number</label>
                        <div class="col-sm-8">
                            <input type="text" tabindex="5" ng-model="data.LEADM[0].U_MNo" class="form-control" id="Email5" placeholder="Mobile No">
                        </div>
                    </div>
                </div>
                <div class="col-lg-6">
                  <div class="form-group">
                        <label for="inputEmail3" class="col-sm-3 control-label">
                            Address Line5</label>
                        <div class="col-sm-8">
                            <input type="text" tabindex="11" ng-model="data.LEADM[0].U_Addr5" class="form-control" id="Text5" placeholder="Address Line5">
                        </div>
                    </div>
                </div>
                <div class="row top-buffer"></div>
                <div class="col-lg-6">
                  <div class="form-group">
                        <label for="inputEmail3" class="col-sm-3 control-label">
                            Email</label>
                        <div class="col-sm-8">
                            <input type="email" tabindex="6" ng-model="data.LEADM[0].U_Email"class="form-control" id="Email2" placeholder="Email">
                        </div>
                    </div>
                </div>
                <div class="col-lg-6">
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-3 control-label">
                           Address Line6</label>
                        <div class="col-sm-8">
                            <input type="text" tabindex="12" ng-model="data.LEADM[0].U_Addr6" class="form-control" id="Text6" placeholder="Address Line6">
                        </div>
                    </div>
                </div>
                <div class="row top-buffer"></div>
                <div class="row" align="center">
                <span>
                    <button class="btn btn-success" ng-disabled="savebtn" ng-show="saveb" ng-click="save();">{{savelable}}</button>
                    <button class="btn btn-primary" ng-show="findbtn" ng-click="find();">Find</button>
                    <button class="btn btn-success" ng-show="update" ng-click="updaterecord();">Update</button>
                    </span>
                <span><button class="btn btn-danger" ng-click="reset();">Cancel</button></span> 
                </div>
            </div>
        </div>
    </div>
    
<!-- Modal -->
<div id="myModal" class="modal fade" role="dialog">
  <div class="modal-dialog">

    <!-- Modal content-->
    <div class="modal-content" >
      <div class="modal-header">        
        <button type="button" class="close" data-dismiss="modal">&times;</button>
        <h4 class="modal-title">Select Any One Record</h4>
      </div>
      <div class="modal-body">
        <table width="100%" border="0" class="table table-bordered table-responsive" cellspacing="5" cellpadding="5">
  <tr>
    <td>Code</td>
    <td>Name</td>
    <td>Action</td>
  </tr>
  <tr ng-repeat="d in fulldata.LEADM">
    <td>{{d.Code}}</td>
    <td>{{d.Name}}</td>
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
</asp:Content>
