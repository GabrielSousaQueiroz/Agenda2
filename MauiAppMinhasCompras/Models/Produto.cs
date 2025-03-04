using SQLite;

namespace MauiAppMinhasCompras.Models
{
    public class Produto
    {
        string _descricao;
        double _imposto;

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Descricao
        {
            get => _descricao;
            set
            {
                if (value == null)
                {
                    throw new Exception("Por favor, preencha a descrição");
                }
                _descricao = value;
            }
        }
        public double Quantidade { get; set; }
        public double Preco { get; set; }
        public double Imposto
        {
            get => _imposto;
            set
            {
                if (value < 0)
                {
                    throw new Exception("O imposto não pode ser negativo");
                }
                _imposto = value;
            }
        }
        public double Total { get => Quantidade * Preco * (1 + Imposto / 100); }
    }
}
