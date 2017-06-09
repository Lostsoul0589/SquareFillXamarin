using System;
using System.Diagnostics;
using SquareFillDomain.Models;

namespace SquareFillDomain.Utils
{
    public class Logger
    {
        private string _message = "";

        public Logger WithMessage(string message)
        {
            _message = message + _message;
            return this;
        }

        public Logger WithPoint(string desc, SquareFillPoint point)
        {
            String xCoord = point.X.ToString();
            String yCoord = point.Y.ToString();
            _message = _message + " " + desc + "(x:" + xCoord + ",y:" + yCoord + ")" + "; ";

            return this;
        }

        public Logger WithShape(string desc, Shape shape)
        {
            _message = _message + desc + ": ";
            _message = _message + shape.TopLeftCornersAsString();
            _message = _message + "; ";

            return this;
        }

        public Logger Clear()
        {
            _message = "";
            return this;
        }

        public void Log()
        {
            Debug.WriteLine(_message);
        }
    }
}