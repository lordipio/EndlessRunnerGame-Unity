using UnityEngine;

[System.Serializable]
public class DataFile
{
    [HideInInspector]
    public int TotalCoins
    {
        get { return totalCoins; }
        set { totalCoins = value; }
    }

    [HideInInspector]
    public int SelectedCharacter
    {
        get { return selectedCharacter; }
        set { selectedCharacter = value; }
    }

    int selectedCharacter = 1;
    int totalCoins = 0;

    public void SetTotalCoin(int totalCoins)
    {
        this.totalCoins = totalCoins;
    }

    public void SetSelectedCharacter(int selectedCharacter)
    {
        this.selectedCharacter = selectedCharacter;
    }
}
