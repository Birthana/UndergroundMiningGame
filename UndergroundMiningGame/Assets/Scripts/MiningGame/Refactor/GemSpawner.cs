using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GemSpawner : MonoBehaviour
{
    #region Field: Mining Board
    [Header("Mining Board")]

    [SerializeField] private Tilemap gemMap;
    [SerializeField] private int NUMBER_OF_COLUMNS;
    [SerializeField] private int NUMBER_OF_ROWS;
    #endregion

    #region Field: Gem Percentage 
    [Header("Gem Percentages")]
    [Range(0, 1f)]
    [SerializeField] private float GEM_SPAWN_PERCENT;
    [Range(0, 5)]
    [SerializeField] private float VARIENCE;
    [Range(0, .5f)]
    [SerializeField] private float LARGE_GEM_SPAWN;
    [Range(0, .5f)]
    [SerializeField] private float MEDIUM_GEM_SPAWN;
    private float SMALL_GEM_SPAWN;
    #endregion

    #region Field: Gem Sprites
    [Header("Sprites")]

    [SerializeField] private Sprite[] smallGemSprites;
    [SerializeField] private Sprite[] mediumGemSprites;
    [SerializeField] private Sprite[] largeGemSprites;

    private const int MEDIUM_GEM_TILES = 4;
    private const int LARGE_GEM_TILES = 9;
    #endregion

    private Vector3Int rngGemPosition;

    #region Function: Setup
    private void Start()
    {
        rngGemPosition = GetRandomBoardPosition();
        CalculateSmallGemSpawnPercentage();
    }

    private void CalculateSmallGemSpawnPercentage()
    {
        SMALL_GEM_SPAWN = 100 - (LARGE_GEM_SPAWN + MEDIUM_GEM_SPAWN);
    }
    #endregion

    #region Function: Main
    public Gem[] SpawnGems()
    {
        //TODO: If first gem, spawn a medium or large gem.
        Gem[] spawnedGems = new Gem[GetNumberOfGemsToSpawnFromBoardSize()];
        for (int i = 0; i < spawnedGems.Length; i++)
        {
            spawnedGems[i] = SpawnRandomSizedGem();
        }
        return spawnedGems;
    }

    private int GetNumberOfGemsToSpawnFromBoardSize()
    {
        float numberOfGemsToSpawn = NUMBER_OF_COLUMNS * NUMBER_OF_ROWS * GEM_SPAWN_PERCENT;
        float actualVariance = Random.Range(0, VARIENCE * 2 + 1) - VARIENCE;
        return Mathf.FloorToInt(numberOfGemsToSpawn + actualVariance);
    }

    private Gem SpawnRandomSizedGem()
    {
        Gem spawnedGem = null;
        bool retryForEmptyBoardPosition = true;
        while (retryForEmptyBoardPosition)
        {
            Gem.Size randomGemSize = Gem.GetRandomWeightedSize(
                SMALL_GEM_SPAWN,
                MEDIUM_GEM_SPAWN,
                LARGE_GEM_SPAWN);

            if (randomGemSize == Gem.Size.SMALL)
                spawnedGem = SpawnSmallGem();
            else if (randomGemSize == Gem.Size.MEDIUM)
                spawnedGem = SpawnMediumGem();
            else if (randomGemSize == Gem.Size.LARGE)
                spawnedGem = SpawnLargeGem();

            if (spawnedGem != null)
                retryForEmptyBoardPosition = false;
        }
        return spawnedGem;
    }
    #endregion

    #region Function: Boolean
    private bool IsBoardPositionEmpty(Tile tile)
    {
        return tile == null;
    }

    private bool IsBoardPositionsEmpty(Vector3Int[] gemPositions)
    {
        Tile[] rngTiles = new Tile[gemPositions.Length];
        for (int i = 0; i < gemPositions.Length; i++)
        {
            rngTiles[i] = GetBoardPosition(gemPositions[i]);
            if (!IsBoardPositionEmpty(rngTiles[i]))
                return false;
        }
        return true;
    }

    private bool IsSmallGemAbleToSpawnAt()
    {
        return IsBoardPositionEmpty(GetBoardPosition(rngGemPosition));
    }

    private bool IsMediumGemAbleToSpawnAt()
    {
        return !IsMediumGemOutOfBounds(rngGemPosition) && 
            IsBoardPositionsEmpty(GetMediumGemPositions(rngGemPosition));
    }

    private bool IsMediumGemOutOfBounds(Vector3Int position)
    {
        return position.x == NUMBER_OF_COLUMNS - 1 || position.y == NUMBER_OF_ROWS - 1;
    }

    private bool IsLargeGemAbleToSpawnAt()
    {
        return !IsLargeGemOutOfBounds(rngGemPosition) &&
            IsBoardPositionsEmpty(GetLargeGemPositions(rngGemPosition));
    }

    private bool IsLargeGemOutOfBounds(Vector3Int position)
    {
        return position.x == NUMBER_OF_COLUMNS - 1 || position.x == NUMBER_OF_COLUMNS - 2
            || position.y == NUMBER_OF_ROWS - 1 || position.y == NUMBER_OF_ROWS - 2;
    }

    #endregion

    #region Function: Gem Spawning
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

    private Gem SpawnSmallGem()
    {
        if (!IsSmallGemAbleToSpawnAt())
            return null;
        return SpawnSmallGemAt();
    }

    private Gem SpawnSmallGemAt()
    {
        int rngSmallGem = Random.Range(0, smallGemSprites.Length);
        SetTileWithSpriteAtPosition(smallGemSprites[rngSmallGem], rngGemPosition);
        return new Gem(Gem.Size.SMALL, rngSmallGem, rngGemPosition);
    }

    private Gem SpawnMediumGem()
    {
        if (!IsMediumGemAbleToSpawnAt())
            return null;
        return SpawnMediumGemAt();
    }

    private Gem SpawnMediumGemAt()
    {
        int rngMediumGem = Random.Range(0, mediumGemSprites.Length / MEDIUM_GEM_TILES);

        Vector3Int[] gemPositions = GetMediumGemPositions(rngGemPosition);

        for (int i = 0; i < gemPositions.Length; i++)
        {
            SetTileWithSpriteAtPosition(mediumGemSprites[rngMediumGem * MEDIUM_GEM_TILES + i], 
                gemPositions[i]);
        }

        return new Gem(Gem.Size.MEDIUM, rngMediumGem, rngGemPosition);
    }

    private Vector3Int[] GetMediumGemPositions(Vector3Int position)
    {
        return new Vector3Int[] { position,
            position + new Vector3Int(1, 0, 0),
            position + new Vector3Int(0, 1, 0),
            position + new Vector3Int(1, 1, 0)
        };
    }

    private Gem SpawnLargeGem()
    {
        if (!IsLargeGemAbleToSpawnAt())
            return null;
        return SpawnLargeGemAt();
    }

    private Gem SpawnLargeGemAt()
    {
        int rngLargeGem = Random.Range(0, largeGemSprites.Length / LARGE_GEM_TILES);

        Vector3Int[] gemPositions = GetLargeGemPositions(rngGemPosition);

        for (int i = 0; i < gemPositions.Length; i++)
        {
            SetTileWithSpriteAtPosition(largeGemSprites[rngLargeGem * LARGE_GEM_TILES + i],
                gemPositions[i]);
        }

        return new Gem(Gem.Size.LARGE, rngLargeGem, rngGemPosition);
    }

    private Vector3Int[] GetLargeGemPositions(Vector3Int position)
    {
        return new Vector3Int[] {
            position,
            position + new Vector3Int(1, 0, 0),
            position + new Vector3Int(2, 0, 0),
            position + new Vector3Int(0, 1, 0),
            position + new Vector3Int(1, 1, 0),
            position + new Vector3Int(2, 1, 0),
            position + new Vector3Int(0, 2, 0),
            position + new Vector3Int(1, 2, 0),
            position + new Vector3Int(2, 2, 0),
        };
    }
    #endregion
}
