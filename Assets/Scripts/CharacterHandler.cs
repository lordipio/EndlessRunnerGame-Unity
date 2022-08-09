using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CharacterHandler : MonoBehaviour
{
    #region Delegations And Events
    public delegate void CharacterDiedDelegate();
    public static event CharacterDiedDelegate characterDiedEvent;

    public delegate void CharacterJumpedDelegate();
    public static event CharacterJumpedDelegate characterJumpedEvent;

    public delegate void CharacterRunDelegate();
    public static event CharacterRunDelegate characterRunEvent;

    #endregion

    bool isCharacterDead = false;

    #region Instances
    MapGenerator mapGenerator;
    CharacterSpawner characterSpawner;
    #endregion

    #region Prefabs
    GameObject characterPrefab;
    #endregion

    [SerializeField]
    Animator characterAnimator;

    BoxCollider2D characterCollider;
    Vector2 touchFirstPos;
    Vector2 touchLastPos;


    Vector2 characterPosition;
    Vector2 slope;

    [SerializeField]
    Vector2 roadLeftSide;

    [SerializeField]
    Vector2 roadRightSide;
    float speed;

    bool transitionIsOver = true;
    [SerializeField]
    float dragLength;


    #region SortingLayers
    string characterLeftSortingLayer = "CharacterLeft";
    string characterRightSortingLayer = "CharacterRight";
    string deadCharacterSortingLayer = "DeadCharacter";
    #endregion

    SpriteRenderer characterSpriteRenderer;

    void Awake()
    {
        characterCollider = GetComponent<BoxCollider2D>();
        characterSpriteRenderer = GetComponent<SpriteRenderer>();
        characterSpriteRenderer.sortingLayerName = characterRightSortingLayer;

    }

    // Start is called before the first frame update
    void Start()
    {
        characterRunEvent?.Invoke();
        AssignValues();
    }

    void Update()
    {
        MoveCharacterAloneXAxis();
        CharacterController();

        if (characterCollider.IsTouchingLayers() && !isCharacterDead)
        {
            isCharacterDead = true;
            CharacterDied();
        }
    }

    void AssignValues()
    {
        if (!MapGenerator.Instance)
        {
            throw new Exception("MapGenerator Instance is not found!");
        }
        mapGenerator = MapGenerator.Instance;

        if (!CharacterSpawner.Instance)
        {
            throw new Exception("CharacterSpawner Instance is not found!");
        }
        characterSpawner = CharacterSpawner.Instance;
        characterPrefab = characterSpawner.Character;

        //roadLeftSide = mapGenerator.leftSideRoad;
        //roadRightSide = mapGenerator.rightSideRoad;
        slope = mapGenerator.MovementSlope;
        speed = mapGenerator.cameraInitialSpeed;
    }

    private void OnEnable()
    {
    }

    // Update is called once per frame
    void MoveCharacterAloneXAxis()
    {
        characterPosition = characterPrefab.transform.position;
        characterPosition.x += slope.x * Time.deltaTime * speed;
        characterPrefab.transform.position = characterPosition;
    }

    void CharacterController()
    {
        if (Input.touchCount != 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                touchFirstPos = Camera.main.ScreenToWorldPoint(touch.position);
                touchLastPos = Camera.main.ScreenToWorldPoint(touch.position);
            }

            if (touch.phase == TouchPhase.Moved)
            {
                touchLastPos = Camera.main.ScreenToWorldPoint(touch.position);
            }

            if (touch.phase == TouchPhase.Ended)
            {
                touchLastPos = Camera.main.ScreenToWorldPoint(touch.position);
            }

            if (touchLastPos.y - touchFirstPos.y >= dragLength && transitionIsOver)
                StartCoroutine(MoveLeft());

            else if (touchLastPos.y - touchFirstPos.y <= -dragLength && transitionIsOver)
                StartCoroutine(MoveRight());

            if (touch.tapCount == 2)
            {
                Jump();
            }
        }

    }

    IEnumerator MoveLeft()
    {
        transitionIsOver = false;
        while (true)
        {
            yield return null;
            characterPosition.y += Time.deltaTime * speed * 2.5f;
            if (characterPosition.y >= roadLeftSide.y) // character is in leftside of road
            {
                characterPosition.y = roadLeftSide.y;
                characterSpriteRenderer.sortingLayerName = characterLeftSortingLayer;
                characterPrefab.transform.position = new Vector2(characterPrefab.transform.position.x, characterPosition.y);
                break;
            }

            characterPrefab.transform.position = new Vector2(characterPrefab.transform.position.x, characterPosition.y);
        }
        transitionIsOver = true;
        
    }

    IEnumerator MoveRight()
    {
        transitionIsOver = false;
        while (true)
        {
            yield return null;
            characterPosition.y -= Time.deltaTime * speed * 2.5f;
            if (characterPosition.y <= roadRightSide.y) // character is in rightside of road
            {
                characterPosition.y = roadRightSide.y;
                characterSpriteRenderer.sortingLayerName = characterRightSortingLayer;
                characterPrefab.transform.position = new Vector2(characterPrefab.transform.position.x, characterPosition.y);

                break;
            }

            characterPrefab.transform.position = new Vector2(characterPrefab.transform.position.x, characterPosition.y);
        }
        transitionIsOver = true;
    }



    void Jump()
    {
        characterJumpedEvent?.Invoke();
        characterAnimator.SetBool("IsJumping", true);
        characterCollider.enabled = false;
    }


    public void SetLandingTrue()
    {
        characterAnimator.SetBool("IsJumping", false);
        characterCollider.enabled = true;
        characterRunEvent?.Invoke();

    }

    void CharacterDied()
    {
        characterDiedEvent?.Invoke();

        characterAnimator.SetBool("IsDead", isCharacterDead);

        mapGenerator.cameraInitialSpeed = 0f;
        speed = 0f;
        characterSpriteRenderer.sortingLayerName = deadCharacterSortingLayer;
    }

}
