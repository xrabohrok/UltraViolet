using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace ultraviolet.builder
{
    [ExecuteInEditMode]
    public class Grid : MonoBehaviour
    {
        private static int gridCount = 0;

		private const float floatAccuracy = .01f;

        private int oldWidth = 0;
        private int oldLength = 0;
		private float oldCellWidth = 0;

        public int widthCount = 1;
        public int lengthCount = 1;

        public GameObject basePrefab = null;
        public float widthScale = 1.0f;

        public const float baseSize = 1.0f;
        public float width { 
            get { return widthScale * baseSize; } 
            set { widthScale = value / baseSize; } 
        }

        // Use this for initialization
        public void Start()
        {
            gridCount++;
            cellObject(0, 0);              
        }

    
        public void Update()
        {
            if (lengthCount < 0)
                lengthCount = oldLength;
            if (widthCount < 0)
                widthCount = oldWidth;

            if ((oldLength != lengthCount || oldWidth != widthCount))
            {
                oldLength = lengthCount;
                oldWidth = widthCount;

                cleanAll();

                var children = this.transform.GetComponentsInChildren<Cell>();
                var operatingChildren = new List<Cell>();
                foreach (var child in children)
                {
                    operatingChildren.Add(child);
                }

                for (int i = 0; i < widthCount; i++)
                {
                    for (int j = 0; j < lengthCount; j++)
                    {
                        cellObject(i, j);
                    }
                }
            }
		
        }

		private bool floatEquality(float valueA, float valueB)
		{
			return (valueA + floatAccuracy > valueB) && (valueA - floatAccuracy < valueB);
		}

        private GameObject cellObject(int x, int y)
        {
            var tempGameObject = new GameObject(x + "," + y, typeof(Cell));
            tempGameObject.GetComponent<Cell>().Parent = this;
            tempGameObject.GetComponent<Cell>().indexX = x;
            tempGameObject.GetComponent<Cell>().indexY = y;
            tempGameObject.GetComponent<Cell>().updateEdit();
            tempGameObject.GetComponent<Transform>().parent = this.transform;

            return tempGameObject;
        }

        private void cleanAll()
        {
            var children = this.transform.GetComponentsInChildren<Cell>();
            var deadChildren = new List<GameObject>();
            //the enumerator builds the list live, you have to solidify the list before deleting
            foreach(var child in children)
            {
                deadChildren.Add(child.gameObject);
            }

            foreach(var target in deadChildren)
            {
                if (target != null)
                    DestroyImmediate(target);
            }
        }
    }

    [ExecuteInEditMode]
    [Serializable]
    public class Cell : MonoBehaviour
    {
        public Grid Parent;


        [SerializeField]
        public int indexX { get; set; }
        [SerializeField]
        public int indexY { get; set; }

        public void Start()
        {
            indexX = 1;
            indexY = 1;
        }

        public void Update()
        {
        }

        public void resize(float width, float spacing, int up, int right)
        {
            this.transform.localScale = new Vector3(width,width,width);

        }

        public void updateEdit()
        {
            if (Parent != null)
            {
                this.transform.localPosition = new Vector3(Parent.width * indexX,0, Parent.width * indexY);
                this.transform.localScale = new Vector3(Parent.widthScale, Parent.widthScale);
            }
        }

        public void OnDrawGizmos()
        {
            if (Parent != null)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawWireCube(transform.position, new Vector3(Parent.width, Parent.width, Parent.width));
            }
        }
    }
}