using System;
using System.Web.UI;
using System.Web.UI.WebControls;
//+
namespace Squid.Web.Controls
{
    internal class RssListTemplate : ITemplate
    {
        ListItemType type = new ListItemType();

        //- @Ctor -//
        public RssListTemplate(ListItemType type)
        {
            this.type = type;
        }

        //- @InstantiateIn -//
        public void InstantiateIn(Control container)
        {
            Literal lit = new Literal();
            switch (type)
            {
                case ListItemType.Header:
                    lit.Text = "<ul>";
                    break;
                case ListItemType.Item:
                    lit.DataBinding += new EventHandler(delegate(Object sender, System.EventArgs ea)
                    {
                        Literal literal = (Literal)sender;
                        RepeaterItem item = (RepeaterItem)literal.NamingContainer;
                        Literal more = new Literal();
                        literal.Text += "<li>";
                        literal.Text += "<a href=\"";
                        literal.Text += DataBinder.Eval(item.DataItem, "Link");
                        literal.Text += "\">";
                        literal.Text += DataBinder.Eval(item.DataItem, "Title");
                        literal.Text += "</a>";
                        literal.Text += "</li>";
                    });
                    break;
                case ListItemType.AlternatingItem:
                    break;
                case ListItemType.Footer:
                    lit.Text = "</ul>";
                    break;
            }
            //+
            container.Controls.Add(lit);
        }
    }
}