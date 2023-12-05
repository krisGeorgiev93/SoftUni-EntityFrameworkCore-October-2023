namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using System.Xml;
    using Theatre.Common;
    using Theatre.Data;
    using Theatre.Data.Models;
    using Theatre.Data.Models.Enums;
    using Theatre.DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfulImportPlay
            = "Successfully imported {0} with genre {1} and a rating of {2}!";

        private const string SuccessfulImportActor
            = "Successfully imported actor {0} as a {1} character!";

        private const string SuccessfulImportTheatre
            = "Successfully imported theatre {0} with #{1} tickets!";



        public static string ImportPlays(TheatreContext context, string xmlString)
        {
            StringBuilder sb = new();

            ImportPlayDto[] playDtos = new XmlHelper()
                .Deserialize<ImportPlayDto[]>(xmlString, "Plays");

            ICollection<Play> validPlays = new HashSet<Play>();

            foreach (var playDto in playDtos)
            {
                if (!IsValid(playDto)
                    || !TimeSpan.TryParseExact(playDto.Duration, "c", CultureInfo.InvariantCulture, out var duration)
                    || duration.Hours < ValidationConstants.PlayDurationMinRange
                    || !Enum.TryParse<Genre>(playDto.Genre, out Genre genre))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Play play = new()
                {
                    Title = playDto.Title,
                    Duration = duration,
                    Rating = (float)playDto.Rating,
                    Genre = genre,
                    Description = playDto.Description,
                    Screenwriter = playDto.Screenwriter
                };

                validPlays.Add(play);
                sb.AppendLine(string.Format(SuccessfulImportPlay, play.Title, genre, play.Rating));
            }

            context.Plays.AddRange(validPlays);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportCasts(TheatreContext context, string xmlString)
        {
            XmlHelper xmlHelper = new XmlHelper();

            StringBuilder sb = new StringBuilder();

            var castDtos = xmlHelper.Deserialize<ImportCastDto[]>(xmlString, "Casts");

            ICollection<Cast> validCasts = new HashSet<Cast>();

            foreach (ImportCastDto castDto in castDtos)
            {
                if (!IsValid(castDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Cast cast = new Cast()
                {
                    FullName = castDto.FullName,
                    IsMainCharacter = castDto.IsMainCharacter,
                    PhoneNumber = castDto.PhoneNumber,
                    PlayId = castDto.PlayId
                };

                validCasts.Add(cast);
                string mainOrLesser;
                if (cast.IsMainCharacter)
                {
                    mainOrLesser = "main";
                }
                else
                {
                    mainOrLesser = "lesser";
                }

                sb.AppendLine(string.Format(SuccessfulImportActor, cast.FullName,mainOrLesser));
            }

            context.Casts.AddRange(validCasts);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportTtheatersTickets(TheatreContext context, string jsonString)
        {
           StringBuilder sb = new StringBuilder();

            var theatreDtos = JsonConvert.DeserializeObject<ImportTheatreDto[]>(jsonString);

            ICollection<Theatre> validTheatres = new HashSet<Theatre>();

            foreach (ImportTheatreDto theatreDto in theatreDtos)
            {
                if (!IsValid(theatreDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Theatre theatre = new Theatre()
                {
                    Name = theatreDto.Name,
                    NumberOfHalls = theatreDto.NumberOfHalls,
                    Director = theatreDto.Director,
                };

                foreach (ImportTicketDto ticketDto in theatreDto.Tickets)
                {
                    if (!IsValid(ticketDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Ticket ticket = new Ticket()
                    {
                        Price = ticketDto.Price,
                        RowNumber = ticketDto.RowNumber,
                        PlayId = ticketDto.PlayId,
                    };

                    theatre.Tickets.Add(ticket);
                }

                validTheatres.Add(theatre);
                sb.AppendLine(String.Format(SuccessfulImportTheatre, theatre.Name, theatre.Tickets.Count()));
            }

            context.Theatres.AddRange(validTheatres);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }


        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}
