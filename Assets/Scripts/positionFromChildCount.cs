using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class positionFromChildCount : MonoBehaviour
{
    public int childCountPosition;

    void Start()
    {
        //once the toggleObject is created it will set this and should be it's position on the list, new ones added later.
        childCountPosition = transform.parent.transform.childCount;
    }

    //// Update is called once per frame
    void Update()
    {
        
    }

    public void RepositionChildren(int childPosition)
    {
         

        //transform.position = new Vector3(transform.position.x, transform.position.y - (float)childCountPosition / 10, transform.position.z);

        //childCountPosition = childPosition;
        ////transform.position.y = new float 
        //transform.position = new Vector3(transform.position.x, transform.position.y + (float)childCountPosition / 10, transform.position.z);
    }
}
