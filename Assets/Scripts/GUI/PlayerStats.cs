using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour {
	bool sleeping;
	public float health;
	public float hunger;
	public float thirst;
	public float energy;
	public float strength;
	public float agility;
	public float stamina;

	public float maxHealth;
	public float maxHunger;
	public float maxThirst;
	public float maxEnergy;
	public float maxStrength;
	public float maxAgility;
	public float maxStamina;

	public	float thirstCoef = 0.8f;
	public  float hungerCoef = 0.4f;
	public  float energyCoef = 0.2f;
	public  float harmCoef = 0.1f;
	public Texture texture;
	public Material material;
	public int size=25;

	void Start () 
	{

		maxHealth=255.0f;
		maxHunger=255.0f;
		maxThirst=255.0f;
		maxEnergy=255.0f;
		maxStrength=255.0f;
		maxAgility=255.0f;
		maxStamina=255.0f;

		health= 255.0f;
		hunger= maxHunger;
		thirst = maxThirst;
		energy = maxEnergy/2;
		strength= maxStrength / 3;
		agility = maxAgility / 3;
		stamina = maxStamina / 3;

		material.SetFloat ("_Width", Screen.width*0.015f*size);
	}
	

	void FixedUpdate () 
	{
		UpdateNeeds ();
	}

	void UpdateNeeds ()
	{
		hunger = hunger-hungerCoef*Time.deltaTime;
		thirst = thirst-thirstCoef*Time.deltaTime;
		energy = energy-energyCoef*Time.deltaTime;
		if(this.hunger<0f) {this.hunger=0f;}
		if(this.thirst<0f) {this.thirst=0f;}
		if(this.energy<0f) {this.energy=0f;}
		checkLowNeeds();
		updateBars ();
	}
	void OnGUI() {
		if (Event.current.type.Equals (EventType.Repaint))
			Graphics.DrawTexture (new Rect (0, 0, Screen.width*0.01f*size, Screen.width*0.01f*size*texture.height/texture.width), texture, material);
	}
	void checkLowNeeds()
	{
		if (hunger<20.0f) 
		{
			this.harm(harmCoef*(Mathf.Abs(this.hunger-20.0f)/10.0f));
		}
		if (this.energy<20.0f) 
		{
			this.harm(harmCoef*(Mathf.Abs(this.energy-20.0f)/10.0f));
		}
		if (this.thirst<20.0f) 
		{
			this.harm(harmCoef*(Mathf.Abs(this.thirst-20.0f)/10.0f));
		}
	}


	void harm(float amount)		//портит игроку здоровье, вызывается несколько в секунду. Используется для плавного уменьшения здоровья
	{
		this.health=this.health - amount*Time.deltaTime;
		updateBars ();
	}	

	void addHunger(float amount)
	{
		hunger=hunger+amount;
		if(hunger>maxHunger) 
		{ hunger=maxHunger;	}
		updateBars ();

	}
	void addHealth(float amount)
	{
		health=health+amount;
		if(health>maxHealth) 
		{ health=maxHealth;	}
		updateBars ();
	}
	void addThirst(float amount)
	{
		thirst=thirst+amount;
		if(thirst>maxThirst) 
		{ thirst=maxThirst;	}
		updateBars ();
	}
	void addEnergy(float amount)
	{
		energy=energy+amount;
		if(energy>maxEnergy) 
		{ energy=maxEnergy;	}
		updateBars ();
	}

	void changeSleepingStatus()
	{
		sleeping=!sleeping;
	}

	void updateBars(){
		float minOffset = 0.08f;
		float maxOffset = 0.78f;
		material.SetFloat ("_Health", health / maxHealth * maxOffset + minOffset);
		material.SetFloat ("_Energy", energy / maxEnergy * maxOffset + minOffset);
		material.SetFloat ("_Hunger", hunger / maxHunger * maxOffset + minOffset);
		material.SetFloat ("_Thirst", thirst / maxThirst * maxOffset + minOffset);
	}

}
