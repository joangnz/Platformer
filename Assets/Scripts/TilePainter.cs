using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilePainter : MonoBehaviour
{
    [SerializeField] private int minCols, maxCols, minRows, maxRows;
    private Tilemap Fg, Deco;
    private Tile TLCorner, TRCorner, BLCorner, BRCorner, TEdge, LEdge, REdge, BEdge, Fill, Ramp;
    private Dictionary<string, Tile> tileDict;
    private int lastCol, y, x, lastTileColumn;
    public void Init(Tilemap fg, Tilemap deco, Tile tlCorner, Tile trCorner, Tile blCorner, Tile brCorner, Tile tEdge, Tile lEdge, Tile rEdge, Tile bEdge, Tile fill, Tile ramp)
    {
        Fg = fg;
        Deco = deco;
        TLCorner = tlCorner;
        TRCorner = trCorner;
        BLCorner = blCorner;
        BRCorner = brCorner;
        TEdge = tEdge;
        LEdge = lEdge;
        REdge = rEdge;
        BEdge = bEdge;
        Fill = fill;
        Ramp = ramp;

        tileDict = new()
        {
            {"TLCorner", TLCorner },
            {"TRCorner", TRCorner },
            {"BLCorner", BLCorner },
            {"BRCorner", BRCorner },
            {"TEdge", TEdge },
            {"LEdge", LEdge },
            {"REdge", REdge },
            {"BEdge", BEdge },
            {"Fill", Fill },
            {"Ramp", Ramp }
        };

        lastTileColumn = 0;
    }

    private Vector2Int GetStartPos(float playerX)
    {
        Vector2 pos = new(playerX, 0);

        pos.x += 20;
        pos.y += Random.Range(0, 3);
        if (Random.value > 0.5f)
        {
            pos.y *= -1;
        }

        if (lastTileColumn <= pos.x) pos.x += 5;
        return Vector2Int.RoundToInt(pos);
    }

    public void RandomPaint(float playerX)
    {
        int rows = -Random.Range(minRows, maxRows);
        int cols = Random.Range(minCols, maxCols);
        Vector2Int startPos = GetStartPos(playerX);

        lastCol = cols-1;

        for (y = 0; y > rows; y--)
        {
            for (x = 0; x < cols; x++)
            {
                Vector2Int curPos = new(startPos.x + x, startPos.y + y);

                // First Row, First Column
                if (y==0 && x==0) PaintTileOrRamp("TLCorner", curPos, ref cols);

                // First Row, Last Column
                else if (y==0 && x==lastCol) PaintTile("TRCorner", curPos);

                // First Row, Mid Columns
                else if (y==0 && x<lastCol) PaintTileOrRamp("TEdge", curPos, ref cols);

                // Last Row, First Column
                else if (y==rows+1 && x==0) PaintTile("BLCorner", curPos);

                // Last Row, Last Column
                else if (y==rows+1 && x==lastCol) PaintTile("BRCorner", curPos);

                // Last Row, Mid Columns
                else if (y==rows+1 && x<lastCol) PaintTile("BEdge", curPos);

                // Mid Rows, First Column
                else if (y<0 && x==0) PaintTile("LEdge", curPos);

                // Mid Rows, Last Column
                else if (y<0 && x==lastCol) PaintTileOrRamp("REdge", curPos, ref cols);

                // Mid Rows, Mid Columns
                else if (y<0 && x<lastCol) PaintTile("Fill", curPos);
            }
        }
        lastTileColumn = x;
    }

    private void PaintTileOrRamp(string tName, Vector2Int pos, ref int cols)
    {
        if (Random.value > 0.15f) PaintTile(tName, pos);
        else
        {
            if (tName.Contains("Edge") || tName.Contains("Corner"))
            {
                if (tName.Contains("T")) PaintTile(tName, pos);
                else  PaintTile("Fill", pos);

                pos.x++;
                x++;
            }

            PaintTile("Ramp", pos);
            y--;
            lastCol = x;
            x = -1;
            cols++;
        }
    }

    private void PaintTile(string tName, Vector2Int pos)
    {
        Fg.SetTile((Vector3Int)pos, tileDict[tName]);
    }

    // Possible Function to paint decorations later
    private void PaintDeco(string tName, Vector2Int pos)
    {

    }

    public void DestroyTile(Vector2Int pos)
    {
        Debug.Log(pos);
        Fg.SetTile((Vector3Int)pos, null);
    }
}
