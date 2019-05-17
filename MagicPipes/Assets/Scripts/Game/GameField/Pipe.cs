using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Pipe : MonoBehaviour {

    private const int ROTATE_POSITIONS = 4;

    protected enum PipeStates
    {
        Init,
        Waiting,
        Processing,
        Used
    }

    public TextMeshPro TimerLabel;
    public GameObject PipeImage;

    public int FlowTimeInSeconds;

    public UnityEvent TransitionTookPlaceEvent;
    public UnityEvent TransitionDidNotTakePlaceEvent;

    //Top, Right, Bottom, Left
    public Connect[] Connects;

    private PipeStates _state;

    private int _rotationState;

    private int _timeState;

    private bool[] _defaultConnectStates;

    private bool _pause = false;

    public void StartProcessing()
    {
        State = PipeStates.Processing;
    }

    public bool IsOpenConnect(Connect.ConnectType type)
    {
        if(State == PipeStates.Waiting)
        {
            Connect connect = Connects[Convert.ToInt32(type)];
            return connect.IsActive;
        }

        return false;
    }

    public void SetPause(bool pause)
    {
        _pause = pause;
    }

    // Use this for initialization
    void Awake() {
        State = PipeStates.Waiting;

        _defaultConnectStates = new bool[Connects.Length];
        for(int i = 0; i< _defaultConnectStates.Length; i++)
        {
            _defaultConnectStates[i] = Connects[i].IsActive;
        }
    }

    private void UpdateConnects()
    {
        for (int i = 0; i < _defaultConnectStates.Length; i++)
        {
            Connects[i].IsActive = _defaultConnectStates[(i + RotationState) % ROTATE_POSITIONS];
        }

    }

    public void SetNeighboringPipes(Pipe top, Pipe right, Pipe buttom, Pipe left)
    {
        Connects[Convert.ToInt32(Connect.ConnectType.Top)].ToPipe = top;
        Connects[Convert.ToInt32(Connect.ConnectType.Right)].ToPipe = right;
        Connects[Convert.ToInt32(Connect.ConnectType.Bottom)].ToPipe = buttom;
        Connects[Convert.ToInt32(Connect.ConnectType.Left)].ToPipe = left;
    }


    private void OnMouseDown()
    {
        Rotate();
    }

    private PipeStates State
    {
        set {
            if (State == value)
            {
                return;
            }
            _state = value;
            switch (State)
            {
                case PipeStates.Waiting:
                    OnEnterWaitingState();
                    break;
                case PipeStates.Processing:
                    OnEnterProcessingState();
                    break;
                case PipeStates.Used:
                    OnEnterUsedState();
                    break;
            }

        }

        get { return _state; }
    }

    private void OnEnterWaitingState()
    {
        TimerLabel.gameObject.SetActive(false);

    }

    private void OnEnterProcessingState()
    {
        TimerLabel.gameObject.SetActive(true);
        _timeState = FlowTimeInSeconds;
        StartCoroutine(ProcessingActivitieCoroutine());

    }

    private IEnumerator ProcessingActivitieCoroutine()
    {
        while (_timeState >= 0)
        {
            TimerLabel.text = _timeState.ToString();
            if (_timeState > 0)
            {
                yield return new WaitForSecondsRealtime(1f);
            }

            if (_pause)
            {
                continue;
            }
            _timeState--;
        }

        State = PipeStates.Used;
    }

    private void OnEnterUsedState()
    {
        bool transitionTookPlace = false;

        for (int i = 0; i<Connects.Length; i++)
        {
            Connect connect = Connects[i];
            if(connect.IsActive && connect.ToPipe != null)
            {
                if (connect.ToPipe.IsOpenConnect(connect.OppositeType))
                {
                    connect.ToPipe.StartProcessing();
                    transitionTookPlace = true;
                }
            }
        }

        if (transitionTookPlace)
        {
            TransitionTookPlaceEvent.Invoke();
        }
        else
        {
            TransitionDidNotTakePlaceEvent.Invoke();
        }
    }

    private void Rotate()
    {
        if (_pause)
        {
            return;
        }

        if(IsEnebledForRotateState(State))
        {
            RotationState++;
        }        
    }

    protected virtual bool IsEnebledForRotateState(PipeStates state)
    {
        return state == PipeStates.Waiting;
    }

    private int RotationState
    {
        set
        {
            if(_rotationState == value)
            {
                return;
            }
            _rotationState = value % ROTATE_POSITIONS;
            if (value < 0)
            {
                _rotationState += ROTATE_POSITIONS;
            }

            UpdatePipePosition();
            UpdateConnects();
        }
        get
        {
            return _rotationState;
        }
    }

    private void UpdatePipePosition()
    {
        PipeImage.transform.localRotation = Quaternion.Euler(0, 0, 90 * RotationState);
    }
   
}
