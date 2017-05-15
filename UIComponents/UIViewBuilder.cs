using CoreGraphics;
using SquareFillDomain.Interfaces;
using SquareFillXamarin.Builders;
using UIKit;

namespace SquareFillXamarin.UIComponents
{
    public class UIViewBuilder
	{
        private static CGRect _containingRectangle = new CGRect(
        x: ShapeSetBuilder.ContainingSquare.X,
        y: ShapeSetBuilder.ContainingSquare.Y,
        width: ShapeSetBuilder.ContainingSquare.Width,
        height: ShapeSetBuilder.ContainingSquare.Height);

        private static readonly CGRect TopGridBorder = new CGRect(
			x: 3 * ShapeSetBuilder.SquareWidth,
			y: 5 * ShapeSetBuilder.SquareWidth,
			width: 9 * ShapeSetBuilder.SquareWidth,
			height: ShapeSetBuilder.SquareWidth);

        private static readonly CGRect LeftGridBorder = new CGRect(
			x: 3 * ShapeSetBuilder.SquareWidth,
			y: 6 * ShapeSetBuilder.SquareWidth,
			width: ShapeSetBuilder.SquareWidth,
			height: 7 * ShapeSetBuilder.SquareWidth);

        private static readonly CGRect RightGridBorder = new CGRect(
			x: 11 * ShapeSetBuilder.SquareWidth,
			y: 6 * ShapeSetBuilder.SquareWidth,
			width: ShapeSetBuilder.SquareWidth,
			height: 7 * ShapeSetBuilder.SquareWidth);

        private static readonly CGRect BottomLeftGridBorder = new CGRect(
			x: 3 * ShapeSetBuilder.SquareWidth,
			y: 13 * ShapeSetBuilder.SquareWidth,
			width: 2 * ShapeSetBuilder.SquareWidth,
			height: ShapeSetBuilder.SquareWidth);

        private static readonly CGRect BottomRightGridBorder = new CGRect(
			x: 10 * ShapeSetBuilder.SquareWidth,
			y: 13 * ShapeSetBuilder.SquareWidth,
			width: 2 * ShapeSetBuilder.SquareWidth,
			height: ShapeSetBuilder.SquareWidth);
    
        public static ISquareViewMaker InitialiseUIComponents(UIKit.UIView view)
        {
            MakeGameGrid(view: view);
            return new SquareViewMaker(view: view);
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
