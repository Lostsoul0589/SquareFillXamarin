using System.Collections.Generic;
using SquareFillDomain.Builders;
using SquareFillDomain.Interfaces;
using SquareFillDomain.Models;
using SquareFillDomain.Utils;

namespace SquareFillDomain.UnitTests.TestUtils
{
    public class TestShapeSetBuilder : IShapeSetBuilder
    {
        public Shape RightHydrantShape { get { return _rightHydrantShape; } }
        public Shape SevenShape { get { return _sevenShape; } }
        public Shape FourSquareShape { get { return _fourSquareShape; } }
        public Shape LeftCornerShape { get { return _leftCornerShape; } }
        public Shape UpsideDownTShape { get { return _upsideDownTShape; } }
        public Shape ThreePoleShape { get { return _threePoleShape; } }
        public Shape TwoPoleShape { get { return _twoPoleShape; } }
        public Shape BackwardsLShape { get { return _backwardsLShape; } }
        public Shape SingleSquareShape01 { get { return _singleSquareShape01; } }

        public List<SquareFillPoint> TopRowBorderSquares { get { return _borderBuilder.TopRowBorderSquares; } }
        public List<SquareFillPoint> LeftWallBorderSquares { get { return _borderBuilder.LeftWallBorderSquares; } }
        public List<SquareFillPoint> RightWallBorderSquares { get { return _borderBuilder.RightWallBorderSquares; } }
        public List<SquareFillPoint> BottomLeftBorderSquares { get { return _borderBuilder.BottomLeftBorderSquares; } }
        public List<SquareFillPoint> BottomRightBorderSquares { get { return _borderBuilder.BottomRightBorderSquares; } }

        private readonly BorderBuilder _borderBuilder = new BorderBuilder();

        private Shape _rightHydrantShape;
        private Shape _sevenShape;
        private Shape _fourSquareShape;
        private Shape _leftCornerShape;
        private Shape _upsideDownTShape;
        private Shape _threePoleShape;
        private Shape _twoPoleShape;
        private Shape _backwardsLShape;
        private Shape _singleSquareShape01;
        private Shape _topLeftCornerOfContainingBorder;
        private Shape _topRightCornerOfContainingBorder;
        private Shape _bottomLeftCornerOfContainingBorder;
        private Shape _bottomRightCornerOfContainingBorder;

        public TestShapeSetBuilder(ISquareViewFactory squareViewFactory)
        {
            MakeShapes(squareViewFactory: squareViewFactory);
            _borderBuilder.BuildBorderSquares(squareWidth: TestConstants.SquareWidth, containingRectangle: TestConstants.ContainingRectangle);
        }

        public ShapeSet GetShapeSet()
        {
            return MakeTestShapeSet();
        }

        public void OccupyBorderSquares(List<List<GridSquare>> occupiedGridSquares)
        {
            _borderBuilder.OccupyBorderSquares(occupiedGridSquares: occupiedGridSquares);
        }

        private ShapeSet MakeTestShapeSet()
        {
            return new ShapeSet(shapes: new List<Shape> {
                _rightHydrantShape,
                _sevenShape,
                _fourSquareShape,
                _leftCornerShape,
                _upsideDownTShape,
                _threePoleShape,
                _twoPoleShape,
                _backwardsLShape,
                _singleSquareShape01,
                _topLeftCornerOfContainingBorder,
                _topRightCornerOfContainingBorder,
                _bottomLeftCornerOfContainingBorder,
                _bottomRightCornerOfContainingBorder
			});
        }

        public List<List<GridSquare>> MakeGridSquares()
        {
            return TestConstants.MakeGridSquares();
        }

        private void MakeShapes(ISquareViewFactory squareViewFactory)
        {
            var originX = TestConstants.SquareWidth / 2;
            var originY = TestConstants.SquareWidth / 2;

            // 1:
            _rightHydrantShape = new Shape(colour: SquareFillColour.Red,
                topLeftCorner: new SquareFillPoint(x: 3 * TestConstants.SquareWidth, y: TestConstants.SquareWidth),
                relativePointsTopLeftCorner: ShapeConstants.RightHydrantPoints,
                squareFactory: squareViewFactory);

            // 3:
            _sevenShape = new Shape(colour: SquareFillColour.Black,
                topLeftCorner: new SquareFillPoint(x: 9 * TestConstants.SquareWidth, y: TestConstants.SquareWidth),
                relativePointsTopLeftCorner: ShapeConstants.SevenPoints,
                squareFactory: squareViewFactory);

            // 4:
            _fourSquareShape = new Shape(colour: SquareFillColour.Orange,
                topLeftCorner: new SquareFillPoint(x: 6 * TestConstants.SquareWidth,
                    y: 2 * TestConstants.SquareWidth),
                relativePointsTopLeftCorner: ShapeConstants.FourSquarePoints,
                squareFactory: squareViewFactory);

            // 5:
            _leftCornerShape = new Shape(colour: SquareFillColour.Green,
                topLeftCorner: new SquareFillPoint(x: 7 * TestConstants.SquareWidth,
                    y: 15 * TestConstants.SquareWidth),
                relativePointsTopLeftCorner: ShapeConstants.LeftCornerPoints,
                squareFactory: squareViewFactory);

            // 7:
            _upsideDownTShape = new Shape(colour: SquareFillColour.Purple,
                topLeftCorner: new SquareFillPoint(x: 3 * TestConstants.SquareWidth,
                    y: 17 * TestConstants.SquareWidth),
                relativePointsTopLeftCorner: ShapeConstants.UpsideDownTPoints,
                squareFactory: squareViewFactory);

            // 8:
            _threePoleShape = new Shape(colour: SquareFillColour.Magenta,
                topLeftCorner: new SquareFillPoint(x: 0, y: 16 * TestConstants.SquareWidth),
                relativePointsTopLeftCorner: ShapeConstants.ThreePolePoints,
                squareFactory: squareViewFactory);

            // 9:
            _twoPoleShape = new Shape(colour: SquareFillColour.Brown,
                topLeftCorner: new SquareFillPoint(x: 6 * TestConstants.SquareWidth,
                    y: 17 * TestConstants.SquareWidth),
                relativePointsTopLeftCorner: ShapeConstants.TwoPolePoints,
                squareFactory: squareViewFactory);

            // 11:
            _backwardsLShape = new Shape(colour: SquareFillColour.DarkGrey,
                topLeftCorner: new SquareFillPoint(x: TestConstants.SquareWidth, y: 5 * TestConstants.SquareWidth),
                relativePointsTopLeftCorner: ShapeConstants.BackwardsLPoints,
                squareFactory: squareViewFactory);

            // 14:
            _singleSquareShape01 = new Shape(colour: SquareFillColour.LightGrey,
                topLeftCorner: new SquareFillPoint(x: 9 * TestConstants.SquareWidth,
                    y: 18 * TestConstants.SquareWidth),
                relativePointsTopLeftCorner: ShapeConstants.SingleSquarePoints,
                squareFactory: squareViewFactory);

            // created purely for test:
            _topLeftCornerOfContainingBorder = new Shape(colour: SquareFillColour.LightGrey,
                topLeftCorner: new SquareFillPoint(x: 9 * TestConstants.SquareWidth,
                    y: 18 * TestConstants.SquareWidth),
                relativePointsTopLeftCorner: ShapeConstants.SingleSquarePoints,
                squareFactory: squareViewFactory);

            // created purely for test:
            _topRightCornerOfContainingBorder = new Shape(colour: SquareFillColour.LightGrey,
                topLeftCorner: new SquareFillPoint(x: 9 * TestConstants.SquareWidth,
                    y: 18 * TestConstants.SquareWidth),
                relativePointsTopLeftCorner: ShapeConstants.SingleSquarePoints,
                squareFactory: squareViewFactory);

            // created purely for test:
            _bottomLeftCornerOfContainingBorder = new Shape(colour: SquareFillColour.LightGrey,
                topLeftCorner: new SquareFillPoint(x: 9 * TestConstants.SquareWidth,
                    y: 18 * TestConstants.SquareWidth),
                relativePointsTopLeftCorner: ShapeConstants.SingleSquarePoints,
                squareFactory: squareViewFactory);

            // created purely for test:
            _bottomRightCornerOfContainingBorder = new Shape(colour: SquareFillColour.LightGrey,
                topLeftCorner: new SquareFillPoint(x: 9 * TestConstants.SquareWidth,
                    y: 18 * TestConstants.SquareWidth),
                relativePointsTopLeftCorner: ShapeConstants.SingleSquarePoints,
                squareFactory: squareViewFactory);
        }
    }
}
