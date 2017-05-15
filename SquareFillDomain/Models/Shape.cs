using System;
using System.Collections.Generic;
using SquareFillDomain.Builders;

namespace SquareFillDomain.Models
{
    public class Shape
	{
        public CGPoint CentreOfShape { get; private set; }
        public List<Square> Squares { get; private set; }

	    public nfloat NumSquaresLeftOfShapeCentre { get; private set; }
	    public nfloat NumSquaresRightOfShapeCentre { get; private set; }
	    public nfloat NumSquaresAboveShapeCentre { get; private set; }
	    public nfloat NumSquaresBelowShapeCentre { get; private set; }
    
        private UIView _view = null;

		public Shape(
			UIColor colour,
			CGPoint centreOfShape,
			List<CGPoint> relativePoints,
			UIView view,
			CGRect containingRectangle)
		{
            List<Square> squares = new List<Square>();

            foreach(var point in relativePoints)
            {
                var imageView = new UIImageView();
                imageView.Frame = new CGRect(
                    x: 0,
                    y: 0,
                    width: ShapeSetBuilder.SquareWidth,
                    height: ShapeSetBuilder.SquareWidth);
                imageView.BackgroundColor = colour;
                view.AddSubview(imageView);
                imageView.Layer.BorderColor = UIColor.Black.CGColor;
                imageView.Layer.BorderWidth = 1;

                squares.Add(new Square(positionRelativeToParent: point, sprite: imageView));
            }
        
            _view = view;
            CentreOfShape = centreOfShape;
            Squares = squares;
           
        
            Initialise();
		}

        public Shape(CGPoint centreOfShape,
         List<Square> squareDefinitions,
         CGRect containingRectangle)
        {
        
            CentreOfShape = centreOfShape;
            Squares = squareDefinitions;
        
            Initialise();
        }

        public bool IsInShape(CGPoint point)
        {
            bool isInShape = false;
            
            foreach(var square in Squares) 
            {
                isInShape = isInShape || square.IsInSquare(point: point);
            }

            return isInShape;
        }

        public void PutShapeInNewLocation(CGPoint newCentreOfShape)
        {
            CentreOfShape = newCentreOfShape;
            foreach(var square in Squares) 
            {
                if (square.Sprite != null)
                {
                    square.Sprite.Center = new CGPoint(
                        x: CentreOfShape.X + (square.PositionRelativeToParent.X*ShapeSetBuilder.SquareWidth),
                        y: CentreOfShape.Y + (square.PositionRelativeToParent.Y*ShapeSetBuilder.SquareWidth));
                }
            }
        }

        public void CalculateOrigins(CGPoint newCentreOfShape)
        {
            foreach (var square in Squares) 
            {
                square.CalculateOrigin(parentShapeCentre: newCentreOfShape);
            }
        }

         public MovementResult AttemptToUpdateOrigins(List<List<GridSquare>> occupiedGridSquares,
                                              CGPoint newShapeCentre)
        {
            bool somethingIsintheWay = false;
            var movementResult = new MovementResult();
        
            foreach (var square in Squares)
            {
                var newOrigin = square.CalculatePotentialOrigin(parentShapeCentre: newShapeCentre);

                List<int> newGridXCoords = new List<int>();
                List<int> newGridYCoords = new List<int>();

                float oldGridXCoord = Convert.ToInt16(square.Origin.X)/Convert.ToInt16(ShapeSetBuilder.SquareWidth);
                float oldGridYCoord = Convert.ToInt16(square.Origin.Y)/Convert.ToInt16(ShapeSetBuilder.SquareWidth);
                CGPoint oldGridOrigin = new CGPoint(
                    x: oldGridXCoord,
                    y: oldGridYCoord);


                bool oldXDivisibleBySquareWidth = Convert.ToInt16(square.Origin.X)%
                                                 Convert.ToInt16(ShapeSetBuilder.SquareWidth) == 0;
                bool oldYDivisibleBySquareWidth = Convert.ToInt16(square.Origin.Y)%
                                                 Convert.ToInt16(ShapeSetBuilder.SquareWidth) == 0;
            
                float newGridXCoord = Convert.ToInt16(newOrigin.X)/Convert.ToInt16(ShapeSetBuilder.SquareWidth);
                float newGridYCoord = Convert.ToInt16(newOrigin.Y)/Convert.ToInt16(ShapeSetBuilder.SquareWidth);

                var newGridOrigin = new CGPoint(
                    x: newGridXCoord,
                    y: newGridYCoord);

                if (newOrigin.X < 0)
                {
                    newGridOrigin.X = newGridOrigin.X - 1;
                }

                if (newOrigin.Y < 0)
                {
                    newGridOrigin.Y = newGridOrigin.Y - 1;
                }

                bool newXDivisibleBySquareWidth = Convert.ToInt16(newOrigin.X)%
                                                  Convert.ToInt16(ShapeSetBuilder.SquareWidth) == 0;
                bool newYDivisibleBySquareWidth = Convert.ToInt16(newOrigin.Y)%
                                                 Convert.ToInt16(ShapeSetBuilder.SquareWidth) == 0;
            
                if (oldXDivisibleBySquareWidth != newXDivisibleBySquareWidth
                    || oldGridOrigin.X != newGridOrigin.X)
                {
                    movementResult.ShapeHasCrossedAHorizontalGridBoundary = true;
                }
            
                if (oldYDivisibleBySquareWidth != newYDivisibleBySquareWidth
                    || oldGridOrigin.Y != newGridOrigin.Y)
                {
                    movementResult.ShapeHasCrossedAVerticalGridBoundary = true;
                }
            
                if (movementResult.ShapeHasCrossedAHorizontalGridBoundary
                    || movementResult.ShapeHasCrossedAVerticalGridBoundary)
                {
                    if (newXDivisibleBySquareWidth)
                    {
                        newGridXCoords.Add(Convert.ToInt16(newGridOrigin.X));
                    } else
                    {
                        newGridXCoords.Add(Convert.ToInt16(newGridOrigin.X));
                        newGridXCoords.Add(Convert.ToInt16(newGridOrigin.X) + 1);
                    }
                
                    if (newYDivisibleBySquareWidth)
                    {
                        newGridYCoords.Add(Convert.ToInt16(newGridOrigin.Y));
                    } else
                    {
                        newGridYCoords.Add(Convert.ToInt16(newGridOrigin.Y));
                        newGridYCoords.Add(Convert.ToInt16(newGridOrigin.Y) + 1);
                    }
                
                    // These nested for loops work because at the moment we are just considering one square, not the whole shape.
                    foreach (var xCoord in newGridXCoords) 
                    {
                        foreach (var yCoord in newGridYCoords) {
                            if (xCoord >= occupiedGridSquares.Count
                                || yCoord >= occupiedGridSquares[0].Count
                                || xCoord < 0
                                || yCoord < 0)
                            {
                                somethingIsintheWay = true;
                            } else
                            {
                                somethingIsintheWay = somethingIsintheWay
                                                      ||
                                                      occupiedGridSquares[Convert.ToInt16(xCoord)][
                                                          Convert.ToInt16(yCoord)].Occupied;
                            }
                        }
                    }
                }
            }
        
            if (!somethingIsintheWay) {
                foreach (var square in Squares)
                {
                    square.CalculateOrigin(parentShapeCentre: newShapeCentre);
                }
            }

             movementResult.NoShapesAreInTheWay = !somethingIsintheWay;

             return movementResult;
        }

        public void VacateGridSquares(List<List<GridSquare>>occupiedGridSquares) 
        {
            foreach (var square in Squares)
            {
                int occupiedXCoordinate = Convert.ToInt16(square.Origin.X)/Convert.ToInt16(ShapeSetBuilder.SquareWidth);
                int occupiedYCoordinate = Convert.ToInt16(square.Origin.Y)/Convert.ToInt16(ShapeSetBuilder.SquareWidth);
            
                occupiedGridSquares[occupiedXCoordinate][occupiedYCoordinate].Occupied = false;
                occupiedGridSquares[occupiedXCoordinate][occupiedYCoordinate].ShapeInSquare = null;
            }
        }

        public void OccupyGridSquares(List<List<GridSquare>> occupiedGridSquares)
        {
            foreach (var square in Squares)
            {
                int occupiedXCoordinate = Convert.ToInt16(square.Origin.X) / Convert.ToInt16(ShapeSetBuilder.SquareWidth);
                int occupiedYCoordinate = Convert.ToInt16(square.Origin.Y) / Convert.ToInt16(ShapeSetBuilder.SquareWidth);

                occupiedGridSquares[occupiedXCoordinate][occupiedYCoordinate].Occupied = true;
                occupiedGridSquares[occupiedXCoordinate][occupiedYCoordinate].ShapeInSquare = this;
            }
        }

        private void Initialise()
        {
            foreach(var square in Squares)
            {
            square.CalculateOrigin(parentShapeCentre: CentreOfShape);
            }
        
            CalculateNumSquaresAroundCentre();
            PutShapeInNewLocation(newCentreOfShape: CentreOfShape);
        }

        private void CalculateNumSquaresAroundCentre()
        {
            foreach (var square in Squares) {
                NumSquaresLeftOfShapeCentre =
                    (nfloat)Convert.ToDouble(Math.Min(NumSquaresLeftOfShapeCentre, square.PositionRelativeToParent.X));
            
                NumSquaresRightOfShapeCentre =
                     (nfloat)Convert.ToDouble(Math.Max(NumSquaresRightOfShapeCentre, square.PositionRelativeToParent.X));
            
                NumSquaresAboveShapeCentre =
                    (nfloat)Convert.ToDouble(Math.Min(NumSquaresAboveShapeCentre, square.PositionRelativeToParent.Y));
            
                NumSquaresBelowShapeCentre =
                    (nfloat)Convert.ToDouble(Math.Max(NumSquaresBelowShapeCentre, square.PositionRelativeToParent.Y));
            }
        
            DealWithNegativeNumbersOfSquares();
        }

        private void DealWithNegativeNumbersOfSquares()
        {
           NumSquaresLeftOfShapeCentre = (nfloat) Convert.ToDouble((Math.Abs(NumSquaresLeftOfShapeCentre)));
           NumSquaresAboveShapeCentre = (nfloat) Convert.ToDouble(Math.Abs(NumSquaresAboveShapeCentre));
        }
	}
}