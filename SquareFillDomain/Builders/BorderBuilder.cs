using System.Collections.Generic;
using SquareFillDomain.Models;

namespace SquareFillDomain.Builders
{
    public class BorderBuilder
    {
        public readonly List<SquareFillPoint> TopRowBorderSquares = new List<SquareFillPoint>();
        public readonly List<SquareFillPoint> LeftWallBorderSquares = new List<SquareFillPoint>();
        public readonly List<SquareFillPoint> RightWallBorderSquares = new List<SquareFillPoint>();
        public readonly List<SquareFillPoint> BottomLeftBorderSquares = new List<SquareFillPoint>();
        public readonly List<SquareFillPoint> BottomRightBorderSquares = new List<SquareFillPoint>();

        private readonly List<SquareFillPoint> _borderSquares = new List<SquareFillPoint>();

        public void OccupyBorderSquares(Grid occupiedGridSquares)
        {
            foreach (var element in _borderSquares) {
                occupiedGridSquares.OccupyGridSquare(x: element.X, y:element.Y);
            }
        }

        public void BuildBorderSquares(int squareWidth, SquareFillRect containingRectangle)
        {
            BuildTopRowBorderSquares(squareWidth: squareWidth, containingRectangle: containingRectangle);
            BuildRightWallBorderSquares(squareWidth: squareWidth, containingRectangle: containingRectangle);
            BuildBottomRightBorderSquares(squareWidth: squareWidth, containingRectangle: containingRectangle);
            BuildBottomLeftBorderSquares(squareWidth: squareWidth, containingRectangle: containingRectangle);
            BuildLeftWallBorderSquares(squareWidth: squareWidth, containingRectangle: containingRectangle);
        }

        private void BuildTopRowBorderSquares(int squareWidth, SquareFillRect containingRectangle)
        {
            var topRowY = (containingRectangle.Y / squareWidth) - 1;
            var leftWallX = (containingRectangle.X / squareWidth) - 1;

            for (int squareCount = 0; squareCount <= (containingRectangle.Width/squareWidth) + 1; squareCount++)
            {
                TopRowBorderSquares.Add(new SquareFillPoint(x: leftWallX + squareCount, y: topRowY));
            }

            foreach (var element in TopRowBorderSquares) {
                _borderSquares.Add(element);
            }
        }

        private void BuildRightWallBorderSquares(int squareWidth, SquareFillRect containingRectangle)
        {
            var rightWallX = (containingRectangle.X + containingRectangle.Width) / squareWidth;
            var topY = (containingRectangle.Y / squareWidth) - 1;

            for (int squareCount = 1; squareCount <= containingRectangle.Height/squareWidth; squareCount++)
            {
                RightWallBorderSquares.Add(new SquareFillPoint(x: rightWallX, y: topY + squareCount));
            }

            foreach (var element in RightWallBorderSquares) {
                _borderSquares.Add(element);
            }
        }

        private void BuildBottomRightBorderSquares(int squareWidth, SquareFillRect containingRectangle)
        {
            var bottomY = (containingRectangle.Y + containingRectangle.Height) / squareWidth;
            var rightWallX = (containingRectangle.X + containingRectangle.Width) / squareWidth;

            BottomRightBorderSquares.Add(new SquareFillPoint(x: rightWallX - 1, y: bottomY));
            BottomRightBorderSquares.Add(new SquareFillPoint(x: rightWallX, y: bottomY));

            foreach (var element in BottomRightBorderSquares) {
                _borderSquares.Add(element);
            }
        }

        private void BuildBottomLeftBorderSquares(int squareWidth, SquareFillRect containingRectangle)
        {
            var bottomY = (containingRectangle.Y + containingRectangle.Height) / squareWidth;
            var leftWallX = (containingRectangle.X / squareWidth) - 1;

            BottomLeftBorderSquares.Add(new SquareFillPoint(x: leftWallX, y: bottomY));
            BottomLeftBorderSquares.Add(new SquareFillPoint(x: leftWallX + 1, y: bottomY));

            foreach (var element in BottomLeftBorderSquares) {
                _borderSquares.Add(element);
            }
        }

        private void BuildLeftWallBorderSquares(int squareWidth, SquareFillRect containingRectangle)
        {
            var leftWallX = (containingRectangle.X / squareWidth) - 1;
            var topY = (containingRectangle.Y / squareWidth) - 1;

            for (int squareCount = 1; squareCount <= containingRectangle.Height/squareWidth; squareCount++)
            {
                LeftWallBorderSquares.Add(new SquareFillPoint(x: leftWallX, y: topY + squareCount));
            }

            foreach (var element in LeftWallBorderSquares) {
                _borderSquares.Add(element);
            }
        }
    }
}