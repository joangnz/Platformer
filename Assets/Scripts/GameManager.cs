using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player playerPrefab;
    public Player Player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player = Instantiate(playerPrefab);
        Player.name = "Player";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
