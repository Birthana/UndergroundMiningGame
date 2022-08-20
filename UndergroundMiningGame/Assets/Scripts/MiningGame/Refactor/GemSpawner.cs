using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GemSpawner : MonoBehaviour
{
    [Header("Mining Board")]
    [SerializeField]
    private Tilemap gemMap;
    [SerializeField]
    private int NUMBER_OF_COLUMNS;
    [SerializeField]
    private int NUMBER_OF_ROWS;
    [Range(0, 1f)]
    [SerializeField]
    private float GEM_SPAWN_PERCENT;
    [Range(0, 5)]
    [SerializeField]
    private float SPAWN_NUMBER_VARIENCE;

    [Header("Gem Percentages")]
    [Range(0, .5f)]
    [SerializeField]
    private float LARGE_GEM_SPAWN_PERCENTAGE;
    [Range(0, .5f)]
    [SerializeField]
    private float MEDIUM_GEM_SPAWN_PERCENTAGE;
    private float SMALL_GEM_SPAWN_PERCENTAGE;

    [Header("Sprites")]
    [SerializeField]
    private Sprite[] smallGemSprites;
    [SerializeField]
    private Sprite[] mediumGemSprites;
    [SerializeField]
    private Sprite[] largeGemSprites;

    private void Start()
    {
        CalculateSmallGemSpawnPercentage();
    }

    private void CalculateSmallGemSpawnPercentage()
    {
        SMALL_GEM_SPAWN_PERCENTAGE = 100 - (LARGE_GEM_SPAWN_PERCENTAGE + MEDIUM_GEM_SPAWN_PERCENTAGE);
    }

    public Gem[] SpawnGems()
    {
        //if (i == 0 || i == 1)
        //{
        //    int mediumOrLarge = Random.Range(0, 2);
        //    size = mediumOrLarge + 8;
        //}
        Gem[] spawnedGems = new Gem[GetNumberOfGemsToSpawnFromBoardSize()];
        for (int i = 0; i < spawnedGems.Length; i++)
        {
            spawnedGems[i] = SpawnRandomSizedGem();
        }
        return spawnedGems;
    }

    private int GetNumberOfGemsToSpawnFromBoardSize()
    {
        float percentageOfBoardSize = NUMBER_OF_COLUMNS * NUMBER_OF_ROWS * GEM_SPAWN_PERCENT;
        float actualVariance = Random.Range(0, SPAWN_NUMBER_VARIENCE * 2 + 1) - SPAWN_NUMBER_VARIENCE;
        return Mathf.FloorToInt(percentageOfBoardSize + actualVariance);
    }

    private Gem SpawnRandomSizedGem()
    {
        Gem spawnedGem = null;
        bool retryForEmptyBoardPosition = true;
        while (retryForEmptyBoardPosition)
        {
            Gem.Size randomGemSize = Gem.GetRandomWeightedSize(
                SMALL_GEM_SPAWN_PERCENTAGE,
                MEDIUM_GEM_SPAWN_PERCENTAGE,
                LARGE_GEM_SPAWN_PERCENTAGE);
            Vector3Int rngPosition = GetRandomBoardPosition();
            if (randomGemSize == Gem.Size.SMALL)
            {
                if (IsSmallGemAbleToSpawnAt(rngPosition))
                {
                    return SpawnSmallGem(rngPosition);
                }
            }
            else if (randomGemSize == Gem.Size.MEDIUM)
            {
                if (IsMediumGemAbleToSpawnAt(rngPosition))
                {
                    {
                        return SpawnMediumGem(rngPosition);
                    }
                }
            }
            else if (randomGemSize == Gem.Size.LARGE)
            {
                if (IsLargeGemAbleToSpawnAt(rngPosition))
                {
                    {
                        return SpawnLargeGem(rngPosition);
                    }
                }
            }
        }
        return spawnedGem;
    }

    private bool IsBoardPositionEmpty(Tile tile)
    {
        return tile == null;
    }

    private bool IsSmallGemAbleToSpawnAt(Vector3Int position)
    {
        return IsBoardPositionEmpty(GetBoardPosition(position));
    }

    private bool IsMediumGemAbleToSpawnAt(Vector3Int position)
    {
        bool result = position.x != NUMBER_OF_COLUMNS - 1 && position.y != NUMBER_OF_ROWS - 1;
        if (!result)
            return result;
        Tile rngTile = GetBoardPosition(position);
        Tile rngTile1 = GetBoardPosition(position + new Vector3Int(1, 0, 0));
        Tile rngTile2 = GetBoardPosition(position + new Vector3Int(0, 1, 0));
        Tile rngTile3 = GetBoardPosition(position + new Vector3Int(1, 1, 0));
        return IsBoardPositionEmpty(rngTile) && IsBoardPositionEmpty(rngTile1) &&
            IsBoardPositionEmpty(rngTile2) && IsBoardPositionEmpty(rngTile3);
    }

    private bool IsLargeGemAbleToSpawnAt(Vector3Int position)
    {
        bool result = position.x != NUMBER_OF_COLUMNS - 1 && position.x != NUMBER_OF_COLUMNS - 2
            && position.y != NUMBER_OF_ROWS - 1 && position.y != NUMBER_OF_ROWS - 2;
        if (!result)
            return result;
        Tile rngTile = GetBoardPosition(position);
        Tile rngTile1 = GetBoardPosition(position + new Vector3Int(1, 0, 0));
        Tile rngTile2 = GetBoardPosition(position + new Vector3Int(0, 1, 0));
        Tile rngTile3 = GetBoardPosition(position + new Vector3Int(1, 1, 0));
        Tile rngTile4 = GetBoardPosition(position + new Vector3Int(0, 2, 0));
        Tile rngTile5 = GetBoardPosition(position + new Vector3Int(1, 2, 0));
        Tile rngTile6 = GetBoardPosition(position + new Vector3Int(2, 0, 0));
        Tile rngTile7 = GetBoardPosition(position + new Vector3Int(2, 1, 0));
        Tile rngTile8 = GetBoardPosition(position + new Vector3Int(2, 2, 0));
        return IsBoardPositionEmpty(rngTile) && IsBoardPositionEmpty(rngTile1) &&
            IsBoardPositionEmpty(rngTile2) && IsBoardPositionEmpty(rngTile3) &&
            IsBoardPositionEmpty(rngTile4) && IsBoardPositionEmpty(rngTile5) &&
            IsBoardPositionEmpty(rngTile6) && IsBoardPositionEmpty(rngTile7) &&
            IsBoardPositionEmpty(rngTile8);
    }

    private Vector3Int GetRandomBoardPosition()
    {
        return new Vector3Int(
            Random.Range(0, NUMBER_OF_COLUMNS), 
            Random.Range(0, NUMBER_OF_ROWS),
            0);
    }

    private Tile GetBoardPosition(Vector3Int position)
    {
        return (Tile)gemMap.GetTile(position);
    }

    private void SetTileWithSpriteAtPosition(Sprite sprite, Vector3Int position)
    {
        Tile newTile = ScriptableObject.CreateInstance<Tile>();
        newTile.sprite = sprite;
        gemMap.SetTile(position, newTile);
    }

    private Gem SpawnSmallGem(Vector3Int position)
    {
        int rngSmallGem = Random.Range(0, smallGemSprites.Length);
        SetTileWithSpriteAtPosition(smallGemSprites[rngSmallGem], position);
        return new Gem(Gem.Size.SMALL, rngSmallGem, position);
    }

    private Gem SpawnMediumGem(Vector3Int position)
    {
        int rngMediumGem = Random.Range(0, mediumGemSprites.Length / 4);

        Vector3Int bottomLeft = position;
        Vector3Int bottomRight = position + new Vector3Int(1, 0, 0);
        Vector3Int topLeft = position + new Vector3Int(0, 1, 0);
        Vector3Int topRight = position + new Vector3Int(1, 1, 0);

        SetTileWithSpriteAtPosition(mediumGemSprites[rngMediumGem * 4], bottomLeft);
        SetTileWithSpriteAtPosition(mediumGemSprites[rngMediumGem * 4 + 1], bottomRight);
        SetTileWithSpriteAtPosition(mediumGemSprites[rngMediumGem * 4 + 2], topLeft);
        SetTileWithSpriteAtPosition(mediumGemSprites[rngMediumGem * 4 + 3], topRight);

        return new Gem(Gem.Size.MEDIUM, rngMediumGem, position);
    }

    private Gem SpawnLargeGem(Vector3Int position)
    {
        int rngLargeGem = Random.Range(0, largeGemSprites.Length / 9);

        Vector3Int bottomLeft = position;
        Vector3Int bottomMiddle = position + new Vector3Int(1, 0, 0);
        Vector3Int bottomRight = position + new Vector3Int(2, 0, 0);
        Vector3Int centerLeft = position + new Vector3Int(0, 1, 0);
        Vector3Int centerMiddle = position + new Vector3Int(1, 1, 0);
        Vector3Int centerRight = position + new Vector3Int(2, 1, 0);
        Vector3Int topLeft = position + new Vector3Int(0, 2, 0);
        Vector3Int topMiddle = position + new Vector3Int(1, 2, 0);
        Vector3Int topRight = position + new Vector3Int(2, 2, 0);

        SetTileWithSpriteAtPosition(largeGemSprites[rngLargeGem * 9], bottomLeft);
        SetTileWithSpriteAtPosition(largeGemSprites[rngLargeGem * 9 + 1], bottomMiddle);
        SetTileWithSpriteAtPosition(largeGemSprites[rngLargeGem * 9 + 2], bottomRight);
        SetTileWithSpriteAtPosition(largeGemSprites[rngLargeGem * 9 + 3], centerLeft);
        SetTileWithSpriteAtPosition(largeGemSprites[rngLargeGem * 9 + 4], centerMiddle);
        SetTileWithSpriteAtPosition(largeGemSprites[rngLargeGem * 9 + 5], centerRight);
        SetTileWithSpriteAtPosition(largeGemSprites[rngLargeGem * 9 + 6], topLeft);
        SetTileWithSpriteAtPosition(largeGemSprites[rngLargeGem * 9 + 7], topMiddle);
        SetTileWithSpriteAtPosition(largeGemSprites[rngLargeGem * 9 + 8], topRight);

        return new Gem(Gem.Size.LARGE, rngLargeGem, position);
    }
}
