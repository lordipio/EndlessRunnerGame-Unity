using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ExplosionSpawner : MonoBehaviour
{
    #region Character
    CharacterSpawner characterSpawner;
    GameObject character;
    #endregion

    #region Explosion
    [Header("Explosion:")]
    [Space]
    [Space]
    [SerializeField]
    GameObject explosionPrefab;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        AssignInitialValues();
    }

    void AssignInitialValues()
    {
        if (!CharacterSpawner.Instance)
            throw new Exception("CharacterSpawner Instance is not found!");
        characterSpawner = CharacterSpawner.Instance;
        if (!explosionPrefab)
            throw new Exception("explosionPrefab is not found!");
        character = characterSpawner.Character;
        if (!character)
            throw new Exception("Character is not found!");

        GameManager.characterDiedEvent += Explode;
    }

    public void Explode()
    {
        if (!explosionPrefab)
            print("explosion prefab is not found!");
        if (!character)
            print("character is not found!");
        Instantiate<GameObject>(explosionPrefab, character.transform.position, explosionPrefab.transform.rotation);
        GameManager.characterDiedEvent -= Explode;
    }
}