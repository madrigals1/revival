using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuAdmin : MonoBehaviour {

	Button newMap, loadMap;

	void Start () {
		newMap = GameObject.Find("New Map").GetComponent<Button>();
		loadMap = GameObject.Find("Load Map").GetComponent<Button>();
		newMap.onClick.AddListener( () => GoTo(true));
		loadMap.onClick.AddListener( () => GoTo(false));
	}

	void GoTo(bool nm) {
		Dictionary<string, string> arguments = new Dictionary<string, string>();
		arguments.Add("new", "" + nm);
		Scenes.Load("Map", arguments);
	}
}
