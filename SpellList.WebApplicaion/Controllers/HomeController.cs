using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using SpellList.Algorithm;
using SpellList.WebApplicaion.Models;

namespace SpellList.WebApplicaion.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpLoadFile(List<IFormFile> files, decimal amount, decimal minNum)
        {
            long size = files.Sum(f => f.Length);

            try
            {
                foreach (var formFile in files)
                {
                    if (formFile.Length > 0)
                    {
                        {
                            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                            var excelPackage = new ExcelPackage(formFile.OpenReadStream());
                            var workbook = excelPackage.Workbook;
                            foreach (var excelWorksheet in workbook.Worksheets)
                            {
                                var result = HandleExcel(excelWorksheet,amount, minNum);
                                if (result != null)
                                {
                                    ViewData["Calculate"] = result;
                                    return View("Calculate", result);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return Ok(new { count = files.Count, size });
        }


        private static List<Allocation> HandleExcel(ExcelWorksheet excelWorksheet, in decimal amount, in decimal minNum)
        {
            if (excelWorksheet.Name != "满减")
            {
                return null;
            }

            var start = excelWorksheet.Dimension.Start;
            var end = excelWorksheet.Dimension.End;
            List<Product> list = new List<Product>();
            for (int row = start.Row; row <= end.Row; row++)
            {
                string name = excelWorksheet.Cells[row, 1].Text; // This got me the actual value I needed.
                string price = excelWorksheet.Cells[row, 2].Text; // This got me the actual value I needed.
                var product = new Product(name, Convert.ToDecimal(price));
                list.Add(product);
            }

            return DynamicCalculate.Calculate(amount, minNum, list);

        }

        public IActionResult Calculate(List<Allocation> list)
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
