using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingManager : MonoBehaviour
{
    public List<Light> lights;
    
    public virtual void ToggleHeadLights()
    {
        
        foreach (Light light in lights)
        {
            if(light.intensity == 0)
            {
                light.intensity = 2;
            }
            else
            {
                light.intensity = 0;
            }
            //temp = light.intensity == 0 ? 2 : 0;
            //light.intensity = temp;
        }
    }


}
