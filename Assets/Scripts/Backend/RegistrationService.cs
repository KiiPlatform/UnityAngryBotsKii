using UnityEngine;
using System.Collections;
using KiiCorp.Cloud.Storage;
using System;

public class RegistrationService : MonoBehaviour {

	public delegate void MethodReferenceWithResponse(Response response);

	public void sendRegistrationData(RegistrationData registrationData, MethodReferenceWithResponse responseHandler) {
		
		Response response = (Response)gameObject.AddComponent("Response"); 
		
		Debug.Log("Sending registration request to Kii Cloud");

		if (registrationData.password.Equals (registrationData.passwordConfirm)) {
			Debug.Log("Creating user builder");
			KiiUser.Builder builder;
			builder = KiiUser.BuilderWithName (registrationData.username);
			builder.WithEmail(registrationData.email);
			KiiUser user = builder.Build();
			try {
				    Debug.Log("Registering...");
					user.Register (registrationData.password);
					response.error = false;
					response.message = "";
					Debug.Log ("User registration successful");
			} catch (Exception e) {
					response.error = true;
					response.message = e.Message;
					Debug.Log (e.Message);
			}
		} else {
			response.error = true;
			response.message = "Passwords don't match!";
		}

		// Calling response handler:
		responseHandler(response);
	}
}
