using SquareFillDomain.Models;

namespace SquareFillDomain.Interfaces
{
    public class NullSquareView : ISquareView
    {
        // public func MoveSquare(newX: Int, newY: Int)
        public void MoveSquare(int newX, int newY)
        {
        }

        // public func MoveTopLeftCorner(newX: Int, newY: Int)
        public void MoveTopLeftCorner(int newX, int newY)
        {
        }

        // public func Centre() -> SquareFillPoint
        public SquareFillPoint Centre()
        {
            return new SquareFillPoint(x: 0, y: 0);
        }

        // public func TopLeftCorner() -> SquareFillPoint
        public SquareFillPoint TopLeftCorner()
        {
            return new SquareFillPoint(x: 0, y: 0);
        }
    }
}
