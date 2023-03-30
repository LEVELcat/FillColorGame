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
        Vector2Int startPoint;

        Vector2Int size;

        [SerializeField]
        GameObject cellPrefab;

        [SerializeField]
        GameObject startPointPrefab;
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
            size = Size;
            Color[] localColor = new Color[ColorCount];
            Array.Copy(ColorCollection.Instance.colors, localColor, ColorCount);

            cellComponents = new CellComponent[size.x, size.y];

            WeightRandomClass<Color> weightRandom = new WeightRandomClass<Color>(localColor, size.x * size.y);

            for (int y = 0; y < size.y; y++)
                for (int x = 0; x < size.x; x++)
                {
                    GameObject go = Instantiate(cellPrefab, new Vector3((-size.x / 2f) + (1 * x), (size.y / 2f) - (1 * y), 0), Quaternion.identity, this.transform);

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

            ChangeColor(cellComponents[startPoint.x, startPoint.y].CurentColor);
        }

        public void ChangeColor(Color color)
        {
            Vector2Int[] direction = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };

            RecursiveCellAlgoritm(startPoint);
            void RecursiveCellAlgoritm(Vector2Int position)
            {
                CellComponent currentCell = cellComponents[position.x, position.y];

                currentCell.Marker |= MarkerType.PathFinderMarker;

                if (currentCell.CurentColor == color) 
                    currentCell.Marker |= MarkerType.ColorChangingMarker;

                else
                {
                    if (currentCell.Marker.HasFlag(MarkerType.ColorChangingMarker) == false)
                        return;
                }

                foreach(var vec in direction)
                {
                    Vector2Int nextVector = position + vec;
                    if (nextVector.x >= 0 && nextVector.x < size.x && nextVector.y >= 0 && nextVector.y < size.y)
                        if (cellComponents[nextVector.x, nextVector.y].Marker.HasFlag(MarkerType.PathFinderMarker) == false)
                            RecursiveCellAlgoritm(nextVector);
                }
            }

            foreach(var cell in cellComponents)
            {
                if(cell.Marker.HasFlag(MarkerType.ColorChangingMarker) == true)
                    cell.ChangeColorWithAnimationAsync(color);

                cell.Marker &= ~MarkerType.PathFinderMarker;
            }
        }
    }

    public interface IPathFinderMarker
    {
        MarkerType Marker { get; set; }
    }

    [Flags]
    public enum MarkerType : byte
    {
        None = 0,
        PathFinderMarker = 1,
        ColorChangingMarker = 2
    }
}

