using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpellList.Algorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpellList.Algorithm.Tests
{
    [TestClass()]
    public class DynamicCalculateTests
    {
        [TestMethod()]
        public void CalculateTest()
        {
            //a：500，b：195，c：105，d：100
            var product = new List<Product>()
            {
                new Product("a", 500),
                new Product("b", 195),
                new Product("c", 105),
                new Product("d", 100),
            };
            var result = DynamicCalculate.Calculate(300, 10, product);
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod()]
        public void GetAllCombinationTest()
        {
            var product = new List<Product>()
            {
                new Product("a", 500),
                new Product("b", 195),
                new Product("c", 105),
                new Product("d", 100),
            };
            var result = DynamicCalculate.FullCombination(product);



        }
    }
}