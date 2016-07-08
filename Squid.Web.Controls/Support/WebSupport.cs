using System;
using System.Web.UI;
//+
namespace Squid.Web.Controls.Support
{
    public static class WebSupport
    {
        //- @RecursiveFindControl -//
        public static Control RecursiveFindControl(Control container, String name)
        {
            Control control = container.FindControl(name);
            if (control != null)
            {
                return control;
            }
            if (container.NamingContainer != null)
            {
                return RecursiveFindControl(container.NamingContainer, name);
            }
            //+
            return null;
        }
    }
}