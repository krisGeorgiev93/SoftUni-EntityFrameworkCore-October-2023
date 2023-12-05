namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System;
    using Theatre.Data;
    using Theatre.DataProcessor.ExportDto;

    public class Serializer
    {
        public static string ExportTheatres(TheatreContext context, int numbersOfHalls)
        {
            var theatres = context.Theatres
                 .Where(t => t.NumberOfHalls >= numbersOfHalls && t.Tickets.Count >= 20)
                 .ToArray()
                 .Select(t => new
                 {
                     Name = t.Name,
                     Halls = t.NumberOfHalls,
                     TotalIncome = t.Tickets.Where(t=> t.RowNumber>= 1 && t.RowNumber <= 5).Sum(t => t.Price),
                     Tickets = t.Tickets
                     .Where(t => t.RowNumber >= 1 && t.RowNumber <= 5)
                     .Select(ticket => new
                     {
                         Price = ticket.Price,
                         RowNumber = ticket.RowNumber,
                     })
                     .OrderByDescending(t => t.Price)
                     .ToArray()
                 })
                 .OrderByDescending(t => t.Halls)
                 .ThenBy(t => t.Name)
                 .ToArray();

            return JsonConvert.SerializeObject(theatres, Formatting.Indented);
                 

        }

        public static string ExportPlays(TheatreContext context, double raiting)
        {
            XmlHelper xmlHelper = new XmlHelper();

            var plays = context.Plays
                .Where(p => p.Rating <= raiting)
                .ToArray()
                .Select(p => new ExportPlayDto()
                {
                    Title = p.Title,
                    Duration = p.Duration.ToString("c"),
                    Rating = p.Rating == 0 ? "Premier" : p.Rating.ToString(),
                    Genre = p.Genre.ToString(),
                    Actors = p.Casts
                    .Where(c => c.IsMainCharacter)
                   .Select(a => new ExportActorDto()
                   {
                       FullName = a.FullName,
                       IsMainCharacter = $"Plays main character in '{p.Title}'."
                   })
                   .OrderByDescending(a => a.FullName)
                .ToArray()
                })
                .OrderBy(p => p.Title)
                .ThenByDescending(p => p.Genre)
                .ToArray();


            return xmlHelper.Serialize<ExportPlayDto[]>(plays, "Plays");
        }
    }
}