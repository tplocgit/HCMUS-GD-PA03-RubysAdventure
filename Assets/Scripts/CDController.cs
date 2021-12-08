using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CDController : MonoBehaviour
{

    Image cogMaskCd;

    TMP_Text cogCdText;

    bool isCd = false;
    float cogCdTimer = 0.0f;
    public static float cogCdTime = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        cogCdText = GameObject.Find("UIText").GetComponent<TextMeshProUGUI>();
        cogMaskCd = GameObject.Find("CDMask").GetComponent<Image>();

        cogCdText.gameObject.SetActive(isCd);
        cogMaskCd.fillAmount = 0.0f;
    }

    void CogCdTimeOutEffect() {
        isCd = false;
        cogCdText.gameObject.SetActive(isCd);
        cogMaskCd.fillAmount = 0.0f;
        cogCdTimer = 0f;
    }

    void CogCdStartEffect() {
        isCd = true;
        cogCdText.gameObject.SetActive(isCd);
        cogCdTimer = cogCdTime;
        cogMaskCd.fillAmount = 1f;
    }

    // Update is called once per frame
    void Update()
    {   
        if(isCd)
            cogCdTimer -= Time.deltaTime;

        if(cogCdTimer <= 0) {
            CogCdTimeOutEffect();
        }
        else {
            cogCdText.text = cogCdTimer.ToString("0.0");
            cogMaskCd.fillAmount = cogCdTimer / cogCdTime;
        }
    }

    public bool ThrowCogCdEffect() {
        if(isCd) return false;

        CogCdStartEffect();

        return true;
    }
}
