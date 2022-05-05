using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScoreHandler : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    TextMeshProUGUI scoreTextField;

    [SerializeField]
    Slider scoreBarFill;

    [SerializeField]
    Transform scoreBarGO;

    [Range(0, 0.1f)]
    public float slamShrinkFactor;


    private float score = 0;
    private float maxScorePossible;

    private Vector3 baselineBarScale;

    private bool slamming = false;

    /// <summary>
    /// Combo stuff
    /// </summary>
    public float comboTimer;

    [SerializeField]
    GameObject combosGO;

    [SerializeField]
    [Range(0, 10)]
    int comboThreshhold;

    [SerializeField]
    TextMeshProUGUI multiplierTextField;

    [SerializeField]
    Image comboBarFill;

    [SerializeField]
    int[] comboArray;

    private int currentComboIndex = 0;  // gets the current multiplier
    private int currentComboChain = 0;  // how many items ive destroyed / progress towards next multiplier
    private float currentComboTimer = 0;  // how long until current combo expires
    public void Start()
    {
        baselineBarScale = scoreBarGO.transform.localScale;

        ChangeScoreText();
        SetBarFill();

    }

    public void Update()
    {
        if (scoreBarGO.transform.localScale.x <= baselineBarScale.x)
        {
            slamming = false;
        }
        if (slamming)
        {
            scoreBarGO.transform.localScale = Vector3.Lerp(scoreBarGO.transform.localScale, baselineBarScale, slamShrinkFactor);
        }

        currentComboTimer -= Time.deltaTime;
        comboBarFill.fillAmount = currentComboTimer / comboTimer;
        if (currentComboTimer <= 0)
        {
            combosGO.SetActive(false);
            currentComboIndex = 0;
            currentComboChain = 0;
            multiplierTextField.text = "COMBO! " + comboArray[currentComboIndex].ToString() + "x";
        }

    }

    public void UpdateScore(int delta)
    {
        UpdateComboChain();
        score += ApplyComboMultiplier(delta);
        ChangeScoreText();
        SetBarFill();
        BarSlamEffect();

    }

    public float GetScore()
    {
        return score;
    }

    public void AddToMaxScore(float delta)    
    {
        maxScorePossible += delta;
    }

    void ChangeScoreText()
    {
        scoreTextField.text = score.ToString(); // + '/' + maxScorePossible;
    }
    void SetBarFill() 
    {
        scoreBarFill.value = score / maxScorePossible;
    }

    void BarSlamEffect()
    {
        scoreBarGO.transform.localScale = new Vector3(baselineBarScale.x + 1, baselineBarScale.y + 1, 1);
        slamming = true;
    }

    /// <summary>
    /// Stuff having to do with Combos
    /// </summary>
    void UpdateComboChain()
    {
        combosGO.SetActive(true);
        currentComboTimer = comboTimer;
        currentComboChain++;
        if (currentComboChain % comboThreshhold == 0)
        {
            currentComboIndex = Mathf.Min(comboArray.Length - 1, currentComboIndex + 1);
            currentComboChain = 0;
        }
        multiplierTextField.text = "COMBO! " + comboArray[currentComboIndex].ToString() + "x";
    }

    float ApplyComboMultiplier(float delta)
    {
        return delta * comboArray[currentComboIndex];
    }
}
