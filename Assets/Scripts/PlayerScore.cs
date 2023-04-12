using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    
    private int score = 0;
    
    [SerializeField] private PhoneController phoneController;
    
    public void Start()
    {
        if (phoneController == null)
        {
            phoneController = FindObjectOfType<PhoneController>();
        }
    }
    
    public void Increment(int score)
    {
        this.score += score;
        phoneController.SetScoreText(this.score.ToString());
    }
    
    public int Get()
    {
        return this.score;
    }
    
}
