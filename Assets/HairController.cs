using UnityEngine;

public class HairController : MonoBehaviour
{
    [Header("粽子设置")]
    public GameObject zongziPrefab;  // 粽子预制体
    public Transform spawnPoint;     // 粽子生成位置
    public float spawnForce = 200f;    // 弹射力度，调大很多
    public float cooldown = 0f;    // 点击冷却时间

    private bool isMouseOver = false;
    [Header("点击设置")]
    private float lastClickTime = 0f;  // 确保在这里初始化  

    private void Start()
    {
        // 安全初始化
        lastClickTime = -cooldown; // 保证游戏开始时可以立即点击
    }

    private void OnMouseDown()
    {
        SpawnZongzi();
        // 播放生成音效
        AudioManager.Instance?.PlayBiu();
    }

    private void OnMouseEnter()
    {
        isMouseOver = true;
    }

    private void OnMouseExit()
    {
        isMouseOver = false;
    }

    void SpawnZongzi()
    {
        if (zongziPrefab == null || spawnPoint == null)
        {
            Debug.LogError("粽子预制体或生成点未设置！");
            return;
        }

        GameObject newZongzi = Instantiate(zongziPrefab, spawnPoint.position, Quaternion.identity);

        Rigidbody2D rb = newZongzi.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // X轴左右小幅随机，Y轴向上大力弹射
            Vector2 randomDirection = new Vector2(
                Random.Range(-0.5f, 0.5f),  // 水平方向小随机
                Random.Range(0.7f, 1f)      // 竖直方向向上
            ).normalized;

            rb.AddForce(randomDirection * spawnForce, ForceMode2D.Impulse);
        }

        newZongzi.transform.Rotate(0, 0, Random.Range(-15f, 15f));
    }

    [Header("点击设置")]
    [SerializeField] private float _lastClickTime = Mathf.NegativeInfinity;  // 更安全的初始化方式

    public float LastClickTime
    {
        get => _lastClickTime;
        set => _lastClickTime = value;
    }

    public bool CanBeClicked
    {
        get => Time.time > _lastClickTime + cooldown;
    }

    public void HandleClick()
    {
        if (!CanBeClicked) return;

        _lastClickTime = Time.time;
        SpawnZongzi();
    }
}
