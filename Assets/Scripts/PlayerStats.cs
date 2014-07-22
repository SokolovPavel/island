﻿using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour {
	bool sleeping;
	float health;
	float hunger;
	float thirst;
	float energy;
	float strength;
	float agility;
	float stamina;

	float maxHealth;
	float maxHunger;
	float maxThirst;
	float maxEnergy;
	float maxStrength;
	float maxAgility;
	float maxStamina;

	public	float thirstCoef = 0.8f;
	public  float hungerCoef = 0.4f;
	public  float energyCoef = 0.2f;
	public  float harmCoef = 0.1f;


	void Start () 
	{

		maxHealth=255.0f;
		maxHunger=255.0f;
		maxThirst=255.0f;
		maxEnergy=255.0f;
		maxStrength=255.0f;
		maxAgility=255.0f;
		maxStamina=255.0f;

		health= maxHealth;
		hunger= maxHunger;
		thirst = maxThirst;
		energy = maxEnergy;
		strength= maxStrength / 3;
		agility = maxAgility / 3;
		stamina = maxStamina / 3;
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
	}	

	void addHunger(float amount)
	{
		hunger=hunger+amount;
		if(hunger>maxHunger) 
		{ hunger=maxHunger;	}

	}
	void addHealth(float amount)
	{
		health=health+amount;
		if(health>maxHealth) 
		{ health=maxHealth;	}
	}
	void addThirst(float amount)
	{
		thirst=thirst+amount;
		if(thirst>maxThirst) 
		{ thirst=maxThirst;	}
	}
	void addEnergy(float amount)
	{
		energy=energy+amount;
		if(energy>maxEnergy) 
		{ energy=maxEnergy;	}
	}

	void changeSleepingStatus()
	{
		sleeping=!sleeping;
	}

}