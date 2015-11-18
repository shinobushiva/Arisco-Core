using UnityEngine;
using System.Collections; 
using UnityEngine.UI;

public class PlayControl : SingletonMonoBehaviour<PlayControl>
{


	public Button play;
    public Button pause;
    public Button step;
    public Button finish;

    public Canvas canvas;

	public AgentWorldRun agentRun;

	private float duration = 1f;
	public float Duration {
		get{
			return duration;
		}
		set{
			duration = value;
			if(agentRun != null && agentRun.runner != null)
				agentRun.runner.Weight = duration;
		}
	}


	public void PlayClicked ()
	{
		agentRun.Play ();
	}

	public void PauseClicked ()
	{
		agentRun.Pause ();
	}

    public void StepClicked ()
    {
		agentRun.Step ();
    }

	public void StopClicked ()
	{
		agentRun.Stop ();
	}

	void Update(){
		ManageButtons ();
	}

    public void OnShowUI(){
        canvas.enabled = true;
    }

    public void OnHideUI(){
        canvas.enabled = false;
    }

	void ManageButtons(){
		if (!agentRun.runner.Running && !agentRun.runner.Finished) {
            play.interactable = true;
		} else {
            play.interactable = false;
		}

		if (agentRun.runner.Running)
        {
            pause.interactable = true;
			if (agentRun.runner.Paused)
            {
				if(pause.GetComponentInChildren<Text>())
                	pause.GetComponentInChildren<Text>().text = "Un Pause";
            } else
            {
				if(pause.GetComponentInChildren<Text>())
                	pause.GetComponentInChildren<Text>().text = "Pause";
            }
        } else
        {
            pause.interactable = false;
        }

		if (!agentRun.runner.Finished)
        {
            step.interactable = true;
        }else{
            step.interactable = false;
        }

		if (agentRun.runner.Running || agentRun.runner.Finished || !agentRun.runner.Started)
        {
            finish.interactable = true;
            if(agentRun.runner.Finished || !agentRun.runner.Started){
				if(finish.GetComponentInChildren<Text>())
               		finish.GetComponentInChildren<Text>().text = "Rebuild";
            }else{
				if(finish.GetComponentInChildren<Text>())
                	finish.GetComponentInChildren<Text>().text = "Finish";
            }
        } else
        {
            finish.interactable = false;
        }


	}

	void Start(){

        if (agentRun == null)
        {
            agentRun = FindObjectOfType<AgentWorldRun>();
        }

	}

	
}
