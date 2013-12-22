using UnityEngine;
using System.Collections;
using KiiCorp.Cloud.Storage;
using System;

public class LoginService : MonoBehaviour {

	public delegate void MethodReferenceWithResponse(Response response);
	public KiiUser user = null;
	
	public void sendLoginData(LoginData loginData, MethodReferenceWithResponse responseHandler) {
		
		Response response = (Response)gameObject.AddComponent("Response");

		Debug.Log("Sending login request to Kii Cloud");
		
		try {
			user = KiiUser.LogIn(loginData.username, loginData.password);
			response.error = false;
			response.message = "";
			Debug.Log("User log-in successful");
		}
		catch(Exception e){
			response.error = true;
			response.message = e.Message;
			Debug.Log(e.Message);
		}

		// Calling response handler
		responseHandler(response);
	} 
}
