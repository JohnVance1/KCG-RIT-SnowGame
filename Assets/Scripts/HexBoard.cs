/**
 * @author  Lingxiao Yu
 * @github  http://github.com/KHN190
 */

using UnityEngine;
using EasyButtons;

// Example of hex coordinates for snow flake to grow.
//   We can render using mesh (debug),
//   but mesh generated here is hard to animate.
//   if we want to render with sprites, remove the code below, use the coordinates only.
public class HexBoard : MeshGenBase
{
    [Header("Config")]
    // xsize and ysize of the board
    [Range(1, 10)]
    public int size = 1;
    // flip mesh normals
    public bool flip = true;

    [Header("Debug")]
    public bool render = true;
    public bool debug = true;


    public HexCoordinates[] Coords { get; private set; }


    private void Start()
    {
    }

    private void Update()
    {
        GetComponent<MeshRenderer>().enabled = render;
    }



    #region Mesh Generate Examples
    // Example of generating coordinates using distance to origin point
    public void CreateCoords()
    {
        Debug.Log("Generate coords.");

        int nCoords = 1;
        for (int step = 1; step <= size; step++)
            nCoords += 6 * step;
        Coords = new HexCoordinates[nCoords];

        // the first coord is the origin point
        Coords[0] = HexCoordinates.Origin;
        int i = 1;
        // create the closest hexagon vertices first
        for (int step = 1; step <= size; step++)
        {
            for (int outer = 0; outer < step; outer++)
            {
                for (int dir = 0; dir < 6; dir++)
                {
                    Coords[i] = new HexCoordinates((HexDirection)dir, outer, step);
                    i++;
                }
            }
        }
        Debug.Log("coords created: " + i);
    }

    // Example of creating mesh using the coordinates
    [Button]
    public void CreateMesh()
    {
        Debug.Log("Generate mesh.");

        Clear();

        mesh = new Mesh
        {
            name = "HexMesh"
        };
        GetComponent<MeshFilter>().mesh = mesh;

        CreateCoords();

        // create plane mesh facing upwards
        // it has duplicated vertices, not optimized for now
        foreach (HexCoordinates c1 in Coords)
        {
            Vector3 v1 = c1.ToPosition();
            Vector3 v2 = c1.Step(HexDirection.N).ToPosition();
            Vector3 v3 = c1.Step(HexDirection.NE).ToPosition();
            AddTriangle(v1, v2, v3);
        }

        UpdateMesh();
    }

    private void UpdateMesh()
    {
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        if (flip) FlipNormals();
    }

    [Button]
    public void Clear()
    {
        vertices.Clear();
        triangles.Clear();

        mesh = null;
        GetComponent<MeshFilter>().mesh = null;
    }
    #endregion


    // debug render
    private void OnDrawGizmos()
    {
        if (debug && vertices.Count > 0)
        {
            Gizmos.DrawSphere(vertices[0], .2f);

            for (int i = 1; i < vertices.Count; i++)
            {
                Gizmos.DrawSphere(vertices[i], .1f);
            }
        }
    }
}
