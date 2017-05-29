using System.Collections.Generic;
using SquareFillDomain.Models;
using SquareFillDomain.Utils;

namespace SquareFillDomain.UnitTests.TestUtils
{
    public class TestConstants
    {
        public static int SquareWidth = ShapeConstants.SquareWidth;

        public static SquareFillRect ContainingRectangle = ShapeConstants.ContainingRectangle;

        public static SquareFillPoint CentreOfTopLeftGridSquare = ShapeConstants.CentreOfTopLeftGridSquare;
        public static SquareFillPoint TopLeftGridSquare = ShapeConstants.TopLeftGridSquare;

        public static int GridWidth = ShapeConstants.GridWidth;
        public static int GridHeight = ShapeConstants.GridHeight;
        public static int ScreenWidth = ShapeConstants.ScreenWidth;
        public static int ScreenHeight = ShapeConstants.ScreenHeight;

        public static List<List<GridSquare>> MakeGridSquares()
        {
            return ShapeConstants.MakeGridSquares();
        }

        public static List<SquareFillPoint> RightHydrantCentrePoints = ShapeConstants.RightHydrantCentrePoints;
        public static List<SquareFillPoint> FourBarCentrePoints = ShapeConstants.FourBarCentrePoints;
        public static List<SquareFillPoint> SevenCentrePoints = ShapeConstants.SevenCentrePoints;
        public static List<SquareFillPoint> FourSquareCentrePoints = ShapeConstants.FourSquareCentrePoints;
        public static List<SquareFillPoint> LeftCornerCentrePoints = ShapeConstants.LeftCornerCentrePoints;
        public static List<SquareFillPoint> UpsideDownTCentrePoints = ShapeConstants.UpsideDownTCentrePoints;
        public static List<SquareFillPoint> ThreePoleCentrePoints = ShapeConstants.ThreePoleCentrePoints;
        public static List<SquareFillPoint> TwoPoleCentrePoints = ShapeConstants.TwoPoleCentrePoints;
        public static List<SquareFillPoint> BackwardsLCentrePoints = ShapeConstants.BackwardsLCentrePoints;
        public static List<SquareFillPoint> SingleSquareCentrePoints = ShapeConstants.SingleSquareCentrePoints;

        public static List<SquareFillPoint> LeftHydrantCentrePoints = ShapeConstants.LeftHydrantCentrePoints;
        public static List<SquareFillPoint> RightWayUpTCentrePoints = ShapeConstants.RightWayUpTCentrePoints;
        public static List<SquareFillPoint> NineSquareCentrePoints = ShapeConstants.NineSquareCentrePoints;

        public static List<SquareFillPoint> RightHydrantPoints = ShapeConstants.RightHydrantPoints;
        public static List<SquareFillPoint> FourBarPoints = ShapeConstants.FourBarPoints;
        public static List<SquareFillPoint> SevenPoints = ShapeConstants.SevenPoints;
        public static List<SquareFillPoint> FourSquarePoints = ShapeConstants.FourSquarePoints;
        public static List<SquareFillPoint> LeftCornerPoints = ShapeConstants.LeftCornerPoints;
        public static List<SquareFillPoint> UpsideDownTPoints = ShapeConstants.UpsideDownTPoints;
        public static List<SquareFillPoint> ThreePolePoints = ShapeConstants.ThreePolePoints;
        public static List<SquareFillPoint> TwoPolePoints = ShapeConstants.TwoPolePoints;
        public static List<SquareFillPoint> BackwardsLPoints = ShapeConstants.BackwardsLPoints;
        public static List<SquareFillPoint> SingleSquarePoints = ShapeConstants.SingleSquarePoints;

        public static List<SquareFillPoint> LeftHydrantPoints = ShapeConstants.LeftHydrantPoints;
        public static List<SquareFillPoint> RightWayUpTPoints = ShapeConstants.RightWayUpTPoints;
        public static List<SquareFillPoint> NineSquarePoints = ShapeConstants.NineSquarePoints;
    }
}