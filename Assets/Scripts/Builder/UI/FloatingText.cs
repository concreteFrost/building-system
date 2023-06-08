using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public Vector2 textStartPosistion;
    public Vector2 textEndPosistion;
    public GameObject floatingText;

    public List<GameObject> floatingList;
    public int maxTextSpawn;

    private Queue<string> coroutineQueue; // Queue to store coroutine parameters
    private bool isProcessingQueue; // Flag to indicate if the queue is being processed
    public float coroutineDelay;

    // Start is called before the first frame update
    void Start()
    {
        textStartPosistion = new Vector2(Screen.width / 2, Screen.height / 2);
        coroutineQueue = new Queue<string>(); // Initialize the coroutine queue
        isProcessingQueue = false;

        for (int i = 0; i < maxTextSpawn; i++)
        {
            GameObject textClone = Instantiate(floatingText);
            textClone.transform.SetParent(this.transform, false);
            textClone.transform.position = textStartPosistion;
            floatingList.Add(textClone);
            textClone.SetActive(false);
        }
    }

    public void DeductionNote(float val)
    {
        string coroutineParam = "-" + val.ToString();
        EnqueueCoroutine(coroutineParam);
    }

    public void DepositNote(float val)
    {
        string coroutineParam = "+" + val.ToString();
        EnqueueCoroutine(coroutineParam);
    }

    private void EnqueueCoroutine(string coroutineParam)
    {
        coroutineQueue.Enqueue(coroutineParam); // Enqueue coroutine parameters

        if (!isProcessingQueue)
        {
            StartCoroutine(ProcessCoroutineQueue());
        }
    }

    private IEnumerator ProcessCoroutineQueue()
    {
        isProcessingQueue = true;

        while (coroutineQueue.Count > 0)
        {
            string coroutineParam = coroutineQueue.Dequeue(); // Dequeue coroutine parameters
            StartCoroutine(ActivateAndTransform(coroutineParam));
            yield return new WaitForSeconds(coroutineDelay);
        }

        isProcessingQueue = false;
    }

    public IEnumerator ActivateAndTransform(string textValue)
    {
        var disabledText = floatingList.FirstOrDefault(x => !x.activeInHierarchy);

        if (disabledText != null)
        {
            disabledText.SetActive(true);
            var textData = disabledText.GetComponent<TextMeshProUGUI>();

            bool isDeducting = textValue.StartsWith("-");
            var color = isDeducting ? Color.red : Color.green;
            textData.color = color;
            textData.text = textValue;

            float elapsedTime = 0f;
            float duration = 3f; // Adjust the duration as desired

            Vector2 startPos = disabledText.transform.position;
            Vector2 endPos = new Vector2(disabledText.transform.position.x, 700);

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / duration);
                textData.color = new Color(textData.color.r, textData.color.g, textData.color.b, textData.color.a - 1 * Time.deltaTime);
                disabledText.transform.position = Vector2.Lerp(startPos, endPos, t);
                yield return null;
            }

            disabledText.SetActive(false);
            disabledText.transform.position = textStartPosistion;
        }
    }
}
