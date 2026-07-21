using Playsmart.UIToolkit;
using System;
using UnityEngine;
using UnityEngine.UIElements;

public class RadialBarDemoScript : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
    private RadialBar ring;
    float secondProgress = 0;
    public float ringSpeed = 5;

    private void OnEnable()
    {
        VisualElement root = uiDocument.rootVisualElement;
        ring = root.Q<RadialBar>("ring");
    }

    private void Update()
    {
        ring.value = secondProgress/ringSpeed;
        if(secondProgress >= ringSpeed)
        {
            secondProgress = 0;
            ShakeUp();
        }
        else
        {
            secondProgress += Time.deltaTime;
            
        }
    }

    private void ShakeUp()
    {
        ring.progressColor = UnityEngine.Random.ColorHSV();
        ring.roundedCaps = !ring.roundedCaps;
        ring.innerEmptyPercent = UnityEngine.Random.Range(0.1f, 0.9f);
        ring.startAngle = UnityEngine.Random.Range(-180, 180);
    }
}
