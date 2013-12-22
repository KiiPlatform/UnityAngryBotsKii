using UnityEngine;
using System.Collections;

public interface ViewInterface {

	// Rendering UI function:
	void render();
	
	// Enable/disable UI components:
	void setBlockUI(bool blockUI);
}
