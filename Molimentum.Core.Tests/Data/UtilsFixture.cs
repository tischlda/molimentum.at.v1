using System;
using NUnit.Framework;

namespace Molimentum.Tests
{
    [TestFixture]
    public class UtilsFixture
    {
        [Test]
        [TestCase(10, 0, Result = 1)]
        [TestCase(10, 1, Result = 1)]
        [TestCase(10, 10, Result = 1)]
        [TestCase(10, 11, Result = 2)]
        [TestCase(10, 20, Result = 2)]
        [TestCase(10, 21, Result = 3)]
        [TestCase(1, 1, Result = 1)]
        [TestCase(1, 2, Result = 2)]
        public int CalculatePagesTest(int pageSize, int itemCount)
        {
            return Utils.CalculatePages(pageSize, itemCount);
        }

        [Test]
        [TestCase(0, 1)]
        [TestCase(-1, 1)]
        public void InvalidPageSizeTest(int pageSize, int itemCount)
        {
            try
            {
                Utils.CalculatePages(pageSize, itemCount);
                Assert.Fail();
            }
            catch(ArgumentException ex)
            {                
                Assert.That(ex.ParamName, Is.EqualTo("pageSize"));
            }
        }

        [Test]
        [TestCase(10, -1)]
        public void InvalidItemCountTest(int pageSize, int itemCount)
        {
            try
            {
                Utils.CalculatePages(pageSize, itemCount);
                Assert.Fail();
            }
            catch (ArgumentException ex)
            {
                Assert.That(ex.ParamName, Is.EqualTo("itemCount"));
            }
        }

        [Test]
        [TestCase(0, 10, Result = 1)]
        [TestCase(1, 10, Result = 1)]
        [TestCase(9, 10, Result = 1)]
        [TestCase(10, 10, Result = 2)]
        [TestCase(19, 10, Result = 2)]
        [TestCase(20, 10, Result = 3)]
        [TestCase(0, 1, Result = 1)]
        [TestCase(1, 1, Result = 2)]
        public int CalculatePageNumberTest(int itemIndex, int pageSize)
        {
            return Utils.CalculatePageNumber(itemIndex, pageSize);
        }
    }
}
