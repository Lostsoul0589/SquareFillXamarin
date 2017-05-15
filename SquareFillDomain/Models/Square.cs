using SquareFillDomain.Builders;

namespace SquareFillDomain.Models
{
    public class Square
    {
        public CGPoint PositionRelativeToParent { get; private set; }
        public UIImageView Sprite { get; private set; }
        public CGPoint Origin { get; private set; }

        public Square()
        {
            Origin = new CGPoint(x: 0, y: 0);
        }

        public Square(CGPoint positionRelativeToParent, UIImageView sprite)
        {
            PositionRelativeToParent = positionRelativeToParent;
            Sprite = sprite;
            Origin = new CGPoint(x: 0, y: 0);
        }

        public void CalculateOrigin(CGPoint parentShapeCentre)
        {
            Origin = CalculatePotentialOrigin(parentShapeCentre: parentShapeCentre);
        }

        public CGPoint CalculatePotentialOrigin(CGPoint parentShapeCentre)
        {
            return new CGPoint(
           x: parentShapeCentre.X + (PositionRelativeToParent.X * ShapeSetBuilder.SquareWidth)
               - ShapeSetBuilder.SquareWidth / 2,
           y: parentShapeCentre.Y + (PositionRelativeToParent.Y * ShapeSetBuilder.SquareWidth)
               - ShapeSetBuilder.SquareWidth / 2);
        }

        public bool IsInSquare(CGPoint point)
        {
            return Origin.X <= point.X
                   && point.X <= (Origin.X + ShapeSetBuilder.SquareWidth)
                   && Origin.Y <= point.Y
                   && point.Y <= (Origin.Y + ShapeSetBuilder.SquareWidth);
        }
    }
}