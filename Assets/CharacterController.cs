using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [Header("呆毛设置")]
    public Collider2D hairTrigger; // 呆毛碰撞体
    public float hairClickCooldown = 0.0f; // 点击冷却时间
    private float lastClickTime;

    [Header("粽子设置")]
    public GameObject zongziPrefab;
    public Transform zongziSpawnPoint; // 粽子生成位置
    public float spawnForce = 5f; // 弹射力度

    private void Update()
    {
        // 点击检测
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // 检查是否点击了呆毛
            if (hairTrigger.OverlapPoint(mousePos) && Time.time > lastClickTime + hairClickCooldown)
            {
                lastClickTime = Time.time;
                SpawnZongzi();
            }
        }
    }

    void SpawnZongzi()
    {
        // 实例化粽子
        GameObject newZongzi = Instantiate(zongziPrefab, zongziSpawnPoint.position, Quaternion.identity);

        // 给粽子添加随机力
        Rigidbody2D rb = newZongzi.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 randomDirection = new Vector2(
                Random.Range(-0.5f, 0.5f),
                Random.Range(0.7f, 1f)
            ).normalized;

            rb.AddForce(randomDirection * spawnForce, ForceMode2D.Impulse);
        }

        // 随机旋转
        float randomRotation = Random.Range(-15f, 15f);
        newZongzi.transform.Rotate(0, 0, randomRotation);
    }

}