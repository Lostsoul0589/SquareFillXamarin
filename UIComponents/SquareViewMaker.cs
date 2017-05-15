using SquareFillDomain.Interfaces;
using SquareFillDomain.Utils;

namespace SquareFillXamarin.UIComponents
{
    public class SquareViewMaker : ISquareViewMaker
    {
        private readonly UIKit.UIView _view;

        public SquareViewMaker(UIKit.UIView view)
        {
            _view = view;
        }
    
        public ISquareView MakeSquare(SquareFillColour colour)
        {
            return new SquareView(view: _view, colour: colour);
        }
    }
}