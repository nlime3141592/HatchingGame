using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // public static HatchingFile s_hatchingFile;

    public static HatchingData s_currentHatchingDataOrNull;
    public static long s_currentTimestamp;
    public static HatchingStatus s_currentHatchingStatus;

    public static HatchingDataSO s_currentHatchingDataSO;

    public static bool s_shouldManagingFlag;

    public static List<HatchingDataSO> s_hatchingDataSO;

    public float autoSavePeriod = 60.0f;

    public List<HatchingDataSO> hatchingDataSO;

    public HatchingStatus debug_hatchingStatus;

    private float leftAutoSaveTime;

    private string hatchingFilePath;

    // private Thread loadThread;
    // private Thread saveThread;

    private void Awake()
    {
        leftAutoSaveTime = autoSavePeriod;

        s_hatchingDataSO = hatchingDataSO;

        // hatchingFilePath = Application.persistentDataPath + "/data/hatching_data_0.dat";

        // LoadGameData();
    }

    private void Update()
    {
        UpdateHatchingState();
        UpdateManagingEgg();
        // UpdateAutoSave();
    }

    private void UpdateHatchingState()
    {
        // s_currentHatchingDataOrNull = GetCurrentHatchingEggOrNull();
        s_currentTimestamp = GetCurrentTimestamp();

        if (s_currentHatchingDataOrNull == null)
        {
            s_currentHatchingStatus = HatchingStatus.None;
        }
        else if (s_currentHatchingDataOrNull.hatchingStatus == HatchingStatus.Hatching)
        {
            if (s_currentTimestamp >= s_currentHatchingDataOrNull.hatchingEndTime)
            {
                s_currentHatchingStatus = HatchingStatus.HatchingSuccess;
                s_currentHatchingDataOrNull.hatchingStatus = s_currentHatchingStatus;
                BreakEgg();
            }
            else if (s_currentTimestamp >= s_currentHatchingDataOrNull.managingEndTime)
            {
                s_currentHatchingStatus = HatchingStatus.HatchingFailure;
                s_currentHatchingDataOrNull.hatchingStatus = s_currentHatchingStatus;
                ThrowEgg();
            }
            else
                s_currentHatchingStatus = HatchingStatus.Hatching;
        }

        Debug.Log(s_currentHatchingStatus);
    }
    /*
    private void UpdateAutoSave()
    {
        if (leftAutoSaveTime > 0.0f)
        {
            leftAutoSaveTime -= Time.deltaTime;
            return;
        }
        else
        {
            SaveGameData();
        }
    }
    
    public void LoadGameData()
    {
        if (loadThread != null)
            return;

        loadThread = new Thread(new ThreadStart(this.OnLoadHatchingFile));
        loadThread.Start();
    }

    public void SaveGameData()
    {
        leftAutoSaveTime = autoSavePeriod;

        if (saveThread != null)
            return;

        saveThread = new Thread(new ThreadStart(this.OnSaveHatchingFile));
        saveThread.Start();
    }

    private void OnLoadHatchingFile()
    {
        s_hatchingFile = HatchingFile.LoadFile(this.hatchingFilePath);
        loadThread = null;
    }
    
    private void OnSaveHatchingFile()
    {
        try
        {
            s_hatchingFile.SaveFile(this.hatchingFilePath);
        }
        catch (Exception)
        {
            Debug.Log("세이브 실패");
        }
        finally
        {
            saveThread = null;
        }
    }
    */
    private long GetCurrentTimestamp()
    {
        DateTime dateTime = DateTime.UtcNow;
        long ticksPerSecond = 10000000;
        return dateTime.Ticks / ticksPerSecond;
    }
    /*
    private static HatchingData GetCurrentHatchingEggOrNull()
    {
        List<HatchingData> hatchingDatas = s_hatchingFile.HatchingDatas;

        for (int i = hatchingDatas.Count - 1; i >= 0; --i)
        {
            HatchingData hatchingData = hatchingDatas[i];

            if (hatchingData.hatchingStatus == HatchingStatus.Hatching ||
                hatchingData.hatchingStatus == HatchingStatus.HatchingSuccess ||
                hatchingData.hatchingStatus == HatchingStatus.HatchingFailure
                )
            {
                return hatchingData;
            }
        }

        return null;
    }
    */
    private void FetchEgg(HatchingData hatchingData)
    {
        hatchingData.hatchingStatus = HatchingStatus.Hatching;
        hatchingData.hatchingEndTime = s_currentTimestamp + hatchingData.hatchingRequireTime;
        hatchingData.managingEndTime = s_currentTimestamp + hatchingData.managingRequireTime;

        // SaveGameData();
    }

    public static void ManagingEgg()
    {
        if (s_currentHatchingStatus != HatchingStatus.Hatching)
            return;

        s_shouldManagingFlag = true;
    }

    private void UpdateManagingEgg()
    {
        if (!s_shouldManagingFlag)
            return;

        s_shouldManagingFlag = false;
        HatchingData hatchingData = s_currentHatchingDataOrNull;

        hatchingData.managingEndTime = s_currentTimestamp + hatchingData.managingRequireTime;

        // SaveGameData();
    }

    private void BreakEgg()
    {
        HatchingData hatchingData = s_currentHatchingDataOrNull;

        float w01 = UnityEngine.Random.value;
        float min = hatchingData.bodyLengthMax;
        float max = hatchingData.bodyLengthMax;
        float bodyLength = min + (max - min) * w01;

        hatchingData.bodyLength = bodyLength;

        // SaveGameData();
    }

    private void ThrowEgg()
    {
        HatchingData hatchingData = s_currentHatchingDataOrNull;
    }
}