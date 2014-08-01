using UnityEngine;
using System.Collections;

public class Chest : MonoBehaviour {

	private bool isActive;
	private GameObject user;
	private Inventory plInv;
	public Inventory chInv;
	private Menu menu;
	public AudioClip openSound;
	public AudioClip closeSound;
	public int chestCapacity;
	private int index = 0;
	private int dialog;
	private string takeEditStr = "1";
	private string putEditStr = "1";
	private int qua;
	// Use this for initialization
	void Start () {
	
	}
	


	void Use(GameObject ply) {
		user = ply;
		plInv = user.GetComponent<Inventory> ();
		menu = user.GetComponent<Menu> ();


		if (!isActive) {

			isActive = true;
			menu.locked = true;
			menu.lockControl();
			menu.showCursor ();
			menu.enabled = false;
			audio.PlayOneShot(openSound);
			animation.Play("Open");

		} else {

		}
	}

	void Update() {
		if (isActive) {
			if ((Input.GetButtonDown ("Inventory")) || (Input.GetButtonDown ("Use"))) {
				isActive = false;
				menu.enabled = true;
				menu.locked = false;
				menu.unlockControl();
				menu.showCrosshair();
				animation.Play ("Close");
				audio.PlayOneShot(closeSound);
			}
		}
	}


	void OnGUI()
	{
		if(isActive)
		{
			ShowChestDialog();	
			if( dialog == 1 )
			{
				Rect TakeWindowRect  =new Rect ((Screen.width/2)-120, (Screen.height/2)-40, 240, 80);
				TakeWindowRect = GUI.Window (2,TakeWindowRect, DrawTakeDialog, "Take from chest");
			}
			if( dialog == 2 )
			{
				Rect PutWindowRect  = new Rect ((Screen.width/2)-120, (Screen.height/2)-40, 240, 80);
				PutWindowRect = GUI.Window (3,PutWindowRect, DrawPutDialog, "Put to chest");
			}	
		}
	}

	void ShowChestDialog()
	{

		Rect ChestWindowRect = new Rect ((Screen.width / 2) - 175, (Screen.height / 2) - 180, 450, 350);
		ChestWindowRect = GUI.Window (1,ChestWindowRect, DrawChestDialog, "Chest");

	} 

	void DrawChestDialog (int windowID) 
	{

		GUI.Label (new Rect (15, 20, 160, 20), "Items from your inventory:");
		GUI.Label (new Rect (260, 20, 130, 20), "Items from chest:");
		int pos=0; //Используется для задания положения очередной кнопки, FYI
		for (var i=0;i<Inventory.inventorySize;i++)
		{
			if (plInv.items[i]!= null)
			{
				if( GUI.Button(new Rect(15,50+27*pos,140,23),plInv.items[i].title+" x "+ plInv.items[i].quantity ))
				{
					index=i;
					dialog=2;
				}
				if( GUI.Button(new Rect(160,50+27*pos,30,23),"All"))
				{

					if (chInv.AddItem (plInv.items [i]) >= 0) {

						plInv.items [i] = null;
					}
				}
				pos++;
			}

		}
		pos=0;	
		for (int i=0;i<chestCapacity;i++)
		{
			if (chInv.items[i] != null)
			{
				if( GUI.Button(new Rect(250,50+27*pos,140,23),chInv.items[i].title+" x "+chInv.items[i].quantity))
				{
					index=i;
					dialog=1;
				}
				if( GUI.Button(new Rect(395,50+27*pos,30,23),"All"))
				{
					if (plInv.AddItem (chInv.items [i]) >= 0) {
						chInv.items [i] = null;
					}
				}
				pos++;
			}

		}


		if (GUI.Button (new Rect (20,370,60,20), "Close"))
		{
			isActive=false;

			dialog=0;
			animation.Play("Close");
			audio.PlayOneShot(closeSound);
			menu.enabled = true;
			menu.locked = false;
			menu.unlockControl();
			menu.showCrosshair();
			//animation.Play ("Close");
		}   

	}

	void DrawTakeDialog(int WindowID)
	{
		GUI.BringWindowToBack(1);
		GUI.Label(new Rect (15, 20, 220, 20), "Enter an amount of " + chInv.items[index].title + " to take:" );
		takeEditStr =  GUI.TextField (new Rect (90, 50, 40, 25), takeEditStr);
		if(takeEditStr!="") {qua= int.Parse(takeEditStr);}

		if (GUI.Button (new Rect (140,50,50,25), "Take"))
		{
			dialog=0;
			if (qua < 0) {
				qua *= -1;
			}
			if (qua > chInv.items [index].quantity) {
				qua = chInv.items [index].quantity;
			}
				if (plInv.AddItem (chInv.items [index], qua) >= 0) {


					if (qua == chInv.items [index].quantity) {
						chInv.items [index] = null;
					} else {
						chInv.AddQuantity (index, -qua);
					}
				}
			
			index=0;
			qua=1;
		}

	}

	void DrawPutDialog(int WindowID)
	{
		GUI.BringWindowToBack(1);
		GUI.Label(new Rect (15, 20, 220, 20), "Enter an amount of " + plInv.items[index].title + " to put:" );
		putEditStr =  GUI.TextField (new Rect (90, 50, 40, 25), putEditStr);
		if(putEditStr!="") {qua = int.Parse(putEditStr);}
		if (GUI.Button (new Rect (140,50,50,25), "Put"))
		{
			dialog=0;
			if (qua < 0) {
				qua *= -1;
			}
			if (qua > plInv.items [index].quantity) {
				qua = plInv.items [index].quantity;
			}
				if (chInv.AddItem (plInv.items [index], qua) >= 0) {

					if (qua == plInv.items [index].quantity) {
						plInv.items [index] = null;
					} else {
						plInv.AddQuantity (index, -qua);
					}
				}
			
			index=0;
			qua=1;

		}

	}


}
