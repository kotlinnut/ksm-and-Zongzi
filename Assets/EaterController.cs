using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class EaterController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("��ͼ����")]
    public Sprite normalSprite;
    public Sprite eatingSprite;
    public float eatingDuration = 2.85f;

    [Header("����������")]
    public LayerMask zongziLayer;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Camera mainCamera;
    private Vector3 offset;
    private Vector3? targetPosition = null;

    private Coroutine eatingRoutine;
    public AudioClip spawnSound; // biu ��Ч
    private AudioSource audioSource;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0f;
        rb.freezeRotation = true;

        if (TryGetComponent<Collider2D>(out var col))
        {
            col.isTrigger = false;
        }
        else
        {
            gameObject.AddComponent<BoxCollider2D>();
        }

        spriteRenderer.sprite = normalSprite;
    }

    #region ��ק�ӿ�ʵ��
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

    private void FixedUpdate()
    {
        if (targetPosition.HasValue)
        {
            rb.MovePosition(targetPosition.Value);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & zongziLayer) != 0 && other.CompareTag("Zongzi"))
        {
            // ֻ����һ��Э�̣���Ҫ����ظ�����
            StartCoroutine(EatZongzi(other.gameObject));
        }
    }

    private IEnumerator EatZongzi(GameObject zongzi)
    {
        var zongziBehaviour = zongzi.GetComponent<ZongziBehaviour>();
        if (zongziBehaviour == null || zongziBehaviour.IsEaten)
            yield break;

        if (eatingRoutine != null)
        {
            StopCoroutine(eatingRoutine);
        }

        spriteRenderer.sprite = eatingSprite;

        ZongziCounterTMP.Instance?.AddCount();

        zongziBehaviour.BeEaten();

        eatingRoutine = StartCoroutine(ResetSpriteAfterDelay());

        yield return null;
    }


    private IEnumerator ResetSpriteAfterDelay()
    {
        yield return new WaitForSeconds(eatingDuration);
        spriteRenderer.sprite = normalSprite;
        eatingRoutine = null;
    }
}
