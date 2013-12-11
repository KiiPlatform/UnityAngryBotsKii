using UnityEngine;
using System.Collections.Generic;
using KiiCorp.Cloud.Storage;
using KiiCorp.Cloud.Analytics;


public class GameScore : MonoBehaviour
{
	
	void Awake ()
	{
		Debug.Log("GameScore - Awake called.");
	}
	
	
	void Start ()
	{
		Debug.Log("GameScore - Start called.");
	}
	
	static GameScore instance;
	static KiiUser user;
	static KiiBucket appBucket;
	
	static GameScore()
	{
		Debug.Log("Gamescore static");
	}
	
	static GameScore Instance
	{
		get
		{
			if (instance == null)
			{
				instance = (GameScore)FindObjectOfType (typeof (GameScore));
				if (instance == null)
					instance = (new GameObject("GameScore")).AddComponent<GameScore>();
			}
			
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
		
		Debug.Log ("Game score loaded");
		Debug.Log("Getting KiiUser");
		user = KiiUser.CurrentUser;
		Debug.Log("Creating app bucket");
		appBucket = Kii.Bucket("game_score");
		
		int
			playerLayer = LayerMask.NameToLayer (Instance.playerLayerName),
			enemyLayer = LayerMask.NameToLayer (Instance.enemyLayerName);
		
		Debug.Log("Creating death object");
		KiiObject death = appBucket.NewKiiObject();
		death["user"] = user.Username;
		death ["time"] = Time.time;
		
		if (deadObject.layer == playerLayer)
		{
			Instance.deaths++;
			death["type"] = "Player";
			death["count"] = Instance.deaths;
			Debug.Log ("Dead player counted");
			Debug.Log("Saving death object");
			death.Save();
		}
		else if (deadObject.layer == enemyLayer)
		{
			Instance.kills[deadObject.name] = Instance.kills.ContainsKey (deadObject.name) ? Instance.kills[deadObject.name] + 1 : 1;
			death["type"] = "Enemy";
			death["enemy"] = deadObject.name;
			death["count"] = Instance.kills[deadObject.name];
			Debug.Log ("Dead enemy counted");
			Debug.Log("Saving death object");
			death.Save();
		}
	}
	
	
	void OnLevelWasLoaded (int level)
	{
		if (startTime == 0.0f)
		{
			startTime = Time.time;
		}
	}
}
