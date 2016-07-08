<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeFile="Default.aspx.cs" Inherits="Default" Title="InfoBlocks Sample" %>

<asp:Content ID="cInformationBlocks" ContentPlaceHolderID="cphPrimary" runat="Server">
    <div id="information" class="Display">
        <h2>Information Center</h2>
        <squid:InfoBlock ID="ibBlock1" runat="server" BlockSize="Tall" RegisterFeedWithBrowser="True" DataSource="http://localhost:2001/Service/Feed.svc/GetFeedByTitle/Company+Announcements" Visible="true"></squid:InfoBlock>
        <squid:InfoBlock ID="ibBlock2" runat="server" BlockSize="Wide" RegisterFeedWithBrowser="True" DataSource="http://localhost:2001/Service/Feed.svc/GetFeedBySecretId/0E0028FD-62E7-4CFA-A2ED-7C415A5C2555" Visible="true"></squid:InfoBlock>
        <squid:InfoBlock ID="ibBlock3" runat="server" BlockSize="Regular" RegisterFeedWithBrowser="True" DataSource="http://localhost:2001/Service/Feed.svc/GetFeedBySecretId/94905906-50ED-4F17-AEAA-0B24857E717B" Visible="true"></squid:InfoBlock>
        <squid:InfoBlock ID="ibBlock4" runat="server" BlockSize="Regular" RegisterFeedWithBrowser="False" DataSource="http://feeds.feedburner.com/FXHarmonics/" Visible="true"> </squid:InfoBlock>
        <squid:InfoBlock ID="ibBlock5" runat="server" BlockSize="Wide" RegisterFeedWithBrowser="True" DataSource="http://localhost:2001/Service/Feed.svc/GetFeedById/4" Visible="true"></squid:InfoBlock>
        <squid:InfoBlock ID="ibBlock6" runat="server" BlockSize="Regular" RegisterFeedWithBrowser="False" DataSource="http://rss.news.yahoo.com/rss/topstories" Visible="true"> </squid:InfoBlock>
    </div>
</asp:Content>