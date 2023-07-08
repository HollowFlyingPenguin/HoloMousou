using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EffectsManager : MonoBehaviour
{
    [SerializeField] Transform UIEffectsParent;
    [SerializeField] GameObject scoreGetText;

    private static EffectsManager _instance;

    public static EffectsManager Instance //Singleton Stuff
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("EffectsManager is Null");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    public void SpawnScoreText(Vector2 pos, int amount)
    {
        GameObject obj = EffectsPoolManager.Instance.InitializeObject(scoreGetText);
        obj.transform.parent.SetParent(UIEffectsParent, false);
        obj.transform.position = pos;
        //obj.transform.position = Camera.main.WorldToScreenPoint(pos);
        TMP_Text text = obj.GetComponent<TMP_Text>();
        text.text = amount.ToString("N0");
    }
}
