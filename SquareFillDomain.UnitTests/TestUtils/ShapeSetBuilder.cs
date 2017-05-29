using System.Collections.Generic;
using SquareFillDomain.Builders;
using SquareFillDomain.Interfaces;
using SquareFillDomain.Models;
using SquareFillDomain.Utils;

namespace SquareFillDomain.UnitTests.TestUtils
{
    public class TestShapeSetBuilder : IShapeSetBuilder
    {
        private static readonly List<SquareFillPoint> _borderSquares = new List<SquareFillPoint>();

        public ShapeSet GetShapeSet(ISquareViewFactory squareViewFactory)
        {
            return MakeTestShapeSet(squareViewFactory: squareViewFactory);
        }

        public void OccupyBorderSquares(List<List<GridSquare>> occupiedGridSquares)
        {
            BuildBorderSquares();
            foreach (var borderSquare in _borderSquares)
            {
                occupiedGridSquares[borderSquare.X][borderSquare.Y].Occupied = true;
            }
        }

        private ShapeSet MakeTestShapeSet(ISquareViewFactory squareViewFactory)
        {
            var originX = ShapeConstants.SquareWidth / 2;
            var originY = ShapeConstants.SquareWidth / 2;

            return new ShapeSet(shapes: new List<Shape> {
                // 1:
                new Shape(colour: SquareFillColour.Red,
					      centreOfShape: new SquareFillPoint(x:originX + 3*ShapeConstants.SquareWidth, y:originY + 2*ShapeConstants.SquareWidth),
                          topLeftCorner: new SquareFillPoint(x:3*ShapeConstants.SquareWidth, y:ShapeConstants.SquareWidth),
                          relativePoints: ShapeConstants.RightHydrantCentrePoints,
                          relativePointsTopLeftCorner: ShapeConstants.RightHydrantPoints,
                          squareFactory: squareViewFactory),
			    // 2:
                new Shape(colour: SquareFillColour.Blue,
						  centreOfShape: new SquareFillPoint(x:originX + 3*ShapeConstants.SquareWidth, y:originY + 15*ShapeConstants.SquareWidth),
                          topLeftCorner: new SquareFillPoint(x:2*ShapeConstants.SquareWidth, y:15*ShapeConstants.SquareWidth),
                          relativePoints: ShapeConstants.FourBarCentrePoints,
                          relativePointsTopLeftCorner: ShapeConstants.FourBarPoints,
                          squareFactory: squareViewFactory),
                // 3:
                new Shape(colour: SquareFillColour.Black,
						  centreOfShape: new SquareFillPoint(x:originX + 10*ShapeConstants.SquareWidth, y:originY + ShapeConstants.SquareWidth),
                          topLeftCorner: new SquareFillPoint(x:9*ShapeConstants.SquareWidth, y:ShapeConstants.SquareWidth),
                          relativePoints: ShapeConstants.SevenCentrePoints,
                          relativePointsTopLeftCorner: ShapeConstants.SevenPoints,
					      squareFactory: squareViewFactory),
                // 4:
                new Shape(colour: SquareFillColour.Orange,
						  centreOfShape: new SquareFillPoint(x:originX + 7*ShapeConstants.SquareWidth, y:originY + 2*ShapeConstants.SquareWidth),
                          topLeftCorner: new SquareFillPoint(x:6*ShapeConstants.SquareWidth, y:2*ShapeConstants.SquareWidth),
                          relativePoints: ShapeConstants.FourSquareCentrePoints,
                          relativePointsTopLeftCorner: ShapeConstants.FourSquarePoints,
                          squareFactory: squareViewFactory),
                // 5:
                new Shape(colour: SquareFillColour.Green,
					      centreOfShape: new SquareFillPoint(x:originX + 8*ShapeConstants.SquareWidth, y:originY + 15*ShapeConstants.SquareWidth),
                          topLeftCorner: new SquareFillPoint(x:7*ShapeConstants.SquareWidth, y:15*ShapeConstants.SquareWidth),
                          relativePoints: ShapeConstants.LeftCornerCentrePoints,
                          relativePointsTopLeftCorner: ShapeConstants.LeftCornerPoints,
                          squareFactory: squareViewFactory),
                // 6:
                new Shape(colour: SquareFillColour.Yellow,
					      centreOfShape: new SquareFillPoint(x:originX, y:originY + 2*ShapeConstants.SquareWidth),
                          topLeftCorner: new SquareFillPoint(x:0, y:ShapeConstants.SquareWidth),
                          relativePoints: ShapeConstants.RightHydrantCentrePoints,
                          relativePointsTopLeftCorner: ShapeConstants.RightHydrantPoints,
                          squareFactory: squareViewFactory),
			    // 7:
                new Shape(colour: SquareFillColour.Purple,
					      centreOfShape: new SquareFillPoint(x:originX + 3*ShapeConstants.SquareWidth, y:originY + 18*ShapeConstants.SquareWidth),
                          topLeftCorner: new SquareFillPoint(x:3*ShapeConstants.SquareWidth, y:17*ShapeConstants.SquareWidth),
                          relativePoints: ShapeConstants.UpsideDownTCentrePoints,
                          relativePointsTopLeftCorner: ShapeConstants.UpsideDownTPoints,
                          squareFactory: squareViewFactory),
                // 8:
                new Shape(colour: SquareFillColour.Magenta,
						  centreOfShape: new SquareFillPoint(x:originX, y:originY + 17*ShapeConstants.SquareWidth),
                          topLeftCorner: new SquareFillPoint(x:0, y:16*ShapeConstants.SquareWidth),
                          relativePoints: ShapeConstants.ThreePoleCentrePoints,
                          relativePointsTopLeftCorner: ShapeConstants.ThreePolePoints,
                          squareFactory: squareViewFactory),
                // 9:
                new Shape(colour: SquareFillColour.Brown,
						  centreOfShape: new SquareFillPoint(x:originX + 6*ShapeConstants.SquareWidth, y:originY + 18*ShapeConstants.SquareWidth),
                          topLeftCorner: new SquareFillPoint(x:6*ShapeConstants.SquareWidth, y:17*ShapeConstants.SquareWidth),
                          relativePoints: ShapeConstants.TwoPoleCentrePoints,
                          relativePointsTopLeftCorner: ShapeConstants.TwoPolePoints,
                          squareFactory: squareViewFactory),
                // 10:
                new Shape(colour: SquareFillColour.Cyan,
						  centreOfShape: new SquareFillPoint(x:originX + ShapeConstants.SquareWidth, y:originY + 9*ShapeConstants.SquareWidth),
                          topLeftCorner: new SquareFillPoint(x:0, y:9*ShapeConstants.SquareWidth),
                          relativePoints: ShapeConstants.FourSquareCentrePoints,
                          relativePointsTopLeftCorner: ShapeConstants.FourSquarePoints,
                          squareFactory: squareViewFactory),
                // 11:
                new Shape(colour: SquareFillColour.DarkGrey,
					      centreOfShape: new SquareFillPoint(x:originX + ShapeConstants.SquareWidth, y:originY + 7*ShapeConstants.SquareWidth),
                          topLeftCorner: new SquareFillPoint(x:ShapeConstants.SquareWidth, y:5*ShapeConstants.SquareWidth),
                          relativePoints: ShapeConstants.BackwardsLCentrePoints,
                          relativePointsTopLeftCorner: ShapeConstants.BackwardsLPoints,
                          squareFactory: squareViewFactory),
                // 12:
                new Shape(colour: SquareFillColour.Grey,
					      centreOfShape: new SquareFillPoint(x:originX, y:originY + 13*ShapeConstants.SquareWidth),
                          topLeftCorner: new SquareFillPoint(x:0, y:12*ShapeConstants.SquareWidth),
                          relativePoints: ShapeConstants.RightHydrantCentrePoints,
                          relativePointsTopLeftCorner: ShapeConstants.RightHydrantPoints,
                          squareFactory: squareViewFactory),
                // 13:
                new Shape(colour: SquareFillColour.White,
						  centreOfShape: new SquareFillPoint(x:originX + 11*ShapeConstants.SquareWidth, y:originY + 16*ShapeConstants.SquareWidth),
                          topLeftCorner: new SquareFillPoint(x:11*ShapeConstants.SquareWidth, y:15*ShapeConstants.SquareWidth),
                          relativePoints: ShapeConstants.UpsideDownTCentrePoints,
                          relativePointsTopLeftCorner: ShapeConstants.UpsideDownTPoints,
                          squareFactory: squareViewFactory),
                // 14:
                new Shape(colour: SquareFillColour.LightGrey,
                          centreOfShape: new SquareFillPoint(x:originX + 9*ShapeConstants.SquareWidth, y:originY + 18*ShapeConstants.SquareWidth),
                          topLeftCorner: new SquareFillPoint(x:9*ShapeConstants.SquareWidth, y:18*ShapeConstants.SquareWidth),
                          relativePoints: ShapeConstants.SingleSquareCentrePoints,
                          relativePointsTopLeftCorner: ShapeConstants.SingleSquarePoints,
                          squareFactory: squareViewFactory)
			});
        }

        private static void BuildBorderSquares()
        {
            _borderSquares.Add(new SquareFillPoint(x: 3, y: 5));
            _borderSquares.Add(new SquareFillPoint(x: 4, y: 5));
            _borderSquares.Add(new SquareFillPoint(x: 5, y: 5));
            _borderSquares.Add(new SquareFillPoint(x: 6, y: 5));
            _borderSquares.Add(new SquareFillPoint(x: 7, y: 5));
            _borderSquares.Add(new SquareFillPoint(x: 8, y: 5));
            _borderSquares.Add(new SquareFillPoint(x: 9, y: 5));
            _borderSquares.Add(new SquareFillPoint(x: 10, y: 5));
            _borderSquares.Add(new SquareFillPoint(x: 11, y: 5));
            _borderSquares.Add(new SquareFillPoint(x: 11, y: 6));
            _borderSquares.Add(new SquareFillPoint(x: 11, y: 7));
            _borderSquares.Add(new SquareFillPoint(x: 11, y: 8));
            _borderSquares.Add(new SquareFillPoint(x: 11, y: 9));
            _borderSquares.Add(new SquareFillPoint(x: 11, y: 10));
            _borderSquares.Add(new SquareFillPoint(x: 11, y: 11));
            _borderSquares.Add(new SquareFillPoint(x: 11, y: 12));
            _borderSquares.Add(new SquareFillPoint(x: 11, y: 13));
            _borderSquares.Add(new SquareFillPoint(x: 10, y: 13));
            _borderSquares.Add(new SquareFillPoint(x: 4, y: 13));
            _borderSquares.Add(new SquareFillPoint(x: 3, y: 13));
            _borderSquares.Add(new SquareFillPoint(x: 3, y: 12));
            _borderSquares.Add(new SquareFillPoint(x: 3, y: 11));
            _borderSquares.Add(new SquareFillPoint(x: 3, y: 10));
            _borderSquares.Add(new SquareFillPoint(x: 3, y: 9));
            _borderSquares.Add(new SquareFillPoint(x: 3, y: 8));
            _borderSquares.Add(new SquareFillPoint(x: 3, y: 7));
            _borderSquares.Add(new SquareFillPoint(x: 3, y: 6));
        }
    }
}
