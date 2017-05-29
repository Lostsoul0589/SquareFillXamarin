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

        public static SquareFillPoint CentreOfTopLeftGridSquare = new SquareFillPoint(
            x: ContainingRectangle.X + SquareWidth / 2,
            y: ContainingRectangle.Y + SquareWidth / 2);
        public static SquareFillPoint TopLeftGridSquare = new SquareFillPoint(
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

        public static List<SquareFillPoint> RightHydrantCentrePoints = new List<SquareFillPoint> { new SquareFillPoint(x: 0, y: -1), new SquareFillPoint(x: 0, y: 0), new SquareFillPoint(x: 0, y: 1), new SquareFillPoint(x: 1, y: 0) };
        public static List<SquareFillPoint> FourBarCentrePoints = new List<SquareFillPoint> { new SquareFillPoint(x: -1, y: 0), new SquareFillPoint(x: 0, y: 0), new SquareFillPoint(x: 1, y: 0), new SquareFillPoint(x: 2, y: 0) };
        public static List<SquareFillPoint> SevenCentrePoints = new List<SquareFillPoint> { new SquareFillPoint(x: -1, y: 0), new SquareFillPoint(x: 0, y: 0), new SquareFillPoint(x: 0, y: 1), new SquareFillPoint(x: 0, y: 2) };
        public static List<SquareFillPoint> FourSquareCentrePoints = new List<SquareFillPoint> { new SquareFillPoint(x: -1, y: 0), new SquareFillPoint(x: 0, y: 0), new SquareFillPoint(x: -1, y: 1), new SquareFillPoint(x: 0, y: 1) };
        public static List<SquareFillPoint> LeftCornerCentrePoints = new List<SquareFillPoint> { new SquareFillPoint(x: -1, y: 0), new SquareFillPoint(x: 0, y: 0), new SquareFillPoint(x: 0, y: 1) };
        public static List<SquareFillPoint> UpsideDownTCentrePoints = new List<SquareFillPoint> { new SquareFillPoint(x: 0, y: -1), new SquareFillPoint(x: -1, y: 0), new SquareFillPoint(x: 0, y: 0), new SquareFillPoint(x: 1, y: 0) };
        public static List<SquareFillPoint> ThreePoleCentrePoints = new List<SquareFillPoint> { new SquareFillPoint(x: 0, y: -1), new SquareFillPoint(x: 0, y: 0), new SquareFillPoint(x: 0, y: 1) };
        public static List<SquareFillPoint> TwoPoleCentrePoints = new List<SquareFillPoint> { new SquareFillPoint(x: 0, y: -1), new SquareFillPoint(x: 0, y: 0) };
        public static List<SquareFillPoint> BackwardsLCentrePoints = new List<SquareFillPoint> { new SquareFillPoint(x: 0, y: -2), new SquareFillPoint(x: 0, y: -1), new SquareFillPoint(x: 0, y: 0), new SquareFillPoint(x: -1, y: 0) };
        public static List<SquareFillPoint> SingleSquareCentrePoints = new List<SquareFillPoint> { new SquareFillPoint(x: 0, y: 0) };

        public static List<SquareFillPoint> LeftHydrantCentrePoints = new List<SquareFillPoint> { new SquareFillPoint(x: 0, y: -1), new SquareFillPoint(x: -1, y: 0), new SquareFillPoint(x: 0, y: 0), new SquareFillPoint(x: 0, y: 1) };
        public static List<SquareFillPoint> RightWayUpTCentrePoints = new List<SquareFillPoint> { new SquareFillPoint(x: -1, y: 0), new SquareFillPoint(x: 0, y: 0), new SquareFillPoint(x: 1, y: 0), new SquareFillPoint(x: 0, y: 1) };

        public static List<SquareFillPoint> RightHydrantPoints = new List<SquareFillPoint> { new SquareFillPoint(x: 0, y: 0), new SquareFillPoint(x: 0, y: 1), new SquareFillPoint(x: 0, y: 2), new SquareFillPoint(x: 1, y: 1) };
        public static List<SquareFillPoint> FourBarPoints = new List<SquareFillPoint> { new SquareFillPoint(x: 0, y: 0), new SquareFillPoint(x: 1, y: 0), new SquareFillPoint(x: 2, y: 0), new SquareFillPoint(x: 3, y: 0) };
        public static List<SquareFillPoint> SevenPoints = new List<SquareFillPoint> { new SquareFillPoint(x: 0, y: 0), new SquareFillPoint(x: 1, y: 0), new SquareFillPoint(x: 1, y: 1), new SquareFillPoint(x: 1, y: 2) };
        public static List<SquareFillPoint> FourSquarePoints = new List<SquareFillPoint> { new SquareFillPoint(x: 0, y: 0), new SquareFillPoint(x: 1, y: 0), new SquareFillPoint(x: 0, y: 1), new SquareFillPoint(x: 1, y: 1) };
        public static List<SquareFillPoint> LeftCornerPoints = new List<SquareFillPoint> { new SquareFillPoint(x: 0, y: 0), new SquareFillPoint(x: 1, y: 0), new SquareFillPoint(x: 1, y: 1) };
        public static List<SquareFillPoint> UpsideDownTPoints = new List<SquareFillPoint> { new SquareFillPoint(x: 0, y: 0), new SquareFillPoint(x: -1, y: 1), new SquareFillPoint(x: 0, y: 1), new SquareFillPoint(x: 1, y: 1) };
        public static List<SquareFillPoint> ThreePolePoints = new List<SquareFillPoint> { new SquareFillPoint(x: 0, y: 0), new SquareFillPoint(x: 0, y: 1), new SquareFillPoint(x: 0, y: 2) };
        public static List<SquareFillPoint> TwoPolePoints = new List<SquareFillPoint> { new SquareFillPoint(x: 0, y: 0), new SquareFillPoint(x: 0, y: 1) };
        public static List<SquareFillPoint> BackwardsLPoints = new List<SquareFillPoint> { new SquareFillPoint(x: 0, y: 0), new SquareFillPoint(x: 0, y: 1), new SquareFillPoint(x: 0, y: 2), new SquareFillPoint(x: -1, y: 2) };
        public static List<SquareFillPoint> SingleSquarePoints = new List<SquareFillPoint> { new SquareFillPoint(x: 0, y: 0) };

        public static List<SquareFillPoint> LeftHydrantPoints = new List<SquareFillPoint> { new SquareFillPoint(x: 0, y: 0), new SquareFillPoint(x: -1, y: 1), new SquareFillPoint(x: 0, y: 1), new SquareFillPoint(x: 0, y: 2) };
        public static List<SquareFillPoint> RightWayUpTPoints = new List<SquareFillPoint> { new SquareFillPoint(x: 0, y: 0), new SquareFillPoint(x: 1, y: 0), new SquareFillPoint(x: 2, y: 0), new SquareFillPoint(x: 1, y: 1) };

        public static List<List<GridSquare>> MakeGridSquares()
        {
            var occupiedGridSquares = new List<List<GridSquare>>();

            for (int xCoord = 0; xCoord < ShapeConstants.GridWidth; xCoord++)
            {
                occupiedGridSquares.Add(new List<GridSquare>());

                for (int yCoord = 0; yCoord < ShapeConstants.GridHeight; yCoord++)
                {
                    occupiedGridSquares[xCoord].Add(new GridSquare());
                }
            }

            return occupiedGridSquares;
        }
    }
}