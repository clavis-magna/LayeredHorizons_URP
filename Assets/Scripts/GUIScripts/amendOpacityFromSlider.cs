using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class amendOpacityFromSlider : MonoBehaviour
{

    [HideInInspector]
    public GameObject meshParent;

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
        float mappedAlpha = helpers.Remap(thisSlider.value, 0.0f, 1.0f, 0.2f, 1.0f);

        m_Alpha = mappedAlpha;

        foreach (Transform child in meshParent.transform)
        {
            Renderer meshRenderer = child.GetComponent<Renderer>();
            var OGColor = meshRenderer.material.GetColor("_BaseColor");
            meshRenderer.material.SetColor("_BaseColor", new Color(OGColor.r, OGColor.g, OGColor.b, m_Alpha));
        }

    }
}
