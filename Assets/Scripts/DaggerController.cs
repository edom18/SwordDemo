using UnityEngine;
using System.Collections;

/// <summary>
/// ダガーを出現させるコントローラ
/// </summary>
public class DaggerController : MonoBehaviour
{
    public GameObject DaggerFunnelPrefab;
    GameObject _target;
    GameObject _funnelDagger;

    SteamVR_TrackedObject _trackedObject;

    float _motionThreshold = 0.6f;
    float _slashLimit = 300f;
    float _offsetScale = 0.4f;
    float _smoothTime = 0.8f;

    bool _isTargetting = false;


    Vector3 _moveVelocity;

    Vector3 _prevPosition = Vector3.zero;
    Vector3 _prevVelocity = Vector3.zero;
    Vector3 _acc = Vector3.zero;

	// Use this for initialization
	void Start ()
    {
        _trackedObject = GetComponent<SteamVR_TrackedObject>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (_target == null)
        {
            return;
        }

        var device = SteamVR_Controller.Input((int)_trackedObject.index);
        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            GenerateDagger();
            return;
        }
        if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            DestroyDagger();
            return;
        }

        if (_funnelDagger == null)
        {
            return;
        }

        // ターゲットすべきかの計算
        CalcTarget();
        CalcAcc();

        if (CheckSlach())
        {
            var funnel = _funnelDagger.GetComponent<FunnelDagger>();
            funnel.Attack();
        }
	}

    /// <summary>
    /// 剣を振ったかのチェック
    /// </summary>
    /// <returns></returns>
    bool CheckSlach()
    {
        float power = _acc.magnitude;
        bool slashing = (power >= _slashLimit &&
                         _isTargetting);
        return slashing;
    }

    /// <summary>
    /// 加速度の計算
    /// </summary>
    void CalcAcc()
    {
        if (_prevPosition == Vector3.zero)
        {
            _prevPosition = transform.position;
            return;
        }

        var velocity = (transform.position - _prevPosition) / Time.deltaTime;
        if (_prevVelocity == Vector3.zero)
        {
            _prevVelocity = velocity;
            return;
        }

        var acc = (velocity - _prevVelocity) / Time.deltaTime;
        _acc += acc;

        _prevPosition = transform.position;
        _prevVelocity = velocity;
    }

    /// <summary>
    /// ターゲットするかの計算
    /// </summary>
    void CalcTarget()
    {
        if (_isTargetting)
        {
            CalcTargetInTargetting();
        }
        else
        {
            CalcTargetOutTargetting();
        }
    }

    /// <summary>
    /// ターゲットを確定後の場合の計算
    /// </summary>
    void CalcTargetInTargetting()
    {
        //var dot = Vector3.Dot(transform.forward, Vector3.up);
        CalcTargetOutTargetting();
    }

    /// <summary>
    /// ターゲット確定前の計算
    /// </summary>
    void CalcTargetOutTargetting()
    {
        var dot = Vector3.Dot(transform.forward, Vector3.up);
        if (dot >= _motionThreshold)
        {
            if (_isTargetting)
            {
                return;
            }

            _isTargetting = true;
            Targetting(true);
        }
        else
        {
            if (!_isTargetting)
            {
                return;
            }

            _isTargetting = false;
            Targetting(false);
        }
    }

    /// <summary>
    /// ダガーを生成する
    /// </summary>
    void GenerateDagger()
    {
        if (_funnelDagger != null)
        {
            return;
        }

        _funnelDagger = Instantiate(DaggerFunnelPrefab);

        var gameView = transform.parent.GetComponentInChildren<SteamVR_GameView>();
        _funnelDagger.transform.position = gameView.transform.position + gameView.transform.forward;
        _funnelDagger.transform.rotation = gameView.transform.rotation;

        //_funnelDagger.transform.position = _target.transform.position - _target.transform.forward * 0.5f;
        //_funnelDagger.transform.rotation = _target.transform.rotation;

        MoveToTarget();
    }

    /// <summary>
    /// ファンネルのターゲットの位置を取得する
    /// </summary>
    /// <returns></returns>
    Vector3 GetTargetPosition()
    {
        return _target.transform.position - _target.transform.forward * _offsetScale;
    }

    /// <summary>
    /// ファンネルを指定の位置に移動させる
    /// </summary>
    /// <param name="position"></param>
    void MoveToTarget()
    {
        if (_target == null)
        {
            return;
        }

        StartCoroutine(MoveTo(GetTargetPosition()));
    }

    IEnumerator MoveTo(Vector3 position)
    {
        while (_funnelDagger.transform.position != position)
        {
            yield return 0;
            if (_funnelDagger == null)
            {
                break;
            }
            _funnelDagger.transform.position = Vector3.SmoothDamp(_funnelDagger.transform.position, position, ref _moveVelocity, _smoothTime);
        }
    }

    void Targetting(bool enable)
    {
        if (_funnelDagger == null)
        {
            return;
        }

        var funnel = _funnelDagger.GetComponent<FunnelDagger>();
        funnel.SetTarget(enable ? _target : null);
    }

    void DestroyDagger()
    {
        if (_funnelDagger == null)
        {
            return;
        }

        Destroy(_funnelDagger);
        _funnelDagger = null;

        _prevPosition = Vector3.zero;
        _prevVelocity = Vector3.zero;
        _acc = Vector3.zero;
    }

    public void SetTarget(GameObject target)
    {
        _target = target;

        if (_funnelDagger == null)
        {
            return;
        }

        if (_funnelDagger.GetComponent<FunnelDagger>().IsAttacked)
        {
            return;
        }

        MoveToTarget();
    }
}
