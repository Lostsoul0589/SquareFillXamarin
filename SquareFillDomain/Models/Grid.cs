using System.Collections.Generic;
using SquareFillDomain.Utils;

namespace SquareFillDomain.Models
{
    public class Grid
    {
        public int Width { get { return _gridSquares.Count; } }
        public int Height { get { return _gridSquares[0].Count; } }

        // private var _gridSquares: [[GridSquare]] = [];
        // private var _occupied: [[Bool]] = [[Bool]](repeating: [Bool](repeating: false, count: 7), count: 7);
        private readonly List<List<GridSquare>> _gridSquares = new List<List<GridSquare>>();

        // init (width: Int, height: Int)
        public Grid(int width, int height)
        {
            Initialise(width: width, height: height);
        }

        // public func VacateGridSquareUsingPixels(gridReferenceInPixels: SquareFillPoint)
        public void VacateGridSquareUsingPixels(SquareFillPoint gridReferenceInPixels)
        {
            ChangeGridSquareOccupation(
                gridReferenceInPixels: gridReferenceInPixels,
                shapeInSquare: null,
                occupied: false);
        }

        // public func OccupyGridSquareUsingPixels(gridReferenceInPixels: SquareFillPoint, shapeInSquare: Shape)
        public void OccupyGridSquareUsingPixels(SquareFillPoint gridReferenceInPixels, Shape shapeInSquare)
        {
            ChangeGridSquareOccupation(
                gridReferenceInPixels: gridReferenceInPixels, 
                shapeInSquare: shapeInSquare, 
                occupied: true);
        }

        // public func OccupyGridSquare(x: Int, y: Int, occupied: Bool = true)
        public void OccupyGridSquare(int x, int y, bool occupied = true)
        {
            _gridSquares[x][y].Occupied = occupied;
        }

        // public func PlaceShapeInSquare(int x, int y, shapeInSquare: Shape)
        public void PlaceShapeInSquare(int x, int y, Shape shapeInSquare)
        {
            _gridSquares[x][y].ShapeInSquare = shapeInSquare;
        }

        // public func IsSquareOccupied(int x, int y) -> Bool
        public bool IsSquareOccupied(int x, int y)
        {
            return _gridSquares[x][y].Occupied;
        }

        // public func OccupyAllSquares()
        public void OccupyAllSquares()
        {
            foreach (var outerElement in _gridSquares) {
                foreach (var innerElement in outerElement) {
                    innerElement.Occupied = true;
                }
            }
        }

        // public func VacateAllSquares()
        public void VacateAllSquares()
        {
            foreach (var outerElement in _gridSquares) {
                foreach (var innerElement in outerElement) {
                    innerElement.Occupied = false;
                    innerElement.ShapeInSquare = null;
                }
            }
        }

        // private func Initialise(width: Int, height: Int)
        private void Initialise(int width, int height)
        {
            var start1 = 0;
            var end1 = width - 1;
            for (int count1 = start1; count1 <= end1; count1++)
            {
                //_gridSquares.append([]);
                _gridSquares.Add(new List<GridSquare>());

                var start2 = 0;
                var end2 = height - 1;
                for (int count2 = start2; count2 <= end2; count2++)
                {
                    _gridSquares[count1].Add(new GridSquare());
                }
            }
        }

        // private func ChangeGridSquareOccupation(
        //      gridReferenceInPixels: SquareFillPoint,
        //      shapeInSquare: Shape,
        //      occupied: Bool)
        private void ChangeGridSquareOccupation(
            SquareFillPoint gridReferenceInPixels,
            Shape shapeInSquare,
            bool occupied)
        {
            var occupiedXCoordinate = gridReferenceInPixels.X / ShapeConstants.SquareWidth;
            var occupiedYCoordinate = gridReferenceInPixels.Y / ShapeConstants.SquareWidth;

            OccupyGridSquare(x: occupiedXCoordinate, y: occupiedYCoordinate, occupied: occupied);
            PlaceShapeInSquare(x: occupiedXCoordinate, y: occupiedYCoordinate, shapeInSquare: shapeInSquare);
        }
    }
}