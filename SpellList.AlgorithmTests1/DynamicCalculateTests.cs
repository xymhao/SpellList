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
            Assert.AreEqual(1, result.Count);

            var spell = new SpellAllocation(product, 300, 10);
            var result2 = spell.GetOptimalCombination();
            
            CheckAlgorithm(result2, result);
        }

        private static void CheckAlgorithm(List<Allocation> result2, List<Allocation> result)
        {
            Assert.AreEqual(result2.Count, result.Count);
            for (int i = 0; i < result.Count; i++)
            {
                for (int j = 0; j < result[i].Products.Count; j++)
                {
                    Assert.AreEqual(result[i].Products[j], result2[i].Products[j]);
                }

                for (int j = 0; j < result[i].ExceptAllocations.Count; j++)
                {
                    Assert.AreEqual(result[i].ExceptAllocations[j], result2[i].ExceptAllocations[j]);
                }
            }
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
            Assert.AreEqual(3, result.Count);

            var spell = new SpellAllocation(product, 300, 20);
            var result2 = spell.GetOptimalCombination();

            CheckAlgorithm(result2, result);
        }
    }
}