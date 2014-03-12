using UnityEngine;
using System.Collections;
using KiiCorp.Cloud.Storage;
using System;

public class LoginServiceAsync : MonoBehaviour {

	public delegate void MethodReferenceWithResponse(Response response);
	public KiiUser user = null;
	
	public void sendLoginData(LoginData loginData, MethodReferenceWithResponse responseHandler) {
		
		Response response = (Response)gameObject.AddComponent("Response");
		bool inHandler = true;
		Debug.Log("Attempting login...");
		KiiUser.LogIn(loginData.username, loginData.password, (KiiUser user, Exception e) => {
			if (e != null) {
				response.error = true;
				response.message = "Login failed: " + e.ToString();
				inHandler = false;
				Debug.Log("Login failed: " + e.ToString());
			} else {
				response.error = false;
				response.message = "";
				inHandler = false;
				Debug.Log("Login successful");
			}
		});
		// Calling response handler
		while(inHandler) {}
		responseHandler(response);
	} 
}
