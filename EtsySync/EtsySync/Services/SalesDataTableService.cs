using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using SharedProject.Models;


namespace EtsySync.Services
{
    public class SalesDataTableService
    {
        public void WorksheetsTable(IXLWorksheet worksheet, List<InvoiceItem> salesData)
        {

            //--------------------------------Lentele-------------------------------------
            string[] headers = { "El.\nNr.\nNo.", "Pavadinimas/Item", "Kiekis/Quantity", "Kaina/\nPrice(€)", "Suma/\nAmount(€)" };

            // Eiluciu ir stulpeliu skaiciavimas ir pritaikymas pagal leneteleje esancius duomenis
            int rowCount = salesData.Count();
            int columnCount = headers.Length;

            int startRow = 21; // Elutes pradzia 21
            int startColumn = 3; // Stulpelio pradzia 3 = C
            double totalAmount = 0;


            for (int col = 0; col < columnCount; col++)
            {
                if (col == 0) // El. Nr. No.(Prekes numeris, stulpelio pavadinimas)
                {
                    var headerCell = worksheet.Cell(20, startColumn);
                    headerCell.Value = headers[col];
                    headerCell.Style.Font.FontSize = 10;
                    headerCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    headerCell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    headerCell.Style.Alignment.WrapText = true;
                    headerCell.Style.Font.Bold = true;
                }
                else if (col == 1) // Pavadinimas/Item(Stulpelio pavadinimas)
                {
                    var tableName = worksheet.Range(20, 4, 20, 6);
                    tableName.Merge();
                    tableName.Value = headers[col];
                    tableName.Style.Font.FontSize = 10;
                    tableName.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    tableName.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    tableName.Style.Alignment.WrapText = true;
                    tableName.Style.Font.Bold = true;
                }
                else if (col == 2) // Kiekis/Quantity(Stulpelio pavadinimas)
                {
                    var tableQuantity = worksheet.Range(20, 7, 20, 8);
                    tableQuantity.Merge();
                    tableQuantity.Value = headers[col];
                    tableQuantity.Style.Font.FontSize = 10;
                    tableQuantity.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    tableQuantity.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    tableQuantity.Style.Alignment.WrapText = true;
                    tableQuantity.Style.Font.Bold = true;
                }
                else if (col == 3) // Kaina/Price(Stulpelio pavadinimas)
                {
                    var tablePrice = worksheet.Range(20, 9, 20, 10);
                    tablePrice.Merge();
                    tablePrice.Value = headers[col];
                    tablePrice.Style.Font.FontSize = 10;
                    tablePrice.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    tablePrice.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    tablePrice.Style.Alignment.WrapText = true;
                    tablePrice.Style.Font.Bold = true;
                }
                else if (col == 4) // Suma/Amount(Stulpelio pavadinimas)
                {
                    var tableAmount = worksheet.Range(20, 11, 20, 12);
                    tableAmount.Merge();
                    tableAmount.Value = headers[col];
                    tableAmount.Style.Font.FontSize = 10;
                    tableAmount.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    tableAmount.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    tableAmount.Style.Alignment.WrapText = true;
                    tableAmount.Style.Font.Bold = true;
                }
            }

            int itemNumber = 1;

            foreach (var item in salesData)
            {
                // Prekiu numeracija/Items No
                var tableNrValue = worksheet.Cell(startRow, 3);
                tableNrValue.Value = itemNumber;
                tableNrValue.Style.Font.FontSize = 10;
                tableNrValue.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                tableNrValue.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                tableNrValue.Style.Alignment.WrapText = true;
                tableNrValue.Style.Font.Bold = true;
                itemNumber++;

                // Prekes pavadinimas/Item Name
                var tableNameValue = worksheet.Range(startRow, 4, startRow, 6);
                tableNameValue.Merge();
                tableNameValue.Value = item.ItemName;
                tableNameValue.Style.Font.FontSize = 10;
                tableNameValue.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                tableNameValue.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                tableNameValue.Style.Alignment.WrapText = true;

                // Prekiu Kiekis/Quantity
                var tableQuantityValue = worksheet.Range(startRow, 7, startRow, 8);
                tableQuantityValue.Merge();
                tableQuantityValue.Value = item.Quantity;
                tableQuantityValue.Style.Font.FontSize = 10;
                tableQuantityValue.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                tableQuantityValue.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                tableQuantityValue.Style.Alignment.WrapText = true;

                // Prekes Kaina/Price
                var tablePriceValue = worksheet.Range(startRow, 9, startRow, 10);
                tablePriceValue.Merge();
                tablePriceValue.Value = item.Price;
                tablePriceValue.Style.Font.FontSize = 10;
                tablePriceValue.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                tablePriceValue.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                tablePriceValue.Style.Alignment.WrapText = true;
                tablePriceValue.Style.NumberFormat.Format = "#,##0.00";

                // Suma/Amount = kiekis * kaina
                var tableAmountValue = worksheet.Range(startRow, 11, startRow, 12);
                tableAmountValue.Merge();
                double amount = item.Quantity * item.Price;
                tableAmountValue.Value = amount;
                tableAmountValue.Style.Font.FontSize = 10;
                tableAmountValue.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                tableAmountValue.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                tableAmountValue.Style.Alignment.WrapText = true;
                tableAmountValue.Style.NumberFormat.Format = "#,##0.00";

                // Sudetis for total amount
                totalAmount += amount;

                startRow++;
            }

            // Is viso/Total
            var emptySpace = worksheet.Range(startRow, 3, startRow, 6);
            emptySpace.Merge();
            emptySpace.Value = "";
            emptySpace.Style.Border.OutsideBorder = XLBorderStyleValues.None;

            var tableTotal = worksheet.Range(startRow, 7, startRow, 10);
            tableTotal.Merge();
            tableTotal.Value = "Suma is viso/Total:";
            tableTotal.Style.Font.FontSize = 10;
            tableTotal.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            tableTotal.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            tableTotal.Style.Alignment.WrapText = true;
            tableTotal.Style.Border.TopBorder = XLBorderStyleValues.Dotted;
            tableTotal.Style.Border.LeftBorder = XLBorderStyleValues.Dotted;
            tableTotal.Style.Border.RightBorder = XLBorderStyleValues.Dotted;
            tableTotal.Style.Border.BottomBorder = XLBorderStyleValues.None;

            var tableTotalValue = worksheet.Range(startRow, 11, startRow, 12);
            tableTotalValue.Merge();
            tableTotalValue.Value = totalAmount;
            tableTotalValue.Style.Font.FontSize = 10;
            tableTotalValue.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            tableTotalValue.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            tableTotalValue.Style.Alignment.WrapText = true;
            tableTotalValue.Style.NumberFormat.Format = "#,##0.00";
            tableTotalValue.Style.Font.Bold = true;
            tableTotalValue.Style.Border.TopBorder = XLBorderStyleValues.Dotted;
            tableTotalValue.Style.Border.LeftBorder = XLBorderStyleValues.Dotted;
            tableTotalValue.Style.Border.RightBorder = XLBorderStyleValues.Dotted;
            tableTotalValue.Style.Border.BottomBorder = XLBorderStyleValues.None;

            var tableRange = worksheet.Range(20, 3, startRow - 1, 12);
            tableRange.Style.Border.InsideBorder = XLBorderStyleValues.Dotted;

            int seperateRow = 25;
            seperateRow = startRow + 2;

            //Apmoketi/Invoice total
            var invoiceTotal = worksheet.Range($"C{seperateRow}:D{seperateRow}");
            invoiceTotal.Merge();
            invoiceTotal.Value = "Apmoketi/Invoice total:";
            invoiceTotal.Style.Font.FontSize = 10;
            invoiceTotal.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            invoiceTotal.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            invoiceTotal.Style.Alignment.WrapText = true;

            var invoiceTotalValue = worksheet.Range($"E{seperateRow}:F{seperateRow}");
            invoiceTotalValue.Merge();
            invoiceTotalValue.Value = totalAmount;
            invoiceTotalValue.Style.Font.FontSize = 10;
            invoiceTotalValue.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            invoiceTotalValue.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            invoiceTotalValue.Style.Alignment.WrapText = true;
            invoiceTotalValue.Style.NumberFormat.Format = "€ #,##0.00";
            invoiceTotalValue.Style.Font.Bold = true;
        }
    }
}
