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

    public bool IsAttacked { get; private set; }

    bool _isAttacking = false;
    float _attackInterval = 0.02f;

	// Use this for initialization
	void Start ()
    {
        _daggers = new GameObject[FunnelCount];
        SetupDaggers();
        IsAttacked = false;
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

    IEnumerator AttackImpl(Vector3 target)
    {
        foreach(var dagger in _daggers)
        {
            yield return new WaitForSeconds(_attackInterval);

            var mover = dagger.GetComponentInChildren<MoveDagger>();
            mover.MoveTo(target);
        }
    }

    /// <summary>
    /// 攻撃を開始
    /// </summary>
    public void Attack(Vector3 target)
    {
        Debug.Log("Attack!!");

        if (_isAttacking)
        {
            return;
        }

        _isAttacking = true;
        IsAttacked = true;

        StartCoroutine(AttackImpl(target));
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
        if (_isAttacking)
        {
            return;
        }

        foreach(var dagger in _daggers)
        {
            var lookAt = dagger.GetComponentInChildren<LookAtDagger>();
            lookAt.SetTarget(null);

            var rotate = dagger.GetComponent<RotateDagger>();
            rotate.StartRotate();
        }
    }
}
