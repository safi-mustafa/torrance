using System;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Hosting;

namespace Helpers.ExcelReader
{
    public static class LogExcelHelper
    {
        public static void AddLogo(IXLWorksheet overrideLogSheet, IHostingEnvironment env)
        {
            var logoUrl = env.ContentRootPath + "/wwwroot/img/logo.png";

            // Add the image to the worksheet
            var picture = overrideLogSheet.AddPicture(logoUrl)
                .MoveTo(1, 1); // Set the location where the image should appear

            // Set the dimensions (size) of the cell containing the image
            var cellContainingImage = overrideLogSheet.Cell(1, 1);
            cellContainingImage.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            cellContainingImage.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            cellContainingImage.Style.Alignment.WrapText = true; // Wrap text within the cell
            cellContainingImage.WorksheetRow().Height = 60; // Adjust row height to fit the image

            // Merge all cells in the first row to create a colspan effect
            overrideLogSheet.Range(1, 1, 1, overrideLogSheet.ColumnCount()).Merge();

            // Set the dimensions (size) of the image within the merged cell
            picture.Width = (int)cellContainingImage.WorksheetRow().Height + 80; // Set the width to match the row height
            picture.Height = (int)cellContainingImage.WorksheetRow().Height + 20; // Set the height to match the row height
        }
    }
}

