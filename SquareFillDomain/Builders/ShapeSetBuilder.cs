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
        private static Shape _singleSquareShape01;

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

        public void OccupyBorderSquares(List<List<GridSquare>> occupiedGridSquares)
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
                _singleSquareShape01
            });
        }

        public List<List<GridSquare>> MakeGridSquares()
        {
            return ShapeConstants.MakeGridSquares();
        }

        private void MakeShapes(ISquareViewFactory squareViewFactory)
        {
            var originX = ShapeConstants.SquareWidth / 2;
            var originY = ShapeConstants.SquareWidth / 2;

            // 1:
            _rightHydrantShape01 = new Shape(colour: SquareFillColour.Red,
                topLeftCorner: new SquareFillPoint(x: 3 * ShapeConstants.SquareWidth, y: ShapeConstants.SquareWidth),
                relativePointsTopLeftCorner: ShapeConstants.RightHydrantPoints,
                squareFactory: squareViewFactory);

            // 2:
            _fourBarShape = new Shape(colour: SquareFillColour.Blue,
                topLeftCorner: new SquareFillPoint(x: 2 * ShapeConstants.SquareWidth,
                    y: 15 * ShapeConstants.SquareWidth),
                relativePointsTopLeftCorner: ShapeConstants.FourBarPoints,
                squareFactory: squareViewFactory);

            // 3:
            _sevenShape = new Shape(colour: SquareFillColour.Black,
                topLeftCorner: new SquareFillPoint(x: 9 * ShapeConstants.SquareWidth, y: ShapeConstants.SquareWidth),
                relativePointsTopLeftCorner: ShapeConstants.SevenPoints,
                squareFactory: squareViewFactory);

            // 4:
            _fourSquareShape01 = new Shape(colour: SquareFillColour.Orange,
                topLeftCorner: new SquareFillPoint(x: 6 * ShapeConstants.SquareWidth,
                    y: 2 * ShapeConstants.SquareWidth),
                relativePointsTopLeftCorner: ShapeConstants.FourSquarePoints,
                squareFactory: squareViewFactory);

            // 5:
            _leftCornerShape = new Shape(colour: SquareFillColour.Green,
                topLeftCorner: new SquareFillPoint(x: 7 * ShapeConstants.SquareWidth,
                    y: 15 * ShapeConstants.SquareWidth),
                relativePointsTopLeftCorner: ShapeConstants.LeftCornerPoints,
                squareFactory: squareViewFactory);

            // 6:
            _rightHydrantShape02 = new Shape(colour: SquareFillColour.Yellow,
                topLeftCorner: new SquareFillPoint(x: 0, y: ShapeConstants.SquareWidth),
                relativePointsTopLeftCorner: ShapeConstants.RightHydrantPoints,
                squareFactory: squareViewFactory);

            // 7:
            _upsideDownTShape01 = new Shape(colour: SquareFillColour.Purple,
                topLeftCorner: new SquareFillPoint(x: 3 * ShapeConstants.SquareWidth,
                    y: 17 * ShapeConstants.SquareWidth),
                relativePointsTopLeftCorner: ShapeConstants.UpsideDownTPoints,
                squareFactory: squareViewFactory);

            // 8:
            _threePoleShape = new Shape(colour: SquareFillColour.Magenta,
                topLeftCorner: new SquareFillPoint(x: 0, y: 16 * ShapeConstants.SquareWidth),
                relativePointsTopLeftCorner: ShapeConstants.ThreePolePoints,
                squareFactory: squareViewFactory);

            // 9:
            _twoPoleShape = new Shape(colour: SquareFillColour.Brown,
                topLeftCorner: new SquareFillPoint(x: 6 * ShapeConstants.SquareWidth,
                    y: 17 * ShapeConstants.SquareWidth),
                relativePointsTopLeftCorner: ShapeConstants.TwoPolePoints,
                squareFactory: squareViewFactory);

            // 10:
            _fourSquareShape02 = new Shape(colour: SquareFillColour.Cyan,
                topLeftCorner: new SquareFillPoint(x: 0, y: 9 * ShapeConstants.SquareWidth),
                relativePointsTopLeftCorner: ShapeConstants.FourSquarePoints,
                squareFactory: squareViewFactory);

            // 11:
            _backwardsLShape = new Shape(colour: SquareFillColour.DarkGrey,
                topLeftCorner: new SquareFillPoint(x: ShapeConstants.SquareWidth, y: 5 * ShapeConstants.SquareWidth),
                relativePointsTopLeftCorner: ShapeConstants.BackwardsLPoints,
                squareFactory: squareViewFactory);

            // 12:
            _rightHydrantShape03 = new Shape(colour: SquareFillColour.Grey,
                topLeftCorner: new SquareFillPoint(x: 0, y: 12 * ShapeConstants.SquareWidth),
                relativePointsTopLeftCorner: ShapeConstants.RightHydrantPoints,
                squareFactory: squareViewFactory);

            // 13:
            _upsideDownTShape02 = new Shape(colour: SquareFillColour.White,
                topLeftCorner: new SquareFillPoint(x: 11 * ShapeConstants.SquareWidth,
                    y: 15 * ShapeConstants.SquareWidth),
                relativePointsTopLeftCorner: ShapeConstants.UpsideDownTPoints,
                squareFactory: squareViewFactory);

            // 14:
            _singleSquareShape01 = new Shape(colour: SquareFillColour.LightGrey,
                topLeftCorner: new SquareFillPoint(x: 9 * ShapeConstants.SquareWidth,
                    y: 18 * ShapeConstants.SquareWidth),
                relativePointsTopLeftCorner: ShapeConstants.SingleSquarePoints,
                squareFactory: squareViewFactory);
        }
    }
}
