﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ultraviolet.builder;
using ultraviolet.Generic;

namespace ultraviolet.Actors
{
    public abstract class GridNavigator
    {
        public Cell currentCell;
        public bool NeedsRefresh = true;

        protected List<Cell> path;

        private List<Cell> closedSet;
        private BinaryHeap<PathNode> neighbors;
        private PathNode treeHead;

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
            //get all cells
            //take out start cell
            //generate neighbors
            //start loop
            //--if there is still neighbors, choose lowest
            //--am I the target? yes no
            //--calculate heuristic for all neighbors not in closed set
            //--push newly calculated neighbors to heap
            //stop loop
            //go up tree to generate path.
        }

        public class PathNode
        {
            public int stepsToHere;
            public int hValue;

            public int score;

            public PathNode parent;
            public List<PathNode> branches;

            public bool target = false;

        }

    }

    public class PathNodeComparer : IComparer<GridNavigator.PathNode>
    {

        public int Compare(GridNavigator.PathNode x, GridNavigator.PathNode y)
        {
            if (x.score == y.score)
                return 0;
            if (x.score > y.score)
                return 1;
            else return -1;
        }
    }


}