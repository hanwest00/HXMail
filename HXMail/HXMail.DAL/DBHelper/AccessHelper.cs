using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace HXMail.DAL
{
    public class AccessHelper
    {

        private static readonly string connectString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|\DB\H_m_d.mdb;Persist Security Info=True;Jet OLEDB:Database Password=liaMXH";

        private static OleDbConnection conn = new OleDbConnection(connectString);

        private static OleDbCommand comm = new OleDbCommand();

        public static object ExecuteScaler(CommandType commType, string commText)
        {
            return ExecuteScaler(commType, commText, null, null);
        }

        public static object ExecuteScaler(CommandType commType, string commText, OleDbParameter param)
        {
            return ExecuteScaler(commType, commText, new OleDbParameter[] { param }, null);
        }

        public static object ExecuteScaler(CommandType commType, string commText, OleDbParameter[] param)
        {
            return ExecuteScaler(commType, commText, param, null);
        }

        public static object ExecuteScaler(CommandType commType, string commText, OleDbParameter[] param, OleDbTransaction tran)
        {
            object ret = null;
            using (OleDbConnection conn = new OleDbConnection(connectString))
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

        public static int ExecuteNoQuery(CommandType commType, string commText, OleDbParameter param)
        {
            return ExecuteNoQuery(commType, commText, new OleDbParameter[] { param }, null);
        }

        public static int ExecuteNoQuery(CommandType commType, string commText, OleDbParameter[] param)
        {
            return ExecuteNoQuery(commType, commText, param, null);
        }

        public static int ExecuteNoQuery(CommandType commType, string commText, OleDbParameter[] param, OleDbTransaction tran)
        {
            int result = 0;
            using (OleDbConnection conn = new OleDbConnection(connectString))
            {
                PrepareCommand(conn, comm, commType, commText, param, tran);
                result = comm.ExecuteNonQuery();
            }
            return result;
        }

        public static DataSet GetDataSet(CommandType commType, string commText)
        {
            return GetDataSet(commType, commText, null, null);
        }

        public static DataSet GetDataSet(CommandType commType, string commText, OleDbParameter param)
        {
            return GetDataSet(commType, commText, new OleDbParameter[] { param }, null);
        }

        public static DataSet GetDataSet(CommandType commType, string commText, OleDbParameter[] param)
        {
            return GetDataSet(commType, commText, param, null);
        }

        public static DataSet GetDataSet(CommandType commType, string commText, OleDbParameter[] param, OleDbTransaction tran)
        {
            DataSet ds = new DataSet();
            using (OleDbConnection conn = new OleDbConnection(connectString))
            {
                PrepareCommand(conn, comm, commType, commText, param, tran);
                using (OleDbDataAdapter adapter = new OleDbDataAdapter(comm))
                    adapter.Fill(ds);
            }
            return ds;
        }

        public static OleDbDataReader ExecuteReader(CommandType commType, string commText)
        {
            return ExecuteReader(commType, commText, null, null);
        }

        public static OleDbDataReader ExecuteReader(CommandType commType, string commText, OleDbParameter param)
        {
            return ExecuteReader(commType, commText, new OleDbParameter[] { param }, null);
        }

        public static OleDbDataReader ExecuteReader(CommandType commType, string commText, OleDbParameter[] param)
        {
            return ExecuteReader(commType, commText, param, null);
        }

        public static OleDbDataReader ExecuteReader(CommandType commType, string commText, OleDbParameter[] param, OleDbTransaction tran)
        {
            OleDbDataReader dataReader = null;
            OleDbConnection conn = new OleDbConnection(connectString);
            try
            {
                PrepareCommand(conn, comm, commType, commText, param, tran);
                dataReader = comm.ExecuteReader();
            }
            catch (Exception e)
            {
                conn.Close();
                throw e;
            }
            return dataReader;
        }


        private static void PrepareCommand(OleDbConnection conn, OleDbCommand comm, CommandType commType, string commText, OleDbParameter[] param, OleDbTransaction tran)
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
                foreach (OleDbParameter p in param)
                    comm.Parameters.Add(p);

        }

    }
}
