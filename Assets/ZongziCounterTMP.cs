using UnityEngine;
using TMPro;  // 记得引用TMP命名空间

public class ZongziCounterTMP : MonoBehaviour
{
    public static ZongziCounterTMP Instance;

    public TextMeshProUGUI counterText;  // TMP文本组件

    private int eatenCount = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        UpdateCounter();
     
    }

    public void AddCount(int amount = 1)
    {
        eatenCount += amount;
        UpdateCounter();
    }

    private void UpdateCounter()
    {
            counterText.text = "Score: " + eatenCount;
        if (eatenCount != 0)
        {
            // 播放音效
            AudioManager.Instance?.PlayChi();
        }
      
    }
}
