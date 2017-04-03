using System;
using CoreGraphics;
using System.Collections.Generic;
using UIKit;
using Foundation;

namespace SquareFillXamarin
{
	public class ShapeSetBuilder
	{
		static Double SquareWidth = 32;
		static int GridWidth = 13;
		static int GridHeight = 20;
		static Double ScreenWidth = GridWidth * SquareWidth;
		static Double ScreenHeight = GridHeight * SquareWidth;

		static CGRect ContainingSquare = new CGRect(
			x: 4 * SquareWidth,
			y: 6 * SquareWidth,
			width: 7 * SquareWidth,
			height: 7 * SquareWidth);

		static CGRect TopGridBorder = new CGRect(
			x: 3 * SquareWidth,
			y: 5 * SquareWidth,
			width: 9 * SquareWidth,
			height: SquareWidth);

		static CGRect LeftGridBorder = new CGRect(
			x: 3 * SquareWidth,
			y: 6 * SquareWidth,
			width: SquareWidth,
			height: 7 * SquareWidth);

		static CGRect RightGridBorder = new CGRect(
			x: 11 * SquareWidth,
			y: 6 * SquareWidth,
			width: SquareWidth,
			height: 7 * SquareWidth);

		static List<CGPoint> borderSquares = new List<CGPoint>();

		static CGPoint CentreOfTopLeftGridSquare = new CGPoint(
			x: ContainingSquare.X + SquareWidth / 2,
			y: ContainingSquare.Y + SquareWidth / 2);

		static List<CGPoint> rightHydrantPoints = new List<CGPoint> { new CGPoint(x: 0, y: 0), new CGPoint(x: 1, y: 0), new CGPoint(x: 0, y: 1), new CGPoint(x: 0, y: -1) };
		static List<CGPoint> fourBarPoints = new List<CGPoint> { new CGPoint(x: 0, y: 0), new CGPoint(x: -1, y: 0), new CGPoint(x: 1, y: 0), new CGPoint(x: 2, y: 0) };
		static List<CGPoint> sevenPoints = new List<CGPoint> { new CGPoint(x: 0, y: 0), new CGPoint(x: -1, y: 0), new CGPoint(x: 0, y: 1), new CGPoint(x: 0, y: 2) };
		static List<CGPoint> fourSquarePoints = new List<CGPoint> { new CGPoint(x: 0, y: 0), new CGPoint(x: -1, y: 0), new CGPoint(x: 0, y: 1), new CGPoint(x: -1, y: 1) };
		static List<CGPoint> leftCornerPoints = new List<CGPoint> { new CGPoint(x: 0, y: 0), new CGPoint(x: -1, y: 0), new CGPoint(x: 0, y: 1) };
		static List<CGPoint> upsideDownTPoints = new List<CGPoint> { new CGPoint(x: 0, y: 0), new CGPoint(x: -1, y: 0), new CGPoint(x: 0, y: -1), new CGPoint(x: 1, y: 0) };
		static List<CGPoint> threePolePoints = new List<CGPoint> { new CGPoint(x: 0, y: 0), new CGPoint(x: 0, y: -1), new CGPoint(x: 0, y: 1) };
		static List<CGPoint> twoPolePoints = new List<CGPoint> { new CGPoint(x: 0, y: 0), new CGPoint(x: 0, y: -1) };
		static List<CGPoint> backwardsLPoints = new List<CGPoint> { new CGPoint(x: 0, y: 0), new CGPoint(x: -1, y: 0), new CGPoint(x: 0, y: -1), new CGPoint(x: 0, y: -2) };
		static List<CGPoint> singleSquarePoints = new List<CGPoint> { new CGPoint(x: 0, y: 0) };

		static List<CGPoint> leftHydrantPoints = new List<CGPoint> { new CGPoint(x: 0, y: 0), new CGPoint(x: -1, y: 0), new CGPoint(x: 0, y: 1), new CGPoint(x: 0, y: -1) };
		static List<CGPoint> rightWayUpTPoints = new List<CGPoint> { new CGPoint(x: 0, y: 0), new CGPoint(x: -1, y: 0), new CGPoint(x: 0, y: 1), new CGPoint(x: 1, y: 0) };

		public static List<List<GridSquare>> MakeGridSquares()
		{
			var occupiedGridSquares = new List<List<GridSquare>>();

			for (int xCoord = 0; xCoord < GridWidth; xCoord++)
			{
				occupiedGridSquares.Add(new List<GridSquare>());

				for (int yCoord = 0; yCoord < GridHeight; yCoord++)
				{
					occupiedGridSquares[xCoord].Add(new GridSquare());
				}
			}

			return occupiedGridSquares;
		}

		public static void OccupyBorderSquares(List<List<GridSquare>> occupiedGridSquares)
		{
			foreach (var borderSquare in borderSquares)
			{
				occupiedGridSquares[Convert.ToInt32(borderSquare.X)][Convert.ToInt32(borderSquare.Y)].Occupied = true;
			}
		}

		public static void BuildBorderSquares()
		{
			borderSquares.Add(new CGPoint(x: 3, y: 5));
			borderSquares.Add(new CGPoint(x: 4, y: 5));
			borderSquares.Add(new CGPoint(x: 5, y: 5));
			borderSquares.Add(new CGPoint(x: 6, y: 5));
			borderSquares.Add(new CGPoint(x: 7, y: 5));
			borderSquares.Add(new CGPoint(x: 8, y: 5));
			borderSquares.Add(new CGPoint(x: 9, y: 5));
			borderSquares.Add(new CGPoint(x: 10, y: 5));
			borderSquares.Add(new CGPoint(x: 11, y: 5));
			borderSquares.Add(new CGPoint(x: 11, y: 6));
			borderSquares.Add(new CGPoint(x: 11, y: 7));
			borderSquares.Add(new CGPoint(x: 11, y: 8));
			borderSquares.Add(new CGPoint(x: 11, y: 9));
			borderSquares.Add(new CGPoint(x: 11, y: 10));
			borderSquares.Add(new CGPoint(x: 11, y: 11));
			borderSquares.Add(new CGPoint(x: 11, y: 12));
			borderSquares.Add(new CGPoint(x: 11, y: 13));
			borderSquares.Add(new CGPoint(x: 10, y: 13));
			borderSquares.Add(new CGPoint(x: 4, y: 13));
			borderSquares.Add(new CGPoint(x: 3, y: 13));
			borderSquares.Add(new CGPoint(x: 3, y: 12));
			borderSquares.Add(new CGPoint(x: 3, y: 11));
			borderSquares.Add(new CGPoint(x: 3, y: 10));
			borderSquares.Add(new CGPoint(x: 3, y: 9));
			borderSquares.Add(new CGPoint(x: 3, y: 8));
			borderSquares.Add(new CGPoint(x: 3, y: 7));
			borderSquares.Add(new CGPoint(x: 3, y: 6));
		}

		public static void MakeContainingRectangle(UIView view)
		{
			var containingRectangle = new UIImageView();

			containingRectangle.Frame = ContainingSquare;

			containingRectangle.BackgroundColor = UIColor.Yellow;

			view.AddSubview(containingRectangle);
		}

		public static void MakeBorderPiece(UIView view, CGRect rect)
		{
			var borderPiece = new UIImageView();

			borderPiece.Frame = rect;

			borderPiece.BackgroundColor = UIColor.Black;

			view.AddSubview(borderPiece);
		}

		public ShapeSetBuilder()
		{
		}
	}
}
