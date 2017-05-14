using System;
using System.Collections.Generic;
using System.Diagnostics;
using CoreGraphics;

namespace SquareFillXamarin.Models
{
    public class Logger
    {
        private string _message = "";

        public Logger Make(string message)
        {
            _message = message + _message;
            return this;
        }

        public Logger Plus(string desc, CGPoint point)
        {
            String xCoord = Convert.ToInt16(point.X).ToString();
            String yCoord = Convert.ToInt16(point.Y).ToString();
            _message = _message + desc + "(x:" + xCoord + ",y:" + yCoord + ")" + "; ";

            return this;
        }

        public Logger Plus(string desc, List<Square> squares)
        {
            _message = _message + desc + ": ";
            foreach(var square in squares)
            {
                string originX = Convert.ToInt16(square.Origin.X).ToString();
                string originY = Convert.ToInt16(square.Origin.Y).ToString();
                _message = _message + originX + "," + originY + " ";
            }
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