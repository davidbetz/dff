using System;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
//+
namespace Squid.Web.Controls
{
    [ToolboxData("<{0}:InfoBlock runat=\"server\"></{0}:InfoBlock>")]
    public class InfoBlock : CompositeControl
    {
        private Repeater repeater;

        private InfoBlockData informationBlockData;

        private Boolean registerFeedWithBrowser = false;

        //- $InformationBlockData -//
        private InfoBlockData InformationBlockData
        {
            get
            {
                if (informationBlockData == null)
                {
                    informationBlockData = new InfoBlockData();
                }
                //+
                return informationBlockData;
            }
            set { informationBlockData = value; }
        }

        //- @BlockHeight -//
        public Int32 BlockHeight
        {
            get { return this.InformationBlockData.BlockHeight; }
            set { this.InformationBlockData.BlockHeight = value; }
        }

        //- @BlockWidth -//
        public Int32 BlockWidth
        {
            get { return this.InformationBlockData.BlockWidth; }
            set { this.InformationBlockData.BlockWidth = value; }
        }

        //- @ShowBorder -//
        public Boolean ShowBorder
        {
            get { return this.InformationBlockData.ShowBorder; }
            set { this.InformationBlockData.ShowBorder = value; }
        }

        //- @BlockSize -//
        public BlockSize BlockSize
        {
            get { return this.InformationBlockData.BlockSize; }
            set { this.InformationBlockData.BlockSize = value; }
        }

        //- @RegisterFeedWithBrowser -//
        public Boolean RegisterFeedWithBrowser
        {
            get { return registerFeedWithBrowser; }
            set { registerFeedWithBrowser = value; }
        }

        //- @Title -//
        public String Title { get; private set; }

        //- @DataSource -//
        public String DataSource
        {
            get
            {
                if (this.InformationBlockData.DataSource.Length < 1)
                {
                    throw new ApplicationException("DataSource cannot be blank for InformationBlock");
                }
                //+
                return this.InformationBlockData.DataSource;
            }
            set { this.InformationBlockData.DataSource = value; }
        }

        //- @BlockCssClass -//
        String BlockCssClass
        {
            get
            {
                switch (this.BlockSize)
                {
                    case BlockSize.Regular:
                        return "InformationBlock";
                    case BlockSize.Wide:
                        return "InformationWideBlock";
                    case BlockSize.Tall:
                        return "InformationHighBlock";
                    default:
                        return "InformationBlock";
                }
            }
        }

        //- @Ctor -//
        public InfoBlock() { }

        //- #CreateChildControls -//
        protected override void CreateChildControls()
        {
            HtmlGenericControl div = new HtmlGenericControl("div");
            String inlineStyle = "";
            if (this.BlockWidth > 0)
            {
                inlineStyle = "width: " + this.BlockWidth.ToString() + "px !important;";
            }
            if (this.BlockHeight > 0)
            {
                inlineStyle = "height: " + this.BlockHeight.ToString() + "px !important;";
            }
            if (inlineStyle.Length > 0)
            {
                div.Attributes.Add("style", inlineStyle);
            }
            if (ShowBorder)
            {
                div.Attributes.Add("class", this.BlockCssClass);
            }
            else
            {
                div.Attributes.Add("class", this.BlockCssClass + " InformationBlockNoBorder");
            }
            HtmlGenericControl footerDiv = new HtmlGenericControl("div");
            footerDiv.Attributes.Add("class", "BlockFooter");
            //+
            Object bindingSource = null;
            Object feedLink = null;
            Boolean localResource = false;
            String dffLocalPrefixList = ConfigurationFacade.ApplicationSettings("DataFeedFrameworkLocalPrefixList");
            String[] dffLocalPrefixes = dffLocalPrefixList.Split(",".ToCharArray());
            foreach (String prefix in dffLocalPrefixes)
            {
                if (this.DataSource.Contains(prefix))
                {
                    localResource = true;
                }
            }
            if (localResource)
            {
                feedLink = new Literal();
            }
            else
            {
                feedLink = new HyperLink();
            }
            //+
            try
            {
                XmlReader xmlReader = XmlReader.Create(this.DataSource);
                SyndicationFeed feed = SyndicationFeed.Load(xmlReader);
                bindingSource = feed.Items.Select(entry => new SimpleFeedEntry
                                {
                                    DateTime = entry.PublishDate.DateTime,
                                    Link = entry.Links.First().Uri.AbsoluteUri,
                                    Text = entry.Content != null ? entry.Content.ToString() : entry.Summary.Text,
                                    Title = entry.Title.Text
                                });
                if (String.IsNullOrEmpty(this.Title))
                {
                    this.Title = feed.Title.Text;
                }
                if (!localResource)
                {
                    ((HyperLink)feedLink).NavigateUrl = feed.Links.First().Uri.AbsoluteUri;
                }
                //+
                if (this.RegisterFeedWithBrowser)
                {
                    if (this.Page.Header == null)
                    {
                        throw new InvalidOperationException("To register a feed with a browser, the header of the page must have runat=\"server\" set.");
                    }
                    HtmlGenericControl browserLink = new HtmlGenericControl("link");
                    browserLink.Attributes.Add("rel", "alternate");
                    browserLink.Attributes.Add("type", "application/rss+xml");
                    browserLink.Attributes.Add("href", feed.Links.First().Uri.AbsoluteUri);
                    browserLink.Attributes.Add("title", feed.Title.Text);
                    this.Page.Header.Controls.Add(browserLink);
                }
                //+
                HyperLink link = new HyperLink
                {
                    NavigateUrl = this.DataSource
                };
                String url = Page.ClientScript.GetWebResourceUrl(this.GetType(), "Squid.Web.Controls._Resource.Image.RssLive.png");
                Image rssButton = new Image
                {
                    ImageUrl = url,
                    AlternateText = "RSS"
                };
                link.Controls.Add(rssButton);
                footerDiv.Controls.Add(link);
            }
            catch (WebException ex)
            {
                Literal error = new Literal();

                if (ex.Message == "Invalid URI: The URI scheme is not valid")
                {
                    error.Text = "<span class=\"ErrorMessage\">Incompatible feed detected.</a>";
                }
                else
                {
                    error.Text = "<span class=\"ErrorMessage\">" + ex.Message + "</a>";
                }

                div.Controls.Add(error);
            }
            //+
            HtmlGenericControl h3 = new HtmlGenericControl("h3");
            Literal h3Text = new Literal();
            h3Text.Text = this.Title;
            if (feedLink != null && feedLink is HyperLink)
            {
                ((HyperLink)feedLink).Controls.Add(h3Text);
                h3.Controls.Add((HyperLink)feedLink);
            }
            else
            {
                h3.Controls.Add(h3Text);
            }
            div.Controls.Add(h3);
            //+
            try
            {
                repeater = new Repeater();
                repeater.DataSource = bindingSource;
                repeater.DataMember = "text";
                repeater.ItemTemplate = new RssListTemplate(ListItemType.Item);
                repeater.HeaderTemplate = new RssListTemplate(ListItemType.Header);
                repeater.FooterTemplate = new RssListTemplate(ListItemType.Footer);
                //+
                div.Controls.Add(repeater);
                repeater.DataBind();
            }
            catch (Exception ex)
            {
                Literal error = new Literal();
                error.Text = "<span class=\"ErrorMessage\">" + ex.Message + "</a>";
                //+
                div.Controls.Add(error);
            }
            div.Controls.Add(footerDiv);
            //+
            this.Controls.Add(div);
            base.CreateChildControls();
        }
    }
}