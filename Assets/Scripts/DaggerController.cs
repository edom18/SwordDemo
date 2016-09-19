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

    float _motionThreshold = 0.9f;

    bool _isTargetting = false;

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
        }
        if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            DestroyDagger();
        }

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
    }
}
