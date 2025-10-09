using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DepthSortByY : MonoBehaviour
{
    private Transform player;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        // Find player by tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("DynamicSortingLayer: No GameObject found with tag 'Player'");
        }
    }

    void Update()
    {
        if (LightManager.Instance == null) return;

        bool castShadow = LightManager.Instance.IsPlayerCastingShadow(player.transform.position);


        if (player == null) return;

        // If player is below object (visually in front)
        if (castShadow && (this.transform.position.y < player.transform.position.y))
            sr.sortingLayerName = "InfrontPlayer"; // player in front
        else
            sr.sortingLayerName = "BehindPlayer"; // player behind
    }
}
