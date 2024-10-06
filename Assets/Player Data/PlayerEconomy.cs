using UnityEngine;

[CreateAssetMenu(fileName = "PlayerEconomy", menuName = "Player/Economy")]
public class PlayerEconomy : ScriptableObject
{
    [SerializeField] public int startingCoal = 10;
    [SerializeField] public int startingIron = 10;
    [SerializeField] public int coalCount = 0;
    [SerializeField] public int ironCount = 0;

    public bool Pay(int coalPrice, int ironPrice)
    {
        if (coalPrice <= coalCount && ironPrice <= ironCount)
        {
            coalCount -= coalPrice;
            ironCount -= ironPrice;
            return true;
        }
        else return false;
    }
    public void AddCoal(int value)
    {
        if(value > 0)
            coalCount += value;
    }
    public void AddIron(int value)
    {
        if (value > 0)
            ironCount += value;
    }
    public void ResetEconomy() {
        coalCount = startingCoal;
        ironCount = startingIron;
    }
}
