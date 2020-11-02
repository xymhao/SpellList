using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpellList.Algorithm
{
    public class DynamicCalculate
    {
        public static List<Allocation> GetOptimalCombination(decimal amount, decimal miniNum, List<Product> products)
        {
            var calculates = DynamicCalculate.Calculate(amount, miniNum, products);
            foreach (Allocation allocation in calculates
                                                .Where(allocation => allocation.Products.Count == products.Count))
            {
                return new List<Allocation>() { allocation };
            }

            //处理所有拼单 缺失项
            foreach (Allocation allocation in calculates)
            {
                allocation.ExceptProducts = products.Except(allocation.Products).ToList();
            }

            return calculates;
        }

        public static List<Allocation> Calculate(decimal amount, decimal miniNum, List<Product> products)
        {
            var allocations = Allocation(amount, miniNum, products);

            return allocations;
        }

        private static List<Allocation> Allocation(decimal amount, decimal miniNum,
            List<Product> products)
        {
            var result = new List<Allocation>();
            var allCombination = FullCombination(products);
            foreach (var prods in allCombination)
            {
                var allocation = new Allocation(prods);
                if (allocation.Validate(amount, miniNum))
                {
                    result.Add(allocation);
                }
            }
            return result;
        }

        public static List<List<T>> FullCombination<T>(List<T> lstSource)
        {
            var n = lstSource.Count;
            var max = 1 << n;
            var lstResult = new List<List<T>>();
            for (var i = 0; i < max; i++)
            {
                var lstTemp = new List<T>();
                for (var j = 0; j < n; j++)
                {
                    if ((i >> j & 1) > 0)
                    {
                        lstTemp.Add(lstSource[j]);
                    }
                }
                lstResult.Add(lstTemp);
            }
            lstResult.RemoveAt(0);
            return lstResult;
        }
    }
}
