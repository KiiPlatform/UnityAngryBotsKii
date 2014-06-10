using UnityEngine;
using System.Collections;

public class LoginController : MonoBehaviour {

	// Common GUI skin:
	public GUISkin guiSkin;
	
	// GUI styles for labels:
	public GUIStyle header1Style;
	public GUIStyle header2Style;
	public GUIStyle header2ErrorStyle; 
	public GUIStyle formFieldStyle;
	public GUIStyle errorMessageStyle;
	
	// Active view name:
	string activeViewName = LoginView.NAME;
	
	// Map views by name:
	Hashtable viewByName;
	
	// Login view:
	LoginView loginView;

	// Registration view:
	RegistrationView registrationView;
	
	// Login service:
	LoginService loginService;
	
	// Registration service:
	RegistrationService registrationService;
	
	// Do we need block UI:
	bool blockUI = false;
	
	// This function will be called when scene loaded:
	void Start () {   
		//init components
		viewByName = new Hashtable ();
		loginService = (LoginService)gameObject.AddComponent("LoginService");
		registrationService = (RegistrationService)gameObject.AddComponent("RegistrationService");
		loginView = (LoginView)gameObject.AddComponent("LoginView");
		registrationView = (RegistrationView)gameObject.AddComponent("RegistrationView");

		// Setup of login view:
		loginView.guiSkin = guiSkin;
		loginView.header1Style = header1Style;
		loginView.header2Style = header2Style;
		loginView.header2ErrorStyle = header2ErrorStyle;
		loginView.formFieldStyle = formFieldStyle;
		
		// Handler of registration button click:
		loginView.registrationHandler = delegate(){
			// Clear reistration fields:
			registrationView.data.clear();
			// Set active view to registration:
			activeViewName = RegistrationView.NAME;
		};
		
		// Setup of login view:
		registrationView.guiSkin = guiSkin;
		registrationView.header2Style = header2Style;
		registrationView.formFieldStyle = formFieldStyle;
		registrationView.errorMessageStyle = errorMessageStyle;
		
		// Handler of cancel button click:
		registrationView.cancelHandler = delegate() {
			// Clear reistration fields:
			loginView.data.clear();
			// Set active view to registration:
			activeViewName = LoginView.NAME;
		};
		
		viewByName = new Hashtable();
		
		// Adding views to views by name map:
		viewByName[LoginView.NAME] = loginView;
		viewByName[RegistrationView.NAME] = registrationView;
		
		loginView.loginHandler = delegate() {
			blockUI = true; 
			// Sending login request:
			loginService.sendLoginData(loginView.data, loginResponseHandler);
		};
		
		// Handler of Register button:
		registrationView.registrationHandler = delegate() {
			blockUI = true;
			// Sending registration request:
			registrationService.sendRegistrationData(registrationView.data, registrationResponseHandler);
		};
	}

	private AsyncOperation asyncOp;
	
	// This function will draw UI components
	void OnGUI () {
		
		// Getting current view by active view name:
		ViewInterface currentView = (ViewInterface)viewByName[activeViewName];
		
		// Set blockUI for current view:
		currentView.setBlockUI(blockUI);
		
		// Rendering current view:
		currentView.render();
		
		// Show box with "Wait..." when UI is blocked:
		var screenWidth = Screen.width;
		var screenHeight = Screen.height;
		if(blockUI) {
			GUI.Box(new Rect((screenWidth - 200)/2, (screenHeight - 60)/2, 200, 60), "Wait...");
		}

		if (asyncOp != null && !asyncOp.isDone) {
			GUIStyle style = new GUIStyle( GUI.skin.box );
			style.normal.background = MakeTex( 2, 2, new Color( 0f, 1f, 0f, 0.5f ) );
			GUI.Box (new Rect(0, Screen.height - 40, asyncOp.progress * Screen.width, 40), "", style);
		}
		
	}

	private Texture2D MakeTex(int width, int height, Color color){
		Texture2D texture = new Texture2D(width, height);

		for (int y = 0; y < texture.height; ++y) 
			for (int x = 0; x < texture.width; ++x) 
				texture.SetPixel(x, y, color);
		
		texture.Apply();
		return texture;
	}
		
	// Processing login response
	void loginResponseHandler(Response response) {
		blockUI = false;
		loginView.error = response.error;
		loginView.errorMessage = response.message;
		if(!response.error) {
			asyncOp = Application.LoadLevelAsync(1);
		}
	}
	
	// Processing registration response
	void registrationResponseHandler(Response response) {
		blockUI = false;
		registrationView.error = response.error;
		registrationView.errorMessage = response.message;
		if(!response.error) {
			asyncOp = Application.LoadLevelAsync(1);
		}
	}
}
