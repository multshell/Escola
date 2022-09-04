// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;
using System.Configuration;

public class Aluno
{
    private static List<Aluno> alunos = new List<Aluno>();

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

    public static List<Aluno> Todos()
    {
        if (File.Exists(caminhoJson()))
        {
            var conteudo = File.ReadAllText(caminhoJson());
            Aluno.alunos = JsonConvert.DeserializeObject<List<Aluno>>(conteudo);
        }

        return Aluno.alunos;
    }

    private static string? caminhoJson()
    {
        return ConfigurationManager.AppSettings["caminho_json"];

    }

    public static void Adicionar(Aluno aluno)
    {
        Aluno.alunos = Aluno.Todos();
        Aluno.alunos.Add(aluno);
        File.WriteAllText(caminhoJson(), JsonConvert.SerializeObject(Aluno.alunos));
    }
}