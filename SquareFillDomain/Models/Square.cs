using SquareFillDomain.Builders;
using SquareFillDomain.Interfaces;

namespace SquareFillDomain.Models
{
    public class Square
    {
        public SquareFillPoint PositionRelativeToParent { get; private set; }
        public ISquareView Sprite { get; private set; }
        public SquareFillPoint Origin { get; private set; }

        public Square()
        {
            Origin = new SquareFillPoint(x: 0, y: 0);
        }

        public Square(SquareFillPoint positionRelativeToParent, ISquareView sprite)
        {
            PositionRelativeToParent = positionRelativeToParent;
            Sprite = sprite;
            Origin = new SquareFillPoint(x: 0, y: 0);
        }

        public void CalculateOrigin(SquareFillPoint parentShapeCentre)
        {
            Origin = CalculatePotentialOrigin(parentShapeCentre: parentShapeCentre);
        }

        public SquareFillPoint CalculatePotentialOrigin(SquareFillPoint parentShapeCentre)
        {
            return new SquareFillPoint(
                x: parentShapeCentre.X + (PositionRelativeToParent.X * ShapeSetBuilder.SquareWidth)
                    - ShapeSetBuilder.SquareWidth / 2,
                y: parentShapeCentre.Y + (PositionRelativeToParent.Y * ShapeSetBuilder.SquareWidth)
                    - ShapeSetBuilder.SquareWidth / 2);
        }

        public bool IsInSquare(SquareFillPoint point)
        {
            return Origin.X <= point.X
                   && point.X <= (Origin.X + ShapeSetBuilder.SquareWidth)
                   && Origin.Y <= point.Y
                   && point.Y <= (Origin.Y + ShapeSetBuilder.SquareWidth);
        }
    }
}