using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private bool drawGizmos = true;
    [SerializeField] private GameObject GameAreaCenter;
    [SerializeField] private RectTransform gameAreaTransform, bombMeter;
    [SerializeField] private RectTransform breakpoint1, breakpoint2, bombFill;
    private float minGameX, maxGameX, minGameY, maxGameY;
    private float minBombY, maxBombY, bombHeight, bombX;

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

    public void SetFillMeter(float percent)
    {
        Rect rect = bombFill.rect;
        rect.height = percent * bombHeight;
        bombFill.sizeDelta = new Vector2(rect.width, rect.height);
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
}