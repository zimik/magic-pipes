﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameSceneController : MonoBehaviour {

    [Inject]
    private readonly SignalBus _signalBus;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ExitFromGame()
    {
        _signalBus.Fire<TryExitFromGameSignal>();
    }

}
