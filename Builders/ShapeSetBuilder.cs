using System;
using CoreGraphics;
using System.Collections.Generic;

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

		public ShapeSetBuilder()
		{
		}
	}
}
