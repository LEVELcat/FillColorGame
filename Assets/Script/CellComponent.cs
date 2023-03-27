using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Sprites;
using FillColorGame.GridComponents;
using System.Threading;
using FillColorGame.GridComponents;


public class CellComponent : MonoBehaviour, IPathFinderMarker 
{
    [SerializeField]
    SpriteRenderer spriteRenderer;



    private CancellationTokenSource cancellationTokenSource;

    public Color CurentColor { get; private set; }
    public MarkerType Marker { get ; set; }

    static float timeToChangeColor = 1f;

    // Start is called before the first frame update
    void Start()
    {
        if (spriteRenderer== null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetColor(Color color)
    {
        spriteRenderer.color = color;
        CurentColor = color;
    }

    public async void ChangeColorWithAnimationAsync(Color color)
    {
        if (cancellationTokenSource != null)
        {
            cancellationTokenSource.Cancel();

            while (cancellationTokenSource != null) await Task.Yield();
        }

        cancellationTokenSource = new CancellationTokenSource();

        CurentColor = color;

        Color startColor = spriteRenderer.color;

        for(float time = 0; time < timeToChangeColor && cancellationTokenSource.IsCancellationRequested == false; time += Time.deltaTime)
        {
            spriteRenderer.color = Color.Lerp(startColor, color, time);

            await Task.Yield();
        }

        if (cancellationTokenSource.IsCancellationRequested == false) 
            spriteRenderer.color = color;

        cancellationTokenSource.Dispose();

        cancellationTokenSource = null;
    }
}
