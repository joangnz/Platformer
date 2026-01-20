using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player playerPrefab;
    [SerializeField] private Camera Cam;
    [SerializeField] private BoxCollider2D bottom;
    [SerializeField] private Tilemap foreground, deco;
    [SerializeField] private Tile TLCorner, TRCorner, BLCorner, BRCorner, TEdge, LEdge, REdge, BEdge, Fill, Ramp;
    private SpriteRenderer background;
    private TilePainter tp;
    private List<int> spawnedPositions = new();

    private Player Player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tp = GetComponent<TilePainter>();
        tp.Init(foreground, deco, TLCorner, TRCorner, BLCorner, BRCorner, TEdge, LEdge, REdge, BEdge, Fill, Ramp);

        background = GetComponent<SpriteRenderer>();

        Player = Instantiate(playerPrefab);
        Player.Init(tp, Cam);
        Player.name = "Player";
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pp = Player.transform.position;
        bottom.transform.position = new(pp.x, bottom.transform.position.y, bottom.transform.position.z);
        Cam.transform.position = new(pp.x, pp.y +1, Cam.transform.position.z);

        background.transform.position = new(pp.x, pp.y/2, 0);

        Vector2Int playerPos = Vector2Int.RoundToInt(pp);

        if (playerPos.x % 10 == 0 && !spawnedPositions.Contains(playerPos.x))
        {
            tp.RandomPaint(pp.x);
            spawnedPositions.Add(playerPos.x);
        }
    }
}
