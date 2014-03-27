using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using KiiCorp.Cloud.Storage;

[InitializeOnLoad]
public class KiiAutoInitialize : ScriptableObject {

	private static KiiAutoInitialize sInstance = null;

	string mFile;
	Dictionary<string, string> mDict = new Dictionary<string, string>();

	public const string CFG_FILE_PATH = "Assets/Plugins/KiiConfig.txt";
	public const string CFG_APP_ID = "cfg.AppId";
	public const string CFG_APP_KEY = "cfg.AppKey";
	public const string CFG_APP_SITE = "cfg.AppSite";

	static KiiAutoInitialize()
	{
		//Sign up on developer.kii.com and create a Unity app to get these parameters!
		//See the Assets/Readme.txt file in this project for more info
		//Your backend location options: Kii.Site.US, Kii.Site.JP, Kii.Site.CN
		//IMPORTANT: backend location here must match backend location configured in your app at developer.kii.com

		Debug.Log("Initializing Kii...");
		KiiAutoInitialize init = Instance;
		string appID = init.GetAppId("__KII_APP_ID__");
		string appKey = init.GetAppKey("__KII_APP_KEY__");
		Kii.Site appSite = init.GetAppSiteValue();
		Debug.Log ("AppId:" + appID + " / AppKey:" + appKey + " / AppSite:" + appSite.ToString());

		Kii.Initialize(appID, appKey, appSite);

		//Interested in Game Analytics? Get our Analytics SDK http://developer.kii.com/#/sdks
		//More info: http://documentation.kii.com/en/guides/unity/managing-analytics
	}
	
	public static KiiAutoInitialize Instance {
		get {
			if (sInstance == null) {
				sInstance = ScriptableObject.CreateInstance<KiiAutoInitialize>();
			}
			return sInstance;
		}
	}
	
	private KiiAutoInitialize() {
		string ds = Path.DirectorySeparatorChar.ToString();
		mFile = CFG_FILE_PATH.Replace("/", ds);
		
		if (File.Exists(mFile)) {
			StreamReader rd = new StreamReader(mFile);
			while (!rd.EndOfStream) {
				string line = rd.ReadLine();
				if (line == null || line.Trim().Length == 0) {
					break;
				}
				line = line.Trim();
				string[] p = line.Split(new char[] { '=' }, 2);
				if (p.Length >= 2) {
					mDict[p[0].Trim()] = p[1].Trim();
				}
			}
			rd.Close();
		}
		
	}
	
	public string Get(string key, string defaultValue) {
		if (mDict.ContainsKey(key)) {
			return mDict[key];
		} else {
			return defaultValue;
		}
	}
	
	public string Get(string key) {
		return Get(key, "");
	}
	
	public string GetAppId (string defaultValue)
	{
		return Get(CFG_APP_ID, defaultValue);
	}
	
	public string GetAppKey (string defaultValue)
	{
		return Get(CFG_APP_KEY, defaultValue);
	}
	
	public string GetAppSite (string defaultValue)
	{
		return Get(CFG_APP_SITE, defaultValue);
	}

	public Kii.Site GetAppSiteValue ()
	{
		string site = GetAppSite("US");
		if(site.Equals("US"))
			return Kii.Site.US;
		if(site.Equals("JP"))
			return Kii.Site.JP;
		if(site.Equals("CN"))
			return Kii.Site.CN;
		return Kii.Site.US;
	}

}