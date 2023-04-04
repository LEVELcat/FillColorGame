using FillColorGame.GridComponents;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class DOD_OptimizedCellComponent
{
    private SpriteRenderer[,] spriteRenderers = null;

    private CancellationTokenSource[,] cancellationTokenSources = null;

    public Color[,] CurentColors { get; private set; }

    public MarkerType[,] Markers { get; set; }

    static readonly float timeToChangeColr = 1f;

    public DOD_OptimizedCellComponent(SpriteRenderer[,] SpriteRenderers)
    {
        this.spriteRenderers = SpriteRenderers;
        cancellationTokenSources = new CancellationTokenSource[SpriteRenderers.GetLength(0), SpriteRenderers.GetLength(1)];
        CurentColors = new Color[SpriteRenderers.GetLength(0), SpriteRenderers.GetLength(1)];
        Markers = new MarkerType[SpriteRenderers.GetLength(0), SpriteRenderers.GetLength(1)];

        for(int index_Y = 0; index_Y < SpriteRenderers.GetLength(1); index_Y++)
            for(int index_X = 0; index_X < SpriteRenderers.GetLength(0); index_X++)
            {
                CurentColors[index_X, index_Y] = SpriteRenderers[index_X, index_Y].color;
                Markers[index_X, index_Y] = MarkerType.None;
            }
    }

    public void SetColor(Vector2Int Id, Color color)
    {
        spriteRenderers[Id.x, Id.y].color = color;
        CurentColors[Id.x, Id.y] = color;
    }

    public async void ChangeColorWithAnimationAsync(Vector2Int Id, Color color)
    {
        if (cancellationTokenSources[Id.x, Id.y] != null)
        {
            cancellationTokenSources[Id.x, Id.y].Cancel();
            while (cancellationTokenSources[Id.x, Id.y] != null) 
                await Task.Yield();
        }

        cancellationTokenSources[Id.x, Id.y] = new CancellationTokenSource();

        CurentColors[Id.x, Id.y] = color;

        Color startColor = spriteRenderers[Id.x, Id.y].color;

        for (float time = 0; time < timeToChangeColr && cancellationTokenSources[Id.x, Id.y].IsCancellationRequested == false; time += Time.deltaTime)
        {
            spriteRenderers[Id.x, Id.y].color = Color.Lerp(startColor, color, time);

            await Task.Yield();
        }

        if (cancellationTokenSources[Id.x, Id.y].IsCancellationRequested == false)
            spriteRenderers[Id.x, Id.y].color = color;

        cancellationTokenSources[Id.x, Id.y].Dispose();
        cancellationTokenSources[Id.x, Id.y] = null;
    }
}
