using UnityEngine;
using System.Collections;
using System;
using System.Reflection;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WorldParametersUIBuilder : MonoBehaviour
{

    public Slider sliderPrefab;
    public RectTransform panelPrefab;
    private WorldParameters parameters;
    private Canvas canvas;

    // Use this for initialization
    void Start()
    {
        if (FindObjectOfType<EventSystem>() == null)
        {
            var es = new GameObject("EventSystem", typeof(EventSystem));
            es.AddComponent<StandaloneInputModule>();
            es.AddComponent<TouchInputModule>();
        }

        canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            var canvasObject = new GameObject("Canvas");
            canvas = canvasObject.AddComponent<Canvas>();
            canvasObject.AddComponent<GraphicRaycaster>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }

        var panel = Instantiate(panelPrefab) as RectTransform;
        panel.transform.SetParent(canvas.transform);
        panel.anchoredPosition = Vector2.zero;
        panel.sizeDelta = Vector2.zero;
        panel.anchorMin = Vector2.zero;
        panel.anchorMax = Vector2.one * 0.5f;

        parameters = GetComponent<WorldParameters>();

        Type t = parameters.GetType();

        int i = 0;
        FieldInfo[] fs = t.GetFields();
        foreach (FieldInfo ff in fs)
        {
            FieldInfo f = ff;

            object val = f.GetValue(parameters);
            if (AriscoTools.IsNumeric(ref val))
            {
                bool isInt = AriscoTools.IsNumeric(ref val);

                Slider obj = Instantiate(sliderPrefab) as Slider;
                obj.transform.SetParent(panel.transform);
                if(isInt){
                    obj.wholeNumbers = true;
                }

                FieldAttributes fa = f.Attributes;


                var rect = obj.GetComponent<RectTransform>();
                rect.anchoredPosition = Vector2.zero;
                rect.sizeDelta = Vector2.zero;
                rect.anchorMin = new Vector2(0, 0);
                rect.anchorMax = new Vector2(1, 0);
                rect.pivot = new Vector2(.5f, 0);
                rect.sizeDelta = new Vector2(0, 20);
                rect.anchoredPosition = new Vector2(0, 20 * i);

                f.SetValue(obj.value, val);
                 
                obj.onValueChanged.AddListener((value) => {
                    print(f.Name);
                    f.SetValue(parameters, value);
                });
            }
            
            i++;
        }

    
    }
    
    // Update is called once per frame
    void Update()
    {
    
    }
}
