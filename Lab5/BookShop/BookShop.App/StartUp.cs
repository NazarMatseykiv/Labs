namespace BookShop
{
    using System;
    using System.Linq;
    using System.Text;
    using Data;
    using Models;

    public class StartUp
    {
        public static void Main()
        {
            BookShopContext dbContext = new BookShopContext();

            using (dbContext)
            {
                // 1.
                string input = Console.ReadLine();
                string result1 = GetBooksByAgeRestriction(dbContext, input);
                Console.WriteLine(result1);
                // 2.
                string reslut2 = GetGoldenBooks(dbContext);
                Console.WriteLine(reslut2);
                // 3.
                string result3 = GetBooksByPrice(dbContext);
                Console.WriteLine(result3);
                // 4.
                int year = 2001;
                string result4 = GetBooksNotRealeasedIn(dbContext, year);
                Console.WriteLine(result4);
                // 5.
                string input5 = Console.ReadLine();
                string result5 = GetBooksByCategory(dbContext, input5);
                Console.WriteLine(result5);
                // 6.
                string input6 = Console.ReadLine();
                string result6 = GetBooksReleasedBefore(dbContext, input6);
                Console.WriteLine(result6);
                // 7.
                string input7 = Console.ReadLine();
                string result7 = GetAuthorNamesEndingIn(dbContext, input7);
                Console.WriteLine(result7);
                // 8.
                string input8 = Console.ReadLine();
                string result8 = GetBookTitlesContaining(dbContext, input8);
                Console.WriteLine(result8);
                // 9.
                string input9 = Console.ReadLine();
                string result9 = GetBooksByAuthor(dbContext, input9);
                Console.WriteLine(result9);
                // 10.
                int count = 10;
                int result10 = CountBooks(dbContext, count);
                Console.WriteLine(result10);
                // 11.
                string result11 = CountCopiesByAuthor(dbContext);
                Console.WriteLine(result11);
                // 12.
                string result12 = GetTotalProfitByCategory(dbContext);
                Console.WriteLine(result12);
                // 13.
                string result13 = GetMostRecentBooks(dbContext);
                Console.WriteLine(result13);
                // 14.
                IncreasePrices(dbContext);
                // 15.
                int result = RemoveBooks(dbContext);
                Console.WriteLine(result);
            }
        }
        // 1. Age Restriction
        public static string GetBooksByAgeRestriction(BookShopContext dbContext, string input)
        {
            AgeRestriction ageRestriction = Enum.Parse<AgeRestriction>(input, true);

            string[] books = dbContext
                .Books
                .Where(b => b.AgeRestriction == ageRestriction)
                .Select(b => b.Title)
                .OrderBy(b => b)
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }
        // 2. Golden Books
        public static string GetGoldenBooks(BookShopContext dbContext)
        {
            string[] books = dbContext
                .Books
                .Where(b => b.EditionType == EditionType.Gold && b.Copies < 5000)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }
        // 3. Books by Price
        public static string GetBooksByPrice(BookShopContext dbContext)
        {
            string[] books = dbContext
                .Books
                .Where(b => b.Price > 40)
                .OrderByDescending(b => b.Price)
                .Select(b => $"{b.Title} - ${b.Price:F2}")
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }
        // 4. Not Released In
        public static string GetBooksNotRealeasedIn(BookShopContext dbContext, int year)
        {
            string[] books = dbContext
                .Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }
        // 5. Book Titles be Category
        public static string GetBooksByCategory(BookShopContext dbContext, string input)
        {
            string[] categories = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(c => c.ToLower()).ToArray();

            string[] books = dbContext
                .Books
                .Where(b => b.BookCategories.Select(bc => bc.Category.Name.ToLower()).Intersect(categories).Any())
                .Select(b => b.Title)
                .OrderBy(b => b)
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }
        // 6. Realesed Before Date
        public static string GetBooksReleasedBefore(BookShopContext dbContext, string input)
        {
            DateTime date = DateTime.ParseExact(input, "dd-MM-yyyy", null);

            string[] books = dbContext
                .Books
                .Where(b => b.ReleaseDate < date)
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => $"{b.Title} - {b.EditionType} - ${b.Price:F2}")
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }
        // 7. Author Search
        public static string GetAuthorNamesEndingIn(BookShopContext dbContext, string input)
        {
            string[] authors = dbContext
                .Authors
                .Where(a => a.FirstName != null && a.FirstName.EndsWith(input))
                .Select(a => $"{a.FirstName} {a.LastName}")
                .OrderBy(a => a)
                .ToArray();

            return string.Join(Environment.NewLine, authors);
        }
        // 8. Book Search
        public static string GetBookTitlesContaining(BookShopContext dbContext, string input)
        {
            string[] books = dbContext
                .Books
                .Where(b => b.Title.ToLower().Contains(input.ToLower()))
                .Select(b => b.Title)
                .OrderBy(b => b)
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }
        // 9. Book Search by Author
        public static string GetBooksByAuthor(BookShopContext dbContext, string input)
        {
            string[] books = dbContext
                .Books
                .Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .OrderBy(b => b.BookId)
                .Select(b => $"{b.Title} ({b.Author.FirstName} {b.Author.LastName})")
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }
        // 10. Count Books
        public static int CountBooks(BookShopContext dbContext, int lengthCheck)
        {
            int countBooks = dbContext
                .Books
                .Count(b => b.Title.Length > lengthCheck);

            return countBooks;
        }
        // 11. Total Book Copies
        public static string CountCopiesByAuthor(BookShopContext dbContext)
        {
            var authors = dbContext
                .Authors
                .Select(a => new
                {
                    FullName = $"{a.FirstName} {a.LastName}",
                    Copies = a.Books.Sum(b => b.Copies)
                })
                .OrderByDescending(a => a.Copies)
                .ToArray();

            string result = string.Join(Environment.NewLine, authors.Select(a => $"{a.FullName} - {a.Copies}"));
            return result;
        }
        // 12. Profit by Category
        public static string GetTotalProfitByCategory(BookShopContext dbContext)
        {
            var categories = dbContext
                .Categories
                .Select(c => new
                {
                    CategoryName = c.Name,
                    Profit = c.CategoryBooks.Sum(bc => bc.Book.Copies * bc.Book.Price)
                })
                .OrderByDescending(c => c.Profit)
                .ThenBy(c => c.CategoryName)
                .ToArray();

            string result = string.Join(Environment.NewLine, categories.Select(c => $"{c.CategoryName} ${c.Profit:F2}"));
            return result;
        }
        // 13. Most Recent Books
        public static string GetMostRecentBooks(BookShopContext dbContext)
        {
            var categories = dbContext
                .Categories
                .Select(c => new
                {
                    CategoryName = c.Name,
                    Books = c.CategoryBooks
                        .Select(bc => new
                        {
                            Title = bc.Book.Title,
                            ReleaseDate = bc.Book.ReleaseDate
                        })
                        .OrderByDescending(bc => bc.ReleaseDate)
                        .Take(3)
                })
                .OrderBy(c => c.CategoryName)
                .ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var category in categories)
            {
                sb.AppendLine($"--{category.CategoryName}");
                foreach (var book in category.Books)
                {
                    sb.AppendLine($"{book.Title} ({book.ReleaseDate.Value.Year})");
                }
            }

            return sb.ToString().Trim();
        }
        // 14. Increase Prices
        public static void IncreasePrices(BookShopContext dbContext)
        {
            Book[] books = dbContext
                .Books
                .Where(b => b.ReleaseDate.Value.Year < 2010)
                .ToArray();

            foreach (Book book in books)
            {
                book.Price += 5;
            }

            dbContext.SaveChanges();
        }
        // 15. Remove Books
        public static int RemoveBooks(BookShopContext dbContext)
        {
            Book[] books = dbContext
                .Books
                .Where(b => b.Copies < 4200)
                .ToArray();

            dbContext.Books.RemoveRange(books);
            dbContext.SaveChanges();

            return books.Length;
        }
    }
}