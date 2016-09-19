using UnityEngine;
using System.Collections;

public class DaggerDebug : MonoBehaviour
{
    public GameObject Target;

    bool _isTargetting = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (!_isTargetting)
            {
                _isTargetting = true;
                var funnel = FindObjectOfType<FunnelDagger>();
                funnel.SetTarget(Target);
            }
            else
            {
                _isTargetting = false;
                var funnel = FindObjectOfType<FunnelDagger>();
                funnel.SetTarget(null);
            }
        }
	}
}
