  using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Linq;
using UnityAsync;


public class ReadGenericData : MonoBehaviour
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

    [Header("Header column needs to match the DataType (string for word, link for audio, etc)")]
  public string headerColumn = "dog";

    [Header("Deform Mesh Settings (Important if bool is true)")]
   public Color meshColor = new Color(0.3f, 0.4f, 0.6f, 0.3f);

    public bool deformMesh;
  //public GameObject meshDeformer;
  public bool edgeSmoothing = true;
  public float depressionHeight = 1.0f;
  public float depressionRadius = 1.0f;

  List<Dictionary<string, object>> data;


  private int scaleX;
  private int scaleY;

  [HideInInspector] // Hides var below
  public GameObject meshObject;
  [HideInInspector] // Hides var below
  public Renderer meshRenderer;


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

    void generateMesh()
    {
        //Lists to store positions of where the lat lon data sits.
        List<float> XList = new List<float>();
        List<float> ZList = new List<float>();

        for (var i = 0; i < data.Count; i++)
        {
            //add the positions to this list to then find where the max min point is
            float[] thisXY = helpers.getXYPos((float)data[i]["latitude"], (float)data[i]["longitude"], scaleX, scaleY);
            XList.Add(thisXY[0]);
            ZList.Add(thisXY[1]);
        }
        float xMin = XList.Min();
        float xMax = XList.Max();

        float zMin = ZList.Min();
        float zMax = ZList.Max();

        //calculate the point at which this data centrepoint is to get position.
        Vector2 centrePoint = new Vector2((xMin + xMax) / 2f , (zMin + zMax) / 2f);


        //Create the GenerateQuad Object which creates the mesh at centrepoint.
        meshObject = Instantiate(Resources.Load("GenerateQuad"), new Vector3(centrePoint.x, -0.5f, centrePoint.y), Quaternion.identity) as GameObject;

        if (meshObject != null)
        {
            Debug.Log("Mesh Plane Generated");

            //Set the size of the mesh leaving a buffer space for the mesh.
            GeneratePlaneMesh meshGenerationScript = meshObject.GetComponent<GeneratePlaneMesh>();
            meshGenerationScript.xSize = xMax - xMin + 10;
            meshGenerationScript.zSize = zMax - zMin + 10;

            //Change the size of the collider too.
            BoxCollider colliderChild = meshObject.transform.GetChild(0).gameObject.GetComponent<BoxCollider>();
            colliderChild.size = new Vector3(xMax - xMin + 10, 10, zMax - zMin + 10);

            //TODO: Make these changable from the inspector
            //Colour of the mesh
            Renderer meshRenderer = meshObject.GetComponent<Renderer>();
            meshRenderer.material.SetColor("_AlbedoColor", meshColor);
            meshRenderer.material.SetColor("_Emission", meshColor);

        }
        else
        {
            Debug.Log("Error: A Mesh Plane Object was not instantiated");
        }

        Debug.Log(xMin);
        Debug.Log(xMax);
        Debug.Log(zMin);
        Debug.Log(zMax);

    }


    async void loadData()
    {

          //check if the file exists in the streaming assets folder
          if (BetterStreamingAssets.FileExists(CSVFileName))
          {
                //convert the csv to a String
                string csvContents = BetterStreamingAssets.ReadAllText(CSVFileName);

                //send the csv string to the csv reader.
                data = CSVReader.Read(csvContents);

                generateMesh();


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
                      await new WaitForFrames(100);
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
                    await new WaitForFrames(100);
                }
        }
        else
        {
                Debug.LogErrorFormat("Streaming asset not found: {0}", CSVFileName);
        }
    }
}
