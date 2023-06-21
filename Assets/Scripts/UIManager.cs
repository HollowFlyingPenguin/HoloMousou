using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private bool drawGizmos = true;
    [SerializeField] private GameObject GameAreaCenter;
    [SerializeField] private RectTransform gameAreaTransform;
    private float minGameX, maxGameX, minGameY, maxGameY;

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
    }

    private void Start()
    {
        SetAreaData();
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