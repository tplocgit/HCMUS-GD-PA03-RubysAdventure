using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class MyGameManager
{
    private static MyGameManager _instance;

    public static MyGameManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new MyGameManager();
            }
            return _instance;
        }
    }

    public bool isNewGame = false;
    public int saveType = 0;

    public string savePath = "";

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    
    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    private RubyData GetRubyData(GameObject player, GameObject npc)
    {
        RubyController rubyController = player.GetComponent<RubyController>();
        NPC npcCtrl = npc.GetComponent<NPC>();
        CogAmmoController cogAmmoController = player.GetComponent<CogAmmoController>();
        RubyData data = new RubyData(
            rubyController.Health, cogAmmoController.Ammo, player.transform.position,
            rubyController.destroyed, rubyController.enemyPositions, npcCtrl.isGivingMission,
            npcCtrl.missionAccepted
            );

        return data;
    }

    public void SaveGame(GameObject player, GameObject npc)
    {
        string path = Path.Combine(Application.persistentDataPath, $"{System.DateTime.Now.ToString("yyyyMMddTHHmmssZ")}.hd");
        FileStream file = File.Create(path);
        RubyData data = GetRubyData(player, npc);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(file, data);
        file.Close();
        Debug.Log($"Saved: {path}");
    }

    public void SaveGameJson(GameObject player, GameObject npc)
    {
        string path = Path.Combine(Application.persistentDataPath, $"{System.DateTime.Now.ToString("yyyyMMddTHHmmssZ")}.json");
        FileStream file = File.Create(path);
        file.Close();
        RubyData data = GetRubyData(player, npc);
        string strData = JsonUtility.ToJson(data);
        File.WriteAllText(path, strData);
        Debug.Log($"Saved: {path}");
    }

    
    public void LoadGameRuby(GameObject player)
    {
        string path = this.savePath;
        if(!File.Exists(path)) return;
        FileStream file = File.Open(path, FileMode.Open);
        BinaryFormatter formatter = new BinaryFormatter();
        RubyData data = (RubyData) formatter.Deserialize(file);
        file.Close();
        
        RubyController rubyController = player.GetComponent<RubyController>();
        CogAmmoController cogAmmoController = player.GetComponent<CogAmmoController>();
        rubyController.Health = data.hp;
        player.transform.position = new Vector3(data.x, data.y, 0);
        cogAmmoController.Ammo = data.ammo;
        rubyController.destroyed = new List<string>();
        for(int i = 0; i < data.destroyed.Length; i++)
        {
            rubyController.destroyed.Add(data.destroyed[i]);
        }

        Debug.Log($"Loaded: {path}");
    }

    public void LoadGameRubyJson(GameObject player)
    {
        string path = this.savePath;
        if(!File.Exists(path)) return;
        string json = File.ReadAllText(path);
        RubyData data = JsonUtility.FromJson<RubyData>(json);
        
        RubyController rubyController = player.GetComponent<RubyController>();
        CogAmmoController cogAmmoController = player.GetComponent<CogAmmoController>();
        rubyController.Health = data.hp;
        player.transform.position = new Vector3(data.x, data.y, 0);
        cogAmmoController.Ammo = data.ammo;
        rubyController.destroyed = new List<string>();
        for(int i = 0; i < data.destroyed.Length; i++)
        {
            rubyController.destroyed.Add(data.destroyed[i]);
        }

        Debug.Log($"Loaded: {path}");
    }

    public void LoadGameNPC(GameObject npc)
    {
        string path = this.savePath;
        if(!File.Exists(path)) return;
        FileStream file = File.Open(path, FileMode.Open);
        BinaryFormatter formatter = new BinaryFormatter();
        RubyData data = (RubyData) formatter.Deserialize(file);
        file.Close();
        NPC npcCtrl = npc.GetComponent<NPC>();
        npcCtrl.LoadGame(data.enemyPostitions, data.givingMission, data.missionAccepted);
    }

    public void LoadGameNPCJson(GameObject npc)
    {
        string path = this.savePath;
        if(!File.Exists(path)) return;
        string json = File.ReadAllText(path);
        RubyData data = JsonUtility.FromJson<RubyData>(json);
        NPC npcCtrl = npc.GetComponent<NPC>();
        npcCtrl.LoadGame(data.enemyPostitions, data.givingMission, data.missionAccepted);
    }
}
