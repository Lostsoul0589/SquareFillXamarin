using System.Collections.Generic;
using SquareFillDomain.Utils;

namespace SquareFillDomain.Models
{
    public class Grid
    {
        public int Width { get { return _gridSquares.Count; } }
        public int Height { get { return _gridSquares[0].Count; } }

        private readonly List<List<GridSquare>> _gridSquares = new List<List<GridSquare>>();

        public Grid(int width, int height)
        {
            Initialise(width: width, height: height);
        }

        private void Initialise(int width, int height)
        {
            var start1 = 0;
            var end1 = width - 1;
            for (int count1 = start1; count1 <= end1; count1++)
            {
                _gridSquares.Add(new List<GridSquare>());

                var start2 = 0;
                var end2 = height - 1;
                for (int count2 = start2; count2 <= end2; count2++)
                {
                    _gridSquares[count1].Add(new GridSquare());
                }
            }
        }

        public void VacateGridSquareUsingPixels(SquareFillPoint gridReferenceInPixels)
        {
            ChangeGridSquareOccupation(
                gridReferenceInPixels: gridReferenceInPixels,
                shapeInSquare: null,
                occupied: false);
        }

        public void OccupyGridSquareUsingPixels(SquareFillPoint gridReferenceInPixels, Shape shapeInSquare)
        {
            ChangeGridSquareOccupation(
                gridReferenceInPixels: gridReferenceInPixels, 
                shapeInSquare: shapeInSquare, 
                occupied: true);
        }

        private void ChangeGridSquareOccupation(
            SquareFillPoint gridReferenceInPixels,
            Shape shapeInSquare,
            bool occupied)
        {
            int occupiedXCoordinate = gridReferenceInPixels.X / ShapeConstants.SquareWidth;
            int occupiedYCoordinate = gridReferenceInPixels.Y / ShapeConstants.SquareWidth;

            OccupyGridSquare(x: occupiedXCoordinate, y: occupiedYCoordinate, occupied: occupied);
            PlaceShapeInSquare(x: occupiedXCoordinate, y: occupiedYCoordinate, shapeInSquare: shapeInSquare);
        }

        public void OccupyGridSquare(int x, int y, bool occupied = true)
        {
            _gridSquares[x][y].Occupied = occupied;
        }

        public void PlaceShapeInSquare(int x, int y, Shape shapeInSquare)
        {
            _gridSquares[x][y].ShapeInSquare = shapeInSquare;
        }

        public bool IsSquareOccupied(int x, int y)
        {
            return _gridSquares[x][y].Occupied;
        }

        public void OccupyAllSquares()
        {
            foreach (var outerElement in _gridSquares) {
                foreach (var innerElement in outerElement) {
                    innerElement.Occupied = true;
                }
            }
        }

        public void VacateAllSquares()
        {
            foreach (var outerElement in _gridSquares) {
                foreach (var innerElement in outerElement) {
                    innerElement.Occupied = false;
                    innerElement.ShapeInSquare = null;
                }
            }
        }
    }
}