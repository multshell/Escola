namespace Escola.Interfaces
{
    public interface IRepositorio
    {
        List<Aluno> Todos();
        void Salvar(Aluno aluno);
        int Quantidade();
    }
}
