// See https://aka.ms/new-console-template for more information
public class Aluno
{
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
}