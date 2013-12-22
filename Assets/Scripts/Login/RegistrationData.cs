using UnityEngine;
using System.Collections;

public class RegistrationData : MonoBehaviour {

	public string username = "";
	public string password = "";
	public string passwordConfirm = "";
	public string email = "";
	
	public void clear() {
		username = "";
		password = "";
		passwordConfirm = "";
		email = "";
	}
}
