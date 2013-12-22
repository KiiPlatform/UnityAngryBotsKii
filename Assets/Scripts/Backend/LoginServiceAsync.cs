using UnityEngine;
using System.Collections;
using KiiCorp.Cloud.Storage;
using System;

public class LoginServiceAsync : MonoBehaviour {

	public delegate void MethodReferenceWithResponse(Response response);
	public KiiUser user = null;
	
	public void sendLoginData(LoginData loginData, MethodReferenceWithResponse responseHandler) {
		
		Response response = (Response)gameObject.AddComponent("Response");
		Debug.Log("Attempting login...");
		KiiUser.LogIn(loginData.username, loginData.password, (KiiUser user, Exception e) => {
			if (e != null) {
				response.error = true;
				response.message = "Login falied: " + e.ToString();
				Debug.Log("Login falied: " + e.ToString());
			} else {
				response.error = false;
				response.message = "";
				Debug.Log("Login successful");
			}
		});
		// Calling response handler
		responseHandler(response);
	} 
}
