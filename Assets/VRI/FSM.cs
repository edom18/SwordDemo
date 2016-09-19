using UnityEngine;
using System.Collections;

/// <summary>
/// 有限状態機械
/// </summary>
namespace VRI.FSM
{
    /// <summary>
    /// ステートのベースクラス
    /// </summary>
    public class State
    {
        public System.Action OnEnter;
        public System.Action OnExit;
        public System.Action OnStay;

        public virtual void Enter()
        {
            if (OnEnter != null)
            {
                OnEnter();
            }
        }

        public virtual void Exit()
        {
            if (OnExit != null)
            {
                OnExit();
            }
        }

        public virtual void Stay()
        {
            if (OnStay != null)
            {
                OnStay();
            }
        }
    }

    public class StateMachine : System.IDisposable
    {
        private State _state;
        public State State
        {
            get
            {
                return _state;
            }
            set
            {
                if (_state != null)
                {
                    _state.Exit();
                }

                _state = value;

                _state.Enter();
            }
        }

        // Update用のコルーチンを保持（停止用）
        private IEnumerator _coroutine;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public StateMachine()
        {
            _coroutine = Update();
            EnumeratorExecuteManager.Push(_coroutine);
        }

        /// <summary>
        /// Dispose this class
        /// </summary>
        public void Dispose()
        {
            Debug.Log("Dispose " + this);
            EnumeratorExecuteManager.PushStop(_coroutine);
            _state = null;
        }

        /// <summary>
        /// ステートのアップデートを開始する
        /// </summary>
        /// <returns></returns>
        IEnumerator Update()
        {
            while(true)
            {
                if (_state != null)
                {
                    _state.Stay();
                    yield return 0;
                }
            }
        }
    }
}
