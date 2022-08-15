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
    GameObject[] allCharacters;
    GameObject character;

    #endregion

    #region SpawnInfo
    [Header("SpawnInfo:")]
    [Space]
    [Space]
    [SerializeField]
    Vector2 spawnPoint;
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
        Spawn();
    }

    void Spawn()
    {
        int selectedCharacterNumber = SaveAndLoadSystem.LoadSelectedCharacter().SelectedCharacter - 1;
        character = Instantiate<GameObject>(allCharacters[selectedCharacterNumber], spawnPoint, allCharacters[selectedCharacterNumber].transform.rotation);
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
