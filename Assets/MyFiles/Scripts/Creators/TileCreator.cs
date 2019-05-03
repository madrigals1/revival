using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

[System.Serializable]
public class TileCreator {
	public int id;
	public string name;
	public int gold;
	public int minerals;
	public int food;
	public bool ground;
	
	public TileCreator(string name, int food, int minerals, int gold, bool ground, int lastID){
		this.name = name;
		this.food = food;
		this.minerals = minerals;
		this.gold = gold;
		this.ground = ground;
		this.id = lastID + 1;
	}

	public void SetTile(int food, int minerals, int gold, bool ground){
		this.food = food;
		this.minerals = minerals;
		this.gold = gold;
		this.ground = ground;
	}
}