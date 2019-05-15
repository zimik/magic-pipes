using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IApplicationDataModule
{
    bool ApplicationDataHasLoaded
    {
        get;
        set;
    }

    string TestString
    {
        get;
        set;
    }


}

public class ApplicationDataModule : IApplicationDataModule
{
    public string TestString { get; set; }

    public bool ApplicationDataHasLoaded { get; set; }
}
