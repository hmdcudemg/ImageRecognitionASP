<%@ Page Async="true" Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ImageRecognition_ASP._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <center><h1>Welcome to Image Recognition</h1></center>
    </div>

    <div class="row">
        <div class="col-lg-5">
            <h3>Image Upload</h3>
            <asp:Image ID="imgPicture" Width="400" Height="200" runat="server" />
            <br />
            <asp:FileUpload ID="fluPicture" onchange="this.form.submit();" runat="server" Width="400px" />
            <br />
            <asp:CustomValidator ID="CustomValidator1" OnServerValidate="ValidateFileSize" ForeColor="Red" runat="server" ErrorMessage="CustomValidator"></asp:CustomValidator>
        </div>
        <div class="col-lg-7">
            <h3>Image Description</h3>
            <pre><code id="details" runat="server"></code></pre>
        </div>
    </div>

</asp:Content>
