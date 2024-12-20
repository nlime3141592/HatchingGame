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

    public HatchingData()
    {

    }

    public HatchingData(HatchingData _source)
    {
        this.eggType = _source.eggType;
        this.bugType = _source.bugType;
        this.hatchingRequireTime = _source.hatchingRequireTime;
        this.managingRequireTime = _source.managingRequireTime;
        this.hatchingEndTime = _source.hatchingEndTime;
        this.managingEndTime = _source.managingEndTime;
        this.bodyLengthMin = _source.bodyLengthMin;
        this.bodyLengthMax = _source.bodyLengthMax;
        this.bodyLength = _source.bodyLength;
        this.hatchingStatus = _source.hatchingStatus;
    }
}
