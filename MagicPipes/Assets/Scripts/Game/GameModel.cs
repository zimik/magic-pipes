using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameModel : MonoBehaviour {

    public UnityEvent GameInitedEvent;
    public IntUnityEvent ChangePointsEvent;
    public IntUnityEvent ChangeLifesEvent;

    private int _lives;

    private int _points;

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

    public void RemoveLife()
    {
        Lives--;
    }

    public void AddPoint()
    {
        Points++;
    }
    // Use this for initialization
    public void InitGame () {
        Points = 0;
        Lives = 2;
        GameInitedEvent.Invoke();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
