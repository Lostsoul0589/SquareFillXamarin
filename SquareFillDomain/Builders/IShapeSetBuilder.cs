using System.Collections.Generic;
using SquareFillDomain.Interfaces;
using SquareFillDomain.Models;

namespace SquareFillDomain.Builders
{
    public interface IShapeSetBuilder
    {
        void OccupyBorderSquares(List<List<GridSquare>> occupiedGridSquares);

        ShapeSet GetShapeSet(ISquareViewFactory squareViewFactory);
    }
}