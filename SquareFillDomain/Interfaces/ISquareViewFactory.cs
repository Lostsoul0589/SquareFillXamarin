using SquareFillDomain.Utils;

namespace SquareFillDomain.Interfaces
{
    public interface ISquareViewFactory
    {
        ISquareView MakeSquare(SquareFillColour colour);
    }
}