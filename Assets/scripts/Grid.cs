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

        public int widthCount = 1;
        public int lengthCount = 1;

        public GameObject basePrefab = null;

        public float widthScale = 1.0f;

        public const float baseSize = 1.0f;
        public float width { 
            get { return widthScale * baseSize; } 
            set { widthScale = value / baseSize; } 
        }

        public void Start()
        {
            gridCount++;
        }
    
        public void Update()
        {           
            //It is unlikely the grid itself will do anything, other than have a name
        }

        public void refreshEditorView()
        {
#if UNITY_EDITOR
            cleanAll();

            var tempPosition = this.transform.position;
            this.transform.position = new Vector3();

            for (int i = 0; i < widthCount; i++)
            {
                for (int j = 0; j < lengthCount; j++)
                {
                    cellObject(i, j);
                }
            }

            this.transform.position = tempPosition;
#endif
        }

		private bool floatEquality(float valueA, float valueB)
		{
			return (valueA + floatAccuracy > valueB) && (valueA - floatAccuracy < valueB);
		}

        private GameObject cellObject(int x, int y)
        {
            var tempGameObject = new GameObject(x + "," + y, typeof(Cell));
            tempGameObject.GetComponent<Cell>().Parent = this;
            tempGameObject.GetComponent<Cell>().updateEdit(x, y);
            tempGameObject.GetComponent<Transform>().parent = this.transform;

            return tempGameObject;
        }

        private void cleanAll()
        {
            var children = allChildren();

            foreach(var target in children)
            {
                if (target != null)
                    DestroyImmediate(target.gameObject);
            }
        }

        private List<Cell> allChildren()
        {
            //the enumerator builds the list live, you have to solidify the list before deleting
            var children = this.transform.GetComponentsInChildren<Cell>();
            var operatingChildren = new List<Cell>();
            foreach (var child in children)
            {
                operatingChildren.Add(child);
            }

            return operatingChildren;
        }
    }

    [ExecuteInEditMode]
    [Serializable]
    public class Cell : MonoBehaviour
    {
        public Grid Parent;
        public int indexX { get; private set; }
        public int indexY { get; private set; }


        public void Start()
        {
            indexX = 1;
            indexY = 1;

            if (Parent.basePrefab != null && Application.isPlaying)
                instantiateChild(Parent.basePrefab);
        }

        public void Update()
        {
        }

        public void instantiateChild(GameObject prefab)
        {
            var child = (GameObject)Instantiate(prefab, this.transform.position, this.transform.rotation);
            child.transform.parent = this.transform;
        }

#if UNITY_EDITOR
        public void resize(float width, float spacing, int up, int right)
        {
            this.transform.localScale = new Vector3(width,width,width);
        }

        public void updateEdit(int index_X, int index_Y)
        {
            setPosition(index_X, index_Y);

            indexX = index_X;
            indexY = index_Y;
        }

        private void setPosition(int x, int y)
        {
            if (Parent != null)
            {
                this.transform.localPosition = new Vector3(Parent.width * (float)x , 0, Parent.width * (float)y);
            }
        }

        public void OnDrawGizmos()
        {
            if (Parent != null)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawWireCube(transform.position, new Vector3(Parent.width, .2f, Parent.width));
                //Gizmos.DrawWireCube(transform.position, new Vector3(1, Parent.width, 1));

            }
        }
#endif
    }
}