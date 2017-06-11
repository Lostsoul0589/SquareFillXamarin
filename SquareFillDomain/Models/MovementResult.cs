namespace SquareFillDomain.Models
{
    public class MovementResult
    {
        public MovementResult()
        {
            ThereAreShapesInTheWay = false;
        }

        public bool ShapeHasCrossedAHorizontalGridBoundary { get; set; }
        public bool ShapeHasCrossedAVerticalGridBoundary { get; set; }
        public bool NoShapesAreInTheWay { get { return !ThereAreShapesInTheWay; } }
        public bool ThereAreShapesInTheWay { get; set; }
    }
}