using System.Collections.Generic;
using UnityEngine;
namespace ProcGen
{
    public class GridBuilder : MonoBehaviour
    {
        public enum CellType
        {
            Empty = 0,
            Corridor = 1,
            SpecialRoom
        }
        public class GridCell
        {
            public CellType value;
            public GridCell(CellType value)
            {
                this.value = value;
            }
        }
        public int gridLength = 10, gridWidth = 10;
        public GridCell[,] grid;
        public int padding = 2;
        public int specialRoomCount = 3;
        public int maxIterations = 100;
        public bool buildGrid;

        readonly Dictionary<CellType, Color> colors = new() { { CellType.Empty, Color.gray1 },
            { CellType.Corridor, Color.white }, { CellType.SpecialRoom, Color.red } };
        float chanceToShift = 1;
        private void OnEnable()
        {
            BuildGrid();
        }
        private void Update()
        {
            if (buildGrid)
            {
                buildGrid = false;
                BuildGrid();
            }
        }
        public void BuildGrid()
        {
            grid = new GridCell[gridLength, gridWidth];
            for (int i = 0; i < gridLength; i++)
            {
                for (int j = 0; j < gridWidth; j++) grid[i, j] = new(CellType.Empty);
            }

            #region Build corridors
            int mainCorridor = UnityEngine.Random.Range(padding + 1, gridLength - padding - 1);
            bool shifted = false;
            int start = padding + 1;
            int end = gridLength - padding - 1;
            for (int j = start; j < end; j++)
            {
                grid[mainCorridor, j].value = CellType.Corridor;
                if (!shifted)
                {
                    var value = UnityEngine.Random.Range(0.0f, 1.0f);
                    if (value < chanceToShift)
                    {
                        shifted = true;
                        mainCorridor += UnityEngine.Random.Range(0, 2) * 2 - 1;
                        grid[mainCorridor, j].value = CellType.Corridor;
                        continue;
                    }
                }
            }
            #endregion

            bool ValidSpecialRoom(int i, int j)
            {
                if (grid[i, j].value != 0) return false;
                if (grid[i + 1, j].value != CellType.Corridor &&
                    grid[i - 1, j].value != CellType.Corridor &&
                    grid[i, j + 1].value != CellType.Corridor &&
                    grid[i, j - 1].value != CellType.Corridor) return false;
                return true;
            }
        }
        private void OnDrawGizmos()
        {
            if (grid == null) return;
            var values = (CellType[])System.Enum.GetValues(typeof(CellType));
            foreach (var value in values)
            {
                Gizmos.color = colors[value];
                for (int i = 0; i < gridLength; i++)
                {
                    for (int j = 0; j < gridWidth; j++)
                    {
                        if (grid[i, j].value == value)
                        {
                            var pos = new Vector3(i, 0, j);
                            Gizmos.DrawWireCube(pos, Vector3.one);
                        }
                    }
                }

            }
        }
    }
}