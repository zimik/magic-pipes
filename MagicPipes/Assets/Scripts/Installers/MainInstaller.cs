using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "MainInstaller", menuName = "Installers/MainInstaller")]
public class MainInstaller : ScriptableObjectInstaller<MainInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<IApplicationDataModule>().To<ApplicationDataModule>().AsSingle();

        SignalBusInstaller.Install(Container);

        Container.DeclareSignal<LoadingCompleteSignal>();
        Container.DeclareSignal<TryStartGameSignal>();
        Container.DeclareSignal<TryExitFromGameSignal>();

        Container.Bind<LoadSceneCommand>().AsTransient();

        Container.BindSignal<LoadingCompleteSignal>().ToMethod<LoadSceneCommand>((x,s) => x.LoadScene(ScenesNames.MAIN_MENU_SCENE)).FromResolve();
        Container.BindSignal<TryStartGameSignal>().ToMethod<LoadSceneCommand>((x, s) => x.LoadScene(ScenesNames.GAME_SCENE)).FromResolve();
        Container.BindSignal<TryExitFromGameSignal>().ToMethod<LoadSceneCommand>((x, s) => x.LoadScene(ScenesNames.MAIN_MENU_SCENE)).FromResolve();

        LoadSceneCommand loadSceneCommand = new LoadSceneCommand();
        loadSceneCommand.LoadScene(ScenesNames.LOADING_SCENE);
    }
}