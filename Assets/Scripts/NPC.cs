using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class NPC : MonoBehaviour
{
    public float displayTime = 4.0f;
    public GameObject dialogBox;
    GameObject missionMark;
    GameObject missionItem;
    float timerDisplay;
    public GameObject clockPrefab;
    public static int MaxEnemyCount = 4;
    int enemyCount = MaxEnemyCount;
    public int Count { get { return enemyCount; }}
    int enemyFixed = 0;
    TMP_Text missionText;
    public AudioClip missionCompleteClip;
    public AudioClip missionAcceptedClip;
    public List<Vector3> enemyPositions = new List<Vector3>(){
        new Vector3(0, 0, 0),
        new Vector3(5, 5, 0),
        new Vector3(5, -3, 0),
        new Vector3(-5, -5, 0)
    };

    bool missionAccepted = false;

    GameObject introCanvas;

    public float introTextTimmer = 3f;

    bool isGivingMission = true;
    void Start()
    {
        dialogBox.SetActive(false);
        timerDisplay = -1.0f;
        missionMark = GameObject.Find("MissionMark");
        missionItem = GameObject.Find("MissionItem");
        missionText = GameObject.Find("MissionFixRobot").GetComponent<TextMeshProUGUI>();
        introCanvas = GameObject.Find("IntroCanvas");
        missionItem.SetActive(false);
    }
    
    void Update()
    {
        if (timerDisplay >= 0)
        {
            timerDisplay -= Time.deltaTime;
            if (timerDisplay < 0)
            {
                if(isGivingMission) {
                    ChangeNPCDialogText("Here is your ammo. Please help us.");
                    isGivingMission = false;
                }
                dialogBox.SetActive(false);
            }
        }

        if (introTextTimmer >= 0) {
            introTextTimmer -= Time.deltaTime;
            if(introTextTimmer < 0) {
                introCanvas.SetActive(false);
            }
        }
    }

    public void RubyPlaySound(AudioClip clip) {
        RubyController rubyCtrl = GameObject.Find("ruby").GetComponent<RubyController>();
        if(rubyCtrl) {
            rubyCtrl.PlaySound(clip);
        }
    }

    void CreateEnemy(List<Vector3> positions) {
        foreach(Vector3 pos in positions) {
            Instantiate(clockPrefab, pos, Quaternion.identity);
        }
    }
    
    public void DisplayDialog()
    {
        timerDisplay = displayTime;
        dialogBox.SetActive(true);
        missionMark.SetActive(false);
        missionItem.SetActive(true);
        CogAmmoController ctrl = GameObject.Find("ruby").GetComponent<CogAmmoController>();
        if(ctrl) {
            ctrl.UpdateAmmo(1);
        }
        if(!missionAccepted) {
            ctrl.UpdateAmmo(2);
            missionAccepted = true;
            RubyPlaySound(missionAcceptedClip);
            CreateEnemy(enemyPositions);
        }
    }

    void ChangeNPCDialogText(string text) {
        TMP_Text dialogTextGO = GameObject.Find("NPCDialogText").GetComponent<TextMeshProUGUI>();
        dialogTextGO.text = text;
    }

    public void updateEnemy(int count) {
        enemyFixed -= count;
        missionText.text = $"Fix Broken Mr.Clocks: {enemyFixed}/{MaxEnemyCount}";
        if(enemyFixed == MaxEnemyCount) {
            missionText.color = Color.green;
            RubyPlaySound(missionCompleteClip);
            dialogBox.SetActive(true);
            ChangeNPCDialogText("Thank you adventure!! You fixed all Mr.Clock.");
        }
    }
}
