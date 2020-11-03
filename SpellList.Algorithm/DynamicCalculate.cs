using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpellList.Algorithm
{
    public class DynamicCalculate
    {
        public static List<Allocation> GetOptimalCombination2(decimal amount, decimal miniNum, List<Product> products)
        {
            List<Allocation> allocations = new List<Allocation>();
            Allocation allocation = new Allocation();
            //GetAllocation(amount, miniNum, products, allocation, allocations);
            GetAllocation2(amount, miniNum, 0, products, allocation, allocations);
            return allocations;
        }

        private static void GetAllocation(in decimal amount, in decimal miniNum, List<Product> products, Allocation allocation, List<Allocation> allocations)
        {
            if (allocation.Validate(amount, miniNum))
            {
                if (!allocations.Contains(allocation))
                {
                    allocations.Add(allocation);
                }
                else
                {
                    return;
                }
            }

            foreach (var product in products)
            {
                if (!allocation.Products.Contains(product))
                {
                    allocation.Products.Add(product);
                }
                else
                {
                    continue;
                }
                GetAllocation(amount, miniNum, products.ToList(), allocation, allocations);

                var except = new Allocation();
                GetAllocation(amount, miniNum, products.Where(x => x.Id != product.Id).ToList(), except, allocations);
            }
        }

        private static void GetAllocation2(in decimal amount, in decimal miniNum,int num, List<Product> products, Allocation allocation, List<Allocation> allocations)
        {
            if (allocation.Validate(amount, miniNum))
            {
                if (!allocations.Contains(allocation))
                {
                    var item = new Allocation();
                    item.Products.AddRange(allocation.Products);
                    allocations.Add(item);
                }
                else
                {
                    return;
                }
            }

            if (num == products.Count)
            {
                return;
            }

            var product = products[num];
            if (!allocation.Products.Contains(product))
            {
                allocation.Products.Add(product);
            }
            //包含product
            GetAllocation2(amount, miniNum, num + 1, products.ToList(), allocation, allocations);
            
            //不包含product
            allocation.Products.Remove(product);
            GetAllocation2(amount, miniNum, num + 1, products.ToList(), allocation, allocations);
        }

        public static List<Allocation> GetOptimalCombination(decimal amount, decimal miniNum, List<Product> products)
        {
            var calculates = DynamicCalculate.Calculate(amount, miniNum, products);
            foreach (Allocation allocation in calculates
                                                .Where(allocation => allocation.Products.Count == products.Count))
            {
                return new List<Allocation>() { allocation };
            }

            Recursive(amount, miniNum, products, calculates);

            calculates.Sort((allocation, allocation1) => (allocation.Count % 300).CompareTo(allocation1.Count % 300));
            return calculates;
        }

        private static void Recursive(decimal amount, decimal miniNum, List<Product> products, List<Allocation> calculates)
        {
            //处理所有拼单 缺失项
            foreach (Allocation allocation in calculates)
            {
                allocation.ExceptProducts = products.Except(allocation.Products).ToList();
                allocation.ExceptAllocations = Calculate(amount, miniNum, allocation.ExceptProducts);
                Recursive(amount, miniNum, allocation.ExceptProducts, allocation.ExceptAllocations);
            }
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
