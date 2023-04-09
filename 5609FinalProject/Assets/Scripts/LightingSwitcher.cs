using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingSwitcher : MonoBehaviour
{
    // Start is called before the first frame update
    public Light directionalLight;
    public Light topDownLight;

    public float transitionDuration;
    bool isClear;
    bool isTransitioning;

    void Start()
    {
        if (GetComponent<SkyboxBlender>() == null)
        {
            throw new System.Exception("Please assign Skybox Blender script to this object.");
        }

        isClear = true;
        isTransitioning = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.B) && !isTransitioning)
        {
            StartCoroutine(transitionBetweenSkyboxes(!isClear));
            isClear = !isClear;
        }
    }

    public IEnumerator transitionBetweenSkyboxes(bool toClear)
    {
        isTransitioning = true;
        float startValue = 1;
        float endValue = 0;
        if (toClear)
        {
            startValue = 0;
            endValue = 1;
        }
        float timeElapsed = 0;
        while (timeElapsed < transitionDuration)
        {
            float scalar = Mathf.Lerp(startValue, endValue, timeElapsed / transitionDuration);
            GetComponent<SkyboxBlender>().blend = scalar;
            directionalLight.intensity = 2*scalar;
            topDownLight.intensity = 2*scalar;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        isTransitioning = false;
    }


}
