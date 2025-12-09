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
    private TilePainter tp;
    private List<int> spawnedPositions = new();

    private Player Player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tp = GetComponent<TilePainter>();
        tp.Init(foreground, deco, TLCorner, TRCorner, BLCorner, BRCorner, TEdge, LEdge, REdge, BEdge, Fill, Ramp);
        Player = Instantiate(playerPrefab);
        Player.name = "Player";
    }

    // Update is called once per frame
    void Update()
    {
        bottom.transform.position = new(Player.transform.position.x, bottom.transform.position.y, bottom.transform.position.z);
        Cam.transform.position = new(Player.transform.position.x, Player.transform.position.y +1, Cam.transform.position.z);

        Vector2Int playerPos = Vector2Int.RoundToInt(Player.transform.position);

        if (playerPos.x % 10 == 0 && !spawnedPositions.Contains(playerPos.x))
        {
            tp.RandomPaint(Player.transform.position.x);
            spawnedPositions.Add(playerPos.x);
        }
    }
}
