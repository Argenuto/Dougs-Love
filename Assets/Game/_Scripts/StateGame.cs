using UnityEngine;
using System.Collections.Generic;
using System.Collections;
/*
using System.Data;
using Mono.Data.Sqlite; 
*/
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class StateGame : MonoBehaviour {
	
	public static StateGame g_state;
	string datapath;
	public int hi_score,runner_head,runner_body,runner_bottom,runner_feet,momos,plats;
	[Range(1,4)]
	public int difficulty = 1;
	[Range(0,1)]
	public int controlmode, gemi2,tutorial;
	public string rnner_name = "Dordo";
	UnityEvent m_unityev;
	#if !UNITY_WEBGL
	//public IDbConnection dbconn;
	#endif

	 // Use this for initialization
	void Awake () {
		if (g_state == null) {
			g_state = this;
			DontDestroyOnLoad (gameObject);
		} else if (g_state != this) {
			Destroy (gameObject);
		}
		OnValidate ();

	}

	void OnValidate (){
		datapath = (Application.persistentDataPath + "/scores.dat");
	
//			dbconn = connection ();
	
		Load ();
			//LoadSerializedData ();



	}

	void Start () {
		m_unityev = new UnityEvent ();
		m_unityev.AddListener (UpdateOptionData);
		m_unityev.AddListener (ShowOptionDataInScreen);
		m_unityev.AddListener (UpdateGemi2);
		m_unityev.AddListener (SaveOptions);	
	}
	
	// Update is called once per frame
	void Update () {
		


	}
	/*
	IDbConnection connection(){
		string conn = "URI=file:" + Application.dataPath + "/jueguillo001.db"; //Path to database.
		IDbConnection dbconn = (IDbConnection) new SqliteConnection(conn);
		dbconn.Open(); //Open connection to the database.
		IDbCommand dbcmd = dbconn.CreateCommand();
	//string sqlQuery = "drop table if exists score";
		ArrayList querys = new ArrayList();
		querys.Add("CREATE TABLE IF NOT EXISTS score (id INTEGER,high_score INTEGER,momos INTEGER,plats INTEGER, UNIQUE(id))");
		querys.Add("INSERT OR IGNORE INTO score(id,high_score,momos,plats) VALUES (1,2,0,0)");
		querys.Add( "CREATE TABLE IF NOT EXISTS options (id INTEGER, gemi2 INTEGER,controlmode INTEGER,difficulty INTEGER, tutorial INTEGER,UNIQUE(id))");
		querys.Add( "INSERT OR IGNORE INTO options (id,gemi2,controlmode,difficulty,tutorial) VALUES (1,1,0,1,1)");
		querys.Add("CREATE TABLE IF NOT EXISTS runner (id INTEGER,head INTEGER,body INTEGER,bottom INTEGER,feet INTEGER,name TEXT, UNIQUE(id))");
		querys.Add( "INSERT OR IGNORE INTO runner (id,head,body,bottom,feet,name) VALUES (1,0,0,0,0,'"+name+"')");
		//dbcmd.CommandText = sqlQuery;
	//dbcmd.ExecuteNonQuery ();
		foreach(string q in querys){
			dbcmd.CommandText = q;
			dbcmd.ExecuteNonQuery ();
		}
		return dbconn;
	}
*/
	public void Load(){
		/*
			IDbCommand dbcmd = dbconn.CreateCommand ();
			string sqlQuery = "SELECT * FROM score";
			dbcmd.CommandText = sqlQuery;
	
			IDataReader reader = dbcmd.ExecuteReader ();

			while (reader.Read ()) {
				hi_score = reader.GetInt32 (1);
				momos = reader.GetInt32 (2);
				plats = reader.GetInt32 (3);
	
			}
			reader.Close ();
			//dbcmd.Dispose ();
	 
	 sqlQuery = "SELECT * FROM options";
			dbcmd.CommandText = sqlQuery;
			reader = dbcmd.ExecuteReader ();

				while (reader.Read ()) {
					gemi2 = reader.GetInt32 (1);
					controlmode = reader.GetInt32 (2);
					difficulty = reader.GetInt32 (3);
					tutorial = reader.GetInt32 (4);
					
			}
			reader.Close ();
			reader = null;
			dbcmd.Dispose ();
			dbcmd = null; 
		*/

		LoadPlayerPrefs ();
		LoadSerializedData ();


	}

	public void SaveScore(int new_hi_score,int new_momos,int new_plats){
		/*
			IDbCommand dbcmd = dbconn.CreateCommand ();
			string sqlQuery = "UPDATE score SET high_score = " + new_hi_score + ",momos = "+ new_momos +",plats = "+new_plats+" where id = 1";
			dbcmd.CommandText = sqlQuery;
			dbcmd.ExecuteNonQuery ();
			dbcmd.Dispose ();
			dbcmd = null;
*/
			SaveSerializedData ();

	}

	public void SaveOptions(/*IDbConnection dbconn,int cntrlmd,int gm2,int dffclty*/){
		/*
			IDbCommand dbcmd = dbconn.CreateCommand ();
			string sqlQuery = "UPDATE options SET gemi2 = " + gemi2 + ",controlmode= " + controlmode + ", difficulty = " + difficulty + " where id = 1";
			dbcmd.CommandText = sqlQuery;
			dbcmd.ExecuteNonQuery ();
			dbcmd.Dispose ();
			dbcmd = null;
*/
			PlayerPrefs.SetInt ("gemi2", gemi2);
			PlayerPrefs.SetInt ("controlmode", controlmode);
			PlayerPrefs.SetInt ("difficulty", difficulty);
			PlayerPrefs.SetInt ("tutorial", tutorial);
	
	}
	public void UpdateOptionData(){
		gemi2 = (int) GameObject.Find ("moaning_slider").GetComponent<Slider> ().value;
		controlmode = (int) GameObject.Find ("controlmode_slider").GetComponent<Slider> ().value;
		tutorial = (int) GameObject.Find ("tutorial_slider").GetComponent<Slider> ().value;
		GameObject df = GameObject.Find ("difficulty");
		int i = 1;
		foreach(Transform df_sl in df.transform){
			if (df_sl.gameObject.GetComponent<Toggle>().isOn) {
				difficulty = i;
			}
			i++;
		}
	}

	public void ShowOptionDataInScreen(){
		GameObject.Find ("moaning_slider").GetComponent<Slider> ().value = gemi2;
		GameObject.Find ("controlmode_slider").GetComponent<Slider> ().value = controlmode;
		GameObject.Find ("tutorial_slider").GetComponent<Slider> ().value = tutorial;
		GameObject df = GameObject.Find ("difficulty");
		int i = 1;
		foreach(Transform df_sl in df.transform){
			if (difficulty == i) {
				df_sl.gameObject.GetComponent<Toggle> ().isOn = true;
			}
			i++;
		}
	}

	public void UpdateGemi2(){
		gemi2 = (gemi2 == 1 ? 0 :  1);
		Debug.Log("los gemidos estan "+(gemi2 == 1 ? "activados" : "desactivados"));
	}
	public void LoadPlayerPrefs(){
		if (PlayerPrefs.HasKey ("gemi2")) {
			gemi2 = PlayerPrefs.GetInt ("gemi2");
		} else {
			PlayerPrefs.SetInt ("gemi2", 0);
		}
		if (PlayerPrefs.HasKey ("controlmode")) {
			gemi2 = PlayerPrefs.GetInt ("controlmode");
		} else {
			PlayerPrefs.SetInt ("controlmode", 0);
		}
		if (PlayerPrefs.HasKey ("difficulty")) {
			gemi2 = PlayerPrefs.GetInt ("difficulty");
		} else {
			PlayerPrefs.SetInt ("difficulty", 1);
		}
		if (PlayerPrefs.HasKey ("tutorial")) {
			gemi2 = PlayerPrefs.GetInt ("tutorial");
		} else {
			PlayerPrefs.SetInt ("tutorial", 1);
		}
		Debug.Log ("wardadito");

	}

	public void LoadSerializedData(){


		if (File.Exists (datapath)) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream fs;
			fs	= File.Open (datapath, FileMode.Open);
			ScoreData scoredata = (ScoreData)bf.Deserialize (fs);
			hi_score = scoredata.highscore;
			momos = scoredata.momos;
			plats = scoredata.plats;
			fs.Close ();
		} else {
			hi_score = 2;
			momos = 2;
			plats = 2;
		}
	}
	public void SaveSerializedData(){
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream fs = File.Create (datapath);
		ScoreData scoredata= new ScoreData (hi_score, momos, plats);
		bf.Serialize (fs, scoredata);
		fs.Close ();

}

[Serializable]
class ScoreData{
	public int highscore,momos,plats;
	public ScoreData(int hi_s,int m,int p){
		highscore = hi_s;
		momos = m;
		plats = p;
	}
}
}