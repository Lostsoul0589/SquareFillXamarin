using System;
using NUnit.Framework;
using SquareFillDomain.Builders;
using SquareFillDomain.Controllers;
using SquareFillDomain.Interfaces;
using SquareFillDomain.Models;
using SquareFillDomain.Utils;

namespace SquareFillDomain.UnitTests
{
    public class MockShapeMaker : ISquareViewMaker
    {
        public ISquareView MakeSquare(SquareFillColour colour)
        {
            return new MockSquareView();
        }
    }
}
