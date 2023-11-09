namespace BookShop
{
    using System.Diagnostics;
    using System.Text;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Models.Enums;

    public class StartUp
    {
        public static void Main()
        {           
            using var dbContext = new BookShopContext();
            //DbInitializer.ResetDatabase(dbContext);         
        }

        // problem 02
        public static string GetBooksByAgeRestriction(BookShopContext dbContext, string command)
        {
            try
            {
                AgeRestriction ageRestriction = Enum.Parse<AgeRestriction>(command, true);

                string[] bookTitles = dbContext.Books
                    .Where(b => b.AgeRestriction == ageRestriction)
                    .OrderBy(b => b.Title)
                    .Select(b => b.Title)
                    .ToArray();
                return string.Join(Environment.NewLine, bookTitles);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}


