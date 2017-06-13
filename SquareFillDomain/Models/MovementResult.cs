using System.Collections.Generic;
using SquareFillDomain.Utils;

namespace SquareFillDomain.Models
{
    public class MovementAnalyser
    {
        public bool ShapeHasCrossedAHorizontalGridBoundary { get; private set; }
        public bool ShapeHasCrossedAVerticalGridBoundary { get; private set; }
        public bool ThereAreShapesInTheWay { get; private set; }
        public bool NoShapesAreInTheWay { get { return !ThereAreShapesInTheWay; } }

        public MovementAnalyser(
            List<Square> squares,
            Grid occupiedGridSquares,
            SquareFillPoint newTopLeftCorner)
        {
            ThereAreShapesInTheWay = false;

            CheckWhetherBoundariesHaveBeenCrossed(anySquare: squares[0], newTopLeftCorner: newTopLeftCorner);

            foreach (var element in squares) {
                CheckWhetherMovementIsPossible(
                    square: element,
                    occupiedGridSquares: occupiedGridSquares,
                    newParentTopLeftCorner: newTopLeftCorner);
            }
        }

        private void CheckWhetherBoundariesHaveBeenCrossed(
            Square anySquare,
            SquareFillPoint newTopLeftCorner)
        {
            SquareFillPoint oldSquareGridOrigin = anySquare.GetGridOrigin();
            SquareFillPoint newSquareTopLeftCorner = anySquare.CalculatePotentialTopLeftCorner(parentTopLeftCorner: newTopLeftCorner);
            SquareFillPoint newSquareGridOrigin = CalculateGridOrigin(topLeftCorner: newSquareTopLeftCorner);

            ShapeHasCrossedAHorizontalGridBoundary = HasBoundaryBeenCrossed(
                oldPixelValue: anySquare.TopLeftCornerX,
                newPixelValue: newSquareTopLeftCorner.X,
                oldGridRef: oldSquareGridOrigin.X,
                newGridRef: newSquareGridOrigin.X);

            ShapeHasCrossedAVerticalGridBoundary = HasBoundaryBeenCrossed(
                oldPixelValue: anySquare.TopLeftCornerY,
                newPixelValue: newSquareTopLeftCorner.Y,
                oldGridRef: oldSquareGridOrigin.Y,
                newGridRef: newSquareGridOrigin.Y);
        }

        private void CheckWhetherMovementIsPossible(
            Square square,
            Grid occupiedGridSquares,
            SquareFillPoint newParentTopLeftCorner)
        {
            SquareFillPoint newSquareTopLeftCorner = square.CalculatePotentialTopLeftCorner(parentTopLeftCorner: newParentTopLeftCorner);
            SquareFillPoint newSquareGridOrigin = CalculateGridOrigin(topLeftCorner: newSquareTopLeftCorner);

            if (ShapeHasCrossedAHorizontalGridBoundary
                || ShapeHasCrossedAVerticalGridBoundary)
            {
                ThereAreShapesInTheWay = IsSomethingInTheWay(
                    newSquareTopLeftCorner: newSquareTopLeftCorner,
                    newSquareGridOrigin: newSquareGridOrigin,
                    occupiedGridSquares: occupiedGridSquares);
            }
        }

        private bool IsSomethingInTheWay(
            SquareFillPoint newSquareTopLeftCorner,
            SquareFillPoint newSquareGridOrigin,
            Grid occupiedGridSquares)
        {
            bool somethingIsInTheWay = ThereAreShapesInTheWay;

            List<int> newGridXCoords = GetNewGridCoordinates(newPixelValue: newSquareTopLeftCorner.X, newGridCoord: newSquareGridOrigin.X);
            List<int> newGridYCoords = GetNewGridCoordinates(newPixelValue: newSquareTopLeftCorner.Y, newGridCoord: newSquareGridOrigin.Y);

            foreach (var xCoord in newGridXCoords) {
                foreach (var yCoord in newGridYCoords) {
                    if (xCoord >= occupiedGridSquares.Width
                        || yCoord >= occupiedGridSquares.Height
                        || xCoord < 0
                        || yCoord < 0)
                    {
                        somethingIsInTheWay = true;
                    }
                    else
                    {
                        somethingIsInTheWay = somethingIsInTheWay || occupiedGridSquares.IsSquareOccupied(x: xCoord, y: yCoord);
                    }
                }
            }

            return somethingIsInTheWay;
        }

        private SquareFillPoint CalculateGridOrigin(SquareFillPoint topLeftCorner)
        {
            var gridOrigin = new SquareFillPoint(
                x: topLeftCorner.X / ShapeConstants.SquareWidth,
                y: topLeftCorner.Y / ShapeConstants.SquareWidth);

            if (topLeftCorner.X < 0)
            {
                gridOrigin.X = gridOrigin.X - 1;
            }

            if (topLeftCorner.Y < 0)
            {
                gridOrigin.Y = gridOrigin.Y - 1;
            }

            return gridOrigin;
        }

        private List<int> GetNewGridCoordinates(int newPixelValue, int newGridCoord)
        {
            List<int> newGridCoords = new List<int>();

            if (DivisibleBySquareWidth(value: newPixelValue))
            {
                newGridCoords.Add(newGridCoord);
            }
            else
            {
                newGridCoords.Add(newGridCoord);
                newGridCoords.Add(newGridCoord + 1);
            }

            return newGridCoords;
        }

        private bool HasBoundaryBeenCrossed(
            int oldPixelValue,
            int newPixelValue,
            int oldGridRef,
            int newGridRef)
        {
            return OnePositionIsAlignedWithGridButTheOtherIsNot(pixelValue1: oldPixelValue, pixelValue2: newPixelValue)
                   || TheyAreInDifferentGridSquares(gridRef1: oldGridRef, gridRef2: newGridRef);
        }

        private bool OnePositionIsAlignedWithGridButTheOtherIsNot(int pixelValue1, int pixelValue2)
        {
            return DivisibleBySquareWidth(value: pixelValue1) != DivisibleBySquareWidth(value: pixelValue2);
        }

        private bool TheyAreInDifferentGridSquares(int gridRef1, int gridRef2)
        {
            return gridRef1 != gridRef2;
        }

        private bool DivisibleBySquareWidth(int value)
        {
            return (value % ShapeConstants.SquareWidth) == 0;
        }
    }
}