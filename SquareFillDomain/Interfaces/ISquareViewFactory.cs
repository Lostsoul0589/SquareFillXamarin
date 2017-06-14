using SquareFillDomain.Utils;

namespace SquareFillDomain.Interfaces
{
    // protocol ISquareViewFactory
    public interface ISquareViewFactory
    {
        // func MakeSquare(colour: SquareFillColour) -> ISquareView
        ISquareView MakeSquare(SquareFillColour colour);
    }
}