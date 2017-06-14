using SquareFillDomain.Interfaces;
using SquareFillDomain.Models;

namespace SquareFillDomain.UnitTests.TestUtils
{
    public class MockSquareView : ISquareView
    {
        private readonly SquareFillPoint _centre = new SquareFillPoint(x: 0, y: 0);
        private readonly SquareFillPoint _topLeftCorner = new SquareFillPoint(x: 0, y: 0);

        // public func MoveSquare(newX: Int, newY: Int)
        public void MoveSquare(int newX, int newY)
        {
            _centre.X = newX;
            _centre.Y = newY;
            _topLeftCorner.X = newX - TestConstants.SquareWidth/2;
            _topLeftCorner.Y = newY - TestConstants.SquareWidth / 2;
        }

        // public func MoveTopLeftCorner(newX: Int, newY: Int)
        public void MoveTopLeftCorner(int newX, int newY)
        {
            _topLeftCorner.X = newX;
            _topLeftCorner.Y = newY;
            _centre.X = newX + TestConstants.SquareWidth / 2;
            _centre.Y = newY + TestConstants.SquareWidth / 2;
        }

        // public func Centre() -> SquareFillPoint
        public SquareFillPoint Centre()
        {
            return _centre;
        }

        // public func TopLeftCorner() -> SquareFillPoint
        public SquareFillPoint TopLeftCorner()
        {
            return _topLeftCorner;
        }
    }
}
