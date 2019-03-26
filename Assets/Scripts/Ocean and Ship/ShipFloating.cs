using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
namespace Tools{
    public class ShipFloating : MonoBehaviour
    {

        private Transform waterPlane;
        private Cloth planeCloth;
        [SerializeField]
        private int closestVertexIndex = -1;

        // Use this for initialization
        void Start()
        {
            waterPlane = GameObject.Find("OceanWater").transform;
            planeCloth = waterPlane.GetComponent<Cloth>();
        }

        // Update is called once per frame
        void Update()
        {
            GetClosestVertex();
        }

        void GetClosestVertex()
        {
            for (int i = 0; i < planeCloth.vertices.Length; i++)
            {
                if (closestVertexIndex == -1)
                {
                    closestVertexIndex = i;
                }
                float distance = Vector3.Distance(planeCloth.vertices[i], transform.position);
                float closestDistance = Vector3.Distance(planeCloth.vertices[closestVertexIndex], transform.position);
                if (distance < closestDistance)
                    closestVertexIndex = i;
            }
            transform.localPosition = new Vector3(transform.localPosition.x, planeCloth.vertices[closestVertexIndex].y / 10, transform.localPosition.z);
        }
    }
}
