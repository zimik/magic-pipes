public class EnterPipe : Pipe {

    protected override bool IsEnebledForRotateState(PipeStates state)
    {
        return state == PipeStates.Waiting || state == PipeStates.Processing;
    }
}
