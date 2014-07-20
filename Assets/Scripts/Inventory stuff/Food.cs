public class Food : ItemG {
	private int _healthReg;//Reg - regeneration
	private int _energyReg;
	private int _hungerReg;
	private int _thirstReg;

	public Food(){
		_healthReg = 5;
		_energyReg = 5;
		_healthReg = 5;
		_thirstReg = 5;
	}
	public Food(int he,int en,int hu, int th){
		_healthReg = he;
		_energyReg = en;
		_hungerReg = hu;
		_thirstReg = th;
	}
	public int HealthRegeneration {
		get { return _healthReg; }
		set { if (value>0) _healthReg = value; }
	}
	public int EnergyRegeneration {
		get { return _energyReg; }
		set { if (value>0) _energyReg = value; }
	}
	public int HungerhRegeneration {
		get { return _hungerReg; }
		set { if (value>0) _hungerReg = value; }
	}
	public int ThirstRegeneration {
		get { return _thirstReg; }
		set { if (value>0) _thirstReg = value; }
	}
}
