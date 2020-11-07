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
        public IActionResult UpLoadFile([FromForm] List<IFormFile> files, [FromForm] decimal amount, [FromForm] decimal minNum)
        {
            Console.WriteLine("进入");

            try
            {
                foreach (var formFile in files)
                {
                    if (formFile.Length <= 0) continue;

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    var excelPackage = new ExcelPackage(formFile.OpenReadStream());
                    var workbook = excelPackage.Workbook;
                    foreach (var excelWorksheet in workbook.Worksheets)
                    {
                        var result = GetProductAllocation(excelWorksheet, amount, minNum);
                        if (result != null)
                        {
                            ViewData["Calculate"] = result;
                            return View("Calculate");
                        }
                    }
                }
            }
            catch (ExcelTransferException e)
            {
                return View("Error", new ErrorViewModel() { Message = e.Message });

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return View("Error", new ErrorViewModel() { Message = "服务器内部错误" });
            }

            return Ok(new { count = files.Count });
        }

        public FileResult DownTemplate()
        {
            string root = AppContext.BaseDirectory;
            var path = Path.Combine(root, "满减模板.xlsx");
            var file = System.IO.File.ReadAllBytes(path);
            return File(file, "application/x-xls");
        }

        [HttpPost]
        public IActionResult UpLoadFile2(decimal amount, decimal minNum)
        {
            return Ok(new { count = amount, minNum });
        }


        private static List<Allocation> GetProductAllocation(ExcelWorksheet excelWorksheet, in decimal amount, in decimal minNum)
        {
            if (excelWorksheet.Name != "满减")
            {
                throw new ExcelTransferException($"模板不符合要求，请下载最新的模板。");
            }

            List<Product> list = TransferProduct(excelWorksheet);
            var allocation = new SpellAllocation(list, amount, minNum);
            return allocation.GetOptimalCombination();
        }

        private static List<Product> TransferProduct(ExcelWorksheet excelWorksheet)
        {
            var start = excelWorksheet.Dimension.Start;
            var end = excelWorksheet.Dimension.End;
            List<Product> list = new List<Product>();
            for (int row = start.Row; row <= end.Row; row++)
            {
                string name = excelWorksheet.Cells[row, 1].Text; // This got me the actual value I needed.
                string price = excelWorksheet.Cells[row, 2].Text; // This got me the actual value I needed.
                if (name.Equals("商品名称") || price.Equals("金额"))
                {
                    continue;
                }

                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(price))
                {
                    continue;
                }

                if (!decimal.TryParse(price, out decimal priceValue))
                {
                    throw new ExcelTransferException($"商品名称:{name},金额：{price}，价格类型转换失败。");
                }

                var product = new Product(name, priceValue);
                list.Add(product);
            }

            return list;
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
            return View(new ErrorViewModel() { Message = "系统异常" });
        }
    }

    internal class ExcelTransferException : Exception
    {
        public ExcelTransferException(string message) : base(message)
        {
        }
    }
}
