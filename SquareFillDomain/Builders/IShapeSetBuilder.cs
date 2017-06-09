using SquareFillDomain.Models;

namespace SquareFillDomain.Builders
{
    public interface IShapeSetBuilder
    {
        void OccupyBorderSquares(Grid occupiedGridSquares);

        ShapeSet GetShapeSet();

        Grid MakeGridSquares();
    }
}