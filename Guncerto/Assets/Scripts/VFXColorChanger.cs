using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXColorChanger : MonoBehaviour
{
    public Gun gun;
    public VisualEffect effect;
    // Start is called before the first frame update
    void Start()
    {
        effect = GetComponent<VisualEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
