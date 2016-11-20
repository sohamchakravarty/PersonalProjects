namespace SocialContactScript.Utilities
{
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;

    class FileUtils
    {
        public static DataTable GetDataTableFromCsv(string csvFilePath, IList<string> columnNames)
        {
            var dataTable = new DataTable();
            var lines = File.ReadAllLines(csvFilePath);
            foreach (var columnName in lines[0].Split(','))
            {
                dataTable.Columns.Add(columnName, typeof(string));
            }

            for (int i = 1; i < lines.Count(); i++)
            {
                dataTable.Rows.Add(lines[i].Split(',').Select(data => data.Trim()).ToArray());
            }

            return dataTable;
        }

        public static string GetCsvFromDataTable(DataTable dataTable, IList<string> columnNames)
        {
            var lines = new List<string>();
            var line = string.Empty;

            lines.Add(string.Join(",", columnNames));

            foreach (DataRow row in dataTable.Rows)
            {
                line = string.Join(",", row.ItemArray);
                lines.Add(line);
            }

            return string.Join("\n", lines.ToArray());
        }
    }
}
