using System;
using UnityEngine;
using Zenject;

public interface IGameSettingsLoader
{
    void LoadGameSettings(Action onComplete = null);
}

public class GameSettingsFromJsonFileLoader: IGameSettingsLoader {

    [Inject]
    private IFileLoader _fileLoader;

    [Inject]
    private IGameSettings _gameSettings;

    private Action _onComplete;

    public void LoadGameSettings(Action onComplete = null)
    {
        _onComplete = onComplete;
        _fileLoader.LoadFromFile(GlobalSettings.CONFIG_FILE_NAME, SerializeData);
    }

    private void SerializeData(string jsonSettings)
    {
        JsonSettingFormat jsonSettingFormat = JsonUtility.FromJson<JsonSettingFormat>(jsonSettings);
        _gameSettings.Lives = jsonSettingFormat.Lives;

        if (_onComplete != null)
        {
            _onComplete();
        }
    }

    private class JsonSettingFormat
    {
        public int Lives = 0;
    }
}


