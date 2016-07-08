<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/MasterPage.master" CodeFile="SnippetViewer.aspx.cs" Inherits="ShowFeedInformation" Title="Snippet Viewer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPrimary" runat="Server">
    <div id="feedDetails" class="Display">
        <h3><asp:Literal id="litTitleSrc" runat="server"></asp:Literal></h3>
        <div id="description">
            <asp:Literal ID="litDescription" runat="server"></asp:Literal>
        </div>
    </div>
</asp:Content>