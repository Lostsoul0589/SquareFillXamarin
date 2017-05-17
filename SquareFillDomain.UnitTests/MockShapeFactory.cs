using SquareFillDomain.Interfaces;
using SquareFillDomain.Utils;

namespace SquareFillDomain.UnitTests
{
    public class MockShapeFactory : ISquareViewFactory
    {
        public ISquareView MakeSquare(SquareFillColour colour)
        {
            return new MockSquareView();
        }
    }
}
