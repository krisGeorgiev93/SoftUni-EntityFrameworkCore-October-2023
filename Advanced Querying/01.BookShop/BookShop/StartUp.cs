namespace BookShop
{
    using BookShop.Models;
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualBasic;
    using System.Formats.Asn1;
    using System.Globalization;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            //DbInitializer.ResetDatabase(db);

            //var result = RemoveBooks(db);
            //Console.WriteLine(result);
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

        //problem03
        public static string GetGoldenBooks(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var goldenEdition = Enum.Parse<EditionType>("Gold", true);

            var books = context.Books
                .Where(b => b.EditionType == goldenEdition && b.Copies < 5000)
                .OrderBy(b => b.BookId)
                .ToArray();

            foreach (var bookTitle in books)
            {
                sb.AppendLine(bookTitle.Title);
            }

            return sb.ToString().TrimEnd();
        }

        //problem 04
        public static string GetBooksByPrice(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var booksByPrice = context.Books
                .Where(p => p.Price > 40)
                .Select(b => new
                {
                    b.Title,
                    b.Price
                })
                .OrderByDescending(p => p.Price)
                .ToArray();

            foreach (var book in booksByPrice)
            {
                sb.AppendLine($"{book.Title} - ${book.Price:f2}");
            }
            return sb.ToString().TrimEnd();
        }

        //problem 05
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {

            StringBuilder stringBuilder = new StringBuilder();

            var booksNotReleasedInThisYear = context.Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .OrderBy(b => b.BookId)
                .Select(b => new
                {
                    b.Title
                });

            foreach (var book in booksNotReleasedInThisYear)
            {
                stringBuilder.AppendLine(book.Title);
            }

            return stringBuilder.ToString().TrimEnd();

        }

        //problem 06
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();

            string[] categories = input
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(c => c.ToLower())
                .ToArray();

            var booksByCategory = context.Books
                .Where(b => b.BookCategories.Any(bc => categories.Contains(bc.Category.Name.ToLower())))
                .OrderBy(b => b.Title);

            foreach (var book in booksByCategory)
            {
                sb.AppendLine(book.Title);
            }
            return sb.ToString().TrimEnd();

        }

        //problem 07
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            DateTime convertedDate = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);


            StringBuilder sb = new StringBuilder();

            var booksReleasedBeforeTheGivenDate = context.Books
                .Where(b => b.ReleaseDate < convertedDate)
                .Select(b => new
                {
                    b.ReleaseDate,
                    b.Title,
                    b.EditionType,
                    b.Price,
                })
                .OrderByDescending(b => b.ReleaseDate)
                .ToArray();

            foreach (var book in booksReleasedBeforeTheGivenDate)
            {
                sb.AppendLine($"{book.Title} - {book.EditionType} - ${book.Price:f2}");
            }
            return sb.ToString().TrimEnd();

        }

        //problem08
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            StringBuilder stringBuilder = new StringBuilder();

            var authors = context.Authors
                .Where(a => a.FirstName.EndsWith(input))
                .Select(a => new
                {
                    a.FirstName,
                    a.LastName,
                })
                .OrderBy(a => a.FirstName)
                .ThenBy(a => a.LastName)
                .ToArray();

            foreach (var author in authors)
            {
                stringBuilder.AppendLine($"{author.FirstName} {author.LastName}");
            }

            return stringBuilder.ToString().TrimEnd();

        }

        //problem09
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();

            var books = context.Books
                .Where(b => b.Title.ToLower().Contains(input.ToLower()))
                .OrderBy(b => b.Title)
                .ToArray();

            foreach (var b in books)
            {
                sb.AppendLine(b.Title);
            }

            return sb.ToString().TrimEnd();

        }

        //problem10
        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();
            var books = context.Books
                .Include(a => a.Author)
                .Where(a => a.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .Select(b => new
                {
                    b.BookId,
                    b.Title,
                    b.Author.FirstName,
                    b.Author.LastName,
                })
                .OrderBy(b => b.BookId)
                .ToArray();

            foreach (var b in books)
            {
                sb.AppendLine($"{b.Title} ({b.FirstName} {b.LastName})");
            }

            return sb.ToString().TrimEnd();

        }

        //problem 11
        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var books = context.Books
                .Where(b => b.Title.Length > lengthCheck)
                .Select(b => b.Title)
                .ToArray();

            int count = books.Count();

            return count;

        }

        //problem 12
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var authorsCopies = context.Authors
                .Include(b => b.Books)
                .Select(a => new
                {
                    a.FirstName,
                    a.LastName,
                    BookCopies = a.Books.Select(c => c.Copies).Sum()
                })
                .OrderByDescending(b => b.BookCopies)
                .ToArray();

            foreach (var ac in authorsCopies)
            {
                sb.AppendLine($"{ac.FirstName} {ac.LastName} - {ac.BookCopies}");
            }

            return sb.ToString().TrimEnd();

        }

        //problem 13
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var profitByCategory = context.Categories
                .Select(c => new
                {
                    CategoryName = c.Name,
                    TotalProfit = c.CategoryBooks.Sum(c => c.Book.Copies * c.Book.Price)
                })
                .OrderByDescending(t => t.TotalProfit)
                .ThenBy(c => c.CategoryName)
                .ToArray();

            foreach (var cat in profitByCategory)
            {
                sb.AppendLine($"{cat.CategoryName} ${cat.TotalProfit:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        //problem 14
        public static string GetMostRecentBooks(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var categoriesWithMostRecentBooks = context.Categories
                .OrderBy(c => c.Name)
                .Select(c => new
                {
                    CategoryName = c.Name,
                    MostRecentBooks = c.CategoryBooks
                        .OrderByDescending(cb => cb.Book.ReleaseDate)
                        .Take(3) // This can lower network load
                        .Select(cb => new
                        {
                            BookTitle = cb.Book.Title,
                            ReleaseYear = cb.Book.ReleaseDate.Value.Year
                        })
                        .ToArray()
                })
                .ToArray();

            foreach (var c in categoriesWithMostRecentBooks)
            {
                sb.AppendLine($"--{c.CategoryName}");

                foreach (var b in c.MostRecentBooks/*.Take(3) This is lowering query complexity*/)
                {
                    sb.AppendLine($"{b.BookTitle} ({b.ReleaseYear})");
                }
            }

            return sb.ToString().TrimEnd();
        }

        //problem 15
        public static void IncreasePrices(BookShopContext context)
        {
            var targetBooks = context.Books
                .Where(b => b.ReleaseDate.Value.Year < 2010)
                .ToArray();

            foreach (var book in targetBooks)
            {
                book.Price += 5;
            }
        }

        //problem 16
        public static int RemoveBooks(BookShopContext context)
        {

            var targetBooks = context.Books
                .Where(b => b.Copies < 4200)
                .ToArray();

            int removedBooks = targetBooks.Count();
            context.RemoveRange(targetBooks);

            context.SaveChanges();

            return removedBooks;



        }

    }
}


