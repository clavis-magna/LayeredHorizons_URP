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
      StartCoroutine(loadData());
    }

    //needs to be IEnumerator to allow for a delay
    IEnumerator loadData()
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

          DeformableMesh parentMesh = meshObject.GetComponent<DeformableMesh>();


          //needs a wait until the deformMesh = true before this is executed.
          // //Creates a ball that will eventually create the text.
          // if (dataType == DataType.Word)
          // {
          //   GameObject thisTextCreator = Instantiate(textCreator, new Vector3(thisXY[0], 200.0f + 1.0f * i, thisXY[1]), Quaternion.Euler(0, 0, 0));
          //   createText textScript = thisTextCreator.GetComponent<createText>();
          //   if (parentMesh != null)
          //   {
          //     textScript.deformableMesh = parentMesh;
          //     textScript.textData = (string)data[i][headerColumn];
          //
          //   } else {
          //     Debug.Log("No DeformableMesh found on parent GameObject!");
          //   }
          //
          //   // TextMeshPro nameText = thisMarker.GetComponentInChildren<TMPro.TextMeshPro>();
          //   // nameText.text = (string)data[i][headerColumn];
          // }




          //WITH POOL
          if (deformMesh)
          {
            var thisDeformer = MeshPool.Instance.Get();
            thisDeformer.transform.rotation = Quaternion.Euler(0, 0, 0);
            thisDeformer.transform.position = new Vector3(thisXY[0], 1.0f, thisXY[1]);

            PhysicsDeformer script = thisDeformer.GetComponent<PhysicsDeformer>();
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

            thisDeformer.gameObject.SetActive(true);
          }

          //creates a delay after each loop through to give the performance room to breath
          yield return new WaitForSeconds(1);
        }
      } else {
        Debug.LogErrorFormat("Streaming asset not found: {0}", CSVFileName);
      }
    }
}
