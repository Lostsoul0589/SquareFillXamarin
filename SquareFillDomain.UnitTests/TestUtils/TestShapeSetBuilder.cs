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
            _borderBuilder.BuildBorderSquares(squareWidth: TestConstants.SquareWidth, containingRectangle: TestConstants.ContainingRectangle);
            MakeShapes(squareViewFactory: squareViewFactory);
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
            // 1:
            _rightHydrantShape = new Shape(colour: SquareFillColour.Red,
                topLeftCorner: new SquareFillPoint(x: 3, y: 1),
                relativePointsTopLeftCorner: ShapeConstants.RightHydrantPoints,
                squareFactory: squareViewFactory,
                topLeftCornerIsInPixels: false);

            // 3:
            _sevenShape = new Shape(colour: SquareFillColour.Black,
                topLeftCorner: new SquareFillPoint(x: 9, y: 1),
                relativePointsTopLeftCorner: ShapeConstants.SevenPoints,
                squareFactory: squareViewFactory,
                topLeftCornerIsInPixels: false);

            // 4:
            _fourSquareShape = new Shape(colour: SquareFillColour.Orange,
                topLeftCorner: new SquareFillPoint(x: 6, y: 2),
                relativePointsTopLeftCorner: ShapeConstants.FourSquarePoints,
                squareFactory: squareViewFactory,
                topLeftCornerIsInPixels: false);

            // 5:
            _leftCornerShape = new Shape(colour: SquareFillColour.Green,
                topLeftCorner: new SquareFillPoint(x: 7, y: 15),
                relativePointsTopLeftCorner: ShapeConstants.LeftCornerPoints,
                squareFactory: squareViewFactory,
                topLeftCornerIsInPixels: false);

            // 7:
            _upsideDownTShape = new Shape(colour: SquareFillColour.Purple,
                topLeftCorner: new SquareFillPoint(x: 3, y: 17),
                relativePointsTopLeftCorner: ShapeConstants.UpsideDownTPoints,
                squareFactory: squareViewFactory,
                topLeftCornerIsInPixels: false);

            // 8:
            _threePoleShape = new Shape(colour: SquareFillColour.Magenta,
                topLeftCorner: new SquareFillPoint(x: 0, y: 16),
                relativePointsTopLeftCorner: ShapeConstants.ThreePolePoints,
                squareFactory: squareViewFactory,
                topLeftCornerIsInPixels: false);

            // 9:
            _twoPoleShape = new Shape(colour: SquareFillColour.Brown,
                topLeftCorner: new SquareFillPoint(x: 6, y: 17),
                relativePointsTopLeftCorner: ShapeConstants.TwoPolePoints,
                squareFactory: squareViewFactory,
                topLeftCornerIsInPixels: false);

            // 11:
            _backwardsLShape = new Shape(colour: SquareFillColour.DarkGrey,
                topLeftCorner: new SquareFillPoint(x: 1, y: 5),
                relativePointsTopLeftCorner: ShapeConstants.BackwardsLPoints,
                squareFactory: squareViewFactory,
                topLeftCornerIsInPixels: false);

            // 14:
            _singleSquareShape01 = new Shape(colour: SquareFillColour.LightGrey,
                topLeftCorner: new SquareFillPoint(x: 9, y: 18),
                relativePointsTopLeftCorner: ShapeConstants.SingleSquarePoints,
                squareFactory: squareViewFactory,
                topLeftCornerIsInPixels: false);

            // created purely for test:
            _topLeftCornerOfContainingBorder = new Shape(colour: SquareFillColour.LightGrey,
                topLeftCorner: new SquareFillPoint(x: TopRowBorderSquares[0].X, y: TopRowBorderSquares[0].Y),
                relativePointsTopLeftCorner: ShapeConstants.SingleSquarePoints,
                squareFactory: squareViewFactory,
                topLeftCornerIsInPixels: false);

            // created purely for test:
            _topRightCornerOfContainingBorder = new Shape(colour: SquareFillColour.LightGrey,
                topLeftCorner: new SquareFillPoint(
                    x: TopRowBorderSquares[TopRowBorderSquares.Count - 1].X, 
                    y: TopRowBorderSquares[TopRowBorderSquares.Count - 1].Y),
                relativePointsTopLeftCorner: ShapeConstants.SingleSquarePoints,
                squareFactory: squareViewFactory,
                topLeftCornerIsInPixels: false);

            // created purely for test:
            _bottomLeftCornerOfContainingBorder = new Shape(colour: SquareFillColour.LightGrey,
                topLeftCorner: new SquareFillPoint(x: BottomLeftBorderSquares[0].X, y: BottomLeftBorderSquares[0].Y),
                relativePointsTopLeftCorner: ShapeConstants.SingleSquarePoints,
                squareFactory: squareViewFactory,
                topLeftCornerIsInPixels: false);

            // created purely for test:
            _bottomRightCornerOfContainingBorder = new Shape(colour: SquareFillColour.LightGrey,
                topLeftCorner: new SquareFillPoint(
                    x: BottomRightBorderSquares[BottomRightBorderSquares.Count - 1].X, 
                    y: BottomRightBorderSquares[BottomRightBorderSquares.Count - 1].Y),
                relativePointsTopLeftCorner: ShapeConstants.SingleSquarePoints,
                squareFactory: squareViewFactory,
                topLeftCornerIsInPixels: false);
        }
    }
}
