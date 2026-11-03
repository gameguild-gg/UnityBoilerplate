using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class DialogueCanvas : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static DialogueCanvas Instance;
    [SerializeField] Image spriteImage;
    [SerializeField] TMPro.TMP_Text UITextMeshPro;
    [SerializeField] string[] text;

    private InputAction interactAction; //I need to remeber how the new input system works lol
    void Start()
    {
        this.gameObject.SetActive(false);
    }

    public void ContinueThroughText()
    {
        if(text==null || text.Length == 0)
        {
            //No more text to show
            this.gameObject.SetActive(false);
        }

        else
        {
            //display next text in text array
        }
    }

    public void UpdateText(string[] txt)
    {
        text = txt;
    }

    public void UpdateImage(Image image)
    {
        spriteImage=image;
    }
}
