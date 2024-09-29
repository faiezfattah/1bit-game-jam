using UnityEngine;

[CreateAssetMenu(fileName = "PlayerEconomy", menuName = "Player/Economy")]
public class PlayerEconomy : ScriptableObject
{
    // TODO: need to notify at changes, use void event 
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
}
