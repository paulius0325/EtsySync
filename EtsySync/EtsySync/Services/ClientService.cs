﻿using ClosedXML.Excel;
using SharedProject.Models;

namespace EtsySync.Services
{
    public class ClientService
    {
        public void WorksheetClient(IXLWorksheet worksheet, List<Client> clientsData)
        {
            //worksheet.PageSetup.PageOrientation = XLPageOrientation.Landscape;
            //worksheet.Column("C").Width = 20;
            //---------------------------Kliento informacija--------------------
            foreach (var client in clientsData)
            {
                var buyer = worksheet.Range("J9:K9");
                buyer.Merge();
                buyer.Value = "PIRKEJAS/BUYER:";
                buyer.Style.Font.FontSize = 10;
                buyer.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                buyer.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                var buyerName = worksheet.Range("L9:M9");
                buyerName.Merge();
                buyerName.Value = client.Buyer;
                buyerName.Style.Font.FontSize = 9;
                buyerName.Style.Font.Bold = true;

                var clientAddress = worksheet.Range("J10:K10");
                clientAddress.Merge();
                clientAddress.Value = "Adresas/Address:";
                clientAddress.Style.Font.FontSize = 10;
                clientAddress.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                clientAddress.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                var clientAddressValue = worksheet.Range("L10:M12");
                clientAddressValue.Merge();
                clientAddressValue.Value = client.Address;
                clientAddressValue.Style.Font.FontSize = 10;
                clientAddressValue.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                clientAddressValue.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                clientAddressValue.Style.Alignment.WrapText = true;
            }
        }
    }
}
