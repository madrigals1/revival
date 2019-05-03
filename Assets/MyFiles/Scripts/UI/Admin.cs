using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Admin : MonoBehaviour {

	GameObject btnAddTile, btnEditTile, btnDeleteTile;
	GameObject tileListMenu, addTileMenu, editTileMenu, deleteTileMenu;
	
	// ATM
	GameObject btnATMBack, btnETMBack, btnDTMBack;
	GameObject ifATMName, ifATMGold, ifATMFood, ifATMMinerals, tgATMGround;
	GameObject btnATMAdd;
	GameObject ddATMpick;

	// DTM
	GameObject btnDTMYes;
	GameObject txtDTMText;
	string textYS;

	// ETM
	GameObject btnETMEdit;
	GameObject ifETMGold, ifETMFood, ifETMMinerals, tgETMGround;
	GameObject txtETMID;

	Sprite[] tiles;

	public GameObject row;
	GameObject tileList;
	public int iSelected = -1;
	public int IDSelected = -1;
	int amount;


	JsonManager jsonManager;

	void Start () {
		Initialize();
		Go("TLM", false);

		btnAddTile.GetComponent<Button>().onClick.AddListener(() => Go("ATM", true));
		btnEditTile.GetComponent<Button>().onClick.AddListener(() => Go("ETM", true));
		btnDeleteTile.GetComponent<Button>().onClick.AddListener(() => Go("DTM", true));

		btnATMBack.GetComponent<Button>().onClick.AddListener(() => Go("ATM", false));
		btnETMBack.GetComponent<Button>().onClick.AddListener(() => Go("ETM", false));
		btnDTMBack.GetComponent<Button>().onClick.AddListener(() => Go("DTM", false));

		btnATMAdd.GetComponent<Button>().onClick.AddListener(AddTile);
		ddATMpick.GetComponent<Dropdown>().onValueChanged.AddListener(delegate{ValueChanged();});
		btnDTMYes.GetComponent<Button>().onClick.AddListener(DeleteTile);
		btnETMEdit.GetComponent<Button>().onClick.AddListener(EditTile);

		SetRows();
	}

	void Update() {
		if(addTileMenu.active){
			ddATMpick.transform.GetChild(2).GetComponent<Image>().sprite = tiles[ddATMpick.GetComponent<Dropdown>().value];
		}
		ValueChanged();
	}

	void SetDDOptions() {
		ddATMpick.GetComponent<Dropdown>().ClearOptions();
		tiles = Resources.LoadAll<Sprite>("Sprites/NewTiles/") as Sprite[];
		List<string> DropOptions = new List<string>();
		for(int i = 0; i < tiles.Length; i++) {
			DropOptions.Add(tiles[i].name);
		}

		ddATMpick.GetComponent<Dropdown>().AddOptions(DropOptions);
	}

	void Initialize(){
		tileList = GameObject.Find("TileList");
		jsonManager = gameObject.GetComponent<JsonManager>();

		btnAddTile = GameObject.Find("BtnAddTile");
		btnEditTile = GameObject.Find("BtnEditTile");
		btnDeleteTile = GameObject.Find("BtnDeleteTile");

		tileListMenu = GameObject.Find("TileListMenu");
		addTileMenu = GameObject.Find("AddTileMenu");
		editTileMenu = GameObject.Find("EditTileMenu");
		deleteTileMenu = GameObject.Find("DeleteTileMenu");

		ddATMpick = GameObject.Find("ATMPicturePick");

		btnATMBack = GameObject.Find("ATMBack");
		btnETMBack = GameObject.Find("ETMBack");
		btnDTMBack = GameObject.Find("DTMBack");

		btnATMAdd = GameObject.Find("ATMAdd");

		ifATMName = GameObject.Find("ATMName");
		ifATMGold = GameObject.Find("ATMGold");
		ifATMFood = GameObject.Find("ATMFood");
		ifATMMinerals = GameObject.Find("ATMMinerals");

		tgATMGround = GameObject.Find("ATMGround");

		btnDTMYes = GameObject.Find("DTMYes");
		txtDTMText = GameObject.Find("DTMText");

		btnETMEdit = GameObject.Find("ETMEdit");

		ifETMGold = GameObject.Find("ETMGold");
		ifETMFood = GameObject.Find("ETMFood");
		ifETMMinerals = GameObject.Find("ETMMinerals");

		tgETMGround = GameObject.Find("ETMGround");

		txtETMID = GameObject.Find("ETMID");
	}

	void Go(string type, bool dir) {
		switch(type) {
			case "ATM":
				addTileMenu.SetActive(dir);
				SetDDOptions();
				break;
			case "ETM":
				editTileMenu.SetActive(dir);
				ETMSet();
				break;
			case "DTM":
				deleteTileMenu.SetActive(dir);
				txtDTMText.GetComponent<Text>().text = textYS;
				break;
			case "TLM":
				deleteTileMenu.SetActive(dir);
				editTileMenu.SetActive(dir);
				addTileMenu.SetActive(dir);
				break;
		}

		tileListMenu.SetActive(!dir);
	}

	void BtnSelect(int sel) {
		iSelected = sel;
		IDSelected = Int32.Parse(tileList.transform.GetChild(iSelected + 1).GetChild(0).GetComponent<Text>().text);
		int i = jsonManager.GetLocal(IDSelected);
		textYS = "You sure you want to delete tile " + jsonManager.tiles[i].name + " with ID = " + jsonManager.tiles[i].id;
	}

	void AddTile() {
		string name = ifATMName.GetComponent<InputField>().text;
		int gold = Int32.Parse(ifATMGold.GetComponent<InputField>().text);
		int food = Int32.Parse(ifATMFood.GetComponent<InputField>().text);
		int minerals = Int32.Parse(ifATMMinerals.GetComponent<InputField>().text);
		bool ground = tgATMGround.GetComponent<Toggle>().isOn;
		
		jsonManager.AddTile(name, food, minerals, gold, ground);
		if(ground)
			Debug.Log("Created ground tile with name " + name + "\nwith Gold = " + gold + ", Food = " + food + ", Minerals = " + minerals);
		else 
			Debug.Log("Created non-ground tile with name  " + name + "\nwith Gold = " + gold + ", Food = " + food + ", Minerals = " + minerals);
		
		SetRows();
		SetDDOptions();
	}

	void EditTile(){
		int gold = Int32.Parse(ifETMGold.GetComponent<InputField>().text);
		int food = Int32.Parse(ifETMFood.GetComponent<InputField>().text);
		int minerals = Int32.Parse(ifETMMinerals.GetComponent<InputField>().text);
		bool ground = tgETMGround.GetComponent<Toggle>().isOn;
		string name = jsonManager.tiles[jsonManager.GetLocal(IDSelected)].name;

		jsonManager.EditTile(IDSelected, food, minerals, gold, ground);
		if(ground)
			Debug.Log("Edited ground tile with ID "+ IDSelected + "\nwith name " + name + "\nwith Gold = " + gold + ", Food = " + food + ", Minerals = " + minerals);
		else 
			Debug.Log("Edited non-ground tile with ID "+ IDSelected + "\nwith name " + name + "\nwith Gold = " + gold + ", Food = " + food + ", Minerals = " + minerals);
		
		SetRows();
		Go("TLM", false);	
	}

	void DeleteTile(){
		string name = jsonManager.tiles[jsonManager.GetLocal(IDSelected)].name;
		jsonManager.DeleteTile(Int32.Parse(tileList.transform.GetChild(iSelected + 1).GetChild(0).GetComponent<Text>().text));
		Debug.Log("Deleted tile with ID "+ IDSelected + "\nwith name " + name);
		SetRows();
		Go("TLM", false);
	}

	void ValueChanged() {
		if(ddATMpick.transform.GetChild(2).GetComponent<Image>().sprite != null){
			ifATMName.GetComponent<InputField>().text = ddATMpick.transform.GetChild(2).GetComponent<Image>().sprite.name;
		}
	}

	void ETMSet(){
		btnETMEdit.GetComponent<Button>().onClick.AddListener(EditTile);
		txtETMID.GetComponent<Text>().text = "ID = " + IDSelected;
		int i = jsonManager.GetLocal(IDSelected);
		ifETMGold.GetComponent<InputField>().text = "" + jsonManager.tiles[i].gold;
		ifETMMinerals.GetComponent<InputField>().text = "" + jsonManager.tiles[i].minerals;
		ifETMFood.GetComponent<InputField>().text = "" + jsonManager.tiles[i].food;
		tgETMGround.GetComponent<Toggle>().isOn = jsonManager.tiles[i].ground;
	}

	void SetRows() {
		amount = jsonManager.GetAmount();
		int rowamount = tileList.transform.childCount;
		for(int i = 1; i < rowamount; i++) {
			Destroy(tileList.transform.GetChild(i).gameObject);
		}
		for(int i = 0; i < amount; i++){
			GameObject rowins = Instantiate(row, tileList.transform);
			rowins.transform.GetChild(0).GetComponent<Text>().text = "" + jsonManager.tiles[i].id;
			rowins.transform.GetChild(1).GetComponent<Text>().text = jsonManager.tiles[i].name;
			rowins.transform.GetChild(2).GetComponent<Text>().text = "" + jsonManager.tiles[i].gold;
			rowins.transform.GetChild(3).GetComponent<Text>().text = "" + jsonManager.tiles[i].food;
			rowins.transform.GetChild(4).GetComponent<Text>().text = "" + jsonManager.tiles[i].minerals;
			rowins.transform.GetChild(5).GetComponent<Image>().sprite = jsonManager.GetPicture(jsonManager.tiles[i].name);
			rowins.transform.GetChild(6).GetComponent<Text>().text = "" + jsonManager.tiles[i].ground;
			int ilocal = i;
			rowins.GetComponent<Button>().onClick.AddListener(() => BtnSelect(ilocal));
		}
		Vector2 tsize = tileList.GetComponent<RectTransform>().sizeDelta;
		tileList.GetComponent<RectTransform>().sizeDelta = new Vector2(tsize.x, 35*(amount+1));
		tileList.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, (35/2)*(amount+1));
	}
}
