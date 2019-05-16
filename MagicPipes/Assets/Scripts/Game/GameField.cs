using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameField : MonoBehaviour {

    public Vector2Int GameFieldSize;

    public Vector2Int EnterPipePosition;

    public Vector2Int ExitPipePosition;

    public Pipe EnterPipeTemplateForGenerate;
    public Pipe ExitPipeTemplateForGenerate;

    public List<Pipe> PipesTemplatesForGenerate;

    public Transform PipesContainer;

    public UnityEvent GeneratePipesFinished;

    private List<Pipe> _pipes;
    private Pipe _enterPipe;
    private Pipe _exitPipe;

    // Use this for initialization
    void Awake () {
        _pipes = new List<Pipe>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GenetatePipes()
    {
        Quaternion quaternion = new Quaternion();
        Vector2Int vector2Int = new Vector2Int();

        for (int y = 0; y < GameFieldSize.y; y++)
        {
            for (int x = 0; x < GameFieldSize.x; x++)
            {
                Pipe pipeTemplate;
                if (EnterPipePosition.x == x && EnterPipePosition.y == y)
                {
                    pipeTemplate = EnterPipeTemplateForGenerate;
                }
                else if(ExitPipePosition.x == x && ExitPipePosition.y == y)
                {
                    pipeTemplate = ExitPipeTemplateForGenerate;
                }
                else
                {
                    pipeTemplate = PipesTemplatesForGenerate[Random.Range(0, PipesTemplatesForGenerate.Count)];
                }

                Pipe pipe = Instantiate(pipeTemplate, new Vector3(x - GameFieldSize.x/2, y - GameFieldSize.y / 2, 0),quaternion,PipesContainer);

                vector2Int.x = x;
                vector2Int.y = y;
                AddPipe(vector2Int, pipe);
            }
        }

        for (int y = 0; y < GameFieldSize.y; y++)
        {
            for (int x = 0; x < GameFieldSize.x; x++)
            {
                vector2Int.x = x;
                vector2Int.y = y;

                Pipe pipe = _pipes[From2to1(vector2Int)];

                pipe.SetNeighboringPipes(
                        GetNeighboringPipeTo(vector2Int, Vector2Int.up),
                        GetNeighboringPipeTo(vector2Int, Vector2Int.right),
                        GetNeighboringPipeTo(vector2Int, Vector2Int.down),
                        GetNeighboringPipeTo(vector2Int, Vector2Int.left)
                    );
            }
        }

        _enterPipe = _pipes[From2to1(EnterPipePosition)];
        _exitPipe = _pipes[From2to1(ExitPipePosition)];

        GeneratePipesFinished.Invoke();
    }

    private Pipe GetNeighboringPipeTo(Vector2Int pipePos, Vector2Int shift)
    {
        Vector2Int pos = pipePos + shift;

        if (pos.x >= 0 && pos.x < GameFieldSize.x && pos.y >= 0 && pos.y < GameFieldSize.y)
        {
            return _pipes[From2to1(pos)];
        }

        return null;
    }

    public void StartGame()
    {
        _enterPipe.StartProcessing();
    }

    private void AddPipe(Vector2Int position, Pipe pipe)
    {
        int pos = From2to1(position);
        for (int i = _pipes.Count; i<=pos; i++)
        {
            _pipes.Add(null);
        }

        _pipes[pos] = pipe;
    }

    private int From2to1(Vector2Int from)
    {
        return from.y * GameFieldSize.x + from.x;
    }
}
