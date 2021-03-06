﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph_Renderer : MonoBehaviour
{
    /* Goals:
     * Get number of data points
     * evenly space verteices for each along the x axis !INSIDE THE RECT!
     * Get marks for each data point
     * Create a second set of vertecies with the same x, and a y of 0 to 1 depending on percentage
     * Create two triangles for each interval following vertex scheme:
     * 1 3
     * 0 2
     * Render graph with current light colour
    */

    static public bool GenerateGraph(Deck_Handler.Deck deck, Rect container, MeshFilter graph)
    {
        int count = deck.practiceSessions.Count;
        if (count > 1)
        {
            float offsetX = -container.width / 2;
            float offsetY = -container.height / 2;

            Mesh mesh = new Mesh();
            List<Vector3> vertecies = new List<Vector3>();
            List<int> triangles = new List<int>();

            float interval = container.width / (count - 1);
            for (int i = 0; i < count; i++)
            {
                // Assign the vertecies for that mark
                vertecies.Add(new Vector3(offsetX + (i * interval), offsetY, 0));
                vertecies.Add(new Vector3(offsetX + (i * interval), offsetY + (container.height * deck.practiceSessions[i].grade), 0));

                // if a triangle can be created, do so.
                if (i < count - 1)
                {
                    /* vertex scheme:
                     * 1 3
                     * 0 2
                     * 
                     * connections:
                     * 0, 3, 2 & 0, 1, 3
                     */

                    int index = i * 2;
                    triangles.Add(index);
                    triangles.Add(index + 3);
                    triangles.Add(index + 2);

                    triangles.Add(index);
                    triangles.Add(index + 1);
                    triangles.Add(index + 3);
                }
            }

            mesh.vertices = vertecies.ToArray();
            mesh.triangles = triangles.ToArray();

            graph.mesh = mesh;
            return true;
        }
        else
        { return false; }
    }
}
