using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escola.Interfaces
{
    public interface IRepositorio
    {
        List<Aluno> Todos();
        void Salvar(Aluno aluno);
        int Quantidade();
    }
}
