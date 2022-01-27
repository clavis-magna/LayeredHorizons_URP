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
    [Header("Name of Data File")]
    public string CSVFileName = ".csv";

    [Header("Title of Header Column")]
    public string headerColumn;

    //bools for deforming the mesh and being able to customise colour and opacity all set to true for now to limit options
    //[Header("Deform Mesh Settings (Important if bool is true)")]
    [HideInInspector] public bool deformMesh;
    [HideInInspector] public bool customiseColour = true;
    [HideInInspector] public bool customiseOpacity = true;

    //string for the csv data
    [HideInInspector]
    public List<Dictionary<string, object>> data;

    public List<Dictionary<string, object>> formattedData;


    private int scaleX;
    private int scaleY;

    [HideInInspector] // Hides var below
    public GameObject meshObject;
    [HideInInspector] // Hides var below
    public Renderer meshRenderer;

    //this is the distance between two values in which to split up
    public static float maxClusterDistance = 0.5f;

    //These are for creating the GUI for each hand to be able to turn on and off and adjust settings
    createToggleGUI createToggleScript;
    createAdjustmentGUI createAdjustmentScript;

    //create a class for each of the lat long positions add in variables to check if clustered
    public class latlonPositions
    {
        public bool clustered;
        public Vector2 position;
        public latlonPositions (Vector2 _position)
        {
            position = _position;
            clustered = false;
        }

        //function to find it's neighbours and add them to the cluster
        //This is a recursive function so complex bullshit here
        public void findNeighbours(List<latlonPositions> _latlonList, List<latlonPositions> _cluster)
        {


            //if not clustered yet loop through all points and look for neighbours
            for (var i = 0; i < _latlonList.Count; i++)
            {
                //remove any that are already clustered
                if (_latlonList[i].clustered != true)
                {
                    //check if the distance is within the clusterDistance and that it's not itself
                    float distance = Vector2.Distance(position, _latlonList[i].position);
                    //Debug.Log(distance);

                    if (distance <= maxClusterDistance && distance != 0)
                    {

                        //add it to cluster and find neighbours again which will call this same function again.
                        _cluster.Add(_latlonList[i]);
                        _latlonList[i].clustered = true;
                        //Debug.Log("added cluster");

                        _latlonList[i].findNeighbours(_latlonList, _cluster);
                    }
                }
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        //BetterStreamingAssets plugin for reading files on android devices
        BetterStreamingAssets.Initialize();
        // grab world scale
        // set in the inspector
        scaleX = (int)InitiateWorldScale.mapScale.x;
        scaleY = (int)InitiateWorldScale.mapScale.y;
        loadData();
    }

    //called in async to allow for data to load better and avoid lag spikes.
    //Also to complete the steps in the right order
    async void loadData()
    {

          //check if the file exists in the streaming assets folder
          if (BetterStreamingAssets.FileExists(CSVFileName))
          {

            //convert the csv to a String
            string csvContents = BetterStreamingAssets.ReadAllText(CSVFileName);

            //send the csv string to the csv reader.
            data = CSVReader.Read(csvContents);

            //initiate the new formatted list of data points.
            formattedData = new List<Dictionary<string, object>>();

            //list of lat longs to find where the gaps are and create meshes accordingly
            List<latlonPositions> latlonList = new List<latlonPositions>();


            //format and parse the data first
            //TODO put this in its own separate script
            //loop through each data entry
            for (var i = 0; i < data.Count; i++)
            {
				//check if there is an entry or if it's empty
                if (data[i]["latitude"].Equals("") || data[i]["longitude"].Equals(""))
                {
                    Debug.Log("A Latitude or Longitude is missing from CSV");
                }
                else
                {
                    //add the data that does not have missing lattitude and longitude to the new list
                    formattedData.Add(data[i]);


                    //create a vec2 containing the lat long
                    Vector2 pos = new Vector2((float)data[i]["latitude"], (float)data[i]["longitude"]);

                    latlonPositions latlon = new latlonPositions(pos);

                    //add the lat and long to this list if there is one.
                    latlonList.Add(latlon);
                }
            }

            //find clusters in the list of vectors
            List<List<latlonPositions>> clusterGroup = new List<List<latlonPositions>>();

            //Loop through each value in the list to check against each value in the list again.
            for (var i = 0; i < latlonList.Count; i++)
            {


                //check to see if this point has already been clustered
                if (latlonList[i].clustered != true)
                {
                    //create its own cluster
                    List<latlonPositions> cluster = new List<latlonPositions>();

                    //add itself to this cluster and change its bool
                    cluster.Add(latlonList[i]);
                    latlonList[i].clustered = true;

                    //run the find neighbours which should perpetually find their neighbours
                    latlonList[i].findNeighbours(latlonList, cluster);

                    //add the resulting cluster to the main group of clusters
                    clusterGroup.Add(cluster);
                }
            }

            Debug.Log(clusterGroup.Count);


            GenerateDataMesh dataMeshInstance = new GenerateDataMesh();
            GameObject meshObject = dataMeshInstance.generateMesh(formattedData);



            //Get the deformableMesh GO ready to place in the prefabs further down
            DeformableMesh parentMesh = meshObject.GetComponent<DeformableMesh>();

            meshObject.name = "Mesh-" + name;

            //create the things that deform the mesh
            deformMeshScript createDeformMeshInstance = new deformMeshScript();
            await createDeformMeshInstance.createDeformMesh(formattedData, parentMesh);

            //then create the labels that go on top of the mesh.
            textCreatorScript createTextCreatorInstance = new textCreatorScript();
            await createTextCreatorInstance.createTextCreator(formattedData, parentMesh, headerColumn);


            //tell the left hand GUI to create some toggles when a mesh is fully loaded.
            GameObject leftHand = GameObject.FindGameObjectWithTag("LeftGUI");
            createToggleScript = leftHand.GetComponent<createToggleGUI>();
            createToggleScript.createToggleObject(meshObject, name);

            GameObject rightHand = GameObject.FindGameObjectWithTag("RightGUI");
            createAdjustmentScript = rightHand.GetComponent<createAdjustmentGUI>();
            //create the layer first and then within the script create the valid adjusters.
            createAdjustmentScript.createAdjustmentLayer(meshObject, name, customiseColour, customiseOpacity);


        }
        else
        {
                Debug.LogErrorFormat("Streaming asset not found: {0}", CSVFileName);
        }
    }
}
