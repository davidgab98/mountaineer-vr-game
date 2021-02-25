using UnityEngine;

public class ScreenFade : MonoBehaviour {

    private Texture2D texture;
    private float alpha;

    private float fadeDuration;
    private bool fadingOut;
    private bool fadingIn;

    private void Start() {
        texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, new Color(0, 0, 0, 0));
        texture.Apply();
    }

    private void Update() {
        if(fadingOut) {
            alpha += Time.deltaTime / fadeDuration;
            if(alpha >= 1.0f)
                fadingOut = false;
        } else if(fadingIn) {
            alpha -= Time.deltaTime / fadeDuration;
            if(alpha <= 0.0f)
                fadingIn = false;
        }
    }

    private void OnGUI() {
        if(fadingOut || fadingIn) {
            texture.SetPixel(0, 0, new Color(0, 0, 0, alpha));
            texture.Apply();
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), texture);
        }
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