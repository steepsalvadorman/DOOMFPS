using TMPro;
using UnityEngine;

public class DoomFace : MonoBehaviour
{
    PlayerController playerController;
    [SerializeField] private Animator animator;
    [SerializeField] TextMeshProUGUI healtText;
    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }



    private void Update()
    {
        UpdateHealt();
    }



    private void UpdateHealt()
    {
        float healt = playerController.health * 10;
        healtText.text = healt.ToString() + "%";

        if (healt < 76 && healt > 50)
        {
            animator.SetTrigger("75%");
        }
        else if (healt < 51 && healt > 25)
        {
            animator.SetTrigger("50%");
        }
        else if (healt < 26 && healt > 0)
        {
            animator.SetTrigger("25%");
        }
        else if (healt < 0)
        {
            healtText.text = "0%";
        }
    }
}
