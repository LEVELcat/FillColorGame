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
            Color[] localColor = new Color[ColorCount];
            Array.Copy(ColorCollection.Instance.colors, localColor, ColorCount);

            cellComponents = new CellComponent[Size.x, Size.y];

            WeightRandomClass<Color> weightRandom = new WeightRandomClass<Color>(localColor, Size.x * Size.y);

            for (int y = 0; y < Size.y; y++)
                for (int x = 0; x < Size.x; x++)
                {
                    GameObject go = Instantiate(cellPrefab, new Vector3((-Size.x / 2f) + (1 * x), (Size.y / 2f) - (1 * y), 0), Quaternion.identity, this.transform);

                    CellComponent cell = go.GetComponent<CellComponent>();

                    cellComponents[x, y] = cell;

                    Color rndColor = weightRandom.GetRandomValue();

                    cell.SetColor(rndColor);

                    weightRandom.ChangeValueWeight(rndColor, -1);
                }

            if (cellComponents[0, 0] != null)
            {
                startPoint = new Vector2Int(0, 0);

                GameObject go = Instantiate(startPointPrefab, new Vector3((-Size.x / 2f) + (1 * 0), (Size.y / 2f) - (1 * 0), -1), Quaternion.identity, this.transform);
            }
        }

        public void ChangeColor(Color color)
        {
            cellComponents[startPoint.x, startPoint.y].ChangeColorWithAnimationAsync(color);
        }


    }
    public interface PathFinderMarker
    {
        bool Marker { get; set; }
    }
}

