namespace Boardgames.DataProcessor
{
    using Boardgames.Data;
    using Boardgames.Data.Models;
    using Boardgames.DataProcessor.ExportDto;
    using Boardgames.Utilities;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;

    public class Serializer
    {
        public static string ExportCreatorsWithTheirBoardgames(BoardgamesContext context)
        {
            XmlHelper xmlHelper = new XmlHelper();

            ExportCreatorDto[] creatorsDto = context.Creators
                .Where(c => c.Boardgames.Any())
                .ToArray()
                .Select(c=> new ExportCreatorDto()
                {
                    CreatorName = c.FirstName + " " + c.LastName,
                    BoardgamesCount = c.Boardgames.Count(),
                    Boardgames = c.Boardgames
                    .Select(b=> new ExportBoardgameDto()
                    {
                        Name =  b.Name,
                        YearPublished = b.YearPublished
                    })
                    .OrderBy(b=> b.Name)
                    .ToArray()
                })
                .OrderByDescending(b => b.BoardgamesCount)
                .ThenBy(b => b.CreatorName)
                .ToArray();

            return xmlHelper.Serialize<ExportCreatorDto[]>(creatorsDto, "Creators");
        }

        public static string ExportSellersWithMostBoardgames(BoardgamesContext context, int year, double rating)
        {
            var sellers = context.Sellers
                 .Include(bs => bs.BoardgamesSellers)
                 .ThenInclude(b => b.Boardgame)
                 .Where(s => s.BoardgamesSellers.Any(s => s.Boardgame.YearPublished >= year
                 && s.Boardgame.Rating <= rating))
                 .ToArray()
                 .Select(s => new
                 {
                     Name = s.Name,
                     Website = s.Website,
                     Boardgames = s.BoardgamesSellers
                          .OrderByDescending(s => s.Boardgame.Rating)
                          .ThenBy(s => s.Boardgame.Name)
                          .Where(s => s.Boardgame.YearPublished >= year && s.Boardgame.Rating <= rating)
                          .Select(bs => new
                          {
                              bs.Boardgame.Name,
                              bs.Boardgame.Rating,
                              bs.Boardgame.Mechanics,
                              Category = bs.Boardgame.CategoryType.ToString()
                          })
                          .ToArray()
                 })
                 .OrderByDescending(s => s.Boardgames.Length)
                 .ThenBy(s => s.Name)
                 .Take(5)
                 .ToArray();

            return JsonConvert.SerializeObject(sellers, Formatting.Indented);
        }
    }
}