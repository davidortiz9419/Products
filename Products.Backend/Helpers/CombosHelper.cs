namespace Products.Backend.Helpers
{
    using Models;
    using Domain;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CombosHelper : IDisposable
    {
        private static DataContextLocal db = new DataContextLocal();
        public static List<Category> GetCategories()
        {
            var categories = db.Categories.ToList();
            categories.Add(new Category
            {
                CategoryId = 0,
                Description = "[Select a category...]",
            });

            return categories.OrderBy(d => d.Description).ToList();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
