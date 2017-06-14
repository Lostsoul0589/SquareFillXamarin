using System;
using System.Collections.Generic;
using SquareFillDomain.Interfaces;
using SquareFillDomain.Models;
using SquareFillDomain.Utils;

namespace SquareFillDomain.Builders
{
    public class ShapeSetBuilder : IShapeSetBuilder
    {
        // private var _rightHydrantShape01: Shape! = nil;
        private Shape _rightHydrantShape01 = null;
        private Shape _rightHydrantShape02 = null;
        private Shape _rightHydrantShape03 = null;
        private Shape _fourBarShape = null;
        private Shape _sevenShape = null;
        private Shape _fourSquareShape01 = null;
        private Shape _fourSquareShape02 = null;
        private Shape _leftCornerShape = null;
        private Shape _upsideDownTShape01 = null;
        private Shape _upsideDownTShape02 = null;
        private Shape _threePoleShape = null;
        private Shape _twoPoleShape = null;
        private Shape _backwardsLShape = null;
        private Shape _singleSquareShape = null;

        private readonly BorderBuilder _borderBuilder = new BorderBuilder();

        // init(squareViewFactory: ISquareViewFactory) 
        public ShapeSetBuilder(ISquareViewFactory squareViewFactory) 
        {
            MakeShapes(squareViewFactory: squareViewFactory);
            _borderBuilder.BuildBorderSquares(
                squareWidth: ShapeConstants.SquareWidth, 
                containingRectangle: ShapeConstants.ContainingRectangle);
        }

        // public static func GetShapeSet() -> ShapeSet 
        public ShapeSet GetShapeSet() 
        {
            return MakeHardCodedShapeSet();
        }

        // public func OccupyBorderSquares(occupiedGridSquares: Grid) 
        public void OccupyBorderSquares(Grid occupiedGridSquares) 
        {
            _borderBuilder.OccupyBorderSquares(occupiedGridSquares: occupiedGridSquares);
        }

        // public func MakeGridSquares() -> Grid 
        public Grid MakeGridSquares() 
        {
            return ShapeConstants.MakeGridSquares();
        }

        // private static func MakeHardCodedShapeSet() -> ShapeSet 
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

        // private func MakeShapes(squareViewFactory: ISquareViewFactory) 
        private void MakeShapes(ISquareViewFactory squareViewFactory) 
        {
            // 1:
            _rightHydrantShape01 = MakeShape(colour: SquareFillColour.Red,
                topLeftCorner: SquareFillPoint(x: 3, y: 1),
                relativePointsTopLeftCorner: ShapeConstants.RightHydrantPoints,
                squareFactory: squareViewFactory);

            // 2:
            _fourBarShape = MakeShape(colour: SquareFillColour.Blue,
                topLeftCorner: SquareFillPoint(x: 2, y: 15),
                relativePointsTopLeftCorner: ShapeConstants.FourBarPoints,
                squareFactory: squareViewFactory);

            // 3:
            _sevenShape = MakeShape(colour: SquareFillColour.Black,
                topLeftCorner: SquareFillPoint(x: 9, y: 1),
                relativePointsTopLeftCorner: ShapeConstants.SevenPoints,
                squareFactory: squareViewFactory);

            // 4:
            _fourSquareShape01 = MakeShape(colour: SquareFillColour.Orange,
                topLeftCorner: SquareFillPoint(x: 6, y: 2),
                relativePointsTopLeftCorner: ShapeConstants.FourSquarePoints,
                squareFactory: squareViewFactory);

            // 5:
            _leftCornerShape = MakeShape(colour: SquareFillColour.Green,
                topLeftCorner: SquareFillPoint(x: 7, y: 15),
                relativePointsTopLeftCorner: ShapeConstants.LeftCornerPoints,
                squareFactory: squareViewFactory);

            // 6:
            _rightHydrantShape02 = MakeShape(colour: SquareFillColour.Yellow,
                topLeftCorner: SquareFillPoint(x: 0, y: 1),
                relativePointsTopLeftCorner: ShapeConstants.RightHydrantPoints,
                squareFactory: squareViewFactory);

            // 7:
            _upsideDownTShape01 = MakeShape(colour: SquareFillColour.Purple,
                topLeftCorner: SquareFillPoint(x: 3, y: 17),
                relativePointsTopLeftCorner: ShapeConstants.UpsideDownTPoints,
                squareFactory: squareViewFactory);

            // 8:
            _threePoleShape = MakeShape(colour: SquareFillColour.Magenta,
                topLeftCorner: SquareFillPoint(x: 0, y: 16),
                relativePointsTopLeftCorner: ShapeConstants.ThreePolePoints,
                squareFactory: squareViewFactory);

            // 9:
            _twoPoleShape = MakeShape(colour: SquareFillColour.Brown,
                topLeftCorner: SquareFillPoint(x: 6, y: 17),
                relativePointsTopLeftCorner: ShapeConstants.TwoPolePoints,
                squareFactory: squareViewFactory);

            // 10:
            _fourSquareShape02 = MakeShape(colour: SquareFillColour.Cyan,
                topLeftCorner: SquareFillPoint(x: 0, y: 9),
                relativePointsTopLeftCorner: ShapeConstants.FourSquarePoints,
                squareFactory: squareViewFactory);

            // 11:
            _backwardsLShape = MakeShape(colour: SquareFillColour.DarkGrey,
                topLeftCorner: SquareFillPoint(x: 1, y: 5),
                relativePointsTopLeftCorner: ShapeConstants.BackwardsLPoints,
                squareFactory: squareViewFactory);

            // 12:
            _rightHydrantShape03 = MakeShape(colour: SquareFillColour.Grey,
                topLeftCorner: SquareFillPoint(x: 0, y: 12),
                relativePointsTopLeftCorner: ShapeConstants.RightHydrantPoints,
                squareFactory: squareViewFactory);

            // 13:
            _upsideDownTShape02 = MakeShape(colour: SquareFillColour.White,
                topLeftCorner: SquareFillPoint(x: 11, y: 15),
                relativePointsTopLeftCorner: ShapeConstants.UpsideDownTPoints,
                squareFactory: squareViewFactory);

            // 14:
            _singleSquareShape = MakeShape(colour: SquareFillColour.LightGrey,
                topLeftCorner: SquareFillPoint(x: 9, y: 18),
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

        // private func MakeSquares(
        //      colour: SquareFillColour,
        //      relativePointsTopLeftCorner: [SquareFillPoint],
        //      squareFactory: ISquareViewFactory) -> [Square] 
        private List<Square> MakeSquares(
            SquareFillColour colour,
            List<SquareFillPoint> relativePointsTopLeftCorner,
            ISquareViewFactory squareFactory)
        {
            //let squares: [Square] = [];
            List<Square> squares = new List<Square>();
            foreach (var element in relativePointsTopLeftCorner) {
                squares.Add(new Square(
                    positionRelativeToParentCorner: element,
                    sprite: squareFactory.MakeSquare(colour: colour)));
            }

            return squares;
        }

        private SquareFillPoint SquareFillPoint(int x, int y)
        {
            return new SquareFillPoint(x: x, y: y);
        }
    }
}
