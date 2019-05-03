using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEditor;

public class JsonManager : MonoBehaviour {
	public TileCreator[] tiles;
	string json;
	int amount;
	public string path;
	public string aPath;

	string jsonI = @"[{""id"":0,""name"":""test"",""gold"":0,""food"":0,""minerals"":0,""ground"":false}]";

	void Start () {
		GetPath();
		ReadJson();
		TrimNormalize();
		ObjectsFromJson();
	}

	public void GetPath(){
		string pathInit = Application.dataPath + "/Resources/JSON/";
		path = pathInit + "tiles.json";
		aPath = pathInit + "amounts.txt";
	}
		
	void ReadJson(){
		json = System.IO.File.ReadAllText(path);
	}

	void ClearJson(){
		json = jsonI;
	}

	void TrimNormalize() {
		json = json.Replace("\n", "");
		json = json.Replace("\t", "");
		json = json.Replace("\r", "");
	}

	void ObjectsFromJson(){
		tiles = JsonHelper.getJsonArray<TileCreator> (json);
		amount = tiles[tiles.Length-1].id;
	}

	public void AddTile(string name, int food, int minerals, int gold, bool ground){
		TileCreator newObject = new TileCreator(name, food, minerals, gold, ground, tiles[tiles.Length-1].id);
		AssetDatabase.MoveAsset("Assets/Resources/Sprites/NewTiles/" + name + ".png", "Assets/Resources/Sprites/TileSprites/" + name + ".png");
		string objjson = JsonUtility.ToJson(newObject);
		AddObjectToJson(json, objjson);
		ObjectsFromJson();
		WriteJson(path,aPath);
	}

	public void EditTile(int id, int food, int minerals, int gold, bool ground){
		ClearJson();
		int idlocal = GetLocal(id);
		tiles[idlocal].SetTile(food, minerals, gold, ground);
		for(int i = 1; i < tiles.Length; i++){
			string objjson = JsonUtility.ToJson(tiles[i]);
			AddObjectToJson(json, objjson);
		}
		ObjectsFromJson();
		WriteJson(path,aPath);
	}

	public void DeleteTile(int id){
		ClearJson();
		for(int i = 1; i < tiles.Length; i++){
			if(tiles[i].id != id){
				string objjson = JsonUtility.ToJson(tiles[i]);
				AddObjectToJson(json, objjson);
			}
		}
		ObjectsFromJson();
		WriteJson(path,aPath);
	}

	void AddObjectToJson(string oldjson, string newjson){
		oldjson = oldjson.Substring(0,oldjson.Length-1);
		json = oldjson + "," + newjson + "]";
	}

	void WriteJson(string pathw, string aPathw){
		System.IO.File.WriteAllText(pathw, json);
		System.IO.File.WriteAllText(aPathw, "" + amount);
	}

	// public int GetAmountOfPics(){
	// 	return Directory.GetFiles(Application.dataPath + "/Resources/Sprites/TileSprites",
	// 		"*.png", SearchOption.AllDirectories).Length;
	// }

	public int GetAmount() {
		return tiles.Length;
	}

	// public Sprite GetPictureByID(int id) {
	// 	return Resources.Load("Sprites/TileSprites/" + tiles[id].name, typeof(Sprite)) as Sprite;
	// }

	public Sprite GetPicture(string name) {
		return Resources.Load("Sprites/TileSprites/" + name, typeof(Sprite)) as Sprite;
	}

	public int GetLocal(int sellocal) {
		int locali = 0;
		for(int i = 0; i < tiles.Length; i++){
			if(sellocal == tiles[i].id){
				locali = i;
			}
		}
		return locali;
	}

	public void PrintAll(){
		for(int i = 0; i < tiles.Length; i++){
			Debug.Log(tiles[i].name + "," + tiles[i].id);
		}
	}

	public int GetTileIByName(string name){
		int locali = 0;
		for(int i = 0; i < tiles.Length; i++){
			if(tiles[i].name == name){
				locali = i;
			}
		}

		return locali;
	}

	public string GetNameByID(int id) {
		string name = "";
		for(int i = 0; i < tiles.Length; i++){
			if(tiles[i].id == id){
				name = tiles[i].name;
			}
		}
		return name;
	}
}
