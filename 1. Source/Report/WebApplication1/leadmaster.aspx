<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="salsequote.aspx.vb" Inherits="WebApplication1.salsequote"
    MasterPageFile="~/Site.Master" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="forms">
        <div class="form-grids row widget-shadow" data-example-id="basic-forms">
            <div class="form-title">
                <div class="col-lg-5">
                    <h4>
                        Lead Master :</h4>
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
            <div class="form-body">
                <div class="col-lg-6">
                  <div class="form-group">
                        <label for="inputEmail3" class="col-sm-3 control-label">
                            Code</label>
                        <div class="col-sm-8">
                            <input type="email" class="form-control" id="Email1" placeholder="Code">
                        </div>
                    </div>
                </div>
                <div class="col-lg-6">
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-3 control-label">
                            Name</label>
                        <div class="col-sm-8">
                            <input type="email" class="form-control" id="inputEmail3" placeholder="Name">
                        </div>
                    </div>
                </div>
                <div class="row top-buffer"></div>
                <div class="col-lg-6">
                  <div class="form-group">
                        <label for="inputEmail3" class="col-sm-3 control-label">
                            Email</label>
                        <div class="col-sm-8">
                            <input type="email" class="form-control" id="Email2" placeholder="Email">
                        </div>
                    </div>
                </div>
                <div class="col-lg-6">
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-3 control-label">
                            Phone</label>
                        <div class="col-sm-8">
                            <input type="email" class="form-control" id="Email3" placeholder="Phone">
                        </div>
                    </div>
                </div>
                <div class="row top-buffer"></div>
                <div class="col-lg-6">
                  <div class="form-group">
                        <label for="inputEmail3" class="col-sm-3 control-label">
                            Fax</label>
                        <div class="col-sm-8">
                            <input type="email" class="form-control" id="Email4" placeholder="Fax">
                        </div>
                    </div>
                </div>
                <div class="col-lg-6">
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-3 control-label">
                            Mobile No</label>
                        <div class="col-sm-8">
                            <input type="email" class="form-control" id="Email5" placeholder="Mobile No">
                        </div>
                    </div>
                </div>
                <div class="row top-buffer"></div>
                <div class="row" align="center">
                <span><button class="btn btn-primary">Save</button></span>
                <span><button class="btn btn-info">Cancel</button></span></div>
            </div>
        </div>
    </div>
</asp:Content>
