using System;
using winiarzapp.UI.Windows.History;
using winiarzapp.UI.Windows.RecipeCreator;
using winiarzapp.UI.Windows.VineCreator;
using winiarzapp.UI.Windows.Warning;
using Winiarzapp.Core.Data;

namespace Winiarzapp.UI.Services
{
    public class HelperService
    {
        private static IRecipeSource recipeSource;
        private static IRecipeHistory recipeHistory;

        /// <summary>
        /// Załaduj i wyświetl plik pomocy (*.chm).
        /// </summary>
        public static void Initialize(IRecipeSource recipeSource, IRecipeHistory recipeHistory)
        {
            HelperService.recipeSource = recipeSource;
            HelperService.recipeHistory = recipeHistory;
        }

        /// <summary>
        /// Załaduj i wyświetl plik pomocy (*.chm).
        /// </summary>
        public static void ShowHelp()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Otwórz okno "History" jako "dialog".
        /// </summary>
        public static void ShowHistory()
        {
            History h = new History();
            h.Initialize(recipeHistory);
            h.ShowDialog();
        }
        /// <summary>
        /// Wyśweitl kreator przepisów
        /// </summary>
        public static void ShowRecipeCreator() //metoda wyświetlająca kreator przepisów
        {
            RecipeCreator rc = new RecipeCreator();
            rc.Initialize(recipeSource);
            rc.ShowDialog();
        }
        /// <summary>
        /// wyświetl kreator win
        /// </summary>
        /// <param name="recipe"></param>
        public static void ShowVineCreator(Recipe recipe) //metoda wyświetlająca kreator wina
        {
            VineCreator vc = new VineCreator(recipe, recipeHistory);
            Warning wg = new Warning();
            //vc.Initialize(recipeSource);
            wg.ShowDialog();
            vc.ShowDialog();
        }
    }
}
