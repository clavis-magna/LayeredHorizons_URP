using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;


[RequireComponent(typeof(GeneratePlaneMesh))]
public class DeformableMesh : MonoBehaviour
{
    //moving this variable to be controlled by the deformer script
    //eventually to be controlled by the data script
    // public float maximumDepression;


    // public List<Vector3> originalVertices;
    public List<Vector3> modifiedVertices;

    //converted vertices is just the XZ modified vertices, used to get distance without regards to the y pos.
    public List<Vector2> convertedVectors;

    public bool edgeSmoothing = true;


    private GeneratePlaneMesh plane;

    public bool additive;

    //called by GeneratePlaneMesh
    //anytime the mesh needs to be regenerated this is called.
    public void MeshRegenerated()
    {
      plane = GetComponent<GeneratePlaneMesh>();
      plane.mesh.MarkDynamic();
      // //references to the mesh, mark as dynamic so that they can change and affect all lists of vertices
      // originalVertices = plane.mesh.vertices.ToList();
      modifiedVertices = plane.mesh.vertices.ToList();

      //extension that converts the vec3 to a vec2 for use in the depression.
      //Gets the x and z of the modified mesh
      //sorry very hacked together
      Vector2[] modifiedVec2 = modifiedVertices.toVector2();
      for (int i = 0; i < modifiedVec2.Length; i++)
      {
        convertedVectors.Add(modifiedVec2[i]);
      }

      //Debug.Log("Mesh Regenerated");
    }


    //This is called in the physics deformer which detects collision from the Collider
    //It then references this add depression which refreshes the values in the mesh.
    public void AddDepression(Vector3 depressionPoint, float radius, float maximumDepression)
    {

        //translate the depression relative to the worldspace and to the point of contact
        //creating a vector3 out of contact point that is relative to worldspace
        var worldPos3 = this.transform.InverseTransformPoint(depressionPoint);

        //removal of the y position of impact
        var worldPos2 = new Vector2(worldPos3.x, worldPos3.z);

        for (int i = 0; i < modifiedVertices.Count; ++i)
        {
            //distance is detecting which pixels in the x and z and y that need to be impacted
            var distance = (worldPos2 - (convertedVectors[i])).sqrMagnitude;

            //Detecting the impact radius in which the vertices of the mesh in that radius will be remade.
            if (distance < radius)
            {
                //newVert is by how much do these vertices change
                Vector3 newVert = modifiedVertices[i];

                if (additive == true)
                {
                    newVert = modifiedVertices[i] + new Vector3(0.0f, 1.0f, 0.0f) * maximumDepression;
                    //vertices to the side of the depression point are also shifted but less. The position number is +1 -1 +MeshSize+1 -MeshSize-1
                    //edge smoothing creates shorter vertices around the collision points for "softer edges".
                    if (edgeSmoothing)
                    {
                        //can someone try to make this code better?
                        //remove the old and bring in the newVert coord
                        //numbered in a clockwise direction 1 being the top
                        Vector3 newVert1 = modifiedVertices[i - 1];
                        Vector3 newVert2 = modifiedVertices[i - plane.zGridlines - 2];
                        Vector3 newVert3 = modifiedVertices[i - plane.zGridlines - 1];
                        Vector3 newVert4 = modifiedVertices[i - plane.zGridlines];
                        Vector3 newVert5 = modifiedVertices[i + 1];
                        Vector3 newVert6 = modifiedVertices[i + plane.zGridlines + 2];
                        Vector3 newVert7 = modifiedVertices[i + plane.zGridlines + 1];
                        Vector3 newVert8 = modifiedVertices[i + plane.zGridlines];

                        newVert1 = modifiedVertices[i - 1] + new Vector3(0.0f, 1.0f, 0.0f) * maximumDepression / 2;
                        newVert2 = modifiedVertices[i - plane.zGridlines - 2] + new Vector3(0.0f, 1.0f, 0.0f) * maximumDepression / 3;
                        newVert3 = modifiedVertices[i - plane.zGridlines - 1] + new Vector3(0.0f, 1.0f, 0.0f) * maximumDepression / 2;
                        newVert4 = modifiedVertices[i - plane.zGridlines] + new Vector3(0.0f, 1.0f, 0.0f) * maximumDepression / 3;
                        newVert5 = modifiedVertices[i + 1] + new Vector3(0.0f, 1.0f, 0.0f) * maximumDepression / 2;
                        newVert6 = modifiedVertices[i + plane.zGridlines + 2] + new Vector3(0.0f, 1.0f, 0.0f) * maximumDepression / 3;
                        newVert7 = modifiedVertices[i + plane.zGridlines + 1] + new Vector3(0.0f, 1.0f, 0.0f) * maximumDepression / 2;
                        newVert8 = modifiedVertices[i + plane.zGridlines] + new Vector3(0.0f, 1.0f, 0.0f) * maximumDepression / 3;

                        modifiedVertices.RemoveAt(i - 1);
                        modifiedVertices.Insert(i - 1, newVert1);

                        modifiedVertices.RemoveAt(i - plane.zGridlines - 2);
                        modifiedVertices.Insert(i - plane.zGridlines - 2, newVert2);

                        modifiedVertices.RemoveAt(i - plane.zGridlines - 1);
                        modifiedVertices.Insert(i - plane.zGridlines - 1, newVert3);

                        modifiedVertices.RemoveAt(i - plane.zGridlines);
                        modifiedVertices.Insert(i - plane.zGridlines, newVert4);

                        modifiedVertices.RemoveAt(i + 1);
                        modifiedVertices.Insert(i + 1, newVert5);

                        modifiedVertices.RemoveAt(i + plane.zGridlines + 2);
                        modifiedVertices.Insert(i + plane.zGridlines + 2, newVert6);

                        modifiedVertices.RemoveAt(i + plane.zGridlines + 1);
                        modifiedVertices.Insert(i + plane.zGridlines + 1, newVert7);

                        modifiedVertices.RemoveAt(i + plane.zGridlines);
                        modifiedVertices.Insert(i + plane.zGridlines, newVert8);
                    }
                }
                else if (additive == false)
                {
                    newVert = new Vector3(modifiedVertices[i].x, 10.0f * maximumDepression, modifiedVertices[i].z);

                }

                modifiedVertices.RemoveAt(i);
                modifiedVertices.Insert(i, newVert);
            }
        }
        plane.mesh.SetVertices(modifiedVertices);
    }

    //Creates a text label at the point of impact above the mesh instead of a mesh deformation
    //maximum depression is used when the meshType is based on height otherwise it will use the position of where the highest part of the mesh is if it is additive.
    public void AddTextLabel(Vector3 pointOfContact, string text, float maximumDepression)
    {
        //translate the depression relative to the worldspace and to the point of contact
        //creating a vector3 out of contact point that is relative to worldspace
        var worldPos3 = this.transform.InverseTransformPoint(pointOfContact);
        //removal of the y position of impact
        var worldPos2 = new Vector2(worldPos3.x, worldPos3.z);

        GameObject thisMarker = Instantiate(Resources.Load("TextParent"), new Vector3(pointOfContact.x, 0, pointOfContact.z), Quaternion.Euler(0, 0, 0)) as GameObject;

        //check for how the meshType is. Additive types will need to be placed on top of the mesh
        //non additive place at the specified position from the data.
        if (additive == true)
        {
            float meshHeight = new float();

            //get the y position of where the contact was. This sets the height of text label
            for (int i = 0; i < modifiedVertices.Count; ++i)
            {
                var distance = (worldPos2 - (convertedVectors[i])).sqrMagnitude;
                if (distance < 0.5)
                {
                    meshHeight = modifiedVertices[i].y;
                }
            }
            thisMarker.transform.position = new Vector3(pointOfContact.x, meshHeight, pointOfContact.z);
        } else if (additive == false)
        {
            thisMarker.transform.position = new Vector3(pointOfContact.x, 10.0f * maximumDepression, pointOfContact.z);
        }
        //Get the created textmeshpro and change the work it is displaying to match the data.
        TextMeshPro nameText = thisMarker.transform.Find("textMesh").GetComponent<TMPro.TextMeshPro>();
        TextMeshPro textBG = thisMarker.transform.Find("textBG").GetComponent<TMPro.TextMeshPro>();

        ////Get the colour of the mesh

        //Color meshColour = GetComponent<Renderer>().material.GetColor("_BaseColor");
        //< mark =#ff800080 padding="2,2,2,2">
        nameText.text = text;
        textBG.text = $"<mark=#ffffff padding='130,40,20,20'>{text}</mark>";
        //textBG.text = $"<mark=#{ColorUtility.ToHtmlStringRGBA(meshColour)}>{text}</mark>";


        //put the textMarker as a child of the quad mesh
        thisMarker.transform.parent = gameObject.transform;

    }
}
