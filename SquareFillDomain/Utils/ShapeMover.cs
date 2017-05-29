using System;
using SquareFillDomain.Builders;
using SquareFillDomain.Models;

namespace SquareFillDomain.Utils
{
    public class ShapeMover
    {
        public Shape ShapeToMove { get; private set; }

        private readonly SquareFillPoint _topLeftCornerRelativeToCursorPosition = new SquareFillPoint(x: 0, y: 0);

        public void StartMove(SquareFillPoint cursorPositionAtStart, Shape shapeToMove)
        {
            ShapeToMove = shapeToMove;
            CalculateTopLeftCornerRelativeToCursorPosition(cursorPositionAtStart);
        }

        public SquareFillPoint CalculateTopLeftCorner(SquareFillPoint newCursorPosition)
        {
            return new SquareFillPoint(
                x: newCursorPosition.X + _topLeftCornerRelativeToCursorPosition.X,
                y: newCursorPosition.Y + _topLeftCornerRelativeToCursorPosition.Y);
        }

        public SquareFillPoint CalculateCursorPosition(SquareFillPoint topLeftCorner)
        {
            return new SquareFillPoint(
                x: topLeftCorner.X - _topLeftCornerRelativeToCursorPosition.X,
                y: topLeftCorner.Y - _topLeftCornerRelativeToCursorPosition.Y);
        }

        public void MoveToNewCursorPosition(SquareFillPoint newCursorPosition)
        {
            if (ShapeToMove != null)
            {
                var newTopLeftCorner = CalculateTopLeftCorner(newCursorPosition: newCursorPosition);
                ShapeToMove.MoveAllShapeSquares(newTopLeftCorner: newTopLeftCorner);
            }
        }

        public void SnapToGrid(SquareFillPoint newCursorPosition)
        {
            if (ShapeToMove != null)
            {
                var topLeftCornerTakingRelativePositionIntoAccount = CalculateTopLeftCorner(newCursorPosition: newCursorPosition);
                var newTopLeftCorner = new SquareFillPoint(
                    x: CalculateSnappedX(newTopLeftCornerX: topLeftCornerTakingRelativePositionIntoAccount.X),
                    y: CalculateSnappedY(newTopLeftCornerY: topLeftCornerTakingRelativePositionIntoAccount.Y));

                ShapeToMove.MoveAllShapeSquares(newTopLeftCorner: newTopLeftCorner);
                ShapeToMove.CalculateTopLeftCorners(newTopLeftCorner: newTopLeftCorner);
            }
        }

        public int CalculateSnappedX(int newTopLeftCornerX)
        {
            return CalculateSnappedCoordinate(
                newTopLeftCornerCoord: newTopLeftCornerX,
                boundaryRectangleOriginCoord: 0,
                boundaryRectangleDimension: ShapeConstants.ScreenWidth,
                numSquaresOnSmallestSide: ShapeToMove.NumSquaresLeftOfTopLeftCorner,
                numSquaresOnLargestSide: ShapeToMove.NumSquaresRightOfTopLeftCorner);
        }

        public int CalculateSnappedY(int newTopLeftCornerY)
        {
            return CalculateSnappedCoordinate(
                newTopLeftCornerCoord: newTopLeftCornerY,
                boundaryRectangleOriginCoord: 0,
                boundaryRectangleDimension: ShapeConstants.ScreenHeight,
                numSquaresOnSmallestSide: ShapeToMove.NumSquaresAboveTopLeftCorner,
                numSquaresOnLargestSide: ShapeToMove.NumSquaresBelowTopLeftCorner);
        }

        private void CalculateTopLeftCornerRelativeToCursorPosition(SquareFillPoint cursorPositionAtStart)
        {
            _topLeftCornerRelativeToCursorPosition.X = ShapeToMove.TopLeftCorner.X - cursorPositionAtStart.X;
            _topLeftCornerRelativeToCursorPosition.Y = ShapeToMove.TopLeftCorner.Y - cursorPositionAtStart.Y;
        }

        private int CalculateSnappedCoordinate(
            int newTopLeftCornerCoord,
            int boundaryRectangleOriginCoord,
            int boundaryRectangleDimension,
            int numSquaresOnSmallestSide,
            int numSquaresOnLargestSide)
        {
            var squareWidth = ShapeConstants.SquareWidth;

            int topLeftCornerCoord = newTopLeftCornerCoord;
            int numberOfSquaresFromEdgeOfScreen = topLeftCornerCoord / squareWidth;
            if (newTopLeftCornerCoord % squareWidth > squareWidth / 2)
            {
                numberOfSquaresFromEdgeOfScreen++;
            }

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

            int actualTopLeftCorner = topLeftCornerOfShapeEdgeOnOtherSideAdjustedForBothContainerEdges
                                     - (numSquaresOnLargestSide * squareWidth);

            return actualTopLeftCorner;
        }
    }
}