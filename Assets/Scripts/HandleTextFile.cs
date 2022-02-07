using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleTextFile : MonoBehaviour
{

    public TextAsset textFile;
    public GameObject ReadGenData;


    [System.Serializable]
    public class dataFile
    {
        public string fileName;
        public string headerColumnName;
        public string deformationScaleColumnName;

    }

    void Start()
    {
        //create a string based on the json file
        string text = textFile.text;

        //custom dataFile class that contains all the variables for each of the items in the json
        //populate the dataFile array with the items from the textFile
        dataFile[] dataFile = JsonHelper.FromJson<dataFile>(text);

        //create a GO prefab for each count of datafiles
        for (int i = 0; i < dataFile.Length; i++)
        {
            //create the GO for each of the items in the JSON
            GameObject dataReader;
            dataReader = Instantiate(ReadGenData, transform.position, transform.rotation);

            //change the name of the GO
            dataReader.name = dataFile[i].fileName;

            //attach a readGenData to the dataRead GO
            ReadGenericData RGDScript = dataReader.GetComponent<ReadGenericData>();

            //set the fileName and headercolumn name in the readGenData Script
            RGDScript.CSVFileName = dataFile[i].fileName;
            RGDScript.headerColumn = dataFile[i].headerColumnName;
            //check if this exists or if its empty in the JSON
            if(dataFile[i].deformationScaleColumnName != null || dataFile[i].deformationScaleColumnName.Equals(""))
            {
                RGDScript.deformationScaleColumn = dataFile[i].deformationScaleColumnName;
            }
        }
    }
}
