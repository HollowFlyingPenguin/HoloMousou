using UnityEngine;
using TMPro;

public class ScoreText : MovementController
{
    [SerializeField] protected float fadeDuration = 3;
    [SerializeField] protected float baseAlpha = 1;
    protected float fadeTimer = 0;
    protected TMP_Text text;

    protected void OnEnable()
    {
        text = GetComponent<TMP_Text>();
        ResetValues();
    }

    public override void ResetValues()
    {
        base.ResetValues();
        fadeTimer = 0;
        SetProportionalAlpha(1);
    }

    protected override void Update()
    {
        base.Update();
        fadeTimer += Time.deltaTime;
        SetProportionalAlpha(fadeTimer / fadeDuration);
        
        if (fadeTimer >= fadeDuration)
        {
            ReturnToPool();
        }
    }

    protected virtual void SetProportionalAlpha(float alpha)
    {
        Color newColor = text.color;
        newColor.a = Mathf.Lerp(baseAlpha, 0, alpha);
        text.color = newColor;
    }
}
