using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class NPCData
{
    public float[,] enemyPostitions;
    public bool givingMission;

    public bool missionAccepted;
    public NPCData(Vector3[] ePositions, bool isGivingMission, bool isMissionAccepted)
    {
        this.missionAccepted = isMissionAccepted;
        this.givingMission = isGivingMission;
        this.enemyPostitions = new float[ePositions.Length, 2];
        for(int i = 0; i < ePositions.Length; i++)
        {
            this.enemyPostitions[i,0] = ePositions[i].x;
            this.enemyPostitions[i,1] = ePositions[i].y;
        }
    }
}