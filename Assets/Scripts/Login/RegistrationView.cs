using UnityEngine;
using System.Collections;

public class RegistrationView : MonoBehaviour, ViewInterface {

	public const string NAME = "Registration";
	
	public GUISkin guiSkin;
	
	public GUIStyle header2Style;
	public GUIStyle formFieldStyle;
	public GUIStyle errorMessageStyle;
	
	public bool error = false;
	public string errorMessage = "";
	
	public RegistrationData data = new RegistrationData();

	public delegate void MethodReference();
	public MethodReference registrationHandler;
	public MethodReference cancelHandler;
	
	private bool blockUI = false;
	
	public void render() {

		if(Application.loadedLevel == 1)
			return;
		
		int screenWidth = Screen.width;
		int screenHeight = Screen.height;
		
		int xShift = (screenWidth - 360)/2;
		int yShift = (screenHeight - 300)/2;
		
		GUI.skin = guiSkin;
		
		// Disable UI in case of blockUI is true or any error:
		if(error || blockUI){
			GUI.enabled = false;
		} else {
			GUI.enabled = true;
		}
		
		// Message label:
		GUI.Label(new Rect(0, yShift + 0, screenWidth, 30), "Please register a player", header2Style);
		
		// Login label and text filed:
		GUI.Label(new Rect(xShift, yShift + 50, 100, 30), "Username:", formFieldStyle);
		data.username = GUI.TextField(new Rect(xShift + 110, yShift + 50, 250, 30), data.username, 16);
		
		// Password label and text filed:
		GUI.Label(new Rect(xShift, yShift + 100, 100, 30), "Password:", formFieldStyle);
		data.password = GUI.PasswordField(new Rect(xShift + 110, yShift + 100, 250, 30), data.password, "*"[0], 16);
		
		// Confirm password label and text filed:
		GUI.Label(new Rect(xShift - 50, yShift + 150, 150, 30), "Confirm password:", formFieldStyle);
		data.passwordConfirm = GUI.PasswordField(new Rect(xShift + 110, yShift + 150, 250, 30), data.passwordConfirm, "*"[0], 16);
		
		// Email label and text filed::
		GUI.Label(new Rect(xShift, yShift + 200, 100, 30), "E-mail:", formFieldStyle);
		data.email = GUI.TextField(new Rect(xShift + 110, yShift + 200, 250, 30), data.email, 32);
		
		// Register button:
		if(GUI.Button(new Rect(xShift + 50, yShift + 250, 120, 30), "Register")) {
			registrationHandler();
		}
		
		// Cancel button:
		if(GUI.Button(new Rect(xShift + 190, yShift + 250, 120, 30), "Cancel")) {
			cancelHandler();
		}
		
		// Enabling UI:
		GUI.enabled = true;
		
		// Show errors:
		showErrors();
	}
	
	
	// In case of registration error render error window:
	private void showErrors() {
		if(error) {
			int screenWidth = Screen.width;
			int screenHeight = Screen.height;
			GUI.Window (0, new Rect((screenWidth - 400)/2, (screenHeight - 300)/2, 400, 300), 
			                         renderErrorWindow, "Registration Error");
		}
	}
	
	// Render error window content:
	private void renderErrorWindow(int windowId) {
		GUI.Label(new Rect(10, 30, 380, 230), errorMessage, errorMessageStyle);
		if(GUI.Button(new Rect((400 - 120)/2, 260, 120, 30), "OK")) {
			error = false;
			errorMessage = "";
		}
	}
	
	public void  setBlockUI(bool blockUI) {
		this.blockUI = blockUI;
	}
}
