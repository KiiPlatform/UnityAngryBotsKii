using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using KiiCorp.Cloud.Storage;
using KiiCorp.Cloud.Analytics;
using System.Linq;

[InitializeOnLoad]
public class Autorun : MonoBehaviour {
	
	void Awake ()
	{
		Debug.Log("Autorun - Awake called.");
	}
	
	// Use this for initialization
	void Start ()
	{
		Debug.Log("Autorun - Start called.");
	}

	// Update is called once per frame
	void Update () {
		
	}
	
	static Autorun()
	{
		Debug.Log("Autorun - Constructor called.");
	}

}
