using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Modelo;

namespace Persistencia
{
    public class ContaDAL
    {
        private SqlConnection conn;
        private CategoriaDAL categoria;

        public ContaDAL(SqlConnection conn)
        {
            this.conn = conn;
            string strConn = Db.Conexao.GetSringConnection();
            this.categoria = new CategoriaDAL(new SqlConnection(strConn));
        }

        public List<Conta> ListarTodos(string data_inicial = "", string data_final = "")
        {
            List<Conta> contas = new List<Conta>();

            StringBuilder sql = new StringBuilder("select ");
            sql.Append("con.id, con.descricao, con.valor, con.tipo, con.data_vencimento, cat.nome, cat.id as categoria_id ");
            sql.Append(" from ");
            sql.Append("contas con inner join categorias cat ");
            sql.Append("on con.categoria_id = cat.id");

            bool condicao = !data_inicial.Equals("") && !data_final.Equals("");
            if(condicao)
            {
                sql.Append(" where con.data_vencimento between ");
                sql.Append("@data_inicial and @data_final");
            }

            var command = new SqlCommand(sql.ToString(), this.conn);
            if(condicao)
            {
                command.Parameters.AddWithValue("@data_inicial", data_inicial);
                command.Parameters.AddWithValue("@data_final", data_final);
            }

            this.conn.Open();

            using (SqlDataReader rd = command.ExecuteReader()) 
            {
                while(rd.Read())
                {
                    Conta conta = new Conta()
                    {
                        Id = Convert.ToInt32(rd["id"].ToString()),
                        Descricao = rd["descricao"].ToString(),
                        Tipo = Convert.ToChar(rd["tipo"].ToString()),
                        DataVencimento = DateTime.Parse(rd["data_vencimento"].ToString()),
                        Valor = Convert.ToDouble(rd["valor"].ToString())
                    };
                    int id_categoria = Convert.ToInt32(rd["id"].ToString());
                    conta.Categoria = this.categoria.GetCategoria(id_categoria);
                    contas.Add(conta);
                }
            }
            this.conn.Close();
            return contas;
        }

        public void Salvar(Conta conta)
        {
            if(conta.Id == null)
            {
                Cadastrar(conta);
            }else
            {
                Editar(conta);
            }
        }

        void Cadastrar(Conta conta)
        {
            this.conn.Open();
            SqlCommand command = this.conn.CreateCommand();
            command.CommandText = "insert into contas (descricao, tipo, valor, data_vencimento, categoria_id) values (@descricao, @tipo, @valor, @data_vencimento, @categoria_id)";
            command.Parameters.AddWithValue("@descricao", conta.Descricao);
            command.Parameters.AddWithValue("@tipo", conta.Tipo);
            command.Parameters.AddWithValue("@valor", conta.Valor);
            command.Parameters.AddWithValue("@data_vencimento", conta.DataVencimento);
            command.Parameters.AddWithValue("@categoria_id", conta.Categoria.Id);
            command.ExecuteNonQuery();
            this.conn.Close();

        }

        void Editar(Conta conta)
        {

        }

    }
}
