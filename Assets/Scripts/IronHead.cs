using UnityEngine;
using UnityEngine.Tilemaps;

public class IronHead : MonoBehaviour
{
    private Player p;
    private TilePainter tp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        p = GetComponentInParent<Player>();
        tp = p.GetTilePainter();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D hit)
    {
        Tilemap tilemap = hit.gameObject.GetComponent<Tilemap>();
        if (tilemap == null) return;
        Vector3Int cell = tilemap.WorldToCell(new(p.transform.position.x, p.transform.position.y + 1, p.transform.position.z));
        Vector3 hitPos = tilemap.CellToWorld(cell);
        tp.DestroyTile(Vector2Int.FloorToInt(hitPos));
    }
}
