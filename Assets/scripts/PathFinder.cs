using UnityEngine;
using System;
using System.Collections;
using ultraviolet.builder;

namespace ultraviolet.actor
{
    public class PathFinder : MonoBehaviour
    {

        public Cell currentCell;
        public Cell targetCell;

        void Start()
        {
            if (currentCell == null)
            {
                throw new Exception("This actor must start at a cell.  Please assign currentCell.");
            }
        }

        void Update()
        {

        }

        private void reEvaluate()
        {
            //gather all nodes
            //begin loops
            //--take point node (start with dude)
            //--get all neighbors
            //--score all neighbors
            //--choose next point node (or end)
        }
    }
}