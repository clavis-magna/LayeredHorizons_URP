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
  [Header("Header column of the data that you want to show")]
  public string headerColumn = "dog";

  public enum DataType
  {
    Word,
    Sound,
    Image
  };

  public DataType dataType;

  public GameObject textMarker;

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
              TextMeshPro nameText = thisMarker.GetComponentInChildren<TMPro.TextMeshPro>();
              nameText.text = (string)data[i][headerColumn];
          }

        }
      } else {
        Debug.LogErrorFormat("Streaming asset not found: {0}", CSVFileName);
      }
    }
}
