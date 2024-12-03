using CsvHelper;
using EtsySync.Interface;
using EtsySync.Mappers;
using SharedProject.Dtos;
using SharedProject.Models;
using System.Formats.Asn1;
using System.Globalization;

namespace EtsySync.Services
{
    public class CsvParserService : ICsvParserService
    {
        public List<SalesItem> ParseCsv(IFormFile file)
        {
            var salesItems = new List<SalesItem>();

            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {

                csv.Context.RegisterClassMap<SalesItemCsvMap>();

                try
                {

                    var records = csv.GetRecords<SalesItemDto>().ToList();


                    foreach (var record in records)
                    {
                        var sale = new SalesItem
                        {
                            SalesId = Guid.NewGuid(),
                            CreatedDate = record.CreatedDate,
                            SerialNumber = record.OrderId,
                            InvoiceItem = new InvoiceItem
                            {
                                ItemName = record.ItemName,
                                Quantity = record.Quantity,
                                Price = (double)record.Price
                            },
                            Client = new Client
                            {
                                Buyer = record.ClientDto?.Buyer,
                                Address = record.ClientDto?.Address
                            }
                        };

                        salesItems.Add(sale);
                    }
                }
                catch (CsvHelperException ex)
                {

                    throw new Exception("Error parsing CSV file", ex);
                }
            }

            return salesItems;
        }
    }
}
