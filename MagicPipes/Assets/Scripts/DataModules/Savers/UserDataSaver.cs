using System;
using UnityEngine;
using Zenject;

public interface IUserDataSaver
{
    void SaveUserData(Action onComplete = null);
}

public class UserDataToPlayerPrefSaver : IUserDataSaver
{
    [Inject]
    private readonly IUserData _userData;

    public void SaveUserData(Action onComplete)
    {
        PlayerPrefs.SetInt(GlobalSettings.USERS_RECORRD_FIELD_NAME, _userData.Record);

        if (onComplete != null)
        {
            onComplete();
        }
    }
}
