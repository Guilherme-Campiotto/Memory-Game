using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class LightCustom : MonoBehaviour
{
	public float magnitude = .5f;
	public float frequency = 2f;

    Light2D myLight;

	public float startIntensity;

    void Start()
    {
		myLight = GetComponent<Light2D>();
		startIntensity = myLight.intensity;
    }

    void Update()
    {
		myLight.intensity = startIntensity * (1f + Mathf.PerlinNoise(Time.time * frequency, 0f) * magnitude);
    }
}
