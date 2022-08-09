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


    #region Prefabs
    [Space]
    [Header("Prefabs:")]
    [Space]
    [Space]

    //[SerializeField]
    //TileInfo roadPrefabTileInfo;

    //[SerializeField]
    //TileInfo tier_4TileInfo;

    //[SerializeField]
    //TileInfo tier_3TileInfo;

    //[SerializeField]
    //TileInfo tier_2TileInfo;

    //[SerializeField]
    //TileInfo tier_1TileInfo;

    //[SerializeField]
    //TileInfo tier1TileInfo;

    //[SerializeField]
    //TileInfo tier2TileInfo;

    //[SerializeField]
    //TileInfo tier3TileInfo;

    [SerializeField]
    GameObject roadPrefab;

    [SerializeField]
    List<GameObject> tier_4Prefab;

    [SerializeField]
    List<GameObject> tier_3Prefab;

    [SerializeField]
    List<GameObject> tier_2Prefab;

    [SerializeField]
    List<GameObject> tier_1Prefab;

    [SerializeField]
    List<GameObject> tier1Prefab;

    [SerializeField]
    List<GameObject> tier2Prefab;

    [SerializeField]
    List<GameObject> tier3Prefab;

    [SerializeField]
    List<GameObject> TreesPrefab;

    [SerializeField]
    List<GameObject> ObstaclesPrefab;

    [SerializeField]
    GameObject loadingScreen;
    #endregion

    #region Queue 
    Queue<GameObject> tier_4Queue = new Queue<GameObject>();
    Queue<GameObject> tier_3Queue = new Queue<GameObject>();
    Queue<GameObject> tier_2Queue = new Queue<GameObject>();
    Queue<GameObject> tier_1Queue = new Queue<GameObject>();
    Queue<GameObject> tier0Queue = new Queue<GameObject>();
    Queue<GameObject> tier1Queue = new Queue<GameObject>();
    Queue<GameObject> tier2Queue = new Queue<GameObject>();
    Queue<GameObject> tier3Queue = new Queue<GameObject>();
    Queue<GameObject> ObstaclesQueue = new Queue<GameObject>();
    #endregion

    #region SortingLayers
    int bigSizePlatformSortingOrder = 0;
    int midSizePlatformSortingOrder = 0;
    int smallSizePlatformSortingOrder = 0;
    #endregion

    #region ObstaclesSortingOrderTags
    string leftObstaclesSortingOrderTag = "LeftObstacles";
    string rightObstaclesSortingOrderTag = "RightObstacles";
    #endregion

    #region SpawnPoints
    Vector2 roadSpawnPoint;
    Vector2 tier_4SpawnPoint;
    Vector2 tier_3SpawnPoint;
    Vector2 tier_2SpawnPoint;
    Vector2 tier_1SpawnPoint;
    Vector2 tier1SpawnPoint;
    Vector2 tier2SpawnPoint;
    Vector2 tier3SpawnPoint;
    #endregion


    #region LastSpawnTiles
    GameObject lastTileTier_4 = null;
    GameObject lastTileTier_3 = null;
    GameObject lastTileTier_2 = null;
    GameObject lastTileTier_1 = null;
    GameObject lastTileTier0 = null;
    GameObject lastTileTier1 = null;
    GameObject lastTileTier2 = null;
    GameObject lastTileTier3 = null;
    #endregion




    #region PlatformOffsets
    [Space]
    [Header("PlatformsInfo:")]
    [Space]
    [Space]
    [SerializeField]
    int platformNumbers = 20;

    [SerializeField]
    [Range(1, 10)]
    int TreeSpawnProbablity;

    [SerializeField]
    Vector2 smallSizePlatformOffset;

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
    Vector2 smallSizePlatformTotalOffset = new Vector2(0f, 0f);
    Vector2 midSizePlatformTotalOffset = new Vector2(0f, 0f);
    Vector2 bigSizePlatformTotalOffset = new Vector2(0f, 0f);
    #endregion

    #region TotalOffsets
    Vector2 tier_4PlatformTotalOffset = new Vector2(0f, 0f);
    Vector2 tier_3PlatformTotalOffset = new Vector2(0f, 0f);
    Vector2 tier_2PlatformTotalOffset = new Vector2(0f, 0f);
    Vector2 tier_1PlatformTotalOffset = new Vector2(0f, 0f);
    Vector2 tier0PlatformTotalOffset = new Vector2(0f, 0f);
    Vector2 tier1PlatformTotalOffset = new Vector2(0f, 0f);
    Vector2 tier2PlatformTotalOffset = new Vector2(0f, 0f);
    Vector2 tier3PlatformTotalOffset = new Vector2(0f, 0f);
    #endregion



    #region FirstTilesInScene
    [Space]
    [Header("FirstTiles:")]
    [Space]
    [Space]

    [SerializeField]
    GameObject tier_4FirstTile;

    [SerializeField]
    GameObject tier_3FirstTile;

    [SerializeField]
    GameObject tier_2FirstTile;

    [SerializeField]
    GameObject tier_1FirstTile;

    [SerializeField]
    GameObject tier0FirstTile;

    [SerializeField]
    GameObject tier1FirstTile;

    [SerializeField]
    GameObject tier2FirstTile;

    [SerializeField]
    GameObject tier3FirstTile;
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

    bool isMapLoaded = false;

    void Awake()
    {
        CreateSingleton();
        AssignSpawnPoint();
        movementSlope = midSizePlatformOffset;
    }

    // Start is called before the first frame update
    void Start()
    {
        loadingScreen = Instantiate<GameObject>(loadingScreen);

        CreateTiles();
        //StartCoroutine(CreateTiles());
        StartCoroutine(ObstaclesSpawner());
        StartCoroutine(MoveCamera());
    }

    void CreateSingleton()
    {
        if (instance && instance != this)
        {
            Destroy(instance);
        }
        else
        {
            instance = this;
        }
    }

    IEnumerator ObstaclesSpawner()
    {
        string SortingOrderTag = "";
        float cameraLength = Camera.main.orthographicSize + 2f;
        while (true)
        {
            float randomWaitDelay = UnityEngine.Random.Range(0.5f, 2f);
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

            int randomObstacles = UnityEngine.Random.Range(0, ObstaclesPrefab.Count);
            
            GameObject currentObstacle = Instantiate<GameObject>(ObstaclesPrefab[randomObstacles], cameraPos, ObstaclesPrefab[0].transform.rotation);
            currentObstacle.GetComponent<SpriteRenderer>().sortingLayerName = SortingOrderTag;
            ObstaclesQueue.Enqueue(currentObstacle);

            DestroyOutofBoundsObstacles();
        }
    }

    void AssignSpawnPoint()
    {
        roadSpawnPoint = tier0FirstTile.transform.position;
        tier_4SpawnPoint = tier_4FirstTile.transform.position;
        tier_3SpawnPoint = tier_3FirstTile.transform.position;
        tier_2SpawnPoint = tier_2FirstTile.transform.position;
        tier_1SpawnPoint = tier_1FirstTile.transform.position;
        tier1SpawnPoint = tier1FirstTile.transform.position;
        tier2SpawnPoint = tier2FirstTile.transform.position;
        tier3SpawnPoint = tier3FirstTile.transform.position;

        StartCoroutine(DestroyFirstTiles());
    }


    IEnumerator DestroyFirstTiles()
    {
        Destroy(tier_4FirstTile);
        yield return null;
        Destroy(tier_3FirstTile);
        yield return null;
        Destroy(tier_2FirstTile);
        yield return null;
        Destroy(tier_1FirstTile);
        yield return null;
        Destroy(tier0FirstTile);
        yield return null;
        Destroy(tier1FirstTile);
        yield return null;
        Destroy(tier2FirstTile);
        yield return null;
        Destroy(tier3FirstTile);
    }




    void CreateTiles()
    {
        for (int i = 0; i < platformNumbers; i++)
        {
            if (i <= platformNumbers / 2) // Size of bigSizePlatform is 3 times bigger than smaller one
            {
                //roadPrefabTileInfo = Instantiate<TileInfo>(roadPrefabTileInfo, tier2SpawnPoint + tier2PlatformTotalOffset, tier2Prefab[0].transform.rotation);

                lastTileTier2 = Instantiate<GameObject>(tier2Prefab[0], tier2SpawnPoint + tier2PlatformTotalOffset, tier2Prefab[0].transform.rotation);
                lastTileTier_1 = Instantiate<GameObject>(tier_1Prefab[0], tier_1SpawnPoint + tier_1PlatformTotalOffset, tier_1Prefab[0].transform.rotation);
                lastTileTier1 = Instantiate<GameObject>(tier1Prefab[0], tier1SpawnPoint + tier1PlatformTotalOffset, tier1Prefab[0].transform.rotation);
                lastTileTier3 = Instantiate<GameObject>(tier3Prefab[0], tier3SpawnPoint + tier3PlatformTotalOffset, tier3Prefab[0].transform.rotation);
                lastTileTier_3 = Instantiate<GameObject>(tier_3Prefab[0], tier_3SpawnPoint + tier_3PlatformTotalOffset, tier_3Prefab[0].transform.rotation);
                lastTileTier_4 = Instantiate<GameObject>(tier_4Prefab[0], tier_4SpawnPoint + tier_4PlatformTotalOffset, tier_4Prefab[0].transform.rotation);
                lastTileTier_2 = Instantiate<GameObject>(tier_2Prefab[0], tier_2SpawnPoint + tier_2PlatformTotalOffset, tier_2Prefab[0].transform.rotation);

                lastTileTier_1.GetComponent<SpriteRenderer>().sortingOrder = bigSizePlatformSortingOrder;
                lastTileTier1.GetComponent<SpriteRenderer>().sortingOrder = bigSizePlatformSortingOrder;
                lastTileTier3.GetComponent<SpriteRenderer>().sortingOrder = bigSizePlatformSortingOrder;
                lastTileTier_3.GetComponent<SpriteRenderer>().sortingOrder = bigSizePlatformSortingOrder;
                lastTileTier_4.GetComponent<SpriteRenderer>().sortingOrder = bigSizePlatformSortingOrder;
                lastTileTier_2.GetComponent<SpriteRenderer>().sortingOrder = bigSizePlatformSortingOrder;
                lastTileTier2.GetComponent<SpriteRenderer>().sortingOrder = bigSizePlatformSortingOrder;

                tier_4Queue.Enqueue(lastTileTier_4);
                tier_3Queue.Enqueue(lastTileTier_3);
                tier_2Queue.Enqueue(lastTileTier_2);
                tier_1Queue.Enqueue(lastTileTier_1);
                tier1Queue.Enqueue(lastTileTier1);
                tier2Queue.Enqueue(lastTileTier2);
                tier3Queue.Enqueue(lastTileTier3);

                tier_1PlatformTotalOffset += smallSizePlatformOffset;
                tier1PlatformTotalOffset += smallSizePlatformOffset;
                tier_4PlatformTotalOffset += bigSizePlatformOffset;
                tier_3PlatformTotalOffset += bigSizePlatformOffset;
                tier_2PlatformTotalOffset += bigSizePlatformOffset;
                tier2PlatformTotalOffset += bigSizePlatformOffset;
                tier3PlatformTotalOffset += bigSizePlatformOffset;

                bigSizePlatformSortingOrder++;
            }

            lastTileTier0 = Instantiate<GameObject>(roadPrefab, roadSpawnPoint + tier0PlatformTotalOffset, roadPrefab.transform.rotation);
            tier0Queue.Enqueue(lastTileTier0);
            lastTileTier0.GetComponent<SpriteRenderer>().sortingOrder = midSizePlatformSortingOrder;
            tier0PlatformTotalOffset += midSizePlatformOffset;

            smallSizePlatformSortingOrder++;
            midSizePlatformSortingOrder++;
            Destroy(loadingScreen);
        }
    }

    //IEnumerator CreateTiles()
    //{

    //    for (int i = 0; i < platformNumbers; i++)
    //    {
    //        yield return null;
    //        if (i <= platformNumbers / 2) // Size of bigSizePlatform is 3 times bigger than smaller one
    //        {


    //            lastTileTier2 = Instantiate<GameObject>(tier2Prefab[0], tier2SpawnPoint + tier2PlatformTotalOffset, tier2Prefab[0].transform.rotation);
    //            lastTileTier_1 = Instantiate<GameObject>(tier_1Prefab[0], tier_1SpawnPoint + tier_1PlatformTotalOffset, tier_1Prefab[0].transform.rotation);
    //            lastTileTier1 = Instantiate<GameObject>(tier1Prefab[0], tier1SpawnPoint + tier1PlatformTotalOffset, tier1Prefab[0].transform.rotation);
    //            lastTileTier3 = Instantiate<GameObject>(tier3Prefab[0], tier3SpawnPoint + tier3PlatformTotalOffset, tier3Prefab[0].transform.rotation);
    //            lastTileTier_3 = Instantiate<GameObject>(tier_3Prefab[0], tier_3SpawnPoint + tier_3PlatformTotalOffset, tier_3Prefab[0].transform.rotation);
    //            lastTileTier_4 = Instantiate<GameObject>(tier_4Prefab[0], tier_4SpawnPoint + tier_4PlatformTotalOffset, tier_4Prefab[0].transform.rotation);
    //            lastTileTier_2 = Instantiate<GameObject>(tier_2Prefab[0], tier_2SpawnPoint + tier_2PlatformTotalOffset, tier_2Prefab[0].transform.rotation);

    //            lastTileTier_1.GetComponent<SpriteRenderer>().sortingOrder = bigSizePlatformSortingOrder;
    //            lastTileTier1.GetComponent<SpriteRenderer>().sortingOrder = bigSizePlatformSortingOrder;
    //            lastTileTier3.GetComponent<SpriteRenderer>().sortingOrder = bigSizePlatformSortingOrder;
    //            lastTileTier_3.GetComponent<SpriteRenderer>().sortingOrder = bigSizePlatformSortingOrder;
    //            lastTileTier_4.GetComponent<SpriteRenderer>().sortingOrder = bigSizePlatformSortingOrder;

    //            lastTileTier_2.GetComponent<SpriteRenderer>().sortingOrder = bigSizePlatformSortingOrder;
    //            lastTileTier2.GetComponent<SpriteRenderer>().sortingOrder = bigSizePlatformSortingOrder;

    //            tier_4Queue.Enqueue(lastTileTier_4);
    //            tier_3Queue.Enqueue(lastTileTier_3);
    //            tier_2Queue.Enqueue(lastTileTier_2);
    //            tier_1Queue.Enqueue(lastTileTier_1);
    //            tier1Queue.Enqueue(lastTileTier1);
    //            tier2Queue.Enqueue(lastTileTier2);
    //            tier3Queue.Enqueue(lastTileTier3);

    //            tier_1PlatformTotalOffset += smallSizePlatformOffset;
    //            tier1PlatformTotalOffset += smallSizePlatformOffset;
    //            tier_4PlatformTotalOffset += bigSizePlatformOffset;
    //            tier_3PlatformTotalOffset += bigSizePlatformOffset;
    //            tier_2PlatformTotalOffset += bigSizePlatformOffset;
    //            tier2PlatformTotalOffset += bigSizePlatformOffset;
    //            tier3PlatformTotalOffset += bigSizePlatformOffset;

    //            bigSizePlatformSortingOrder++;
    //        }

    //        lastTileTier0 = Instantiate<GameObject>(roadPrefab, roadSpawnPoint + tier0PlatformTotalOffset, roadPrefab.transform.rotation);
    //        tier0Queue.Enqueue(lastTileTier0);
    //        lastTileTier0.GetComponent<SpriteRenderer>().sortingOrder = midSizePlatformSortingOrder;
    //        tier0PlatformTotalOffset += midSizePlatformOffset;

    //        smallSizePlatformSortingOrder++;
    //        midSizePlatformSortingOrder++;
    //    }

    //    Destroy(loadingScreen);
    //    StartCoroutine(MoveCamera());
    //}


    int GetTierofOutOfBoundsTile()
    {
        float cameraLength = Camera.main.orthographicSize + 2;
        float cameraPosx = Camera.main.transform.position.x;
        
        if (lastTileTier_4.transform.position.x - cameraLength < cameraPosx) { return -4; }
        if (lastTileTier_3.transform.position.x - cameraLength < cameraPosx) { return -3; }
        if (lastTileTier_2.transform.position.x - cameraLength < cameraPosx) { return -2; }
        if (lastTileTier_1.transform.position.x - cameraLength < cameraPosx) { return -1; }
        if (lastTileTier0.transform.position.x - cameraLength < cameraPosx) { return 0; }
        if (lastTileTier1.transform.position.x - cameraLength < cameraPosx) { return 1; }
        if (lastTileTier2.transform.position.x - cameraLength < cameraPosx) { return 2; }
        if (lastTileTier3.transform.position.x - cameraLength < cameraPosx) { return 3; }

        throw new Exception("Tier not found!");
    }

    bool AreTilesOutofBounds()
    {
        float cameraLength = Camera.main.orthographicSize + 2f;
        float cameraPosx = Camera.main.transform.position.x;

        if (lastTileTier_4.transform.position.x - cameraLength < cameraPosx || lastTileTier_3.transform.position.x - cameraLength < cameraPosx
            || lastTileTier_2.transform.position.x - cameraLength < cameraPosx || lastTileTier_1.transform.position.x - cameraLength < cameraPosx
            || lastTileTier0.transform.position.x - cameraLength < cameraPosx || lastTileTier1.transform.position.x - cameraLength < cameraPosx
            || lastTileTier2.transform.position.x - cameraLength < cameraPosx || lastTileTier3.transform.position.x - cameraLength < cameraPosx)
        {
            return true;
        }
        else
            return false;
    }

    void DestroyOutofBoundsObstacles()
    {
        float cameraLength = Camera.main.orthographicSize + 2f;
        float cameraPosx = Camera.main.transform.position.x;

        while (ObstaclesQueue.Peek().transform.position.x + cameraLength < cameraPosx)
        {
            GameObject TempGameObjectForDestroy = ObstaclesQueue.Peek();
            ObstaclesQueue.Dequeue();
            Destroy(TempGameObjectForDestroy);
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
                ReplaceOldTileWithNew(GetTierofOutOfBoundsTile());
        }
    }


    void ReplaceOldTileWithNew(int TierNum)
    {
        if (TierNum > 3 || TierNum < -4)
        {
            throw new Exception("TierNum is out of given bound!");
        }

        GameObject tempGameObject = null;


        if (TierNum == -4)
        {
            tempGameObject = tier_4Queue.Peek();
            tier_4Queue.Dequeue();
            tempGameObject.transform.position = tier_4SpawnPoint + tier_4PlatformTotalOffset;
            tempGameObject.GetComponent<SpriteRenderer>().sortingOrder = bigSizePlatformSortingOrder;
            tier_4PlatformTotalOffset += bigSizePlatformOffset;
            tier_4Queue.Enqueue(tempGameObject);
            lastTileTier_4 = tempGameObject;
            bigSizePlatformSortingOrder++;
            return;
        }

        if (TierNum == -3)
        {
            tempGameObject = tier_3Queue.Peek();
            tier_3Queue.Dequeue();
            tempGameObject.transform.position = tier_3SpawnPoint + tier_3PlatformTotalOffset;
            tempGameObject.GetComponent<SpriteRenderer>().sortingOrder = bigSizePlatformSortingOrder;
            tier_3PlatformTotalOffset += bigSizePlatformOffset;
            tier_3Queue.Enqueue(tempGameObject);
            lastTileTier_3 = tempGameObject;
            bigSizePlatformSortingOrder++;
            return;
        }

        if (TierNum == -2)
        {
            tempGameObject = tier_2Queue.Peek();
            tier_2Queue.Dequeue();
            tempGameObject.transform.position = tier_2SpawnPoint + tier_2PlatformTotalOffset;
            tier_2Queue.Enqueue(tempGameObject);
            tier_2PlatformTotalOffset += bigSizePlatformOffset;
            tempGameObject.GetComponent<SpriteRenderer>().sortingOrder = bigSizePlatformSortingOrder;
            lastTileTier_2 = tempGameObject;
            bigSizePlatformSortingOrder++;
            return;
        }

        if (TierNum == -1)
        {
            tempGameObject = tier_1Queue.Peek();
            tier_1Queue.Dequeue();
            tempGameObject.transform.position = tier_1SpawnPoint + tier_1PlatformTotalOffset;
            tier_1Queue.Enqueue(tempGameObject);
            tier_1PlatformTotalOffset += smallSizePlatformOffset;
            tempGameObject.GetComponent<SpriteRenderer>().sortingOrder = smallSizePlatformSortingOrder;
            lastTileTier_1 = tempGameObject;
            smallSizePlatformSortingOrder++;
            return;
        }

        if (TierNum == 0)
        {
            tempGameObject = tier0Queue.Peek();
            tier0Queue.Dequeue();
            tempGameObject.transform.position = roadSpawnPoint + tier0PlatformTotalOffset;
            tier0Queue.Enqueue(tempGameObject);
            tier0PlatformTotalOffset += midSizePlatformOffset;
            tempGameObject.GetComponent<SpriteRenderer>().sortingOrder = midSizePlatformSortingOrder;
            lastTileTier0 = tempGameObject;
            midSizePlatformSortingOrder++;
            return;
        }

        if (TierNum == 1)
        {
            tempGameObject = tier1Queue.Peek();
            tier1Queue.Dequeue();
            tempGameObject.transform.position = tier1SpawnPoint + tier1PlatformTotalOffset;
            tier1Queue.Enqueue(tempGameObject);
            tier1PlatformTotalOffset += smallSizePlatformOffset;
            tempGameObject.GetComponent<SpriteRenderer>().sortingOrder = smallSizePlatformSortingOrder;
            lastTileTier1 = tempGameObject;
            smallSizePlatformSortingOrder++;
            return;
        }

        if (TierNum == 2)
        {
            tempGameObject = tier2Queue.Peek();
            tier2Queue.Dequeue();
            tempGameObject.transform.position = tier2SpawnPoint + tier2PlatformTotalOffset;
            tier2Queue.Enqueue(tempGameObject);
            tier2PlatformTotalOffset += bigSizePlatformOffset;
            tempGameObject.GetComponent<SpriteRenderer>().sortingOrder = bigSizePlatformSortingOrder;
            lastTileTier2 = tempGameObject;
            midSizePlatformSortingOrder++;
            return;
        }

        if (TierNum == 3)
        {
            tempGameObject = tier3Queue.Peek();
            tier3Queue.Dequeue();
            tempGameObject.transform.position = tier3SpawnPoint + tier3PlatformTotalOffset;
            tier3Queue.Enqueue(tempGameObject);
            tier3PlatformTotalOffset += bigSizePlatformOffset;
            tempGameObject.GetComponent<SpriteRenderer>().sortingOrder = bigSizePlatformSortingOrder;
            bigSizePlatformSortingOrder++;
            lastTileTier3 = tempGameObject;
            return;
        }

    }

}

