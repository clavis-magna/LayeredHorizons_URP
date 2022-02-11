using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsDeformer : MonoBehaviour
{

    //Radius of which the collision appears
    public float collisionRadius;
    public DeformableMesh deformableMesh;
    public float maximumDepression;

    void OnCollisionStay(Collision collision)
    {
      // Debug.Log("Contact");


      foreach(var contact in collision.contacts)
      {
        deformableMesh.AddDepression(contact.point, collisionRadius, maximumDepression);
        //remove the gameobject when collision occurs
        // Destroy(gameObject);

        // pooled version of destroy Object
        MeshPool.Instance.ReturnToPool(this);

      }
    }
}
