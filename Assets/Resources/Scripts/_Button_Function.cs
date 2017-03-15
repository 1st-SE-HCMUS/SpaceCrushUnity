using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class _Button_Function : MonoBehaviour 
{
	
	public GameObject Pause_Panel;
	//public bool isPause = false;
	
	void Start()
	{
        if(Pause_Panel != null)
        {
            Pause_Panel.SetActive(false);
        }
	}
	
	public void _ChangeScene(string SceneName) 
    {
        SceneManager.LoadScene(SceneName);
	}
	
	public void _Quit()
	{
		Application.Quit();
	}
	
	public void _Pause()
	{
		//isPause = true;
		Pause_Panel.SetActive(true);
		Time.timeScale = 0;
	}
	
	public void _Resume()
	{
		//isPause = false;
		Time.timeScale = 1;
		Pause_Panel.SetActive(false);
	}

   
}
