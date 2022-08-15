using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    #region Delegations And Events
    public delegate void CharacterDiedDelegate();
    public static event CharacterDiedDelegate characterDiedEvent;
    #endregion

    #region Singleton
    private static GameManager instance;
    public static GameManager Instance
    {
        get { return instance; }
    }
    #endregion

    #region Character

    BoxCollider2D characterCollider;
    GameObject character;
    bool isCharacterDead = false;

    #endregion

    #region CoinAndGem
    [SerializeField]
    CoinAndGemHandler coinAndGemHandler;
    #endregion

    void Awake()
    {
        CreateSingleton();
    }

    // Start is called before the first frame update
    void Start()
    {
        character = CharacterSpawner.Instance.Character;
        if (!character.GetComponent<BoxCollider2D>())
            throw new System.Exception("BoxCollider is not found!");
        characterCollider = character.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsCharacterCollided())
            CharacterDied();
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

    bool IsCharacterCollided()
    {
        if (characterCollider.IsTouchingLayers() && !isCharacterDead)
        {
            List<Collider2D> resultsColliders = new List<Collider2D>();
            ContactFilter2D contactFilter = new ContactFilter2D();
            characterCollider.OverlapCollider(contactFilter, resultsColliders);
            string colliderTag = resultsColliders[0].tag;

            if (colliderTag.CompareTo("Gem") == 0 || colliderTag.CompareTo("SilverCoin") == 0 || colliderTag.CompareTo("GoldenCoin") == 0)
            {
                //resultsColliders[0].gameObject.SetActive(false);
                coinAndGemHandler.CollidedWith(resultsColliders[0]);
                return false;
            }
            isCharacterDead = true;
        }
        return isCharacterDead;
    }

    void CharacterDied()
    {
        characterDiedEvent?.Invoke();
        character.GetComponent<Animator>().SetBool("IsDead", true);
        MapGenerator.Instance.cameraInitialSpeed = 0f;
    }
}
