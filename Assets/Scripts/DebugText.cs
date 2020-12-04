using TMPro;
using UnityEngine;

public class DebugText : MonoBehaviour
{
    private TextMeshProUGUI tmp;

    void Setup()
    {
        tmp = gameObject.AddComponent<TextMeshProUGUI>();
        tmp.rectTransform.pivot = new Vector2(0.0f, 1.0f);
        tmp.rectTransform.anchoredPosition = Vector2.zero; 
        tmp.rectTransform.anchoredPosition3D = new Vector3(0,0,10);
        tmp.rectTransform.anchorMin = new Vector2(0, 1);
        tmp.rectTransform.anchorMax = new Vector2(0, 1);
        tmp.rectTransform.position = new Vector3(10f,470f,10f);

        tmp.fontSize = 12f;
        
        tmp.text = "test test";
    }
    void Start()
    {
        Setup();

    }

    private void Update()
    {

    }
}
