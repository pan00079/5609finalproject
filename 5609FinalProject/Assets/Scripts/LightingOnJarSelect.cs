using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingOnJarSelect : MonoBehaviour
{
    // Start is called before the first frame update
    public Light directionalLight;
    public SkyboxBlender blender;
    public float transitionDuration;
    public float minVal;
    public float maxVal;

    GameObject selectedJar;
    float curBlend;

    void Start()
    {
        if (blender == null)
        {
            throw new System.Exception("Please assign Skybox Blender script to this object.");
        }

        selectedJar = null;
        curBlend = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo = new RaycastHit();
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo))
            {

                // Debug.Log(hitInfo.transform.gameObject.name);
                if (hitInfo.transform.gameObject.tag == "FlowerJar")
                {
                    if (selectedJar != null)
                    {
                        selectedJar.GetComponent<CountryData>().SetLightingIfSelected(false);
                    }
                    selectedJar = hitInfo.transform.gameObject;
                    selectedJar.GetComponent<CountryData>().SetLightingIfSelected(true);
                    float sleeping = selectedJar.GetComponent<CountryData>().getTimeVal("Sleeping");
                    sleeping = (sleeping - minVal) / (maxVal - minVal);
                    StartCoroutine(transitionBetweenSkyboxes(sleeping));
                }
                else if (selectedJar != null)
                {
                    selectedJar.GetComponent<CountryData>().SetLightingIfSelected(false);
                }
            }
        }
    }

    public IEnumerator transitionBetweenSkyboxes(float sleepVal)
    {
        float timeElapsed = 0;
        while (timeElapsed < transitionDuration)
        {
            float scalar = Mathf.Lerp(curBlend, sleepVal, timeElapsed / transitionDuration);
            blender.blend = scalar;
            directionalLight.intensity = 2*scalar;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        curBlend = sleepVal;
    }


}
