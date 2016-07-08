using System;
//+
namespace Squid.Web.Controls
{
    internal class RssBindingItem
    {
        //- @Name -//
        public String Name { get; set; }

        //- @Link -//
        public String Link { get; set; }

        //- @Ctor -//
        public RssBindingItem() { }

        //- @Ctor -//
        public RssBindingItem(String name, String link)
        {
            this.Name = name;
            this.Link = link;
        }
    }
}