using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character")]
public class CharacterScriptableObject : ScriptableObject
{
    public SpriteRenderer spriteRenderer;

    public Animator animator;

    public Rigidbody2D rigidbody;

    public BoxCollider2D boxCollider;

    public CharacterHandler characterHandler;

    public Sprite CharacterIdlePose;

    public int price;

    //[HideInInspector]
    public bool isUnlocked;
}
