using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using System;

public class readGenericData : MonoBehaviour
{


  // csv filename
  // in streaming assets (include .csv extension)
  [Header("Name of data file here")]
  public string CSVFileName = "dog.csv";

  public enum DataType
  {
    Word,
    Sound,
    Image
  };

  public DataType dataType;
  public GameObject meshObject;


  [Header("Header column needs to match the DataType (string for word, link for audio, etc)")]
  public string headerColumn = "dog";

  [Header("If Data Type == Word")]
  //a textcreator ball goes here MeshInteractionObjects only
  public GameObject textCreator;


  [Header("Deform Mesh Settings (Important if bool is true)")]
  public bool deformMesh;
  public GameObject meshDeformer;
  public bool edgeSmoothing = true;
  public float depressionHeight = 1.0f;
  public float depressionRadius = 1.0f;

  List<Dictionary<string, object>> data;


  private int scaleX;
  private int scaleY;


    // Start is called before the first frame update
    void Start()
    {
      BetterStreamingAssets.Initialize();
      // grab world scale
      // set in the inspector
      scaleX = (int)InitiateWorldScale.mapScale.x;
      scaleY = (int)InitiateWorldScale.mapScale.y;
      loadData();
    }

    private void loadData()
    {

      //check if the file exists in the streaming assets folder
      if (BetterStreamingAssets.FileExists(CSVFileName))
      {
        //convert the csv to a String
        string csvContents = BetterStreamingAssets.ReadAllText(CSVFileName);

        //send the csv string to the csv reader.
        data = CSVReader.Read(csvContents);

        for (var i = 0; i < data.Count; i++)
        {

          // convert from lat/long to world units
          // using the helper method in the 'helpers' script
          float[] thisXY = helpers.getXYPos((float)data[i]["latitude"], (float)data[i]["longitude"], scaleX, scaleY);

          //Creates a ball that will eventually create the text.
          if (dataType == DataType.Word)
          {
            GameObject thisTextCreator = Instantiate(textCreator, new Vector3(thisXY[0], 15.0f + 0.01f * i, thisXY[1]), Quaternion.Euler(0, 0, 0));
            createText textScript = thisTextCreator.GetComponent<createText>();
            DeformableMesh parentMesh = meshObject.GetComponent<DeformableMesh>();
            if (parentMesh != null)
            {
              textScript.deformableMesh = parentMesh;
              textScript.textData = (string)data[i][headerColumn];

            } else {
              Debug.Log("No DeformableMesh found on parent GameObject!");
            }

            // TextMeshPro nameText = thisMarker.GetComponentInChildren<TMPro.TextMeshPro>();
            // nameText.text = (string)data[i][headerColumn];
          }


          //if the bool is switched to drop deformers
          if (deformMesh)
          {
            //create the gameobjects
            GameObject thisDeformer = Instantiate(meshDeformer, new Vector3(thisXY[0], 5.0f + 0.01f * i, thisXY[1]), Quaternion.Euler(0, 0, 0));

            //get the physics deformer script from the marker
            PhysicsDeformer script = thisDeformer.GetComponent<PhysicsDeformer>();

            // assign the deformableMesh to the script
            DeformableMesh parentMesh = meshObject.GetComponent<DeformableMesh>();
            if (parentMesh != null)
            {
              parentMesh.edgeSmoothing = edgeSmoothing;
              script.deformableMesh = parentMesh;
            } else {
              Debug.Log("No DeformableMesh found on parent GameObject!");
            }
            //height and radius of the deformed mesh collision point controlled here
            script.maximumDepression = depressionHeight;
            script.collisionRadius = depressionRadius;
          }



        }
      } else {
        Debug.LogErrorFormat("Streaming asset not found: {0}", CSVFileName);
      }
    }

    // private void CreateTextObject()
    // {
    //   if (BetterStreamingAssets.FileExists(CSVFileName))
    //   {
    //     string csvContents = BetterStreamingAssets.ReadAllText(CSVFileName);
    //
    //     //send the csv string to the csv reader.
    //     data = CSVReader.Read(csvContents);
    //
    //     // //get the modified vertices list from DeformableMesh Script
    //     // //I bet this creates a tonne of trash
    //     // DeformableMesh deformableMesh = meshObject.GetComponent<DeformableMesh>();
    //     // List<Vector3> modifiedVertices = deformableMesh.modifiedVertices;
    //     // List<Vector2> convertedVectors = deformableMesh.convertedVectors;
    //
    //     for (var i = 0; i < data.Count; i++)
    //     {
    //       // convert from lat/long to world units
    //       // using the helper method in the 'helpers' script
    //       float[] thisXY = helpers.getXYPos((float)data[i]["latitude"], (float)data[i]["longitude"], scaleX, scaleY);
    //
    //       //get the vector2 of the position of the text textMarker and convert it to the world position
    //       // var positionVec3 = new Vector3(thisXY[0], 0.0f, thisXY[1]);
    //       // var worldPos3 = this.transform.InverseTransformPoint(positionVec3);
    //       // var worldPos2 = new Vector2(worldPos3.x, worldPos3.z);
    //
    //       // var meshHeight = 5.0f;
    //
    //       // //loop through each vertices to find a distance match.
    //       for (int j = 0; j < modifiedVertices.Count; ++j)
    //       {
    //       //Using distance and caluclating magnitude for each one is way too much of a crash
    //       //distance is detecting which pixels in the x and z and y that need to be impacted
    //         var distance = (worldPos2 - (convertedVectors[j])).sqrMagnitude;
    //
    //         if (distance < 0.5)
    //         {
    //           meshHeight = modifiedVertices[j].y;
    //         }
    //       }
    //       // instantiate the marker game object
    //       // it should be a parent object with a textmesh on a child object
    //       GameObject thisMarker = Instantiate(textMarker, new Vector3(thisXY[0], meshHeight, thisXY[1]), Quaternion.Euler(0, 0, 0));
    //       //Get the created textmeshpro and change the work it is displaying to match the data.
    //       TextMeshPro nameText = thisMarker.GetComponentInChildren<TMPro.TextMeshPro>();
    //       nameText.text = (string)data[i][headerColumn];
    //     }
    //   }
    //
    // }
}
