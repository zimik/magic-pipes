using UnityEngine;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IUserDataSaver>().To<UserDataToPlayerPrefSaver>().AsTransient();
    }
}