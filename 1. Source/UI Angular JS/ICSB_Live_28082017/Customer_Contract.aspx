<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="salsequote.aspx.vb" Inherits="WebApplication1.salsequote"
    MasterPageFile="~/Site.Master" %>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
<script src="asset/js/services/CS_service.js" ></script>
    <script src="asset/js/ctrl/CustomerContract_ctrl.js"></script>

    <div class="forms" ng-controller="CustomerContract_ctrl">
        <div class="form-grids row widget-shadow" data-example-id="basic-forms">
            <div class="form-title">
                <div class="col-lg-5">
                    <h4>
                        Customer Contract : </h4>
                </div>
                <div class="btn-group pull-right" role="group" aria-label="Basic example">
                    <button type="button" class="btn btn-white"  ng-click="FirstRecord();" title="First Data Record">
                        <i class="fa fa-fast-backward"></i>
                    </button>
                    <button type="button" class="btn btn-white" ng-click="PreviousRecord();" title="Previous Record">
                        <i class="fa fa-arrow-left"></i>
                    </button>
                    <button type="button" class="btn btn-white" ng-click="addclick();" title="Add">
                        <i class="fa fa-plus"></i>
                    </button>
                    <button type="button" class="btn btn-white" ng-click="duplicateclick();" title="Duplicate">
                        <i class="fa fa-files-o"></i>
                    </button>
                    <button type="button" class="btn btn-white" ng-click="findclick();" title="Find" >
                        <i class="fa fa-search"></i>
                    </button>
                    <button type="button" class="btn btn-white" ng-click="NextRecord();"  title="Next Record">
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
                            Contract No
                        </label>
                        <div class="col-sm-8">
                            <span ng-hide="findbtn" >{{data.CCON[0].U_Qno}}</span>
                            <span ng-show="findbtn"><input type="text" ng-model="data.CCON[0].U_Qno" class="form-control"
                                id="Text22"></span>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Status
                        </label>
                        <div class="col-sm-8">
                            {{data.CCON[0].U_Status}}
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            User Name
                        </label>
                        <div class="col-sm-8">
                            
                                <input type="text" ng-model="data.CCON[0].U_Uname" ng-disabled="true" class="form-control"
                                id="username" value="{{UserData[0].Name}}" placeholder="">
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Create Date
                        </label>
                        <div class="col-sm-8">
                            <input type="text" ng-model="data.CCON[0].U_Cdate" class="form-control datepicker"
                                id="Email2" placeholder="Create Date">
                        </div>
                    </div>
                    
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Company Code
                        </label>
                        <div class="col-sm-8">
                          <div class="input-group">
                            <input type="text" ng-keydown="CCselected=false" ng-model="data.CCON[0].U_Ccode" class="form-control" id="Email6"
                                placeholder="Company Code">
                                <span class="input-group-btn">
        <button class="btn btn-info" type="button" ng-click="getcustomer('code');"><i class="fa fa-search" aria-hidden="true"></i></button>
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
                         <input type="text" ng-keydown="CCselected=false" ng-model="data.CCON[0].U_Cname"  class="form-control" id="Email7"
                                placeholder="Company Name">
      <span class="input-group-btn">
        <button class="btn btn-info" type="button" ng-click="getcustomer('name');"><i class="fa fa-search" aria-hidden="true"></i></button>
      </span>

                           
                                </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Contract Period
                        </label>
                        <div class="col-sm-4">
                            <input type="text" ng-model="data.CCON[0].U_CPeriod1" ng-disabled="whilefind" class="form-control datepicker"
                                id="Email8" placeholder="Period From">
                        </div>
                        <div class="col-sm-4">
                            <input type="text" ng-model="data.CCON[0].U_CPeriod2" ng-disabled="whilefind" class="form-control datepicker"
                                id="Email4" placeholder="Period To">
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Project Code
                        </label>
                        <div class="col-sm-8">
                            <input type="text" ng-model="data.CCON[0].U_Pcode" class="form-control" id="Email5"
                                placeholder="Project Code">
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmailss" class="col-sm-4 control-label">
                            Remarks
                        </label>
                        <div class="col-sm-8">
                        <textarea class="form-control" ng-model="data.CCON[0].U_Remarks"></textarea>
                            
                        </div>
                    </div>
                </div>
                <div class="col-lg-6">
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Address Name
                        </label>
                        <div class="col-sm-8">
                        <select class="form-control"  ng-options="item.U_AddrN as item.U_AddrN for item in addressarray" ng-model="U_AddrN" ng-change="setaddress(U_AddrN);" ></select>
                        
                            
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Address Line1
                        </label>
                        <div class="col-sm-8">
                            <input type="text" ng-model="data.CCON[0].U_Addr1" ng-disabled="whilefind" class="form-control" id="Email19"
                                placeholder="Address Line1">
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Address Line2
                        </label>
                        <div class="col-sm-8">
                            <input type="text" ng-model="data.CCON[0].U_Addr2" ng-disabled="whilefind" class="form-control" id="Email9"
                                placeholder="Address Line2">
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Address Line3
                        </label>
                        <div class="col-sm-8">
                            <input type="text" ng-model="data.CCON[0].U_Addr3" ng-disabled="whilefind" class="form-control" id="Email10"
                                placeholder="Address Line3">
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Address Line4
                        </label>
                        <div class="col-sm-8">
                            <input type="text" ng-model="data.CCON[0].U_Addr4" ng-disabled="whilefind" class="form-control" id="Email11"
                                placeholder="Address Line4">
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Address Line5
                        </label>
                        <div class="col-sm-8">
                            <input type="text" ng-model="data.CCON[0].U_Addr5" ng-disabled="whilefind" class="form-control" id="Email13"
                                placeholder="Address Line5">
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Address Line6
                        </label>
                        <div class="col-sm-8">
                            <input type="text" ng-model="data.CCON[0].U_Addr6" ng-disabled="whilefind" class="form-control" id="Email14"
                                placeholder="Address Line6">
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Tel Number
                        </label>
                        <div class="col-sm-8">
                            <input type="text" ng-model="data.CCON[0].U_TelNo" ng-disabled="whilefind" class="form-control" id="Email15"
                                placeholder="Tel Number">
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Fax Number
                        </label>
                        <div class="col-sm-8">
                            <input type="text" ng-model="data.CCON[0].U_FaxNo" ng-disabled="whilefind" class="form-control" id="Email17"
                                placeholder="Fax Number">
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Mobile Number
                        </label>
                        <div class="col-sm-8">
                            <input type="text" ng-model="data.CCON[0].U_Mno" ng-disabled="whilefind" class="form-control" id="Email12"
                                placeholder="Mobile Number">
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Email
                        </label>
                        <div class="col-sm-8">
                            <input type="email" ng-model="data.CCON[0].U_Email" ng-disabled="whilefind" class="form-control" id="Email16"
                                placeholder="Email">
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
                           
                            <table class="table table-bordered table-striped">
                                <thead>
                                    <tr style="background-color:#00a7db;color:#fff">
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
                                        <th style="width: 25px">
                                            Rate
                                        </th>
                                        <th style="width: 25px">
                                            R Rate
                                        </th>
                                        <th>
                                            U.O.M
                                        </th>
                                        <th>
                                            GST %
                                        </th>
                                       
                                        <th colspan="3" ng-inti="filter=false">
                                            <button data-toggle="tooltip" data-placement="bottom" title="Filter"  ng-click="filter= !filter" class="btn-sm btn-white btn" type="button" ><i class="fa fa-filter" aria-hidden="true"></i> Filter</button>
                                        </th>
                                    </tr>
                                </thead>
                                <thead>
                                    <tr ng-show="filter">
                                        <th>
                                            #
                                        </th>
                                        <th>
                                           <select class="form-control" ng-options="item.U_Stype as item.U_StypeName for item in stype" ng-model="searchText.U_Stype" ></select>
                                        </th>
                                        <th>
                                            <select class="form-control" ng-options="item.U_Conti as item.U_ContiName for item in Continent" ng-model="searchText.U_Conti"></select>
                                        </th>
                                        <th>
                                            <select class="form-control" ng-options="item.U_Country as item.U_CountryName for item in CountryMaster" ng-model="searchText.U_Country"></select>
                                        </th>
                                        <th>
                                            <select class="form-control" ng-options="item.U_City as item.U_CityName for item in CityMaster" ng-model="searchText.U_City" ></select>
                                        </th>
                                        <th>
                                            <select class="form-control" ng-options="item.U_EQGroup as item.U_EQGroupName for item in egroup" ng-model="searchText.U_EQGroup"></select>
                                        </th>
                                        <th>
                                            <select class="form-control" ng-options="item.U_Currency as item.U_CurrencyName for item in curency" ng-model="searchText.U_Currency"></select>
                                        </th>
                                        <th style="width: 25px">
                                            <input ng-model="searchText.U_Rate" class="form-control">
                                        </th>
                                        <th style="width: 25px">
                                            <input ng-model="searchText.U_RRate" class="form-control">
                                        </th>
                                        <th>
                                            <select class="form-control" ng-options="item.U_UOM as item.U_UOMName for item in UOM" ng-model="searchText.U_UOM" ></select>
                                        </th>
                                        <th>
                                            <select class="form-control" ng-options="item.U_GST as item.U_GSTName for item in GST" ng-model="searchText.U_GST" ></select>
                                        </th>
                                        
                                        <th colspan="3">
                                          <span class="btn btn-danger btn-sm" ng-click="searchText=''"><i class="fa fa-times"></i> 
                                        </th>
                                    </tr>
                                </thead>

                                <tbody>
                                    <tr ng-repeat="d in data.CCONGEN | filter:searchText">
                                        <th scope="row">
                                            {{$index+1}}
                                        </th>
                                        <td>
                                            <span ng-hide="d.edit">{{getestypeName(d.U_Stype)}}</span> <span ng-show="d.edit">
                                                
                                                <select class="form-control" ng-options="item.U_Stype as item.U_StypeName for item in stype" ng-model="d.U_Stype" ></select>
                                                
                                               
                                            </span>
                                        </td>
                                        <td >
                                            <span ng-hide="d.edit">{{getContinentName(d.U_Conti)}}</span> <span ng-show="d.edit">
                                            <select class="form-control" ng-options="item as item.U_ContiName for item in Continent track by item.U_Conti" ng-model="d.E_Conti" ng-change="getcountry(d.E_Conti.U_Conti);"></select>
                                           
                                              
                                        </td>
                                        <td>
                                            <span ng-hide="d.edit">{{getcountryName(d.U_Country)}}</span> <span ng-show="d.edit">
                                           <select class="form-control" ng-options="item as item.U_CountryName for item in country track by item.U_Country" ng-model="d.E_Country" ng-change="getcity(d.E_Country.U_Country);"></select>
                                           </span>
                                        </td>
                                        <td>
                                            <span ng-hide="d.edit">{{getcityName(d.U_City)}}</span> <span ng-show="d.edit">
                                            <select class="form-control" ng-options="item as item.U_CityName for item in city track by item.U_City" ng-model="d.E_City" "></select>
                                         
                                        </td>
                                        <td>
                                            <span ng-hide="d.edit">{{getegroupName(d.U_EQGroup)}}</span> <span ng-show="d.edit">
                                            
                                             <select class="form-control" ng-options="item.U_EQGroup as item.U_EQGroupName for item in egroup" ng-model="d.U_EQGroup"></select>
                                             
                                            </span>
                                        </td>
                                        <td>
                                            <span ng-hide="d.edit">{{getecurencyName(d.U_Currency)}}</span> <span ng-show="d.edit"> 
                                             <select class="form-control" ng-options="item.U_Currency as item.U_CurrencyName for item in curency" ng-model="d.U_Currency"></select>
                                             </span>
                                        </td>
                                        <td>
                                            <span ng-hide="d.edit">{{d.U_Rate}}</span> <span ng-show="d.edit">
                                           <input value"" type="number" ng-model="d.U_Rate"  class="form-control" /></span>
                                        </td>
                                         <td>
                                            <span ng-hide="d.edit">{{d.U_RRate}}</span> <span ng-show="d.edit">
                                           <input value"" type="number" ng-model="d.U_RRate"  class="form-control" /></span>
                                        </td>
                                        <td>
                                            <span ng-hide="d.edit">{{getuomname(d.U_UOM)}} </span><span ng-show="d.edit">
                                            <select class="form-control" ng-options="item.U_UOM as item.U_UOMName for item in UOM" ng-model="d.U_UOM" ></select>
                                            
                                          </span>
                                        </td>
                                        <td>
                                            <span ng-hide="d.edit">{{getGSTName(d.U_GST)}} </span><span ng-show="d.edit">
                                            <select class="form-control" ng-options="item.U_GST as item.U_GSTName for item in GST" ng-model="d.U_GST" ></select>
                                            </span>
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
                                    <tr style="background-color:White;">
                                        <th scope="row">
                                        </th>
                                        <td>
                                        <select class="form-control" ng-options="item.U_Stype as item.U_StypeName for item in stype" ng-model="g_Stype" ></select>
                                      
                                        </td>
                                        <td>
                                        <select class="form-control" ng-options="item as item.U_ContiName for item in Continent track by item.U_Conti" ng-model="g_Continent" ng-change="getcountry(g_Continent.U_Conti);"></select>
                                        
                                        </td>
                                        <td>
                                         <select class="form-control" ng-options="item as item.U_CountryName for item in country  track by item.U_Country" ng-model="g_Country" ng-change="getcity(g_Country.U_Country);"></select>
                                         
                                        </td>
                                        <td>
                                         <select class="form-control" ng-options="item as item.U_CityName for item in city track by item.U_City" ng-model="g_city" ></select>
                                           
                                        </td>
                                        <td>
                                        <select class="form-control" ng-options="item.U_EQGroup as item.U_EQGroupName for item in egroup" ng-model="g_egroup"></select>

                                           
                                        </td>
                                        <td>
                                         <select class="form-control" ng-options="item.U_Currency as item.U_CurrencyName for item in curency" ng-model="g_curency"></select>
                                            
                                        </td>
                                        <td>
                                        <input style="width:60px;" value"" type="number" ng-model="g_rate"   onfocus="toolshow('ratetool');" onblur="toolhide('ratetool');"  id="price" class="form-control" /><div style="position:relative"><div class="tooltiptext" id="ratetool">{{g_rate}}</div></div>
                                           
                                        </td>
                                         <td>
                                        <input style="width:60px;" value"" type="number" ng-model="g_Rrate"   id="Number1" class="form-control" />
                                           
                                        </td>
                                        <td>
                                        <select class="form-control" ng-options="item.U_UOM as item.U_UOMName for item in UOM" ng-model="g_uom" ></select>
                                        </td>
                                        <td>
                                        <select class="form-control" ng-options="item.U_GST as item.U_GSTName for item in GST" ng-model="g_gst" ></select>
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
                            <table class="table table-bordered table-striped">
                                <thead>
                                    <tr style="background-color:#00a7db;color:#fff">
                                        <th>
                                            #
                                        </th>
                                        <th>Charge Type</th>
                                        <th>
                                            Continent
                                        </th>
                                        <th>
                                            Country
                                        </th>
                                        <th>
                                            City
                                        </th>
                                        <th>Location</th>
                                        <th>
                                            Currency
                                        </th>
                                        <th style="width: 25px">
                                            Rate
                                        </th>
                                        <th>
                                            U.O.M
                                        </th>
                                        <th>
                                            GST %
                                        </th>
                                        <th>
                                            Remarks
                                        </th>
                                        <th colspan="3" ng-init="Afilter=false">
                                            <button  data-toggle="tooltip" data-placement="bottom" title="Filter" ng-click="Afilter= !Afilter" class="btn-sm btn-white btn" type="button" ><i class="fa fa-filter" aria-hidden="true"></i> Filter</button>
                                        </th>
                                    </tr>
                                </thead>
                                <thead>
                                    <tr ng-show="Afilter">
                                        <th>
                                            #
                                        </th>
                                        <th>
                                           <select class="form-control" ng-options="item.U_Stype as item.U_StypeName for item in ChargeType" ng-model="EsearchText.U_Ctype" ></select>
                                        </th>
                                        <th>
                                            <select class="form-control" ng-options="item.U_Conti as item.U_ContiName for item in Continent" ng-model="EsearchText.U_Continent"></select>
                                        </th>
                                        <th>
                                            <select class="form-control" ng-options="item.U_Country as item.U_CountryName for item in CountryMaster" ng-model="EsearchText.U_Country"></select>
                                        </th>
                                        <th>
                                            <select class="form-control" ng-options="item.U_City as item.U_CityName for item in CityMaster" ng-model="EsearchText.U_City" ></select>
                                        </th>
                                        <th>
                                            <select class="form-control" ng-options="item.U_Loc as item.U_LocName for item in LocationMaster" ng-model="EsearchText.U_EQGroup"></select>
                                        </th>
                                        <th>
                                            <select class="form-control" ng-options="item.U_Currency as item.U_CurrencyName for item in curency" ng-model="EsearchText.U_Currency"></select>
                                        </th>
                                        <th style="width: 25px">
                                            <input ng-model="EsearchText.U_Rate" class="form-control">
                                        </th>
                                        <th>
                                            <select class="form-control" ng-options="item.U_UOM as item.U_UOMName for item in UOM" ng-model="EsearchText.U_UOM" ></select>
                                        </th>
                                        <th>
                                            <select class="form-control" ng-options="item.U_GST as item.U_GSTName for item in GST" ng-model="EsearchText.U_GST" ></select>
                                        </th>
                                        <th>
                                            <input ng-model="EsearchText.U_Remarks" type="text" class="form-control" />
                                        </th>
                                        <th colspan="3">
                                          <span class="btn btn-danger btn-sm" ng-click="EsearchText=''"><i class="fa fa-times"></i> 
                                        </th>
                                    </tr>
                                </thead>


                                <tbody>
                                    <tr ng-repeat="d in data.CCONADD | filter:EsearchText">
                                        <th scope="row">
                                            {{$index+1}}
                                        </th>
                                        <td>
                                            <span ng-hide="d.edit">{{getCtypeName(d.U_Ctype)}}</span> <span ng-show="d.edit">
                                             <select class="form-control" ng-options="item.U_Stype as item.U_StypeName for item in ChargeType" ng-model="d.U_Ctype" ></select>
                                                                                            
                                            </span>
                                        </td>
                                        <td>
                                            <span ng-hide="d.edit">{{getContinentName(d.U_Continent)}}</span> <span ng-show="d.edit">
                                            <select class="form-control" ng-options="item as item.U_ContiName for item in Continent track by item.U_Conti" ng-model="d.Ea_Conti" ng-change="getcountry(d.Ea_Conti.U_Conti);"></select>
                                            
                                              
                                        </td>
                                        <td>
                                            <span ng-hide="d.edit">{{getcountryName(d.U_Country)}}</span> <span ng-show="d.edit">
                                           <select class="form-control" ng-options="item as item.U_CountryName for item in country track by item.U_Country" ng-model="d.Ea_Country" ng-change="getcity(d.Ea_Country.U_Country);"></select>
                                           </span>
                                        </td>
                                        <td>
                                            <span ng-hide="d.edit">{{getcityName(d.U_City)}}</span> <span ng-show="d.edit">
                                            <select class="form-control" ng-options="item as item.U_CityName for item in city  track by item.U_City" ng-model="d.Ea_City" ng-change="getlocation(d.Ea_City.U_City);"></select>
                                         
                                        </td>
                                        <td>
                                            <span ng-hide="d.edit">{{getlocationName(d.U_EQGroup)}}</span> <span ng-show="d.edit">
                                            <select class="form-control" ng-options="item as item.U_LocName for item in location  track by item.U_Loc" ng-model="d.Ea_EQGroup" ></select>
                                            
                                            </span>
                                        </td>
                                        <td>
                                            <span ng-hide="d.edit">{{getecurencyName(d.U_Currency)}}</span> <span ng-show="d.edit"> 
                                             <select class="form-control" ng-options="item.U_Currency as item.U_CurrencyName for item in curency" ng-model="d.U_Currency"></select>
                                             </span>
                                        </td>
                                        <td>
                                            <span ng-hide="d.edit">{{d.U_Rate}}</span> <span ng-show="d.edit"><input ng-model="d.U_Rate" type="number" class="form-control" /></span>
                                        </td>
                                        <td>
                                            <span ng-hide="d.edit">{{getuomname(d.U_UOM)}} </span><span ng-show="d.edit">
                                            <select class="form-control" ng-options="item.U_UOM as item.U_UOMName for item in UOM" ng-model="d.U_UOM" ></select>
                                            
                                          </span>
                                        </td>
                                        <td>
                                            <span ng-hide="d.edit">{{getGSTName(d.U_GST)}} </span><span ng-show="d.edit">
                                            <select class="form-control" ng-options="item.U_GST as item.U_GSTName for item in GST" ng-model="d.U_GST" ></select>
                                            </span>
                                        </td>
                                        <td>
                                            <span ng-hide="d.edit">{{d.U_Remarks}}</span> <span ng-show="d.edit"><input ng-model="d.U_Remarks" type="text" class="form-control" /></span>
                                        </td>
                                        <td>
                                            <span ng-hide="d.edit" class="btn btn-success btn-sm" ng-click="a_edit($index);"><i
                                                class="fa fa-pencil"></i></span><span ng-show="d.edit" class="btn btn-primary btn-sm"
                                                    ng-click="a_ok($index);"><i class="fa fa-plus"></i></span>
                                        </td>
                                        <td>
                                            <span class="btn btn-danger btn-sm" ng-click="a_del($index);"><i class="fa fa-trash-o">
                                            </i></span>
                                        </td>
										 <td>
                                            <span class="btn btn-info btn-sm" ng-click="a_copy($index);"><i class="fa fa-copy"></i></span>
                                        </td>
                                    </tr>
                                    <tr style="background-color:White;">
                                        <th scope="row">
                                        </th>
                                        <td>                                        
                                         <select class="form-control" ng-options="item.U_Stype as item.U_StypeName for item in ChargeType" ng-model="a_Ctype" ></select>
                                           
                                        </td>
                                         <td>
                                        <select class="form-control" ng-options="item as item.U_ContiName for item in Continent track by item.U_Conti" ng-model="a_Continent" ng-change="getcountry(a_Continent.U_Conti);"></select>
                                         
                                        </td>
                                        <td>
                                         <select class="form-control" ng-options="item as item.U_CountryName for item in country track by item.U_Country" ng-model="a_Country" ng-change="getcity(a_Country.U_Country);"></select>
                                         
                                        </td>
                                        <td>
                                         <select class="form-control" ng-options="item as item.U_CityName for item in city track by item.U_City" ng-model="a_city" ng-change="getlocation(a_city.U_City);" ></select>
                                           
                                        </td>
                                        <td>
                                         <select class="form-control" ng-options="item as item.U_LocName for item in location  track by item.U_Loc" ng-model="a_egroup" ></select>
                                                                                
                                        </td>
                                        
                                         <td>
                                         <select class="form-control" ng-options="item.U_Currency as item.U_CurrencyName for item in curency" ng-model="a_curency"></select>
                                            
                                        </td>
                                        <td>
                                        <input style="width:60px;" value"" type="number" ng-model="a_rate"  onfocus="toolshow('ratetooladd');" onblur="toolhide('ratetooladd');"  id="Text1" class="form-control" /><div style="position:relative"><div class="tooltiptext" id="ratetooladd">{{a_rate}}</div></div>
                                           
                                        </td>
                                        <td>
                                        <select class="form-control" ng-options="item.U_UOM as item.U_UOMName for item in UOM" ng-model="a_uom" ></select>
                                        </td>
                                        <td>
                                        <select class="form-control" ng-options="item.U_GST as item.U_GSTName for item in GST" ng-model="a_gst" ></select>
                                        </td>
                                        <td>
                                            <input ng-model="a_remark" type="text" class="form-control" />
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

                                 
                                <button type="button" class="btn btn-success" ng-click="UpdateRecord();"  ng-show="updatebtn">
                                Update</button>
                                
                                </span> <span>
                                    <button class="btn btn-primary" ng-disabled="btndisable">
                                        Print</button></span> <span>
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
    <td>Company Code</td>
    <td>Company Name</td>
    <td>Project Code</td>
    <td>Tel Number</td>
    <td>Fax Number</td>
    <td>Mobile Number</td>
    <td>Email</td>
    <td></td>
    
    
  </tr>
  <tr ng-repeat="d in findresult.CCON">
   <td>{{d.U_Qno}}</td>
    <td>{{d.U_Status}}</td>
    <td>{{d.U_Uname}}</td>
    <td>{{d.U_Cdate}}</td>
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
