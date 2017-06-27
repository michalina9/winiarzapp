using System.Windows.Controls;
using Winiarzapp.Core.Data;

namespace winiarzapp.UI.Windows.MainWindow.Components
{
    /// <summary>
    /// Lista przepisów.
    /// </summary>
    public partial class RecipeList : UserControl
    {
        IRecipeSource recipeSource;
        StackPanel stackPanel;
        string query;

        public RecipeList()
        {
            InitializeComponent();
        }

        public void Initialize(IRecipeSource recipeSource)
        {
            this.recipeSource = recipeSource;
            stackPanel = FindName("StackPanel") as StackPanel;

            recipeSource.RecipesChanged += IRecipeSource_RecipesChanged;

            Filter("");
        }

        /// <summary>
        /// Metoda wywoływana kiedy zmienia się baza przepisów.
        /// </summary>
        private void IRecipeSource_RecipesChanged()
        {
            Filter(query);
        }

        /// <summary>
        /// Metoda wywoływana kiedy w SearchBarze zmieniony jest tekst.
        /// </summary>
        private void SearchBar_QueryChanged(string query)
        {
            this.query = query;

            Filter(query);
        }

        /// <summary>
        /// Metoda filtrująca przepisy z bazy.
        /// </summary>
        public void Filter(string query)
        {
            stackPanel.Children.Clear();

            foreach (Recipe recipe in recipeSource.Recipes)
            {
                if (string.IsNullOrWhiteSpace(query) || recipe.Name.ToLower().Contains(query.ToLower()))
                {
                    ListElement listElement = new ListElement();
                    listElement.RenderRecord(recipe);
                    stackPanel.Children.Add(listElement);
                }
            }
            stackPanel.Height = stackPanel.Children.Count * 65;
        }
    }
}
