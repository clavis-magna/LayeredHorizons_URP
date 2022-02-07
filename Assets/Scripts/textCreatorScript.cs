using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Linq;
using UnityAsync;
using System.Threading.Tasks;
using latlonPositions = ReadGenericData.latlonPositions;


public class textCreatorScript : MonoBehaviour
{
    private int scaleX;
    private int scaleY;

    //I would make these variables public but because it's instantiated it ends up being overwritten
    //Make this bigger if experiencing lag spikes
    public int delayPeriod = 5;

    //I've just made this true by default to make things easier
    [HideInInspector]
    public bool edgeSmoothing = true;

    public async Task createTextCreator(List<latlonPositions> data, DeformableMesh parentMesh)
    {
        // grab world scale
        scaleX = (int)InitiateWorldScale.mapScale.x;
        scaleY = (int)InitiateWorldScale.mapScale.y;

        for (var i = 0; i < data.Count; i++)
        {
                    float[] thisXY = helpers.getXYPos(data[i].position.x, data[i].position.y, scaleX, scaleY);

                    var thisTextCreator = TextPool.Instance.Get();
                    thisTextCreator.transform.rotation = Quaternion.Euler(0, 0, 0);
                    thisTextCreator.transform.position = new Vector3(thisXY[0], -1.0f, thisXY[1]);

                    createText textScript = thisTextCreator.GetComponent<createText>();
                    if (parentMesh != null)
                    {
                        textScript.deformableMesh = parentMesh;
                        textScript.textData = data[i].headText;
                    }
                    else
                    {
                        Debug.Log("No DeformableMesh found on parent GameObject!");
                    }
                    thisTextCreator.gameObject.SetActive(true);

            await new WaitForFrames(delayPeriod);
        }
    }
}
