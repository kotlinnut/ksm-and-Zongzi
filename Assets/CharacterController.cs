using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [Header("��ë����")]
    public Collider2D hairTrigger; // ��ë��ײ��
    public float hairClickCooldown = 0.0f; // �����ȴʱ��
    private float lastClickTime;

    [Header("��������")]
    public GameObject zongziPrefab;
    public Transform zongziSpawnPoint; // ��������λ��
    public float spawnForce = 5f; // ��������

    private void Update()
    {
        // ������
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // ����Ƿ����˴�ë
            if (hairTrigger.OverlapPoint(mousePos) && Time.time > lastClickTime + hairClickCooldown)
            {
                lastClickTime = Time.time;
                SpawnZongzi();
            }
        }
    }

    void SpawnZongzi()
    {
        // ʵ��������
        GameObject newZongzi = Instantiate(zongziPrefab, zongziSpawnPoint.position, Quaternion.identity);

        // ��������������
        Rigidbody2D rb = newZongzi.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 randomDirection = new Vector2(
                Random.Range(-0.5f, 0.5f),
                Random.Range(0.7f, 1f)
            ).normalized;

            rb.AddForce(randomDirection * spawnForce, ForceMode2D.Impulse);
        }

        // �����ת
        float randomRotation = Random.Range(-15f, 15f);
        newZongzi.transform.Rotate(0, 0, randomRotation);
    }

}