using ClosedXML.Excel;

namespace PersonMVC.Common.Helpers;

public class FileHelper<T>
{
    public static byte[] GenerateExcelFile(List<T> items)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add(typeof(T).Name + "s");

        // Add header row
        var properties = typeof(T).GetProperties();
        for (var i = 0; i < properties.Length; i++)
        {
            worksheet.Cell(1, i + 1).Value = properties[i].Name;
        }

        // Add items data
        for (var i = 0; i < items.Count; i++)
        {
            var item = items[i];
            for (var j = 0; j < properties.Length; j++)
            {
                var value = properties[j].GetValue(item, null);
                if (value is DateTime dateTime)
                {
                    worksheet.Cell(i + 2, j + 1).Value = dateTime.ToString("dd/MM/yyyy");
                    continue;
                }
                worksheet.Cell(i + 2, j + 1).Value = value?.ToString();
            }
        }

        // Format header
        var headerRange = worksheet.Range(1, 1, 1, properties.Length);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        // Auto-fit columns
        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);

        return stream.ToArray();
    }
}