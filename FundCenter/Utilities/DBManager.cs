using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FundCenter.Utilities
{
    public static class DBManager<T>
    {
        private static string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public static T GetData(string sql)
        {
            T t = default(T);
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = connectionString;
                    conn.Open(); // 打开数据库连接

                    SqlCommand com = new SqlCommand();
                    com.Connection = conn;
                    com.CommandType = CommandType.Text;
                    com.CommandText = sql;
                    SqlDataReader dr = com.ExecuteReader();//执行SQL语句
                    t = DataReaderToObj<T>(dr)[0];
                    dr.Close();//关闭执行
                    conn.Close();//关闭数据库
                }
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, "DB get error", ex.Message);
            }

            return t;
        }

        public static T AddData(string sql)
        {
            T t = default(T);

            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = connectionString;
                    conn.Open(); // 打开数据库连接

                    SqlCommand com = new SqlCommand();
                    com.Connection = conn;
                    com.CommandType = CommandType.Text;
                    com.CommandText = sql;
                    SqlDataReader dr = com.ExecuteReader();//执行SQL语句
                    dr.Close();//关闭执行
                    conn.Close();//关闭数据库
                }
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, "DB add error", ex.Message);
            }

            return t;
        }

        public static List<T> GetDatas(string sql)
        {
            List<T> list = new List<T>();
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = connectionString;
                    conn.Open(); // 打开数据库连接

                    SqlCommand com = new SqlCommand();
                    com.Connection = conn;
                    com.CommandType = CommandType.Text;
                    com.CommandText = sql;
                    SqlDataReader dr = com.ExecuteReader();//执行SQL语句
                    list = DataReaderToObjs<T>(dr);
                    dr.Close();//关闭执行
                    conn.Close();//关闭数据库
                }
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, "GetDatas error", ex.Message);
            }

            return list;
        }

        public static List<T> DataReaderToObjs<T>(SqlDataReader rdr)
        {
            List<T> list = new List<T>();

            while (rdr.Read())
            {
                T t = System.Activator.CreateInstance<T>();
                Type obj = t.GetType();
                // 循环字段
                for (int i = 0; i < rdr.FieldCount; i++)
                {
                    object tempValue = null;

                    if (rdr.IsDBNull(i))
                    {

                        tempValue = null;

                    }
                    else
                    {
                        tempValue = rdr.GetValue(i);

                    }

                    obj.GetProperty(rdr.GetName(i)).SetValue(t, tempValue, null);

                }

                list.Add(t);

            }
            return list;
        }

        public static List<T> DataReaderToObj<T>(SqlDataReader rdr)
        {
            List<T> list = new List<T>();

            if (rdr.Read())
            {
                T t = System.Activator.CreateInstance<T>();
                Type obj = t.GetType();
                // 循环字段
                for (int i = 0; i < rdr.FieldCount; i++)
                {
                    object tempValue = null;

                    if (rdr.IsDBNull(i))
                    {

                        tempValue = null;

                    }
                    else
                    {
                        tempValue = rdr.GetValue(i);

                    }

                    obj.GetProperty(rdr.GetName(i)).SetValue(t, tempValue, null);

                }

                list.Add(t);

            }
            return list;
        }

        //public static T DataReaderToObj<T>(SqlDataReader rdr)
        //{
        //    T t = System.Activator.CreateInstance<T>();
        //    Type obj = t.GetType();

        //    if (rdr.Read())
        //    {
        //        // 循环字段
        //        for (int i = 0; i < rdr.FieldCount; i++)
        //        {
        //            object tempValue = null;

        //            if (rdr.IsDBNull(i))
        //            {
        //                tempValue = null;
        //            }
        //            else
        //            {
        //                tempValue = rdr.GetValue(i);

        //            }

        //            obj.GetProperty(rdr.GetName(i)).SetValue(t, tempValue, null);

        //        }
                
        //    }
        //    return obj;

        //}

    }
}