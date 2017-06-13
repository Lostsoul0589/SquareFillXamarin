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

	        foreach(var element in _shapes) {
                if (element.IsInShape(point: selectedPoint))
                {
                    selectedShape = element;
                }
            }
        
            return selectedShape;
	    }

        public void OccupyGridSquares(Grid occupiedGridSquares)
        {
            foreach (var element in _shapes) {
                element.OccupyGridSquares(occupiedGridSquares: occupiedGridSquares);
            }
        }
    }
}