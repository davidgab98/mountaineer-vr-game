using UnityEngine;
using UnityEngine.UI;

public class ImageFader : MonoBehaviour {

    private Image image;
    private float alpha;

    private float fadeDuration;
    private bool fadingOut;
    private bool fadingIn;

    private void Awake() {
        image = GetComponent<Image>();
    }

    private void Start() {
        image.color = new Color(0, 0, 0, 0);
    }

    private void Update() {
        if(fadingOut) {
            UpdateImageAlphaValue();
            alpha += Time.deltaTime / fadeDuration;
            if(alpha >= 1.0f)
                fadingOut = false;
        } else if(fadingIn) {
            UpdateImageAlphaValue();
            alpha -= Time.deltaTime / fadeDuration;
            if(alpha <= 0.0f)
                fadingIn = false;
        }
    }

    void UpdateImageAlphaValue() {
        image.color = new Color(0, 0, 0, alpha);
    }

    public void FadeOut(float duration) {
        if(!fadingOut) {
            alpha = 0;
            fadingOut = true;
            fadeDuration = duration;
        }
    }

    public void FadeIn(float duration) {
        if(!fadingIn) {
            alpha = 1;
            fadingIn = true;
            fadeDuration = duration;
        }
    }
}