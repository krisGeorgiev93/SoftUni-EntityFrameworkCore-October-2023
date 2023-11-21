namespace Invoices.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using Invoices.Data;
    using Invoices.Data.Models;
    using Invoices.DataProcessor.ImportDto;
    using Invoices.Utilities;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedClients
            = "Successfully imported client {0}.";

        private const string SuccessfullyImportedInvoices
            = "Successfully imported invoice with number {0}.";

        private const string SuccessfullyImportedProducts
            = "Successfully imported product - {0} with {1} clients.";


        public static string ImportClients(InvoicesContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlParser xmlParser = new XmlParser();

            ImportClientDto[] clientsDto = xmlParser.Deserialize<ImportClientDto[]>(xmlString, "Clients");

            ICollection<Client> validClients = new HashSet<Client>();

            foreach (ImportClientDto clientDto in clientsDto)
            {
                if (!IsValid(clientDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                ICollection<Address> validAddresses = new HashSet<Address>();
                foreach (ImportAddressDto addressDto in clientDto.Addresses)
                {
                    if (!IsValid(addressDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Address address = new Address()
                    {
                        StreetName = addressDto.StreetName,
                        StreetNumber = addressDto.StreetNumber,
                        PostCode = addressDto.PostCode,
                        City = addressDto.City,
                        Country = addressDto.Country,
                    };

                    validAddresses.Add(address);
                }

                Client client = new Client()
                {
                    Name = clientDto.Name,
                    NumberVat = clientDto.NumberVat,
                    Addresses = validAddresses
                };

                validClients.Add(client);
                sb.AppendLine(String.Format(SuccessfullyImportedClients, client.Name));

            }
            context.Clients.AddRange(validClients);
            context.SaveChanges();
            return sb.ToString().TrimEnd();

        }


        public static string ImportInvoices(InvoicesContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ImportInvoiceDto[] invoicesDto = JsonConvert.DeserializeObject<ImportInvoiceDto[]>(jsonString);

            ICollection<Invoice> validInvoices = new HashSet<Invoice>();

            //validations
            foreach (ImportInvoiceDto invoiceDto in invoicesDto)
            {
                if (!IsValid(invoiceDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                //invalid issue or due date
                if (invoiceDto.DueDate == DateTime.ParseExact("01/01/0001", "dd/MM/yyyy", CultureInfo.InvariantCulture) ||
                    invoiceDto.IssueDate == DateTime.ParseExact("01/01/0001", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }                

                Invoice invoice = new Invoice()
                {
                    Number = invoiceDto.Number,
                    IssueDate = invoiceDto.IssueDate,
                    DueDate = invoiceDto.DueDate,
                    CurrencyType = invoiceDto.CurrencyType,
                    Amount = invoiceDto.Amount,
                    ClientId = invoiceDto.ClientId
                };

                if (invoiceDto.IssueDate > invoiceDto.DueDate)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                validInvoices.Add(invoice);
                sb.AppendLine(String.Format(SuccessfullyImportedInvoices, invoice.Number));
            }
            context.Invoices.AddRange(validInvoices);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportProducts(InvoicesContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ImportProductDto[] productsDto = JsonConvert.DeserializeObject<ImportProductDto[]>(jsonString);

            ICollection<Product> validProducts = new HashSet<Product>();

            foreach (ImportProductDto productDto in productsDto)
            {
                if (!IsValid(productDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Product product = new Product()
                {
                    Name = productDto.Name,
                    Price = productDto.Price,
                    CategoryType = productDto.CategoryType,

                };

                foreach (int clientId in productDto.Clients.Distinct()) //unique clients
                {
                    Client c = context.Clients.Find(clientId);
                    if (c == null)  //If a client does not exist in the database, append an error message
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    product.ProductsClients.Add(new ProductClient()
                    {
                        Client = c
                    });
                }
                validProducts.Add(product);
                sb.AppendLine(String.Format(SuccessfullyImportedProducts, product.Name, product.ProductsClients.Count));
            }
            context.Products.AddRange(validProducts);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }
        

        public static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
