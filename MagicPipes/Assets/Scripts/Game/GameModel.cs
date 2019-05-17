using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class GameModel : MonoBehaviour {


    public UnityEvent GameInitedEvent;
    public IntUnityEvent ChangePointsEvent;
    public IntUnityEvent ChangeLifesEvent;
    public IntUnityEvent ChangeRecordEvent;

    [Inject]
    private readonly IGameSettings _gameSettings;

    [Inject]
    private readonly IUserData _userData;

    [Inject]
    private readonly IUserDataSaver _userDataSaver;

    private int _lives;

    private int _points;

    private int _record;

    public int Points
    {
        get
        {
            return _points;
        }

        set
        {
            _points = value;
            ChangePointsEvent.Invoke(Points);
        }
    }

    public int Lives
    {
        get
        {
            return _lives;
        }

        set
        {
            _lives = value;
            ChangeLifesEvent.Invoke(Lives);
        }
    }

    public int Record
    {
        get
        {
            return _record;
        }

        set
        {
            _record = value;
            ChangeRecordEvent.Invoke(Record);
        }
    }

    public void RemoveLife()
    {
        Lives--;
    }

    public void AddPoint()
    {
        Points++;
        if (Points > Record)
        {
            _userData.Record = Record = Points;
            _userDataSaver.SaveUserData();
        }
    }
    // Use this for initialization
    public void InitGame () {
        Points = 0;
        Lives = _gameSettings.Lives;
        Record = _userData.Record;
        GameInitedEvent.Invoke();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
