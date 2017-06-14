using SquareFillDomain.Models;

namespace SquareFillDomain.Builders
{
    // public protocol IShapeSetBuilder
    public interface IShapeSetBuilder
    {
        // func OccupyBorderSquares(occupiedGridSquares: Grid);
        void OccupyBorderSquares(Grid occupiedGridSquares);

        // func GetShapeSet() -> ShapeSet;
        ShapeSet GetShapeSet();

        // func MakeGridSquares() -> Grid;
        Grid MakeGridSquares();
    }
}