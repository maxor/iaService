using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iaSyncDWH
{
    public class DBSync
    {
        string _connectionString =
            "Data Source=(local);Initial Catalog=Northwind;"
            + "Integrated Security=true";

        public event EventHandler<ExecEventArgs> onExecution;
        List<Task> _tasks;

        public DBSync()
        {
            _tasks = new List<Task>();
        }
        public DBSync(string conectionString) : this()
        {
            _connectionString = conectionString;
        }

        public void RunProcedure(string[] procs)
        {
            foreach (string proc in procs)
            {
                ExecProcedure(proc);
            }
           
        }

        public void RunProcedure(string procedureName)
        {
            ExecProcedure(procedureName);
        }


        protected void ExecProcedure(string procName)
        {
             ExecProcedureAsync(procName, _connectionString);
        }

        async protected void ExecProcedureAsync(string procName, string connectionString, )
        {
            using (SqlConnection connection =
            new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("exec " + procName, connection);
                //command.Parameters.AddWithValue("@pricePoint", paramValue);
                try
                {
                    connection.Open();
                    int counter  = await command.ExecuteNonQueryAsync();
                    connection.Close();
                    onExecution(this, new ExecEventArgs(DBEvents.Done, "Процедура "+ procName + " выполнена. Затронуто строк - " + counter));
                }
                catch (Exception ex)
                {
                    onExecution(this, new ExecEventArgs(DBEvents.Error, "При выполнении процедуры " + procName + " произошла ошибка.", ex));
                }
            }
        }

        protected int CountFields(SqlCommand command)
        {
            using (SqlConnection connection =
            new SqlConnection(_connectionString))
            {
                SqlCommand Command  = new SqlCommand("SELECT count(1) from InfoBufferNew.dbo.ALLMoves" , connection);
                SqlCommand maxCommand = new SqlCommand("SELECT max(ID) from InfoBufferNew.dbo.ALLMoves", connection);
                int count = -1;
                //command.Parameters.AddWithValue("@pricePoint", paramValue);
                try
                {
                    connection.Open();
                    count = Command.ExecuteNonQuery();
                    onExecution(this, new ExecEventArgs(DBEvents.Done, "Затронуто строк - " + count));
                }
                catch (Exception ex)
                {
                    onExecution(this, new ExecEventArgs(DBEvents.Error, "При выполнении запроса \""+ command + "\" произошла ошибка.", ex));
                }
                return count;
            }
        }



        

    }
}
