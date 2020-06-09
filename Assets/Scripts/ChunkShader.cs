using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ChunkManager;

public class ChunkShader
{
    private static ComputeBuffer pointBuf, triBuf, triCountBuf;

    public struct V3
    {
        public float x, y, z;
    }

    public struct Triangle
    {
        public Vector3 a;
        public Vector3 b;
        public Vector3 c;
        public Vector3 this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0:
                        return a;
                    case 1:
                        return b;
                    default:
                        return c;
                }
            }
        }
    }

    public static Triangle[] RunShader(ComputeShader shader, Point?[] p, int numPointsPerAxis, float isoVal)
    {
        Point[] data = new Point[p.Length];
        for (int i = 0; i < p.Length; i++)
            if (p[i] != null)
                data[i] = p[i].Value;

        CreateBuffers(data.Length);

        pointBuf.SetData(data);
        triBuf.SetCounterValue(0);
        int kernel = shader.FindKernel("March");
        shader.SetBuffer(kernel, "points", pointBuf);
        shader.SetBuffer(kernel, "triangles", triBuf);
        shader.SetFloat("isoLevel", isoVal);
        shader.SetInt("numPointsPerAxis", numPointsPerAxis);

        int numThreads = Mathf.CeilToInt((numPointsPerAxis - 1) / 8f);
        shader.Dispatch(kernel, numThreads, numThreads, numThreads);
        ComputeBuffer.CopyCount(triBuf, triCountBuf, 0);
        int[] triCountArr = { 0 };
        triCountBuf.GetData(triCountArr);
        int numTris = triCountArr[0];

        Triangle[] points = new Triangle[numTris];
        triBuf.GetData(points, 0, 0, numTris);
       
        ReleaseBuffers();

        return points;
    }

    private static void CreateBuffers(int pointLen)
    {
        ReleaseBuffers();
        pointBuf = new ComputeBuffer(pointLen, sizeof(float) * 4);
        triCountBuf = new ComputeBuffer(1, sizeof(int), ComputeBufferType.Raw);
        triBuf = new ComputeBuffer((int) Mathf.Pow(Mathf.Pow(pointLen, 1f / 3f) - 1, 3f) * 5, sizeof(float) * 3 * 3, ComputeBufferType.Append); // len is (pointsPerAxis-1)^3 * 5 (max tris per voxel)
    }

    private static void ReleaseBuffers()
    {
        if(pointBuf != null)
            pointBuf.Dispose();
        if(triBuf != null)
            triBuf.Dispose();
        if(triCountBuf != null)
            triCountBuf.Dispose();
    }
}
