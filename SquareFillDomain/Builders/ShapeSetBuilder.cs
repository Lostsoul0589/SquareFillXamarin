using System.Collections.Generic;
using SquareFillDomain.Interfaces;
using SquareFillDomain.Models;
using SquareFillDomain.Utils;

namespace SquareFillDomain.Builders
{
    public class ShapeSetBuilder : IShapeSetBuilder
    {
        private static Shape _rightHydrantShape01;
        private static Shape _rightHydrantShape02;
        private static Shape _rightHydrantShape03;
        private static Shape _fourBarShape;
        private static Shape _sevenShape;
        private static Shape _fourSquareShape01;
        private static Shape _fourSquareShape02;
        private static Shape _leftCornerShape;
        private static Shape _upsideDownTShape01;
        private static Shape _upsideDownTShape02;
        private static Shape _threePoleShape;
        private static Shape _twoPoleShape;
        private static Shape _backwardsLShape;
        private static Shape _singleSquareShape;

        private readonly BorderBuilder _borderBuilder = new BorderBuilder();

        public ShapeSetBuilder(ISquareViewFactory squareViewFactory)
        {
            MakeShapes(squareViewFactory: squareViewFactory);
            _borderBuilder.BuildBorderSquares(squareWidth: ShapeConstants.SquareWidth, containingRectangle: ShapeConstants.ContainingRectangle);
        }

        public ShapeSet GetShapeSet()
        {
            return MakeHardCodedShapeSet();
        }

        public void OccupyBorderSquares(Grid occupiedGridSquares)
        {
            _borderBuilder.OccupyBorderSquares(occupiedGridSquares: occupiedGridSquares);
        }

        private ShapeSet MakeHardCodedShapeSet()
        {
            return new ShapeSet(shapes: new List<Shape> {
                _rightHydrantShape01,
                _fourBarShape,
                _sevenShape,
                _fourSquareShape01,
                _leftCornerShape,
                _rightHydrantShape02,
                _upsideDownTShape01,
                _threePoleShape,
                _twoPoleShape,
                _fourSquareShape02,
                _backwardsLShape,
                _rightHydrantShape03,
                _upsideDownTShape02,
                _singleSquareShape
            });
        }

        public Grid MakeGridSquares()
        {
            return ShapeConstants.MakeGridSquares();
        }

        private void MakeShapes(ISquareViewFactory squareViewFactory)
        {
            // 1:
            _rightHydrantShape01 = MakeShape(colour: SquareFillColour.Red,
                topLeftCorner: new SquareFillPoint(x: 3, y: 1),
                relativePointsTopLeftCorner: ShapeConstants.RightHydrantPoints,
                squareFactory: squareViewFactory);

            // 2:
            _fourBarShape = MakeShape(colour: SquareFillColour.Blue,
                topLeftCorner: new SquareFillPoint(x: 2, y: 15),
                relativePointsTopLeftCorner: ShapeConstants.FourBarPoints,
                squareFactory: squareViewFactory);

            // 3:
            _sevenShape = MakeShape(colour: SquareFillColour.Black,
                topLeftCorner: new SquareFillPoint(x: 9, y: 1),
                relativePointsTopLeftCorner: ShapeConstants.SevenPoints,
                squareFactory: squareViewFactory);

            // 4:
            _fourSquareShape01 = MakeShape(colour: SquareFillColour.Orange,
                topLeftCorner: new SquareFillPoint(x: 6, y: 2),
                relativePointsTopLeftCorner: ShapeConstants.FourSquarePoints,
                squareFactory: squareViewFactory);

            // 5:
            _leftCornerShape = MakeShape(colour: SquareFillColour.Green,
                topLeftCorner: new SquareFillPoint(x: 7, y: 15),
                relativePointsTopLeftCorner: ShapeConstants.LeftCornerPoints,
                squareFactory: squareViewFactory);

            // 6:
            _rightHydrantShape02 = MakeShape(colour: SquareFillColour.Yellow,
                topLeftCorner: new SquareFillPoint(x: 0, y: 1),
                relativePointsTopLeftCorner: ShapeConstants.RightHydrantPoints,
                squareFactory: squareViewFactory);

            // 7:
            _upsideDownTShape01 = MakeShape(colour: SquareFillColour.Purple,
                topLeftCorner: new SquareFillPoint(x: 3, y: 17),
                relativePointsTopLeftCorner: ShapeConstants.UpsideDownTPoints,
                squareFactory: squareViewFactory);

            // 8:
            _threePoleShape = MakeShape(colour: SquareFillColour.Magenta,
                topLeftCorner: new SquareFillPoint(x: 0, y: 16),
                relativePointsTopLeftCorner: ShapeConstants.ThreePolePoints,
                squareFactory: squareViewFactory);

            // 9:
            _twoPoleShape = MakeShape(colour: SquareFillColour.Brown,
                topLeftCorner: new SquareFillPoint(x: 6, y: 17),
                relativePointsTopLeftCorner: ShapeConstants.TwoPolePoints,
                squareFactory: squareViewFactory);

            // 10:
            _fourSquareShape02 = MakeShape(colour: SquareFillColour.Cyan,
                topLeftCorner: new SquareFillPoint(x: 0, y: 9),
                relativePointsTopLeftCorner: ShapeConstants.FourSquarePoints,
                squareFactory: squareViewFactory);

            // 11:
            _backwardsLShape = MakeShape(colour: SquareFillColour.DarkGrey,
                topLeftCorner: new SquareFillPoint(x: 1, y: 5),
                relativePointsTopLeftCorner: ShapeConstants.BackwardsLPoints,
                squareFactory: squareViewFactory);

            // 12:
            _rightHydrantShape03 = MakeShape(colour: SquareFillColour.Grey,
                topLeftCorner: new SquareFillPoint(x: 0, y: 12),
                relativePointsTopLeftCorner: ShapeConstants.RightHydrantPoints,
                squareFactory: squareViewFactory);

            // 13:
            _upsideDownTShape02 = MakeShape(colour: SquareFillColour.White,
                topLeftCorner: new SquareFillPoint(x: 11, y: 15),
                relativePointsTopLeftCorner: ShapeConstants.UpsideDownTPoints,
                squareFactory: squareViewFactory);

            // 14:
            _singleSquareShape = MakeShape(colour: SquareFillColour.LightGrey,
                topLeftCorner: new SquareFillPoint(x: 9, y: 18),
                relativePointsTopLeftCorner: ShapeConstants.SingleSquarePoints,
                squareFactory: squareViewFactory);
        }

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

        private List<Square> MakeSquares(
            SquareFillColour colour,
            List<SquareFillPoint> relativePointsTopLeftCorner,
            ISquareViewFactory squareFactory)
        {
            List<Square> squares = new List<Square>();
            foreach (var element in relativePointsTopLeftCorner) {
                squares.Add(new Square(
                    positionRelativeToParentCorner: element,
                    sprite: squareFactory.MakeSquare(colour: colour)));
            }

            return squares;
        }
    }
}
