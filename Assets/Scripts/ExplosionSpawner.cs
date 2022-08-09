using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ExplosionSpawner : MonoBehaviour
{


    CharacterSpawner characterSpawner;
    GameObject character;


    [SerializeField]
    GameObject explosionPrefab;




    // Start is called before the first frame update
    void Start()
    {
        AssignValues();
    }

    void AssignValues()
    {
        if (!CharacterSpawner.Instance)
            throw new Exception("CharacterSpawner Instance is not found!");
        characterSpawner = CharacterSpawner.Instance;
        if (!explosionPrefab)
            throw new Exception("explosionPrefab is not found!");
        character = characterSpawner.Character;
        if (!character)
            throw new Exception("Character is not found!");

        CharacterHandler.characterDiedEvent += Explode;
    }


    public void Explode()
    {
        if (!explosionPrefab)
            print("explosion prefab is not found!");
        if (!character)
            print("character is not found!");
        Instantiate<GameObject>(explosionPrefab, character.transform.position, explosionPrefab.transform.rotation);
        CharacterHandler.characterDiedEvent -= Explode;

    }


}