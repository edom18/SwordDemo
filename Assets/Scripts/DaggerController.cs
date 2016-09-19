using UnityEngine;
using System.Collections;

/// <summary>
/// ダガーを出現させるコントローラ
/// </summary>
public class DaggerController : MonoBehaviour
{
    public GameObject Target;
    public GameObject DaggerFunnelPrefab;
    GameObject _funnelDagger;

    SteamVR_TrackedObject _trackedObject;

    float _motionThreshold = 0.6f;
    float _slashLimit = 300f;

    bool _isTargetting = false;

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
            Debug.Log("Slash!!!!");
        }
	}

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
    }

    void Targetting(bool enable)
    {
        if (_funnelDagger == null)
        {
            return;
        }

        var funnel = _funnelDagger.GetComponent<FunnelDagger>();
        funnel.SetTarget(enable ? Target : null);
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
}
