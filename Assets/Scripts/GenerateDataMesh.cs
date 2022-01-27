using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Linq;
using UnityAsync;
using latlonPositions = ReadGenericData.latlonPositions;

public class GenerateDataMesh : MonoBehaviour
{
    private int scaleX;
    private int scaleY;


    [HideInInspector] // Hides var below
    public GameObject meshObject;

    public Color meshColor = new Color(0.3f, 0.4f, 0.6f, 0.3f);

    private int missingData;

    public GameObject generateMesh(List<latlonPositions> data)
    {



        // grab world scale
        scaleX = (int)InitiateWorldScale.mapScale.x;
        scaleY = (int)InitiateWorldScale.mapScale.y;


        //Lists to store positions of where the lat lon data sits.
        List<float> XList = new List<float>();
        List<float> ZList = new List<float>();



        for (var i = 0; i < data.Count; i++) {

            //add the positions to this list to then find where the max min point is
            float[] thisXY = helpers.getXYPos(data[i].position.x, data[i].position.y, scaleX, scaleY);
            XList.Add(thisXY[0]);
            ZList.Add(thisXY[1]);

            //if ((float)data[i]["latitude"] != null || (float)data[i]["longitude"] != null)
            //{
            //    //check if the lat lon is equal to zero in which it will equate to null
            //    //all lat lon needs a float value in the csv or it will send error
            //    if ((float)data[i]["latitude"] != 0.0 && (float)data[i]["longitude"] != 0.0)
            //    {
            //        //add the positions to this list to then find where the max min point is
            //        float[] thisXY = helpers.getXYPos((float)data[i]["latitude"], (float)data[i]["longitude"], scaleX, scaleY);
            //        XList.Add(thisXY[0]);
            //        ZList.Add(thisXY[1]);
            //    }
            //    else
            //    {
            //        missingData++;
            //    }
            //} else
            //{
            //    Debug.Log("Lattitude and Longitude not found in data!");
            //}
        }
        

        float xMin = XList.Min();
        float xMax = XList.Max();

        float zMin = ZList.Min();
        float zMax = ZList.Max();



        //calculate the point at which this data centrepoint is to get position.
        Vector2 centrePoint = new Vector2((xMin + xMax) / 2f, (zMin + zMax) / 2f);


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


            //Colour of the mesh
            Renderer meshRenderer = meshObject.GetComponent<Renderer>();
            meshRenderer.material.SetColor("_BaseColor", meshColor);
        }
        else
        {
            Debug.Log("Error: A Mesh Plane Object was not instantiated");

        }
        Debug.Log("There are " + missingData + " data entries with a 0.0, 0.0 lat/lon");

        return meshObject;

    }
}
