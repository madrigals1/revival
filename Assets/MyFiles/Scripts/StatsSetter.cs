using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class StatsSetter : MonoBehaviour {

	MoveCursor mc;
	Tilemap tmGoods;
	public Vector3Int pos = new Vector3Int(0,0,0);
	
	Text goldTxt;
	Text foodTxt;
	Text mineralsTxt;

	JsonManager jsonManager;

	int gold = 0;
	int food = 0;
	int minerals = 0;

	void Start () {
		jsonManager = GameObject.Find("Controller").GetComponent<JsonManager>();
		tmGoods = GameObject.Find("Goods").GetComponent<Tilemap>();
		goldTxt = GameObject.Find("Gold").GetComponent<Text>();
		foodTxt = GameObject.Find("Food").GetComponent<Text>();
		mineralsTxt = GameObject.Find("Minerals").GetComponent<Text>();
	}

	void Zero () {
		gold = 0;
		food = 0;
		minerals = 0;
	}

	public void Reset () {
		Zero();
		if(tmGoods.GetTile(pos) != null){
			int tileI = jsonManager.GetTileIByName(tmGoods.GetTile(pos).name);
			gold += jsonManager.tiles[tileI].gold;
			food += jsonManager.tiles[tileI].food;
			minerals += jsonManager.tiles[tileI].minerals;
		}
		
	}

	void Update() {
		goldTxt.text = "Gold : " + gold;
		foodTxt.text = "Food : " + food;
		mineralsTxt.text = "Minerals : " + minerals;
	}

}
