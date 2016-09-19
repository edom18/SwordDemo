using UnityEngine;
using System.Collections;

public class LookAtDagger : MonoBehaviour
{
    GameObject _target;

    Quaternion _originalRotation;

	// Use this for initialization
	void Start ()
    {
        _originalRotation = transform.rotation;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (_target == null)
        {
            return;
        }

        transform.LookAt(_target.transform);
    }

    public void SetTarget(GameObject target)
    {
        if (target == null)
        {
            _target = null;
            transform.rotation = _originalRotation;
        }
        else
        {
            _target = target;
        }
    }
}
