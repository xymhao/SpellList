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
            var result = DynamicCalculate.GetOptimalCombination(300, 10, product);
            var result2 = DynamicCalculate.GetOptimalCombination2(300, 10, product);
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod()]
        public void CalculateTest2()
        {
            //a：500，b：195，c：105，d：100
            var product = new List<Product>()
            {
                new Product("内衣", 28.8m),
                new Product("盆子", 32.9m),
                new Product("架子", 62.1m),
                new Product("靴子", 1214),
                new Product("智能锁", 4019),
                new Product("擦脸", 216)
            };
            var result = DynamicCalculate.GetOptimalCombination(300, 20, product);
            var result2 = DynamicCalculate.GetOptimalCombination2(300, 20, product);
            Assert.AreEqual(3, result.Count);
        }
    }
}