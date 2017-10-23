using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

using CourseTRForms;

namespace NUnitTest1
{
    [TestFixture]
    public class TestFixture1
    {
       /* [Test]
        public void TestTrue()
        {
            Assert.IsTrue(true);
        }

        // This test fail for example, replace result or delete this test to see all tests pass
        [Test]
        public void TestFault()
        {
            Assert.IsTrue(false);
        }*/

        [Test]
        public void testSubsetSetNoResults()
        {
            double[] products = { 2.41, 3.24, 2.09, 2.56, 3.28, 3.88, 1.70, 4.93, 3.30 };
            string actualResult = CourseTRForms.Form1.subsetSet(products, 10);
            string expectedResult = "";

            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void testSubsetSetResult()
        {
            double[] products = { 2.41, 3.24, 2.09, 2.56, 3.28, 3.88, 1.70, 4.93, 3.30 };
            string actualResult = CourseTRForms.Form1.subsetSet(products, 7.8);
            string expectedResult = "2,41 2,09 3,3 \r\n";

            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
