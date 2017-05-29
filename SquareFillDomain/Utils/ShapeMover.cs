using System;
using SquareFillDomain.Builders;
using SquareFillDomain.Models;

namespace SquareFillDomain.Utils
{
    public class ShapeMover
    {
        public Shape ShapeToMove { get; private set; }

        private readonly SquareFillPoint _shapeCentreRelativeToCursorPosition = new SquareFillPoint(x: 0, y: 0);
        private readonly SquareFillPoint _topLeftCornerRelativeToCursorPosition = new SquareFillPoint(x: 0, y: 0);

        public void StartMove(SquareFillPoint cursorPositionAtStart, Shape shapeToMove)
        {
            ShapeToMove = shapeToMove;
            _shapeCentreRelativeToCursorPosition.X = ShapeToMove.CentreOfShape.X - cursorPositionAtStart.X;
            _shapeCentreRelativeToCursorPosition.Y = ShapeToMove.CentreOfShape.Y - cursorPositionAtStart.Y;
            CalculateTopLeftCornerRelativeToCursorPosition(cursorPositionAtStart);
        }

        public SquareFillPoint CalculateTopLeftCorner(SquareFillPoint newCursorPosition)
        {
            return new SquareFillPoint(
                x: newCursorPosition.X + _topLeftCornerRelativeToCursorPosition.X,
                y: newCursorPosition.Y + _topLeftCornerRelativeToCursorPosition.Y);
        }

        public SquareFillPoint CalculateShapeCentre(SquareFillPoint newCursorPosition)
        {
            return new SquareFillPoint(
                x: newCursorPosition.X + _shapeCentreRelativeToCursorPosition.X,
                y: newCursorPosition.Y + _shapeCentreRelativeToCursorPosition.Y);
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
                var newShapeCentre = CalculateShapeCentre(newCursorPosition: newCursorPosition);
                var newTopLeftCorner = CalculateTopLeftCorner(newCursorPosition: newCursorPosition);
                ShapeToMove.PutShapeInNewLocation(newCentreOfShape: newShapeCentre);
                ShapeToMove.MoveAllShapeSquares(newTopLeftCorner: newTopLeftCorner);
            }
        }

        public void SnapToGrid(SquareFillPoint newCursorPosition)
        {
            if (ShapeToMove != null)
            {
                var shapeCentreTakingRelativePositionIntoAccount =
                    CalculateShapeCentre(newCursorPosition: newCursorPosition);
                var newShapeCentre = new SquareFillPoint(
                    x: CalculateSnappedX1(newShapeCentreX: shapeCentreTakingRelativePositionIntoAccount.X),
                    y: CalculateSnappedY1(newShapeCentreY: shapeCentreTakingRelativePositionIntoAccount.Y));
                ShapeToMove.PutShapeInNewLocation(newCentreOfShape: newShapeCentre);
                ShapeToMove.CalculateOrigins(newCentreOfShape: newShapeCentre);

                var topLeftCornerTakingRelativePositionIntoAccount = CalculateTopLeftCorner(newCursorPosition: newCursorPosition);
                var newTopLeftCorner = new SquareFillPoint(
                    x: CalculateSnappedX(newTopLeftCornerX: topLeftCornerTakingRelativePositionIntoAccount.X),
                    y: CalculateSnappedY(newTopLeftCornerY: topLeftCornerTakingRelativePositionIntoAccount.Y));

                ShapeToMove.MoveAllShapeSquares(newTopLeftCorner: newTopLeftCorner);
                ShapeToMove.CalculateTopLeftCorners(newTopLeftCorner: newTopLeftCorner);
            }
        }

        public int CalculateSnappedX1(int newShapeCentreX)
        {
            return CalculateSnappedCoordinate1(
                newShapeCentreCoord: newShapeCentreX,
                boundaryRectangleOriginCoord: 0,
                boundaryRectangleDimension: ShapeConstants.ScreenWidth,
                numSquaresOnSmallestSide: ShapeToMove.NumSquaresLeftOfShapeCentre,
                numSquaresOnLargestSide:  ShapeToMove.NumSquaresRightOfShapeCentre);
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

        public int CalculateSnappedY1(int newShapeCentreY)
        {
            return CalculateSnappedCoordinate1(
                newShapeCentreCoord: newShapeCentreY,
                boundaryRectangleOriginCoord: 0,
                boundaryRectangleDimension: ShapeConstants.ScreenHeight,
                numSquaresOnSmallestSide: ShapeToMove.NumSquaresAboveShapeCentre,
                numSquaresOnLargestSide: ShapeToMove.NumSquaresBelowShapeCentre);
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

        private int CalculateSnappedCoordinate1(
            int newShapeCentreCoord, 
            int boundaryRectangleOriginCoord, 
            int boundaryRectangleDimension, 
            int numSquaresOnSmallestSide, 
            int numSquaresOnLargestSide)
        {
            var squareWidth = ShapeConstants.SquareWidth;

            int shapeCentreCoord = newShapeCentreCoord;
            int numberOfSquaresFromEdgeOfScreen = shapeCentreCoord / squareWidth;

            var potentialNewSquareCentre = numberOfSquaresFromEdgeOfScreen * squareWidth +
                                           (squareWidth/2) ;

            var squareCentreAtOneEndOfContainer = boundaryRectangleOriginCoord + squareWidth/2;

            var squareCentreAtOtherEndOfContainer =
                boundaryRectangleOriginCoord + boundaryRectangleDimension - squareWidth/2;

            var potentialCentreOfShapeEdgeOnOneSide =
                potentialNewSquareCentre - numSquaresOnSmallestSide * squareWidth;

            var centreOfShapeEdgeOnOneSideAdjustedForSmallestContainerEdge =
                Math.Max(potentialCentreOfShapeEdgeOnOneSide, squareCentreAtOneEndOfContainer);

            var shapeCentreAdjustedForSmallestContainerEdge =
                centreOfShapeEdgeOnOneSideAdjustedForSmallestContainerEdge
                + (numSquaresOnSmallestSide * squareWidth);

            var potentialCentreOfShapeEdgeOnOtherSide = shapeCentreAdjustedForSmallestContainerEdge +
                                                        (numSquaresOnLargestSide * squareWidth);

            var centreOfShapeEdgeOnOtherSideAdjustedForBothContainerEdges =
                Math.Min(potentialCentreOfShapeEdgeOnOtherSide, squareCentreAtOtherEndOfContainer);

            int actualSquareCentre = centreOfShapeEdgeOnOtherSideAdjustedForBothContainerEdges
                                     - (numSquaresOnLargestSide * squareWidth);

            return actualSquareCentre;
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
            int numberOfSquaresFromEdgeOfScreen = newTopLeftCornerCoord / squareWidth;

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