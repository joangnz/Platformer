using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

public class TilePainter : MonoBehaviour
{
    private Tilemap Fg, Deco;
    private Tile TLCorner, TRCorner, BLCorner, BRCorner, TEdge, LEdge, REdge, BEdge, Fill, Ramp;
    private Dictionary<string, Tile> tileDict;
    private int lastCol, y, x;
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
    }

    private Vector2Int GetStartPos(Vector2 playerPos)
    {
        Vector2 pos = playerPos;

        pos.x += 20;
        pos.y += Random.Range(2, 6);
        if (Random.value > 0.5f)
        {
            pos.y *= -1;
        }
        return Vector2Int.RoundToInt(pos);
    }

    public void RandomPaint(Vector2 playerPos)
    {
        int rows = -Random.Range(2, 11);
        int cols = Random.Range(4, 18);
        Vector2Int startPos = GetStartPos(playerPos);

        lastCol = cols-1;

        for (y = 0; y > rows; y--)
        {
            for (x = 0; x < cols; x++)
            {
                Vector2Int curPos = new(startPos.x + x, startPos.y + y);

                // First Row, First Column
                if (y==0 && x==0) PaintTileOrRamp("TLCorner", curPos);

                // First Row, Last Column
                else if (y==0 && x==lastCol) PaintTile("BRCorner", curPos);

                // First Row, Mid Columns
                else if (y==0 && x<lastCol) PaintTileOrRamp("TEdge", curPos);

                // Last Row, First Column
                else if (y==rows+1 && x==0) PaintTile("BLCorner", curPos);

                // Last Row, Last Column
                else if (y==rows+1 && x==lastCol) PaintTile("BRCorner", curPos);

                // Last Row, Mid Columns
                else if (y==rows+1 && x<lastCol) PaintTile("BEdge", curPos);

                // Mid Rows, First Column
                else if (y<0 && x==0) PaintTile("LEdge", curPos);

                // Mid Rows, Last Column
                else if (y<0 && x==lastCol) PaintTileOrRamp("REdge", curPos);

                // Mid Rows, Mid Columns
                else PaintTile("Fill", curPos);
            }
        }
    }

    private void PaintTileOrRamp(string tName, Vector2Int pos)
    {
        if (Random.value > 0.15f) PaintTile(tName, pos);
        else
        {
            PaintTile("Ramp", pos);
            y--;
            lastCol = x;
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
}
