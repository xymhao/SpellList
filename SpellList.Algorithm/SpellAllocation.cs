using System.Collections.Generic;
using System.Linq;

namespace SpellList.Algorithm
{
    public class SpellAllocation
    {
        private readonly List<Product> _products;
        private readonly decimal _amount;
        private readonly decimal _miniNum;

        public SpellAllocation(List<Product> products, decimal amount, decimal miniNum)
        {
            _products = products;
            _amount = amount;
            _miniNum = miniNum;
        }
        public List<Allocation> GetOptimalCombination()
        {
            List<Allocation> allocations = new List<Allocation>();
            AllocationRecursive(0, _products, new Allocation(), allocations);

            foreach (Allocation allocation in allocations
                .Where(allocation => allocation.Products.Count == _products.Count))
            {
                return new List<Allocation>() { allocation };
            }
            Recursive(_products, allocations);
            allocations.Sort((allocation, allocation1) => (allocation.Count % _amount).CompareTo(allocation1.Count % _amount));
            return allocations;
        }

        private void Recursive(List<Product> products, List<Allocation> calculates)
        {
            //处理所有拼单 缺失项
            foreach (Allocation allocation in calculates)
            {
                var list = new List<Allocation>();
                allocation.ExceptProducts = products.Except(allocation.Products).ToList();
                AllocationRecursive(0, allocation.ExceptProducts, new Allocation(), list);
                allocation.ExceptAllocations = list;
                Recursive(allocation.ExceptProducts, allocation.ExceptAllocations);
            }
        }

        private void AllocationRecursive(int num, List<Product> products, Allocation allocation, List<Allocation> allocations)
        {
            if (allocation.Validate(_amount, _miniNum))
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
            AllocationRecursive(num + 1, products.ToList(), allocation, allocations);

            //不包含product
            allocation.Products.Remove(product);
            AllocationRecursive(num + 1, products.ToList(), allocation, allocations);
        }

        private void GetAllocationBak(List<Product> products, Allocation allocation, List<Allocation> allocations)
        {
            if (allocation.Validate(_amount, _miniNum))
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
                GetAllocationBak(products.ToList(), allocation, allocations);

                var except = new Allocation();
                GetAllocationBak(products.Where(x => x.Id != product.Id).ToList(), except, allocations);
            }
        }

    }
}