using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LoadingSceneController : MonoBehaviour {

    [Inject]
    private readonly IApplicationDataModule _applicationDataModule;

    [Inject]
    private readonly SignalBus _signalBus;

    // Use this for initialization
    void Start () {
        _applicationDataModule.TestString = "This is test string";
        StartCoroutine(EndLoading());
    }

    private IEnumerator EndLoading()
    {
        yield return new WaitForSeconds(1f);
        _signalBus.Fire(new LoadingCompleteSignal());

    }
}
