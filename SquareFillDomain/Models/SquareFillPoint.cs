using SquareFillDomain.Utils;

namespace SquareFillDomain.Models
{
    public class SquareFillPoint
    {
        public int X { get; set; }
        public int Y { get; set; }

        public SquareFillPoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        public SquareFillPoint ConvertToPixels()
        {
            return new SquareFillPoint(
                x: X * ShapeConstants.SquareWidth,
                y: Y * ShapeConstants.SquareWidth);
        }
    }
}