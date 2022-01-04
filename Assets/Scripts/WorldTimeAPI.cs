using UnityEngine;
using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine.Networking;


public class WorldTimeAPI : MonoBehaviour {
	
	#region Singleton class: WorldTimeAPI
	public static WorldTimeAPI Instance;

	void Awake(){
		if (Instance == null){
			Instance = this;
			DontDestroyOnLoad(this.gameObject);
		}
		else{
			Destroy(this.gameObject);
		}
	}
	#endregion

	struct TimeData { 
		public string datetime;
	}

	const string API_URL = "http://worldtimeapi.org/api/ip";
	[HideInInspector] public bool IsTimeLodaed = false;
	private DateTime _currentDateTime = DateTime.Now; //En caso de que no haya conexión a internet, se guarda la hora desde el sistema.

	// Start is called before the first frame update
	void Start() {
		StartCoroutine(GetRealDateTimeFromAPI());
	}

	public DateTime GetCurrentDateTime() { 
		return _currentDateTime.AddSeconds(Time.realtimeSinceStartup); //A la fechaHora actual le sumo los segundos desde iniciado el juego
	}

	IEnumerator GetRealDateTimeFromAPI() {
		UnityWebRequest webRequest = UnityWebRequest.Get(API_URL);
		yield return webRequest.SendWebRequest();
		
		TimeData timeData = JsonUtility.FromJson<TimeData>(webRequest.downloadHandler.text);
		_currentDateTime = ParseDateTime(timeData.datetime);
		IsTimeLodaed = true;
	}
	
	DateTime ParseDateTime(string datetime){ //Para darle el formato y quitar los valores innecesarios del JSON
		string date = Regex.Match(datetime, @"^\d{4}-\d{2}-\d{2}").Value;
		string time = Regex.Match(datetime, @"\d{2}:\d{2}:\d{2}").Value;
		return DateTime.Parse(string.Format("{0} {1}", date, time));
	}
}