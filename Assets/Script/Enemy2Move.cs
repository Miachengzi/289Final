using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Enemy2Move : MonoBehaviour
{
    public Tilemap movableTilemap;
    public GameObject miniEnemyPrefab;
    public float spawnInterval = 2f;

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnMiniEnemy();
        }
    }

    void SpawnMiniEnemy()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        Vector3Int enemyCell = movableTilemap.WorldToCell(transform.position);

        bool playerInRange = false;
        foreach (GameObject player in players)
        {
            Vector3Int playerCell = movableTilemap.WorldToCell(player.transform.position);
            int distance = Mathf.Abs(playerCell.x - enemyCell.x) + Mathf.Abs(playerCell.y - enemyCell.y);

            if (distance <= 10)
            {
                playerInRange = true;
                break;
            }
        }

        if (!playerInRange)
        {
            return;
        }


        List<Vector3Int> validPositions = new List<Vector3Int>();

        for (int x = -3; x <= 3; x++)
        {
            for (int y = -3; y <= 3; y++)
            {
                if (x == 0 && y == 0) continue;

                Vector3Int offsetCell = enemyCell + new Vector3Int(x, y, 0);

                if (movableTilemap.HasTile(offsetCell))
                {
                    Vector3 worldPos = movableTilemap.CellToWorld(offsetCell) + new Vector3(0.5f, 0.5f, 0);

                    Collider2D[] hits = Physics2D.OverlapCircleAll(worldPos, 0.4f);

                    bool blocked = false;
                    foreach (Collider2D hit in hits)
                    {
                        if (hit.CompareTag("box") || hit.CompareTag("door") || hit.CompareTag("wave"))
                        {
                            blocked = true;
                            break;
                        }
                    }

                    if (!blocked)
                    {
                        validPositions.Add(offsetCell);
                    }
                }
            }
        }

        if (validPositions.Count > 0)
        {
            Vector3Int chosenCell = validPositions[Random.Range(0, validPositions.Count)];
            Vector3 spawnPos = movableTilemap.CellToWorld(chosenCell) + new Vector3(0.5f, 0.5f, 0);
            Instantiate(miniEnemyPrefab, spawnPos, Quaternion.identity);
        }
    }
}
