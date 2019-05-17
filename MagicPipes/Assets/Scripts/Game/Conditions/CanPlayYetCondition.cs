public class CanPlayYetCondition : SSMCondition {

    public GameModel GameModel;

    public override bool IsFulFilled()
    {
        return GameModel.Lives > 0;
    }

}
