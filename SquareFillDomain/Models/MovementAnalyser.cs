using System.Collections.Generic;
using SquareFillDomain.Utils;

namespace SquareFillDomain.Models
{
    public class MovementAnalyser
    {
        // private(set) var ShapeHasCrossedAHorizontalGridBoundary: Bool;
        public bool ShapeHasCrossedAHorizontalGridBoundary { get; private set; }
        public bool ShapeHasCrossedAVerticalGridBoundary { get; private set; }
        public bool ThereAreShapesInTheWay { get; private set; }

        // public var NoShapesAreInTheWay: Bool { get { return !ThereAreShapesInTheWay; } }
        public bool NoShapesAreInTheWay { get { return !ThereAreShapesInTheWay; } }

        // init(
        //      squares: [Square],
        //      occupiedGridSquares: Grid,
        //      newTopLeftCorner: SquareFillPoint) 
        public MovementAnalyser(
            List<Square> squares,
            Grid occupiedGridSquares,
            SquareFillPoint newTopLeftCorner)
        {
            // Even though these values are initialised in the methods below, 
            // we have to set them here too otherwise Swift will complain.
            ShapeHasCrossedAHorizontalGridBoundary = false;
            ShapeHasCrossedAVerticalGridBoundary = false;
            ThereAreShapesInTheWay = false;

            CheckWhetherBoundariesHaveBeenCrossed(anySquare: squares[0], newTopLeftCorner: newTopLeftCorner);

            foreach (var element in squares) {
                CheckWhetherMovementIsPossible(
                    square: element,
                    occupiedGridSquares: occupiedGridSquares,
                    newParentTopLeftCorner: newTopLeftCorner);
            }
        }

        // private func CheckWhetherBoundariesHaveBeenCrossed(
        //      anySquare: Square,
        //      newTopLeftCorner: SquareFillPoint)
        private void CheckWhetherBoundariesHaveBeenCrossed(
            Square anySquare,
            SquareFillPoint newTopLeftCorner)
        {
            var oldSquareGridOrigin = anySquare.GetGridOrigin();
            var newSquareTopLeftCorner = anySquare.CalculatePotentialTopLeftCorner(parentTopLeftCorner: newTopLeftCorner);
            var newSquareGridOrigin = CalculateGridOrigin(topLeftCorner: newSquareTopLeftCorner);

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

        // private func CheckWhetherMovementIsPossible(
        //      square: Square,
        //      occupiedGridSquares: Grid,
        //      newParentTopLeftCorner: SquareFillPoint)
        private void CheckWhetherMovementIsPossible(
            Square square,
            Grid occupiedGridSquares,
            SquareFillPoint newParentTopLeftCorner)
        {
            var newSquareTopLeftCorner = square.CalculatePotentialTopLeftCorner(parentTopLeftCorner: newParentTopLeftCorner);
            var newSquareGridOrigin = CalculateGridOrigin(topLeftCorner: newSquareTopLeftCorner);

            if (ShapeHasCrossedAHorizontalGridBoundary
                || ShapeHasCrossedAVerticalGridBoundary)
            {
                ThereAreShapesInTheWay = IsSomethingInTheWay(
                    newSquareTopLeftCorner: newSquareTopLeftCorner,
                    newSquareGridOrigin: newSquareGridOrigin,
                    occupiedGridSquares: occupiedGridSquares);
            }
        }

        // private func IsSomethingInTheWay(
        //      newSquareTopLeftCorner: SquareFillPoint,
        //      newSquareGridOrigin: SquareFillPoint,
        //      occupiedGridSquares: Grid) -> Bool
        private bool IsSomethingInTheWay(
            SquareFillPoint newSquareTopLeftCorner,
            SquareFillPoint newSquareGridOrigin,
            Grid occupiedGridSquares)
        {
            var somethingIsInTheWay = ThereAreShapesInTheWay;

            var newGridXCoords = GetNewGridCoordinates(newPixelValue: newSquareTopLeftCorner.X, newGridCoord: newSquareGridOrigin.X);
            var newGridYCoords = GetNewGridCoordinates(newPixelValue: newSquareTopLeftCorner.Y, newGridCoord: newSquareGridOrigin.Y);

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

        // private func CalculateGridOrigin(topLeftCorner: SquareFillPoint) -> SquareFillPoint
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

        // private func GetNewGridCoordinates(newPixelValue: Int, newGridCoord: Int) -> [Int]
        private List<int> GetNewGridCoordinates(int newPixelValue, int newGridCoord)
        {
            var newGridCoords = new List<int>();

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

        // private func HasBoundaryBeenCrossed(
        //      oldPixelValue: Int,
        //      newPixelValue: Int,
        //      oldGridRef: Int,
        //      newGridRef: Int) -> Bool
        private bool HasBoundaryBeenCrossed(
            int oldPixelValue,
            int newPixelValue,
            int oldGridRef,
            int newGridRef)
        {
            return OnePositionIsAlignedWithGridButTheOtherIsNot(pixelValue1: oldPixelValue, pixelValue2: newPixelValue)
                   || TheyAreInDifferentGridSquares(gridRef1: oldGridRef, gridRef2: newGridRef);
        }

        // private func OnePositionIsAlignedWithGridButTheOtherIsNot(pixelValue1: Int, pixelValue2: Int) -> Bool
        private bool OnePositionIsAlignedWithGridButTheOtherIsNot(int pixelValue1, int pixelValue2)
        {
            return DivisibleBySquareWidth(value: pixelValue1) != DivisibleBySquareWidth(value: pixelValue2);
        }

        // private func TheyAreInDifferentGridSquares(gridRef1: Int, gridRef2: Int) -> Bool
        private bool TheyAreInDifferentGridSquares(int gridRef1, int gridRef2)
        {
            return gridRef1 != gridRef2;
        }

        // private func DivisibleBySquareWidth(value: Int) -> Bool
        private bool DivisibleBySquareWidth(int value)
        {
            return (value % ShapeConstants.SquareWidth) == 0;
        }

        private SquareFillPoint SquareFillPoint(int x, int y)
        {
            return new SquareFillPoint(x: x, y: y);
        }
    }
}