using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Linq;
using UnityAsync;

public class textCreatorScript : MonoBehaviour
{
    private int scaleX;
    private int scaleY;

    [Header("Header column needs to match the DataType (string for word, link for audio, etc)")]
    public string headerColumn = "dog";

    [Header("Increase delay period to reduce lag spike")]
    public int delayPeriod = 10;

    public float depressionHeight = 0.3f;
    public float depressionRadius = 2.0f;

    [HideInInspector]
    public bool edgeSmoothing = true;

    public async void createTextCreator(List<Dictionary<string, object>> data, DeformableMesh parentMesh)
    {
        // grab world scale
        scaleX = (int)InitiateWorldScale.mapScale.x;
        scaleY = (int)InitiateWorldScale.mapScale.y;

        for (var i = 0; i < data.Count; i++)
        {
            float[] thisXY = helpers.getXYPos((float)data[i]["latitude"], (float)data[i]["longitude"], scaleX, scaleY);

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

            await new WaitForFrames(delayPeriod);
        }
    }
}
