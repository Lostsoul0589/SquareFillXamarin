// using NUnit.Framework;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SquareFillDomain.Models;

namespace SquareFillDomain.UnitTests.TestUtils
{
    // public class Asserter
    public static class Asserter
    {
        public static void AreEqual(object actual, object expected)
        {
            Assert.AreEqual(expected, actual);
        }

        //public static func AreEqual(actual: Bool, expected: Bool)
        //{
        //    XCTAssertEqual(expected, actual);
        //}

        //public static func AreEqual(actual: Int, expected: Int)
        //{
        //    XCTAssertEqual(expected, actual);
        //}
    }
}