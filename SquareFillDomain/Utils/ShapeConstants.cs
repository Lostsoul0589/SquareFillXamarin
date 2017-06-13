using System.Collections.Generic;
using SquareFillDomain.Models;

namespace SquareFillDomain.Utils
{
    public class ShapeConstants
    {
        public static int SquareWidth = 32;

        public static SquareFillRect ContainingRectangle = new SquareFillRect(
            x: 4 * SquareWidth,
            y: 6 * SquareWidth,
            width: 7 * SquareWidth,
            height: 7 * SquareWidth);

        public static SquareFillPoint TopLeftGridSquare = SquareFillPoint(
            x: ContainingRectangle.X,
            y: ContainingRectangle.Y);

        // NB If these four v\alues ever stop being hard-coded - eg calculated from device screen dimensions
        //  ... we can use the RoundDimensionDownToMultipleOfSquareWidth method below.
        public static int GridWidth = 13;
        public static int GridHeight = 20;
        public static int ScreenWidth = GridWidth * SquareWidth; 
        public static int ScreenHeight = GridHeight * SquareWidth;

        private int RoundDimensionDownToMultipleOfSquareWidth(int screenDimension)
        {
            var maxNumberOfGridSquaresInDimension = screenDimension / ShapeConstants.SquareWidth;
            return maxNumberOfGridSquaresInDimension * ShapeConstants.SquareWidth;
        }

        public static List<SquareFillPoint> RightHydrantPoints = new List<SquareFillPoint> { SquareFillPoint(x: 0, y: 0), SquareFillPoint(x: 0, y: 1), SquareFillPoint(x: 0, y: 2), SquareFillPoint(x: 1, y: 1) };
        public static List<SquareFillPoint> FourBarPoints = new List<SquareFillPoint> { SquareFillPoint(x: 0, y: 0), SquareFillPoint(x: 1, y: 0), SquareFillPoint(x: 2, y: 0), SquareFillPoint(x: 3, y: 0) };
        public static List<SquareFillPoint> SevenPoints = new List<SquareFillPoint> { SquareFillPoint(x: 0, y: 0), SquareFillPoint(x: 1, y: 0), SquareFillPoint(x: 1, y: 1), SquareFillPoint(x: 1, y: 2) };
        public static List<SquareFillPoint> FourSquarePoints = new List<SquareFillPoint> { SquareFillPoint(x: 0, y: 0), SquareFillPoint(x: 1, y: 0), SquareFillPoint(x: 0, y: 1), SquareFillPoint(x: 1, y: 1) };
        public static List<SquareFillPoint> LeftCornerPoints = new List<SquareFillPoint> { SquareFillPoint(x: 0, y: 0), SquareFillPoint(x: 1, y: 0), SquareFillPoint(x: 1, y: 1) };
        public static List<SquareFillPoint> UpsideDownTPoints = new List<SquareFillPoint> { SquareFillPoint(x: 0, y: 0), SquareFillPoint(x: -1, y: 1), SquareFillPoint(x: 0, y: 1), SquareFillPoint(x: 1, y: 1) };
        public static List<SquareFillPoint> ThreePolePoints = new List<SquareFillPoint> { SquareFillPoint(x: 0, y: 0), SquareFillPoint(x: 0, y: 1), SquareFillPoint(x: 0, y: 2) };
        public static List<SquareFillPoint> TwoPolePoints = new List<SquareFillPoint> { SquareFillPoint(x: 0, y: 0), SquareFillPoint(x: 0, y: 1) };
        public static List<SquareFillPoint> BackwardsLPoints = new List<SquareFillPoint> { SquareFillPoint(x: 0, y: 0), SquareFillPoint(x: 0, y: 1), SquareFillPoint(x: 0, y: 2), SquareFillPoint(x: -1, y: 2) };
        public static List<SquareFillPoint> SingleSquarePoints = new List<SquareFillPoint> { SquareFillPoint(x: 0, y: 0) };

        public static List<SquareFillPoint> LeftHydrantPoints = new List<SquareFillPoint> { SquareFillPoint(x: 0, y: 0), SquareFillPoint(x: -1, y: 1), SquareFillPoint(x: 0, y: 1), SquareFillPoint(x: 0, y: 2) };
        public static List<SquareFillPoint> CrossShapePoints = new List<SquareFillPoint> { SquareFillPoint(x: 0, y: 0), SquareFillPoint(x: -1, y: 1), SquareFillPoint(x: 0, y: 1), SquareFillPoint(x: 1, y: 1), SquareFillPoint(x: 0, y: 2) };
        public static List<SquareFillPoint> RightWayUpTPoints = new List<SquareFillPoint> { SquareFillPoint(x: 0, y: 0), SquareFillPoint(x: 1, y: 0), SquareFillPoint(x: 2, y: 0), SquareFillPoint(x: 1, y: 1) };
        public static List<SquareFillPoint> NineSquarePoints = new List<SquareFillPoint>
        {
            SquareFillPoint(x: 0, y: 0), SquareFillPoint(x: 1, y: 0), SquareFillPoint(x: 2, y: 0), 
            SquareFillPoint(x: 0, y: 1), SquareFillPoint(x: 1, y: 1), SquareFillPoint(x: 2, y: 1), 
            SquareFillPoint(x: 0, y: 2), SquareFillPoint(x: 1, y: 2), SquareFillPoint(x: 2, y: 2)
        };

        public static Grid MakeGridSquares()
        {
            var occupiedGridSquares = new Grid(width: GridWidth, height: GridHeight);
            return occupiedGridSquares;
        }

        private static SquareFillPoint SquareFillPoint(int x, int y)
        {
            return new SquareFillPoint(x: x, y: y);
        }
    }
}