using System;
using System.Diagnostics;
using SquareFillDomain.Models;

namespace SquareFillDomain.Utils
{
    public class Logger
    {
        private string _message = "";

        // public func WithMessage(message: String) -> Logger
        public Logger WithMessage(string message)
        {
            _message = message + _message;
            return this;
        }

        // public func WithPoint(desc: String, point: SquareFillPoint) -> Logger
        public Logger WithPoint(string desc, SquareFillPoint point)
        {
            // var xCoord = String(point.X);
            var xCoord = point.X.ToString();
            var yCoord = point.Y.ToString();
            _message = _message + " " + desc + "(x:" + xCoord + ",y:" + yCoord + ")" + "; ";

            return this;
        }

        // public func WithShape(desc: String, shape: Shape) -> Logger
        public Logger WithShape(string desc, Shape shape)
        {
            _message = _message + desc + ": ";
            _message = _message + shape.TopLeftCornersAsString();
            _message = _message + "; ";

            return this;
        }

        // public func Clear() -> Logger
        public Logger Clear()
        {
            _message = "";
            return this;
        }

        // public func Log()
        public void Log()
        {
            // NSLog(_message);
            Debug.WriteLine(_message);
        }
    }
}