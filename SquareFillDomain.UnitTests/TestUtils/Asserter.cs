using NUnit.Framework;

namespace SquareFillDomain.UnitTests.TestUtils
{
    public static class Asserter
    {
        public static void AreEqual(object objectToCompare, object expectedValue)
        {
            Assert.AreEqual(expectedValue, objectToCompare);
        }
    }
}