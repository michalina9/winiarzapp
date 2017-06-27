using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Winiarzapp.Core.Data
{
    /// <summary>
    /// Model przepisu na wino. Zawiera listę składników oraz ich proporcje.
    /// </summary>
    [Serializable]
    public class Recipe : IComparable, IEqualityComparer<Recipe>, INotifyPropertyChanged
    {

        public Recipe() { }
        
        // Implementacja interfejsu INotifyPropertyChanged.
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        string name;
        public string Name
        {
            get { return this.name; }
            set
            {
                if (this.name != value)
                {
                    this.name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }
        string description;
        public string Description
        {
            get { return this.description; }
            set
            {
                if (this.description != value)
                {
                    this.description = value;
                    NotifyPropertyChanged("Description");
                }
            }
        }
        List<Ingredient> ingredients;
        public List<Ingredient> Ingredients
        {
            get { return this.ingredients; }
            set
            {
                if (this.ingredients != value)
                {
                    this.ingredients = value;
                    NotifyPropertyChanged("Ingredients");
                }
            }
        }
        public Guid UniqueID { get; set; }
        
        public Recipe(Ingredient[] ingredients, string name, string description)
        {
            UniqueID = Guid.NewGuid();
            this.Name = name;
            this.Description = description;

            if (!ValidateIngredients(ingredients)) throw new Exception("Proporcje składników nie sumują się do 1.0.");
            this.Ingredients = new List<Ingredient>(ingredients);
        }
        
        /// <summary>
        /// Implementacja interfejsu IComparable 
        /// </summary>
        public int CompareTo(object obj)
        {
            if (obj as Recipe != null)
            {
                var recipe = obj as Recipe;
                return String.Compare(this.Name, recipe.Name, true);
            }
            return -1;
        }
        
        /// <summary>
        /// Metoda sprawdzająca proporcje składników.
        /// </summary>
        public bool ValidateIngredients(Ingredient[] ingredients)
        {
            double sum = 0.0;
            foreach (var i in ingredients)
            {
                if (i.Unit != Unit.STATIC)
                    sum += i.Ratio;
            }
            if (sum == 1.0) return true;
            return false;
        }
        
        /// <summary>
        /// Metoda sprawdzająca równoważność dwóch obiektów przepisów.
        /// </summary>
        public bool Equals(Recipe x, Recipe y)
        {
            return x.UniqueID == y.UniqueID;
        }

        public int GetHashCode(Recipe obj)
        {
            return 0;
        }
        
        /// <summary>
        /// Metoda wyszukiwania poprzez frazę.
        /// </summary>
        public bool SearchByPhrase(string searchPhrase)
        {
            bool searchIngredients = false;

            var enumerator = this.Ingredients.GetEnumerator();
            do
            {
                var obj = enumerator.Current;
                searchIngredients |= obj.Name.Contains(searchPhrase) || obj.Description.Contains(searchPhrase);
            }
            while (enumerator.MoveNext());

            return this.Name.Contains(searchPhrase)
                || this.Description.Contains(searchPhrase)
                    || searchIngredients;
        }
        
        /// <summary>
        /// Metoda wyszukiwania poprzez wyrażenia regularne.
        /// </summary>
        public bool SearchByRegex(string regexPattern)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(regexPattern);

            bool searchIngredients = false;

            var enumerator = this.Ingredients.GetEnumerator();
            do
            {
                var obj = enumerator.Current;
                searchIngredients |= regex.IsMatch(obj.Name) || regex.IsMatch(obj.Description);
            }
            while (enumerator.MoveNext());

            return regex.IsMatch(this.Name) || regex.IsMatch(this.Description)
                    || searchIngredients;
        }
    }
}
