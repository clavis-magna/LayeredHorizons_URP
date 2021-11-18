using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class amendMeshColourFromSlider : MonoBehaviour
{
    [HideInInspector]
    public GameObject mesh;

    SliderComponent thisSlider;

    float m_Hue;
    float m_Saturation;
    float m_Value;

    void Start()
    {
        thisSlider = GetComponent<SliderComponent>();
    }

    // Update is called once per frame
    void Update()
    {

        m_Hue = thisSlider.value;
        m_Saturation = 0.3f;
        m_Value = 1.0f;

        Renderer meshRenderer = mesh.GetComponent<Renderer>();
        meshRenderer.material.SetColor("_BaseColor", Color.HSVToRGB(m_Hue, m_Saturation, m_Value));
    }
}
