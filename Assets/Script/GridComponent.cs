using Assets.Script;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FillColorGame.GridComponents
{
    public class GridComponent : MonoBehaviour
    {
        CellComponent[,] cellComponents;
        DOD_OptimizedCellComponent optimizedCellComponent;

        Vector2Int startPoint;

        Vector2Int size;

        [SerializeField]
        GameObject cellPrefab;

        [SerializeField]
        GameObject startPointPrefab;

        [SerializeField]
        Paradigme paradigmeMode;

        enum Paradigme : byte
        {
            None = 0,
            OOP = 1,
            DOD = 2
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void GenerateGame(Vector2Int Size, int ColorCount)
        {
            if (paradigmeMode == Paradigme.OOP)
            {
                OOP_GenerateGame(Size, ColorCount);
                ChangeColor(cellComponents[startPoint.x, startPoint.y].CurentColor);
            }
                
            else
            {
                DOD_GenerateGame(Size, ColorCount);
                ChangeColor(optimizedCellComponent.CurentColors[startPoint.x, startPoint.y]);
            }
                

            
        }

        private void OOP_GenerateGame(Vector2Int Size, int ColorCount)
        {
            size = Size;
            Color[] localColor = new Color[ColorCount];
            Array.Copy(ColorCollection.Instance.colors, localColor, ColorCount);

            cellComponents = new CellComponent[size.x, size.y];


            WeightRandomClass<Color> weightRandom = new WeightRandomClass<Color>(localColor, size.x * size.y);

            for (int y = 0; y < size.y; y++)
                for (int x = 0; x < size.x; x++)
                {
                    GameObject go = Instantiate(cellPrefab, new Vector3((-size.x / 2f) + (1 * x), (size.y / 2f) - (1 * y), 0), Quaternion.identity, this.transform);

                    go.AddComponent<CellComponent>();

                    CellComponent cell = go.GetComponent<CellComponent>();

                    cellComponents[x, y] = cell;

                    Color rndColor = weightRandom.GetRandomValue();

                    cell.SetColor(rndColor);

                    weightRandom.ChangeValueWeight(rndColor, -1);
                }

            if (cellComponents[0, 0] != null)
            {
                startPoint = new Vector2Int(0, 0);

                GameObject go = Instantiate(startPointPrefab, new Vector3((-size.x / 2f) + (1 * 0), (size.y / 2f) - (1 * 0), -1), Quaternion.identity, this.transform);
            }

            cellComponents[startPoint.x, startPoint.y].Marker |= MarkerType.ColorChangingMarker;
        }

        private void DOD_GenerateGame(Vector2Int Size, int ColorCount)
        {
            size = Size;
            Color[] localColor = new Color[ColorCount];
            Array.Copy(ColorCollection.Instance.colors, localColor, ColorCount);

            SpriteRenderer[,] spriteRenderers = new SpriteRenderer[size.x, size.y];

            for(int y = 0; y < size.y; y++)
                for(int x = 0; x < size.x; x++)
                {
                    spriteRenderers[x,y] = Instantiate(cellPrefab, new Vector3((-size.x / 2f) + (1 * x), (size.y / 2f) - (1 * y), 0), Quaternion.identity, this.transform)
                        .GetComponent<SpriteRenderer>();
                }

            optimizedCellComponent = new DOD_OptimizedCellComponent(spriteRenderers);

            WeightRandomClass<Color> weightRandom = new WeightRandomClass<Color>(localColor, size.x * size.y);
            {
                Vector2Int id = new Vector2Int();
                for (id.y = 0; id.y < size.y; id.y++)
                    for (id.x = 0; id.x < size.x; id.x++)
                    {
                        Color rndColor = weightRandom.GetRandomValue();
                        weightRandom.ChangeValueWeight(rndColor, -1);
                        optimizedCellComponent.SetColor(id, rndColor);
                    }
            }

            startPoint = new Vector2Int(0, 0);
            GameObject go = Instantiate(startPointPrefab, new Vector3((-size.x / 2f) + (1 * 0), (size.y / 2f) - (1 * 0), -1), Quaternion.identity, this.transform);

            optimizedCellComponent.Markers[startPoint.x, startPoint.y] |= MarkerType.ColorChangingMarker;
        }

        public void ChangeColor(Color color)
        {
            Vector2Int[] direction = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };

            if (paradigmeMode == Paradigme.OOP)
            {
                RecursiveCellAlgoritm_OOPmode(startPoint);


                foreach (var cell in cellComponents)
                {
                    if (cell.Marker.HasFlag(MarkerType.ColorChangingMarker) == true)
                        cell.ChangeColorWithAnimationAsync(color);

                    cell.Marker &= ~MarkerType.PathFinderMarker;
                }
            }
            else if (paradigmeMode == Paradigme.DOD)
            {
                RecursiveCellAlgoritm_DODmode(startPoint);

                {
                    Vector2Int Id = new Vector2Int(0, 0);
                    for (Id.y = 0 ; Id.y < size.y; Id.y++)
                        for (Id.x = 0; Id.x < size.x; Id.x++)
                        {
                            if (optimizedCellComponent.Markers[Id.x, Id.y].HasFlag(MarkerType.ColorChangingMarker) == true)
                                optimizedCellComponent.ChangeColorWithAnimationAsync(Id, color);

                            optimizedCellComponent.Markers[Id.x, Id.y] &= ~MarkerType.PathFinderMarker;
                        }
                }
            }

            void RecursiveCellAlgoritm_OOPmode(Vector2Int position)
            {
                CellComponent currentCell = cellComponents[position.x, position.y];

                currentCell.Marker |= MarkerType.PathFinderMarker;

                if (currentCell.CurentColor == color) 
                    currentCell.Marker |= MarkerType.ColorChangingMarker;

                else if (currentCell.Marker.HasFlag(MarkerType.ColorChangingMarker) == false)
                        return;

                foreach(var vec in direction)
                {
                    Vector2Int nextPosition = position + vec;
                    if (nextPosition.x >= 0 && nextPosition.x < size.x && nextPosition.y >= 0 && nextPosition.y < size.y)
                        if (cellComponents[nextPosition.x, nextPosition.y].Marker.HasFlag(MarkerType.PathFinderMarker) == false)
                            RecursiveCellAlgoritm_OOPmode(nextPosition);
                }
            }
            void RecursiveCellAlgoritm_DODmode(Vector2Int position)
            {
                optimizedCellComponent.Markers[position.x, position.y] |= MarkerType.PathFinderMarker;

                if (optimizedCellComponent.CurentColors[position.x, position.y] == color)
                    optimizedCellComponent.Markers[position.x, position.y] |= MarkerType.ColorChangingMarker;

                else if (optimizedCellComponent.Markers[position.x, position.y].HasFlag(MarkerType.ColorChangingMarker) == false)
                        return;

                foreach(var vec in direction)
                {
                    Vector2Int nextPosition = position + vec;
                    if (nextPosition.x >= 0 && nextPosition.x < size.x && nextPosition.y >= 0 && nextPosition.y < size.y)
                        if (optimizedCellComponent.Markers[nextPosition.x, nextPosition.y].HasFlag(MarkerType.PathFinderMarker) == false)
                            RecursiveCellAlgoritm_DODmode(nextPosition);
                }
            }
        }
    }
}

