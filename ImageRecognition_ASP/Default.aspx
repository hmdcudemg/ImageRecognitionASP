<%@ Page Async="true" Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ImageRecognition_ASP._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row" style="margin-top: 20px;">
        <div class="col-lg-6">
            <div class="card border-primary mb-6">
                <div class="card-header">Image Upload</div>
                <div class="card-body">
                    <asp:Image ID="imgPicture" Width="100%" Height="300" runat="server" />
                    <br />
                    <asp:Label ID="Label2" CssClass="text-primary" runat="server" Text=""></asp:Label>
                    <br />
                    <asp:Label ID="Label1" CssClass="text-info" runat="server" Text=""></asp:Label>
                    <br />
                    <br />
                    <asp:FileUpload ID="fluPicture" Style="display: none;" onchange="this.form.submit();" runat="server" Width="400px" />
                    <div class="row">
                        <div class="col-lg-9">
                            <div class="input-group mb-3">
                                <asp:TextBox ID="txbURL" type="text" class="form-control" placeholder="Image URL" aria-label="Image URL" aria-describedby="button-addon2" onchange="this.form.submit();" runat="server" />
                                <div class="input-group-append">
                                    <button class="btn btn-outline-primary" type="button" id="button-addon2">Submit</button>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-3">
                            <button id="btnUpload" class="btn btn-primary" type="button" runat="server" style="width: 100%;">
                                <img src="Content/img/upload-icon.png" height="20" />
                                Browse
                            </button>
                        </div>
                    </div>
                    <br />
                    <asp:CustomValidator ID="CustomValidator1" OnServerValidate="ValidateFileSize" ForeColor="Red" runat="server" ErrorMessage="CustomValidator"></asp:CustomValidator>
                </div>
            </div>
        </div>
        <div class="col-lg-6">
            <div class="card border-primary mb-6" style="max-height: 565px; height: 565px;">
                <div class="card-header">Image Description</div>
                <div class="card-body">
                    <pre style="max-height: 450px;"><code class="text-info" id="details" runat="server"></code></pre>
                </div>
            </div>

        </div>
    </div>

</asp:Content>
