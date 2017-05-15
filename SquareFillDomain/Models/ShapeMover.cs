using System;
using SquareFillDomain.Builders;

namespace SquareFillDomain.Models
{
    internal class ShapeMover
    {
        public Shape ShapeToMove { get; private set; }
        public nfloat ScreenWidth { get; private set; }
        public nfloat ScreenHeight { get; private set; }

        private CGPoint _shapeCentreRelativeToCursorPosition = new CGPoint(x: 0, y: 0);

        public ShapeMover(nfloat screenWidth, nfloat screenHeight)
        {
            ScreenWidth = RoundDimensionDownToMultipleOfSquareWidth(screenDimension: screenWidth);
            ScreenHeight = RoundDimensionDownToMultipleOfSquareWidth(screenDimension: screenHeight);
        }

        public void StartMove(CGPoint cursorPositionAtStart, Shape shapeToMove)
        {
            ShapeToMove = shapeToMove;
            _shapeCentreRelativeToCursorPosition.X = ShapeToMove.CentreOfShape.X - cursorPositionAtStart.X;
            _shapeCentreRelativeToCursorPosition.Y = ShapeToMove.CentreOfShape.Y - cursorPositionAtStart.Y;
        }

        public CGPoint CalculateShapeCentre(CGPoint newCursorPosition)
        {
            return new CGPoint(
                x: newCursorPosition.X + _shapeCentreRelativeToCursorPosition.X,
                y: newCursorPosition.Y + _shapeCentreRelativeToCursorPosition.Y);
        }

        public CGPoint CalculateCursorPosition(CGPoint centreOfShape)
        {
            return new CGPoint(
                x: centreOfShape.X - _shapeCentreRelativeToCursorPosition.X,
                y: centreOfShape.Y - _shapeCentreRelativeToCursorPosition.Y);
        }

        public void MoveToNewCursorPosition(CGPoint newCursorPosition)
        {
            if (ShapeToMove != null)
            {
                var newShapeCentre = CalculateShapeCentre(newCursorPosition: newCursorPosition);
                ShapeToMove.PutShapeInNewLocation(newCentreOfShape: newShapeCentre);
            }
        }

        public void SnapToGrid(CGPoint newCursorPosition)
        {
            if (ShapeToMove != null)
            {
                var shapeCentreTakingRelativePositionIntoAccount =
                    CalculateShapeCentre(newCursorPosition: newCursorPosition);
                var newShapeCentre = new CGPoint(
                    x: CalculateSnappedX(newShapeCentreX: shapeCentreTakingRelativePositionIntoAccount.X),
                    y: CalculateSnappedY(newShapeCentreY: shapeCentreTakingRelativePositionIntoAccount.Y));
        
                ShapeToMove.PutShapeInNewLocation(newCentreOfShape: newShapeCentre);
                ShapeToMove.CalculateOrigins(newCentreOfShape: newShapeCentre);
            }
        }

        public nfloat CalculateSnappedX(nfloat newShapeCentreX)
        {
            return CalculateSnappedCoordinate(
                newShapeCentreCoord: newShapeCentreX,
                boundaryRectangleOriginCoord: 0,
                boundaryRectangleDimension: ScreenWidth,
                numSquaresOnSmallestSide: Convert.ToInt16(ShapeToMove.NumSquaresLeftOfShapeCentre),
                numSquaresOnLargestSide: Convert.ToInt16(ShapeToMove.NumSquaresRightOfShapeCentre));
        }

        public nfloat CalculateSnappedY(nfloat newShapeCentreY)
        {
            return CalculateSnappedCoordinate(
                newShapeCentreCoord: newShapeCentreY,
                boundaryRectangleOriginCoord: 0,
                boundaryRectangleDimension: ScreenHeight,
                numSquaresOnSmallestSide: Convert.ToInt16(ShapeToMove.NumSquaresAboveShapeCentre),
                numSquaresOnLargestSide: Convert.ToInt16(ShapeToMove.NumSquaresBelowShapeCentre));
        }

        private nfloat RoundDimensionDownToMultipleOfSquareWidth(nfloat screenDimension)
        {
            var maxNumberOfGridSquaresInDimension =
                Convert.ToInt16(Convert.ToInt16(screenDimension)/Convert.ToInt16(ShapeSetBuilder.SquareWidth));
            return (nfloat)(maxNumberOfGridSquaresInDimension*ShapeSetBuilder.SquareWidth);
        }

        private nfloat CalculateSnappedCoordinate(
            nfloat newShapeCentreCoord, 
            int boundaryRectangleOriginCoord, 
            nfloat boundaryRectangleDimension, 
            int numSquaresOnSmallestSide, 
            int numSquaresOnLargestSide)
        {
            var squareWidth = ShapeSetBuilder.SquareWidth;

            var shapeCentreCoord = newShapeCentreCoord;
            int numberOfSquaresFromEdgeOfScreen = Convert.ToInt16(shapeCentreCoord)/Convert.ToInt16(squareWidth);

            var potentialNewSquareCentre = numberOfSquaresFromEdgeOfScreen*Convert.ToInt16(squareWidth) +
                                           Convert.ToInt16(Convert.ToInt16(squareWidth)/2);

            var squareCentreAtOneEndOfContainer = boundaryRectangleOriginCoord + squareWidth/2;

            var squareCentreAtOtherEndOfContainer =
                boundaryRectangleOriginCoord + boundaryRectangleDimension - squareWidth/2;

            var potentialCentreOfShapeEdgeOnOneSide =
                potentialNewSquareCentre - (numSquaresOnSmallestSide*Convert.ToInt16(squareWidth));

            var centreOfShapeEdgeOnOneSideAdjustedForSmallestContainerEdge =
                Math.Max(potentialCentreOfShapeEdgeOnOneSide,
                    Convert.ToInt16(squareCentreAtOneEndOfContainer));

            var shapeCentreAdjustedForSmallestContainerEdge =
                centreOfShapeEdgeOnOneSideAdjustedForSmallestContainerEdge
                + (numSquaresOnSmallestSide*Convert.ToInt16(squareWidth));

            var potentialCentreOfShapeEdgeOnOtherSide = Convert.ToInt16(shapeCentreAdjustedForSmallestContainerEdge) +
                                                        (numSquaresOnLargestSide*Convert.ToInt16(squareWidth));

            var centreOfShapeEdgeOnOtherSideAdjustedForBothContainerEdges =
                Math.Min(potentialCentreOfShapeEdgeOnOtherSide,
                    Convert.ToInt16(squareCentreAtOtherEndOfContainer));

            nfloat actualSquareCentre = centreOfShapeEdgeOnOtherSideAdjustedForBothContainerEdges
                                     - (numSquaresOnLargestSide*Convert.ToInt16(squareWidth));

            return actualSquareCentre;
        }
    }
}