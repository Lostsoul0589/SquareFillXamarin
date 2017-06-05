using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SquareFillDomain.Builders;
using SquareFillDomain.Interfaces;
using SquareFillDomain.Models;
using SquareFillDomain.Utils;

namespace SquareFillDomain.Controllers
{
    public class ShapeController
    {
        public int CurrentShapeCornerX { get { return _shapeToMove.TopLeftCorner.X; } }
        public int CurrentShapeCornerY { get { return _shapeToMove.TopLeftCorner.Y; } }
        public int GameGridWidth { get { return _occupiedGridSquares.Width(); } }
        public int GameGridHeight { get { return _occupiedGridSquares.Height(); } }
        public int NumShapes { get { return _shapeSet.Shapes.Count(); } }

        private Shape _shapeToMove = null;
        private readonly Grid _occupiedGridSquares;
        private readonly ShapeSet _shapeSet = null;
    
        private readonly ShapeMover _shapeMover;
        private SquareFillPoint _lastGoodLocation;
        private bool _colliding = false;

        public ShapeController(IShapeSetBuilder shapeSetBuilder)
        {
            _occupiedGridSquares = shapeSetBuilder.MakeGridSquares();
            _shapeSet = shapeSetBuilder.GetShapeSet();
            PutAllShapesIntoGrid(shapeSetBuilder);

            _shapeMover = new ShapeMover();
            _lastGoodLocation = new SquareFillPoint(x:0, y:0);
        }

        public Shape StartMove(SquareFillPoint cursorPositionAtStart)
        {
            _shapeToMove = _shapeSet.SelectShape(selectedPoint: cursorPositionAtStart);
        
            if (_shapeToMove != null) 
            {
                _shapeToMove.VacateGridSquares(occupiedGridSquares: _occupiedGridSquares);
                _shapeMover.StartMove(cursorPositionAtStart: cursorPositionAtStart, shapeToMove: _shapeToMove);
                _lastGoodLocation = cursorPositionAtStart;
            }

            return _shapeToMove;
        }

        public void ContinueMove(SquareFillPoint newLocation) 
        {
            if (_shapeToMove != null)
            {
                SquareFillPoint newTopLeftCorner = _shapeMover.CalculateTopLeftCorner(newCursorPosition: newLocation);
                SquareFillPoint positionInGrid = CalculateGridPosition(topLeftCorner: newTopLeftCorner);
                var logger = new Logger();

                bool cursorIsInShape = _shapeToMove.IsInShape(point: newLocation);
                MovementResult movementResult = CheckWhetherMovementIsPossible(
                    newTopLeftCorner: newTopLeftCorner,
                    logger: logger);

                logger = logger
                    .Clear()
                    .Plus(desc: "Cursor", point: newLocation)
                    .Plus(desc: "NewCorner", point: newTopLeftCorner)
                    .Plus(desc: "NewGrid", point: positionInGrid);
            
                if(_colliding == false) {
                    if(movementResult.NoShapesAreInTheWay)
                    {
                        _shapeMover.MoveToNewCursorPosition(newCursorPosition: newLocation);
                        _lastGoodLocation = newLocation;
                        LogMessagePlusOrigins(logger: logger, message: "Clear. ");
                    } 
                    else
                    {
                        _colliding = true;
                        _lastGoodLocation = _shapeMover.CalculateCursorPosition();
                        _shapeMover.SnapToGridInRelevantDimensionsIfPossible(
                            movementResult: movementResult,
                            occupiedGridSquares: _occupiedGridSquares);
                        LogMessagePlusOrigins(logger: logger, message: "Blocked. ");
                    }
                }
                else 
                {
                    if (cursorIsInShape)
                    {
                        _shapeMover.StartMove(cursorPositionAtStart: newLocation, shapeToMove: _shapeToMove);
                        _lastGoodLocation = newLocation;
                        _colliding = false;
                        LogMessagePlusOrigins(logger: logger, message: "Moving again. ");
                    }
                }
            }
        }
    
        public void EndMove(SquareFillPoint finalLocation) 
        {
            if (_shapeToMove != null)
            {
                SquareFillPoint newTopLeftCorner = _shapeMover.CalculateTopLeftCorner(newCursorPosition: finalLocation);
                MovementResult movementResult = _shapeToMove.AttemptToUpdateOrigins(
                    occupiedGridSquares: _occupiedGridSquares,
                    newTopLeftCorner: newTopLeftCorner);
            
                if(movementResult.NoShapesAreInTheWay && !_colliding)
                {
                    LogLocation(message: "End move with no obstacles.", locationName: "Final", location: finalLocation);
                    _shapeMover.SnapToGrid(newCursorPosition: finalLocation);
                } 
                else
                {
                    LogLocation(message: "End move with obstacles.", locationName: "LastGood",
                        location: _lastGoodLocation);
                    _shapeMover.SnapToGrid(newCursorPosition: _lastGoodLocation);
                }
            
                _shapeToMove.OccupyGridSquares(occupiedGridSquares: _occupiedGridSquares);
                _shapeToMove = null;
                _colliding = false;
            }
        }

        private MovementResult CheckWhetherMovementIsPossible(SquareFillPoint newTopLeftCorner, Logger logger)
        {
            logger.Clear().Plus(desc: "Origins1", shape: _shapeToMove).Log();
            var movementResult = _shapeToMove.AttemptToUpdateOrigins(
                occupiedGridSquares: _occupiedGridSquares,
                newTopLeftCorner: newTopLeftCorner);
            logger.Clear().Plus(desc: "Origins2", shape: _shapeToMove).Log();

            return movementResult;
        }

        private SquareFillPoint CalculateGridPosition(SquareFillPoint topLeftCorner)
        {
            return new SquareFillPoint(
                x: topLeftCorner.X / ShapeConstants.SquareWidth,
                y: topLeftCorner.Y / ShapeConstants.SquareWidth);
        }

        private void LogMessagePlusOrigins(Logger logger, string message)
        {
            logger
                .Plus(desc: "Origins", shape: _shapeToMove)
                .Make(message: message)
                .Log();
        }

        private void LogLocation(string message, string locationName, SquareFillPoint location)
        {
            string xCoord = location.X.ToString();
            string yCoord = location.Y.ToString();
            Debug.WriteLine(message + " " + locationName + "(x:" + xCoord + ",y:" + yCoord + ")");
        }

        private void PutAllShapesIntoGrid(IShapeSetBuilder shapeSetBuilder)
        {
            shapeSetBuilder.OccupyBorderSquares(occupiedGridSquares: _occupiedGridSquares);
            foreach (var shape in _shapeSet.Shapes)
            {
                shape.OccupyGridSquares(occupiedGridSquares: _occupiedGridSquares);
            }
        }

        public void OccupyAllGridSquares()
        {
            _occupiedGridSquares.OccupyAllSquares();
        }

        public bool IsSquareOccupied(int x, int y)
        {
            return _occupiedGridSquares.IsSquareOccupied(x: x, y: y);
        }

        public int NumSquares(int shapeIndex)
        {
            return _shapeSet.Shapes.ElementAt(shapeIndex).NumSquares;
        }

        public int SquareCornerX(int shapeIndex, int squareIndex)
        {
            return _shapeSet.Shapes.ElementAt(shapeIndex).SquareCornerX(squareIndex: squareIndex);
        }

        public int SquareCornerY(int shapeIndex, int squareIndex)
        {
            return _shapeSet.Shapes.ElementAt(shapeIndex).SquareCornerY(squareIndex: squareIndex);
        }
    }
}