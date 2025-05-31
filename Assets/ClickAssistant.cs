using UnityEngine;

public class ClickAssistant : MonoBehaviour
{
    [Header("����")]
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
                // ��ȫ���ʷ�ʽ
                if (hairController != null)
                {
                    hairController.HandleClick();
                }
                else
                {
                    Debug.LogWarning("HairControllerδ����!", this);
                }
            }
        }
    }
}