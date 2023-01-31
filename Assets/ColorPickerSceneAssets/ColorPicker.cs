using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour
{
    public Slider red, green, blue;
    public Camera cam;

    public Color defaultColor;




    // Start is called before the first frame update
    void Start()
    {
        red.value = defaultColor.r;
        green.value = defaultColor .g;
        blue.value = defaultColor.b;
        cam.backgroundColor = new Color(red.value, green.value, blue.value);
    }

    // Update is called once per frame
    void Update()
    {
        cam.backgroundColor = new Color(red.value, green.value, blue.value);
    }
}
