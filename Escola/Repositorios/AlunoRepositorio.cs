using Escola.Interfaces;

namespace Escola.Repositorios
{
    public class AlunoRepositorio
    {
        private IRepositorio repo;
        public AlunoRepositorio(IRepositorio repo)
        {
            this.repo = repo;
        }

        public int Quantidade()
        {
            return repo.Quantidade();
        }

        public List<Aluno> Todos()
        {
            return repo.Todos();
        }

        public void Salvar(Aluno aluno)
        {
            repo.Salvar(aluno);
        }
    }
}
