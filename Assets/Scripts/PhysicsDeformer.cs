using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsDeformer : MonoBehaviour
{

    //Radius of which the collision appears
    public float collisionRadius = 0.1f;
    public DeformableMesh deformableMesh;

    // void Update()
    // {
    //   if (this.transform.position.y < -1) {
    //     Destroy(gameObject);
    //
    //   }
    // }


    void OnCollisionStay(Collision collision)
    {
      // Debug.Log("Contact");


      foreach(var contact in collision.contacts)
      {
        deformableMesh.AddDepression(contact.point, collisionRadius);
            Destroy(gameObject);

      }
    }
}
