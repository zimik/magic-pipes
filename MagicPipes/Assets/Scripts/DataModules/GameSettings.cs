using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameSettings
{
    int Lives{get;set;}
}

public class GameSettings : IGameSettings
{
    public int Lives{get;set;}
}
