using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class NovoProduto : ContentPage
{
    public NovoProduto()
    {
        InitializeComponent();

        // Define a categoria padrão como "Outros" ao iniciar
        picker_categoria.SelectedIndex = picker_categoria.Items.IndexOf("Outros");
        AtualizarLabelCategoria(); // Atualiza a label com a categoria padrão
    }

    private void Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        AtualizarLabelCategoria();
    }

    private void AtualizarLabelCategoria()
    {
        if (picker_categoria.SelectedIndex != -1)
        {
            lbl_categoria_selecionada.Text = $"Categoria selecionada: {picker_categoria.SelectedItem}";
        }
        else
        {
            lbl_categoria_selecionada.Text = "Nenhuma categoria selecionada";
        }
    }

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Validação dos campos obrigatórios
            if (string.IsNullOrWhiteSpace(txt_descricao.Text))
            {
                await DisplayAlert("Atenção", "Por favor, informe a descrição do produto", "OK");
                return;
            }

            if (!double.TryParse(txt_quantidade.Text, out double quantidade) || quantidade <= 0)
            {
                await DisplayAlert("Atenção", "Por favor, informe uma quantidade válida", "OK");
                return;
            }

            if (!double.TryParse(txt_preco.Text, out double preco) || preco <= 0)
            {
                await DisplayAlert("Atenção", "Por favor, informe um preço válido", "OK");
                return;
            }

            Produto p = new Produto
            {
                Descricao = txt_descricao.Text,
                Categoria = picker_categoria.SelectedItem?.ToString() ?? "Outros",
                Quantidade = quantidade,
                Preco = preco
            };

            await App.Db.Insert(p);
            await DisplayAlert("Sucesso!", "Produto cadastrado com sucesso", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "OK");
        }
    }
}