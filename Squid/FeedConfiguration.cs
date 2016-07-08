using System;
//+
namespace Squid
{
    public static class Configuration
    {
        //- @DatabaseConnectionString -//
        public static String DatabaseConnectionString
        {
            get
            {
                return ConfigurationFacade.ConnectionString("SquidConnectionString");
            }
        }

        //- @DefaultFeedLinkTemplate -//
        public static String DefaultFeedLinkTemplate
        {
            get
            {
                return ConfigurationFacade.ApplicationSettings("DefaultFeedLinkTemplate");
            }
        }

        //- @DefaultEntryLinkTemplate -//
        public static String DefaultEntryLinkTemplate
        {
            get
            {
                return ConfigurationFacade.ApplicationSettings("DefaultEntryLinkTemplate");
            }
        }

        //- @SiteRoot -//
        public static String SiteRoot
        {
            get
            {
                return ConfigurationFacade.ApplicationSettings("SiteRoot");
            }
        }
    }
}