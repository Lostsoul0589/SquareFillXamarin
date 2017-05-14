namespace SquareFillXamarin.Models
{
    public class MovementResult
    {
        public bool ShapeHasCrossedAHorizontalGridBoundary { get; set; }
        public bool ShapeHasCrossedAVerticalGridBoundary { get; set; }
        public bool NoShapesAreintheWay { get; set; }
    }
}