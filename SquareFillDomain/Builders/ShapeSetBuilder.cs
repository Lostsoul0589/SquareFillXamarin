using System.Collections.Generic;
using SquareFillDomain.Interfaces;
using SquareFillDomain.Models;
using SquareFillDomain.Utils;

namespace SquareFillDomain.Builders
{
	public class ShapeSetBuilder
	{
        public static int SquareWidth = 32;

        public static SquareFillRect ContainingRectangle = new SquareFillRect(
            x: 4 * SquareWidth,
            y: 6 * SquareWidth,
            width: 7 * SquareWidth,
            height: 7 * SquareWidth);

        public static SquareFillPoint CentreOfTopLeftGridSquare = new SquareFillPoint(
            x: ContainingRectangle.X + SquareWidth / 2,
            y: ContainingRectangle.Y + SquareWidth / 2);
        public static SquareFillPoint TopLeftGridSquare = new SquareFillPoint(
            x: ContainingRectangle.X,
            y: ContainingRectangle.Y);

        public static int GridWidth = 13;
        public static int GridHeight = 20;
        public static int ScreenWidth = GridWidth * SquareWidth;
        public static int ScreenHeight = GridHeight * SquareWidth;

        private static readonly List<SquareFillPoint> BorderSquares = new List<SquareFillPoint>();

        public static List<SquareFillPoint> RightHydrantCentrePoints = new List<SquareFillPoint> { new SquareFillPoint(x: 0, y: 0), new SquareFillPoint(x: 1, y: 0), new SquareFillPoint(x: 0, y: 1), new SquareFillPoint(x: 0, y: -1) };
        public static List<SquareFillPoint> FourBarCentrePoints = new List<SquareFillPoint> { new SquareFillPoint(x: 0, y: 0), new SquareFillPoint(x: -1, y: 0), new SquareFillPoint(x: 1, y: 0), new SquareFillPoint(x: 2, y: 0) };
        public static List<SquareFillPoint> SevenCentrePoints = new List<SquareFillPoint> { new SquareFillPoint(x: 0, y: 0), new SquareFillPoint(x: -1, y: 0), new SquareFillPoint(x: 0, y: 1), new SquareFillPoint(x: 0, y: 2) };
        public static List<SquareFillPoint> FourSquareCentrePoints = new List<SquareFillPoint> { new SquareFillPoint(x: 0, y: 0), new SquareFillPoint(x: -1, y: 0), new SquareFillPoint(x: 0, y: 1), new SquareFillPoint(x: -1, y: 1) };
        public static List<SquareFillPoint> LeftCornerCentrePoints = new List<SquareFillPoint> { new SquareFillPoint(x: 0, y: 0), new SquareFillPoint(x: -1, y: 0), new SquareFillPoint(x: 0, y: 1) };
        public static List<SquareFillPoint> UpsideDownTCentrePoints = new List<SquareFillPoint> { new SquareFillPoint(x: 0, y: 0), new SquareFillPoint(x: -1, y: 0), new SquareFillPoint(x: 0, y: -1), new SquareFillPoint(x: 1, y: 0) };
        public static List<SquareFillPoint> ThreePoleCentrePoints = new List<SquareFillPoint> { new SquareFillPoint(x: 0, y: 0), new SquareFillPoint(x: 0, y: -1), new SquareFillPoint(x: 0, y: 1) };
        public static List<SquareFillPoint> TwoPoleCentrePoints = new List<SquareFillPoint> { new SquareFillPoint(x: 0, y: 0), new SquareFillPoint(x: 0, y: -1) };
        public static List<SquareFillPoint> BackwardsLCentrePoints = new List<SquareFillPoint> { new SquareFillPoint(x: 0, y: 0), new SquareFillPoint(x: -1, y: 0), new SquareFillPoint(x: 0, y: -1), new SquareFillPoint(x: 0, y: -2) };
        public static List<SquareFillPoint> SingleSquareCentrePoints = new List<SquareFillPoint> { new SquareFillPoint(x: 0, y: 0) };

        public static List<SquareFillPoint> LeftHydrantCentrePoints = new List<SquareFillPoint> { new SquareFillPoint(x: 0, y: 0), new SquareFillPoint(x: -1, y: 0), new SquareFillPoint(x: 0, y: 1), new SquareFillPoint(x: 0, y: -1) };
        public static List<SquareFillPoint> RightWayUpTCentrePoints = new List<SquareFillPoint> { new SquareFillPoint(x: 0, y: 0), new SquareFillPoint(x: -1, y: 0), new SquareFillPoint(x: 0, y: 1), new SquareFillPoint(x: 1, y: 0) };

        public static List<SquareFillPoint> RightHydrantPoints = new List<SquareFillPoint> { new SquareFillPoint(x: 0, y: 0), new SquareFillPoint(x: 0, y: 1), new SquareFillPoint(x: 0, y: 2), new SquareFillPoint(x: 1, y: 1) };
        public static List<SquareFillPoint> FourBarPoints = new List<SquareFillPoint> { new SquareFillPoint(x: 0, y: 0), new SquareFillPoint(x: 1, y: 0), new SquareFillPoint(x: 2, y: 0), new SquareFillPoint(x: 3, y: 0) };
        public static List<SquareFillPoint> SevenPoints = new List<SquareFillPoint> { new SquareFillPoint(x: 0, y: 0), new SquareFillPoint(x: 1, y: 0), new SquareFillPoint(x: 1, y: 1), new SquareFillPoint(x: 1, y: 2) };
        public static List<SquareFillPoint> FourSquarePoints = new List<SquareFillPoint> { new SquareFillPoint(x: 0, y: 0), new SquareFillPoint(x: 1, y: 0), new SquareFillPoint(x: 0, y: 1), new SquareFillPoint(x: 1, y: 1) };
        public static List<SquareFillPoint> LeftCornerPoints = new List<SquareFillPoint> { new SquareFillPoint(x: 0, y: 0), new SquareFillPoint(x: 1, y: 0), new SquareFillPoint(x: 1, y: 1) };
        public static List<SquareFillPoint> UpsideDownTPoints = new List<SquareFillPoint> { new SquareFillPoint(x: 0, y: 0), new SquareFillPoint(x: -1, y: 1), new SquareFillPoint(x: 0, y: 1), new SquareFillPoint(x: 1, y: 1) };
        public static List<SquareFillPoint> ThreePolePoints = new List<SquareFillPoint> { new SquareFillPoint(x: 0, y: 0), new SquareFillPoint(x: 0, y: 1), new SquareFillPoint(x: 0, y: 2) };
        public static List<SquareFillPoint> TwoPolePoints = new List<SquareFillPoint> { new SquareFillPoint(x: 0, y: 0), new SquareFillPoint(x: 0, y: 1) };
        public static List<SquareFillPoint> BackwardsLPoints = new List<SquareFillPoint> { new SquareFillPoint(x: 0, y: 0), new SquareFillPoint(x: 0, y: 1), new SquareFillPoint(x: 0, y: 2), new SquareFillPoint(x: -1, y: 2) };
        public static List<SquareFillPoint> SingleSquarePoints = new List<SquareFillPoint> { new SquareFillPoint(x: 0, y: 0) };

        public static List<SquareFillPoint> LeftHydrantPoints = new List<SquareFillPoint> { new SquareFillPoint(x: 0, y: 0), new SquareFillPoint(x: -1, y: 1), new SquareFillPoint(x: 0, y: 1), new SquareFillPoint(x: 0, y: 2) };
        public static List<SquareFillPoint> RightWayUpTPoints = new List<SquareFillPoint> { new SquareFillPoint(x: 0, y: 0), new SquareFillPoint(x: 1, y: 0), new SquareFillPoint(x: 2, y: 0), new SquareFillPoint(x: 1, y: 1) };

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
            BuildBorderSquares();
            foreach (var borderSquare in BorderSquares)
            {
                occupiedGridSquares[borderSquare.X][borderSquare.Y].Occupied = true;
            }
        }

		public static ShapeSet MakeHardCodedShapeSet(ISquareViewFactory squareViewFactory)
		{
			var originX = SquareWidth / 2;
			var originY = SquareWidth / 2;

			return new ShapeSet(shapes: new List<Shape> {
                // 1:
                new Shape(colour: SquareFillColour.Red,
					      centreOfShape: new SquareFillPoint(x:originX + 3*SquareWidth, y:originY + 2*SquareWidth),
                          topLeftCorner: new SquareFillPoint(x:3*SquareWidth, y:SquareWidth),
                          relativePoints: RightHydrantCentrePoints,
                          relativePointsTopLeftCorner: RightHydrantPoints,
                          squareFactory: squareViewFactory),
			    // 2:
                new Shape(colour: SquareFillColour.Blue,
						  centreOfShape: new SquareFillPoint(x:originX + 3*SquareWidth, y:originY + 15*SquareWidth),
                          topLeftCorner: new SquareFillPoint(x:2*SquareWidth, y:15*SquareWidth),
                          relativePoints: FourBarCentrePoints,
                          relativePointsTopLeftCorner: FourBarPoints,
                          squareFactory: squareViewFactory),
                // 3:
                new Shape(colour: SquareFillColour.Black,
						  centreOfShape: new SquareFillPoint(x:originX + 10*SquareWidth, y:originY + SquareWidth),
                          topLeftCorner: new SquareFillPoint(x:9*SquareWidth, y:SquareWidth),
                          relativePoints: SevenCentrePoints,
                          relativePointsTopLeftCorner: SevenPoints,
					      squareFactory: squareViewFactory),
                // 4:
                new Shape(colour: SquareFillColour.Orange,
						  centreOfShape: new SquareFillPoint(x:originX + 7*SquareWidth, y:originY + 2*SquareWidth),
                          topLeftCorner: new SquareFillPoint(x:6*SquareWidth, y:2*SquareWidth),
                          relativePoints: FourSquareCentrePoints,
                          relativePointsTopLeftCorner: FourSquarePoints,
                          squareFactory: squareViewFactory),
                // 5:
                new Shape(colour: SquareFillColour.Green,
					      centreOfShape: new SquareFillPoint(x:originX + 8*SquareWidth, y:originY + 15*SquareWidth),
                          topLeftCorner: new SquareFillPoint(x:7*SquareWidth, y:15*SquareWidth),
                          relativePoints: LeftCornerCentrePoints,
                          relativePointsTopLeftCorner: LeftCornerPoints,
                          squareFactory: squareViewFactory),
                // 6:
                new Shape(colour: SquareFillColour.Yellow,
					      centreOfShape: new SquareFillPoint(x:originX, y:originY + 2*SquareWidth),
                          topLeftCorner: new SquareFillPoint(x:0, y:SquareWidth),
                          relativePoints: RightHydrantCentrePoints,
                          relativePointsTopLeftCorner: RightHydrantPoints,
                          squareFactory: squareViewFactory),
			    // 7:
                new Shape(colour: SquareFillColour.Purple,
					      centreOfShape: new SquareFillPoint(x:originX + 3*SquareWidth, y:originY + 18*SquareWidth),
                          topLeftCorner: new SquareFillPoint(x:3*SquareWidth, y:17*SquareWidth),
                          relativePoints: UpsideDownTCentrePoints,
                          relativePointsTopLeftCorner: UpsideDownTPoints,
                          squareFactory: squareViewFactory),
                // 8:
                new Shape(colour: SquareFillColour.Magenta,
						  centreOfShape: new SquareFillPoint(x:originX, y:originY + 17*SquareWidth),
                          topLeftCorner: new SquareFillPoint(x:0, y:16*SquareWidth),
                          relativePoints: ThreePoleCentrePoints,
                          relativePointsTopLeftCorner: ThreePolePoints,
                          squareFactory: squareViewFactory),
                // 9:
                new Shape(colour: SquareFillColour.Brown,
						  centreOfShape: new SquareFillPoint(x:originX + 6*SquareWidth, y:originY + 18*SquareWidth),
                          topLeftCorner: new SquareFillPoint(x:6*SquareWidth, y:17*SquareWidth),
                          relativePoints: TwoPoleCentrePoints,
                          relativePointsTopLeftCorner: TwoPolePoints,
                          squareFactory: squareViewFactory),
                // 10:
                new Shape(colour: SquareFillColour.Cyan,
						  centreOfShape: new SquareFillPoint(x:originX + SquareWidth, y:originY + 9*SquareWidth),
                          topLeftCorner: new SquareFillPoint(x:0, y:9*SquareWidth),
                          relativePoints: FourSquareCentrePoints,
                          relativePointsTopLeftCorner: FourSquarePoints,
                          squareFactory: squareViewFactory),
                // 11:
                new Shape(colour: SquareFillColour.DarkGrey,
					      centreOfShape: new SquareFillPoint(x:originX + SquareWidth, y:originY + 7*SquareWidth),
                          topLeftCorner: new SquareFillPoint(x:SquareWidth, y:5*SquareWidth),
                          relativePoints: BackwardsLCentrePoints,
                          relativePointsTopLeftCorner: BackwardsLPoints,
                          squareFactory: squareViewFactory),
                // 12:
                new Shape(colour: SquareFillColour.Grey,
					      centreOfShape: new SquareFillPoint(x:originX, y:originY + 13*SquareWidth),
                          topLeftCorner: new SquareFillPoint(x:0, y:12*SquareWidth),
                          relativePoints: RightHydrantCentrePoints,
                          relativePointsTopLeftCorner: RightHydrantPoints,
                          squareFactory: squareViewFactory),
                // 13:
                new Shape(colour: SquareFillColour.White,
						  centreOfShape: new SquareFillPoint(x:originX + 11*SquareWidth, y:originY + 16*SquareWidth),
                          topLeftCorner: new SquareFillPoint(x:11*SquareWidth, y:15*SquareWidth),
                          relativePoints: UpsideDownTCentrePoints,
                          relativePointsTopLeftCorner: UpsideDownTPoints,
                          squareFactory: squareViewFactory),
                // 14:
                new Shape(colour: SquareFillColour.LightGrey,
                          centreOfShape: new SquareFillPoint(x:originX + 9*SquareWidth, y:originY + 18*SquareWidth),
                          topLeftCorner: new SquareFillPoint(x:9*SquareWidth, y:18*SquareWidth),
                          relativePoints: SingleSquareCentrePoints,
                          relativePointsTopLeftCorner: SingleSquarePoints,
                          squareFactory: squareViewFactory)
			});
		}

        private static void BuildBorderSquares()
        {
            BorderSquares.Add(new SquareFillPoint(x: 3, y: 5));
            BorderSquares.Add(new SquareFillPoint(x: 4, y: 5));
            BorderSquares.Add(new SquareFillPoint(x: 5, y: 5));
            BorderSquares.Add(new SquareFillPoint(x: 6, y: 5));
            BorderSquares.Add(new SquareFillPoint(x: 7, y: 5));
            BorderSquares.Add(new SquareFillPoint(x: 8, y: 5));
            BorderSquares.Add(new SquareFillPoint(x: 9, y: 5));
            BorderSquares.Add(new SquareFillPoint(x: 10, y: 5));
            BorderSquares.Add(new SquareFillPoint(x: 11, y: 5));
            BorderSquares.Add(new SquareFillPoint(x: 11, y: 6));
            BorderSquares.Add(new SquareFillPoint(x: 11, y: 7));
            BorderSquares.Add(new SquareFillPoint(x: 11, y: 8));
            BorderSquares.Add(new SquareFillPoint(x: 11, y: 9));
            BorderSquares.Add(new SquareFillPoint(x: 11, y: 10));
            BorderSquares.Add(new SquareFillPoint(x: 11, y: 11));
            BorderSquares.Add(new SquareFillPoint(x: 11, y: 12));
            BorderSquares.Add(new SquareFillPoint(x: 11, y: 13));
            BorderSquares.Add(new SquareFillPoint(x: 10, y: 13));
            BorderSquares.Add(new SquareFillPoint(x: 4, y: 13));
            BorderSquares.Add(new SquareFillPoint(x: 3, y: 13));
            BorderSquares.Add(new SquareFillPoint(x: 3, y: 12));
            BorderSquares.Add(new SquareFillPoint(x: 3, y: 11));
            BorderSquares.Add(new SquareFillPoint(x: 3, y: 10));
            BorderSquares.Add(new SquareFillPoint(x: 3, y: 9));
            BorderSquares.Add(new SquareFillPoint(x: 3, y: 8));
            BorderSquares.Add(new SquareFillPoint(x: 3, y: 7));
            BorderSquares.Add(new SquareFillPoint(x: 3, y: 6));
        }
	}
}
