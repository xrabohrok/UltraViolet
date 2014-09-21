using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ultraviolet.builder;

namespace ultraviolet.Actors
{
    public abstract class GridNavigator
    {
        public Cell currentCell;
        public bool NeedsRefresh = true;

        protected List<Cell> path;

        private List<Cell> closedSet;
        private List<Cell> openSet;

        //Euclidean distance hueristic (distance/cell width)
        private Func<Cell, Cell, int> hueristic = 
            (a, b) => Mathf.RoundToInt( 
                Mathf.Sqrt( 
                    Mathf.Pow(a.transform.position.x + b.transform.position.x , 2) + 
                    Mathf.Pow(a.transform.position.y + b.transform.position.y,  2) + 
                    Mathf.Pow(a.transform.position.z + b.transform.position.z , 2)
                ) / a.width); 

        protected void updatePath( )
        {

        }
    }
}
