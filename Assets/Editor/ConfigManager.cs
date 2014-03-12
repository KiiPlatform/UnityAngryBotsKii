using System;
using System.Collections.Generic;
using System.IO;

public class ConfigManager {

	private static ConfigManager sInstance = null;

	bool mDirty = false;
	string mFile;
	Dictionary<string, string> mDict = new Dictionary<string, string>();

	public const string CFG_FILE_PATH = "Assets/Plugins/KiiConfig.txt";
	public const string CFG_APP_ID = "cfg.AppId";
	public const string CFG_APP_KEY = "cfg.AppKey";
	public const string CFG_APP_SITE = "cfg.AppSite";
	
	public static ConfigManager Instance {
		get {
			if (sInstance == null) {
				sInstance = new ConfigManager();
			}
			return sInstance;
		}
	}
	
	private ConfigManager() {
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
	
	public bool GetBool(string key, bool defaultValue) {
		return Get(key, defaultValue ? "true" : "false").Equals("true");
	}
	
	public bool GetBool(string key) {
		return Get(key, "false").Equals("true");
	}

	public string GetAppId ()
	{
		return Get(CFG_APP_ID);
	}

	public string GetAppKey ()
	{
		return Get(CFG_APP_KEY);
	}

	public string GetAppSite ()
	{
		return Get(CFG_APP_SITE);
	}
	
	public void Set(string key, string val) {
		mDict[key] = val;
		mDirty = true;
	}
	
	public void Set(string key, bool val) {
		Set(key, val ? "true" : "false");
	}

	public void SetAppId(string val)
	{
		Set (CFG_APP_ID, val);
	}

	public void SetAppKey(string val)
	{
		Set (CFG_APP_KEY, val);
	}

	public void SetAppSite(string val)
	{
		Set (CFG_APP_SITE, val);
	}
	
	public void Save() {
		if (!mDirty) {
			return;
		}
		StreamWriter wr = new StreamWriter(mFile, false);
		foreach (string key in mDict.Keys) {
			wr.WriteLine(key + "=" + mDict[key]);
		}
		wr.Close();
		mDirty = false;
	}
}
