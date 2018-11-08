//https://www.oracle.com/webfolder/technetwork/tutorials/obe/db/dotnet/CodeFirst/index.html#overview
//http://www.entityframeworktutorial.net/EntityFramework4.3/raw-sql-query-in-entity-framework.aspx

using System.Configuration;
using System.Data;
using System.Windows.Controls;

namespace WPF_Explorer_Tree.DB
{
    public static class DBmain
    {
       
        static public void executeSQL(string sql,string schema)
        {
            string cn = ConfigurationSettings.AppSettings[schema].ToString();
            OraCls.OraDataAccess.ExecuteNonQuery(cn, CommandType.Text, sql);
        }

        static public void selectSQL(string sql,string schema,DataGrid dataGrid)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds = OraCls.OraDataAccess.ExecuteDataset(ConfigurationSettings.AppSettings[schema].ToString(), CommandType.Text, sql, null);
            //     string CmdString = string.Empty;
            dt = ds.Tables[0];
            //    sda.Fill(dt);
            dataGrid.ItemsSource = dt.DefaultView;
        }

        static public void RunSQLs(string selectedText, string schema, DataGrid dataGrid)
        {
            // lay cac cau lenh sql tu chuoi selectedText
            string[] SQLs = selectedText.Split(';');

            string sql = "";
            foreach (string s in SQLs)
            {
                sql = s.Trim();
                if (sql.Length>10) {
                    string first = sql.Substring(0, 6).ToLower();
                    if (first == "select")
                    {
                        selectSQL(sql, schema, dataGrid);
                    }
                    else
                    {
                        executeSQL(sql, schema);
                    }
                }
                
            }
        }

    }
}