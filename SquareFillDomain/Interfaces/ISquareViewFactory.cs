using SquareFillDomain.Utils;

namespace SquareFillDomain.Interfaces
{
    // public protocol ISquareViewFactory
    public interface ISquareViewFactory
    {
        // func MakeSquare(colour: SquareFillColour) -> ISquareView
        ISquareView MakeSquare(SquareFillColour colour);
    }
}