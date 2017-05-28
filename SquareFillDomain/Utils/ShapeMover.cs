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
                    x: CalculateSnappedX(newShapeCentreX: shapeCentreTakingRelativePositionIntoAccount.X),
                    y: CalculateSnappedY(newShapeCentreY: shapeCentreTakingRelativePositionIntoAccount.Y));
        
                ShapeToMove.PutShapeInNewLocation(newCentreOfShape: newShapeCentre);
                ShapeToMove.CalculateOrigins(newCentreOfShape: newShapeCentre);
            }
        }

        public int CalculateSnappedX(int newShapeCentreX)
        {
            return CalculateSnappedCoordinate(
                newShapeCentreCoord: newShapeCentreX,
                boundaryRectangleOriginCoord: 0,
                boundaryRectangleDimension: ShapeSetBuilder.ScreenWidth,
                numSquaresOnSmallestSide: ShapeToMove.NumSquaresLeftOfShapeCentre,
                numSquaresOnLargestSide:  ShapeToMove.NumSquaresRightOfShapeCentre);
        }

        public int CalculateSnappedY(int newShapeCentreY)
        {
            return CalculateSnappedCoordinate(
                newShapeCentreCoord: newShapeCentreY,
                boundaryRectangleOriginCoord: 0,
                boundaryRectangleDimension: ShapeSetBuilder.ScreenHeight,
                numSquaresOnSmallestSide: ShapeToMove.NumSquaresAboveShapeCentre,
                numSquaresOnLargestSide: ShapeToMove.NumSquaresBelowShapeCentre);
        }

        private void CalculateTopLeftCornerRelativeToCursorPosition(SquareFillPoint cursorPositionAtStart)
        {
            _topLeftCornerRelativeToCursorPosition.X = ShapeToMove.TopLeftCorner.X - cursorPositionAtStart.X;
            _topLeftCornerRelativeToCursorPosition.Y = ShapeToMove.TopLeftCorner.Y - cursorPositionAtStart.Y;
        }

        private int CalculateSnappedCoordinate(
            int newShapeCentreCoord, 
            int boundaryRectangleOriginCoord, 
            int boundaryRectangleDimension, 
            int numSquaresOnSmallestSide, 
            int numSquaresOnLargestSide)
        {
            var squareWidth = ShapeSetBuilder.SquareWidth;

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
    }
}