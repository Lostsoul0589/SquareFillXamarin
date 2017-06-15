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
        public Shape FourSquareShape { get { return _fourSquareShape; } }
        public Shape LeftCornerShape { get { return _leftCornerShape; } }
        public Shape SingleSquareShape { get { return _singleSquareShape; } }

        // public var TopRowBorderSquares: [SquareFillPoint] { get { return _borderBuilder.TopRowBorderSquares; } }
        public List<SquareFillPoint> TopRowBorderSquares { get { return _borderBuilder.TopRowBorderSquares; } }
        public List<SquareFillPoint> LeftWallBorderSquares { get { return _borderBuilder.LeftWallBorderSquares; } }
        public List<SquareFillPoint> RightWallBorderSquares { get { return _borderBuilder.RightWallBorderSquares; } }
        public List<SquareFillPoint> BottomLeftBorderSquares { get { return _borderBuilder.BottomLeftBorderSquares; } }
        public List<SquareFillPoint> BottomRightBorderSquares { get { return _borderBuilder.BottomRightBorderSquares; } }

        private readonly BorderBuilder _borderBuilder = new BorderBuilder();

        // These all have to be initialised in XCode to keep Swift happy 
        // (which means they have to be of type Shape!, and then initialised to nil).
        private Shape _rightHydrantShape;
        private Shape _fourSquareShape;
        private Shape _leftCornerShape;
        private Shape _singleSquareShape;
        private Shape _topLeftCornerOfContainingBorder;
        private Shape _topRightCornerOfContainingBorder;
        private Shape _bottomLeftCornerOfContainingBorder;
        private Shape _bottomRightCornerOfContainingBorder;

        // init(squareViewFactory: ISquareViewFactory)
        public TestShapeSetBuilder(ISquareViewFactory squareViewFactory)
        {
            _borderBuilder.BuildBorderSquares(squareWidth: TestConstants.SquareWidth, containingRectangle: TestConstants.ContainingRectangle);
            MakeShapes(squareViewFactory: squareViewFactory);
        }

        // public func GetShapeSet() -> ShapeSet
        public ShapeSet GetShapeSet()
        {
            return MakeTestShapeSet();
        }

        // public func OccupyBorderSquares(occupiedGridSquares: Grid)
        public void OccupyBorderSquares(Grid occupiedGridSquares)
        {
            _borderBuilder.OccupyBorderSquares(occupiedGridSquares: occupiedGridSquares);
        }

        // public func MakeGridSquares() -> Grid
        public Grid MakeGridSquares()
        {
            return TestConstants.MakeGridSquares();
        }

        // public static func MakeSquares(
        //      colour: SquareFillColour,
        //      relativePointsTopLeftCorner: [SquareFillPoint],
        //      squareFactory: ISquareViewFactory) -> [Square]
        public List<Square> MakeSquares(
            SquareFillColour colour,
            List<SquareFillPoint> relativePointsTopLeftCorner,
            ISquareViewFactory squareFactory)
        {
            var squares = new List<Square>();
            foreach (var element in relativePointsTopLeftCorner)
            {
                squares.Add(new Square(
                    positionRelativeToParentCorner: element,
                    sprite: squareFactory.MakeSquare(colour: colour)));
            }

            return squares;
        }

        // private func MakeTestShapeSet() -> ShapeSet
        private ShapeSet MakeTestShapeSet()
        {
            var shapeList = new List<Shape> {
                _rightHydrantShape,
                _fourSquareShape,
                _leftCornerShape,
                _singleSquareShape,
                _topLeftCornerOfContainingBorder,
                _topRightCornerOfContainingBorder,
                _bottomLeftCornerOfContainingBorder,
                _bottomRightCornerOfContainingBorder
            };
            return new ShapeSet(shapes: shapeList);
        }

        // private func MakeShapes(squareViewFactory: ISquareViewFactory)
        private void MakeShapes(ISquareViewFactory squareViewFactory)
        {
            // 1:
            _rightHydrantShape = MakeShape(colour: SquareFillColour.Red,
                topLeftCorner: SquareFillPoint(x: 3, y: 1),
                relativePointsTopLeftCorner: ShapeConstants.RightHydrantPoints,
                squareFactory: squareViewFactory);

            // 4:
            _fourSquareShape = MakeShape(colour: SquareFillColour.Orange,
                topLeftCorner: SquareFillPoint(x: 6, y: 2),
                relativePointsTopLeftCorner: ShapeConstants.FourSquarePoints,
                squareFactory: squareViewFactory);

            // 5:
            _leftCornerShape = MakeShape(colour: SquareFillColour.Green,
                topLeftCorner: SquareFillPoint(x: 7, y: 15),
                relativePointsTopLeftCorner: ShapeConstants.LeftCornerPoints,
                squareFactory: squareViewFactory);

            // 14:
            _singleSquareShape = MakeShape(colour: SquareFillColour.LightGrey,
                topLeftCorner: SquareFillPoint(x: 9, y: 18),
                relativePointsTopLeftCorner: ShapeConstants.SingleSquarePoints,
                squareFactory: squareViewFactory);

            // created purely for test:
            _topLeftCornerOfContainingBorder = MakeShape(colour: SquareFillColour.LightGrey,
                topLeftCorner: SquareFillPoint(x: TopRowBorderSquares[0].X, y: TopRowBorderSquares[0].Y),
                relativePointsTopLeftCorner: ShapeConstants.SingleSquarePoints,
                squareFactory: squareViewFactory);

            // created purely for test:
            _topRightCornerOfContainingBorder = MakeShape(colour: SquareFillColour.LightGrey,
                topLeftCorner: SquareFillPoint(
                    x: TopRowBorderSquares[TopRowBorderSquares.Count - 1].X, 
                    y: TopRowBorderSquares[TopRowBorderSquares.Count - 1].Y),
                relativePointsTopLeftCorner: ShapeConstants.SingleSquarePoints,
                squareFactory: squareViewFactory);

            // created purely for test:
            _bottomLeftCornerOfContainingBorder = MakeShape(colour: SquareFillColour.LightGrey,
                topLeftCorner: SquareFillPoint(x: BottomLeftBorderSquares[0].X, y: BottomLeftBorderSquares[0].Y),
                relativePointsTopLeftCorner: ShapeConstants.SingleSquarePoints,
                squareFactory: squareViewFactory);

            // created purely for test:
            _bottomRightCornerOfContainingBorder = MakeShape(colour: SquareFillColour.LightGrey,
                topLeftCorner: SquareFillPoint(
                    x: BottomRightBorderSquares[BottomRightBorderSquares.Count - 1].X, 
                    y: BottomRightBorderSquares[BottomRightBorderSquares.Count - 1].Y),
                relativePointsTopLeftCorner: ShapeConstants.SingleSquarePoints,
                squareFactory: squareViewFactory);
        }

        // private func MakeShape(
        //      colour: SquareFillColour,
        //      topLeftCorner: SquareFillPoint,
        //      relativePointsTopLeftCorner: [SquareFillPoint],
        //      squareFactory: ISquareViewFactory) -> Shape
        private Shape MakeShape(
            SquareFillColour colour,
            SquareFillPoint topLeftCorner,
            List<SquareFillPoint> relativePointsTopLeftCorner,
            ISquareViewFactory squareFactory)
        {
            var squares = MakeSquares(
                colour: colour,
                relativePointsTopLeftCorner: relativePointsTopLeftCorner,
                squareFactory: squareFactory);

            return new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: squares);
        }

        private SquareFillPoint SquareFillPoint(int x, int y)
        {
            return new SquareFillPoint(x: x, y: y);
        }
    }
}
