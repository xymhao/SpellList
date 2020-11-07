using System;
using System.Collections.Generic;
using System.Linq;

namespace SpellList.Algorithm
{
    public class Allocation
    {
        public Allocation()
        {
            
        }
        public Allocation(List<Product> products)
        {
            Products = products;
        }

        public List<Allocation> ExceptAllocations { get; set; } = new List<Allocation>();

        /// <summary>
        /// 当前方案集合
        /// </summary>
        public List<Product> Products { get; set; } = new List<Product>();

        public List<Product> ExceptProducts { get; set; } = new List<Product>();

        public decimal Count => Products.Sum(x => x.Price);

        public string Combination => string.Join(",", Products.Select(x => $"{x.Name}({x.Price})"));

        public string ExceptCombination => string.Join(",", ExceptProducts.Select(x => $"{x.Name}({x.Price})"));

        public override string ToString()
        {
            return $"{Combination,-50} 总价:{Count,-10}  排除商品：{ExceptCombination}";
        }

        public bool AddProduct(Product product, in decimal amount, in decimal miniNum)
        {
            var elm = (Count + product.Price) % amount;
            if (Products.Contains(product))
            {
                return false;
            }

            if (Products.Count == 0)
            {
                Products.Add(product);
                return elm < miniNum;
            }

            if (elm < miniNum)
            {
                Products.Add(product);
                return true;
            }
            ExceptProducts.Add(product);
            return false;
        }

        public void AddPro(Product product)
        {
            if (!Products.Contains(product))
            {
                Products.Add(product);
            }

        }


        public bool Validate(in decimal amount, in decimal miniNum)
        {
            return Count % amount < miniNum && Count > amount;
        }

        public override bool Equals(object? obj)
        {
            Allocation allocation = obj as Allocation;
            if (allocation == null)
            {
                return false;
            }
            if (allocation.Products.Count != Products.Count)
            {
                return false;
            }

            for (int i = 0; i < allocation.Products.Count; i++)
            {
                if (allocation.Products[i] != Products[i])
                {
                    return false;
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            return string.Join(",",Products.Select(x=>x.Id)).GetHashCode();
        }
    }
}