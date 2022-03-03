using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//reusable pool code just needs to make the meshpool with the prefab of choice.
//https://www.youtube.com/watch?v=uxm4a0QnQ9E

public abstract class GenericObjectPool<T> : MonoBehaviour where T : Component
{
  [SerializeField]
  private T prefab;

  public static GenericObjectPool<T> Instance { get; private set; }
  private Queue<T> objects = new Queue<T>();

  //Increase the size of the object pool here
  //The pool will automatically increase if not enough available
  public int amountToPool = 5;

  private void Awake()
  {
    Instance = this;
  }

  //Precreates a pool of instantiated objects which can be accessed through the get.
  private void OnEnable()
  {
    AddObjects(amountToPool);
  }

  //This function is used instead of instantiate.
  //Needs to be put into a coroutine, limiting the number of objects to be created.
  //Need the coroutine to creates a stop in the queue, waiting for objects to be added into the queue first
  //essentially throttling ram usage.
  public T Get()
  {
    // startCoroutine(waitForQueue());
    if (objects.Count == 0)
      AddObjects(1);
    return objects.Dequeue();
  }



  //function for destroy, deactivates and returns an object to the queue.
  public void ReturnToPool(T objectToReturn)
  {
    objectToReturn.gameObject.SetActive(false);
    objects.Enqueue(objectToReturn);
  }

  //Function to create objects
  private void AddObjects(int count)
  {
    for (var i = 0; i < count; i++)
    {
      var newObject = GameObject.Instantiate(prefab);
      newObject.gameObject.SetActive(false);
      objects.Enqueue(newObject);
    }
  }

}
