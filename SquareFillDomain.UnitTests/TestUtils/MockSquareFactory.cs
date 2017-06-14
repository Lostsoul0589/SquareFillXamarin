using SquareFillDomain.Interfaces;
using SquareFillDomain.Utils;

namespace SquareFillDomain.UnitTests.TestUtils
{
    public class MockSquareFactory : ISquareViewFactory
    {
        // public func MakeSquare(colour: SquareFillColour) -> ISquareView
        public ISquareView MakeSquare(SquareFillColour colour)
        {
            return new MockSquareView();
        }
    }
}
