using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MapManager : MonoBehaviour {

	string[] paths;
	public string path;
	
	//string[][] texts;
	string text;
	string[] mapLayers, mapLayersLines;
	int[,,] map;
	int length;
	int width;

	public int lastID;
	public int mapID = 0;

	Tilemap tmGoods;
	Tilemap tmGround;
	Tilemap tmWater;

	Button btnSave;

	JsonManager jsonManager;

	void Start () {
		path = Application.dataPath + "/Resources/Maps/";
		jsonManager = GetComponent<JsonManager>();
		btnSave = GameObject.Find("Save").GetComponent<Button>();

		GetTilemaps();

		if(Scenes.getParam("new") == "True") {	
			btnSave.onClick.AddListener(SaveMap);
		} else {
			btnSave.gameObject.SetActive(false);
			GetMap();
			SetMap();
		}
	}

	void GetMap () {
		paths = Directory.GetFiles(path, "*.txt");
		lastID = Int32.Parse(paths[paths.Length-1].Substring(path.Length,1));
		
		text = File.ReadAllText(paths[mapID]);
		Debug.Log(text);
		mapLayers = text.Split('#');
		mapLayers[1] = mapLayers[1].Substring(1, mapLayers[1].Length-1);
		mapLayers[2] = mapLayers[2].Substring(1, mapLayers[2].Length-1);
		mapLayersLines = mapLayers[0].Split('\n'); 
		width = mapLayersLines.Length - 1;
		length = mapLayersLines[0].Split(' ').Length;
		Debug.Log("width : " + width);
		Debug.Log("length : " + length);

		map = new int[3,width,length];
		for(int m = 0; m < 3; m++){
			mapLayersLines = mapLayers[m].Split('\n');
			for(int i = 0; i < width; i++){
				string[] textArr = mapLayersLines[i].Split(' ');
				for(int j = 0; j < length; j++){
					//Debug.Log(textArr[j] + ", m : " + m + ", i : " + i + ", j : " + j);
					map[m,i,j] = Int32.Parse(textArr[j]);
				}
			}
		}

		// for(int m = 0; m < 3; m++){
		// 	for(int i = 0; i < width; i++){
		// 		string line = "";
		// 		for(int j = 0; j < length; j++){
		// 			line = line + map[m,i,j] + " ";  
		// 		}
		// 		Debug.Log(line);
		// 	}
		// 	Debug.Log("#");
		// }	
	}

	void SetMap() {
		for(int i = 0; i < width; i++){
			for(int j = 0; j < length; j++){
				string name = jsonManager.GetNameByID(map[0,i,j]);
				Tile newTile = new Tile();
				newTile.sprite = Resources.Load("Sprites/TileSprites/" + name, typeof(Sprite)) as Sprite;
				tmGoods.SetTile(new Vector3Int(j,i,0), newTile);
			}
			for(int j = 0; j < length; j++){
				string name = jsonManager.GetNameByID(map[1,i,j]);
				Tile newTile = new Tile();
				newTile.sprite = Resources.Load("Sprites/TileSprites/" + name, typeof(Sprite)) as Sprite;
				tmGround.SetTile(new Vector3Int(j,i,0), newTile);
			}
			for(int j = 0; j < length; j++){
				string name = jsonManager.GetNameByID(map[2,i,j]);
				Tile newTile = new Tile();
				newTile.sprite = Resources.Load("Sprites/TileSprites/" + name, typeof(Sprite)) as Sprite;
				tmWater.SetTile(new Vector3Int(j,i,0), newTile);
			}
		}
	}

	void GetTilemaps() {
		tmGoods = GameObject.Find("Goods").GetComponent<Tilemap>();
		tmGround = GameObject.Find("Ground").GetComponent<Tilemap>();
		tmWater = GameObject.Find("Water").GetComponent<Tilemap>();
	}

	public void SaveMap() {
		string goods = "";
		string ground = "";
		string water = "";
		int minmax = 30;
		for(int i = -minmax; i < minmax; i++){
			for(int j = -minmax; j < minmax; j++){
				if(tmGoods.GetTile(new Vector3Int(j,i,0)) != null) {
					goods += jsonManager.GetTileIByName(tmGoods.GetSprite(new Vector3Int(j,i,0)).name);
				} else {
					goods += 2;
				}
				goods += " ";
				
				if(tmGround.GetTile(new Vector3Int(j,i,0)) != null) {
					ground += jsonManager.GetTileIByName(tmGround.GetSprite(new Vector3Int(j,i,0)).name);
					Debug.Log(tmGround.GetTile(new Vector3Int(j,i,0)).name);	
				} else {
					ground += 2;
				}
				ground += " ";

				if(tmWater.GetTile(new Vector3Int(j,i,0)) != null) {
					water += jsonManager.GetTileIByName(tmWater.GetSprite(new Vector3Int(j,i,0)).name);	
				} else {
					water += 2;
				}
				water += " ";
			}

			goods = goods.Substring(0, goods.Length-1);
			ground = ground.Substring(0, ground.Length-1);
			water = water.Substring(0, water.Length-1);

			goods += "\n";
			ground += "\n";
			water += "\n";
		}
		string mapFinal = goods + "#\n" + ground + "#\n" + water;
		File.WriteAllText(path + "1.txt", mapFinal);
	}
} 
