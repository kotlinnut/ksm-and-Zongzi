using UnityEngine;

public class ClickAssistant : MonoBehaviour
{
    [Header("设置")]
    public HairController hairController;
    public float assistRadius = 1f;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float distance = Vector2.Distance(mousePos, transform.position);

            if (distance < assistRadius)
            {
                // 安全访问方式
                if (hairController != null)
                {
                    hairController.HandleClick();
                }
                else
                {
                    Debug.LogWarning("HairController未分配!", this);
                }
            }
        }
    }
}