using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Connect  {

    public enum ConnectType
    {
        Top,
        Right,
        Bottom,
        Left
    }

    public enum ConnectFlowDirection
    {
        Empty,
        In,
        Out
    }

    public Pipe ToPipe;
    public bool IsActive;
    public ConnectType Type;
    public ConnectType OppositeType;
    public ConnectFlowDirection DefaultFlowDirection;
}
