using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Linq;
using UnityAsync;
using System.Threading.Tasks;


public class textCreatorScript : MonoBehaviour
{
    private int scaleX;
    private int scaleY;

    [Header("Increase delay period to reduce lag spike")]
    public int delayPeriod = 10;

    [HideInInspector]
    public bool edgeSmoothing = true;

    public async Task createTextCreator(List<Dictionary<string, object>> data, DeformableMesh parentMesh, string m_headerColumn)
    {
        // grab world scale
        scaleX = (int)InitiateWorldScale.mapScale.x;
        scaleY = (int)InitiateWorldScale.mapScale.y;

        for (var i = 0; i < data.Count; i++)
        {

            if ((float)data[i]["latitude"] != null || (float)data[i]["longitude"] != null)
            {
                //check if the lat lon is equal to zero in which it will equate to null
                //all lat lon needs a float value in the csv or it will send error
                if ((float)data[i]["latitude"] != 0.0 && (float)data[i]["longitude"] != 0.0)
                {
                    float[] thisXY = helpers.getXYPos((float)data[i]["latitude"], (float)data[i]["longitude"], scaleX, scaleY);

                    var thisTextCreator = TextPool.Instance.Get();
                    thisTextCreator.transform.rotation = Quaternion.Euler(0, 0, 0);
                    thisTextCreator.transform.position = new Vector3(thisXY[0], -1.0f, thisXY[1]);

                    createText textScript = thisTextCreator.GetComponent<createText>();
                    if (parentMesh != null)
                    {
                        textScript.deformableMesh = parentMesh;
                        textScript.textData = (string)data[i][m_headerColumn];
                    }
                    else
                    {
                        Debug.Log("No DeformableMesh found on parent GameObject!");
                    }
                    thisTextCreator.gameObject.SetActive(true);
                }
            }



            await new WaitForFrames(delayPeriod);
        }
    }
}
