namespace SocialContactScript.DataStore
{
    using System.Configuration;
    using System.Data.SqlClient;

    class DbSource
    {
        private void func()
        {
            ConnectionStringSettings conn = ConfigurationManager.ConnectionStrings["LinkedInScriptDB"];

            // Create connection object

            SqlConnection connection = new SqlConnection(conn.ConnectionString);

            SqlCommand command = connection.CreateCommand();

            try
            {

                // Open the connection.

                connection.Open();

                // Execute the insert command.

                command.CommandText = "INSERT INTO Your_Tbl(FirstName,LastName) VALUES('Soham', 'Chakravarty')";

                command.ExecuteNonQuery();

            }

            finally
            {

                // Close the connection.

                connection.Close();

            }
        }
    }
}
