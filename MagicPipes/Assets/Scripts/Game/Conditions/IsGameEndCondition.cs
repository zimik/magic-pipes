using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsGameEndCondition : SSMCondition
{
    public GameModel GameModel;

    public override bool IsFulFilled()
    {
        return GameModel.Lives <= 0;
    }

}
