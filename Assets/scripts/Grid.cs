using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ultraviolet.builder
{
    public class Grid : MonoBehaviour
    {
        public int widthCount = 1;
        public int heightCount = 1;

        [SerializeField]
        public List<List<Cell>> allCells;

        public const float baseSize = 1.0f;
        public float widthScale { get; set; }
        public float width { 
            get { return widthScale * baseSize; } 
            set { widthScale = value / baseSize; } 
        }

        // Use this for initialization
        public void Start()
        {
            allCells = new List<List<Cell>>();
            allCells.Add(new List<Cell>());
            allCells[0].Add(new Cell());
        }

        // Update is called once per frame
        public void Update()
        {

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