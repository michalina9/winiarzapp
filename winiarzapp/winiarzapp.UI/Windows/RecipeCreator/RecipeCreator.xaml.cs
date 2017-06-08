﻿using System.Collections.Generic;
using System.Windows;
using Winiarzapp.Core.Data;

namespace winiarzapp.UI.Windows.RecipeCreator
{
    /// <summary>
    /// Interaction logic for RecipeCreator.xaml
    /// </summary>
    public partial class RecipeCreator : Window
    {
        private List<IngredientModel> ingredients = new List<IngredientModel>();
        private IRecipeSource recipeSource;

        public RecipeCreator()
        {
            InitializeComponent();

            dataGrid.AutoGeneratedColumns += DataGrid_AutoGeneratedColumns;

            dataGrid.ItemsSource = ingredients;
        }

        /// <summary>
        /// Nadpisz domyślne nazwy kolumn.
        /// </summary>
        private void DataGrid_AutoGeneratedColumns(object sender, System.EventArgs e)
        {
            dataGrid.Columns[0].Header = "Nazwa";
            dataGrid.Columns[1].Header = "Opis";
            dataGrid.Columns[2].Header = "Udział (%/jednostka)";
            dataGrid.Columns[3].Header = "Jednostka";
        }

        /// <summary>
        /// Metoda służąca do inicjalizacji komponentu. 
        /// </summary>
        public void Initialize(IRecipeSource recipeSource)
        {
            this.recipeSource = recipeSource;
        }

        /// <summary>
        /// Wywoływane po wciśnięciu przycisku "Utwórz"
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            /* Walidacja nazwy */
            var name = recipeNameTextBox.Text;
            if (string.IsNullOrEmpty(name))
            {
                ValidationAlert("Nazwa jest wymagana!");
                return;
            }

            /* Walidacja opisu */
            var description = recipeDescriptionTextBox.Text;
            if (string.IsNullOrEmpty(description))
            {
                ValidationAlert("Opis jest wymagany!");
                return;
            }

            /* Walidacja składników. */
            double sum = 0;
            foreach (var ingredient in ingredients)
            {
                if (ingredient.Unit != Unit.STATIC)
                    sum += ingredient.Percentage / 100.00;
            }

            if (sum != 1.0)
            {
                ValidationAlert("Udziały procentowe powinny sumować się do 100. Pamiętaj, że składniki statyczne (STATIC) oznaczają jednostkę charakterystyczą dla składnika i nie biorą udziału w równaniu!");
                return;
            }

            /* Tworzenie przepisu */
            List<Ingredient> list = new List<Ingredient>(); // O NIE ILE PAMIĘCI I PROCESORA ZMARNOWANE, CO MY MOGLIBYŚMY ZROBIĆ Z Tą DODATKOWĄ MOCĄ OBLICZENIOWĄ GDYBYM TYLKO UŻYŁ TABLICY :O 
            foreach (var i in ingredients) // A TEN FOREACH, STRASZNIE NIEWYDAJNE
            {
                list.Add(new Ingredient(i.Name, i.Description, i.Percentage / 100.00, i.Unit));
            }

            var recipe = new Recipe(list.ToArray(), name, description);

            recipeSource.AddRecipe(recipe);
            this.Close();
            SuccessMessage();
        }

        /// <summary>
        /// Wyświetl okno z błędem walidacji.
        /// </summary>
        private void ValidationAlert(string msg)
        {
            MessageBox.Show(msg, "Popraw dane!", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        /// <summary>
        /// Wyświetl okno oznajmiające pomyślne dodanie przepisu.
        /// </summary>
        private void SuccessMessage()
        {
            MessageBox.Show("Przepis dodany!", "Sukces!", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    /// <summary>
    /// Model wykorzystywany przez DataGrid.
    /// </summary>
    class IngredientModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Percentage { get; set; }
        public Unit Unit { get; set; }
    }
}