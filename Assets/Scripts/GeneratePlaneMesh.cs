using System.Collections;
using System.Collections.Generic;
using UnityEngine;


  [RequireComponent(typeof(MeshFilter))]
public class GeneratePlaneMesh : MonoBehaviour
{

  //public float size = 100;
  public float xSize = 100;
  public float zSize = 100;
  //public int gridSize = 256;

  private MeshFilter filter;

  //get mesh object
  public Mesh mesh
  {
    get { return filter.mesh; }
  }
    // Start is called before the first frame update
    void Start()
    {
        filter = GetComponent<MeshFilter>();
        filter.mesh = GenerateMesh();



        //send a message to call the deformableMesh script
        SendMessage("MeshRegenerated");
    }


    Mesh GenerateMesh()
    {
      Mesh mesh = new Mesh();

      var vertices = new List<Vector3>();
      var normals = new List<Vector3>();
      var uvs = new List<Vector2>();

        int xGridlines = (int)(xSize / 0.3906);
        //returns 4
        int zGridlines = (int)(zSize / 0.3906);
        //returns 2

        //int xGridlines = 2;
        //int zGridlines = 2;


        //for loops to detect overlapping vertices
        for (int x = 0; x < xGridlines + 1; ++x)
      {
        for(int y = 0; y < zGridlines + 1; ++y)
        {
          //sets the four sides of the mesh not the triangles inside.
          //setting how big the mesh is from the size variable.
            vertices.Add(new Vector3(
                -xSize * 0.5f + xSize * (x / ((float)xGridlines)),
                0,
                -zSize * 0.5f + zSize * (y / ((float)zGridlines))));


            //setting the vector 3 to be up.
            normals.Add(Vector3.up);
            //Setting positioning of the material to the corners
            uvs.Add(new Vector2(x / (float)xGridlines, y / (float)zGridlines));
        }
      }

      //setting the gridsize to create a grid inside the size, facing upwards
      //loops to the next xy position without creating overlapping vertices.
      var triangles = new List<int>();
      var zVertCount = zGridlines + 1;
      var xVertCount = xGridlines + 1;

        for (int i = 0; i < zVertCount * xVertCount - zVertCount; ++i)
      {
        if ((i + 1)% zVertCount == 0)
        {
          continue;
        }
        triangles.AddRange(new List<int>()
        {
          i + 1 + zVertCount, i + zVertCount, i,
          i, i + 1, i + zVertCount +1
          });
      }

      mesh.SetVertices(vertices);
      mesh.SetNormals(normals);
      mesh.SetUVs(0, uvs);
      mesh.SetTriangles(triangles, 0);


      // //sets the triangle vertices inside the mesh
      // //these are set in a way to make the vertices face up
      // mesh.SetTriangles(new List<int>()
      // {
      //   3, 1, 0,
      //   3, 2, 1
      // }, 0);


      //sets the four sides of the mesh not the triangles inside.
      //setting how big the mesh is from the size variable.
      // mesh.SetVertices(new List<Vector3>()
      // {
      // new Vector3(-size * 0.5f, 0, -size * 0.5f),
      // new Vector3(size * 0.5f, 0, -size * 0.5f),
      // new Vector3(size * 0.5f, 0, size * 0.5f),
      // new Vector3(-size * 0.5f, 0, size * 0.5f)
      // });
      // //setting the vector 3 to be up.
      // mesh.SetNormals(new List<Vector3>()
      // {
      //   Vector3.up,
      //   Vector3.up,
      //   Vector3.up,
      //   Vector3.up
      // });
      //
      // mesh.SetUVs(0, new List<Vector2>()
      // {
      //   new Vector2(0,0),
      //   new Vector2(1,0),
      //   new Vector2(1,1),
      //   new Vector2(0,1)
      // });


      return mesh;
    }
}
