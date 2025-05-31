using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFitToContent : MonoBehaviour
{
    [Header("Settings")]
    public float padding = 0.5f; // 内容周围的额外空间
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

    // 手动调用此方法来调整相机
    public void FitCameraToContent()
    {
        // 确保是正交相机
        cam.orthographic = true;

        // 获取所有2D内容的边界
        Bounds contentBounds = CalculateContentBounds();

        // 如果没有任何内容，使用默认大小
        if (contentBounds.size == Vector3.zero)
        {
            cam.orthographicSize = 5f;
            return;
        }

        // 计算屏幕宽高比
        float screenRatio = (float)Screen.width / Screen.height;
        float contentRatio = contentBounds.size.x / contentBounds.size.y;

        // 添加padding
        contentBounds.Expand(padding * 2);

        // 调整相机大小
        if (screenRatio >= contentRatio)
        {
            // 以高度为准
            cam.orthographicSize = contentBounds.extents.y;
        }
        else
        {
            // 以宽度为准，考虑屏幕比例
            cam.orthographicSize = contentBounds.extents.x / screenRatio;
        }

        // 定位相机在内容中心
        transform.position = new Vector3(
            contentBounds.center.x,
            contentBounds.center.y,
            transform.position.z
        );
    }

    // 计算所有2D渲染内容的边界
    private Bounds CalculateContentBounds()
    {
        // 获取所有渲染器和碰撞体
        Renderer[] renderers = FindObjectsOfType<Renderer>();
        Collider2D[] colliders = FindObjectsOfType<Collider2D>();

        if (renderers.Length == 0 && colliders.Length == 0)
            return new Bounds(Vector3.zero, Vector3.zero);

        // 初始化边界
        Bounds bounds = new Bounds();
        bool hasBounds = false;

        // 包含所有渲染器
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

        // 包含所有碰撞体
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