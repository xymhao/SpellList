#nullable enable
using System;
using System.Runtime.InteropServices.ComTypes;

namespace SpellList.Algorithm
{
    public class Product
    {
        public Product(string name, decimal price)
        {
            Name = name;
            Price = price;
        }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public Guid Id { get; } = Guid.NewGuid();

        public override string ToString()
        {
            return $"{Name}-{Price}";
        }

        public override bool Equals(object? obj)
        {
            return obj is Product y && this.Id.Equals(y.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}