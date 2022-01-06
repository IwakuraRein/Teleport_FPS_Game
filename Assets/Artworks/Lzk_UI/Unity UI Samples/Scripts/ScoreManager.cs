using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Text score1Text, score2Text;
    public int score1, score2;
    void Start()
    {
        score1 = 0;
        score2 = 0;
    }

    // Update is called once per frame
    void Update()
    {

        score1Text.text = score1.ToString();
        score2Text.text = score2.ToString();

    }
    public void AddOurPoint()
    {
        score1++;
    }
    public void AddEnemyPoint()
    {
        score2++;
    }
}
