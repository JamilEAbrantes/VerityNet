using System;
using System.Data;
using System.Data.SqlClient;
using VerityNet.Shared;

namespace VerityNet.Infra.Data.DataContext
{
    public class VerityNetDataContext : IDisposable
    {
        public SqlConnection Connection { get; set; }
        public SqlCommand Command { get; set; }
        public SqlParameter Parameter { get; set; }

        public VerityNetDataContext()
        {
            Connection = new SqlConnection(Settings.ConnectionString);
            Connection.Open();

            Command = new SqlCommand();
            Command.Connection = Connection;
            Command.CommandType = CommandType.StoredProcedure;

            LimparParametros();

            Parameter = new SqlParameter();
        }

        public void AdicionarParametro(
            string nome,
            SqlDbType tipo,
            int tamanho,
            object valor)
        {
            Parameter = new SqlParameter();
            Parameter.ParameterName = nome;
            Parameter.SqlDbType = tipo;
            Parameter.Size = tamanho;
            Parameter.Value = valor;
            Command.Parameters.Add(Parameter);
        }

        public void AdicionarParametro(
            string nome,
            SqlDbType tipo,
            object valor)
        {
            var parametro = new SqlParameter();
            parametro.ParameterName = nome;
            parametro.SqlDbType = tipo;
            parametro.Value = valor;
            Command.Parameters.Add(parametro);
        }

        public void LimparParametros()
        {
            Command.Parameters.Clear();
        }

        public DataTable ExecutarConsulta(string procNome)
        {
            Command.CommandText = procNome;
            Command.ExecuteNonQuery();

            var dtreader = Command.ExecuteReader();
            var dtresult = new DataTable();
            dtresult.Load(dtreader);

            LimparParametros();

            return dtresult;
        }

        public void ExecutarCommando(string procNome)
        {
            Command.CommandText = procNome;
            Command.ExecuteNonQuery();

            LimparParametros();
        }

        public void Dispose()
        {
            if (Connection.State != ConnectionState.Closed)
                Connection.Close();
        }
    }
}