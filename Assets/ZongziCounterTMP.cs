using UnityEngine;
using TMPro;  // �ǵ�����TMP�����ռ�

public class ZongziCounterTMP : MonoBehaviour
{
    public static ZongziCounterTMP Instance;

    public TextMeshProUGUI counterText;  // TMP�ı����

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
            // ������Ч
            AudioManager.Instance?.PlayChi();
        }
      
    }
}
