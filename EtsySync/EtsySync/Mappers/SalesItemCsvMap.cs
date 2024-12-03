using CsvHelper.Configuration;
using SharedProject.Dtos;

namespace EtsySync.Mappers
{
    public class SalesItemCsvMap : ClassMap<SalesItemDto>
    {
        public SalesItemCsvMap()
        {
            Map(m => m.OrderId).Name("Order ID");
            Map(m => m.CreatedDate).Name("Sale Date").TypeConverterOption.Format("MM/dd/yy");
            Map(m => m.ItemName).Name("SKU");
            Map(m => m.Quantity).Name("Number of Items");
            Map(m => m.Price).Name("Order Value");

            Map(m => m.ClientDto).Convert(args =>
            {
                var street1 = args.Row.GetField("Street 1");
                var street2 = args.Row.GetField("Street 2");
                var city = args.Row.GetField("Delivery City");
                var state = args.Row.GetField("Delivery State");
                var zip = args.Row.GetField("Delivery Zipcode");
                var country = args.Row.GetField("Delivery Country");

                return new ClientDto
                {
                    Buyer = args.Row.GetField("Buyer User ID"),
                    Address = $"{street1}, {street2}, {city}, {state}, {zip}, {country}".Trim(',', ' ') // Formuojamas adreso turinys
                };
            });
        }
    }
}
