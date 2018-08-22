using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text;
using Newtonsoft.Json;

namespace JeonLib
{
    public class JeonSQL
    {
        private string strConn = "";

        private string _serverip;
        private string _dbname;
        private string _uid;
        private string _pwd;

        public string serverip
        {
            get { return serverip; }
            set { _serverip = value; RefresConn(); }
        }
        public string dbname
        {
            get { return _dbname; }
            set { _dbname = value; RefresConn(); }
        }
        public string id
        {
            get { return _uid; }
            set { _uid = value; RefresConn(); }
        }
        public string password
        {
            get { return _pwd; }
            set { _pwd = value; RefresConn(); }
        }


        public void Configuration(string ServerIP, string DBName, string AccountID, string Password)
        {
            serverip = ServerIP; _dbname = DBName; id = AccountID; password = Password;
            RefresConn();
        }

      


        private void RefresConn()
        {
            strConn = string.Format("Server={0};Database={1};Uid={2};Pwd={3};SslMode=none;"
                , _serverip, _dbname, _uid, _pwd);
        }




        public bool Insert<T>(string dbname, Dictionary<string, T> data)
        {
            StringBuilder colList = new StringBuilder();
            StringBuilder valueList = new StringBuilder();

            int i = 0;
            foreach (var item in data)
            {
                colList.Append(item.Key);

                valueList.Append("'");
                valueList.Append(item.Value);
                valueList.Append("'");
                if (data.Count != ++i)
                {
                    colList.Append(", ");
                    valueList.Append(", ");
                }
            }

            string query = string.Format("INSERT INTO {0}({1}) VALUES({2})", dbname, colList, valueList);


            using (MySqlConnection conn = new MySqlConnection(strConn))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = query;

                foreach (var item in data)
                {
                    cmd.Parameters.AddWithValue(item.Key, item.Value);
                }

                var res = (cmd.ExecuteNonQuery() != -1);
                conn.Close();
                return res;
            }
        }

        public string Update<T>(string dbname, Dictionary<string, T> dataset, string condition)
        {

            StringBuilder dataList = new StringBuilder();

            int i = 0;
            foreach (var item in dataset)
            {
                dataList.Append(item.Key);

                dataList.Append("='");
                dataList.Append(item.Value);
                dataList.Append("'");
                if (dataset.Count != ++i)
                {
                    dataList.Append(", ");
                }
            }

            string query = string.Format("UPDATE {0} set {1} where {2}", dbname, dataList, condition);

            using (MySqlConnection conn = new MySqlConnection(strConn))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = query;

                var res = (cmd.ExecuteNonQuery() != -1).ToString();
                conn.Close();
                return res;
            }
        }


        public List<Dictionary<string, string>> Where(string dbname, string condition)
        {
            DataTable dt = new DataTable();
            string query = string.Format("SELECT * FROM {0} WHERE {1}", dbname, condition);
            using (MySqlConnection conn = new MySqlConnection(strConn))
            {
                MySqlDataAdapter adpt = new MySqlDataAdapter(query, conn);
                adpt.Fill(dt);
            }

            List<Dictionary<string, string>> temp = new List<Dictionary<string, string>>();

            foreach (DataRow row in dt.Rows)
            {
                var data = new Dictionary<string, string>();
                foreach (DataColumn col in dt.Columns)
                {
                    data.Add(col.ColumnName, row[col].ToString());
                }
                temp.Add(data);
            }
            return temp;
        }

        public List<Dictionary<string, string>> Select(string dbname)
        {
            DataTable dt = new DataTable();
            string query = string.Format("SELECT * FROM {0}", dbname);
            using (MySqlConnection conn = new MySqlConnection(strConn))
            {
                MySqlDataAdapter adpt = new MySqlDataAdapter(query, conn);
                adpt.Fill(dt);
            }

            List<Dictionary<string, string>> temp = new List<Dictionary<string, string>>();

            foreach (DataRow row in dt.Rows)
            {
                var data = new Dictionary<string, string>();
                foreach (DataColumn col in dt.Columns)
                {
                    data.Add(col.ColumnName, row[col].ToString());
                }
                temp.Add(data);
            }
            return temp;
        }


        public Dictionary<string, string> First(string dbname, string condition)
        {
            DataTable dt = new DataTable();
            string query = string.Format("SELECT * FROM {0} WHERE {1}", dbname, condition);
            using (MySqlConnection conn = new MySqlConnection(strConn))
            {
                MySqlDataAdapter adpt = new MySqlDataAdapter(query, conn);
                adpt.Fill(dt);
            }
            foreach (DataRow row in dt.Rows)
            {
                var data = new Dictionary<string, string>();
                foreach (DataColumn col in dt.Columns)
                {
                    data.Add(col.ColumnName, row[col].ToString());
                }
                return data;
            }


            return default(Dictionary<string, string>);
        }


        public bool Find(string dbname, string condition)
        {
            DataSet ds = new DataSet();
            string query = string.Format("SELECT * FROM {0} WHERE {1}", dbname, condition);
            using (MySqlConnection conn = new MySqlConnection(strConn))
            {
                MySqlDataAdapter adpt = new MySqlDataAdapter(query, conn);
                adpt.Fill(ds, "Tab1");
            }


            List<List<string>> de = new List<List<string>>();

            foreach (DataRow dr in ds.Tables["Tab1"].Rows)
            {
                var d = new List<string>();

                for (int i = 0; i < dr.ItemArray.Length; i++)
                {
                    d.Add(dr[i].ToString());
                }
                de.Add(d);
            }

            return !JsonConvert.SerializeObject(de).Equals("[]");
        }

    }
}
