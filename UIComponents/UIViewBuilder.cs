using CoreGraphics;
using SquareFillDomain.Builders;
using SquareFillDomain.Interfaces;
using SquareFillDomain.Utils;
using UIKit;

namespace SquareFillXamarin.UIComponents
{
    public class UIViewBuilder
	{
        private static CGRect _containingRectangle = new CGRect(
        x: ShapeConstants.ContainingRectangle.X,
        y: ShapeConstants.ContainingRectangle.Y,
        width: ShapeConstants.ContainingRectangle.Width,
        height: ShapeConstants.ContainingRectangle.Height);

        private static readonly CGRect TopGridBorder = new CGRect(
			x: 3 * ShapeConstants.SquareWidth,
			y: 5 * ShapeConstants.SquareWidth,
			width: 9 * ShapeConstants.SquareWidth,
			height: ShapeConstants.SquareWidth);

        private static readonly CGRect LeftGridBorder = new CGRect(
			x: 3 * ShapeConstants.SquareWidth,
			y: 6 * ShapeConstants.SquareWidth,
			width: ShapeConstants.SquareWidth,
			height: 7 * ShapeConstants.SquareWidth);

        private static readonly CGRect RightGridBorder = new CGRect(
			x: 11 * ShapeConstants.SquareWidth,
			y: 6 * ShapeConstants.SquareWidth,
			width: ShapeConstants.SquareWidth,
			height: 7 * ShapeConstants.SquareWidth);

        private static readonly CGRect BottomLeftGridBorder = new CGRect(
			x: 3 * ShapeConstants.SquareWidth,
			y: 13 * ShapeConstants.SquareWidth,
			width: 2 * ShapeConstants.SquareWidth,
			height: ShapeConstants.SquareWidth);

        private static readonly CGRect BottomRightGridBorder = new CGRect(
			x: 10 * ShapeConstants.SquareWidth,
			y: 13 * ShapeConstants.SquareWidth,
			width: 2 * ShapeConstants.SquareWidth,
			height: ShapeConstants.SquareWidth);
    
        public static ISquareViewFactory InitialiseUIComponents(UIKit.UIView view)
        {
            MakeGameGrid(view: view);
            return new SquareViewFactory(view: view);
        }

        public static void MakeGameGrid(UIKit.UIView view)
        {
            MakeContainingRectangle(view: view);

            MakeBorderPiece(view: view, rect: TopGridBorder);
            MakeBorderPiece(view: view, rect: LeftGridBorder);
            MakeBorderPiece(view: view, rect: RightGridBorder);
            MakeBorderPiece(view: view, rect: BottomLeftGridBorder);
            MakeBorderPiece(view: view, rect: BottomRightGridBorder);
        }

        private static void MakeContainingRectangle(UIKit.UIView view)
        {
            var containingRectangle = new UIImageView();

            containingRectangle.Frame = _containingRectangle;

            containingRectangle.BackgroundColor = UIColor.Yellow;

            view.AddSubview(containingRectangle);
        }

        private static void MakeBorderPiece(UIKit.UIView view, CGRect rect)
        {
            var borderPiece = new UIImageView();

            borderPiece.Frame = rect;

            borderPiece.BackgroundColor = UIColor.Black;

            view.AddSubview(borderPiece);
        }
	}
}
