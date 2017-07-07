<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="salsequote.aspx.vb" Inherits="WebApplication1.salsequote"
    MasterPageFile="~/Site.Master" %>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <script src="asset/js/services/css1_service.js" ></script>
    <script src="asset/js/ctrl/ccSurvey1_ctrl.js"></script>

    <div class="forms" ng-controller="ccSurvey1_ctrl">
        <div class="form-grids row widget-shadow" data-example-id="basic-forms">
            <div class="form-title">
                <div class="col-lg-5">
                    <h4>
                       Cleaning survey :</h4>
                </div>
                 <div class="btn-group pull-right" role="group"  aria-label="Basic example">
                <button type="button" class="btn btn-white"  ng-click="backToSO();">
                        <i class="fa fa-fast-backward"></i> Back to Sales Order
                    </button>
                    <button type="button" class="btn btn-white"  ng-click="SurveyList();">
                        <i class="fa fa-th-list"></i> Back to Survey List
                    </button>

                     <a  ng-hide="data.ODLN[0].U_SurvyNo===undefined" target="_blank" href="CleaningCertificateReport.aspx?key={{data.ODLN[0].U_SurvyNo}}"<button class="btn btn-white"  ng-disabled="false">
                                              <i class="fa fa-print" aria-hidden="true"></i> Print</button></a>


                </div>
            </div>
            
          
         
            <form id="ss" runat="server">
            <div class="form-body">
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
                           Survey No
                        </label>
                        <div class="col-sm-8">
                            <span >{{data.ODLN[0].U_SurvyNo}}</span>
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
                            Company Name
                        </label>
                        <div class="col-sm-8">
                        <div class="input-group">
                         <input type="text" ng-model="data.ODLN[0].CardName" ng-disabled="true"  class="form-control" id="Email7"
                                placeholder="Company Name">
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
                            <input type="text" ng-disabled="updatebtn" ng-keydown="SIselected=false" ng-model="data.ODLN[0].U_SurveyorID" class="form-control" id="Text1"
                                placeholder="Surveyor ID">
                                <span class="input-group-btn">
        <button class="btn btn-info" type="button" ng-disabled="updatebtn" ng-click="getsurveyorID();"><i class="fa fa-search" aria-hidden="true"></i></button>
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
                </div>
                <div class="col-lg-6">
                   
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
                            IMO
                        </label>
                        <div class="col-sm-8">
                             <select ng-disabled="updatedisabled" class="form-control" ng-options="item.Name as item.Name for item in IMO" ng-model="data.ODLN[0].U_IMO"></select>
                        </div>
                    </div>


                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Equipment Number
                        </label>
                        <div class="col-sm-8">
                           <div style="position:relative"><div class="tooltiptext" id="equtool">Sample: MSCU 123456 7</div></div> <input type="text" ng-keypress="checkEQNO($event.charCode);" onfocus="toolshow('equtool');" onblur="toolhide('equtool');"    capitalize ng-model="data.ODLN[0].U_EqNo" ng-blur="getDamageHistory(data.ODLN[0].U_EqNo);" class="form-control" id="Text5"
                                placeholder="Equipment No">
                        </div>
                    </div>

                    

                    
                    

                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Survey Result
                        </label>
                        <div class="col-sm-8">
                       
                         <select class="form-control"  ng-model="data.ODLN[0].U_SResult" ng-change="Survey_Result_changed();" >
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

                    
                    
                </div>
                <div class="row top-buffer">
                </div>
                <!-- table start -->
                
                <div class="row">
                    <ul id="myTabs" class="nav nav-tabs" role="tablist">
                        <li role="presentation" class="active"><a href="#General" id="General-tab" role="tab"
                            data-toggle="tab" aria-controls="home" aria-expanded="false">Details</a></li>
                        <li role="presentation" class=""><a href="#Addon" role="tab" id="Addon-tab" data-toggle="tab"
                            aria-controls="Addon" aria-expanded="true">Damage History</a></li>
                        
                    </ul>
                    <div id="myTabContent" class="tab-content scrollbar1">
                        <div role="tabpanel" class="tab-pane fade active in" id="General" aria-labelledby="General-tab-tab">
                            <span class="top-buffer"></span>
                            <div class="col-lg-6">
                            
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            
CSC Number
                        </label>
                        <div class="col-sm-8">
                       
                        <input type="text" capitalize class="form-control" id="Number1" ng-model="data.ODLN[0].U_CSCNum" >
                            
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                           Last Test Date
                        </label>
                        <div class="col-sm-8">
                       
                        <input type="text" capitalize class="form-control datepicker" id="Text7" ng-model="data.ODLN[0].U_CSC" onclick="return Text7_onclick()">
                            
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            D.O.M
                        </label>
                        <div class="col-sm-8">
                       
                         <input type="text" capitalize class="form-control datepicker" id="Text10" ng-model="data.ODLN[0].U_DOM" >
                            
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            MGW (KG)
                        </label>
                        <div class="col-sm-8">
                       
                        <input type="text" capitalize class="form-control" id="Text13" ng-model="data.ODLN[0].U_MGW" >
                            
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            PayLoad (KG)
                        </label>
                        <div class="col-sm-8">
                       
                         <input type="text" capitalize class="form-control" id="Text16" ng-model="data.ODLN[0].U_PayLoad" >
                            
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Tare (KG)
                        </label>
                        <div class="col-sm-8">
                       
                         <input type="text" class="form-control" id="Text3" ng-disabled='false' capitalize ng-model="data.ODLN[0].U_Tare" ng-init="{{data.ODLN[0].U_Tare=data.ODLN[0].U_MGW-data.ODLN[0].U_PayLoad}}" >
                            
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Capacity (L)
                        </label>
                        <div class="col-sm-8">
                       
                         <input type="text" class="form-control" capitalize id="Text4" ng-model="data.ODLN[0].U_Capacity" >
                            
                        </div>
                    </div>

                    
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            <strong>EXTERIOR</strong>
                        </label>
                        <div class="col-sm-8">
                       
                         
                            
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-10 control-label">
                            Frame, tank and walkways free of contamination and cargo
                        </label>
                        <div class="col-sm-2">
                       
                         <input type="checkbox" name="vehicle" value="Bike" style="width:30px;height:30px;"  ng-model="data.ODLN[0].U_EX_Fram" ng-true-value="'YES'" ng-false-value="'NO'"> 
                            
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-10 control-label">
                            Manlid and valve compartments free of contamination and cargo
                        </label>
                        <div class="col-sm-2">
                       
                         <input type="checkbox" name="vehicle" value="Bike" style="width:30px;height:30px;" ng-model="data.ODLN[0].U_EX_Man"  ng-true-value="'YES'" ng-false-value="'NO'"> 
                            
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-10 control-label">
                            Serial numbers and satutory markings legible
                        </label>
                        <div class="col-sm-2">
                       
                         <input type="checkbox" name="vehicle" value="Bike" style="width:30px;height:30px;" ng-model="data.ODLN[0].U_EX_Ser"  ng-true-value="'YES'" ng-false-value="'NO'"> 
                            
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-10 control-label">
                            Cargo labels removed
                        </label>
                        <div class="col-sm-2">
                       
                         <input type="checkbox" name="vehicle" value="Bike" style="width:30px;height:30px;" ng-model="data.ODLN[0].U_EX_Car"  ng-true-value="'YES'" ng-false-value="'NO'"> 
                            
                        </div>
                    </div>

                     <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            <strong>INTERIOR</strong>
                        </label>
                        <div class="col-sm-8">
                       
                         
                            
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-10 control-label">
                            Free from Odour
                        </label>
                        <div class="col-sm-2">
                       
                         <input type="checkbox" name="vehicle" value="Bike" style="width:30px;height:30px;"  ng-model="data.ODLN[0].U_INT_Free"  ng-true-value="'YES'" ng-false-value="'NO'"> 
                            
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-10 control-label">
                            Clean and free from all Cargo and Contamination
                        </label>
                        <div class="col-sm-2">
                       
                         <input type="checkbox" name="vehicle" value="Bike" style="width:30px;height:30px;" ng-model="data.ODLN[0].U_INT_Clean"  ng-true-value="'YES'" ng-false-value="'NO'"> 
                            
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-10 control-label">
                            Dry
                        </label>
                        <div class="col-sm-2">
                       
                         <input type="checkbox" name="vehicle" value="Bike" style="width:30px;height:30px;" ng-model="data.ODLN[0].U_INT_Dry"  ng-true-value="'YES'" ng-false-value="'NO'"> 
                            
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-10 control-label">
                            Pitting
                        </label>
                        <div class="col-sm-2">
                       
                         <input type="checkbox" name="vehicle" value="Bike" style="width:30px;height:30px;" ng-model="data.ODLN[0].U_INT_Pitt"  ng-true-value="'YES'" ng-false-value="'NO'"> 
                            
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-10 control-label">
                            Discolouration
                        </label>
                        <div class="col-sm-2">
                       
                         <input type="checkbox" name="vehicle" value="Bike" style="width:30px;height:30px;" ng-model="data.ODLN[0].U_INT_Disc"  ng-true-value="'YES'" ng-false-value="'NO'"> 
                            
                        </div>
                    </div>


</div>
<div class="col-lg-6">

<div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            <strong>VALVES/FITTINGS </strong>
                        </label>
                        <div class="col-sm-8">
                        
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-6 control-label">
                            Valves
                        </label>
                        <div class="col-sm-6">
                       
                         
                           <select class="form-control" ng-disabled="flase"  ng-model="data.ODLN[0].U_VAL_Val" >
                         <option value='YES'>YES</option>
                         <option value='NO'>NO</option>
                         </select>
                       
                            
                        </div>
                    </div>
                    
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-6 control-label">
                            Bottom Outlet Type
                        </label>
                        <div class="col-sm-6">
                       
                         <select class="form-control" ng-disabled="flase"  ng-model="data.ODLN[0].U_VAL_Bott" >
                         <option value='3" BSP'>3" BSP</option>
                         <option value='3" BLANK FLANGE'>3" BLANK FLANGE</option>
                         <option value='CAMLOCK'>CAMLOCK</option>
                         </select>
                            
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-6 control-label">
                            Manlid Gasket
                        </label>
                        <div class="col-sm-6">
                       
                         <select class="form-control" ng-disabled="flase"  ng-model="data.ODLN[0].U_VAL_Man" >
                         <option value='VITON'>VITON</option>
                         <option value='PTFE'>PTFE</option>
                         <option value='SWR'>SWR</option>
                         <option value='SUPERTANKY'>SUPERTANKY</option>
                         <option value='EPDM'>EPDM</option>
                         <option value='NOT FITTED'>NOT FITTED</option>
                         
                         </select>
                            
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-6 control-label">
                            Syphon Tube
                        </label>
                        <div class="col-sm-6">
                       
                         <select class="form-control" ng-disabled="flase"  ng-model="data.ODLN[0].U_VAL_Syp" >
                         <option value='FITTED'>FITTED</option>
                         <option value='NOT FITTED'>NOT FITTED</option>
                         </select>
                            
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-6 control-label">
                            Spillage Drain Tubes Clear
                        </label>
                        <div class="col-sm-6">
                       
                         <select class="form-control" ng-disabled="flase"  ng-model="data.ODLN[0].U_VAL_SPIL" >
                         <option value='BLOCKED'>BLOCKED</option>
                         <option value='UNBLOCKED'>UNBLOCKED</option>
                         </select>
                            
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-6 control-label">
                            Tank has been Steam Clean
                        </label>
                        <div class="col-sm-6">
                       
                         <select class="form-control" ng-disabled="flase"  ng-model="data.ODLN[0].U_VAL_Tank" >
                         <option value='WITNESS'>WITNESS</option>
                         <option value='NOT WITNESS'>NOT WITNESS</option>
                         </select>
                            
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-6 control-label">
                            Available Steam Heating Pipe Line
                        </label>
                        <div class="col-sm-6">
                       
                        
                            
                           <select class="form-control" ng-disabled="flase"  ng-model="data.ODLN[0].U_VAL_Avail" >
                         <option value='YES'>YES</option>
                         <option value='NO'>NO</option>
                         </select>
                       
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-6 control-label">
                            Steam Tube
                        </label>
                        <div class="col-sm-6">
                       
                         <select class="form-control" ng-disabled="flase"  ng-model="data.ODLN[0].U_VAL_Steam" >
                         <option value='WITNESS'>WITNESS</option>
                         <option value='NOT WITNESS'>NOT WITNESS</option>
                         </select>
                            
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-6 control-label">
                            Gas Free Entry Permit Issued
                        </label>
                        <div class="col-sm-6">
                       
                         <select class="form-control" ng-disabled="flase"  ng-model="data.ODLN[0].U_VAL_Gas" >
                         <option value='ISSUED'>ISSUED</option>
                         <option value='NOT ISSUED'>NOT ISSUED</option>
                         </select>
                            
                        </div>
                    </div>
<div class="form-group">
                    <label for="inputEmail3" class="col-sm-4 control-label">
                            <strong>ICSB SEAL NUMBER</strong>
                        </label>
                        <div class="col-sm-8">
                        
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-6 control-label">
                            Manlid / Compartment 
                        </label>
                        <div class="col-sm-6">
                       
                         <input type="text" capitalize class="form-control" ng-model="data.ODLN[0].U_SEAL_MAN" />
                            
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-6 control-label">
                           Airline / Top Outlet Compartment 
                        </label>
                        <div class="col-sm-6">
                       
                         <input type="text" capitalize class="form-control" ng-model="data.ODLN[0].U_SEAL_AIR"  />
                            
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-6 control-label">
                            Bottom Outlet / Compartment 
                        </label>
                        <div class="col-sm-6">
                       
                         <input type="text" capitalize class="form-control"  ng-model="data.ODLN[0].U_SEAL_BOTT"  />
                            
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-6 control-label"  >
                            
                        </label>
                        <div class="col-sm-6">
                       
                        
                            
                        </div>
                    </div>
                    
                    <!-- last cargo -->
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-6 control-label">
                            Last cargo
                        </label>
                        <div class="col-sm-6">
                       <input type="text" capitalize class="form-control"   maxlength="30"  ng-model="data.ODLN[0].U_SEAL_LAST"  />
                      
                            
                        </div>
                    </div>
                     <!-- NEXT CARGO -->
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-6 control-label">
                            Next cargo
                        </label>
                        <div class="col-sm-6">
 <input type="text" class="form-control" capitalize ng-model="data.ODLN[0].U_SEAL_NEXT"   maxlength="30" value="unknown"  />
                        </div>
                    </div>
                    
                    
                    
                    </div>
                        </div>
                        <div role="tabpanel" class="tab-pane fade in" id="Addon" aria-labelledby="Addon-tab">
                            <span class="top-buffer"></span>
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
                            </table>
                        </div>
                        
                    </div>
                    <div class="row top-buffer">
                    </div>
                    
                </div>
                <div class="form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">
                            Remarks
                        </label>
                        <div class="col-sm-8">
                        <textarea capitalize class="form-control" style="height:100px"  maxlength="254"  ng-model="data.ODLN[0].Comments"></textarea>
                           <br />
                        </div>
                    </div>
                            <div class="row" align="right">

                            <span> <button type="button" class="btn btn-white"  ng-click="backToSO();">
                        <i class="fa fa-fast-backward"></i> Back to Sales Order
                    </button></span>
 <span><button type="button" class="btn btn-white"  ng-click="SurveyList();">
                        <i class="fa fa-th-list"></i> Back to Survey List
                    </button>
                    
                    <a  ng-hide="data.ODLN[0].U_SurvyNo===undefined" target="_blank" href="CleaningCertificateReport.aspx?key={{data.ODLN[0].U_SurvyNo}}"<button class="btn btn-white"  ng-disabled="false">
                                              <i class="fa fa-print" aria-hidden="true"></i> Print</button></a>
                    </span>


                        <span>
                          
                              <button type="button" class="btn btn-success" ng-click="save();" ng-model="savebtn" ng-show="Showsavebtn" >
                                        {{savelable}}</button> 
                                        
                                        <button type="button" class="btn btn-success" ng-disabled="data.ODLN[0].U_Status=='Closed'" ng-click="update();" ng-model="updatebtn" ng-show="updatebtn" >
                                        Update</button></span> 
                                        
                                        <span>
                                           <%-- <a  ng-hide="data.ODLN[0].U_SurvyNo===undefined" target="_blank" href="CleaningCertificateReport.aspx?key={{data.ODLN[0].U_SurvyNo}}"<button class="btn btn-primary"  ng-disabled="false">
                                               Print</button></a>--%></span> <span>
                                                            <button type="button" class="btn btn-danger" ng-click="resetall();">
                                                                Cancel</button></span></div>
                
            </div>
            </form>
        </div>
    </div>
    <script type="text/javascript">        $('.datepicker').datepicker({autoclose: true,viewMode: "months", 
    minViewMode: "months",
    format: 'M-yyyy'
    });</script>
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

        function Text7_onclick() {

        }

    </script>
     <script type="text/javascript">         $('.datepickersub').datepicker({ autoclose: true, format: 'dd/mm/yyyy',todayHighlight: true });</script>

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



</asp:Content>
