using System;
using System.Collections.Generic;
using SpellList.Algorithm;

namespace SpellList.CMD
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = "内衣：28.8，盆子:32.9,架子：62.1，靴子：1214，智能锁：4019，擦脸:216".Replace("：", ":").Replace("，", ",");

            do
            {
                var list = new List<Product>();

                Console.WriteLine("请输入 商品：价格，以逗号分隔。例如：内衣：28.8，盆子:32.9,架子：62.1，靴子：1214");
                try
                {
                    input = Console.ReadLine().Replace("：", ":").Replace("，", ",");
                    var products = input.Split(",");
                    foreach (var prod in products)
                    {
                        var arr = prod.Split(":");
                        Product prd = new Product(arr[0], Convert.ToDecimal(arr[1]));
                        list.Add(prd);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("输入格式有问题，请确认。");
                }
                var result = DynamicCalculate.Calculate(300, 20, list);
                foreach (var allocation in result)
                {
                    var index = result.IndexOf(allocation);
                    Console.WriteLine();
                    Console.WriteLine($"{index,4}  {allocation}");
                }

            } while (input != "exist");


            Console.Read();
        }
    }
}
