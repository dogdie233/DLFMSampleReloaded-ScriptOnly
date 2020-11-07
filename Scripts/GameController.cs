using Level.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum DeathCause
{
    Obstacle,
    Water,
    Air,
    Custom
}

namespace Level
{
    public enum GameState
	{
        SelectingSkins,  // ѡ��Ƥ���׶�
        WaitingStart,  // �ȴ���ʼ�׶�
        Playing,  // ������Ϸ�׶�
        WaitingRespawn,  // ȷ�ϸ���׶�
        WaitingContinue,  // �����ȴ���ʼ
        GameOver,  // ��Ϸ����(�ܾ�����)
	}

    public class GameController : MonoBehaviour
    {
        [HideInInspector] public static GameController instance = null;
        public Button bgButton;
        private static GameState _state = GameState.SelectingSkins;
        
        public static GameState State
		{
			get { return _state; }
			set { EventManager.RunStateChangeEvent(_state, value, StateChangeEventCompleted); }
		}

		public static bool IsStarted
		{
            get { return !(_state == GameState.SelectingSkins || _state == GameState.WaitingStart); }
        }

		private static void StateChangeEventCompleted(StateChangeEvent arg)
		{
            if (arg.canceled)
                return;
            _state = arg.newState;
            switch (_state)
			{
                case GameState.Playing:
                    EventManager.onStart.Invoke();
                    break;
			}
		}

		private void Awake()
        {
            bgButton.onClick.AddListener(() => { EventManager.onBGButtonClick.Invoke(); });
            bgButton.onClick.AddListener(() => {
                if (_state != GameState.Playing && _state != GameState.WaitingRespawn && (int)_state + 1 < Enum.GetNames(typeof(GameState)).Length)
				{
                    EventManager.RunStateChangeEvent(_state, (GameState)((int)_state + 1), StateChangeEventCompleted);
                }
            });
            if (instance == null && instance != this)
                instance = this;
            else
                Debug.LogError("[Error] There is more than one Game Controller");
		}
	}
}