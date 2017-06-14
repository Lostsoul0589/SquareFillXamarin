using SquareFillDomain.Models;

namespace SquareFillDomain.Interfaces
{
    // protocol ISquareView
    public interface ISquareView
    {
        // func MoveSquare(newX: Int, newY: Int)
        void MoveSquare(int newX, int newY);

        // func MoveTopLeftCorner(newX: Int, newY: Int)
        void MoveTopLeftCorner(int newX, int newY);

        // func Centre() -> SquareFillPoint
        SquareFillPoint Centre();

        // func TopLeftCorner() -> SquareFillPoint
        SquareFillPoint TopLeftCorner();
    }
}