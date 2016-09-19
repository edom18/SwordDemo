using UnityEngine;
using System.Collections;

public class DaggerDebug : MonoBehaviour
{
    public GameObject Target;

    [SerializeField]
    FunnelDagger _funnelDagger;

    bool _isTargetting = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKey(KeyCode.T))
        {
            if (_isTargetting)
            {
                _isTargetting = false;
                _funnelDagger.SetTarget(null);
            }
            else
            {
                _isTargetting = true;
                _funnelDagger.SetTarget(Target);
            }
            //var newPos = Vector3.MoveTowards(Target.transform.position, Vector3.zero, Time.deltaTime);
            //Target.transform.position = newPos;
        }
	}
}
