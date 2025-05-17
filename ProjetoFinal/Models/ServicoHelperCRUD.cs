using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using System.IO;

namespace ProjetoFinal.Models
{
    internal class ServicoHelperCRUD
    {
        readonly SQLiteAsyncConnection _dbCon;

        public ServicoHelperCRUD()
        {
            //Se não existir base de dados cria uma nova no caminho especificado
            if (App.Conexao == "")
            {
                App.Conexao = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BaseDados.db3");
            }
            _dbCon = new SQLiteAsyncConnection(App.Conexao);
            //Cria uma tabela na base de dados para armazenar os serviços
            _dbCon.CreateTableAsync<Servico>().Wait();
        }
        //vai buscar os serviços à base de dados numa lista 
        public async Task<List<Servico>> GetServicos()
        {
            return await _dbCon.Table<Servico>().ToListAsync();
        }
        public async Task<int> SaveServico(Servico servico)
        {
            //auto ID 0 significa que o objeto ainda nao foi inserido na base de dados, logo insere-o
            if(servico.AutoId == 0)
            {
                return await _dbCon.InsertAsync(servico);
            }
            //caso contrário atualiza o objeto na base de dados
            else
            {
                return await _dbCon.UpdateAsync(servico);
            }
        }
        public async Task<int> DeleteServico(Servico servico)
        {
            return await _dbCon.DeleteAsync(servico);
        }
    }
}
