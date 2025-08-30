using System.Collections.Generic;
using UnityEngine;

/// Oyundaki blokların oluşturulmasını ve hareketini yönetir
public class BlockSpawner : MonoBehaviour
{
    [Header("Oluşturma Ayarları")]
    [SerializeField] private int blocksPerRow = 7;
    [SerializeField] private Block blockPrefab;
    [SerializeField] private float blockSpacing = 4f;
    [SerializeField] private float spawnChance = 0.5f;

    [Header("Zorluk Ayarları")]
    [SerializeField] private int minHitPoints = 1;
    [SerializeField] private int maxHitPoints = 3;
    [SerializeField] private int hitPointsIncreasePerRow = 1;

    private int currentRowNumber = 0;
    private List<Block> spawnedBlocks = new List<Block>();

    private void OnEnable()
    {
        SpawnInitialBlocks();
    }

    /// İlk blok satırını oluşturur
    private void SpawnInitialBlocks()
    {
        SpawnNewRow();
    }

    /// Yeni bir blok satırı oluşturur
    public void SpawnBlocks()
    {
        MoveExistingBlocksDown();
        SpawnNewRow();
    }

    /// Verilen indeksteki blok için konumu hesaplar
    private Vector3 CalculateBlockPosition(int blockIndex)
    {
        return transform.position + Vector3.right * blockIndex * blockSpacing;
    }

    /// Yeni bir blok satırı oluşturur
    private void SpawnNewRow()
    {
        for (int i = 0; i < blocksPerRow; i++)
        {
            if (ShouldSpawnBlock())
            {
                SpawnSingleBlock(i);
            }
        }

        currentRowNumber++;
    }

    /// Oluşturma şansına göre blok oluşturulup oluşturulmayacağını belirler
    private bool ShouldSpawnBlock()
    {
        return Random.value < spawnChance;
    }

    /// Belirtilen indekste tek bir blok oluşturur
    private void SpawnSingleBlock(int blockIndex)
    {
        Vector3 spawnPosition = CalculateBlockPosition(blockIndex);
        Block newBlock = Instantiate(blockPrefab, spawnPosition, Quaternion.identity);

        int hitPoints = CalculateHitPoints();
        newBlock.SetHitPoints(hitPoints);

        spawnedBlocks.Add(newBlock);
    }

    /// Mevcut satıra göre yeni blok için vuruş noktalarını hesaplar
    private int CalculateHitPoints()
    {
        int baseHitPoints = Random.Range(minHitPoints, maxHitPoints + 1);
        return baseHitPoints + currentRowNumber * hitPointsIncreasePerRow;
    }

    /// Tüm mevcut blokları bir satır aşağı taşır
    private void MoveExistingBlocksDown()
    {
        List<Block> activeBlocks = new List<Block>();

        foreach (Block block in spawnedBlocks)
        {
            if (block != null)
            {
                MoveBlockDown(block);
                activeBlocks.Add(block);
            }
        }

        spawnedBlocks = activeBlocks;
    }

    /// Tek bir bloğu bir satır aşağı taşır
    private void MoveBlockDown(Block block)
    {
        Vector3 newPosition = block.transform.position + Vector3.down * blockSpacing;
        block.transform.position = newPosition;
    }
}