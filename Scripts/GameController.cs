using Event;
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
        public static int diamondNumber = 0;
        
        public static GameState State
		{
			get { return _state; }
			set { if (_state != value) { EventManager.onStateChange.Invoke(_state, value); } }
		}

		public static bool IsStarted
		{
            get { return !(_state == GameState.SelectingSkins || _state == GameState.WaitingStart); }
        }

		private static StateChangeEventArgs OnStateChange(StateChangeEventArgs arg)
		{
            if (!arg.canceled)
                _state = arg.newState;
            return arg;
		}

		private void Awake()
        {
            bgButton.onClick.AddListener(() => {
                if (_state != GameState.Playing && _state != GameState.WaitingRespawn && (int)_state + 1 < Enum.GetNames(typeof(GameState)).Length)
                    State = (GameState)((int)_state + 1);
            });
            EventManager.onStateChange.AddListener(OnStateChange, Priority.Lowest);
            if (instance == null && instance != this)
                instance = this;
            else
                Debug.LogError("[Error] There is more than one Game Controller");
		}
	}
}