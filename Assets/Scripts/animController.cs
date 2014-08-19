using UnityEngine;
using System.Collections;

public class animController : MonoBehaviour {

	void Start () {
        animation.Stop();
        animation["Idle"].wrapMode = WrapMode.Loop;
        animation["Chop"].wrapMode = WrapMode.Once;
	}

   public void SetAnim(string animName)
    {
        switch (animName)
        {
            case "Walk":
                if (!animation.IsPlaying("Chop") && !animation.IsPlaying("Idle"))
                    animation.Play("Idle");
                break;
            case "Stand":
                if (!animation.IsPlaying("Chop"))
                    animation.Stop();
                break;
            case "Chop":
                animation.Play("Chop");
                break;
        }
    }
}
