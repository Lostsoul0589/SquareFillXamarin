using SquareFillDomain.Interfaces;
using SquareFillDomain.Models;

namespace SquareFillDomain.UnitTests.TestUtils
{
    public class MockSquareView : ISquareView
    {
        private readonly SquareFillPoint _centre = new SquareFillPoint(x: 0, y: 0);
        private readonly SquareFillPoint _topLeftCorner = new SquareFillPoint(x: 0, y: 0);

        public void MoveSquare(int newX, int newY)
        {
            _centre.X = newX;
            _centre.Y = newY;
        }

        public void MoveTopLeftCorner(int newX, int newY)
        {
            _topLeftCorner.X = newX;
            _topLeftCorner.Y = newY;
        }

        public SquareFillPoint Centre()
        {
            return _centre;
        }

        public SquareFillPoint TopLeftCorner()
        {
            return _topLeftCorner;
        }
    }
}
