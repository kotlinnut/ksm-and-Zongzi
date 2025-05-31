using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFitToContent : MonoBehaviour
{
    [Header("Settings")]
    public float padding = 0.5f; // ������Χ�Ķ���ռ�
    public bool runOnAwake = true;
    public bool runOnUpdate = false;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        if (runOnAwake) FitCameraToContent();
    }

    private void Update()
    {
        if (runOnUpdate) FitCameraToContent();
    }

    // �ֶ����ô˷������������
    public void FitCameraToContent()
    {
        // ȷ�����������
        cam.orthographic = true;

        // ��ȡ����2D���ݵı߽�
        Bounds contentBounds = CalculateContentBounds();

        // ���û���κ����ݣ�ʹ��Ĭ�ϴ�С
        if (contentBounds.size == Vector3.zero)
        {
            cam.orthographicSize = 5f;
            return;
        }

        // ������Ļ��߱�
        float screenRatio = (float)Screen.width / Screen.height;
        float contentRatio = contentBounds.size.x / contentBounds.size.y;

        // ���padding
        contentBounds.Expand(padding * 2);

        // ���������С
        if (screenRatio >= contentRatio)
        {
            // �Ը߶�Ϊ׼
            cam.orthographicSize = contentBounds.extents.y;
        }
        else
        {
            // �Կ��Ϊ׼��������Ļ����
            cam.orthographicSize = contentBounds.extents.x / screenRatio;
        }

        // ��λ�������������
        transform.position = new Vector3(
            contentBounds.center.x,
            contentBounds.center.y,
            transform.position.z
        );
    }

    // ��������2D��Ⱦ���ݵı߽�
    private Bounds CalculateContentBounds()
    {
        // ��ȡ������Ⱦ������ײ��
        Renderer[] renderers = FindObjectsOfType<Renderer>();
        Collider2D[] colliders = FindObjectsOfType<Collider2D>();

        if (renderers.Length == 0 && colliders.Length == 0)
            return new Bounds(Vector3.zero, Vector3.zero);

        // ��ʼ���߽�
        Bounds bounds = new Bounds();
        bool hasBounds = false;

        // ����������Ⱦ��
        foreach (Renderer renderer in renderers)
        {
            if (!hasBounds)
            {
                bounds = new Bounds(renderer.bounds.center, renderer.bounds.size);
                hasBounds = true;
            }
            else
            {
                bounds.Encapsulate(renderer.bounds);
            }
        }

        // ����������ײ��
        foreach (Collider2D collider in colliders)
        {
            if (!hasBounds)
            {
                bounds = new Bounds(collider.bounds.center, collider.bounds.size);
                hasBounds = true;
            }
            else
            {
                bounds.Encapsulate(collider.bounds);
            }
        }

        return bounds;
    }
}