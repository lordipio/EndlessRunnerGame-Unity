using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyExplosion : MonoBehaviour
{
    void DestroyThisExplosion()
    {
        gameObject.SetActive(false);
    }
}
