public interface IApplicationDataModule
{
    bool ApplicationDataHasLoaded
    {
        get;
        set;
    }

}

public class ApplicationDataModule : IApplicationDataModule
{
    public bool ApplicationDataHasLoaded { get; set; }
}
