<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="salsequote.aspx.vb" Inherits="WebApplication1.salsequote"
    MasterPageFile="~/Site.Master" %>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <script src="asset/js/services/DH_service.js" ></script>
    <script src="asset/js/ctrl/DamageHistory_ctrl.js"></script>

    <div class="forms" ng-controller="DamageHistory_ctrl">
        <div class="form-grids row widget-shadow" data-example-id="basic-forms">
            <div class="form-title">
                <div class="col-lg-5">
                    <h4>
                        Damage History :</h4>
                </div>
                
            </div>
            
          
         
            <form id="ss" runat="server">
            <div class="form-body">
                <div class="col-lg-6">
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                           Equipment No
                        </label>
                        <div class="col-sm-8">
                          
                            <input type="text" ng-model="U_SurvyNo" class="form-control" />
                           
                        </div>
                    </div>
                   
                </div>
                
                <div class="row top-buffer">
                </div>
                <!-- table start -->
                <button type="button" class="btn-success btn" ng-click="findDH();">Search</button>
                <div class="row">
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
                                    <tr  ng-repeat="d in DH">
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
                                            {{getcountryName(d.U_Country)}}</td>
                                        <td>
                                            {{getcityName(d.U_City)}}
                                        </td>
                                        <td>
                                            {{getlocationName(d.U_Loc)}}</td>
                                        
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
                            </table>
                            <div align="center" class="text-danger"  ng-show="!DH.length"> <strong> No Data...</strong></div>
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
