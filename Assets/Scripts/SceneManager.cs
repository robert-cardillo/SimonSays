using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {
	static float x1 = -3.2f;
	static float y1 = -3f;
	static float x2 = 3.2f;
	static float y2 = 5.6f;
	static float memoDelay = 1f;
	static float endDelay = 1f;
	static float timeLimit = 60f;
	GameObject[] windows;
	string gameState = "load";	//	{"load", "memo", "play", "end", "results"}
	int[] ons = new int[25];
	int[] mark = new int[25];
	float passedDelta = 0f;
	float passedTime = 0f;
	int oncnt = 0;
	int windowcnt = 0;
	int clickcnt = 0;
	int correctcnt = 0;
	int wrongcnt = 0;
	int difflevel = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		switch(gameState){
			case "load":
				if(correctcnt < 3){
					difflevel = 0;
				}else if(correctcnt < 13){
					difflevel = 1;
				}else{
					difflevel = 2;
				}
				PrepareGame(difflevel);
				break;

			case "memo":
				//	memoMode'a ilk giris
				if(passedDelta == 0f){
					for(int i = 0; i < oncnt; i++){
						SetWindowStatus(ons[i], "on");
					}
				}
				//	memoMode suresince
				passedDelta += Time.deltaTime;
				// memoMode'dan cikarken
				if(passedDelta >= memoDelay){
					gameState = "play";
					passedDelta = 0f;
					for(int i = 0; i < oncnt; i++){
						SetWindowStatus(ons[i], "off");
					}
				}
				break;
			
			case "play":
				passedTime += Time.deltaTime;
				if(passedTime > timeLimit){
					gameState = "dead";
					GameObject.Find("TimeText").guiText.text = "Time:0";
				}else{
					GameObject.Find("TimeText").guiText.text = "Time:"+Mathf.FloorToInt(timeLimit-passedTime);
				}
				break;

			case "end":
				passedDelta += Time.deltaTime;
				// memoMode'dan cikarken
				if(passedDelta >= endDelay){
					gameState = "load";
					passedDelta = 0f;
					for(int i = 0; i < windowcnt; i++){
						Destroy(windows[i]);
					}
				}
				break;
		}
	}

	void PrepareGame(int diffuculty) {
		clickcnt = 0;
		switch(diffuculty){
			case 0: 
				windowcnt = 9;
				windows = new GameObject[windowcnt];
				PrepareBoard(3, 3);
				PrepareOns(4, 4, windowcnt);
				break;
			case 1: 
				windowcnt = 16;
				windows = new GameObject[windowcnt];
				PrepareBoard(4, 4);
				PrepareOns(5, 8, windowcnt);
				break;
			case 2: 
				windowcnt = 25;
				windows = new GameObject[windowcnt];
				PrepareBoard(5, 5);
				PrepareOns(6, 12, windowcnt);
				break;
		}
		gameState = "memo";
	}

	public void WindowClicked(int id) {
		if(gameState != "play") return;
		switch(mark[id]) {
			case 0: 
				SetWindowStatus(id, "wrong");
				for(int i = 0; i < oncnt; i++){
					SetWindowStatus(ons[i], "on");
				}
				wrongcnt++;
				GameObject.Find("WrongText").guiText.text = "Wrong:"+wrongcnt.ToString();
				gameState = "end";
				break;

			case 1: 
				mark[id]++;
				clickcnt++;
				if(clickcnt == oncnt){
				correctcnt++;
				GameObject.Find("CorrectText").guiText.text = "Correct:"+correctcnt.ToString();
					for(int i = 0; i < oncnt; i++){
						SetWindowStatus(ons[i], "correct");
					}
					gameState = "end";
				}else{
					SetWindowStatus(id, "on");
				}
				break;

			case 2: 
				break;
		}
	}

	void SetWindowStatus(int index, string status){
		InsideScript s = (InsideScript) windows[index].transform.Find("Inside").GetComponent(typeof(InsideScript));
		s.SetStatus(status);
	}

	void PrepareBoard(int n, int m) {
		int index;
		float width = (x2 - x1) / (n - 1);
		float heigth = (y2 - y1) / (m - 1);

		for(int i = 0; i < n; i++){
			for(int j = 0; j < m; j++){
				index = i*n+j;
				GameObject go = (GameObject) Instantiate(Resources.Load("Window"));
				go.transform.position = new Vector3(x1 + i * width, y1 + j * heigth, 0f);
				InsideScript s = (InsideScript) go.transform.Find("Inside").GetComponent(typeof(InsideScript));
				s.SetId(index);
				windows[index] = go;
			}
		}
	}

	void PrepareOns(int min, int max, int over) {
		oncnt = GetRandom(min, max);
		for(int i = 0; i < windowcnt; i++){
			mark[i] = 0;
		}
		for(int i = 0; i < oncnt; i++){
			do{
				ons[i] = GetRandom(0, over - 1);
			}while(mark[ons[i]] == 1);
			mark[ons[i]] = 1;
		}
	}

	int GetRandom(int min, int max) {
		int r = Mathf.FloorToInt(Random.Range(min, max+1));
		if(r > max) r = max;
		return r;
	}
}
