using UnityEngine;
using System.Collections;

public class FSMTest : MonoBehaviour
{
    private VRI.FSM.StateMachine _stateMachine;

    bool _switch = false;

	// Use this for initialization
	void Start ()
    {
        _stateMachine = new VRI.FSM.StateMachine();
        _stateMachine.State = new NormalState();

        Destroy(gameObject, 5f);
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            _switch = !_switch;
            if (_switch)
            {
                _stateMachine.State = new OtherState(gameObject);
            }
            else
            {
                _stateMachine.State = new NormalState();
            }
        }
	}

    void OnDestroy()
    {
        _stateMachine.Dispose();
        _stateMachine = null;
    }
}

public class NormalState : VRI.FSM.State
{
    public override void Enter()
    {
        base.Enter();
        Debug.Log("Enter the normal state.");
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("Exit the normal state.");
    }
}

public class OtherState : VRI.FSM.State
{
    private GameObject _target;

    public OtherState(GameObject target)
    {
        _target = target;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Enter the other state.");
        _target.GetComponent<MeshRenderer>().material.color = Color.red;
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("Exit the other state.");
        _target.GetComponent<MeshRenderer>().material.color = Color.blue;
    }

    public override void Stay()
    {
        base.Stay();
        if (_target != null)
        {
            _target.transform.Translate(_target.transform.forward * 0.1f);
        }
    }
}
