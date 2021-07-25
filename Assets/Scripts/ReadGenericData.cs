using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using TMPro;
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


  [Header("Header column of the data that you want to show")]
  public string headerColumn = "dog";

  public GameObject textMarker;


  [Header("Deform Mesh Settings")]
  public bool deformMesh;

  [Header("Attach Deformer Object and Mesh")]
  public GameObject meshDeformer;
  public GameObject meshObject;

  [Header("Decrease depression height if using edge smoothing")]
  public bool edgeSmoothing = true;

  [Header("Set the radius and height of collisions")]
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
          if (dataType == DataType.Word)
          {
              // convert from lat/long to world units
              // using the helper method in the 'helpers' script
              float[] thisXY = helpers.getXYPos((float)data[i]["latitude"], (float)data[i]["longitude"], scaleX, scaleY);

              // instantiate the marker game object
              // it should be a parent object with a textmesh on a child object
              GameObject thisMarker = Instantiate(textMarker, new Vector3(thisXY[0], 1.0f, thisXY[1]), Quaternion.Euler(0, 0, 0));

              //if the bool is switched to drop deformers
              if (deformMesh)
              {
                //create the gameobjects
                GameObject thisDeformer = Instantiate(meshDeformer, new Vector3(thisXY[0], 5.0f, thisXY[1]), Quaternion.Euler(0, 0, 0));

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


              TextMeshPro nameText = thisMarker.GetComponentInChildren<TMPro.TextMeshPro>();
              nameText.text = (string)data[i][headerColumn];
          }

        }
      } else {
        Debug.LogErrorFormat("Streaming asset not found: {0}", CSVFileName);
      }
    }
}
