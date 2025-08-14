using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private GameObject player;

    public int chunkSize = 32; // width/height of each generated area
    public float scale = 1.0f;
    public float heightMultiplier = 20f;

    public float xOrg;
    public float yOrg;

    private HashSet<Vector3Int> generatedPositions = new HashSet<Vector3Int>();

    void Update()
    {
        GenerateAroundPlayer();
    }

    void GenerateAroundPlayer()
    {
        int playerX = Mathf.FloorToInt(player.transform.position.x);
        int playerZ = Mathf.FloorToInt(player.transform.position.z);

        int startX = playerX - chunkSize / 2;
        int endX = playerX + chunkSize / 2;
        int startZ = playerZ - chunkSize / 2;
        int endZ = playerZ + chunkSize / 2;

        for (int z = startZ; z < endZ; z++)
        {
            for (int x = startX; x < endX; x++)
            {
                float xCoord = xOrg + (float)x / chunkSize * scale;
                float zCoord = yOrg + (float)z / chunkSize * scale;

                float sample = Mathf.PerlinNoise(xCoord, zCoord);
                int ySample = Mathf.RoundToInt(sample * heightMultiplier);

                Vector3Int pos = new Vector3Int(x, ySample, z);

                if (!generatedPositions.Contains(pos))
                {
                    Instantiate(cubePrefab, pos, Quaternion.identity);
                    generatedPositions.Add(pos);
                }
            }
        }
    }
}