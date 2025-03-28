using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    ObservableCollection<Produto> lista = new ObservableCollection<Produto>();
    private string _categoriaSelecionada = "Todas";
    private string _textoBusca = string.Empty;

    public ListaProduto()
    {
        InitializeComponent();
        lst_produtos.ItemsSource = lista;
        picker_categorias.SelectedIndex = 0; // Seleciona "Todas" por padrão
        AtualizarLabelFiltro();
    }

    protected async override void OnAppearing()
    {
        await CarregarProdutos();
        AtualizarLabelFiltro();
    }

    private void AtualizarLabelFiltro()
    {
        if (string.IsNullOrWhiteSpace(_textoBusca))
        {
            if (_categoriaSelecionada == "Todas")
            {
                lbl_filtro_ativo.Text = "Mostrando: Todas as categorias";
            }
            else
            {
                lbl_filtro_ativo.Text = $"Filtro ativo: {_categoriaSelecionada}";
            }
        }
        else
        {
            if (_categoriaSelecionada == "Todas")
            {
                lbl_filtro_ativo.Text = $"Buscando: \"{_textoBusca}\" em todas as categorias";
            }
            else
            {
                lbl_filtro_ativo.Text = $"Buscando: \"{_textoBusca}\" em {_categoriaSelecionada}";
            }
        }
    }

    private async Task CarregarProdutos(string categoria = null)
    {
        try
        {
            lista.Clear();
            List<Produto> tmp;

            if (string.IsNullOrEmpty(categoria) || categoria == "Todas")
            {
                tmp = await App.Db.GetAll();
            }
            else
            {
                tmp = await App.Db.GetByCategory(categoria);
            }

            // Aplica filtro de texto se houver
            if (!string.IsNullOrWhiteSpace(_textoBusca))
            {
                tmp = tmp.Where(p => p.Descricao.Contains(_textoBusca, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            tmp.ForEach(i => lista.Add(i));
            AtualizarLabelFiltro();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "OK");
        }
    }

    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            Navigation.PushAsync(new Views.NovoProduto());
        }
        catch (Exception ex)
        {
            DisplayAlert("Erro", ex.Message, "OK");
        }
    }

    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            _textoBusca = e.NewTextValue;
            lst_produtos.IsRefreshing = true;
            await CarregarProdutos(_categoriaSelecionada == "Todas" ? null : _categoriaSelecionada);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "OK");
        }
        finally
        {
            lst_produtos.IsRefreshing = false;
        }
    }

    private async void Picker_Categorias_SelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        int selectedIndex = picker.SelectedIndex;

        if (selectedIndex != -1)
        {
            _categoriaSelecionada = picker.Items[selectedIndex];
            await CarregarProdutos(_categoriaSelecionada == "Todas" ? null : _categoriaSelecionada);
        }
    }

    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
        double soma = lista.Sum(i => i.Total);
        string msg = $"Total: {soma:C}";
        DisplayAlert("Total dos Produtos", msg, "OK");
    }

    private async void ToolbarItem_Clicked_2(object sender, EventArgs e)
    {
        try
        {
            var categorias = new List<string> { "Alimentos", "Bebidas", "Higiene", "Limpeza", "Utilitários", "Cozinha", "Padaria", "Outros" };
            var resumo = new Dictionary<string, double>();

            foreach (var categoria in categorias)
            {
                var produtos = await App.Db.GetByCategory(categoria);
                resumo[categoria] = produtos.Sum(p => p.Total);
            }

            var mensagem = "Gastos por Categoria:\n\n";
            foreach (var item in resumo.OrderByDescending(x => x.Value))
            {
                mensagem += $"{item.Key}: {item.Value:C}\n";
            }

            await DisplayAlert("Resumo por Categoria", mensagem, "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "OK");
        }
    }

    private async void MenuItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            MenuItem selecionado = sender as MenuItem;
            Produto p = selecionado.BindingContext as Produto;

            bool confirm = await DisplayAlert(
                "Confirmação", $"Remover {p.Descricao}?", "Sim", "Não");

            if (confirm)
            {
                await App.Db.Delete(p.Id);
                lista.Remove(p);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "OK");
        }
    }

    private void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        try
        {
            if (e.SelectedItem != null)
            {
                Produto p = e.SelectedItem as Produto;
                Navigation.PushAsync(new Views.EditarProduto
                {
                    BindingContext = p,
                });
                lst_produtos.SelectedItem = null;
            }
        }
        catch (Exception ex)
        {
            DisplayAlert("Erro", ex.Message, "OK");
        }
    }

    private async void lst_produtos_Refreshing(object sender, EventArgs e)
    {
        await CarregarProdutos(_categoriaSelecionada == "Todas" ? null : _categoriaSelecionada);
    }
}