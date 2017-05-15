using System;
using System.Collections.Generic;
using System.Diagnostics;
using SquareFillDomain.Models;

namespace SquareFillDomain.Utils
{
    public class Logger
    {
        private string _message = "";

        public Logger Make(string message)
        {
            _message = message + _message;
            return this;
        }

        public Logger Plus(string desc, SquareFillPoint point)
        {
            String xCoord = point.X.ToString();
            String yCoord = point.Y.ToString();
            _message = _message + desc + "(x:" + xCoord + ",y:" + yCoord + ")" + "; ";

            return this;
        }

        public Logger Plus(string desc, List<Square> squares)
        {
            _message = _message + desc + ": ";
            foreach(var square in squares)
            {
                string originX = square.Origin.X.ToString();
                string originY = square.Origin.Y.ToString();
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