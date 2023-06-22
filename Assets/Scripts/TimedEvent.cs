using UnityEngine.Events;

[System.Serializable]
public class TimedEvent
{
    public bool autoRepeat = false;
    public float delay = 1;
    public float repeatRate = 1;
    public string methodName;
}