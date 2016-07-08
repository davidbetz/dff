using System;
using Squid;
using Squid.Data.Entity;
//+
public partial class ShowFeedInformation : System.Web.UI.Page
{
    //- @Id -//
    public Int32 Id
    {
        get
        {
            Int32 id = 0;
            if (Request.QueryString["id"] != null)
            {
                if (Int32.TryParse(Request.QueryString["id"], out id))
                {
                    return id;
                }
            }
            //+
            throw new ArgumentException("id is a required parameter");
        }
    }

    //- #OnInit -//
    protected override void OnInit(EventArgs e)
    {
        Load += new EventHandler(Page_Load);
        base.OnInit(e);
    }

    //- $Page_Load -//
    private void Page_Load(Object sender, EventArgs e)
    {
        Snippet snippet = SnippetFacade.GetSnippetData(this.Id);
        //+
        this.Title = snippet.SnippetTitle;
        litTitleSrc.Text = snippet.SnippetTitle;
        //+
        if (String.IsNullOrEmpty(snippet.SnippetDescription))
        {
            litDescription.Text = "There is no more information available for this topic.";
        }
        else
        {
            litDescription.Text = snippet.SnippetDescription;
        }
    }
}