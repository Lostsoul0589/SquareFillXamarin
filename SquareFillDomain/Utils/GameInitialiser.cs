using SquareFillDomain.Builders;
using SquareFillDomain.Controllers;
using SquareFillDomain.Models;

namespace SquareFillDomain.Utils
{
    public class GameInitialiser
    {
        public ShapeController MakeShapeController(IShapeSetBuilder shapeSetBuilder)
        {
            var occupiedGridSquares = shapeSetBuilder.MakeGridSquares();
            var shapeSet = shapeSetBuilder.GetShapeSet();
            PutAllShapesIntoGrid(
                shapeSetBuilder: shapeSetBuilder, 
                shapeSet: shapeSet,
                occupiedGridSquares: occupiedGridSquares);

            return new ShapeController(
                shapeSet: shapeSet,
                occupiedGridSquares: occupiedGridSquares);
        }

        private void PutAllShapesIntoGrid(
            IShapeSetBuilder shapeSetBuilder,
            ShapeSet shapeSet,
            Grid occupiedGridSquares)
        {
            shapeSetBuilder.OccupyBorderSquares(occupiedGridSquares: occupiedGridSquares);
            shapeSet.OccupyGridSquares(occupiedGridSquares: occupiedGridSquares);
        }
    }
}