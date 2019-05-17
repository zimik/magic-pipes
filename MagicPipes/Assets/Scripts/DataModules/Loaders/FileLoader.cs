using System;
using System.IO;
using UnityEngine.Networking;
using UnityEngine;

public interface IFileLoader
{
    void LoadFromFile(string fileName, Action<string> onLoaded);
}

public class UnityEditorFileLoader : IFileLoader
{
    public void LoadFromFile(string fileName, Action<string> onLoaded)
    {
        string configFilePath = Path.Combine(Application.dataPath + "/StreamingAssets", fileName);
        if (File.Exists(configFilePath))
        {
            string configDataString = File.ReadAllText(configFilePath);
            onLoaded(configDataString);
        }
    }    
}

public class AndroidFileLoader : IFileLoader
{
    private UnityWebRequest _unityWebRequest;
    private Action<string> _onLoaded;

    public void LoadFromFile(string fileName, Action<string> onLoaded)
    {
        _onLoaded = onLoaded;

        string configFilePath = Path.Combine("jar:file://" + Application.dataPath + "!/assets", fileName);
        _unityWebRequest = UnityWebRequest.Get(configFilePath);
        AsyncOperation asyncOperation = _unityWebRequest.SendWebRequest();
        asyncOperation.completed += LoadingComplete;        
    }

    void LoadingComplete(AsyncOperation asyncOperation)
    {
        if (_unityWebRequest.isNetworkError || _unityWebRequest.isHttpError)
            Debug.Log(_unityWebRequest.error);
        else
        {
            _onLoaded(_unityWebRequest.downloadHandler.text);
        }
    }




}
