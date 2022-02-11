  using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Linq;
using UnityAsync;

//0. This script is assigned a single csv. It will read the data and make sure that there is a latitude and longitude
//1. remove all entries that don't have a latitude or longitude
//2. take those lats and longs and create meshes based on the gaps between the data
//3. create the gameobjects which will deform the mesh
//4. create the text to mark where each of the data points are
//5. tell the handGUIs to create toggles and sliders for each of the files

public class ReadGenericData : MonoBehaviour
{


    // csv filename
    // in streaming assets (include .csv extension)
    [Header("Name of Data File")]
    public string CSVFileName = ".csv";

    [Header("Title of Header Column")]
    public string headerColumn;


    [Header("Title of Column with Numbers indicating by what scale to deform the mesh")]
    public string deformationScaleColumn;

    public bool additiveMesh;


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

    //empty GO
    GameObject meshParent;

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
        public String headText;
        //deform scale is by how much the mesh changes by
        //This isn't essential if empty it will just use a default deform scale
        public float deformScale;

        public latlonPositions(Vector2 _position, String _headText, float _deformScale)
        {
            headText = _headText;
            position = _position;
            deformScale = _deformScale;
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
                    if (distance <= maxClusterDistance && distance != 0)
                    {

                        //add it to cluster and find neighbours again which will call this same function again.
                        _cluster.Add(_latlonList[i]);
                        _latlonList[i].clustered = true;
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


            //Create the empty gameobject that holds all of these layers
            meshParent = new GameObject();
            meshParent.name = "Parent-" + name;

            //format and parse the data first
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

                    String headerText = (string)data[i][headerColumn];
                    float deformationScale = new float();
                    //if this column is empty then just set a default scale for all.
                    //You can either just have the column say nothing or not put a scale at all and it will default to 0.5
                    if (deformationScaleColumn.Equals(""))
                    {
                        Debug.Log("The deformation scale column is empty. A default 0.5 will be applied to each deformation.");
                        deformationScale = 0.5f;
                    } else
                    {
                        if (data[i][deformationScaleColumn].Equals(""))
                        {
                            Debug.Log("A deformation scale value is empty. A default 0.5 will be applied to this entry.");
                            deformationScale = 0.5f;
                        }
                        else
                        {
                            deformationScale = (float)data[i][deformationScaleColumn];
                        }
                    }


                    latlonPositions latlon = new latlonPositions(pos, headerText, deformationScale);

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

            //for each of the clusters
            for(var i = 0; i < clusterGroup.Count; i++)
            {
                //the script that creates the mesh, create one containing the cluster
                GenerateDataMesh dataMeshInstance = new GenerateDataMesh();
                GameObject meshObject = dataMeshInstance.generateMesh(clusterGroup[i]);

                //Get the deformableMesh GO ready to place in the prefabs further down
                DeformableMesh parentMesh = meshObject.GetComponent<DeformableMesh>();
                parentMesh.additive = additiveMesh;

                //rename mesh
                meshObject.name = "Mesh-" + i + "-" + name;

                //set as child of the parent
                meshObject.transform.parent = meshParent.transform;

                //create the things that deform the mesh
                deformMeshScript createDeformMeshInstance = new deformMeshScript();
                await createDeformMeshInstance.createDeformMesh(clusterGroup[i], parentMesh);

                //then create the labels that go on top of the mesh.
                textCreatorScript createTextCreatorInstance = new textCreatorScript();
                await createTextCreatorInstance.createTextCreator(clusterGroup[i], parentMesh);
            }

            //tell the left hand GUI to create some toggles when a mesh is fully loaded.
            //This allows meshes to  be turned on and off on the left hand
            GameObject leftHand = GameObject.FindGameObjectWithTag("LeftGUI");
            createToggleScript = leftHand.GetComponent<createToggleGUI>();
            createToggleScript.createToggleObject(meshParent, name);

            //Tells the right hand to create sliders and toggles
            //There are options to turn them on and off but I've left them on by default for now to reduce complexity
            GameObject rightHand = GameObject.FindGameObjectWithTag("RightGUI");
            createAdjustmentScript = rightHand.GetComponent<createAdjustmentGUI>();
            createAdjustmentScript.createAdjustmentLayer(meshParent, name, customiseColour, customiseOpacity);
        }
        else
        {
                Debug.LogErrorFormat("Streaming asset not found: {0}", CSVFileName);
        }
    }
}
