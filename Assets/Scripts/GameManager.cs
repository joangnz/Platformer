using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player playerPrefab;
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private Camera Cam;
    [SerializeField] private BoxCollider2D bottom;
    [SerializeField] private Tilemap foreground, deco;
    [SerializeField] private Tile TLCorner, TRCorner, BLCorner, BRCorner, TEdge, LEdge, REdge, BEdge, Fill, Ramp;
    private SpriteRenderer background;
    private TilePainter tp;
    private List<int> islandSpawnedPositions = new();

    private Player Player;
    void Start()
    {
        tp = GetComponent<TilePainter>();
        tp.Init(foreground, deco, TLCorner, TRCorner, BLCorner, BRCorner, TEdge, LEdge, REdge, BEdge, Fill, Ramp);

        background = GetComponent<SpriteRenderer>();

        Player = Instantiate(playerPrefab);
        Player.Init(tp, Cam);
        Player.name = "Player";
    }

    void Update()
    {
        Vector3 pp = Player.transform.position;
        bottom.transform.position = new(pp.x, bottom.transform.position.y, bottom.transform.position.z);
        Cam.transform.position = new(pp.x, pp.y +1, Cam.transform.position.z);

        background.transform.position = new(pp.x, pp.y/2, 0);

        Vector2Int playerPos = Vector2Int.RoundToInt(pp);

        if (playerPos.x % 10 == 0)
        {
            if (!islandSpawnedPositions.Contains(playerPos.x))
            {
                tp.RandomPaint(pp.x);
                islandSpawnedPositions.Add(playerPos.x);

                SpawnEnemy(pp);
            }
        }
    }

    private void SpawnEnemy(Vector3 pp)
    {
        Vector3 enemySpawnPos = new(pp.x + 8f, Enemy.StartY, 0);
        Enemy enemy = Instantiate(enemyPrefab, enemySpawnPos, Quaternion.identity);
        enemy.name = "Enemy";
    }
}
