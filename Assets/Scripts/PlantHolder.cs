
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public  class PlantHolder : MonoBehaviour
{
    public PlantableSteps plantableSteps;

    private int stepIndex;

    public SpriteRenderer spriteRenderer;

    public bool readyToHarvest = false;

    public bool isFertilized = false;

    public int cropCount => plantableSteps.cropCount;
    public int fertilizedCount => plantableSteps.fertilizedCount;
    public GameObject cropPrefab => plantableSteps.crop;

    public void StartGrow()
    {
        StartCoroutine(MoveToNextStepWithDelay());
    }

    IEnumerator MoveToNextStepWithDelay()
    {
        StepData currentStep = plantableSteps.steps[stepIndex];
        yield return new WaitForSeconds(currentStep.stepTime);
        spriteRenderer.sprite = currentStep.stepIcon;
        stepIndex++;

        if(stepIndex < plantableSteps.steps.Count)
        {
            StartCoroutine(MoveToNextStepWithDelay());
        }
        else
        {
            readyToHarvest = true;
        }
    }
}