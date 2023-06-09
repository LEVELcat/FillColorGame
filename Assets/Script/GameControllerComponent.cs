using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FillColorGame.GridComponents;

public class GameControllerComponent : MonoBehaviour
{
    public static GameControllerComponent Instance;

    public UIControllerComponent uIControllerComponent;

    public GridComponent gridComponent;

    [SerializeField]
    private ColorCollection ColorCollection;

    Camera mainCamera;


    public Vector2Int minGameSize = new Vector2Int(10,10);
    public Vector2Int maxGameSize = new Vector2Int(40,40);

    public int minimalColorCount = 4;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        mainCamera= Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateRandomGame()
    {
        Vector2Int size = new Vector2Int(Random.Range(minGameSize.x, maxGameSize.x), Random.Range(minGameSize.y, maxGameSize.y));

        int ColorCount = Random.Range(4, ColorCollection.Instance.colors.Length);

        mainCamera.orthographicSize = size.y / 2f + 1f;

        GenerateGame(size, ColorCount);
    }

    public void GenerateGame(Vector2Int Size, int ColorCount)
    {
        if (Size.x > Size.y)
        {
            mainCamera.orthographicSize = Size.x / 2f + 1f;
        }
        else
        {
            mainCamera.orthographicSize = Size.y / 2f + 1f;
        }
        gridComponent.GenerateGame(Size, ColorCount);
    }

    public void ChangeColor(Color color)
    {
        //Debug.Log(color.ToString());

        gridComponent.ChangeColor(color);
    }
}
