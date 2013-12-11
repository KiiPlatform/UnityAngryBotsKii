using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using KiiCorp.Cloud.Storage;
using KiiCorp.Cloud.Analytics;
using System.Linq;

[InitializeOnLoad]
public class Autorun : MonoBehaviour {
	
	void Awake ()
	{
		Debug.Log("Autorun - Awake called.");
	}
	
	// Use this for initialization
	void Start ()
	{
		Debug.Log("Autorun - Start called.");
	}

	// Update is called once per frame
	void Update () {
		
	}
	
	static Autorun()
	{
		Debug.Log("Autorun static");
		Debug.Log("KiiCloudInit");
		Kii.Initialize("776e93b7", "b90ec1a893bd5abaa64ed1a25fbff4d0", Kii.Site.US);
		
		string deviceID = GetDeviceID();
		Debug.Log("KiiAnalyticsInit - DeviceId: " + deviceID);
		KiiAnalytics.Initialize("776e93b7", "b90ec1a893bd5abaa64ed1a25fbff4d0", KiiAnalytics.Site.US, deviceID);
		SignUpUser();
		LogInUser();
		StoreObject();
	}
	
	static private String username;
	static private String password = "123ABC";
	static private KiiUser user;
	
	
	static string GetDeviceID()
	{
		string deviceID = ReadDeviceIDFromStorage();
		if (deviceID == null)
		{
			deviceID = Guid.NewGuid().ToString();
			SaveDeviceID(deviceID);
		}
		return deviceID;
	}
	
	static string ReadDeviceIDFromStorage()
	{
		string id = PlayerPrefs.GetString("deviceId", null);
		if (id == null || id.Length == 0)
		{
			id = System.Guid.NewGuid().ToString();
		}
		return id;
	}
	
	static void SaveDeviceID(string id)
	{
		PlayerPrefs.SetString("deviceId", id);
		PlayerPrefs.Save();
	}
	
	static void SignUpUser(){
		Debug.Log("KiiSignup");
		KiiUser.Builder builder;
		var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		var random = new System.Random();
		username = new string(
			Enumerable.Repeat(chars, 8)
			.Select(s => s[random.Next(s.Length)])
			.ToArray());
		username = username + "_";
		builder = KiiUser.BuilderWithName(username);
		KiiUser user = builder.Build();
		//Debug.Log("KiiSignup - Username: " + user.Username);
		//Debug.Log("KiiSignup - Password: " + password);
		try {
			user.Register(password);
		}
		catch(KiiCorp.Cloud.Storage.CloudException e){
			Debug.Log("status=" + e.Status + " body=" + e.Body);
		}
	}
	
	static void LogInUser(){
		Debug.Log("KiiLogin");
		//Debug.Log("KiiLogin - Username: " + username);
		//Debug.Log("KiiLogin - Password: " + password);
		try {
			user = KiiUser.LogIn(username, password);
		}
		catch(KiiCorp.Cloud.Storage.CloudException e){
			Debug.Log("status=" + e.Status + " body=" + e.Body);
		}
	}
	
	static void StoreObject(){
		if (user == null)
			return;
		Debug.Log("KiiStore");
		KiiBucket userBucket = user.Bucket("user_bucket");
		KiiObject obj = userBucket.NewKiiObject();
		
		obj["score"] = 987;
		obj["mode"] = "easy";
		obj["premiumUser"] = false;
		
		try {
			obj.Save();
		}
		catch(KiiCorp.Cloud.Storage.CloudException e){
			Debug.Log("status=" + e.Status + " body=" + e.Body);
		}
	}
}
