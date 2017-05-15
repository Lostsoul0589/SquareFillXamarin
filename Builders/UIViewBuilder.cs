using System;
using System.Collections.Generic;
using CoreGraphics;
using SquareFillXamarin.Models;
using UIKit;

namespace SquareFillXamarin.Builders
{
	public class ShapeSetBuilder
	{
        public static Double SquareWidth = 32;

        public static CGRect ContainingSquare = new CGRect(
            x: 4 * SquareWidth,
            y: 6 * SquareWidth,
            width: 7 * SquareWidth,
            height: 7 * SquareWidth);

        public static CGPoint CentreOfTopLeftGridSquare = new CGPoint(
            x: ContainingSquare.X + SquareWidth / 2,
            y: ContainingSquare.Y + SquareWidth / 2);

        public static int GridWidth = 13;
        public static int GridHeight = 20;
        public static Double ScreenWidth = GridWidth * SquareWidth;
        public static Double ScreenHeight = GridHeight * SquareWidth;

        private static readonly CGRect TopGridBorder = new CGRect(
			x: 3 * SquareWidth,
			y: 5 * SquareWidth,
			width: 9 * SquareWidth,
			height: SquareWidth);

        private static readonly CGRect LeftGridBorder = new CGRect(
			x: 3 * SquareWidth,
			y: 6 * SquareWidth,
			width: SquareWidth,
			height: 7 * SquareWidth);

        private static readonly CGRect RightGridBorder = new CGRect(
			x: 11 * SquareWidth,
			y: 6 * SquareWidth,
			width: SquareWidth,
			height: 7 * SquareWidth);

        private static readonly CGRect BottomLeftGridBorder = new CGRect(
			x: 3 * SquareWidth,
			y: 13 * SquareWidth,
			width: 2 * SquareWidth,
			height: SquareWidth);

        private static readonly CGRect BottomRightGridBorder = new CGRect(
			x: 10 * SquareWidth,
			y: 13 * SquareWidth,
			width: 2 * SquareWidth,
			height: SquareWidth);

        private static readonly List<CGPoint> BorderSquares = new List<CGPoint>();

        public static List<CGPoint> RightHydrantPoints = new List<CGPoint> { new CGPoint(x: 0, y: 0), new CGPoint(x: 1, y: 0), new CGPoint(x: 0, y: 1), new CGPoint(x: 0, y: -1) };
        public static List<CGPoint> FourBarPoints = new List<CGPoint> { new CGPoint(x: 0, y: 0), new CGPoint(x: -1, y: 0), new CGPoint(x: 1, y: 0), new CGPoint(x: 2, y: 0) };
        public static List<CGPoint> SevenPoints = new List<CGPoint> { new CGPoint(x: 0, y: 0), new CGPoint(x: -1, y: 0), new CGPoint(x: 0, y: 1), new CGPoint(x: 0, y: 2) };
        public static List<CGPoint> FourSquarePoints = new List<CGPoint> { new CGPoint(x: 0, y: 0), new CGPoint(x: -1, y: 0), new CGPoint(x: 0, y: 1), new CGPoint(x: -1, y: 1) };
        public static List<CGPoint> LeftCornerPoints = new List<CGPoint> { new CGPoint(x: 0, y: 0), new CGPoint(x: -1, y: 0), new CGPoint(x: 0, y: 1) };
        public static List<CGPoint> UpsideDownTPoints = new List<CGPoint> { new CGPoint(x: 0, y: 0), new CGPoint(x: -1, y: 0), new CGPoint(x: 0, y: -1), new CGPoint(x: 1, y: 0) };
        public static List<CGPoint> ThreePolePoints = new List<CGPoint> { new CGPoint(x: 0, y: 0), new CGPoint(x: 0, y: -1), new CGPoint(x: 0, y: 1) };
        public static List<CGPoint> TwoPolePoints = new List<CGPoint> { new CGPoint(x: 0, y: 0), new CGPoint(x: 0, y: -1) };
        public static List<CGPoint> BackwardsLPoints = new List<CGPoint> { new CGPoint(x: 0, y: 0), new CGPoint(x: -1, y: 0), new CGPoint(x: 0, y: -1), new CGPoint(x: 0, y: -2) };
        public static List<CGPoint> SingleSquarePoints = new List<CGPoint> { new CGPoint(x: 0, y: 0) };

        public static List<CGPoint> LeftHydrantPoints = new List<CGPoint> { new CGPoint(x: 0, y: 0), new CGPoint(x: -1, y: 0), new CGPoint(x: 0, y: 1), new CGPoint(x: 0, y: -1) };
        public static List<CGPoint> RightWayUpTPoints = new List<CGPoint> { new CGPoint(x: 0, y: 0), new CGPoint(x: -1, y: 0), new CGPoint(x: 0, y: 1), new CGPoint(x: 1, y: 0) };

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

		public static void MakeGameGrid(UIView view, List<List<GridSquare>> occupiedGridSquares)
		{
			MakeContainingRectangle(view: view);

			MakeBorderPiece(view: view, rect: TopGridBorder);

			MakeBorderPiece(view: view, rect: LeftGridBorder);

			MakeBorderPiece(view: view, rect: RightGridBorder);

			MakeBorderPiece(view: view, rect: BottomLeftGridBorder);

			MakeBorderPiece(view: view, rect: BottomRightGridBorder);

			BuildBorderSquares();

			OccupyBorderSquares(occupiedGridSquares: occupiedGridSquares);
		}

		public static ShapeSet MakeHardCodedShapeSet(UIView view)
		{
			var originX = SquareWidth / 2;

			var originY = SquareWidth / 2;

			return new ShapeSet(shapes: new List<Shape> {
            // 1:
            new Shape(colour: UIColor.Red,
					  centreOfShape: new CGPoint(x:originX + 3*SquareWidth, y:originY + 2*SquareWidth),
					  relativePoints: RightHydrantPoints,
					  view: view),
			// 2:
            new Shape(colour: UIColor.Blue,
						centreOfShape: new CGPoint(x:originX + 3*SquareWidth, y:originY + 15*SquareWidth),
					  relativePoints: FourBarPoints,
					  view: view),
            // 3:
            new Shape(colour: UIColor.Black,
						centreOfShape: new CGPoint(x:originX + 10*SquareWidth, y:originY + SquareWidth),
					  relativePoints: SevenPoints,
					  view: view),
            // 4:
            new Shape(colour: UIColor.Orange,
						centreOfShape: new CGPoint(x:originX + 7*SquareWidth, y:originY + 2*SquareWidth),
					  relativePoints: FourSquarePoints,
					  view: view),
            // 5:
            new Shape(colour: UIColor.Green,
					  centreOfShape: new CGPoint(x:originX + 8*SquareWidth, y:originY + 15*SquareWidth),
					  relativePoints: LeftCornerPoints,
					  view: view),
            // 6:
            new Shape(colour: UIColor.Yellow,
					  centreOfShape: new CGPoint(x:originX, y:originY + 2*SquareWidth),
					  relativePoints: RightHydrantPoints,
					  view: view),
			// 7:
            new Shape(colour: UIColor.Purple,
					  centreOfShape: new CGPoint(x:originX + 3*SquareWidth, y:originY + 18*SquareWidth),
					  relativePoints: UpsideDownTPoints,
					  view: view),
            // 8:
            new Shape(colour: UIColor.Magenta,
						centreOfShape: new CGPoint(x:originX, y:originY + 17*SquareWidth),
					  relativePoints: ThreePolePoints,
					  view: view),
            // 9:
            new Shape(colour: UIColor.Brown,
						centreOfShape: new CGPoint(x:originX + 6*SquareWidth, y:originY + 18*SquareWidth),
					  relativePoints: TwoPolePoints,
					  view: view),
            // 10:
            new Shape(colour: UIColor.Cyan,
						centreOfShape: new CGPoint(x:originX + SquareWidth, y:originY + 9*SquareWidth),
					  relativePoints: FourSquarePoints,
					  view: view),
            // 11:
            new Shape(colour: UIColor.DarkGray,
					  centreOfShape: new CGPoint(x:originX + SquareWidth, y:originY + 7*SquareWidth),
					  relativePoints: BackwardsLPoints,
					  view: view),
            // 12:
            new Shape(colour: UIColor.Gray,
					  centreOfShape: new CGPoint(x:originX, y:originY + 13*SquareWidth),
					  relativePoints: RightHydrantPoints,
					  view: view),
            // 13:
            new Shape(colour: UIColor.White,
						centreOfShape: new CGPoint(x:originX + 11*SquareWidth, y:originY + 16*SquareWidth),
					  relativePoints: UpsideDownTPoints,
					  view: view),
            // 14:
            new Shape(colour: UIColor.LightGray,
					centreOfShape: new CGPoint(x:originX + 12*SquareWidth, y:originY + 2*SquareWidth),
					relativePoints: SingleSquarePoints,
					view: view)

			});
		}

        private static void OccupyBorderSquares(List<List<GridSquare>> occupiedGridSquares)
        {
            foreach (var borderSquare in BorderSquares)
            {
                occupiedGridSquares[Convert.ToInt32(borderSquare.X)][Convert.ToInt32(borderSquare.Y)].Occupied = true;
            }
        }

        private static void BuildBorderSquares()
        {
            BorderSquares.Add(new CGPoint(x: 3, y: 5));
            BorderSquares.Add(new CGPoint(x: 4, y: 5));
            BorderSquares.Add(new CGPoint(x: 5, y: 5));
            BorderSquares.Add(new CGPoint(x: 6, y: 5));
            BorderSquares.Add(new CGPoint(x: 7, y: 5));
            BorderSquares.Add(new CGPoint(x: 8, y: 5));
            BorderSquares.Add(new CGPoint(x: 9, y: 5));
            BorderSquares.Add(new CGPoint(x: 10, y: 5));
            BorderSquares.Add(new CGPoint(x: 11, y: 5));
            BorderSquares.Add(new CGPoint(x: 11, y: 6));
            BorderSquares.Add(new CGPoint(x: 11, y: 7));
            BorderSquares.Add(new CGPoint(x: 11, y: 8));
            BorderSquares.Add(new CGPoint(x: 11, y: 9));
            BorderSquares.Add(new CGPoint(x: 11, y: 10));
            BorderSquares.Add(new CGPoint(x: 11, y: 11));
            BorderSquares.Add(new CGPoint(x: 11, y: 12));
            BorderSquares.Add(new CGPoint(x: 11, y: 13));
            BorderSquares.Add(new CGPoint(x: 10, y: 13));
            BorderSquares.Add(new CGPoint(x: 4, y: 13));
            BorderSquares.Add(new CGPoint(x: 3, y: 13));
            BorderSquares.Add(new CGPoint(x: 3, y: 12));
            BorderSquares.Add(new CGPoint(x: 3, y: 11));
            BorderSquares.Add(new CGPoint(x: 3, y: 10));
            BorderSquares.Add(new CGPoint(x: 3, y: 9));
            BorderSquares.Add(new CGPoint(x: 3, y: 8));
            BorderSquares.Add(new CGPoint(x: 3, y: 7));
            BorderSquares.Add(new CGPoint(x: 3, y: 6));
        }

        private static void MakeContainingRectangle(UIView view)
        {
            var containingRectangle = new UIImageView();

            containingRectangle.Frame = ContainingSquare;

            containingRectangle.BackgroundColor = UIColor.Yellow;

            view.AddSubview(containingRectangle);
        }

        private static void MakeBorderPiece(UIView view, CGRect rect)
        {
            var borderPiece = new UIImageView();

            borderPiece.Frame = rect;

            borderPiece.BackgroundColor = UIColor.Black;

            view.AddSubview(borderPiece);
        }
	}
}
