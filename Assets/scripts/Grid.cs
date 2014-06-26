using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ultraviolet.builder
{
    public class Grid : MonoBehaviour
    {
        private int oldWidth = 1;
        private int oldLength = 1;

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
            if (basePrefab != null)
            {
                allCells = new List<List<GameObject>>();
                allCells.Add(new List<GameObject>());

                allCells[0].Add(cellObject());                    
            }
        }

        // Update is called once per frame
        public void Update()
        {

        }

        [ExecuteInEditMode]
        public void editorUpdate()
        {
            if ((oldLength != lengthCount || oldWidth != widthCount)&& basePrefab != null)
            {
                allCells = new List<List<GameObject>>();
                for (int i = 0; i < widthCount; i++)
                {
                    allCells.Add(new List<GameObject>());
                    for (int j = 0; i < lengthCount; j++)
                    {
                        allCells = new List<List<GameObject>>();
                        allCells.Add(new List<GameObject>());

                        allCells[0].Add(cellObject());  
                    }
                }
            }
        }

        private GameObject cellObject()
        {
            var tempGameObject = (GameObject)GameObject.Instantiate(basePrefab);
            tempGameObject.AddComponent<Cell>();
            tempGameObject.GetComponent<Cell>().Parent = this;

            return tempGameObject;
        }
    }

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

        [ExecuteInEditMode]
        public void updateEdit()
        {
            this.transform.localPosition = new Vector3(Parent.width * indexX, Parent.width * indexY);
            this.transform.localScale = new Vector3(Parent.widthScale, Parent.widthScale);
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(transform.position, new Vector3(Parent.width, Parent.width, Parent.width));
        }
    }
}