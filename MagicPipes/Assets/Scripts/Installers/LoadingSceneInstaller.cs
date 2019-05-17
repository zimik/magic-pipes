using UnityEngine;
using Zenject;

public class LoadingSceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IGameSettingsLoader>().To<GameSettingsFromJsonFileLoader>().AsTransient();
#if UNITY_EDITOR
        Container.Bind<IFileLoader>().To<UnityEditorFileLoader>().AsTransient();
#elif UNITY_ANDROID
       Container.Bind<IFileLoader>().To<AndroidFileLoader>().AsTransient();
#endif

        Container.Bind<IUserDataLoader>().To<UserDataFromPlayerPrefLoader>().AsTransient(); 

    }
}