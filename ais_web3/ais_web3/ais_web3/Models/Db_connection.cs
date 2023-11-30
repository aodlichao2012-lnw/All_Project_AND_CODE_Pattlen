using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml;

namespace ais_web3.Models
{
    public  class Db_connection
    {
        public int Execute(string sql = "" , string ConnectionString = "")
        {
            try
            {
                int i = 0;
                DataTable dt = new DataTable();
                using (OracleConnection connection = new OracleConnection(ConnectionString))
                {
                    connection.Open();
                    using (OracleCommand command = connection.CreateCommand())
                    {

                        command.CommandText = sql;
                        i = command.ExecuteNonQuery();
                        connection.Close();

                    }
                    return i;
                }
            }
            catch
            {
                return 0;
            }
         
        }   
        public int Execute1(string sql = "" , string ConnectionString = "")
        {
            try
            {
                int i = 0;
                DataTable dt = new DataTable();
                using (OracleConnection connection = new OracleConnection(ConnectionString))
                {
                    connection.Open();
                    using (OracleCommand command = connection.CreateCommand())
                    {

                        command.CommandText = sql;
                        i = command.ExecuteNonQuery();
                        connection.Close();

                    }
                    return i;
                }
            }
            catch
            {
                return 0;
            }
         
        }   
        
        public int Execute2(string sql = "" , string ConnectionString = "")
        {
            try
            {
                int i = 0;
                DataTable dt = new DataTable();
                using (OracleConnection connection = new OracleConnection(ConnectionString))
                {
                    connection.Open();
                    using (OracleCommand command = connection.CreateCommand())
                    {

                        command.CommandText = sql;
                        i = command.ExecuteNonQuery();
                        connection.Close();

                    }
                    return i;
                }
            }
            catch
            {
                return 0;
            }
         
        }   
        
        public int Execute3(string sql = "" , string ConnectionString = "")
        {
            try
            {
                int i = 0;
                DataTable dt = new DataTable();
                using (OracleConnection connection = new OracleConnection(ConnectionString))
                {
                    connection.Open();
                    using (OracleCommand command = connection.CreateCommand())
                    {

                        command.CommandText = sql;
                        i = command.ExecuteNonQuery();
                        connection.Close();

                    }
                    return i;
                }
            }
            catch
            {
                return 0;
            }
         
        } 
        
        public int Execute4(string sql = "" , string ConnectionString = "")
        {
            try
            {
                int i = 0;
                DataTable dt = new DataTable();
                using (OracleConnection connection = new OracleConnection(ConnectionString))
                {
                    connection.Open();
                    using (OracleCommand command = connection.CreateCommand())
                    {

                        command.CommandText = sql;
                        i = command.ExecuteNonQuery();
                        connection.Close();

                    }
                    return i;
                }
            }
            catch
            {
                return 0;
            }
         
        }  
        
        public int Execute5(string sql = "" , string ConnectionString = "")
        {
            try
            {
                int i = 0;
                DataTable dt = new DataTable();
                using (OracleConnection connection = new OracleConnection(ConnectionString))
                {
                    connection.Open();
                    using (OracleCommand command = connection.CreateCommand())
                    {

                        command.CommandText = sql;
                        i = command.ExecuteNonQuery();
                        connection.Close();

                    }
                    return i;
                }
            }
            catch
            {
                return 0;
            }
         
        }   
        
        public int Execute6(string sql = "" , string ConnectionString = "")
        {
            try
            {
                int i = 0;
                DataTable dt = new DataTable();
                using (OracleConnection connection = new OracleConnection(ConnectionString))
                {
                    connection.Open();
                    using (OracleCommand command = connection.CreateCommand())
                    {

                        command.CommandText = sql;
                        i = command.ExecuteNonQuery();
                        connection.Close();

                    }
                    return i;
                }
            }
            catch
            {
                return 0;
            }
         
        }
        public DataTable Query(string sql, string Connectionstring = "")
        {
            try
            {
                DataTable dt = new DataTable();
                using (OracleConnection connection = new OracleConnection(Connectionstring))
                {
                    connection.Open();
                    using (OracleCommand command = connection.CreateCommand())
                    {

                        command.CommandText = sql;
                        OracleDataReader reader = command.ExecuteReader();
                        {
                            if (reader.HasRows)
                            {
                                dt.Load(reader);
                                connection.Close();
                            }
                        }
                    }
                    return dt;
                }
            }
            catch
            {
                return null;
            }
          
        }  
        
        public DataTable Query1(string sql, string Connectionstring = "")
        {
            try
            {
                DataTable dt = new DataTable();
                using (OracleConnection connection = new OracleConnection(Connectionstring))
                {
                    connection.Open();
                    using (OracleCommand command = connection.CreateCommand())
                    {

                        command.CommandText = sql;
                        OracleDataReader reader = command.ExecuteReader();
                        {
                            if (reader.HasRows)
                            {
                                dt.Load(reader);
                                connection.Close();
                            }
                        }
                    }
                    return dt;
                }
            }
            catch
            {
                return null;
            }
          
        }  
        
        public DataTable Query2(string sql, string Connectionstring = "")
        {
            try
            {
                DataTable dt = new DataTable();
                using (OracleConnection connection = new OracleConnection(Connectionstring))
                {
                    connection.Open();
                    using (OracleCommand command = connection.CreateCommand())
                    {

                        command.CommandText = sql;
                        OracleDataReader reader = command.ExecuteReader();
                        {
                            if (reader.HasRows)
                            {
                                dt.Load(reader);
                                connection.Close();
                            }
                        }
                    }
                    return dt;
                }
            }
            catch
            {
                return null;
            }
          
        } 
        
        public DataTable Query3(string sql, string Connectionstring = "")
        {
            try
            {
                DataTable dt = new DataTable();
                using (OracleConnection connection = new OracleConnection(Connectionstring))
                {
                    connection.Open();
                    using (OracleCommand command = connection.CreateCommand())
                    {

                        command.CommandText = sql;
                        OracleDataReader reader = command.ExecuteReader();
                        {
                            if (reader.HasRows)
                            {
                                dt.Load(reader);
                                connection.Close();
                            }
                        }
                    }
                    return dt;
                }
            }
            catch
            {
                return null;
            }
          
        }
        
        public DataTable Query4(string sql, string Connectionstring = "")
        {
            try
            {
                DataTable dt = new DataTable();
                using (OracleConnection connection = new OracleConnection(Connectionstring))
                {
                    connection.Open();
                    using (OracleCommand command = connection.CreateCommand())
                    {

                        command.CommandText = sql;
                        OracleDataReader reader = command.ExecuteReader();
                        {
                            if (reader.HasRows)
                            {
                                dt.Load(reader);
                                connection.Close();
                            }
                        }
                    }
                    return dt;
                }
            }
            catch
            {
                return null;
            }
          
        }   
        
        public DataTable Query5(string sql, string Connectionstring = "")
        {
            try
            {
                DataTable dt = new DataTable();
                using (OracleConnection connection = new OracleConnection(Connectionstring))
                {
                    connection.Open();
                    using (OracleCommand command = connection.CreateCommand())
                    {

                        command.CommandText = sql;
                        OracleDataReader reader = command.ExecuteReader();
                        {
                            if (reader.HasRows)
                            {
                                dt.Load(reader);
                                connection.Close();
                            }
                        }
                    }
                    return dt;
                }
            }
            catch
            {
                return null;
            }
          
        } 
        
        public DataTable Query6(string sql, string Connectionstring = "")
        {
            try
            {
                DataTable dt = new DataTable();
                using (OracleConnection connection = new OracleConnection(Connectionstring))
                {
                    connection.Open();
                    using (OracleCommand command = connection.CreateCommand())
                    {

                        command.CommandText = sql;
                        OracleDataReader reader = command.ExecuteReader();
                        {
                            if (reader.HasRows)
                            {
                                dt.Load(reader);
                                connection.Close();
                            }
                        }
                    }
                    return dt;
                }
            }
            catch
            {
                return null;
            }
          
        } 
        
        public DataTable Query7(string sql, string Connectionstring = "")
        {
            try
            {
                DataTable dt = new DataTable();
                using (OracleConnection connection = new OracleConnection(Connectionstring))
                {
                    connection.Open();
                    using (OracleCommand command = connection.CreateCommand())
                    {

                        command.CommandText = sql;
                        OracleDataReader reader = command.ExecuteReader();
                        {
                            if (reader.HasRows)
                            {
                                dt.Load(reader);
                                connection.Close();
                            }
                        }
                    }
                    return dt;
                }
            }
            catch
            {
                return null;
            }
          
        }  
        
        public DataTable Query8(string sql, string Connectionstring = "")
        {
            try
            {
                DataTable dt = new DataTable();
                using (OracleConnection connection = new OracleConnection(Connectionstring))
                {
                    connection.Open();
                    using (OracleCommand command = connection.CreateCommand())
                    {

                        command.CommandText = sql;
                        OracleDataReader reader = command.ExecuteReader();
                        {
                            if (reader.HasRows)
                            {
                                dt.Load(reader);
                                connection.Close();
                            }
                        }
                    }
                    return dt;
                }
            }
            catch
            {
                return null;
            }
          
        }
        
        public DataTable Query9(string sql, string Connectionstring = "")
        {
            try
            {
                DataTable dt = new DataTable();
                using (OracleConnection connection = new OracleConnection(Connectionstring))
                {
                    connection.Open();
                    using (OracleCommand command = connection.CreateCommand())
                    {

                        command.CommandText = sql;
                        OracleDataReader reader = command.ExecuteReader();
                        {
                            if (reader.HasRows)
                            {
                                dt.Load(reader);
                                connection.Close();
                            }
                        }
                    }
                    return dt;
                }
            }
            catch
            {
                return null;
            }
          
        }
    }
}