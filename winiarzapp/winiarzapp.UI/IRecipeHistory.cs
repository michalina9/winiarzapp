using System;
using System.Collections.Generic;

namespace Winiarzapp.Core.Data
{
    /// <summary>
    /// Historia zapisanych nastawów.
    /// </summary>
    public interface IRecipeHistory : IDisposable
    {
        /// <summary>
        /// Zwróć historię nastawów.
        /// </summary>
        IEnumerable<Batch> Batches { get; set; }

        /// <summary>
        /// Dodaj nastaw do historii.
        /// </summary>
        void AddBatch(Batch batch);

        /// <summary>
        /// Usuń nastaw z historii.
        /// </summary>
        void RemoveBatch(Batch batch);
    }
}
