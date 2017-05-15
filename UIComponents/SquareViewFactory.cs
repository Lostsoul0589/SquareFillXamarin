using SquareFillDomain.Interfaces;
using SquareFillDomain.Utils;

namespace SquareFillXamarin.UIComponents
{
    public class SquareViewFactory : ISquareViewFactory
    {
        private readonly UIKit.UIView _view;

        public SquareViewFactory(UIKit.UIView view)
        {
            _view = view;
        }
    
        public ISquareView MakeSquare(SquareFillColour colour)
        {
            return new SquareView(view: _view, colour: colour);
        }
    }
}