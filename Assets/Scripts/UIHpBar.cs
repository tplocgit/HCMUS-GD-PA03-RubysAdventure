using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHpBar : MonoBehaviour
{
    public static UIHpBar Instance { get; private set; }
    
    public Image mask;
    float originalSize;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        originalSize = mask.rectTransform.rect.width;
        RubyController ruby = GameObject.Find("ruby").GetComponent<RubyController>();
        SetValue(ruby.Health / ruby.maxHealth);
    }

    public void SetValue(float value)
    {				      
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * value);
        Debug.Log($"{mask.rectTransform.rect.width}");
    }
    void Update()
    {
        
    }
}
