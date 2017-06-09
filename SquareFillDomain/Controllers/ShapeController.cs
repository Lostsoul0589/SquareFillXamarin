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
        public int CurrentShapeCornerX { get { return _shapeToMove.TopLeftCornerX; } }
        public int CurrentShapeCornerY { get { return _shapeToMove.TopLeftCornerY; } }

        private Shape _shapeToMove = null;
        private readonly Grid _occupiedGridSquares;
        private readonly ShapeSet _shapeSet = null;
    
        private readonly ShapeMover _shapeMover;
        private SquareFillPoint _lastGoodLocation;
        private bool _colliding = false;
        private readonly Logger _logger = new Logger();

        public ShapeController(ShapeSet shapeSet, Grid occupiedGridSquares)
        {
            _occupiedGridSquares = occupiedGridSquares;
            _shapeSet = shapeSet;

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

                bool cursorIsInShape = _shapeToMove.IsInShape(point: newLocation);
                MovementResult movementResult = CheckWhetherMovementIsPossible(newTopLeftCorner: newTopLeftCorner);

                NoteLocation(cursor: newLocation, topLeftCorner: newTopLeftCorner, gridPosition: positionInGrid);

                if (_colliding == false) {
                    if(movementResult.NoShapesAreInTheWay)
                    {
                        _shapeMover.MoveToNewCursorPosition(newCursorPosition: newLocation);
                        _lastGoodLocation = newLocation;
                        LogMessagePlusOrigins(message: "Clear. ");
                    } 
                    else
                    {
                        _colliding = true;
                        _lastGoodLocation = _shapeMover.CalculateCursorPosition();
                        _shapeToMove.SnapToGridInRelevantDimensionsIfPossible(
                            movementResult: movementResult,
                            occupiedGridSquares: _occupiedGridSquares);
                        LogMessagePlusOrigins(message: "Blocked. ");
                    }
                }
                else 
                {
                    if (cursorIsInShape)
                    {
                        _shapeMover.StartMove(cursorPositionAtStart: newLocation, shapeToMove: _shapeToMove);
                        _lastGoodLocation = newLocation;
                        _colliding = false;
                        LogMessagePlusOrigins(message: "Moving again. ");
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
                    LogLocation(message: "End move with obstacles.", locationName: "LastGood", location: _lastGoodLocation);
                    _shapeMover.SnapToGrid(newCursorPosition: _lastGoodLocation);
                }
            
                _shapeToMove.OccupyGridSquares(occupiedGridSquares: _occupiedGridSquares);
                _shapeToMove = null;
                _colliding = false;
            }
        }

        private MovementResult CheckWhetherMovementIsPossible(SquareFillPoint newTopLeftCorner)
        {
            _logger.Clear().WithShape(desc: "Origins1", shape: _shapeToMove).Log();
            var movementResult = _shapeToMove.AttemptToUpdateOrigins(
                occupiedGridSquares: _occupiedGridSquares,
                newTopLeftCorner: newTopLeftCorner);
            _logger.Clear().WithShape(desc: "Origins2", shape: _shapeToMove).Log();

            return movementResult;
        }

        private SquareFillPoint CalculateGridPosition(SquareFillPoint topLeftCorner)
        {
            return new SquareFillPoint(
                x: topLeftCorner.X / ShapeConstants.SquareWidth,
                y: topLeftCorner.Y / ShapeConstants.SquareWidth);
        }

        private void NoteLocation(SquareFillPoint cursor, SquareFillPoint topLeftCorner, SquareFillPoint gridPosition)
        {
            _logger
                 .Clear()
                 .WithPoint(desc: "Cursor", point: cursor)
                 .WithPoint(desc: "NewCorner", point: topLeftCorner)
                 .WithPoint(desc: "NewGrid", point: gridPosition);
        }

        private void LogMessagePlusOrigins(string message)
        {
            _logger
                .WithShape(desc: "Origins", shape: _shapeToMove)
                .WithMessage(message: message)
                .Log();
        }

        private void LogLocation(string message, string locationName, SquareFillPoint location)
        {
            _logger
                .WithMessage(message: message)
                .WithPoint(desc: locationName, point: location)
                .Log();
        }
    }
}