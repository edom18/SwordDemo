using UnityEngine;
using System.Collections;

public class DaggerDebug : MonoBehaviour
{
    public GameObject Target;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKey(KeyCode.T))
        {
            var newPos = Vector3.MoveTowards(Target.transform.position, Vector3.zero, Time.deltaTime);
            Target.transform.position = newPos;
        }
	}
}
