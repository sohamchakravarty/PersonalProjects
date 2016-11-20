namespace SocialContactScript.Configuration
{
    using System.ComponentModel;

    public enum EmailType
    {
        [Description("FirstTimeMail")]
        FirstTimeMail,
        [Description("RepeatMail")]
        RepeatMail
    }

    public static class EmailTypeExtensions
    {
        public static string Value(this EmailType val)
        {
            var attributes = (DescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
    }
}
