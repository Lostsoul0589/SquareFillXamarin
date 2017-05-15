using CoreGraphics;
using SquareFillDomain.Interfaces;
using SquareFillDomain.Models;
using SquareFillDomain.Utils;
using SquareFillXamarin.Builders;
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
    
        public SquareFillPoint Centre()
        {
            int x = 1;
            int y = 2;
            int width = 4;
            int height = 7;
            var thing = new SquareFillRect(x: x, y: y, width: width, height: height);
            return new SquareFillPoint(
                x: (int)_imageView.Center.X,
                y: (int)_imageView.Center.Y);
        }

        private UIColor ConvertColour(SquareFillColour colour)
        {
            switch (colour)
            {
                case SquareFillColour.Red:
                    return UIColor.Red;
                    break;
                case SquareFillColour.Blue:
                    return UIColor.Blue;
                    break;
                case SquareFillColour.Black:
                    return UIColor.Black;
                    break;
                case SquareFillColour.Orange:
                    return UIColor.Orange;
                    break;
                case SquareFillColour.Green:
                    return UIColor.Green;
                    break;
                case SquareFillColour.Yellow:
                    return UIColor.Yellow;
                    break;
                case SquareFillColour.Purple:
                    return UIColor.Purple;
                    break;
                case SquareFillColour.Magenta:
                    return UIColor.Magenta;
                    break;
                case SquareFillColour.Brown:
                    return UIColor.Brown;
                    break;
                case SquareFillColour.Cyan:
                    return UIColor.Cyan;
                    break;
                case SquareFillColour.DarkGrey:
                    return UIColor.DarkGray;
                    break;
                case SquareFillColour.Grey:
                    return UIColor.Gray;
                    break;
                case SquareFillColour.White:
                    return UIColor.White;
                    break;
                case SquareFillColour.LightGrey:
                    return UIColor.LightGray;
                    break;
                default: return UIColor.Black;
                    break;
            }
        }
    }
}