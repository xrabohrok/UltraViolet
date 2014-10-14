using System;
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
        public Cell targetCell;
        public bool pathFound { get; private set; }

        protected List<Cell> path;

        private List<Cell> closedSet;
        private List<Cell> openSet;
        private BinaryHeap<PathNode> neighbors;
        private PathNode treeHead;
        private Cell evaluatingCell;

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
            openSet = GameObject.FindObjectsOfType<Cell>().ToList<Cell>();
            path = new List<Cell>();

            //take out start cell
            if (targetCell == null)
                throw new Exception("no target to generate");
            openSet.Remove(targetCell);
            evaluatingCell = targetCell;

            //generate neighbors
            var current = new PathNode(null, currentCell, 0);
            foreach(var entry in currentCell.neighbors)
            {
                openSet.Remove(entry);
                //link to each other
                var node = new PathNode(treeHead, entry, 1);
                treeHead.Branches.Add(node);
                //calculate heuristic
                node.HValue = hueristic(node.Location, targetCell);

                //push into heap
                neighbors.Insert(node);
            }
            
            //start loop
            while (neighbors.Count() > 0)
            {
                //--if there is still neighbors, choose lowest
                current = neighbors.RemoveRoot();

                //--am I the target? yes no
                if (current.Target)
                {
                    current.Target = true;
                    path.Add(current.Location);
                    continue;
                }

                //--calculate heuristic for all neighbors not in closed set
                foreach(var neighbor in current.Location.neighbors)
                {
                    if(openSet.Contains(neighbor))
                    {
                        openSet.Remove(neighbor);

                        var thisLeaf = new PathNode(current, neighbor, ++current.StepsToHere);

                        thisLeaf.HValue = hueristic(neighbor, targetCell);

                        neighbors.Insert(thisLeaf);
                    }
                }
            }

            if (current == null)
            {
                pathFound = false;
                path = null;
                return;
            }
            pathFound = true;

            //flip the path
            Stack<PathNode> reversePath = new Stack<PathNode>();
            while (current.Parent == null)
            {
                reversePath.Push(current);
                current = current.Parent;
            }

            while(reversePath.Count() > 0)
            {
                path.Add(reversePath.Pop().Location);
            }

        }

        public class PathNode
        {
            public int StepsToHere;
            public int HValue;

            public int Score {
                get{
                    return StepsToHere + HValue;    
                }
            }

            public PathNode Parent;
            public List<PathNode> Branches;

            public Cell Location;

            public bool Target = false;

            public PathNode(PathNode parent, Cell location, int stepsToHere)
            {
                StepsToHere = stepsToHere;
                Parent = parent;
                Location = location;
                Branches = new List<PathNode>();
            }

        }

    }

    public class PathNodeComparer : IComparer<GridNavigator.PathNode>
    {

        public int Compare(GridNavigator.PathNode x, GridNavigator.PathNode y)
        {
            if (x.Score == y.Score)
                return 0;
            if (x.Score > y.Score)
                return 1;
            else return -1;
        }
    }


}
