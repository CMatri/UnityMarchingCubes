using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Transactions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[ExecuteInEditMode]
public class ChunkManager : MonoBehaviour
{
    public bool update = false;
    public bool regenerate = false;
    public bool computeShader;
    public float scale = 1;
    public int chunkSize = 16;
    public float blockSize = 0.5f;
    public float tolerance = 0.5f;
    public float noiseScale;
    public bool wireFrame = false;
    public bool invertMesh = false;
    public bool doubleSidedMesh = false;
    public bool extrusion = false;

    public Vector3 noiseOffset;
    public Vector3Int numChunks;

    public ITerrainGenerator generator;
    public ComputeShader chunkComputeShader;

    private Chunk[,,] chunks;
    private int worldSize;

    void OnSceneGUI(SceneView sceneView)
    {
        /*Event e = Event.current;
        if (e.type == EventType.MouseDown && e.button == 0)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && points != null)
            {
                for (int i = 0; i < points.Length; i++)
                {
                    Point p = points[i].Value;
                    if (Vector3.Distance(new Vector3(p.x, p.y, p.z) * transform.localScale.x, hit.point) < .3f)
                        p.val = extrusion ? tolerance - 0.1f : tolerance;
                    points[i] = p;
                }
                BuildMesh();
            }
            e.Use();
        }*/
    }

    void Update()
    {
        SceneView.duringSceneGui += OnSceneGUI;
        if (update)
            update = false;
        else
            return;
     
        worldSize = (int)(chunkSize / (blockSize > 0 ? blockSize : 1));
        generator = new PerlinGenerator();//

        if (regenerate)
        {
            CleanChunks();
            GameObject worldObject = GameObject.Find("World");
            Material defaultMat = GetComponent<MeshRenderer>().material;
            chunks = new Chunk[numChunks.x, numChunks.y, numChunks.z];
            regenerate = false;

            for (int x = 0; x < numChunks.x; x++)
            {
                for (int y = 0; y < numChunks.y; y++)
                {
                    for (int z = 0; z < numChunks.z; z++)
                    {
                        GameObject gm = new GameObject($"chunk {x},{y},{z}");
                        Chunk chunk = gm.AddComponent<Chunk>();
                        gm.AddComponent<MeshFilter>();
                        gm.AddComponent<MeshCollider>();
                        gm.AddComponent<MeshRenderer>();
                        chunk.GeneratePoints(new Vector3(x, y, z), worldSize, scale, generator, noiseScale, noiseOffset);
                        chunk.BuildMesh(defaultMat, computeShader, doubleSidedMesh, invertMesh, wireFrame, tolerance, chunkComputeShader, worldSize);
                        gm.transform.parent = worldObject.transform;
                        chunks[x, y, z] = chunk;
                    }
                }
            }
        }
    }

    void CleanChunks()
    {
        Chunk[] chunksLocal = (Chunk[])FindObjectsOfType(typeof(Chunk));
        for (int i = 0; i < chunksLocal.Length; i++)
            DestroyImmediate(chunksLocal[i].gameObject);
    }
}
