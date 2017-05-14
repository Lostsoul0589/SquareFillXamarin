using System;
using Foundation;
using SquareFillXamarin.Controllers;
using UIKit;

namespace SquareFillXamarin
{
	public partial class ViewController : UIViewController
	{
        private ShapeController _shapeController;

		protected ViewController(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		    _shapeController = new ShapeController(view: View);
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.
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
                _shapeController.StartMove(cursorPositionAtStart: newLocation);
	        }
	    }

        public override void TouchesMoved(NSSet touches, UIEvent evt)
        {
            var touch = touches.AnyObject as UITouch;
            if (touch != null)
            {
                var newLocation = touch.GetPreciseLocation(View);
                _shapeController.ContinueMove(newLocation: newLocation);
            }
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            var touch = touches.AnyObject as UITouch;
            if (touch != null)
            {
                var touchLocation = touch.GetPreciseLocation(View);
                _shapeController.EndMove(finalLocation: touchLocation);
            }
        }

	    //public override void () {
    //    for touch: AnyObject in touches {
    //        let newLocation = touch.location(in: self.view)
    //        _shapeController.StartMove(cursorPositionAtStart: newLocation)
    //    }
    //}
	}
}
