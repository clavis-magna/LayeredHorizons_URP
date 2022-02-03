using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class readLocationData : MonoBehaviour
{

    // file name of location json data - must be in streaming assets folder
    public string filename = "locationData.json";

    // array to read json location data into
    //see locationData script in the referenceScripts folder for values.
    private locationData[] mylocationData;

    [Header("GO to represent PPLC(capital city)")]
    public GameObject PPLC;

    [Header("YPos of PPLC")]
    public int YPosPPLC;

    [Header("GO to represent PPLA(major city)")]
    public GameObject PPLA;

    [Header("YPos of PPLA")]
    public int YPosPPLA;

    [Header("GO to represent PPL(named locations)")]
    public GameObject PPL;

    [Header("YPos of PPL")]
    public int YPosPPL;

    [Header("GO to represent PPLQ(abandoned location)")]
    public GameObject PPLQ;

    [Header("YPos of PPLQ")]
    public int YPosPPLQ;

    [Header("GO to represent Missing DSG Code(check data for missing DSG value")]
    public GameObject MissingDSGCode;

    // holders for the map scale set in initiateWorldScale method
    private int scaleX;
    private int scaleY;


    // Start is called before the first frame update
    void Start()
    {
        scaleX = (int)InitiateWorldScale.mapScale.x;
        scaleY = (int)InitiateWorldScale.mapScale.y;

        loadData();

    }


    private void loadData()
    {
        // create the file path to the json data as a string
        string dataFilePath = Path.Combine(Application.streamingAssetsPath, filename);
        string filePath = Path.Combine(Application.streamingAssetsPath, filename);
        string dataAsJson;

        // check for platform if android for oculus use web protocol
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW reader = new WWW(dataFilePath);
            while (!reader.isDone) { }
            dataAsJson = reader.text;
        }
        else
        {
            dataAsJson = File.ReadAllText(dataFilePath);
        }

        // format the data into the 'loadedData' array using JsonHelper
        locationData[] loadedData = JsonHelper.FromJson<locationData>(dataAsJson);

        // todo: lineIndex is unused - remove completely
        // int lineIndex = 0;

        for (int i = 0; i < loadedData.Length; i++)
        {
            // In the dat NT refers to 'name type'
            // N: approved (The BGN-approved local official name for a geographic feature)
            // V: Variant: A former name, name in local usage, or other spelling found on various sources.
            // for our current purpouse using the 'N' version only if a 'V' exists ignore it
            // todo: This avoids doubles, but we should look at the data and see if this is the approapriate way to deal with this data
            if (loadedData[i].NT == "N")
            {
                // convert from lat/long to world units
                // using the helper method in the 'helpers' script
                float[] thisXY = helpers.getXYPos(loadedData[i].lat, loadedData[i].lon, scaleX, scaleY);

                // check no other point exists here as a quick and dirty way of avoiding overlap
                // todo: we should eventually base this on a heirachy of location size rating
                bool somethingInMySpot = false;
                if (Physics.CheckSphere(new Vector3(thisXY[0], 0, thisXY[1]), 0.09f))
                {
                    somethingInMySpot = true;
                }

                // in the data dsg refers to DSG: Feature Designation Code
                // PPL: populated place
                // PPLQ: populated place abandoned
                // PPLC: capital
                // PPLA: first oder administrative division (major cities and the like)

                if (loadedData[i].dsg == "PPLC" && !somethingInMySpot)
                {
                    GameObject PPLCMarker = Instantiate(PPLC, new Vector3(thisXY[0], YPosPPLC, thisXY[1]), Quaternion.Euler(0, 0, 0));
                }
                else if (loadedData[i].dsg == "PPLA" && !somethingInMySpot)
                {
                    GameObject PPLAMarker = Instantiate(PPLA, new Vector3(thisXY[0], YPosPPLA, thisXY[1]), Quaternion.Euler(0, 0, 0));
                }
                else if (loadedData[i].dsg == "PPL" && !somethingInMySpot)
                {
                    GameObject PPLMarker = Instantiate(PPL, new Vector3(thisXY[0], YPosPPL, thisXY[1]), Quaternion.Euler(0, 0, 0));
                    //send the lattitude and longitude to the getImage so that you can get map data from the API's.
                    //needs to access the cvhild of the gameobject because that's where I put the script
                    //PPLMarker.GetComponentInChildren<getImageMaterial>().SetValues(loadedData[i].lat, loadedData[i].lon);
                }
                else if (loadedData[i].dsg == "PPLQ" && !somethingInMySpot)
                {
                    GameObject PPLQMarker = Instantiate(PPLQ, new Vector3(thisXY[0], YPosPPLQ, thisXY[1]), Quaternion.Euler(0, 0, 0));
                }
                else
                {
                    GameObject MissingDSGCodeMarker = Instantiate(MissingDSGCode, new Vector3(thisXY[0], 5.0f, thisXY[1]), Quaternion.Euler(0, 0, 0));
                }
            }
        }
    }

}
