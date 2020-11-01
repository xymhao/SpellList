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

        public override string ToString()
        {
            return $"{Name}-{Price}";
        }
    }
}