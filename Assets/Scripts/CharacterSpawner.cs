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

    #region Character
    [Header("Character:")]
    [SerializeField]
    private GameObject characterPrefabInScene;
    GameObject character;
    #endregion

    #region SpawnInfo
    [Header("SpawnInfo:")]
    [Space]
    [Space]
    [SerializeField]
    Vector2 SpawnPoint;
    #endregion

    #region Property
    public GameObject Character
    {
        get { return character; }
    }
    #endregion

    private void Awake()
    {
        CreateSingleton();
        AssignInitialValues();
    }

    void AssignInitialValues()
    {
        character = Instantiate<GameObject>(characterPrefabInScene, SpawnPoint, characterPrefabInScene.transform.rotation);
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
