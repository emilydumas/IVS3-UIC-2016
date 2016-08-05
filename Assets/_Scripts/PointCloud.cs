using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;


[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PointCloud : MonoBehaviour {

 	private DataPlot _dataPlot;
    private Mesh mesh;
    int numPoints = 65000;

    // Use this for initialization
    void Start () {
        mesh = new Mesh();

        GetComponent<MeshFilter>().mesh = mesh;
        mesh.name = "Point Cloud";
        CreateMesh();
    }

    void CreateMesh() {
    	 _dataPlot = LoadJson.Instance.LoadFromFile(@"Assets\Data\vecs-depth8.tpz");
    	 Vector3[] points = new Vector3[numPoints];
        int[] indecies = new int[numPoints];
        Color[] colors = new Color[numPoints];

        for (int i=0; i<numPoints; i++)
        {
            var p = _dataPlot.points[i];
            var normal = new Vector4(p.v[0],p.v[1],p.v[2],p.v[3]);
            normal.Normalize();

            points[i] = normal;
            indecies[i] = i;
            colors[i] = new Color(Random.Range(0.0f,1.0f),Random.Range (0.0f,1.0f),Random.Range(0.0f,1.0f),1.0f);
        }

        mesh.vertices = points;
        mesh.colors = colors;
        mesh.SetIndices(indecies, MeshTopology.Points,0);

    }
}
