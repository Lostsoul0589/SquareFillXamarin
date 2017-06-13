using System;
using Foundation;
using SquareFillDomain.Builders;
using SquareFillDomain.Controllers;
using SquareFillDomain.Models;
using SquareFillDomain.Utils;
using SquareFillXamarin.UIComponents;
using UIKit;

namespace SquareFillXamarin
{
	public partial class ViewController : UIViewController
	{
        private ShapeController _shapeController;

		protected ViewController(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            var squareMaker = UIViewBuilder.InitialiseUIComponents(view: View);
		    var shapeSetBuilder = new ShapeSetBuilder(squareViewFactory: squareMaker);

            _shapeController = new GameInitialiser()
                .MakeShapeController(shapeSetBuilder: shapeSetBuilder);
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}

	    public override void TouchesBegan(NSSet touches, UIEvent evt)
	    {
	        var touch = touches.AnyObject as UITouch;
	        if (touch != null)
            {
                var newLocation = touch.GetPreciseLocation(View);
                _shapeController.StartMove(
                    SquareFillPoint(
                        x: Convert.ToInt16(newLocation.X), 
                        y: Convert.ToInt16(newLocation.Y)));
	        }
	    }

        public override void TouchesMoved(NSSet touches, UIEvent evt)
        {
            var touch = touches.AnyObject as UITouch;
            if (touch != null)
            {
                var newLocation = touch.GetPreciseLocation(View);
                _shapeController.ContinueMove(
                    SquareFillPoint(
                        x: Convert.ToInt16(newLocation.X),
                        y: Convert.ToInt16(newLocation.Y)));
            }
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            var touch = touches.AnyObject as UITouch;
            if (touch != null)
            {
                var touchLocation = touch.GetPreciseLocation(View);
                _shapeController.EndMove(
                    SquareFillPoint(
                        x: Convert.ToInt16(touchLocation.X),
                        y: Convert.ToInt16(touchLocation.Y)));
            }
        }

	    private SquareFillPoint SquareFillPoint(int x, int y)
	    {
	        return new SquareFillPoint(x: x, y: y);
	    }
	}
}
