using UnityEngine;
using System.Collections.Generic;
using KiiCorp.Cloud.Storage;
using KiiCorp.Cloud.Analytics;
using System;


public class GameScore : MonoBehaviour
{

	void Awake ()
	{

	}
	
	
	void Start ()
	{

	}

	static string BUCKET_NAME = "game_score";
	static GameScore instance;
	static KiiUser user;
	static KiiBucket appBucket;
	
	static GameScore()
	{
		Debug.Log("Gamescore - Static constructor");
	}
	
	public static GameScore Instance
	{
		get
		{
			if (instance == null)
			{
				instance = (GameScore)FindObjectOfType (typeof (GameScore));
				if (instance == null)
					instance = (new GameObject("GameScore")).AddComponent<GameScore>();
			}
			Debug.Log ("Game score loaded");
			return instance;
		}
	}
	
	
	void OnApplicationQuit ()
	{
		instance = null;
	}
	
	
	public string playerLayerName = "Player", enemyLayerName = "Enemies";
	
	
	int deaths = 0;
	Dictionary<string, int> kills = new Dictionary<string, int> ();
	float startTime = 0.0f;
	
	
	public static int Deaths
	{
		get
		{
			if (Instance == null)
			{
				return 0;
			}
			
			return Instance.deaths;
		}
	}
	
	
	#if !UNITY_FLASH
	public static ICollection<string> KillTypes
	{
		get
		{
			if (Instance == null)
			{
				return new string[0];
			}
			
			return Instance.kills.Keys;
		}
	}
	#endif
	
	
	public static int GetKills (string type)
	{
		if (Instance == null || !Instance.kills.ContainsKey (type))
		{
			return 0;
		}
		
		return Instance.kills[type];
	}
	
	
	public static float GameTime
	{
		get
		{
			if (Instance == null)
			{
				return 0.0f;
			}
			
			return Time.time - Instance.startTime;
		}
	}
	
	
	public static void RegisterDeath (GameObject deadObject)
	{
		
		if (Instance == null)
		{
			Debug.Log ("Game score not loaded");
			return;
		}

		int
			playerLayer = LayerMask.NameToLayer (Instance.playerLayerName),
			enemyLayer = LayerMask.NameToLayer (Instance.enemyLayerName);

		Debug.Log("Getting KiiUser");
		user = KiiUser.CurrentUser;

		if(user == null){
			if (deadObject.layer == playerLayer) {
				Instance.deaths++;
			} else if (deadObject.layer == enemyLayer) {
				Instance.kills [deadObject.name] = Instance.kills.ContainsKey (deadObject.name) ? Instance.kills [deadObject.name] + 1 : 1;
			} else {
				Instance.kills [deadObject.name] = Instance.kills.ContainsKey (deadObject.name) ? Instance.kills [deadObject.name] + 1 : 1;
			}
			return;
		}

		Debug.Log("Creating app bucket");
		appBucket = Kii.Bucket(BUCKET_NAME);

		Debug.Log("Creating death object");
		KiiObject death = appBucket.NewKiiObject();
		death["user"] = user.Username;
		death ["time"] = Time.time;

		if (deadObject.layer == playerLayer) {
			Instance.deaths++;
			death ["type"] = "Player";
			death ["count"] = Instance.deaths;
			Debug.Log ("Dead player counted");
			Debug.Log ("Saving death object");
		} else if (deadObject.layer == enemyLayer) {
			Instance.kills [deadObject.name] = Instance.kills.ContainsKey (deadObject.name) ? Instance.kills [deadObject.name] + 1 : 1;
			death ["type"] = "Enemy";
			death ["enemy"] = deadObject.name;
			death ["count"] = Instance.kills [deadObject.name];
			Debug.Log ("Dead enemy counted");
			Debug.Log ("Saving death object...");
		} else {
			Instance.kills [deadObject.name] = Instance.kills.ContainsKey (deadObject.name) ? Instance.kills [deadObject.name] + 1 : 1;
			death ["type"] = "Unknown";
			death ["enemy"] = deadObject.name;
			death ["count"] = Instance.kills [deadObject.name];
			Debug.Log ("Dead entity counted");
			Debug.Log ("Saving death object...");
		}

		death.Save((KiiObject obj, Exception e) => {
			if (e != null){
				Debug.Log ("GameScore: Failed to create death object: " + e.ToString());
			} else {
				Debug.Log ("GameScore: Create death object succeeded");
			}
		});
	}
	
	
	void OnLevelWasLoaded (int level)
	{
		if (startTime == 0.0f)
		{
			startTime = Time.time;
		}
	}

	public static void RegisterDamage(string target, float amount, float totalHealth, float gameTime, Vector3 direction){
		Debug.Log("Getting KiiUser");
		user = KiiUser.CurrentUser;
		if(user == null)
			return;
		Debug.Log("Creating app bucket");
		appBucket = Kii.Bucket(BUCKET_NAME);
		Debug.Log("Creating damage object");
		KiiObject damage = appBucket.NewKiiObject();
		damage ["user"] = user.Username;
		damage ["target"] = target;
		damage ["time"] = gameTime;
		damage ["health"] = totalHealth;
		damage ["amount"] = amount;
		damage ["direction"] = direction;
		Debug.Log ("Saving damage object...");
		damage.Save((KiiObject obj, Exception e) => {
			if (e != null){
				Debug.Log ("GameScore: Failed to create damage object: " + e.ToString());
			} else {
				Debug.Log ("GameScore: Create damage object succeeded");
			}
		});
	}

	// Send Analytics event for end of level time
	public static void EndOfLevel(float gameTime){
		Debug.Log("Sending end of level event...");
		KiiEvent ev = KiiAnalytics.NewEvent("EndOfLevel");
		
		// Set key-value pairs
		ev ["user"] = user.Username;
		ev ["time"] = gameTime;
		
		// Upload Event Data to Kii Cloud
		try
		{
			KiiAnalytics.Upload(ev);
		}
		catch (Exception e) 
		{
			Debug.LogError("GameScore: Unable to upload gend of level event to Kii Cloud: " + e.ToString());
		}		
	}
}
