using UnityEngine;
using System;
using System.Collections;
using KiiCorp.Cloud.Storage;

public class LoginView : MonoBehaviour, ViewInterface {

	public const string NAME = "Login";
	
	public GUISkin guiSkin;
	
	public GUIStyle header1Style;
	public GUIStyle header2Style;
	public GUIStyle header2ErrorStyle;
	public GUIStyle formFieldStyle;
	
	public LoginData data = new LoginData();
	
	public bool error = false;
	public string errorMessage = "";

	public delegate void MethodReference();
	public MethodReference loginHandler;
	public MethodReference registrationHandler;

	private bool blockUI = false;
	
	public void render() {

		if(Application.loadedLevel == 1)
			return;

		int screenWidth = Screen.width;
		int screenHeight = Screen.height;
		
		int xShift = (screenWidth - 260)/2;
		int yShift = (screenHeight - 260)/2;
		
		GUI.skin = guiSkin;
		
		// Disabling UI if blockUI is true: 
		GUI.enabled = !blockUI;

		if (Kii.AppId == null || Kii.AppKey == null || Kii.AppId.Equals ("__KII_APP_ID__") || Kii.AppKey.Equals ("__KII_APP_KEY__")) {
			GUI.Label(new Rect(0, yShift + 70, screenWidth, 30), "Invalid API keys. See Assets/Readme.txt", header2Style);
			if(GUI.Button(new Rect(xShift, yShift + 220, 120, 30), "Get API Keys")) {
				Application.OpenURL("http://developer.kii.com");
			}
		} else {		
			// Main label:
			GUI.Label(new Rect(0, yShift, screenWidth, 30), "AngryBots Kii Login", header1Style);
			
			// Message label:
			if(error) {
				GUI.Label(new Rect(0, yShift + 70, screenWidth, 30), errorMessage, header2ErrorStyle);
			} else {
				GUI.Label(new Rect(0, yShift + 70, screenWidth, 30), "Please enter your credentials", header2Style);
			}
			
			// Login label and login text field:
			GUI.Label(new Rect(xShift, yShift + 120, 100, 30), "Username:", formFieldStyle);
			data.username = GUI.TextField(new Rect(xShift + 110, yShift + 120, 150, 30), data.username, 16);
			
			// Password label and password text field:
			GUI.Label(new Rect(xShift, yShift + 170, 100, 30), "Password:", formFieldStyle);
			data.password = GUI.PasswordField(new Rect(xShift + 110, yShift + 170, 150, 30), data.password, "*"[0], 16);
			
			// Login button:
			if(GUI.Button(new Rect(xShift, yShift + 220, 120, 30), "Login")) {
				loginHandler();
			}
			
			// Switch to registration view button:
			if(GUI.Button(new Rect(xShift + 140, yShift + 220, 120, 30), "Register")) {
				registrationHandler();
			}
		}
		// Enabling UI: 
		GUI.enabled = true;
	}
	
	public void setBlockUI(bool blockUI) {
		this.blockUI = blockUI;
	}
}
