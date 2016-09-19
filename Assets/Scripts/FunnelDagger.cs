using UnityEngine;
using System.Collections;

/// <summary>
/// ダガーをファンネルのように展開する
/// </summary>
public class FunnelDagger : MonoBehaviour
{
    public GameObject DaggerPrefab;
    public int FunnelCount = 8;
    public float FunnelRadius = 1f;

    GameObject _target;
    GameObject[] _daggers;

	// Use this for initialization
	void Start ()
    {
        _daggers = new GameObject[FunnelCount];
        SetupDaggers();
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    /// <summary>
    /// ダガーを生成して円形状に配置する
    /// </summary>
    void SetupDaggers()
    {
        for (int i = 0; i < FunnelCount; i++)
        {
            var dagger = Instantiate(DaggerPrefab);
            var angle = 360f / FunnelCount * i;
            dagger.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
            dagger.transform.parent = transform;
            dagger.transform.localPosition = dagger.transform.up * FunnelRadius;

            _daggers[i] = dagger;
        }
    }

    /// <summary>
    /// 攻撃対象を設定する
    /// </summary>
    /// <param name="target"></param>
    public void SetTarget(GameObject target)
    {
        if (target == null)
        {
            _target = null;
            CancelLookAtTarget();
        }
        else
        {
            _target = target;
            LookAtTarget();
        }
    }

    void LookAtTarget()
    {
        foreach(var dagger in _daggers)
        {
            var lookAt = dagger.GetComponentInChildren<LookAtDagger>();
            lookAt.SetTarget(_target);

            var rotate = dagger.GetComponent<RotateDagger>();
            rotate.StopRotate();
        }
    }

    void CancelLookAtTarget()
    {
        foreach(var dagger in _daggers)
        {
            var lookAt = dagger.GetComponentInChildren<LookAtDagger>();
            lookAt.SetTarget(null);

            var rotate = dagger.GetComponent<RotateDagger>();
            rotate.StartRotate();
        }
    }
}
