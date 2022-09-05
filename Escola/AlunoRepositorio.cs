﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escola
{
    public class AlunoRepositorio
    {
        public static List<Aluno> TodosJson()
        {
            if (File.Exists(caminhoJson()))
            {
                var conteudo = File.ReadAllText(caminhoJson());
                Aluno.alunos = JsonConvert.DeserializeObject<List<Aluno>>(conteudo);
            }

            return Aluno.alunos;
        }

        public static List<Aluno> TodosSql()
        {
            Aluno.alunos = new List<Aluno>();
            using (var cnn = new SqlConnection(AlunoRepositorio.stringConexaoSql()))
            {
                cnn.Open();
                using (var cmd = new SqlCommand("select * from alunos", cnn))
                {
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var aluno = new Aluno();
                            aluno.Id = Convert.ToInt32(dr["id"]);
                            aluno.Nome = dr["nome"].ToString();
                            aluno.Matricula = dr["matricula"].ToString();

                            Aluno.alunos.Add(aluno);

                        }

                    }

                    foreach (var aluno in Aluno.alunos)
                    {
                        using (var cmdNotas = new SqlCommand("select * from notas where aluno_id=" + aluno.Id, cnn))
                        {
                            using (SqlDataReader drNotas = cmdNotas.ExecuteReader())
                            {
                                aluno.Notas = new List<double>();
                                while (drNotas.Read())
                                {
                                    aluno.Notas.Add(Convert.ToDouble(drNotas["nota"]));
                                }
                            }
                        }
                    }
                }
                cnn.Close();
            }

            return Aluno.alunos;
        }

        private static string? caminhoJson()
        {
            return ConfigurationManager.AppSettings["caminho_json"];

        }

        private static string? stringConexaoSql()
        {
            return ConfigurationManager.AppSettings["conexao_sql"];

        }

        public static void AdicionarJson(Aluno aluno)
        {
            Aluno.alunos = AlunoRepositorio.TodosJson();
            Aluno.alunos.Add(aluno);
            File.WriteAllText(caminhoJson(), JsonConvert.SerializeObject(Aluno.alunos));
        }

        public static void AdicionarSql(Aluno aluno)
        {
            using (var cnn = new SqlConnection(AlunoRepositorio.stringConexaoSql()))
            {
                cnn.Open();
                var cmd = new SqlCommand("insert into alunos(nome, matricula) values (@nome, @matricula); select @@identity", cnn);
                cmd.Parameters.AddWithValue("@nome", aluno.Nome);
                cmd.Parameters.AddWithValue("@matricula", aluno.Matricula);
                int aluno_id = Convert.ToInt32(cmd.ExecuteScalar());

                foreach (var nota in aluno.Notas)
                {
                    var cmdNota = new SqlCommand("insert into notas(aluno_id, nota) values (@aluno_id, @nota)", cnn);
                    cmdNota.Parameters.AddWithValue("@aluno_id", aluno_id);
                    cmdNota.Parameters.AddWithValue("@nota", nota);
                    cmdNota.ExecuteNonQuery();
                }
                cnn.Close();
            }
        }
    }
}
