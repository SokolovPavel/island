using UnityEngine;
using System.Collections;

public class FPSCounter : MonoBehaviour {

		int frameCount = 0;
		float dt = 0.0f;
		public float fps = 0.0f;
		public int updateRate = 4;  // 4 updates per sec.

		void Update()
		{
				frameCount++;
				dt += Time.deltaTime;
				if (dt > 1.0/updateRate)
				{
						fps = frameCount / dt ;
						frameCount = 0;
						dt -= 1.0f/updateRate;
				}
		}
}
