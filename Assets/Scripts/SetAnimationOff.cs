using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAnimationOff : MonoBehaviour
{
    void SetIsAnimationOverTrue()
    {
        GetComponent<Animator>().SetBool("IsAnimationOver", true);
        gameObject.SetActive(false);
    }
}
