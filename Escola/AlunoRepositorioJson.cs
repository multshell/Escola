using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escola
{
    public class AlunoRepositorioJson
    {
        private string? caminhoJson()
        {
            return ConfigurationManager.AppSettings["caminho_json"];

        }

        public List<Aluno> TodosJson()
        {
            var alunos = new List<Aluno>();
            if (File.Exists(this.caminhoJson()))
            {
                var conteudo = File.ReadAllText(this.caminhoJson());
                alunos = JsonConvert.DeserializeObject<List<Aluno>>(conteudo);
            }

            return alunos;
        }
               
        public void AdicionarJson(Aluno aluno)
        {
            var alunos = this.TodosJson();
            alunos.Add(aluno);
            File.WriteAllText(this.caminhoJson(), JsonConvert.SerializeObject(alunos));
        }
                
    }
}
