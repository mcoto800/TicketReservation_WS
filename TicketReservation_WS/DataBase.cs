using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace TicketReservation_WS
{
    public class DataBase
    {
        private SqlConnection connection;
        private SqlCommand command;
        private SqlDataAdapter adapter;

        public DataBase()
        {
            createConnection();
            openConnection();
        }

        public string getConnection()
        {
            try
            {
                return Properties.Settings.Default.db_connection.ToString();
            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry("Application", "GetConnection: " + ex.Message);
                return "";
            }
        }
        public bool createConnection()
        {
            try
            {
                connection = new SqlConnection(getConnection());
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry("Application", "CreateConnection: " + ex.Message);
                return false;
            }
        }
        public bool openConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry("Application", "OpenConnection: " + ex.Message);
                return false;
            }
        }
        public bool closeConnection()
        {
            try
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                return true;

            }
            catch (Exception generatedExceptionName)
            {
                return false;
            }
        }

        public SqlTransaction beginTrans()
        {
            try
            {
                SqlTransaction trans = connection.BeginTransaction();
                return trans;
            }
            catch (Exception generatedExceptionName)
            {
                return null;
            }

        }
        public bool commitTrans(SqlTransaction trans)
        {
            try
            {
                trans.Commit();

                return true;

            }
            catch (Exception generatedExceptionName)
            {
                return false;
            }

        }
        public bool rollbackTrans(SqlTransaction trans)
        {
            try
            {
                trans.Rollback();
                return true;
            }
            catch (Exception generatedExceptionName)
            {
                return false;
            }

        }
        public bool executeCommand(string commandTxt)
        {
            try
            {
                SqlTransaction trans;

                trans = beginTrans();
                command = new SqlCommand(commandTxt, connection, trans);
                command.ExecuteNonQuery();
                closeConnection();
                return true;
            }
            catch (Exception generatedExceptionName)
            {
                closeConnection();
                return false;
            }

        }
        public bool executeCommand(string commandTxt, SqlParameter[] parameters)
        {
            try
            {
                SqlTransaction trans ;
                
                trans = beginTrans();
                command = new SqlCommand(commandTxt, connection, trans);
                int i = 0;
                while (i != parameters.Length)
                {
                    command.Parameters.Add(parameters[i]);
                    i += 1;
                }
                command.ExecuteNonQuery();
                commitTrans(trans);
                closeConnection();
                return true;
            }
            catch (Exception generatedExceptionName)
            {
                System.Diagnostics.EventLog.WriteEntry("Application", "executeCommand: " + generatedExceptionName.Message);
                closeConnection();
                return false;
            }

        }
        public object executeComandEscalar(string commandTxt)
        {
            object response = null;
            try
            {
                command = new SqlCommand(commandTxt, connection);
                response = command.ExecuteScalar();
                return response;
            }
            catch (Exception generatedExceptionName)
            {
                return response;
            }
        }
        public object executeComandEscalar(string commandTxt, SqlTransaction trans, SqlParameter[] parameters)
        {
            object response = null;
            try
            {
                command = new SqlCommand(commandTxt, connection, trans);
                command.CommandType = CommandType.StoredProcedure;
                int i = 0;
                while (i != parameters.Length)
                {
                    command.Parameters.Add(parameters[i]);
                    i += 1;
                }
                response = command.ExecuteScalar();
                closeConnection();
                return response;


            }
            catch (Exception generatedExceptionName)
            {
                return response;
            }

        }
        public DataSet readCommand(string commandTxt, string table)
        {
            DataSet data = new DataSet();
            try
            {
                adapter = new SqlDataAdapter(commandTxt, connection);
                adapter.Fill(data, table);
                closeConnection();
            }
            catch (Exception ex)
            {
                closeConnection();
                System.Diagnostics.EventLog.WriteEntry("Application", "ReadCommand: " + ex.Message);
            }
            return data;

        }
        public DataSet readCommand(string commandTxt, string table, SqlParameter[] parameters)
        {
            DataSet data = new DataSet();

            try
            {
                command = new SqlCommand(commandTxt, connection);
                //  comando.CommandType = CommandType.StoredProcedure
                int i = 0;
                while (i != parameters.Length)
                {
                    command.Parameters.Add(parameters[i]);
                    i += 1;
                }
                //System.Diagnostics.EventLog.WriteEntry("Application", "ReadCommand: " + command.ToString());
                adapter = new SqlDataAdapter(command);
                adapter.Fill(data, table);
                closeConnection();

            }
            catch (Exception generatedExceptionName)
            {
                System.Diagnostics.EventLog.WriteEntry("Application", "ReadCommand: " + generatedExceptionName.Message);
                closeConnection();
            }
            return data;

        }

        public DataSet readCommand2(string commandTxt)
        {
            DataSet data = new DataSet();
            try
            {
                adapter = new SqlDataAdapter(commandTxt, connection);
                adapter.Fill(data);
                closeConnection();
            }
            catch (Exception ex)
            {
                closeConnection();
                System.Diagnostics.EventLog.WriteEntry("Application", "ReadCommand2: " + ex.Message);
            }
            return data;

        }


    }


}