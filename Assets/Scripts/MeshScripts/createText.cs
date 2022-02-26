using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class createText : MonoBehaviour
{
    public string textData;
    public DeformableMesh deformableMesh;
    public float maximumDepression;


    void OnCollisionStay(Collision collision)
    {

      //take the point of collision and send it to the deformable mesh
      foreach(var contact in collision.contacts)
      {

        //tells the deformable mesh to add the label
        deformableMesh.AddTextLabel(contact.point, textData, maximumDepression);

        //remove the gameobject when collision occurs
        TextPool.Instance.ReturnToPool(this);

      }
    }
}
