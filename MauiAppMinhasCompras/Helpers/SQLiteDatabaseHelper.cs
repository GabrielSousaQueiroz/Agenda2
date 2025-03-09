using MauiAppMinhasCompras.Models;
using SQLite;

namespace MauiAppMinhasCompras.Helpers
{
    public class SQLiteDatabaseHelper
    {
        readonly SQLiteAsyncConnection _conn;

        public SQLiteDatabaseHelper(string path)
        {
            _conn = new SQLiteAsyncConnection(path);
            _conn.CreateTableAsync<Produto>().Wait();
        }

        // Método de inserção de produto
        public Task<int> Insert(Produto p)
        {
            return _conn.InsertAsync(p);
        }

        // Método de atualização de produto
        public Task<int> Update(Produto p)
        {
            // Atualizando todos os campos, incluindo Descricao, Quantidade, Preco, Imposto e DataValidade
            string sql = "UPDATE Produto SET Descricao=?, Quantidade=?, Preco=?, Imposto=?, DataValidade=? WHERE Id=?";

            return _conn.ExecuteAsync(sql, p.Descricao, p.Quantidade, p.Preco, p.Imposto, p.DataValidade, p.Id);
        }

        // Método de exclusão de produto
        public Task<int> Delete(int id)
        {
            return _conn.Table<Produto>().DeleteAsync(i => i.Id == id);
        }

        // Método para obter todos os produtos
        public Task<List<Produto>> GetAll()
        {
            return _conn.Table<Produto>().ToListAsync();
        }

        // Método de busca de produtos
        public Task<List<Produto>> Search(string q)
        {
            string sql = "SELECT * FROM Produto WHERE descricao LIKE '%" + q + "%'";

            return _conn.QueryAsync<Produto>(sql);
        }
    }
}

