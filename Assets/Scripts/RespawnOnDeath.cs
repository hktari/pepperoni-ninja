using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class RespawnOnDeath : MonoBehaviour {

    public PlatformerCharacter2D character;

    public int storeHistory = 40;
    Queue<Vector2> validPositions = new Queue<Vector2>();

    private void Awake()
    {
        if (character == null) character = GetComponent<PlatformerCharacter2D>();
    }

    private void Update()
    {
        if (character.IsGrounded)
        {
            validPositions.Enqueue(character.transform.position);
            if (validPositions.Count > storeHistory)
            {
                validPositions.Dequeue();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "FallDownToDeath")
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        Debug.Log("Respawned");
        transform.position = validPositions.Peek();
    }
}
