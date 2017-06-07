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
            for (int xCoord = 0; xCoord < width; xCoord++)
            {
                _gridSquares.Add(new List<GridSquare>());

                for (int yCoord = 0; yCoord < height; yCoord++)
                {
                    _gridSquares[xCoord].Add(new GridSquare());
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
            foreach (var occupiedGridSquareArray in _gridSquares)
            {
                foreach (var occupiedGridSquare in occupiedGridSquareArray)
                {
                    occupiedGridSquare.Occupied = true;
                }
            }
        }

        public void VacateAllSquares()
        {
            foreach (var gridSquareArray in _gridSquares)
            {
                foreach (var gridSquare in gridSquareArray)
                {
                    gridSquare.Occupied = false;
                    gridSquare.ShapeInSquare = null;
                }
            }
        }
    }
}