using System;
using UnityEngine;

[Serializable]
public class HatchingData
{
    [HideInInspector] public long recordNumber;

    public int eggType;
    public int bugType;
    public long hatchingRequireTime;
    public long managingRequireTime;

    [HideInInspector] public long hatchingEndTime;
    [HideInInspector] public long managingEndTime;

    public float bodyLengthMin;
    public float bodyLengthMax;
    [HideInInspector] public float bodyLength;

    [HideInInspector] public HatchingStatus hatchingStatus;
}
