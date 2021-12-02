using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Linq;
using UnityAsync;

public class deformMeshScript : MonoBehaviour
{


    private int scaleX;
    private int scaleY;

    [Header("Increase delay period to reduce lag spike")]
    public int delayPeriod = 10;

    public float depressionHeight = 0.3f;
    public float depressionRadius = 2.0f;

    [HideInInspector]
    public bool edgeSmoothing = true;

    public async void createDeformMesh(List<Dictionary<string, object>> data, DeformableMesh parentMesh)
    {
        // grab world scale
        scaleX = (int)InitiateWorldScale.mapScale.x;
        scaleY = (int)InitiateWorldScale.mapScale.y;



        for (var i = 0; i < data.Count; i++)
        {
            float[] thisXY = helpers.getXYPos((float)data[i]["latitude"], (float)data[i]["longitude"], scaleX, scaleY);

            //get an instance from the mesh pool and apply the position and rotation
            var thisDeformer = MeshPool.Instance.Get();
            thisDeformer.transform.rotation = Quaternion.Euler(0, 0, 0);
            thisDeformer.transform.position = new Vector3(thisXY[0], -1.0f, thisXY[1]);

            //get the physics deformer script that is applied on the prefab and set the variables based on the public variables of this script.
            PhysicsDeformer script = thisDeformer.GetComponent<PhysicsDeformer>();
            if (parentMesh != null)
            {
                //edge smoothing bool and attach the deformable mesh as the thing to change.
                parentMesh.edgeSmoothing = edgeSmoothing;
                script.deformableMesh = parentMesh;
            }
            else
            {
                Debug.Log("No DeformableMesh found on parent GameObject!");
            }
            //height and radius of the deformed mesh collision point controlled here
            script.maximumDepression = depressionHeight;
            script.collisionRadius = depressionRadius;

            //the mesh pool instances are recieved as off so set them to true so they can be used.
            thisDeformer.gameObject.SetActive(true);

            await new WaitForFrames(delayPeriod);
        }





    }
}
