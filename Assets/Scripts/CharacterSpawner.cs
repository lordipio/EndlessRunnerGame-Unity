using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    #region Singleton
    private static CharacterSpawner instance;
    public static CharacterSpawner Instance
    {
        get { return instance; }
    }
    #endregion

    #region Prefab
    [Header("Prefabs:")]
    [SerializeField]
    private GameObject characterPrefabInScene;
    #endregion

    #region SpawnInfo
    [Header("SpawnInfo:")]
    [Space]
    [Space]
    [SerializeField]
    Vector2 SpawnPoint;
    #endregion

    GameObject character;
    public GameObject Character
    {
        get { return character; }
    }

    private void Awake()
    {
        CreateSingleton();
        character = Instantiate<GameObject>(characterPrefabInScene, SpawnPoint, characterPrefabInScene.transform.rotation);
    }



    // Start is called before the first frame update
    void Start()
    {
    }



    void CreateSingleton()
    {
        if (instance && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
}
