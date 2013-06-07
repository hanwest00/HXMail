using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SQLite;
using System.Reflection;

namespace HXMail.DAL
{
    public class SqliteHelper
    {

        private static readonly string connectString = "Data Source=" + HXMail.Common.PathInfo.GetInstallPath + "DB\\H_X_M.sqlite;Pooling=false";

        private static SQLiteCommand comm = new SQLiteCommand();

        public static object ExecuteScaler(CommandType commType, string commText)
        {
            return ExecuteScaler(commType, commText, null, null);
        }

        public static object ExecuteScaler(CommandType commType, string commText, SQLiteParameter param)
        {
            return ExecuteScaler(commType, commText, new SQLiteParameter[] { param }, null);
        }

        public static object ExecuteScaler(CommandType commType, string commText, SQLiteParameter[] param)
        {
            return ExecuteScaler(commType, commText, param, null);
        }

        public static object ExecuteScaler(CommandType commType, string commText, SQLiteParameter[] param, SQLiteTransaction tran)
        {
            object ret = null;
            using (SQLiteConnection conn = new SQLiteConnection(connectString))
            {
                PrepareCommand(conn, comm, commType, commText, param, tran);
                ret = comm.ExecuteScalar();
            }
            return ret;
        }

        public static int ExecuteNoQuery(CommandType commType, string commText)
        {
            return ExecuteNoQuery(commType, commText, null, null);
        }

        public static int ExecuteNoQuery(CommandType commType, string commText, SQLiteParameter param)
        {
            return ExecuteNoQuery(commType, commText, new SQLiteParameter[] { param }, null);
        }

        public static int ExecuteNoQuery(CommandType commType, string commText, SQLiteParameter[] param)
        {
            return ExecuteNoQuery(commType, commText, param, null);
        }

        public static int ExecuteNoQuery(CommandType commType, string commText, SQLiteParameter[] param, SQLiteTransaction tran)
        {
            int result = 0;
            SQLiteConnection conn = new SQLiteConnection(connectString);
            try
            {
                PrepareCommand(conn, comm, commType, commText, param, tran);
                result = comm.ExecuteNonQuery();
                return result;
            }
            catch (Exception e)
            {
                conn.Dispose();
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }

        public static DataSet GetDataSet(CommandType commType, string commText)
        {
            return GetDataSet(commType, commText, null, null);
        }

        public static DataSet GetDataSet(CommandType commType, string commText, SQLiteParameter param)
        {
            return GetDataSet(commType, commText, new SQLiteParameter[] { param }, null);
        }

        public static DataSet GetDataSet(CommandType commType, string commText, SQLiteParameter[] param)
        {
            return GetDataSet(commType, commText, param, null);
        }

        public static DataSet GetDataSet(CommandType commType, string commText, SQLiteParameter[] param, SQLiteTransaction tran)
        {
            DataSet ds = new DataSet();
            using (SQLiteConnection conn = new SQLiteConnection(connectString))
            {
                PrepareCommand(conn, comm, commType, commText, param, tran);
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(comm))
                    adapter.Fill(ds);
            }
            return ds;
        }

        public static SQLiteDataReader ExecuteReader(CommandType commType, string commText)
        {
            return ExecuteReader(commType, commText, null, null);
        }

        public static SQLiteDataReader ExecuteReader(CommandType commType, string commText, SQLiteParameter param)
        {
            return ExecuteReader(commType, commText, new SQLiteParameter[] { param }, null);
        }

        public static SQLiteDataReader ExecuteReader(CommandType commType, string commText, SQLiteParameter[] param)
        {
            return ExecuteReader(commType, commText, param, null);
        }

        public static SQLiteDataReader ExecuteReader(CommandType commType, string commText, SQLiteParameter[] param, SQLiteTransaction tran)
        {
            SQLiteConnection conn = new SQLiteConnection(connectString);
            try
            {
                PrepareCommand(conn, comm, commType, commText, param, tran);
                return comm.ExecuteReader();
            }
            catch (Exception e)
            {
                conn.Close();
                throw e;
            }
        }

        /// <summary>
        /// 通过反射绑定实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="reader">DataReader</param>
        /// <returns>实体对象</returns>
        public static T InfoBinder<T>(IDataReader reader)
        {
            using (reader)
            {
                Type infoType = typeof(T);
                if (!reader.Read())
                    return default(T);
                T model = Activator.CreateInstance<T>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    PropertyInfo pInfo = infoType.GetProperty(reader.GetName(i));
                    pInfo.SetValue(model, reader[i], null);
                }
                return model;

            }
        }

        public static IList<T> InfosBinder<T>(IDataReader reader)
        {
            using (reader)
            {
                IList<T> list = new List<T>();
                Type infoType = typeof(T);
                while (reader.Read())
                {
                    T model = Activator.CreateInstance<T>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        PropertyInfo pInfo = infoType.GetProperty(reader.GetName(i));
                        if (pInfo == null)
                            continue;
                        pInfo.SetValue(model, reader[i] == null ? null : reader[i], null);
                    }
                    list.Add(model);
                }
                return list;
            }
        }

        private static void PrepareCommand(SQLiteConnection conn, SQLiteCommand comm, CommandType commType, string commText, SQLiteParameter[] param, SQLiteTransaction tran)
        {
            comm.Parameters.Clear();
            comm.Connection = conn;
            if (conn.State != ConnectionState.Open)
                conn.Open();
            comm.CommandText = commText;
            comm.CommandType = commType;
            if (tran != null)
                comm.Transaction = tran;
            if (param != null)
                foreach (SQLiteParameter p in param)
                    comm.Parameters.Add(p);
        }

    }
}
