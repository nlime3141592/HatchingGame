using UnityEngine;

[CreateAssetMenu(fileName ="HatchingDataSO", menuName = "HatchingDataSO")]
public class HatchingDataSO : ScriptableObject
{
    public GameObject eggPrefabInWorld;
    public Sprite bugImage;
    public HatchingData hatchingData;
}