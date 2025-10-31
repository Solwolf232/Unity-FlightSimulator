using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TriangleScanner : MonoBehaviour
{
    [MenuItem("Tools/Find High Poly Objects")]

    static void Init()
    {
        int totalTris = 0;
        int highest = 0;
        GameObject worst = null;

        foreach (MeshFilter mf in FindObjectsOfType<MeshFilter>())
        {
            Mesh m = mf.sharedMesh;
            if (m == null) continue;

            int tris = m.triangles.Length / 3;
            totalTris += tris;

            if (tris > highest)
            {
                highest = tris;
                worst = mf.gameObject;
            }
        }

        Debug.Log($"Total Tris: {totalTris}");
        Debug.Log($"Worst Object: {worst?.name} with {highest} tris");
    }
}
