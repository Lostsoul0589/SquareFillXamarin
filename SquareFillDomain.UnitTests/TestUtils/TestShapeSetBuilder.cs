using System.Collections.Generic;
using SquareFillDomain.Builders;
using SquareFillDomain.Interfaces;
using SquareFillDomain.Models;
using SquareFillDomain.Utils;

namespace SquareFillDomain.UnitTests.TestUtils
{
    public class TestShapeSetBuilder : IShapeSetBuilder
    {
        public static Shape RightHydrantShape01;
        public static Shape RightHydrantShape02;
        public static Shape RightHydrantShape03;
        public static Shape FourBarShape;
        public static Shape SevenShape;
        public static Shape FourSquareShape01;
        public static Shape FourSquareShape02;
        public static Shape LeftCornerShape;
        public static Shape UpsideDownTShape01;
        public static Shape UpsideDownTShape02;
        public static Shape ThreePoleShape;
        public static Shape TwoPoleShape;
        public static Shape BackwardsLShape;
        public static Shape SingleSquareShape01;

        private static readonly List<SquareFillPoint> BorderSquares = new List<SquareFillPoint>();

        public TestShapeSetBuilder(ISquareViewFactory squareViewFactory)
        {
            MakeShapes(squareViewFactory: squareViewFactory);
        }

        public ShapeSet GetShapeSet()
        {
            return MakeTestShapeSet();
        }

        public void OccupyBorderSquares(List<List<GridSquare>> occupiedGridSquares)
        {
            BuildBorderSquares();
            foreach (var borderSquare in BorderSquares)
            {
                occupiedGridSquares[borderSquare.X][borderSquare.Y].Occupied = true;
            }
        }

        private ShapeSet MakeTestShapeSet()
        {
            return new ShapeSet(shapes: new List<Shape> {
                RightHydrantShape01,
			    FourBarShape,
                SevenShape,
                FourSquareShape01,
                LeftCornerShape,
                RightHydrantShape02,
			    UpsideDownTShape01,
                ThreePoleShape,
                TwoPoleShape,
                FourSquareShape02,
                BackwardsLShape,
                RightHydrantShape03,
                UpsideDownTShape02,
                SingleSquareShape01
			});
        }

        private static void BuildBorderSquares()
        {
            BorderSquares.Add(new SquareFillPoint(x: 3, y: 5));
            BorderSquares.Add(new SquareFillPoint(x: 4, y: 5));
            BorderSquares.Add(new SquareFillPoint(x: 5, y: 5));
            BorderSquares.Add(new SquareFillPoint(x: 6, y: 5));
            BorderSquares.Add(new SquareFillPoint(x: 7, y: 5));
            BorderSquares.Add(new SquareFillPoint(x: 8, y: 5));
            BorderSquares.Add(new SquareFillPoint(x: 9, y: 5));
            BorderSquares.Add(new SquareFillPoint(x: 10, y: 5));
            BorderSquares.Add(new SquareFillPoint(x: 11, y: 5));
            BorderSquares.Add(new SquareFillPoint(x: 11, y: 6));
            BorderSquares.Add(new SquareFillPoint(x: 11, y: 7));
            BorderSquares.Add(new SquareFillPoint(x: 11, y: 8));
            BorderSquares.Add(new SquareFillPoint(x: 11, y: 9));
            BorderSquares.Add(new SquareFillPoint(x: 11, y: 10));
            BorderSquares.Add(new SquareFillPoint(x: 11, y: 11));
            BorderSquares.Add(new SquareFillPoint(x: 11, y: 12));
            BorderSquares.Add(new SquareFillPoint(x: 11, y: 13));
            BorderSquares.Add(new SquareFillPoint(x: 10, y: 13));
            BorderSquares.Add(new SquareFillPoint(x: 4, y: 13));
            BorderSquares.Add(new SquareFillPoint(x: 3, y: 13));
            BorderSquares.Add(new SquareFillPoint(x: 3, y: 12));
            BorderSquares.Add(new SquareFillPoint(x: 3, y: 11));
            BorderSquares.Add(new SquareFillPoint(x: 3, y: 10));
            BorderSquares.Add(new SquareFillPoint(x: 3, y: 9));
            BorderSquares.Add(new SquareFillPoint(x: 3, y: 8));
            BorderSquares.Add(new SquareFillPoint(x: 3, y: 7));
            BorderSquares.Add(new SquareFillPoint(x: 3, y: 6));
        }

        private void MakeShapes(ISquareViewFactory squareViewFactory)
        {
            var originX = ShapeConstants.SquareWidth / 2;
            var originY = ShapeConstants.SquareWidth / 2;

            // 1:
            RightHydrantShape01 = new Shape(colour: SquareFillColour.Red,
                centreOfShape: new SquareFillPoint(x: originX + 3 * ShapeConstants.SquareWidth,
                    y: originY + 2 * ShapeConstants.SquareWidth),
                topLeftCorner: new SquareFillPoint(x: 3 * ShapeConstants.SquareWidth, y: ShapeConstants.SquareWidth),
                relativePoints: ShapeConstants.RightHydrantCentrePoints,
                relativePointsTopLeftCorner: ShapeConstants.RightHydrantPoints,
                squareFactory: squareViewFactory);

            // 2:
            FourBarShape = new Shape(colour: SquareFillColour.Blue,
                centreOfShape: new SquareFillPoint(x: originX + 3 * ShapeConstants.SquareWidth,
                    y: originY + 15 * ShapeConstants.SquareWidth),
                topLeftCorner: new SquareFillPoint(x: 2 * ShapeConstants.SquareWidth,
                    y: 15 * ShapeConstants.SquareWidth),
                relativePoints: ShapeConstants.FourBarCentrePoints,
                relativePointsTopLeftCorner: ShapeConstants.FourBarPoints,
                squareFactory: squareViewFactory);

            // 3:
            SevenShape = new Shape(colour: SquareFillColour.Black,
                centreOfShape: new SquareFillPoint(x: originX + 10 * ShapeConstants.SquareWidth,
                    y: originY + ShapeConstants.SquareWidth),
                topLeftCorner: new SquareFillPoint(x: 9 * ShapeConstants.SquareWidth, y: ShapeConstants.SquareWidth),
                relativePoints: ShapeConstants.SevenCentrePoints,
                relativePointsTopLeftCorner: ShapeConstants.SevenPoints,
                squareFactory: squareViewFactory);

            // 4:
            FourSquareShape01 = new Shape(colour: SquareFillColour.Orange,
                centreOfShape: new SquareFillPoint(x: originX + 7 * ShapeConstants.SquareWidth,
                    y: originY + 2 * ShapeConstants.SquareWidth),
                topLeftCorner: new SquareFillPoint(x: 6 * ShapeConstants.SquareWidth,
                    y: 2 * ShapeConstants.SquareWidth),
                relativePoints: ShapeConstants.FourSquareCentrePoints,
                relativePointsTopLeftCorner: ShapeConstants.FourSquarePoints,
                squareFactory: squareViewFactory);

            // 5:
            LeftCornerShape = new Shape(colour: SquareFillColour.Green,
                centreOfShape: new SquareFillPoint(x: originX + 8 * ShapeConstants.SquareWidth,
                    y: originY + 15 * ShapeConstants.SquareWidth),
                topLeftCorner: new SquareFillPoint(x: 7 * ShapeConstants.SquareWidth,
                    y: 15 * ShapeConstants.SquareWidth),
                relativePoints: ShapeConstants.LeftCornerCentrePoints,
                relativePointsTopLeftCorner: ShapeConstants.LeftCornerPoints,
                squareFactory: squareViewFactory);

            // 6:
            RightHydrantShape02 = new Shape(colour: SquareFillColour.Yellow,
                centreOfShape: new SquareFillPoint(x: originX, y: originY + 2 * ShapeConstants.SquareWidth),
                topLeftCorner: new SquareFillPoint(x: 0, y: ShapeConstants.SquareWidth),
                relativePoints: ShapeConstants.RightHydrantCentrePoints,
                relativePointsTopLeftCorner: ShapeConstants.RightHydrantPoints,
                squareFactory: squareViewFactory);

            // 7:
            UpsideDownTShape01 = new Shape(colour: SquareFillColour.Purple,
                centreOfShape: new SquareFillPoint(x: originX + 3 * ShapeConstants.SquareWidth,
                    y: originY + 18 * ShapeConstants.SquareWidth),
                topLeftCorner: new SquareFillPoint(x: 3 * ShapeConstants.SquareWidth,
                    y: 17 * ShapeConstants.SquareWidth),
                relativePoints: ShapeConstants.UpsideDownTCentrePoints,
                relativePointsTopLeftCorner: ShapeConstants.UpsideDownTPoints,
                squareFactory: squareViewFactory);

            // 8:
            ThreePoleShape = new Shape(colour: SquareFillColour.Magenta,
                centreOfShape: new SquareFillPoint(x: originX, y: originY + 17 * ShapeConstants.SquareWidth),
                topLeftCorner: new SquareFillPoint(x: 0, y: 16 * ShapeConstants.SquareWidth),
                relativePoints: ShapeConstants.ThreePoleCentrePoints,
                relativePointsTopLeftCorner: ShapeConstants.ThreePolePoints,
                squareFactory: squareViewFactory);

            // 9:
            TwoPoleShape = new Shape(colour: SquareFillColour.Brown,
                centreOfShape: new SquareFillPoint(x: originX + 6 * ShapeConstants.SquareWidth,
                    y: originY + 18 * ShapeConstants.SquareWidth),
                topLeftCorner: new SquareFillPoint(x: 6 * ShapeConstants.SquareWidth,
                    y: 17 * ShapeConstants.SquareWidth),
                relativePoints: ShapeConstants.TwoPoleCentrePoints,
                relativePointsTopLeftCorner: ShapeConstants.TwoPolePoints,
                squareFactory: squareViewFactory);

            // 10:
            FourSquareShape02 = new Shape(colour: SquareFillColour.Cyan,
                centreOfShape: new SquareFillPoint(x: originX + ShapeConstants.SquareWidth,
                    y: originY + 9 * ShapeConstants.SquareWidth),
                topLeftCorner: new SquareFillPoint(x: 0, y: 9 * ShapeConstants.SquareWidth),
                relativePoints: ShapeConstants.FourSquareCentrePoints,
                relativePointsTopLeftCorner: ShapeConstants.FourSquarePoints,
                squareFactory: squareViewFactory);

            // 11:
            BackwardsLShape = new Shape(colour: SquareFillColour.DarkGrey,
                centreOfShape: new SquareFillPoint(x: originX + ShapeConstants.SquareWidth,
                    y: originY + 7 * ShapeConstants.SquareWidth),
                topLeftCorner: new SquareFillPoint(x: ShapeConstants.SquareWidth, y: 5 * ShapeConstants.SquareWidth),
                relativePoints: ShapeConstants.BackwardsLCentrePoints,
                relativePointsTopLeftCorner: ShapeConstants.BackwardsLPoints,
                squareFactory: squareViewFactory);

            // 12:
            RightHydrantShape03 = new Shape(colour: SquareFillColour.Grey,
                centreOfShape: new SquareFillPoint(x: originX, y: originY + 13 * ShapeConstants.SquareWidth),
                topLeftCorner: new SquareFillPoint(x: 0, y: 12 * ShapeConstants.SquareWidth),
                relativePoints: ShapeConstants.RightHydrantCentrePoints,
                relativePointsTopLeftCorner: ShapeConstants.RightHydrantPoints,
                squareFactory: squareViewFactory);

            // 13:
            UpsideDownTShape02 = new Shape(colour: SquareFillColour.White,
                centreOfShape: new SquareFillPoint(x: originX + 11 * ShapeConstants.SquareWidth,
                    y: originY + 16 * ShapeConstants.SquareWidth),
                topLeftCorner: new SquareFillPoint(x: 11 * ShapeConstants.SquareWidth,
                    y: 15 * ShapeConstants.SquareWidth),
                relativePoints: ShapeConstants.UpsideDownTCentrePoints,
                relativePointsTopLeftCorner: ShapeConstants.UpsideDownTPoints,
                squareFactory: squareViewFactory);

            // 14:
            SingleSquareShape01 = new Shape(colour: SquareFillColour.LightGrey,
                centreOfShape: new SquareFillPoint(x: originX + 9 * ShapeConstants.SquareWidth,
                    y: originY + 18 * ShapeConstants.SquareWidth),
                topLeftCorner: new SquareFillPoint(x: 9 * ShapeConstants.SquareWidth,
                    y: 18 * ShapeConstants.SquareWidth),
                relativePoints: ShapeConstants.SingleSquareCentrePoints,
                relativePointsTopLeftCorner: ShapeConstants.SingleSquarePoints,
                squareFactory: squareViewFactory);
        }
    }
}
