using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private bool drawGizmos = true;
    [SerializeField] private GameObject GameAreaCenter;
    [SerializeField] private RectTransform gameAreaTransform, bombMeter;
    [SerializeField] private RectTransform breakpoint1, breakpoint2, bombFill;
    [SerializeField] private Image bombFillImage;
    [SerializeField] private Color[] bombMeterColors;

    [SerializeField] private RectTransform lifeParent;
    [SerializeField] private Image lifeImage;
    [SerializeField] private RectTransform emptyLifeParent;
    [SerializeField] private Image emptyLifeImage;

    [SerializeField] private TMP_Text powerText, highscoreText, scoreText;
    private float minGameX, maxGameX, minGameY, maxGameY;
    private float minBombY, maxBombY, bombHeight, bombX;
    private List<Image> lifeList = new List<Image>();

    private static UIManager _instance;

    public static UIManager Instance //Singleton Stuff
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("UIManager is Null");
            }
            return _instance;
        }
    }

    [ExecuteInEditMode]
    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            SetAreaData();
            Gizmos.color = Color.white;
            var center = GetGameCenter();
            var size = GetGameSize();
            Gizmos.DrawWireCube(center, size);
            if (GameAreaCenter)
            {
                GameAreaCenter.transform.position = center;
            }
        }
    }

    private void Awake()
    {
        _instance = this;
        SetAreaData();
    }

    private void Start()
    {
        SetAreaData();
        SetBombData();
        SetLifeData();
    }

    public void GetGameCorners()
    {
        Vector3[] v = new Vector3[4];
        gameAreaTransform.GetWorldCorners(v);

        Debug.Log("World Corners");
        for (var i = 0; i < 4; i++)
        {
            Debug.Log("World Corner " + i + " : " + v[i]);
        }
    }

    public Vector2 GetGameCenter()
    {
        var x = (maxGameX - minGameX) / 2 + minGameX;
        var y = (maxGameY - minGameY) / 2 + minGameY;
        return new Vector2(x, y);
    }

    public Vector2 GetGameSize()
    {
        var x = maxGameX - minGameX;
        var y = maxGameY - minGameY;
        return new Vector2(x, y);
    }

    private void SetAreaData()
    {
        Vector3[] v = new Vector3[4];
        gameAreaTransform.GetWorldCorners(v);
        Vector2 min = v[0];
        Vector2 max = v[2];
        min = Camera.main.ScreenToWorldPoint(min);
        max = Camera.main.ScreenToWorldPoint(max);
        minGameX = min.x;
        minGameY = min.y;
        maxGameX = max.x;
        maxGameY = max.y;
    }

    private void SetBombData()
    {
        Vector3[] corners = new Vector3[4];
        bombMeter.GetWorldCorners(corners);
        bombX = bombMeter.transform.position.x;
        minBombY = corners[0].y;
        maxBombY = corners[2].y;
        bombHeight = maxBombY - minBombY;
        breakpoint1.transform.position = new Vector2(bombX, minBombY + GameManager.Instance.BombBreakPoint1 * bombHeight);
        breakpoint2.transform.position = new Vector2(bombX, minBombY + GameManager.Instance.BombBreakPoint2 * bombHeight);
    }

    public void SetBombFill(float percent, int bombStage)
    {
        Rect rect = bombFill.rect;
        rect.height = percent * bombHeight;
        bombFill.sizeDelta = new Vector2(rect.width, rect.height);
        bombFillImage.color = bombMeterColors[bombStage];
    }

    private void SetLifeData()
    {
        for (int i = 0; i < GameManager.Instance.StartingLives - 1; i++)
        {
            AddLife();
        }
        for (int i = 0; i < GameManager.Instance.MaxLives - 1; i++)
        {
            AddEmptyLife();
        }
    }

    private void AddLife()
    {
        Image image = Instantiate(lifeImage, lifeParent);
        lifeList.Add(image);
    }

    private void AddEmptyLife()
    {
        Instantiate(emptyLifeImage, emptyLifeParent);
    }

    public void GainLife()
    {
        if (lifeList.Count < GameManager.Instance.MaxLives)
        {
            AddLife();
        }
    }

    public void LoseLife()
    {
        if (lifeList.Count > 0)
        {
            Image image = lifeList[lifeList.Count - 1];
            lifeList.RemoveAt(lifeList.Count - 1);
            Destroy(image.gameObject);
        }
    }

    public bool CheckInGameBounds(Vector2 pos, float offset)
    {
        return pos.x <= maxGameX + offset && pos.x >= minGameX - offset &&
       pos.y <= maxGameY + offset && pos.y >= minGameY - offset;
    }

    public float GetMinGameX()
    {
        return minGameX;
    }

    public float GetMaxGameX()
    {
        return maxGameX;
    }

    public float GetMinGameY()
    {
        return minGameY;
    }

    public float GetMaxGameY()
    {
        return maxGameY;
    }

    public void UpdatePower(int power, int breakpoint)
    {
        powerText.text = "Power " + power + " / " + breakpoint;
    }

    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString("N0");
    }

    public void UpdateHighscore(int highscore)
    {
        highscoreText.text = highscore.ToString("N0");
    }
}