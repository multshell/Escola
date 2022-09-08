using Escola.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escola.Repositorios
{
    public class AlunoRepositorioJson : IRepositorio
    {
        private string? caminhoJson()
        {
            return ConfigurationManager.AppSettings["caminho_json"];

        }

        public List<Aluno> Todos()
        {
            var alunos = new List<Aluno>();
            if (File.Exists(this.caminhoJson()))
            {
                var conteudo = File.ReadAllText(this.caminhoJson());
                alunos = JsonConvert.DeserializeObject<List<Aluno>>(conteudo);
            }

            return alunos;
        }

        public void Salvar(Aluno aluno)
        {
            var alunos = this.Todos();
            alunos.Add(aluno);
            File.WriteAllText(this.caminhoJson(), JsonConvert.SerializeObject(alunos));
        }

        public int Quantidade()
        {
            return this.Todos().Count;
        }
    }
}
