using ClosedXML.Excel;

namespace EtsySync.Services
{
    public class SellerService
    {
        public void WorksheetSeller(IXLWorksheet worksheet)
        {
            
            //-----------------Pardavejo infomacija------------------
            var seller = worksheet.Range("C9:D9");
            seller.Merge();

            seller.Value = "PARDAVEJAS / SELLER:";
            seller.Style.Font.FontSize = 9;

            var sellerValue = worksheet.Range("E9:F9");
            sellerValue.Merge();
            sellerValue.Value = "Simona Rokaite";
            sellerValue.Style.Font.FontSize = 9;
            sellerValue.Style.Font.Bold = true;

            var certificate = worksheet.Range("C10:D12");
            certificate.Merge();
            certificate.Value = "Individualios veiklos\npazymejimas Nr. /\nBusiness certificate No. :";
            certificate.Style.Font.FontSize = 10;
            certificate.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            certificate.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            certificate.Style.Alignment.WrapText = true;


            var certificateNo = worksheet.Range("E10:F12");
            certificateNo.Merge();

            certificateNo.Value = "744373";
            certificateNo.Style.Font.FontSize = 10;
            certificateNo.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            certificateNo.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            var address = worksheet.Range("C13:D14");
            address.Merge();

            address.Value = "Adresas/Address:";
            address.Style.Font.FontSize = 10;
            address.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            address.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            var addressValue = worksheet.Range("E13:G14");
            addressValue.Merge();

            addressValue.Value = "Universiteto g. 2-408,\nAkademija, Kauno raj.";
            addressValue.Style.Font.FontSize = 10;
            addressValue.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            addressValue.Style.Alignment.WrapText = true;


            var bank = worksheet.Range("C15:D15");
            bank.Merge();
            bank.Value = "Bankas/Bank:";
            bank.Style.Font.FontSize = 10;
            bank.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            bank.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            var bankName = worksheet.Range("E15:F15");
            bankName.Merge();
            bankName.Value = "AB 'Swedbank', 73000";
            bankName.Style.Font.FontSize = 10;
            bankName.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            bankName.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            var iban = worksheet.Range("C16:D16");
            iban.Merge();

            iban.Value = "Ats.saskaita/IBAN code:";
            iban.Style.Font.FontSize = 10;
            iban.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            iban.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            var ibanCode = worksheet.Range("E16:F16");
            ibanCode.Merge();
            ibanCode.Value = "LT537300010112740844";
            ibanCode.Style.Font.FontSize = 10;
            ibanCode.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ibanCode.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        }
    }
}
