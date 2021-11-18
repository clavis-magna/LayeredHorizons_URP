using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class amendOpacityFromSlider : MonoBehaviour
{

    [HideInInspector]
    public GameObject mesh;

    SliderComponent thisSlider;

    float m_Alpha;


    // Start is called before the first frame update
    void Start()
    {
        thisSlider = GetComponent<SliderComponent>();
    }

    // Update is called once per frame
    void Update()
    {
        m_Alpha = thisSlider.value;



        Renderer meshRenderer = mesh.GetComponent<Renderer>();

        var OGColor = meshRenderer.material.GetColor("_BaseColor");

        meshRenderer.material.SetColor("_BaseColor", new Color(OGColor.r, OGColor.g, OGColor.b, m_Alpha));
        print("slide being called)");

    }
}
