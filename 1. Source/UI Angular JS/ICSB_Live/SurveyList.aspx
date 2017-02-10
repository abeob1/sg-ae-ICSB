<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="salsequote.aspx.vb" Inherits="WebApplication1.salsequote"
    MasterPageFile="~/Site.Master" %>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <script src="asset/js/services/SL_service.js" ></script>
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
                          
                            <input type="text" ng-model="U_SurvyNo" class="form-control" />
                           
                        </div>
                    </div>
                   
                </div>
                
                <div class="row top-buffer">
                </div>
                <!-- table start -->
                <button type="button" class="btn-success btn" ng-click="findSL();">Search</button>
                <div class="row">
                <table width="100%" border="0" class="table table-bordered table-responsive" cellspacing="5" cellpadding="5">
  <tr>
    <td>Liink</td>
    <td>Survey No</td>
    <td>User Name</td>
    <td>Survey Date</td>
    <td>Customer Code</td>
    <td>Customer Name</td>
    <td>Survey Type</td>
    <td>Location</td>
    <td>Survey Result</td>
  </tr>
  <tr ng-repeat="d in SL">
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
                            <div align="center" class="text-danger"  ng-show="!SL.length"> <strong> No Data...</strong></div>
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






</asp:Content>
