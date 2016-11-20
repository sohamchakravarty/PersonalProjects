namespace SocialContactScript
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    using Configuration;

    using Utilities;

    class Program
    {
        private static readonly CultureInfo FormatProvider = CultureInfo.CreateSpecificCulture("fr-FR");
        private static readonly Gmail mailHelper = new Gmail();
        private static readonly DateTime CurrentDate = DateTime.Today;
        private static readonly List<string> ColumnNames = new List<string>();

        static void Main(string[] args)
        {
            var dataTable = GetInitialTable();
            dataTable.Merge(FileUtils.GetDataTableFromCsv(GeneralConfig.MailingListHistoryPath, ColumnNames));

            foreach (DataRow row in dataTable.Rows)
            {
                EmailMetaData metaData;
                var rowDate = DateTime.Parse(row.Field<string>("Date"), FormatProvider);
                if (rowDate.Equals(CurrentDate))
                {
                    metaData = new EmailMetaData()
                                   {
                                       HtmlTemplatePath = GeneralConfig.FirstEmailTemplatePath,
                                       HtmlVariableCount = GeneralConfig.HtmlVariableCounts[0]
                                   };
                }
                else if((CurrentDate - rowDate).TotalDays.Equals(GeneralConfig.RepeatMailBufferInDays))
                {
                    metaData = new EmailMetaData()
                                   {
                                       HtmlTemplatePath = GeneralConfig.SecondEmailTemplatePath,
                                       HtmlVariableCount = GeneralConfig.HtmlVariableCounts[1]
                                   };
                }
                else
                {
                    continue;
                }

                //take those many variables as specified in the config
                var htmlVariables = new List<string>();
                htmlVariables.AddRange(row.ItemArray.Take(metaData.HtmlVariableCount).OfType<string>());
                htmlVariables.Add(row.Field<string>(ColumnNames[metaData.HtmlVariableCount]));  //adding emailId

                mailHelper.SendHtmlFormattedEmail(metaData.HtmlTemplatePath, htmlVariables);
            }

            //create data


            var csvData = FileUtils.GetCsvFromDataTable(dataTable, ColumnNames);
            File.WriteAllText(GeneralConfig.MailingListHistoryPath, csvData);
        }

        #region Private Members

        private static DataTable GetInitialTable()
        {
            //get FirstTimeMailers
            var path = GeneralConfig.FirstTimerMailingListPath;
            var lines = File.ReadAllLines(path);

            var table = new DataTable();
            foreach (var col in lines[0].Split(','))
            {
                table.Columns.Add(col, typeof(string));
                ColumnNames.Add(col);
            }

            if (!table.Columns.Contains("Date"))
            {
                ColumnNames.Add("Date");
                table.Columns.Add(ColumnNames[ColumnNames.Count - 1], typeof(string));
            }

            ColumnNames.Add("EmailType");
            table.Columns.Add(ColumnNames[ColumnNames.Count - 1], typeof(string));
            table.Columns["EmailType"].DefaultValue = EmailType.FirstTimeMail.Value();

            for (var i = 1; i < lines.Length; i++)
            {
                var line = lines[i].Split(',').ToList();
                if(line.Count <= 1) continue;

                if (line.Count != ColumnNames.Count)
                {
                    line.Add(CurrentDate.ToString(FormatProvider));
                }

                table.Rows.Add(line.Select(data => data.Trim()).ToArray());
            }

            return table;
        }

        #endregion
    }

    internal class EmailMetaData
    {
        internal string HtmlTemplatePath;

        internal int HtmlVariableCount;
    }
}
