using CoreGraphics;
using SquareFillDomain.Builders;
using SquareFillDomain.Interfaces;
using SquareFillDomain.Models;
using SquareFillDomain.Utils;
using UIKit;

namespace SquareFillXamarin.UIComponents
{
    public class SquareView : ISquareView
    {
        private readonly UIImageView _imageView;

        public SquareView(UIKit.UIView view, SquareFillColour colour)
        {
            _imageView = new UIImageView();
            _imageView.Frame = new CGRect(
                x: 0,
                y: 0,
                width: ShapeSetBuilder.SquareWidth,
                height: ShapeSetBuilder.SquareWidth);
            _imageView.BackgroundColor = ConvertColour(colour: colour);
            view.AddSubview(_imageView);
            _imageView.Layer.BorderColor = UIColor.Black.CGColor;
            _imageView.Layer.BorderWidth = 1;
        }
    
        public void MoveSquare(int newX, int newY)
        {
            _imageView.Center = new CGPoint(x: newX, y: newY);
        }

        public void MoveTopLeftCorner(int newX, int newY)
        {
            _imageView.Center = new CGPoint(
                x: newX + _imageView.Frame.Width/2,
                y: newY + _imageView.Frame.Height/2);
        }
    
        public SquareFillPoint Centre()
        {
            return new SquareFillPoint(
                x: (int)_imageView.Center.X,
                y: (int)_imageView.Center.Y);
        }

        public SquareFillPoint TopLeftCorner()
        {
            return new SquareFillPoint(
                x: (int)(_imageView.Center.X - _imageView.Frame.Width/2),
                y: (int)(_imageView.Center.Y - _imageView.Frame.Height/2));
        }

        private UIColor ConvertColour(SquareFillColour colour)
        {
            switch (colour)
            {
                case SquareFillColour.Red:
                    return UIColor.Red;
                case SquareFillColour.Blue:
                    return UIColor.Blue;
                case SquareFillColour.Black:
                    return UIColor.Black;
                case SquareFillColour.Orange:
                    return UIColor.Orange;
                case SquareFillColour.Green:
                    return UIColor.Green;
                case SquareFillColour.Yellow:
                    return UIColor.Yellow;
                case SquareFillColour.Purple:
                    return UIColor.Purple;
                case SquareFillColour.Magenta:
                    return UIColor.Magenta;
                case SquareFillColour.Brown:
                    return UIColor.Brown;
                case SquareFillColour.Cyan:
                    return UIColor.Cyan;
                case SquareFillColour.DarkGrey:
                    return UIColor.DarkGray;
                case SquareFillColour.Grey:
                    return UIColor.Gray;
                case SquareFillColour.White:
                    return UIColor.White;
                case SquareFillColour.LightGrey:
                    return UIColor.LightGray;
                default: 
                    return UIColor.Black;
            }
        }
    }
}