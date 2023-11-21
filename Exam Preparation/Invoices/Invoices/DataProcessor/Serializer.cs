namespace Invoices.DataProcessor
{
    using AutoMapper.QueryableExtensions;
    using Invoices.Data;
    using Invoices.Data.Models;
    using Invoices.DataProcessor.ExportDto;
    using Invoices.Utilities;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using System.Globalization;
    using System.Xml.Linq;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportClientsWithTheirInvoices(InvoicesContext context, DateTime date)
        {
            XmlParser xmlParser = new XmlParser();

            ExportClientDto[] clientsDtos = context
               .Clients
               .Where(c => c.Invoices.Any(ci => ci.IssueDate > date))
               .ToArray()
               .Select(c => new ExportClientDto()
               {
                   NumberVat = c.NumberVat,
                   Name = c.Name,
                   InvoicesCount = c.Invoices.Count,
                   Invoices = c.Invoices
                     .OrderBy(i => i.IssueDate)
                     .ThenByDescending(i => i.DueDate)
                     .ToArray()
                   .Select(i => new ExportClientInvoiceDto()
                   {
                       Number = i.Number,
                       Amount = i.Amount,
                       DueDate = i.DueDate.ToString("d", CultureInfo.InvariantCulture),
                       Currency = i.CurrencyType.ToString(),
                   })
                   .ToArray()
               })
               .OrderByDescending(c => c.InvoicesCount)
               .ThenBy(c => c.Name)
               .ToArray();
           

            return xmlParser.Serialize(clientsDtos, "Clients");

        }

        public static string ExportProductsWithMostClients(InvoicesContext context, int nameLength)
        {

            var products = context
               .Products
               .Where(p => p.ProductsClients.Any(pc => pc.Client.Name.Length >= nameLength))
               .ToArray()
               .Select(p => new
               {
                   //select the product first
                   p.Name,
                   p.Price,
                   Category = p.CategoryType.ToString(),
                   Clients = p.ProductsClients
                       .Where(pc => pc.Client.Name.Length >= nameLength)
                       .Select(pc => new
                       {
                           Name = pc.Client.Name,
                           NumberVat = pc.Client.NumberVat,
                       })
                       .OrderBy(c => c.Name) //Order the clients by name (ascending). 
                       .ToArray()
               })
               //Order the products by all clients (meeting above condition) count (descending),
               //then by name (ascending).
               .OrderByDescending(p => p.Clients.Length)
               .ThenBy(p => p.Name)
               .Take(5)
               .ToArray();

            return JsonConvert.SerializeObject(products, Formatting.Indented);
        }
    }
}
