namespace SocialContactScript.Configuration
{
    using System;
    using System.Configuration;

    class SmtpConfig
    {
        public static string Host => ConfigurationManager.AppSettings["NetworkHost"];

        public static int Port => int.Parse(ConfigurationManager.AppSettings["NetworkPort"]);

        public static bool EnableSsl => Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);

        public static string UserMailId => ConfigurationManager.AppSettings["UserMailId"];

        public static string Username => ConfigurationManager.AppSettings["Username"];

        public static string Password => ConfigurationManager.AppSettings["Password"];
    }
}
