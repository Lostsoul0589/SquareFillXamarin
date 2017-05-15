﻿using System.Collections;
using System.Collections.Generic;
using SquareFillDomain.Models;

namespace SquareFillXamarin.Models
{
	public class ShapeSet
    {
        public IEnumerable<Shape> Shapes { get; private set; }

		public ShapeSet(List<Shape> shapes)
        {
            Shapes = shapes;
		}

	    public Shape SelectShape(SquareFillPoint selectedPoint)
	    {
            Shape selectedShape = null;
        
            foreach(var shape in Shapes) 
            {
                if (shape.IsInShape(point: selectedPoint))
                {
                    selectedShape = shape;
                }
            }
        
            return selectedShape;
	    }
	}
}