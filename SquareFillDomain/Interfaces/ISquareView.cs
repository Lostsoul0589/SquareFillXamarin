using SquareFillDomain.Models;

namespace SquareFillDomain.Interfaces
{
    public interface ISquareView
    {
        void MoveSquare(int newX, int newY);
        void MoveTopLeftCorner(int newX, int newY);
        SquareFillPoint Centre();
        SquareFillPoint TopLeftCorner();
    }
}