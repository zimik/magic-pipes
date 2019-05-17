using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LoadingSceneController : MonoBehaviour {

    [Inject]
    private readonly IApplicationDataModule _applicationDataModule;

    [Inject]
    private readonly SignalBus _signalBus;

    [Inject]
    private readonly IGameSettingsLoader _gameSettingsLoader;

    [Inject]
    private readonly IUserDataLoader _userDataLoader;
    

    // Use this for initialization
    void Start () {
        _gameSettingsLoader.LoadGameSettings(()=> 
        {
            _userDataLoader.LoadUserData(() =>
            {
                StartCoroutine(EndLoading());
            });
            
        });
    }

    private IEnumerator EndLoading()
    {
        yield return new WaitForSeconds(1f);
        _signalBus.Fire(new LoadingCompleteSignal());
    }
}
