using UnityEngine;
using System.Collections;
using KiiCorp.Cloud.Storage;
using KiiCorp.Cloud.Analytics;
using System;

public class KiiInit : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.Log("KiiCloudInit");
		Kii.Initialize("776e93b7", "b90ec1a893bd5abaa64ed1a25fbff4d0", Kii.Site.US);
		
		string deviceID = GetDeviceID();
		Debug.Log("KiiAnalyticsInit - DeviceId: " + deviceID);
		KiiAnalytics.Initialize("776e93b7", "b90ec1a893bd5abaa64ed1a25fbff4d0", KiiAnalytics.Site.US, deviceID);
	}
	
	string GetDeviceID()
	{
		string deviceID = ReadDeviceIDFromStorage();
		if (deviceID == null)
		{
			deviceID = Guid.NewGuid().ToString();
			SaveDeviceID(deviceID);
		}
		return deviceID;
	}
	
	string ReadDeviceIDFromStorage()
	{
		string id = PlayerPrefs.GetString("deviceId", null);
		if (id == null || id.Length == 0)
		{
			id = Guid.NewGuid().ToString();
		}
		return id;
	}
	
	void SaveDeviceID(string id)
	{
		PlayerPrefs.SetString("deviceId", id);
		PlayerPrefs.Save();
	}
}
