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
            _topLeftCorner.X = newX - TestConstants.SquareWidth/2;
            _topLeftCorner.Y = newY - TestConstants.SquareWidth / 2;
        }

        public void MoveTopLeftCorner(int newX, int newY)
        {
            _topLeftCorner.X = newX;
            _topLeftCorner.Y = newY;
            _centre.X = newX + TestConstants.SquareWidth / 2;
            _centre.Y = newY + TestConstants.SquareWidth / 2;
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
