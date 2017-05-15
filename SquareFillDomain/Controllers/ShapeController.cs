using System.Collections.Generic;
using System.Diagnostics;
using SquareFillDomain.Builders;
using SquareFillDomain.Interfaces;
using SquareFillDomain.Models;
using SquareFillDomain.Utils;

namespace SquareFillDomain.Controllers
{
    public class ShapeController
    {
        public Shape ShapeToMove = null;
        public List<List<GridSquare>> OccupiedGridSquares;
        public ShapeSet ShapeSet = null;
    
        private readonly ShapeMover _shapeMover;
        private List<List<Shape>> _shapesInGrid = new List<List<Shape>>();
        private List<List<bool>> _occupied = new List<List<bool>>();
        private SquareFillPoint _lastGoodLocation;
        private bool _colliding = false;

        public ShapeController(
            ISquareViewMaker squareViewMaker,
            int screenWidth,
            int screenHeight)
        {
            OccupiedGridSquares = ShapeSetBuilder.MakeGridSquares();
            ShapeSetBuilder.OccupyBorderSquares(occupiedGridSquares: OccupiedGridSquares);
            ShapeSet = ShapeSetBuilder.MakeHardCodedShapeSet(squareViewMaker: squareViewMaker);
            PutAllShapesIntoGrid();

            _shapeMover = new ShapeMover(
                screenWidth: screenWidth,
                screenHeight: screenHeight);

            _lastGoodLocation = new SquareFillPoint(x:0, y:0);
        }

        public void StartMove(SquareFillPoint cursorPositionAtStart)
        {
            ShapeToMove = ShapeSet.SelectShape(selectedPoint: cursorPositionAtStart);
        
            if (ShapeToMove != null) 
            {
                ShapeToMove.VacateGridSquares(occupiedGridSquares: OccupiedGridSquares);
                _shapeMover.StartMove(cursorPositionAtStart: cursorPositionAtStart, shapeToMove: ShapeToMove);
                _lastGoodLocation = cursorPositionAtStart;
            }
        }

        public void ContinueMove(SquareFillPoint newLocation) 
        {
            if (ShapeToMove != null)
            {
                SquareFillPoint newShapeCentre = _shapeMover.CalculateShapeCentre(newCursorPosition: newLocation);
                SquareFillPoint positionInGrid = CalculateGridPosition(shapeCentre: newShapeCentre);
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
    
        public void EndMove(SquareFillPoint finalLocation) 
        {
            if (ShapeToMove != null)
            {
                SquareFillPoint newShapeCentre = _shapeMover.CalculateShapeCentre(newCursorPosition: finalLocation);
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

        private MovementResult CheckWhetherMovementIsPossible(SquareFillPoint newShapeCentre, Logger logger)
        {
            logger.Clear().Plus(desc: "Origins1", squares: ShapeToMove.Squares).Log();
            var movementResult = ShapeToMove.AttemptToUpdateOrigins(
                occupiedGridSquares: OccupiedGridSquares,
                newShapeCentre: newShapeCentre);
            logger.Clear().Plus(desc: "Origins2", squares: ShapeToMove.Squares).Log();

            return movementResult;
        }

        private SquareFillPoint CalculateGridPosition(SquareFillPoint shapeCentre)
        {
            return new SquareFillPoint(
                x: (shapeCentre.X - ShapeSetBuilder.SquareWidth/2)/ShapeSetBuilder.SquareWidth,
                y: (shapeCentre.Y - ShapeSetBuilder.SquareWidth/2)/ShapeSetBuilder.SquareWidth);
        }

        private void LogMessagePlusOrigins(Logger logger, string message)
        {
            logger
                .Plus(desc: "Origins", squares: ShapeToMove.Squares)
                .Make(message: message)
                .Log();
        }

        private void LogLocation(string message, string locationName, SquareFillPoint location)
        {
            string xCoord = location.X.ToString();
            string yCoord = location.Y.ToString();
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
            SquareFillPoint previousShapeCentre, 
            Shape shapeToMove)
        {
            var newShapeCentre = new SquareFillPoint(x: previousShapeCentre.X, y: previousShapeCentre.Y);
        
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