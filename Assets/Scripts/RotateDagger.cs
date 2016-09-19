using UnityEngine;
using System.Collections;

/// <summary>
/// ダガーを常に回転させる
/// </summary>
public class RotateDagger : MonoBehaviour
{

    bool _canRotate = true;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!_canRotate)
        {
            return;
        }

        transform.Rotate(Vector3.forward, 0.2f);
	}

    /// <summary>
    /// 回転を開始する
    /// </summary>
    public void StartRotate()
    {
        _canRotate = true;
    }

    /// <summary>
    /// 回転を止める
    /// </summary>
    public void StopRotate()
    {
        _canRotate = false;
    }
}
