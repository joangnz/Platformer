using UnityEngine;
using UnityEngine.Tilemaps;

public class NightSlash : MonoBehaviour
{
    private CircleCollider2D circleCol;

    private Player p;
    private TilePainter tp;

    private readonly float baseRad = 0.7f;

    void Start()
    {
        circleCol = GetComponent<CircleCollider2D>();

        p = GetComponentInParent<Player>();
        tp = p.GetTilePainter();
    }

    private void OnTriggerEnter2D(Collider2D hit)
    {
        Tilemap tilemap = hit.GetComponent<Tilemap>();
        if (tilemap == null) return;

        Vector3 center = p.transform.position;
        float radius = baseRad * Mathf.Max(p.transform.localScale.x, p.transform.localScale.y);

        Vector3Int min = tilemap.WorldToCell(center - new Vector3(radius, radius));
        Vector3Int max = tilemap.WorldToCell(center + new Vector3(radius, radius));

        for (int x = min.x; x <= max.x; x++)
        {
            for (int y = min.y; y <= max.y; y++)
            {
                Vector3Int cellPos = new Vector3Int(x, y, 0);
                Vector3 worldPos = tilemap.CellToWorld(cellPos) + tilemap.tileAnchor;

                if (Vector2.Distance(worldPos, center) <= radius)
                {
                    tp.DestroyTile(new Vector2Int(cellPos.x, cellPos.y));
                }
            }
        }
    }
}