using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


namespace IntelligentCooker
{
    class SqlHelper
    {
        static string sqldatabase = @"Data Source=LocalHost;Initial Catalog=MyCook;Persist Security Info=True;User ID=sa;Password=sa";
        /// <summary>
        /// 处理数据库更新，插入，删除操作,返回受影响行数
        /// </summary>
        /// <param name="database"></param>
        /// <param name="sqltxt"></param>
        /// <param name="myparas"></param>
        /// <returns></returns>
        public static int MyHandleUpdateSql(string sqltxt, params SqlParameter[] myparas)
        {
            SqlConnection mycon = new SqlConnection(sqldatabase);
            SqlCommand cmd = new SqlCommand(sqltxt, mycon);
            if (myparas.Length > 0 && myparas != null)
            {
                cmd.Parameters.AddRange(myparas);
            }
            mycon.Open();
            int res = -1;
            res = cmd.ExecuteNonQuery();
            mycon.Close();
            return res;
        }

        /// <summary>
        /// 处理数据库查询操作，有返回值
        /// </summary>
        /// <param name="database"></param>
        /// <param name="sqltxt"></param>
        /// <param name="myparas"></param>
        /// <returns></returns>
        public static DataTable MyHandleSelectSql(string sqltxt, params SqlParameter[] myparas)
        {
            SqlConnection mycon = new SqlConnection(sqldatabase);
            SqlCommand cmd = new SqlCommand(sqltxt, mycon);
            if (myparas.Length > 0 && myparas != null)
            {
                cmd.Parameters.AddRange(myparas);
            }
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            // mycon.Close();
            return dt;
        }
    }
}
