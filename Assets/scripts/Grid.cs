using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace ultraviolet.builder
{
    [ExecuteInEditMode]
    public class Grid : MonoBehaviour
    {
        private int oldWidth = 0;
        private int oldLength = 0;

        public int widthCount = 1;
        public int lengthCount = 1;

        public GameObject basePrefab = null;
        public float widthScale = 1.0f;

        [SerializeField]
        public List<List<GameObject>> allCells;

        public const float baseSize = 1.0f;
        public float width { 
            get { return widthScale * baseSize; } 
            set { widthScale = value / baseSize; } 
        }

        // Use this for initialization
        public void Start()
        {
            allCells = new List<List<GameObject>>();
            allCells.Add(new List<GameObject>());

            allCells[0].Add(cellObject("Constructor", "here"));                    
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

                Debug.Log(String.Format("length: {0}, width: {1}", lengthCount, widthCount));


                allCells = new List<List<GameObject>>();
                for (int i = 0; i < widthCount; i++)
                {
                    allCells.Add(new List<GameObject>());
                    Debug.Log(allCells.Count);

                    for (int j = 0; j < lengthCount; j++)
                    {
                        allCells[i].Add(cellObject(i.ToString(), j.ToString()));  
                    }
                }
            }
        }

        private GameObject cellObject(string x, string y)
        {
            var tempGameObject = new GameObject(x + "," + y, typeof(Cell));
            tempGameObject.AddComponent<Cell>();
            tempGameObject.GetComponent<Cell>().Parent = this;
            tempGameObject.GetComponent<Transform>().parent = this.transform;

            return tempGameObject;
        }

        private void cleanAll()
        {
            var children = this.transform.GetComponentsInChildren<Cell>();
            var deadChildren = new List<GameObject>();
            //the enumerator builds the list live, you have to solidify the list before
            //deleting
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
    public class Cell : MonoBehaviour
    {
        public Grid Parent { get; set; }
        public int indexX { get; set; }
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
                this.transform.localPosition = new Vector3(Parent.width * indexX, Parent.width * indexY);
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