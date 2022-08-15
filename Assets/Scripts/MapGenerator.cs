using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class MapGenerator : MonoBehaviour
{
    #region Singleton
    static MapGenerator instance;
    public static MapGenerator Instance
    {
        get { return instance; }
    }
    #endregion

    #region Delegates And Events
    public delegate void DestroyDelegate();
    public static DestroyDelegate DestroyEvent;
    #endregion

    #region Prefabs
    [Space]
    [Header("Prefabs:")]
    [Space]
    [Space]

    [SerializeField]
    List<TileInfo> obstaclesPrefab;

    [SerializeField]
    GameObject loadingScreenPrefab;
    GameObject loadingScreen;
    #endregion

    #region Queues
    Queue<TileInfo>[] tierQueues;
    Queue<TileInfo> obstaclesQueue = new Queue<TileInfo>();
    #endregion

    #region SortingLayers
    int bigSizePlatformSortingOrder = 0;
    int midSizePlatformSortingOrder = 0;
    #endregion

    #region ObstaclesSortingOrderTags
    string leftObstaclesSortingOrderTag = "LeftObstacles";
    string rightObstaclesSortingOrderTag = "RightObstacles";
    #endregion

    #region SpawnPoints
    Vector2[] tiersSpawnPoint = new Vector2[8];

    #endregion

    #region LastSpawnTiles
    TileInfo[] lastTilesInTier;
    #endregion

    #region PlatformOffsets
    [Space]
    [Header("PlatformsInfo:")]
    [Space]
    [Space]
    [SerializeField]
    int numberofTilesInTier = 20;

    [SerializeField]
    Vector2 midSizePlatformOffset;

    [SerializeField]
    Vector2 bigSizePlatformOffset;

    [SerializeField]
    public Vector2 leftSideRoad;

    [SerializeField]
    public Vector2 rightSideRoad;
    #endregion

    #region TotalOffsets
    Vector2[] tilesTotalOffset = new Vector2[8];
    #endregion

    #region FirstTilesInScene
    [Space]
    [Header("FirstTiles:")]
    [Space]
    [Space]

    [SerializeField]
    TileInfo[] firstTileInfosInScene;

    int numberofTiers;
    #endregion

    #region PlatformOffsets
    [Space]
    [Header("ObstaclesInfo:")]
    [Space]
    [Space]

    [SerializeField]
    Vector2 roadLeftSidePoint;

    [SerializeField]
    Vector2 roadRightSidePoint;
    #endregion

    #region Camera
    [Space]
    [Header("CameraMovement:")]
    [Space]
    [Space]
    [SerializeField]
    public float cameraInitialSpeed = 10f;
    Vector2 movementSlope;
    public Vector2 MovementSlope
    {
        get { return movementSlope; }
    }
    #endregion

    void Awake()
    {
        CreateSingleton();
        AssignInitialValues();
        movementSlope = midSizePlatformOffset;
    }

    // Start is called before the first frame update
    void Start()
    {
        loadingScreen = Instantiate<GameObject>(loadingScreenPrefab);
        CreateTiles();
        //StartCoroutine(CreateTilesWithFrame());
        StartCoroutine(ObstaclesSpawner());
        StartCoroutine(MoveCamera());
        StartCoroutine(DestroyUnessentials());
    }

    void CreateSingleton()
    {
        if (instance && instance != this)
            Destroy(instance);
        else
            instance = this;
    }

    void AssignInitialValues()
    {
        numberofTiers = firstTileInfosInScene.Length;
        tierQueues = new Queue<TileInfo>[numberofTiers];
        Queue<TileInfo> TempQueue;

        for (int i = 0; i < numberofTiers; i++)
        {
            TempQueue = new Queue<TileInfo>();
            tierQueues[i] = TempQueue;
        }

        tiersSpawnPoint = new Vector2[numberofTiers];
        lastTilesInTier = new TileInfo[numberofTiers];

        for (int i = 0; i < numberofTiers; i++)
            tiersSpawnPoint[i] = firstTileInfosInScene[i].transform.position;

        for (int i = 0; i < numberofTiers; i++)
            tilesTotalOffset[i] = new Vector2(0f, 0f);

    }

    IEnumerator DestroyUnessentials()
    {
        for (int i = 0; i < numberofTiers; i++)
        {
            Destroy(firstTileInfosInScene[i].gameObject);
            yield return null;
        }
        Destroy(loadingScreen.gameObject);
    }

    void CreateTiles()
    {
        Quaternion tilesRotation = firstTileInfosInScene[0].transform.rotation;
        for (int i = 0; i < numberofTilesInTier; i++)
        {
            for (int j = 0; j < numberofTiers; j++)
            {
                if (j == 4) // 4th tier element offset are smaller than the other ones
                {
                    lastTilesInTier[4] = Instantiate<TileInfo>(firstTileInfosInScene[4], tiersSpawnPoint[4] + tilesTotalOffset[4], tilesRotation);
                    lastTilesInTier[4].spriteRenderer.sortingOrder = midSizePlatformSortingOrder;
                    tierQueues[4].Enqueue(lastTilesInTier[4]);
                    tilesTotalOffset[4].x += midSizePlatformOffset.x;
                    continue;
                }

                if (i <= numberofTilesInTier / 2)
                {
                    lastTilesInTier[j] = Instantiate<TileInfo>(firstTileInfosInScene[j], tiersSpawnPoint[j] + tilesTotalOffset[j], tilesRotation);
                    lastTilesInTier[j].spriteRenderer.sortingOrder = bigSizePlatformSortingOrder;
                    tierQueues[j].Enqueue(lastTilesInTier[j]);
                    tilesTotalOffset[j].x += bigSizePlatformOffset.x;
                }
            }

            midSizePlatformSortingOrder++;
            bigSizePlatformSortingOrder++;
        }

       loadingScreen.SetActive(false);
    }

    IEnumerator CreateTilesWithFrame()
    {
        Quaternion tilesRotation = firstTileInfosInScene[0].transform.rotation;
        for (int i = 0; i < numberofTilesInTier; i++)
        {
            yield return null;
            for (int j = 0; j < numberofTiers; j++)
            {
                if (j == 4) // 4th tier element offset are smaller than the other ones
                {
                    lastTilesInTier[4] = Instantiate<TileInfo>(firstTileInfosInScene[4], tiersSpawnPoint[4] + tilesTotalOffset[4], tilesRotation);
                    lastTilesInTier[4].spriteRenderer.sortingOrder = midSizePlatformSortingOrder;
                    tierQueues[4].Enqueue(lastTilesInTier[4]);
                    tilesTotalOffset[4].x += midSizePlatformOffset.x;
                    continue;
                }

                if (i <= numberofTilesInTier / 2)
                {
                    lastTilesInTier[j] = Instantiate<TileInfo>(firstTileInfosInScene[j], tiersSpawnPoint[j] + tilesTotalOffset[j], tilesRotation);
                    lastTilesInTier[j].spriteRenderer.sortingOrder = bigSizePlatformSortingOrder;
                    tierQueues[j].Enqueue(lastTilesInTier[j]);
                    tilesTotalOffset[j].x += bigSizePlatformOffset.x;
                }
            }

            midSizePlatformSortingOrder++;
            bigSizePlatformSortingOrder++;
        }

        loadingScreen.SetActive(false);
    }

    int GetTierofOutofBoundsTile()
    {
        float cameraLength = Camera.main.orthographicSize + 2;
        float cameraPosx = Camera.main.transform.position.x;

        for (int i = 0; i < numberofTiers; i++)
        {
            if (lastTilesInTier[i].transform.position.x - cameraLength < cameraPosx)
                return i;
        }

        throw new Exception("Tier not found!");
    }

    bool AreTilesOutofBounds()
    {
        float cameraLength = Camera.main.orthographicSize + 2f;
        float cameraPosx = Camera.main.transform.position.x;

        for (int i = 0; i < numberofTiers; i++)
        {
            if (lastTilesInTier[i].transform.position.x - cameraLength < cameraPosx)
                return true;
        }
        return false;
    }

    void DestroyOutofBoundsObstacles()
    {
        float cameraLength = Camera.main.orthographicSize + 2f;
        float cameraPosx = Camera.main.transform.position.x;


        while (obstaclesQueue.Peek().transform.position.x + cameraLength < cameraPosx)
        {
            TileInfo TempGameObjectForDestroy = obstaclesQueue.Peek();
            obstaclesQueue.Dequeue();
            Destroy(TempGameObjectForDestroy.gameObject);
        }
    }

    public void DestroyOutofBoundsGameObject(List<GameObject> gameObjectList) //for an Individual gameObject
    {
        float cameraLength = Camera.main.orthographicSize + 2f;
        float cameraPosx = Camera.main.transform.position.x;
        GameObject currentGameObject;
        for (int i = 0; i < gameObjectList.Count; i++)
        {
            currentGameObject = gameObjectList[i];
            if (currentGameObject.transform.position.x + cameraLength < cameraPosx)
            {
                Destroy(currentGameObject);
                gameObjectList.RemoveAt(i);
            }
        }
    }

    IEnumerator MoveCamera()
    {
        Vector3 cameraCurrentPos;
        while (true)
        {
            yield return null;
            cameraCurrentPos = Camera.main.transform.position;
            cameraCurrentPos.x += movementSlope.x * Time.deltaTime * cameraInitialSpeed;
            cameraCurrentPos.y += movementSlope.y * Time.deltaTime * cameraInitialSpeed;
            Camera.main.transform.position = cameraCurrentPos;

            if (AreTilesOutofBounds())
                ReplaceOldTileWithNew(GetTierofOutofBoundsTile());
        }
    }

    IEnumerator ObstaclesSpawner()
    {
        string SortingOrderTag = "";
        float cameraLength = Camera.main.orthographicSize + 2f;
        while (true)
        {
            float randomWaitDelay = UnityEngine.Random.Range(0.5f, 2f);
            //randomWaitDelay = 0.5f;
            yield return new WaitForSeconds(randomWaitDelay);

            Vector2 cameraPos = Camera.main.transform.position;
            int roadLeftOrRight = UnityEngine.Random.Range(0, 2);

            if (roadLeftOrRight == 0) //left side of road
            {
                cameraPos.y = roadLeftSidePoint.y;
                SortingOrderTag = leftObstaclesSortingOrderTag;
            }

            if (roadLeftOrRight == 1) //right side of road
            {
                cameraPos.y = roadRightSidePoint.y;
                SortingOrderTag = rightObstaclesSortingOrderTag;
            }

            cameraPos.x += cameraLength;
            int randomObstacles = UnityEngine.Random.Range(0, obstaclesPrefab.Count);
            TileInfo currentObstacle = Instantiate<TileInfo>(obstaclesPrefab[randomObstacles], cameraPos, obstaclesPrefab[0].transform.rotation);
            currentObstacle.spriteRenderer.sortingLayerName = SortingOrderTag;
            obstaclesQueue.Enqueue(currentObstacle);
            DestroyEvent?.Invoke();
            DestroyOutofBoundsObstacles();
        }
    }

    void ReplaceOldTileWithNew(int tierNum)
    {
        if (tierNum < 0 || tierNum > numberofTiers)
            throw new Exception("TierNum is out of given bound!");

        TileInfo tempTileInfo = null;
        tempTileInfo = tierQueues[tierNum].Peek();
        tierQueues[tierNum].Dequeue();
        tempTileInfo.transform.position = tiersSpawnPoint[tierNum] + tilesTotalOffset[tierNum];

        if (tierNum == 4)
        {
            tempTileInfo.spriteRenderer.sortingOrder = midSizePlatformSortingOrder;
            tilesTotalOffset[4].x += midSizePlatformOffset.x;
            midSizePlatformSortingOrder++;
        }

        else
        {
            tempTileInfo.spriteRenderer.sortingOrder = bigSizePlatformSortingOrder;
            tilesTotalOffset[tierNum].x += bigSizePlatformOffset.x;
            bigSizePlatformSortingOrder++;
        }

        tierQueues[tierNum].Enqueue(tempTileInfo);
        lastTilesInTier[tierNum] = tempTileInfo;
    }
}

