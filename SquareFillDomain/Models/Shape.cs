using System;
using System.Collections.Generic;
using SquareFillDomain.Utils;

namespace SquareFillDomain.Models
{
    public class Shape
    {
        public int CentreOfShapeX { get { return _topLeftCorner.X + ShapeConstants.SquareWidth / 2; } }
        public int CentreOfShapeY { get { return _topLeftCorner.Y + ShapeConstants.SquareWidth / 2; } }
        public int TopLeftCornerX { get { return _topLeftCorner.X; } }
        public int TopLeftCornerY { get { return _topLeftCorner.Y; } }
        public int NumSquaresLeftOfTopLeftCorner { get { return _numSquaresLeftOfTopLeftCorner; } }
        public int NumSquaresRightOfTopLeftCorner { get { return _numSquaresRightOfTopLeftCorner; } }
        public int NumSquaresAboveTopLeftCorner { get { return _numSquaresAboveTopLeftCorner; } }
        public int NumSquaresBelowTopLeftCorner { get { return _numSquaresBelowTopLeftCorner; } }

        private SquareFillPoint _topLeftCorner;
        private readonly List<Square> _squares;

        private int _numSquaresLeftOfTopLeftCorner;
        private int _numSquaresRightOfTopLeftCorner;
        private int _numSquaresAboveTopLeftCorner;
        private int _numSquaresBelowTopLeftCorner;

        public Shape(
            SquareFillPoint topLeftCorner,
            List<Square> squareDefinitions,
            bool topLeftCornerIsGridRef = true)
        {
            InitialiseTopLeftCorner(
                topLeftCorner: topLeftCorner,
                topLeftCornerIsGridRef: topLeftCornerIsGridRef);

            _squares = squareDefinitions;

            CalculateNumSquaresAroundTopLeftCorner();
            UpdateTopLeftCorner(newTopLeftCorner: _topLeftCorner);
        }

        public bool IsInShape(SquareFillPoint point)
        {
            bool isInShape = false;
            
            foreach(var element in _squares) {
                isInShape = isInShape || element.IsInSquare(point: point);
            }

            return isInShape;
        }

        public void UpdateTopLeftCorner(SquareFillPoint newTopLeftCorner)
        {
            _topLeftCorner = newTopLeftCorner;
            foreach (var element in _squares) {
                element.MoveTopLeftCorner(newTopLeftCorner: newTopLeftCorner);
            }
        }

        public MovementAnalyser CheckWhetherMovementIsPossible(
            Grid occupiedGridSquares,
            SquareFillPoint newTopLeftCorner)
        {
            var movementAnalyser = new MovementAnalyser(
                squares: _squares,
                occupiedGridSquares: occupiedGridSquares,
                newTopLeftCorner: newTopLeftCorner);
            
            return movementAnalyser;
        }

	    public void VacateGridSquares(Grid occupiedGridSquares) 
        {
            foreach (var element in _squares) {
                element.VacateGridSquare(occupiedGridSquares: occupiedGridSquares);
            }
        }

        public void OccupyGridSquares(Grid occupiedGridSquares)
        {
            foreach (var element in _squares) {
                element.OccupyGridSquare(occupiedGridSquares: occupiedGridSquares, shapeInSquare: this);
            }
        }

        public bool WeStartedWithinTheContainingRectangle()
        {
            var leftEdge = _topLeftCorner.X - _numSquaresLeftOfTopLeftCorner * ShapeConstants.SquareWidth;
            var topEdge = _topLeftCorner.Y - _numSquaresAboveTopLeftCorner * ShapeConstants.SquareWidth;
            var rightEdge = _topLeftCorner.X + _numSquaresRightOfTopLeftCorner * ShapeConstants.SquareWidth;
            var bottomEdge = _topLeftCorner.Y + _numSquaresBelowTopLeftCorner * ShapeConstants.SquareWidth;

            return leftEdge >= ShapeConstants.ContainingRectangle.X
                && topEdge >= ShapeConstants.ContainingRectangle.Y
                && rightEdge <= (ShapeConstants.ContainingRectangle.X + ShapeConstants.ContainingRectangle.Width)
                && bottomEdge <= (ShapeConstants.ContainingRectangle.Y + ShapeConstants.ContainingRectangle.Height);
        }

	    public string TopLeftCornersAsString()
        {
            string topLeftCornerAsString = String.Empty;

            foreach (var element in _squares) {
                topLeftCornerAsString = topLeftCornerAsString + element.TopLeftCornerAsString();
            }

            return topLeftCornerAsString;
        }

        public SquareFillPoint CalculateTopLeftCornerRelativeToCursorPosition(SquareFillPoint cursorPosition)
        {
            return new SquareFillPoint(
                x: _topLeftCorner.X - cursorPosition.X,
                y: _topLeftCorner.Y - cursorPosition.Y);
        }

        public SquareFillPoint CalculateCursorPositionBasedOnTopLeftCorner(SquareFillPoint topLeftCornerRelativeToCursorPosition)
        {
            return new SquareFillPoint(
                x: _topLeftCorner.X - topLeftCornerRelativeToCursorPosition.X,
                y: _topLeftCorner.Y - topLeftCornerRelativeToCursorPosition.Y);
        }

        public void SnapToGrid(SquareFillPoint newTopLeftCorner)
        {
            var snappedTopLeftCorner = new SquareFillPoint(
                x: CalculateSnappedX(newTopLeftCornerX: newTopLeftCorner.X),
                y: CalculateSnappedY(newTopLeftCornerY: newTopLeftCorner.Y));

            UpdateTopLeftCorner(newTopLeftCorner: snappedTopLeftCorner);
        }

        public void SnapToGridInRelevantDimensionsIfPossible(MovementAnalyser movementResult, Grid occupiedGridSquares)
        {
            var previousTopLeftCorner = _topLeftCorner;
            var newTopLeftCorner = new SquareFillPoint(x: previousTopLeftCorner.X, y: previousTopLeftCorner.Y);

            if (movementResult.ShapeHasCrossedAHorizontalGridBoundary)
            {
                newTopLeftCorner.X = CalculateSnappedX(newTopLeftCornerX: newTopLeftCorner.X);
            }

            if (movementResult.ShapeHasCrossedAVerticalGridBoundary)
            {
                newTopLeftCorner.Y = CalculateSnappedY(newTopLeftCornerY: newTopLeftCorner.Y);
            }

            MovementAnalyser newMovementResult = CheckWhetherMovementIsPossible(
                occupiedGridSquares: occupiedGridSquares,
                newTopLeftCorner: newTopLeftCorner);

            if (newMovementResult.NoShapesAreInTheWay)
            {
                UpdateTopLeftCorner(newTopLeftCorner: newTopLeftCorner);
            }
        }

        public int CalculateSnappedX(int newTopLeftCornerX)
        {
            return CalculateSnappedCoordinate(
                newTopLeftCornerCoord: newTopLeftCornerX,
                boundaryRectangleOriginCoord: 0,
                boundaryRectangleDimension: ShapeConstants.ScreenWidth,
                numSquaresOnSmallestSide: _numSquaresLeftOfTopLeftCorner,
                numSquaresOnLargestSide: _numSquaresRightOfTopLeftCorner);
        }

        public int CalculateSnappedY(int newTopLeftCornerY)
        {
            return CalculateSnappedCoordinate(
                newTopLeftCornerCoord: newTopLeftCornerY,
                boundaryRectangleOriginCoord: 0,
                boundaryRectangleDimension: ShapeConstants.ScreenHeight,
                numSquaresOnSmallestSide: _numSquaresAboveTopLeftCorner,
                numSquaresOnLargestSide: _numSquaresBelowTopLeftCorner);
        }

        private int CalculateSnappedCoordinate(
            int newTopLeftCornerCoord,
            int boundaryRectangleOriginCoord,
            int boundaryRectangleDimension,
            int numSquaresOnSmallestSide,
            int numSquaresOnLargestSide)
        {
            var squareWidth = ShapeConstants.SquareWidth;

            var numberOfSquaresFromEdgeOfScreen = CalculateNumSquaresFromEdgeOfScreen(topLeftCornerCoordinate: newTopLeftCornerCoord);

            var potentialNewTopLeftCorner = numberOfSquaresFromEdgeOfScreen * squareWidth;

            var topLeftCornerAtOneEndOfContainer = boundaryRectangleOriginCoord;

            var topLeftCornerAtOtherEndOfContainer = boundaryRectangleOriginCoord + boundaryRectangleDimension;

            var potentialTopLeftCornerOfShapeEdgeOnOneSide = potentialNewTopLeftCorner - (numSquaresOnSmallestSide * squareWidth);

            var topLeftCornerOfShapeEdgeOnOneSideAdjustedForSmallestContainerEdge =
                Math.Max(potentialTopLeftCornerOfShapeEdgeOnOneSide, topLeftCornerAtOneEndOfContainer);

            var topLeftCornerAdjustedForSmallestContainerEdge =
                topLeftCornerOfShapeEdgeOnOneSideAdjustedForSmallestContainerEdge
                + (numSquaresOnSmallestSide * squareWidth);

            var potentialTopLeftCornerOfShapeEdgeOnOtherSide = topLeftCornerAdjustedForSmallestContainerEdge +
                                                        (numSquaresOnLargestSide * squareWidth);

            var topLeftCornerOfShapeEdgeOnOtherSideAdjustedForBothContainerEdges =
                Math.Min(potentialTopLeftCornerOfShapeEdgeOnOtherSide, topLeftCornerAtOtherEndOfContainer);

            var actualTopLeftCorner = topLeftCornerOfShapeEdgeOnOtherSideAdjustedForBothContainerEdges
                                     - (numSquaresOnLargestSide * squareWidth);

            return actualTopLeftCorner;
        }

        private int CalculateNumSquaresFromEdgeOfScreen(int topLeftCornerCoordinate)
        {
            var numberOfSquaresFromEdgeOfScreen = topLeftCornerCoordinate / ShapeConstants.SquareWidth;

            if (MoreThanHalfWayAcrossASquare(topLeftCornerCoordinate: topLeftCornerCoordinate))
            {
                numberOfSquaresFromEdgeOfScreen++;
            }

            return numberOfSquaresFromEdgeOfScreen;
        }

        private bool MoreThanHalfWayAcrossASquare(int topLeftCornerCoordinate)
        {
            return (topLeftCornerCoordinate % ShapeConstants.SquareWidth) > (ShapeConstants.SquareWidth / 2);
        }

        private void InitialiseTopLeftCorner(SquareFillPoint topLeftCorner, bool topLeftCornerIsGridRef)
        {
            if (topLeftCornerIsGridRef)
            {
                _topLeftCorner = topLeftCorner.ConvertToPixels();
            }
            else
            {
                _topLeftCorner = new SquareFillPoint(x: topLeftCorner.X, y: topLeftCorner.Y);
            }
        }

        private void CalculateNumSquaresAroundTopLeftCorner()
        {
            foreach (var element in _squares) {
                _numSquaresLeftOfTopLeftCorner = Math.Min(_numSquaresLeftOfTopLeftCorner, element.XRelativeToParentCorner);
                _numSquaresRightOfTopLeftCorner = Math.Max(_numSquaresRightOfTopLeftCorner, element.XRelativeToParentCorner);
                _numSquaresAboveTopLeftCorner = Math.Min(_numSquaresAboveTopLeftCorner, element.YRelativeToParentCorner);
                _numSquaresBelowTopLeftCorner = Math.Max(_numSquaresBelowTopLeftCorner, element.YRelativeToParentCorner);
            }

            DealWithNegativeNumbersOfSquares();
        }

        private void DealWithNegativeNumbersOfSquares()
        {
            _numSquaresLeftOfTopLeftCorner = Math.Abs(_numSquaresLeftOfTopLeftCorner);
            _numSquaresAboveTopLeftCorner = Math.Abs(_numSquaresAboveTopLeftCorner);
        }
    }
}