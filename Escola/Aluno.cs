// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;
using System.Configuration;
using System.Data.SqlClient;

public class Aluno
{
    private static List<Aluno> alunos = new List<Aluno>();

    public int Id { get; set; }
    public string Nome { get; set; }
    public string Matricula { get; set; }
    public List<double> Notas { get; set; }

    public double Media()
    {
        double somaNotas = 0;
        foreach (var nota in this.Notas) somaNotas += nota;
        
        return somaNotas / this.Notas.Count;
    }

    public string Situacao()
    {
        return (this.Media() > 6 ? "Aprovado" : "Reprovado");
    }

    public string NotasFormatada()
    {
        return string.Join(",", this.Notas);
    }

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
        using (var cnn = new SqlConnection(Aluno.stringConexaoSql()))
        {
            cnn.Open();
            using(var cmd = new SqlCommand("select * from alunos", cnn))
            {
                using(SqlDataReader dr = cmd.ExecuteReader())
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
        Aluno.alunos = Aluno.TodosJson();
        Aluno.alunos.Add(aluno);
        File.WriteAllText(caminhoJson(), JsonConvert.SerializeObject(Aluno.alunos));
    }

    public static void AdicionarSql(Aluno aluno)
    {
        using (var cnn = new SqlConnection(Aluno.stringConexaoSql()))
        {
            cnn.Open();
            var cmd = new SqlCommand("insert into alunos(nome, matricula) values (@nome, @matricula); select @@identity", cnn);
            cmd.Parameters.AddWithValue("@nome", aluno.Nome);
            cmd.Parameters.AddWithValue("@matricula", aluno.Matricula);
            int aluno_id = Convert.ToInt32(cmd.ExecuteScalar());

            foreach(var nota in aluno.Notas)
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