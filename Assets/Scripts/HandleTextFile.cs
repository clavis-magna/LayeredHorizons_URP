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

    }

    void Start()
    {
        string text = textFile.text;


        //string jsonString = "{\r\n    \"Items\": [\r\n        {\r\n            \"playerId\": \"8484239823\",\r\n            \"playerLoc\": \"Powai\",\r\n            \"playerNick\": \"Random Nick\"\r\n        },\r\n        {\r\n            \"playerId\": \"512343283\",\r\n            \"playerLoc\": \"User2\",\r\n            \"playerNick\": \"Rand Nick 2\"\r\n        }\r\n    ]\r\n}";
        dataFile[] dataFile = JsonHelper.FromJson<dataFile>(text);

        //create a GO prefab for each count of datafiles
        for (int i = 0; i < dataFile.Length; i++)
        {
            //create the GO for each of the items in the JSON
            GameObject dataReader;
            dataReader = Instantiate(ReadGenData, transform.position, transform.rotation);

            //change the name of the GO
            dataReader.name = dataFile[i].fileName;

            ReadGenericData RGDScript = dataReader.GetComponent<ReadGenericData>();
            RGDScript.CSVFileName = dataFile[i].fileName;
            RGDScript.headerColumn = dataFile[i].headerColumnName;
        }
    }
}
