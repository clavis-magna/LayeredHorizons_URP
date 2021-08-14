  using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System;
using UnityAsync;


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

  private bool deformMeshFinished = false;


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

    //needs to be IEnumerator to allow for a delay
  
    async void loadData()
    {

          //check if the file exists in the streaming assets folder
          if (BetterStreamingAssets.FileExists(CSVFileName))
          {
                //convert the csv to a String
                string csvContents = BetterStreamingAssets.ReadAllText(CSVFileName);

                //send the csv string to the csv reader.
                data = CSVReader.Read(csvContents);

                //Get the deformableMesh GO ready to place in the prefabs further down
                DeformableMesh parentMesh = meshObject.GetComponent<DeformableMesh>();

                

                for (var i = 0; i < data.Count; i++)
                {
                      // convert from lat/long to world units
                      // using the helper method in the 'helpers' script
                      float[] thisXY = helpers.getXYPos((float)data[i]["latitude"], (float)data[i]["longitude"], scaleX, scaleY);

                      //WITH POOL
                      if (deformMesh)
                      {

                            //get an instance from the mesh pool and apply the position and rotation
                            var thisDeformer = MeshPool.Instance.Get();
                            thisDeformer.transform.rotation = Quaternion.Euler(0, 0, 0);
                            thisDeformer.transform.position = new Vector3(thisXY[0], -1.0f, thisXY[1]);

                            //get the physics deformer script that is applied on the prefab and set the variables based on the public variables of this script.
                            PhysicsDeformer script = thisDeformer.GetComponent<PhysicsDeformer>();
                            if (parentMesh != null)
                            {
                              //edge smoothing bool and attach the deformable mesh as the thing to change.
                              parentMesh.edgeSmoothing = edgeSmoothing;
                              script.deformableMesh = parentMesh;
                            } else {
                              Debug.Log("No DeformableMesh found on parent GameObject!");
                            }
                            //height and radius of the deformed mesh collision point controlled here
                            script.maximumDepression = depressionHeight;
                            script.collisionRadius = depressionRadius;

                            //the mesh pool instances are recieved as off so set them to true so they can be used.
                            thisDeformer.gameObject.SetActive(true);
                      }

                      //creates a delay after each loop through to prevent lag spikes
                      // yield return new WaitForSeconds(1);
                      await new WaitForFrames(5);
                }


                //The text creators are in async so this will perform once everything above is completed.
                //Which is why I duplicated this.
                for (var i = 0; i < data.Count; i++)
                {
                    // convert from lat/long to world units
                    // using the helper method in the 'helpers' script
                    float[] thisXY = helpers.getXYPos((float)data[i]["latitude"], (float)data[i]["longitude"], scaleX, scaleY);

                    // //needs a wait until the deformMesh = true before this is executed.
                    // // //Creates a ball that will eventually create the text.
                    if (dataType == DataType.Word)
                    {
                        var thisTextCreator = TextPool.Instance.Get();
                        thisTextCreator.transform.rotation = Quaternion.Euler(0, 0, 0);
                        thisTextCreator.transform.position = new Vector3(thisXY[0], -1.0f, thisXY[1]);

                        //GameObject thisTextCreator = Instantiate(textCreator, new Vector3(thisXY[0], 200.0f + 1.0f * i, thisXY[1]), Quaternion.Euler(0, 0, 0));
                        createText textScript = thisTextCreator.GetComponent<createText>();
                        if (parentMesh != null)
                        {
                            textScript.deformableMesh = parentMesh;
                            textScript.textData = (string)data[i][headerColumn];

                        }
                        else
                        {
                            Debug.Log("No DeformableMesh found on parent GameObject!");
                        }
                        thisTextCreator.gameObject.SetActive(true);
                }

                    //creates a delay after each loop through to prevent lag spikes
                    // yield return new WaitForSeconds(1);
                    await new WaitForFrames(5);
                }
        }
        else
        {
                Debug.LogErrorFormat("Streaming asset not found: {0}", CSVFileName);
        }
    }
}
