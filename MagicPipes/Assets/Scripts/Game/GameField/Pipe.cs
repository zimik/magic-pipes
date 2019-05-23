using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Pipe : MonoBehaviour {

    private const int ROTATE_POSITIONS = 4;

    private const string EMPTY_PIPE_ANIMATION = "EmptyPipeAnimation";
    private const string EXIT_PIPE_ANIMATION = "FullPipeAnimation";
    private const string PROCESSING_PIPE_ANIMATION = "PipeAnimation";

    private const string PROCESSING_REVERSE_PIPE_ANIMATION = "ReversePipeAnimation";

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

    public Animator Animator;

    //Top, Right, Bottom, Left
    public Connect[] Connects;

    private PipeStates _state;

    private int _rotationState;

    private int _timeState;

    private bool[] _defaultConnectStates;
    private Connect.ConnectFlowDirection[] _defaultDirectionStates;

    private bool _pause = false;

    private string _processingAnimation = PROCESSING_PIPE_ANIMATION;

    public void StartProcessing()
    {
        State = PipeStates.Processing;
    }

    public void StartProcessing(Connect.ConnectType type)
    {
        Connect connect = Connects[Convert.ToInt32(type)];
        if(connect.DefaultFlowDirection == Connect.ConnectFlowDirection.In)
        {
            _processingAnimation = PROCESSING_PIPE_ANIMATION;
//            _processingAnimation = PROCESSING_REVERSE_PIPE_ANIMATION;
        }
        else
        {
            _processingAnimation = PROCESSING_REVERSE_PIPE_ANIMATION;
        }
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
        Animator.enabled = !_pause;
    }

    // Use this for initialization
    void Awake() {
        State = PipeStates.Waiting;

        _defaultConnectStates = new bool[Connects.Length];
        _defaultDirectionStates = new Connect.ConnectFlowDirection[Connects.Length];
        for (int i = 0; i< Connects.Length; i++)
        {
            _defaultConnectStates[i] = Connects[i].IsActive;
            _defaultDirectionStates[i] = Connects[i].DefaultFlowDirection;
        }
    }

    private void UpdateConnects()
    {
        for (int i = 0; i < Connects.Length; i++)
        {
            int defaultIndex = (i + RotationState) % ROTATE_POSITIONS;
            Connects[i].IsActive = _defaultConnectStates[defaultIndex];
            Connects[i].DefaultFlowDirection = _defaultDirectionStates[defaultIndex];
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
        if (Animator != null)
        {
            Animator.Play(EMPTY_PIPE_ANIMATION);
        }

    }

    private void OnEnterProcessingState()
    {
//        TimerLabel.gameObject.SetActive(true);
        _timeState = FlowTimeInSeconds;
        StartCoroutine(ProcessingActivitieCoroutine());
        if(Animator != null)
        {
            Animator.Play(_processingAnimation);
            Animator.speed = 1f/FlowTimeInSeconds;
        }

    }

    private IEnumerator ProcessingActivitieCoroutine()
    {
        while (_timeState >= 0)
        {
//            TimerLabel.text = _timeState.ToString();
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
                    connect.ToPipe.StartProcessing(connect.OppositeType);
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

        if (Animator != null)
        {
            //Animator.Play(EXIT_PIPE_ANIMATION);
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
