using UnityEngine;

public class HairController : MonoBehaviour
{
    [Header("��������")]
    public GameObject zongziPrefab;  // ����Ԥ����
    public Transform spawnPoint;     // ��������λ��
    public float spawnForce = 200f;    // �������ȣ�����ܶ�
    public float cooldown = 0f;    // �����ȴʱ��

    private bool isMouseOver = false;
    [Header("�������")]
    private float lastClickTime = 0f;  // ȷ���������ʼ��  

    private void Start()
    {
        // ��ȫ��ʼ��
        lastClickTime = -cooldown; // ��֤��Ϸ��ʼʱ�����������
    }

    private void OnMouseDown()
    {
        SpawnZongzi();
        // ����������Ч
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
            Debug.LogError("����Ԥ��������ɵ�δ���ã�");
            return;
        }

        GameObject newZongzi = Instantiate(zongziPrefab, spawnPoint.position, Quaternion.identity);

        Rigidbody2D rb = newZongzi.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // X������С�������Y�����ϴ�������
            Vector2 randomDirection = new Vector2(
                Random.Range(-0.5f, 0.5f),  // ˮƽ����С���
                Random.Range(0.7f, 1f)      // ��ֱ��������
            ).normalized;

            rb.AddForce(randomDirection * spawnForce, ForceMode2D.Impulse);
        }

        newZongzi.transform.Rotate(0, 0, Random.Range(-15f, 15f));
    }

    [Header("�������")]
    [SerializeField] private float _lastClickTime = Mathf.NegativeInfinity;  // ����ȫ�ĳ�ʼ����ʽ

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
