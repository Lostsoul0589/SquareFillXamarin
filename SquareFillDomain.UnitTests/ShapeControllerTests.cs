using System;
using NUnit.Framework;
using SquareFillDomain.Builders;
using SquareFillDomain.Controllers;
using SquareFillDomain.Interfaces;
using SquareFillDomain.Models;
using SquareFillDomain.Utils;

namespace SquareFillDomain.UnitTests
{
    [TestFixture]
    public class ShapeControllerTests
    {
        private int _squareWidth = ShapeSetBuilder.SquareWidth;

        private SquareFillPoint _outsideContainingSquareButInsideRightHydrant = new SquareFillPoint(
            x: ShapeSetBuilder.SquareWidth/2,
            y: 2*ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2);

        private SquareFillRect _containingSquare = new SquareFillRect(
            x: ShapeSetBuilder.ContainingSquare.X,
            y: ShapeSetBuilder.ContainingSquare.Y,
            width: ShapeSetBuilder.ContainingSquare.Width,
            height: ShapeSetBuilder.ContainingSquare.Height);

        [Test]
        public void TestOccupiedGridSquareMatrixIsCorrectSizeAtStartOfGame()
        {
            // Arrange & Act
            var shapeController = new ShapeController(
                squareViewMaker: new MockShapeMaker(),
                screenWidth: ShapeSetBuilder.ScreenWidth,
                screenHeight: ShapeSetBuilder.ScreenHeight);
        
            // Assert
            Assert.AreEqual(shapeController.OccupiedGridSquares.Count, ShapeSetBuilder.GridWidth);
            Assert.AreEqual(shapeController.OccupiedGridSquares[0].Count, ShapeSetBuilder.GridHeight);
        }
    }
}
