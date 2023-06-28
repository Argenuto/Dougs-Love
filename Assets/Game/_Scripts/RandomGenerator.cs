using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class TimeTag {
    public float timeInSecs = 0;
    [Range(0, 1)]
    public float prob;
    public TimeTag(float timeInSecs, float prob)
    {
        this.timeInSecs = timeInSecs;
        this.prob = prob;
    }
}
[System.Serializable]
public class Objeto{
	[Range(0,1)]
	public float prob;
	public GameObject go;
    public List<TimeTag> timeTags;
}
public enum IncrementalType { byTime,byOther}

public class RandomGenerator : MonoBehaviour {
    public IncrementalType selectedIncrementalType= IncrementalType.byTime;
    public Objeto[] elementos;
    public float[] prob_table;
	int[] aliast;
    private Dictionary<float, List<Pair>> myTimeLine;
    private List<KeyValuePair<float, List<Pair>>> tlList;
    private float acumVariable=0;
    private int i=0;
    public float speedMultiplier=1;

    public float AcumVariable { get => acumVariable; set => acumVariable = value; }

    // Use this for initialization
    void Awake () {
        CreateTimeLine();
        //Debug.Log(tlList[i].Key);
    }

    // Update is called once per frame
    void Update () {
        if (selectedIncrementalType == IncrementalType.byTime)
        {
            acumVariable += Time.deltaTime*speedMultiplier;
        
        }
        try
        {
            if (tlList[i].Key <= acumVariable && i < tlList.Count)
            {
                //Debug.Log("entre");
                UpdateProbs();
                i++;
            }
        }
        catch (Exception)
        {
            Debug.Log("i: " + i+"//"+"tlist.count: "+tlList.Count);

        }

        //Debug.Log(acumSecs);

    }

    private void UpdateProbs()
    {
        foreach (Pair p in myTimeLine[tlList[i].Key])
            elementos[p.index].prob = p.prob;
      //  Debug.Log("probs cambiados en "+acumVariable);
        CreateAliasTables();
    }

    void OnValidate(){
        CreateTimeLine();
		CreateAliasTables();
        foreach (Objeto o in elementos)
            if (o.timeTags.Count == 0)
                o.timeTags.Add(new TimeTag(0, o.prob));
          
                
	 }

  
    public void CreateAliasTables(){
		//create variables
		float[] norm_tbl = new float[elementos.Length];
		int items = elementos.Length;
		prob_table = new float[items];
		aliast = new int[items];
		//Create 2 WorkList: Small & Large
		ArrayList large = new ArrayList(),small = new ArrayList();

		float totalweight = TotalWeight (elementos);
		//	Debug.Log (totalweight);
		//set probability table
		if (totalweight > 0) {
			for (int i = 0; i < items; i++) {
				float freq = elementos [i].prob;
				//				Debug.Log ("frequency: " + freq);
				float prob = (freq/totalweight) * items;
				//Debug.Log ("probability at item #" + i + ": " + prob);
				norm_tbl [i] = prob;
				// set the list with index numbers of the stuff list according to their sizes
				if (prob >= 1) {
					large.Add (i);
				} else {
					small.Add (i);
				}
			} 

		} else {
			Debug.Log ("prro, no hay nada... :'v");
		}

		//		Debug.Log ("largesize: " + large.Count);
		//	Debug.Log ("smallsize: " + small.Count);

		while (large.Count > 0 && small.Count > 0) {

			int l = (int) large [0], g = (int) small[0];
			large.RemoveAt (0);
			small.RemoveAt (0);
			prob_table[g] = norm_tbl[g];
			aliast[g] = l;
			norm_tbl [l] = (norm_tbl [l] + norm_tbl [g]) - 1;

			if (norm_tbl [l] >= 1) {
				large.Add (l);
				//	Debug.Log ("bien");
			} else {
				small.Add (l);
				//	Debug.Log ("Bien x2");
			}

		}
		//Debug.Log ("largesize after elimination: " + large.Count);
		//Debug.Log ("smallsize after elimination: " + small.Count);
		/*foreach (object o in large) {
		Debug.Log ((int)o+"con probabilidad: "+norm_tbl[(int)o]);

	}*/
		while(large.Count > 0) {
			int l = (int)large [0];
			large.RemoveAt (0);
			norm_tbl [l] = 1;
			prob_table[l] = norm_tbl[l];
		}


		while(small.Count > 0) {
			int g = (int) small [0];
			small.RemoveAt (0);
			norm_tbl [g] = 1;
			prob_table[g]= norm_tbl[g];

		}


	}
	public Objeto TakeElement(){
		int plat_num = 0;	
		int dice = UnityEngine.Random.Range (0, prob_table.Length);
		float coin = UnityEngine.Random.Range (0, 1f);
		if (coin <= prob_table [dice]) {
			plat_num = dice;
		} else {
			plat_num = aliast [dice];
		}
		//Debug.Log (plat_num);
		return elementos[plat_num];
	}


	float TotalWeight(Objeto[] stuff_list){
		float weight = 0;
		foreach (Objeto go in stuff_list) {
			weight += go.prob;


		}
		return weight;
	}
    void CreateTimeLine() {
        myTimeLine = new Dictionary<float,List< Pair>>();
        float tl = 0;
        int i = 0;
        foreach (Objeto o in elementos)
        {
            tl = 0;
            foreach (TimeTag tg in o.timeTags) {
                tl += tg.timeInSecs;
                if (!myTimeLine.ContainsKey(tl))
                {

                    myTimeLine.Add(tl, new List<Pair>
                    {
                        new Pair(i, tg.prob)
                    });
                }
                else
                    myTimeLine[tl].Add(new Pair(i, tg.prob));
            }
            i++;
        }
        tlList = myTimeLine.ToList();
        tlList.Sort((pair1,pair2) => pair1.Key.CompareTo(pair2.Key));
       /* foreach (float k in myTimeLine.Keys)
            foreach (Pair pr in myTimeLine[k])
                Debug.Log("key:  "+ k+"____value: "+pr.ToString());*/
        
        
            
    }
    public void ResetNumbers() {
        i = 0;
        acumVariable = 0;
        UpdateProbs();
    }
}
public struct Pair {
    public int index;
    public float prob;

    public Pair(int index, float prob)
    {
        this.index = index;
        this.prob = prob;
    }
    public override string ToString()
    {
        return "index: " + index + "____prob:" + prob;
    }

}
