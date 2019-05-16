using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Pipe : MonoBehaviour {

    private const int ROTATE_POSITIONS = 4;

    private enum PipeStates
    {
        Init,
        Waiting,
        Processing,
        Used
    }

    public TextMeshPro TimerLabel;
    public GameObject PipeImage;

    public int FlowTimeInSeconds;
    
    //Top, Right, Bottom, Left
    public Connect[] Connects;

    private PipeStates _state;

    private int _rotationState;

    private int _timeState;

    private bool[] _defaultConnectStates;

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
            yield return new WaitForSecondsRealtime(1f);
            _timeState--;
        }

        State = PipeStates.Used;
    }

    private void OnEnterUsedState()
    {
        for (int i = 0; i<Connects.Length; i++)
        {
            Connect connect = Connects[i];
            if(connect.IsActive && connect.ToPipe != null)
            {
                if (connect.ToPipe.IsOpenConnect(connect.OppositeType))
                {
                    connect.ToPipe.StartProcessing();
                }
            }
        }
    }

    private void Rotate()
    {
        RotationState++;
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
