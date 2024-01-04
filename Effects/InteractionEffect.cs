using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;

public class InteractionEffect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private RectTransform rectTransform;
    private Image image;
    private Color defaultColor;
    
    [Header ("Effects")]
    [SerializeField] private float scaleDuration = 0.1f;
    [SerializeField] private float targetScaleFactor = 0.8f;
    private Vector3 originalScale = new Vector3(1f, 1f, 1f);
    [SerializeField] private float targetBrightness = 0.8f;
    private float originalBrightness = 1f;

    private Coroutine interactionEffect;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();

        defaultColor = image.color;
        originalScale = rectTransform.localScale;
    }
    private void OnDisable()
    {
        rectTransform.localScale = originalScale;
        image.color = defaultColor; 
    }

    private void OnDestroy()
    {
        if(interactionEffect != null) StopCoroutine(interactionEffect);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(interactionEffect != null) StopCoroutine(interactionEffect);
        interactionEffect = StartCoroutine(OnInteraction(originalScale * targetScaleFactor, targetBrightness));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(interactionEffect != null) StopCoroutine(interactionEffect);
        interactionEffect = StartCoroutine(OnInteraction(originalScale, originalBrightness));
    }

    private IEnumerator OnInteraction(Vector3 targetScale, float targetBrightness)
    {
        float elapsedTime = 0;
        Vector3 initialScale = rectTransform.localScale;
        Color originalColor = image.color;
        Color.RGBToHSV(originalColor, out float h, out float s, out float v);
        while (elapsedTime < scaleDuration)
        {
            float lerpAmount = elapsedTime / scaleDuration;

            rectTransform.localScale = Vector3.Lerp(initialScale, targetScale, lerpAmount);

            float newBrightness = Mathf.Lerp(v, targetBrightness, lerpAmount);

            Color newColor = SetBrightness(originalColor, newBrightness);
            image.color = newColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rectTransform.localScale = targetScale;
    }

    private Color SetBrightness(Color color, float brightness)
    {
        Color.RGBToHSV(color, out float h, out float s, out float v);
        v = brightness;
        return Color.HSVToRGB(h, s, v);
    }
}
