using System.Collections.Generic;
using System.Linq;

namespace SocialContactScript.Configuration
{
    using System.ComponentModel;
    using System.Configuration;
    using System.IO;

    class GeneralConfig
    {
        public static string FirstTimerMailingListPath => ConfigurationManager.AppSettings["FirstTimerMailingListPath"];

        public static string MailingListHistoryPath => ConfigurationManager.AppSettings["MailingListHistoryPath"];

        public static double RepeatMailBufferInDays => double.Parse(ConfigurationManager.AppSettings["RepeatMailBufferInDays"]);

        public static List<int> HtmlVariableCounts => 
            new List<int>(ConfigurationManager.AppSettings["HtmlVariableCounts"].Split(',').Select(int.Parse));

        public static string FirstEmailTemplatePath
            => Path.Combine(Directory.GetCurrentDirectory(), @"EmailTemplates\FirstEmailTemplate.htm");

        public static string SecondEmailTemplatePath
            => Path.Combine(Directory.GetCurrentDirectory(), @"EmailTemplates\SecondEmailTemplate.htm");

    }
}
