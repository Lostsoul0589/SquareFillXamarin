// using NUnit.Framework;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SquareFillDomain.UnitTests.TestUtils
{
    // public class Asserter
    public static class Asserter
    {
        // public static func AreEqual(objectToCompare: AnyObject, expectedValue: AnyObject)
        public static void AreEqual(object objectToCompare, object expectedValue)
        {
            //XCTAssertEqual(expectedValue, objectToCompare);
            Assert.AreEqual(expectedValue, objectToCompare);
        }
    }
}