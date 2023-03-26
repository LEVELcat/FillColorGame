using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;

public class CellComponent : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    public Color CellColor
    {
        get
        {
            return spriteRenderer.color;
        }
    }

    static float timeToChangeColor = 1f;

    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetColor(Color color)
    {
        spriteRenderer.color = color;
    }


}
