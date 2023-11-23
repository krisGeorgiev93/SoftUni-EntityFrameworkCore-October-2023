namespace Boardgames.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using Boardgames.Data;
    using Boardgames.Data.Models;
    using Boardgames.DataProcessor.ImportDto;
    using Boardgames.Utilities;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCreator
            = "Successfully imported creator – {0} {1} with {2} boardgames.";

        private const string SuccessfullyImportedSeller
            = "Successfully imported seller - {0} with {1} boardgames.";

        public static string ImportCreators(BoardgamesContext context, string xmlString)
        {
            StringBuilder stringBuilder = new StringBuilder();

            XmlHelper xmlHelper = new XmlHelper();

            ImportCreatorDto[] creators = xmlHelper.Deserialize<ImportCreatorDto[]>(xmlString, "Creators");

            ICollection<Creator> validCreators = new HashSet<Creator>();

            foreach (ImportCreatorDto creatorDto in creators)
            {
                if (!IsValid(creatorDto))
                {
                    stringBuilder.AppendLine(ErrorMessage);
                    continue;
                }

                ICollection<Boardgame> validBoardgames = new HashSet<Boardgame>();
                foreach (var boardgameDto in creatorDto.Boardgames)
                {
                    if (!IsValid(boardgameDto))
                    {
                        stringBuilder.AppendLine(ErrorMessage);
                        continue;
                    }

                    Boardgame boardgame = new Boardgame()
                    {
                        Name = boardgameDto.Name,
                        Rating = boardgameDto.Rating,
                        YearPublished = boardgameDto.YearPublished,
                        CategoryType = (Data.Models.Enums.CategoryType)boardgameDto.CategoryType,
                        Mechanics = boardgameDto.Mechanics,
                    };
                    validBoardgames.Add(boardgame);
                }

                Creator creator = new Creator()
                {
                    FirstName = creatorDto.FirstName,
                    LastName = creatorDto.LastName,
                    Boardgames = validBoardgames
                };

                validCreators.Add(creator);
                stringBuilder.AppendLine(string.Format(SuccessfullyImportedCreator, creator.FirstName, creator.LastName, creator.Boardgames.Count()));
            }

            context.Creators.AddRange(validCreators);
            context.SaveChanges();

            return stringBuilder.ToString().TrimEnd();

        }

        public static string ImportSellers(BoardgamesContext context, string jsonString)
        {
            StringBuilder stringBuilder = new StringBuilder();

            ImportSellerDto[] sellers = JsonConvert.DeserializeObject<ImportSellerDto[]>(jsonString);

            ICollection<Seller> validSellers = new HashSet<Seller>();

            ICollection<int> existingBoardgames = context.Boardgames
                .Select(b => b.Id)
                .ToArray();

            foreach (ImportSellerDto sellerDto in sellers)
            {
                if (!IsValid(sellerDto))
                {
                    stringBuilder.AppendLine(ErrorMessage);
                    continue;
                }

                Seller seller = new Seller()
                {
                    Name = sellerDto.Name,
                    Address = sellerDto.Address,
                    Country = sellerDto.Country,
                    Website = sellerDto.Website,
                };

                foreach (var boardgameId in sellerDto.Boardgames.Distinct())
                {
                    if (!existingBoardgames.Contains(boardgameId))
                    {
                        stringBuilder.AppendLine(ErrorMessage);
                        continue;
                    }
                    BoardgameSeller boardgameSeller = new BoardgameSeller()
                    {
                        Seller = seller,
                        BoardgameId = boardgameId,
                    };

                    seller.BoardgamesSellers.Add(boardgameSeller);
                }

                validSellers.Add(seller);
                stringBuilder.AppendLine(String.Format(SuccessfullyImportedSeller, seller.Name, seller.BoardgamesSellers.Count));
            }
            context.Sellers.AddRange(validSellers);
            context.SaveChanges();

            return stringBuilder.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
