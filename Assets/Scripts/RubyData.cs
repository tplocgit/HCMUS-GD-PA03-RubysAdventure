using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class RubyData
{
    public int hp;
    public int ammo;
    public float x;
    public float y;

    public string[] destroyed;
    public float[,] enemyPostitions;
    public bool givingMission;

    public bool missionAccepted;
    public RubyData(int hp, int ammo, Vector2 position, List<string> destroyed, List<Vector3> ePositions, bool isGivingMission, bool isMissionAccepted)
    {
        this.hp = hp;
        this.ammo = ammo;
        this.x = position.x;
        this.y = position.y;
        this.destroyed = new string[destroyed.Count];
        for(int i = 0; i < destroyed.Count; i++)
            this.destroyed[i] = destroyed[i];
        this.missionAccepted = isMissionAccepted;
        this.givingMission = isGivingMission;
        this.enemyPostitions = new float[ePositions.Count, 2];
        for(int i = 0; i < ePositions.Count; i++)
        {
            this.enemyPostitions[i,0] = ePositions[i].x;
            this.enemyPostitions[i,1] = ePositions[i].y;
        }
    }
}