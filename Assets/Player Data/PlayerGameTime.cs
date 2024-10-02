using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PlayerGameTime", menuName = "Player/GameTime")]
public class PlayerGameTime : ScriptableObject
{
    public float gameTime = 0;

    public float dayTime = 0;
    public float maxDayTimeInSeconds = 100f;
    public int dayCount = 0;

    public UnityAction onDayOver;
 
    public void UpdateGameTime()
    {
        gameTime += Time.deltaTime;
        dayTime += Time.deltaTime;

        if (dayTime > maxDayTimeInSeconds)
        {
            TriggerDayOver();
            dayTime = 0;
            dayCount++;
        }
    }
    public void ResetGameTime()
    {
        gameTime = 0;
    }
    public void TriggerDayOver()
    {
        Debug.Log("dayover triggered, " + dayCount);
        onDayOver?.Invoke();
    }
}
