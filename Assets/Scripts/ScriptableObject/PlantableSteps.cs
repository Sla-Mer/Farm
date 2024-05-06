using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Plantable steps", menuName = "Plantable steps", order = 51)]
public class PlantableSteps: ScriptableObject
{
    public ItemType itemType;
    public List<StepData> steps;

    public GameObject crop;

    public int cropCount;

    public int fertilizedCount;
}
