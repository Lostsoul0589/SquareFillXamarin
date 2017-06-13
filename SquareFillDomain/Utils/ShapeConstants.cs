using System.Collections.Generic;
using SquareFillDomain.Models;

namespace SquareFillDomain.Utils
{
    public class ShapeConstants
    {    
        //public static let SquareWidth: Int = 32
        public static int SquareWidth = 32;

        // NB If these four values ever stop being hard-coded - eg calculated from device screen dimensions
        //  ... we can use the RoundDimensionDownToMultipleOfSquareWidth method below.
        public static int GridWidth = 13;
        public static int GridHeight = 20;
        public static int ScreenWidth = GridWidth * SquareWidth;
        public static int ScreenHeight = GridHeight * SquareWidth;
        //public static let GridWidth: Int = 13
        //public static let GridHeight: Int = 20
        //public static let ScreenWidth: Int = GridWidth * SquareWidth
        //public static let ScreenHeight: Int = GridHeight * SquareWidth

        //private static func RoundDimensionDownToMultipleOfSquareWidth(screenDimension: Int) -> Int {
        private int RoundDimensionDownToMultipleOfSquareWidth(int screenDimension)
        {
            var maxNumberOfGridSquaresInDimension = screenDimension / ShapeConstants.SquareWidth;
            return maxNumberOfGridSquaresInDimension * ShapeConstants.SquareWidth;
        }

        //public static var ContainingRectangle = SquareFillRect(
        public static SquareFillRect ContainingRectangle = new SquareFillRect(
            x: 4 * SquareWidth,
            y: 6 * SquareWidth,
            width: 7 * SquareWidth,
            height: 7 * SquareWidth);
        
        //public static let TopLeftGridSquare = SquareFillPoint(
        public static SquareFillPoint TopLeftGridSquare = SquareFillPoint(
            x: ContainingRectangle.X,
            y: ContainingRectangle.Y);

        //public static let RightHydrantPoints: [Point] = [Point(x:0, y:0), Point(x:1, y:0), Point(x:0, y:1), Point(x:0, y:-1)]
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
        
        //public static func MakeGridSquares() -> Grid {
        public static Grid MakeGridSquares() {
            var occupiedGridSquares = new Grid(width: GridWidth, height: GridHeight);
            return occupiedGridSquares;
        }

        private static SquareFillPoint SquareFillPoint(int x, int y)
        {
            return new SquareFillPoint(x: x, y: y);
        }
    }
}