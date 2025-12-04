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
        if (Player.transform.position.x % 10 == 0){
            tp.RandomPaint(Player.transform.position);
        }
    }
}
