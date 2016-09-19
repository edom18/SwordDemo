using UnityEngine;
using System.Collections;

public class LookAtDagger : MonoBehaviour
{
    [SerializeField]
    float _rotateSpeed = 200f;

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
        //if (_target == null)
        //{
        //    return;
        //}

        //transform.LookAt(_target.transform);
        //LookAt();
    }

    void LookAt()
    {
        var rot = Quaternion.LookRotation(_target.transform.position - transform.position);
        StartCoroutine(LookAtImpl(rot));
    }

    void CancelLookAt()
    {
        StartCoroutine(LookAtImpl(_originalRotation));
    }

    IEnumerator LookAtImpl(Quaternion rot)
    {
        var t = 0f;

        while(t <= 1f)
        {
            yield return 0;

            var newRot = Quaternion.Lerp(transform.rotation, rot, t);
            transform.rotation = newRot;
            t += Time.deltaTime;
        }
    }

    public void SetTarget(GameObject target)
    {
        if (target == null)
        {
            _target = null;
            CancelLookAt();
        }
        else
        {
            _target = target;
            LookAt();
        }
    }
}
