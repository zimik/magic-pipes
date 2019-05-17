using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public interface IUserDataLoader
{
    void LoadUserData(Action onComplete = null);
}

public class UserDataFromPlayerPrefLoader : IUserDataLoader
{
    [Inject]
    private readonly IUserData _userData;

	public void LoadUserData(Action onComplete = null)
    {
        _userData.Record = PlayerPrefs.GetInt(GlobalSettings.USERS_RECORRD_FIELD_NAME, 0);

        if (onComplete != null)
        {
            onComplete();
        }
    }
}
