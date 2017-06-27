using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Winiarzapp.Core.Data;

namespace winiarzapp.UI.Windows.VineCreator
{
    /// <summary>
    /// Logika interakcji dla klasy VineCreator.xaml
    /// </summary>
    public partial class VineCreator : Window
    {
        private IRecipeHistory recipeHistory;
        private Recipe recipe;
        double litersOfProduct = 1.0;
        double finalLitersOfProduct = 0;

        public VineCreator(Recipe recipe, IRecipeHistory recipeHistory)
        {
            this.recipe = recipe;
            this.recipeHistory = recipeHistory;

            InitializeComponent();

            // Binding nazwy i opisu w XAMLu
            this.DataContext = recipe;

            ingredientComboBox.Items.Add("Nastaw");

            foreach (var i in recipe.Ingredients.Where(i => i.Unit != Unit.STATIC))
                ingredientComboBox.Items.Add(i.Name);

            ingredientComboBox.SelectedIndex = 0;

            // Ustaw grid pod model reprezentujący ilość składników
            var grid = new GridView();
            ingredientsList.View = grid;

            grid.Columns.Add(new GridViewColumn
            {
                Header = "Nazwa",
                DisplayMemberBinding = new Binding("Name")
            });

            grid.Columns.Add(new GridViewColumn
            {
                Header = "Ilosc",
                DisplayMemberBinding = new Binding("Ammount")
            });

            comment.Visibility = Visibility.Hidden;
        }

        // Wybrano skladnik przepisu
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            updateIngredients();
        }


        private void Button_Click_Nastaw(object sender, RoutedEventArgs e) // przycisk zamykający okno kreatora win
        {
            if (addToHistory.IsChecked == true)
            {
                Batch b = new Batch();
                b.createdAt = DateTime.Now;
                b.Description = comment.Text;
                b.recipe = this.recipe;
                b.LitersOfProduct = finalLitersOfProduct;

                recipeHistory.AddBatch(b);
            }

            this.Close();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e) // chceckbox odpowiedzialny za komentarz
        {
            comment.Visibility = Visibility.Visible;
        }

        private void CheckBox_UnChecked(object sender, RoutedEventArgs e) // chceckbox odpowiedzialny za komentarz
        {
            comment.Visibility = Visibility.Hidden;
        }

        private void updateIngredients()
        {
            ingredientsList.Items.Clear();
            var idx = ingredientComboBox.SelectedIndex;
            double liters;

            if (idx == 0)
                liters = litersOfProduct;
            else
            {
                Ingredient found = recipe.Ingredients.Where(p => p.Name == (string)ingredientComboBox.SelectedValue).First();
                liters = litersOfProduct / found.Ratio;
            }

            foreach (var ingredient in recipe.Ingredients)
            {
                string name = ingredient.Name;
                string ammount = "";

                switch (ingredient.Unit)
                {
                    case Unit.STATIC:
                        ammount = ingredient.Ratio * 100 + " szt.";
                        break;
                    case Unit.LITER:
                        ammount = String.Format("{0:N2}", liters * ingredient.Ratio) + " L";
                        break;
                    case Unit.KILOGRAM:
                        ammount = String.Format("{0:N2}", liters * ingredient.Ratio) + " Kg";
                        break;
                }

                ingredientsList.Items.Add(new { Name = name, Ammount = ammount });
            }

            finalLitersOfProduct = liters;
            finalDisplay.Content = $"Otrzymasz {String.Format("{0:N2}", liters)} L nastawu.";
        }

        private void ingredientsList_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                try
                {
                    litersOfProduct = double.Parse(inputAmmount.Text);
                    updateIngredients();
                }
                catch (FormatException)
                {
                    MessageBox.Show("Niepoprawny format liczby - podaj ułamek dziesiętny lub liczbę całkowitą");
                    inputAmmount.Text = litersOfProduct + "";
                }
            }
        }

        public void TextBox_GotFocus(object sender, RoutedEventArgs e) //metoda czyszcząca textboxa po kliknięciu na niego
        {
            TextBox tb = (TextBox)sender;
            tb.Text = string.Empty;
            tb.GotFocus -= TextBox_GotFocus;
        }
    }
}
