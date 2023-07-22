using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandPresence : MonoBehaviour
{
	public InputDeviceCharacteristics controllerCharacteristics;
	public List<GameObject> controllerPrefabs;
	
    private InputDevice targetDevice;
	private GameObject spawnedController;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetDevices(1.0f));
    }

    IEnumerator GetDevices(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevices(devices);

        Debug.Log("Enumerating XR Devices...");
        foreach (var item in devices)
        {
            Debug.Log(item.name + item.characteristics);
        }
        if (devices.Count > 0)
        {
            targetDevice = devices[0];
			GameObject prefab = controllerPrefabs.Find(controller => controller.name == targetDevice.name);
			if (prefab)
			{
				spawnedController = Instantiate(prefab, transform);
			}
			else
			{
				Debug.LogError("Error");
				spawnedController = Instantiate(controllerPrefabs[0], transform);
			}
			
		}
    }
    // Update is called once per frame
    void Update()
    {
		if (targetDevice.isValid)
		{
        	spawnedController.SetActive(true);
		}
    }
}