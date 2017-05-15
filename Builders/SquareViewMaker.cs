using UIKit;

namespace SquareFillXamarin.Builders
{
    public class SquareViewMaker : ISquareViewMaker
    {
        private readonly UIView _view;

        public SquareViewMaker(UIView view)
        {
            _view = view;
        }
    
        public ISquareView MakeSquare(SquareFillColour colour)
        {
            return new SquareView(view: _view, colour: colour);
        }
    }
}