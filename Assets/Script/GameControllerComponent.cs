using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerComponent : MonoBehaviour
{
    public UIControllerComponent uIControllerComponent;

    public GridComponent gridComponent;

    Camera mainCamera;


    public Vector2Int minGameSize = new Vector2Int(10,10);
    public Vector2Int maxGameSize = new Vector2Int(40,40);

    public int minimalColorCount = 4;

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
        if (size.x > size.y)
        {
            int buffer = size.x;

            size.x = size.x;
            size.y = buffer;
        }

        int ColorCount = Random.Range(4, ColorCollection.Instance.colors.Length);

        mainCamera.orthographicSize = size.y / 2f + 1f;

        GenerateGame(size, ColorCount);
    }

    public void GenerateGame(Vector2Int Size, int ColorCount)
    {
        gridComponent.GenerateGame(Size, ColorCount);
    }
}
