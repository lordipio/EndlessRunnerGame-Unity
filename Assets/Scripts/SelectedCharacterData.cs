using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SelectedCharacterData
{
    [HideInInspector]
    public int SelectedCharacter
    {
        get { return selectedCharacter; }
        set { selectedCharacter = value; }
    }

    int selectedCharacter = 1;

    public void SetSelectedCharacter(int selectedCharacter)
    {
        this.selectedCharacter = selectedCharacter;
    }
}
