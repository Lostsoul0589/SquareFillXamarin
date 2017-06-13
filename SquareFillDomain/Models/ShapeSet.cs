using System.Collections.Generic;

namespace SquareFillDomain.Models
{
    public class ShapeSet
    {
        private readonly IEnumerable<Shape> _shapes;

		public ShapeSet(List<Shape> shapes)
        {
            _shapes = shapes;
		}

        public Shape SelectShape(SquareFillPoint selectedPoint, bool selectedPointIsGridRef = false)
	    {
            Shape selectedShape = null;
	        if (selectedPointIsGridRef)
	        {
	            selectedPoint = selectedPoint.ConvertToPixels();
	        }

	        foreach(var shape in _shapes) 
            {
                if (shape.IsInShape(point: selectedPoint))
                {
                    selectedShape = shape;
                }
            }
        
            return selectedShape;
	    }

        public void OccupyGridSquares(Grid occupiedGridSquares)
        {
            foreach (var shape in _shapes)
            {
                shape.OccupyGridSquares(occupiedGridSquares: occupiedGridSquares);
            }
        }
    }
}