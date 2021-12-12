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
    public string CSVFileName = ".csv";

    public enum DataType
    {
        Word,
        Sound,
        Image
    };

    public DataType dataType;

    [Header("Header column needs to match the DataType (string for word, link for audio, etc)")]
    public string headerColumn;

    [Header("Deform Mesh Settings (Important if bool is true)")]
    public bool deformMesh;

    public bool customiseColour = true;
    public bool customiseOpacity = true;


    [HideInInspector]
    public List<Dictionary<string, object>> data;


    private int scaleX;
    private int scaleY;

    [HideInInspector] // Hides var below
    public GameObject meshObject;
    [HideInInspector] // Hides var below
    public Renderer meshRenderer;

    createToggleGUI createToggleScript;
    createAdjustmentGUI createAdjustmentScript;


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

    async void loadData()
    {

          //check if the file exists in the streaming assets folder
          if (BetterStreamingAssets.FileExists(CSVFileName))
          {

                //convert the csv to a String
                string csvContents = BetterStreamingAssets.ReadAllText(CSVFileName);

                //send the csv string to the csv reader.
                data = CSVReader.Read(csvContents);



                GenerateDataMesh dataMeshInstance = new GenerateDataMesh();
                GameObject meshObject = dataMeshInstance.generateMesh(data);


            
                //Get the deformableMesh GO ready to place in the prefabs further down
                DeformableMesh parentMesh = meshObject.GetComponent<DeformableMesh>();

                meshObject.name = "Mesh-" + name;

                //create the things that deform the mesh
                deformMeshScript createDeformMeshInstance = new deformMeshScript();
                createDeformMeshInstance.createDeformMesh(data, parentMesh);

                //then create the labels that go on top of the mesh.
                textCreatorScript createTextCreatorInstance = new textCreatorScript();
                createTextCreatorInstance.createTextCreator(data, parentMesh, headerColumn);


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
