using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class ZongziBehaviour : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("物理设置")]
    public float rotationSpeed = 100f;
    public float minCollisionForce = 1f;
    public float bounceFactor = 0.5f;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Collider2D physicalCollider;
    private Collider2D triggerCollider;

    private Camera mainCamera;
    private Vector3 offset;
    private Vector3? targetPosition = null;

    private bool isEaten = false;
    public bool IsEaten => isEaten;

    private void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Zongzi");
        gameObject.tag = "Zongzi";

        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        rb.gravityScale = 0;
        rb.angularVelocity = Random.Range(-rotationSpeed, rotationSpeed);

        physicalCollider = GetComponent<Collider2D>();
        physicalCollider.isTrigger = false;

        // 添加额外 Trigger 用于检测吃掉
        triggerCollider = gameObject.AddComponent<CircleCollider2D>();
        triggerCollider.isTrigger = true;
        ((CircleCollider2D)triggerCollider).radius = 2.25f;

        mainCamera = Camera.main;

    }

    private void FixedUpdate()
    {
        if (targetPosition.HasValue)
        {
            rb.MovePosition(targetPosition.Value);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.magnitude > minCollisionForce)
        {
            Vector2 force = collision.relativeVelocity * rb.mass * bounceFactor;
            rb.AddForce(force, ForceMode2D.Impulse);
        }
    }

    public void BeEaten()
    {
        if (isEaten) return;
        isEaten = true;

        // 播放吃粽子音效
        AudioManager.Instance?.PlayChi();

        Destroy(gameObject);
    }

    #region 拖拽接口实现
    public void OnBeginDrag(PointerEventData eventData)
    {
        Vector3 clickWorldPos = mainCamera.ScreenToWorldPoint(
            new Vector3(eventData.position.x, eventData.position.y, mainCamera.nearClipPlane));
        offset = transform.position - clickWorldPos;
        offset.z = 0;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(
            new Vector3(eventData.position.x, eventData.position.y,
            mainCamera.WorldToScreenPoint(transform.position).z));

        worldPos += offset;
        worldPos.z = 0;

        targetPosition = worldPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        targetPosition = null;
    }
    #endregion
}
