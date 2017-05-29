using System.Collections.Generic;
using SquareFillDomain.Builders;
using SquareFillDomain.Interfaces;
using SquareFillDomain.Models;
using SquareFillDomain.Utils;

namespace SquareFillDomain.UnitTests.TestUtils
{
    public class TestShapeSetBuilder : IShapeSetBuilder
    {
        public Shape RightHydrantShape01 { get { return _rightHydrantShape01; } }
        public Shape RightHydrantShape02 { get { return _rightHydrantShape02; } }
        public Shape RightHydrantShape03 { get { return _rightHydrantShape03; } }
        public Shape FourBarShape { get { return _fourBarShape; } }
        public Shape SevenShape { get { return _sevenShape; } }
        public Shape FourSquareShape01 { get { return _fourSquareShape01; } }
        public Shape FourSquareShape02 { get { return _fourSquareShape02; } }
        public Shape FourSquareShape03 { get { return _fourSquareShape03; } }
        public Shape LeftCornerShape { get { return _leftCornerShape; } }
        public Shape UpsideDownTShape01 { get { return _upsideDownTShape01; } }
        public Shape UpsideDownTShape02 { get { return _upsideDownTShape02; } }
        public Shape ThreePoleShape { get { return _threePoleShape; } }
        public Shape TwoPoleShape { get { return _twoPoleShape; } }
        public Shape BackwardsLShape { get { return _backwardsLShape; } }
        public Shape SingleSquareShape01 { get { return _singleSquareShape01; } }

        private Shape _rightHydrantShape01;
        private Shape _rightHydrantShape02;
        private Shape _rightHydrantShape03;
        private Shape _fourBarShape;
        private Shape _sevenShape;
        private Shape _fourSquareShape01;
        private Shape _fourSquareShape02;
        private Shape _fourSquareShape03;
        private Shape _leftCornerShape;
        private Shape _upsideDownTShape01;
        private Shape _upsideDownTShape02;
        private Shape _threePoleShape;
        private Shape _twoPoleShape;
        private Shape _backwardsLShape;
        private Shape _singleSquareShape01;

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
                _singleSquareShape01,
                _fourSquareShape03
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
            _rightHydrantShape01 = new Shape(colour: SquareFillColour.Red,
                centreOfShape: new SquareFillPoint(x: originX + 3 * ShapeConstants.SquareWidth,
                    y: originY + 2 * ShapeConstants.SquareWidth),
                topLeftCorner: new SquareFillPoint(x: 3 * ShapeConstants.SquareWidth, y: ShapeConstants.SquareWidth),
                relativePoints: ShapeConstants.RightHydrantCentrePoints,
                relativePointsTopLeftCorner: ShapeConstants.RightHydrantPoints,
                squareFactory: squareViewFactory);

            // 2:
            _fourBarShape = new Shape(colour: SquareFillColour.Blue,
                centreOfShape: new SquareFillPoint(x: originX + 3 * ShapeConstants.SquareWidth,
                    y: originY + 15 * ShapeConstants.SquareWidth),
                topLeftCorner: new SquareFillPoint(x: 2 * ShapeConstants.SquareWidth,
                    y: 15 * ShapeConstants.SquareWidth),
                relativePoints: ShapeConstants.FourBarCentrePoints,
                relativePointsTopLeftCorner: ShapeConstants.FourBarPoints,
                squareFactory: squareViewFactory);

            // 3:
            _sevenShape = new Shape(colour: SquareFillColour.Black,
                centreOfShape: new SquareFillPoint(x: originX + 10 * ShapeConstants.SquareWidth,
                    y: originY + ShapeConstants.SquareWidth),
                topLeftCorner: new SquareFillPoint(x: 9 * ShapeConstants.SquareWidth, y: ShapeConstants.SquareWidth),
                relativePoints: ShapeConstants.SevenCentrePoints,
                relativePointsTopLeftCorner: ShapeConstants.SevenPoints,
                squareFactory: squareViewFactory);

            // 4:
            _fourSquareShape01 = new Shape(colour: SquareFillColour.Orange,
                centreOfShape: new SquareFillPoint(x: originX + 7 * ShapeConstants.SquareWidth,
                    y: originY + 2 * ShapeConstants.SquareWidth),
                topLeftCorner: new SquareFillPoint(x: 6 * ShapeConstants.SquareWidth,
                    y: 2 * ShapeConstants.SquareWidth),
                relativePoints: ShapeConstants.FourSquareCentrePoints,
                relativePointsTopLeftCorner: ShapeConstants.FourSquarePoints,
                squareFactory: squareViewFactory);

            // 5:
            _leftCornerShape = new Shape(colour: SquareFillColour.Green,
                centreOfShape: new SquareFillPoint(x: originX + 8 * ShapeConstants.SquareWidth,
                    y: originY + 15 * ShapeConstants.SquareWidth),
                topLeftCorner: new SquareFillPoint(x: 7 * ShapeConstants.SquareWidth,
                    y: 15 * ShapeConstants.SquareWidth),
                relativePoints: ShapeConstants.LeftCornerCentrePoints,
                relativePointsTopLeftCorner: ShapeConstants.LeftCornerPoints,
                squareFactory: squareViewFactory);

            // 6:
            _rightHydrantShape02 = new Shape(colour: SquareFillColour.Yellow,
                centreOfShape: new SquareFillPoint(x: originX, y: originY + 2 * ShapeConstants.SquareWidth),
                topLeftCorner: new SquareFillPoint(x: 0, y: ShapeConstants.SquareWidth),
                relativePoints: ShapeConstants.RightHydrantCentrePoints,
                relativePointsTopLeftCorner: ShapeConstants.RightHydrantPoints,
                squareFactory: squareViewFactory);

            // 7:
            _upsideDownTShape01 = new Shape(colour: SquareFillColour.Purple,
                centreOfShape: new SquareFillPoint(x: originX + 3 * ShapeConstants.SquareWidth,
                    y: originY + 18 * ShapeConstants.SquareWidth),
                topLeftCorner: new SquareFillPoint(x: 3 * ShapeConstants.SquareWidth,
                    y: 17 * ShapeConstants.SquareWidth),
                relativePoints: ShapeConstants.UpsideDownTCentrePoints,
                relativePointsTopLeftCorner: ShapeConstants.UpsideDownTPoints,
                squareFactory: squareViewFactory);

            // 8:
            _threePoleShape = new Shape(colour: SquareFillColour.Magenta,
                centreOfShape: new SquareFillPoint(x: originX, y: originY + 17 * ShapeConstants.SquareWidth),
                topLeftCorner: new SquareFillPoint(x: 0, y: 16 * ShapeConstants.SquareWidth),
                relativePoints: ShapeConstants.ThreePoleCentrePoints,
                relativePointsTopLeftCorner: ShapeConstants.ThreePolePoints,
                squareFactory: squareViewFactory);

            // 9:
            _twoPoleShape = new Shape(colour: SquareFillColour.Brown,
                centreOfShape: new SquareFillPoint(x: originX + 6 * ShapeConstants.SquareWidth,
                    y: originY + 18 * ShapeConstants.SquareWidth),
                topLeftCorner: new SquareFillPoint(x: 6 * ShapeConstants.SquareWidth,
                    y: 17 * ShapeConstants.SquareWidth),
                relativePoints: ShapeConstants.TwoPoleCentrePoints,
                relativePointsTopLeftCorner: ShapeConstants.TwoPolePoints,
                squareFactory: squareViewFactory);

            // 10:
            _fourSquareShape02 = new Shape(colour: SquareFillColour.Cyan,
                centreOfShape: new SquareFillPoint(x: originX + ShapeConstants.SquareWidth,
                    y: originY + 9 * ShapeConstants.SquareWidth),
                topLeftCorner: new SquareFillPoint(x: 0, y: 9 * ShapeConstants.SquareWidth),
                relativePoints: ShapeConstants.FourSquareCentrePoints,
                relativePointsTopLeftCorner: ShapeConstants.FourSquarePoints,
                squareFactory: squareViewFactory);

            // 11:
            _backwardsLShape = new Shape(colour: SquareFillColour.DarkGrey,
                centreOfShape: new SquareFillPoint(x: originX + ShapeConstants.SquareWidth,
                    y: originY + 7 * ShapeConstants.SquareWidth),
                topLeftCorner: new SquareFillPoint(x: ShapeConstants.SquareWidth, y: 5 * ShapeConstants.SquareWidth),
                relativePoints: ShapeConstants.BackwardsLCentrePoints,
                relativePointsTopLeftCorner: ShapeConstants.BackwardsLPoints,
                squareFactory: squareViewFactory);

            // 12:
            _rightHydrantShape03 = new Shape(colour: SquareFillColour.Grey,
                centreOfShape: new SquareFillPoint(x: originX, y: originY + 13 * ShapeConstants.SquareWidth),
                topLeftCorner: new SquareFillPoint(x: 0, y: 12 * ShapeConstants.SquareWidth),
                relativePoints: ShapeConstants.RightHydrantCentrePoints,
                relativePointsTopLeftCorner: ShapeConstants.RightHydrantPoints,
                squareFactory: squareViewFactory);

            // 13:
            _upsideDownTShape02 = new Shape(colour: SquareFillColour.White,
                centreOfShape: new SquareFillPoint(x: originX + 11 * ShapeConstants.SquareWidth,
                    y: originY + 16 * ShapeConstants.SquareWidth),
                topLeftCorner: new SquareFillPoint(x: 11 * ShapeConstants.SquareWidth,
                    y: 15 * ShapeConstants.SquareWidth),
                relativePoints: ShapeConstants.UpsideDownTCentrePoints,
                relativePointsTopLeftCorner: ShapeConstants.UpsideDownTPoints,
                squareFactory: squareViewFactory);

            // 14:
            _singleSquareShape01 = new Shape(colour: SquareFillColour.LightGrey,
                centreOfShape: new SquareFillPoint(x: originX + 9 * ShapeConstants.SquareWidth,
                    y: originY + 18 * ShapeConstants.SquareWidth),
                topLeftCorner: new SquareFillPoint(x: 9 * ShapeConstants.SquareWidth,
                    y: 18 * ShapeConstants.SquareWidth),
                relativePoints: ShapeConstants.SingleSquareCentrePoints,
                relativePointsTopLeftCorner: ShapeConstants.SingleSquarePoints,
                squareFactory: squareViewFactory);

            // created purely for test:
            _fourSquareShape03 = new Shape(colour: SquareFillColour.Cyan,
                centreOfShape: new SquareFillPoint(
                    x: originX + 7*ShapeConstants.SquareWidth,
                    y: originY + 8*ShapeConstants.SquareWidth),
                topLeftCorner: new SquareFillPoint(x: 6*ShapeConstants.SquareWidth, y: 8*ShapeConstants.SquareWidth),
                relativePoints: ShapeConstants.FourSquareCentrePoints,
                relativePointsTopLeftCorner: ShapeConstants.FourSquarePoints,
                squareFactory: squareViewFactory);
        }
    }
}
