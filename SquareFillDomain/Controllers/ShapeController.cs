using System;
using System.Collections.Generic;
using System.Diagnostics;
using SquareFillDomain.Builders;
using SquareFillDomain.Models;
using SquareFillDomain.Utils;

namespace SquareFillDomain.Controllers
{
    public class ShapeController
    {
        public Shape ShapeToMove = null;
        public List<List<GridSquare>> OccupiedGridSquares = new List<List<GridSquare>>();
        public ShapeSet ShapeSet = null;
    
        private ShapeMover _shapeMover;
        private List<List<Shape>> _shapesInGrid = new List<List<Shape>>();
        private List<List<bool>> _occupied = new List<List<bool>>();
        private UIView _view;
        private CGPoint _lastGoodLocation;
        private bool _colliding = false;

        public ShapeController(UIView view)
        {
            _view = view;

            OccupiedGridSquares = ShapeSetBuilder.MakeGridSquares();
            ShapeSetBuilder.MakeGameGrid(view: _view, occupiedGridSquares: OccupiedGridSquares);
            ShapeSet = ShapeSetBuilder.MakeHardCodedShapeSet(view: _view);
            PutAllShapesIntoGrid();

            _shapeMover = new ShapeMover(
                screenWidth: view.Frame.Width, 
                screenHeight: view.Frame.Height);

            _lastGoodLocation = new CGPoint(x:0, y:0);
        }

        public void StartMove(CGPoint cursorPositionAtStart)
        {
             ShapeToMove = ShapeSet.SelectShape(selectedPoint: cursorPositionAtStart);
        
            if (ShapeToMove != null) 
            {
                ShapeToMove.VacateGridSquares(occupiedGridSquares: OccupiedGridSquares);
                _shapeMover.StartMove(cursorPositionAtStart: cursorPositionAtStart, shapeToMove: ShapeToMove);
                _lastGoodLocation = cursorPositionAtStart;
            }
        }

        public void ContinueMove(CGPoint newLocation) 
        {
            if (ShapeToMove != null)
            {
                CGPoint newShapeCentre = _shapeMover.CalculateShapeCentre(newCursorPosition: newLocation);
                CGPoint positionInGrid = CalculateGridPosition(shapeCentre: newShapeCentre);
                var logger = new Logger();

                bool cursorIsInShape = ShapeToMove.IsInShape(point: newLocation);
                MovementResult movementResult = CheckWhetherMovementIsPossible(
                    newShapeCentre: newShapeCentre,
                    logger: logger);

                logger = logger
                    .Clear()
                    .Plus(desc: "Cursor", point: newLocation)
                    .Plus(desc: "NewCent", point: newShapeCentre)
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
                        _lastGoodLocation = _shapeMover.CalculateCursorPosition(centreOfShape: ShapeToMove.CentreOfShape);
                        SnapToGridInRelevantDimensionsIfPossible(
                            movementResult: movementResult,
                            previousShapeCentre: ShapeToMove.CentreOfShape,
                            shapeToMove: ShapeToMove);
                        ShapeToMove.CalculateOrigins(newCentreOfShape: ShapeToMove.CentreOfShape);
                        LogMessagePlusOrigins(logger: logger, message: "Blocked. ");
                    }
                }
                else 
                {
                    if (cursorIsInShape)
                    {
                        _shapeMover.StartMove(cursorPositionAtStart: newLocation, shapeToMove: ShapeToMove);
                        _lastGoodLocation = newLocation;
                        _colliding = false;
                        LogMessagePlusOrigins(logger: logger, message: "Moving again. ");
                    }
                }
            }
        }
    
        public void EndMove(CGPoint finalLocation) 
        {
            if (ShapeToMove != null)
            {
                CGPoint newShapeCentre = _shapeMover.CalculateShapeCentre(newCursorPosition: finalLocation);
                MovementResult movementResult = ShapeToMove.AttemptToUpdateOrigins(
                    occupiedGridSquares: OccupiedGridSquares,
                    newShapeCentre: newShapeCentre);
            
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
            
                ShapeToMove.OccupyGridSquares(occupiedGridSquares: OccupiedGridSquares);
                ShapeToMove = null;
                _colliding = false;
            }
        }

        private MovementResult CheckWhetherMovementIsPossible(CGPoint newShapeCentre, Logger logger)
        {
            logger.Clear().Plus(desc: "Origins1", squares: ShapeToMove.Squares).Log();
            var movementResult = ShapeToMove.AttemptToUpdateOrigins(
                occupiedGridSquares: OccupiedGridSquares,
                newShapeCentre: newShapeCentre);
            logger.Clear().Plus(desc: "Origins2", squares: ShapeToMove.Squares).Log();

            return movementResult;
        }

        private CGPoint CalculateGridPosition(CGPoint shapeCentre)
        {
            return new CGPoint(
                x:
                    Convert.ToInt16(shapeCentre.X - ShapeSetBuilder.SquareWidth/2)/
                    Convert.ToInt16(ShapeSetBuilder.SquareWidth),
                y:
                    Convert.ToInt16(shapeCentre.Y - ShapeSetBuilder.SquareWidth/2)/
                    Convert.ToInt16(ShapeSetBuilder.SquareWidth));
        }

        private void LogMessagePlusOrigins(Logger logger, string message)
        {
            logger
                .Plus(desc: "Origins", squares: ShapeToMove.Squares)
                .Make(message: message)
                .Log();
        }

        private void LogLocation(string message, string locationName, CGPoint location)
        {
            string xCoord = Convert.ToInt16(location.X).ToString();
            string yCoord = Convert.ToInt16(location.Y).ToString();
            Debug.WriteLine(message + " " + locationName + "(x:" + xCoord + ",y:" + yCoord + ")");
        }

        private void PutAllShapesIntoGrid()
        {
            foreach (var shape in ShapeSet.Shapes)
            {
                shape.OccupyGridSquares(occupiedGridSquares: OccupiedGridSquares);
            }
        }

        private void SnapToGridInRelevantDimensionsIfPossible(
            MovementResult movementResult, 
            CGPoint previousShapeCentre, 
            Shape shapeToMove)
        {
            var newShapeCentre = new CGPoint(x: previousShapeCentre.X, y: previousShapeCentre.Y);
        
            if (movementResult.ShapeHasCrossedAHorizontalGridBoundary)
            {
                newShapeCentre.X = _shapeMover.CalculateSnappedX(newShapeCentreX: newShapeCentre.X);
            }
        
            if (movementResult.ShapeHasCrossedAVerticalGridBoundary)
            {
                newShapeCentre.Y = _shapeMover.CalculateSnappedY(newShapeCentreY: newShapeCentre.Y);
            }

            MovementResult newMovementResult = ShapeToMove.AttemptToUpdateOrigins(
                occupiedGridSquares: OccupiedGridSquares,
                newShapeCentre: newShapeCentre);
        
            if (newMovementResult.NoShapesAreInTheWay) {
                ShapeToMove.PutShapeInNewLocation(newCentreOfShape: newShapeCentre);
                ShapeToMove.CalculateOrigins(newCentreOfShape: newShapeCentre);
            }
        }
    }
}