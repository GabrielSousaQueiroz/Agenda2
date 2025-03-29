using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class EditarProduto : ContentPage
{
    public EditarProduto()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Configura a categoria selecionada quando a página aparece
        if (BindingContext is Produto produto)
        {
            if (!string.IsNullOrEmpty(produto.Categoria))
            {
                picker_categoria.SelectedItem = produto.Categoria;
            }
            else
            {
                picker_categoria.SelectedIndex = picker_categoria.Items.IndexOf("Outros");
            }
        }
        AtualizarLabelCategoria(); // Atualiza a label ao carregar a página
    }

    private void Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        AtualizarLabelCategoria();
    }

    private void AtualizarLabelCategoria()
    {
        if (picker_categoria.SelectedIndex != -1 && lbl_categoria_selecionada != null)
        {
            lbl_categoria_selecionada.Text = $"Categoria selecionada: {picker_categoria.SelectedItem}";
        }
        else if (lbl_categoria_selecionada != null)
        {
            lbl_categoria_selecionada.Text = "Nenhuma categoria selecionada";
        }
    }

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (BindingContext is not Produto produto_anexado)
            {
                await DisplayAlert("Erro", "Produto não encontrado", "OK");
                return;
            }

            // Validações
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
                Id = produto_anexado.Id,
                Descricao = txt_descricao.Text,
                Categoria = picker_categoria.SelectedItem?.ToString() ?? "Outros",
                Quantidade = quantidade,
                Preco = preco
            };

            await App.Db.Update(p);
            await DisplayAlert("Sucesso!", "Produto atualizado com sucesso", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "OK");
        }
    }
}