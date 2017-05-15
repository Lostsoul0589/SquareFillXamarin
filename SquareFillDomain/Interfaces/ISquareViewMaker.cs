using SquareFillDomain.Utils;

namespace SquareFillDomain.Interfaces
{
    public interface ISquareViewMaker
    {
        ISquareView MakeSquare(SquareFillColour colour);
    }
}