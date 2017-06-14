
namespace SquareFillDomain.Models
{
    public class SquareFillRect
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        // init(x: Int, y: Int, width: Int, height: Int)
        public SquareFillRect(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }
}