using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class EditarProduto : ContentPage
{
    public EditarProduto()
    {
        InitializeComponent();
    }

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Obtendo o Produto a partir do BindingContext
            Produto produto_anexado = BindingContext as Produto;

            if (produto_anexado != null)
            {
                // Criando o novo produto com os valores atualizados
                Produto p = new Produto
                {
                    Id = produto_anexado.Id,
                    Descricao = txt_descricao.Text,
                    Quantidade = Convert.ToDouble(txt_quantidade.Text),
                    Preco = Convert.ToDouble(txt_preco.Text),
                    Imposto = string.IsNullOrWhiteSpace(txt_imposto.Text) ? 0 : Convert.ToDouble(txt_imposto.Text), // Se o imposto estiver vazio, coloca 0
                    DataValidade = dp_validade.Date // Garantindo que a Data de Validade seja lida corretamente
                };

                // Atualizando o produto no banco de dados
                await App.Db.Update(p);

                // Exibindo a mensagem de sucesso e retornando para a página anterior
                await DisplayAlert("Sucesso!", "Registro Atualizado", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Erro", "Produto não encontrado", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }
}
