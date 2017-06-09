using SquareFillDomain.Models;
using SquareFillDomain.Utils;

namespace SquareFillDomain.Controllers
{
    public class ShapeController
    {
        public int CurrentShapeCornerX { get { return _shapeMover.CurrentShapeCornerX; } }
        public int CurrentShapeCornerY { get { return _shapeMover.CurrentShapeCornerY; } }

        private readonly ShapeMover _shapeMover;

        public ShapeController(ShapeSet shapeSet, Grid occupiedGridSquares)
        {
            _shapeMover = new ShapeMover(shapeSet: shapeSet, occupiedGridSquares: occupiedGridSquares);
        }

        public Shape StartMove(SquareFillPoint cursorPositionAtStart)
        {
            return _shapeMover.StartMove(cursorPositionAtStart: cursorPositionAtStart);
        }

        public void ContinueMove(SquareFillPoint newLocation) 
        {
            _shapeMover.ContinueMove(newLocation: newLocation);
        }

        public void EndMove(SquareFillPoint finalLocation) 
        {
            _shapeMover.EndMove(finalLocation: finalLocation);
        }
    }
}