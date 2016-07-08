using System;
//+
namespace Squid
{
    public static class ConfigurationFacade
    {
        //- @ApplicationSettings -//
        public static String ApplicationSettings(String key)
        {
            String value = System.Configuration.ConfigurationManager.AppSettings[key];
            if (String.IsNullOrEmpty(value))
            {
                throw new ApplicationException(String.Format("{0} is required in the application configuration file", key));
            }
            //+
            return value;
        }

        //- @ConnectionString -//
        public static String ConnectionString(String key)
        {
            String value = System.Configuration.ConfigurationManager.ConnectionStrings[key].ConnectionString;
            if (String.IsNullOrEmpty(value))
            {
                throw new ApplicationException(String.Format("{0} is required in the application configuration file", key));
            }
            //+
            return value;
        }
    }
}