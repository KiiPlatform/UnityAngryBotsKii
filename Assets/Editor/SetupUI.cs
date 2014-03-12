using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class SetupUI : EditorWindow {
	private string mAppId = "";
	private string mAppKey = "";
	private string mAppSite = "";
	string[] options = new string[] {"United States", "Japan", "China"};
	int index = 0;
	
	[MenuItem("Kii Game Cloud/Setup...", false, 1)]
	public static void MenuItemSetup() {
		EditorWindow.GetWindow(typeof(SetupUI));
	}
	
	void OnEnable() {
		mAppId = ConfigManager.Instance.GetAppId();
		mAppKey = ConfigManager.Instance.GetAppKey();
		mAppSite = ConfigManager.Instance.GetAppSite();
		index = SiteToIndex (mAppSite);
	}
	
	void Save() {
		ConfigManager.Instance.SetAppId(mAppId);
		ConfigManager.Instance.SetAppKey(mAppKey);
		mAppSite = IndexToSite (index);
		ConfigManager.Instance.SetAppSite(mAppSite);
		ConfigManager.Instance.Save();
	}
	
	void OnGUI() {
		// Title
		GUILayout.BeginArea(new Rect(20, 20, position.width - 40, position.height - 40));
		GUILayout.Label("Kii Game Cloud Setup", EditorStyles.boldLabel);
		GUILayout.Label("Setup your app parameters here to connect your game with Kii Game Cloud\nThese parameters can be obtained by creating an app at developer.kii.com");
		GUILayout.Space(10);
		
		// App ID field
		GUILayout.Label("App Id", EditorStyles.boldLabel);
		//GUILayout.Label("App Id description");
		mAppId = EditorGUILayout.TextField("Enter your App Id", mAppId);
		GUILayout.Space(10);
		
		// App Key field
		GUILayout.Label("App Key", EditorStyles.boldLabel);
		//GUILayout.Label("App Key description");
		mAppKey = EditorGUILayout.TextField("Enter your App Key", mAppKey);
		GUILayout.Space(10);

		// Site combo
		GUILayout.Label("Site", EditorStyles.boldLabel);
		GUILayout.Label("Location of the game backend for this app");
		index = EditorGUILayout.Popup(index, options);
		GUILayout.Space(10);
		
		// Setup button
		if (GUILayout.Button("Save")) {
			DoSetup();
		}
		GUILayout.EndArea();
	}
	
	void DoSetup() {

		Save();

		if (!IsValidAppId(mAppId)) {
			Alert("Malformed App Id");
			return;
		}
		if (!IsValidAppKey(mAppKey)) {
			Alert("Malformed App Key");
			return;
		}
		
		ConfigManager.Instance.Save();
		AssetDatabase.Refresh();
		Alert("Success", "Setup Complete");
		Close();
	}

	bool IsValidAppId (string mAppId)
	{
		return mAppId.Length == 8;
	}

	bool IsValidAppKey (string mAppKey)
	{
		return mAppKey.Length == 32;
	}

	private void Alert(string s) {
		Alert("Error", s);
	}
	
	private void Alert(string title, string s) {
		EditorUtility.DisplayDialog(title, s, "Ok");
	}
	
	private string IndexToSite (int index)
	{
		switch (index)
		{
			case 0:
				return "US";
			case 1:
				return "JP";
			case 2:
				return "CN";
			default:
				return "US";
		}
	}

	private int SiteToIndex (string site)
	{
		if(site.Equals("US"))
			return 0;
		if(site.Equals("JP"))
			return 1;
		if(site.Equals("CN"))
			return 2;
		return 0;
	}

}