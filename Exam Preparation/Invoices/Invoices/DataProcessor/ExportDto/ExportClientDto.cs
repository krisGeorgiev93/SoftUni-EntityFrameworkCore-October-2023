using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Invoices.DataProcessor.ExportDto
{
    [XmlType("Client")]
    public class ExportClientDto
    {
        [XmlElement("ClientName")]
        public string Name { get; set; } = null!;

        [XmlElement("VatNumber")]
        public string NumberVat { get; set; } = null!;

        [XmlAttribute("InvoicesCount")]
        public int InvoicesCount { get; set; }

        [XmlArray("Invoices")]
        public ExportClientInvoiceDto[] Invoices { get; set; } = null!;
    }

}