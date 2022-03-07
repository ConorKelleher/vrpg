using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieManager : MonoBehaviour
{
    public DieFace[] faces;

    public int CheckValue()
    {
        int highestFaceIndex = 0;

        for (int i = 1; i<faces.Length; i++)
        {
            if (faces[i].transform.position.y > faces[highestFaceIndex].transform.position.y)
            {
                highestFaceIndex = i;
            }
        }

        return faces[highestFaceIndex].value;
    }

    [System.Serializable]
    public class DieFace
    {
        public Transform transform;
        public int value;
    }
}
