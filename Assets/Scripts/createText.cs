using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class createText : MonoBehaviour
{
    public string textData;
    public DeformableMesh deformableMesh;

    void OnCollisionStay(Collision collision)
    {
      foreach(var contact in collision.contacts)
      {
        deformableMesh.AddTextLabel(contact.point, textData);

        //remove the gameobject when collision occurs
        // Destroy(gameObject);
        TextPool.Instance.ReturnToPool(this);

      }
    }
}
