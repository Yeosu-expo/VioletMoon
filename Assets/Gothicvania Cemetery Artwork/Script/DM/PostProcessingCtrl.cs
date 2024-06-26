using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingCtrl : MonoBehaviour
{
    private static PostProcessingCtrl _instance;
    public Volume volume;
    private Vignette vignette;
    private ColorAdjustments colorAdjustments;
    private ChromaticAberration chromaticAberration;
    private LiftGammaGain liftGammaGain;

    public Color hitColor = new Color(0.509434f, 0.007208958f, 0.007208958f, 1);
    public Color defaultColor = new Color(0.8926573f, 0, 1, 1);
    public Vector4 levelUpColor = new Vector4(1, 0.41f, 0.86f, 1);

    private float defaultVignetteIntnensity = 0.2f;
    private float defaultVignetteSmoothness = 0.6f;

    private bool resetVigSign = false;
    public delegate void CallBackFunction();

    // ?????????? ???????? ?????? ?? ?????? ???? ????????
    public static PostProcessingCtrl Instance
    {
        get
        {
            // ?????????? ?????? ?????? ??????????
            if (_instance == null)
            {
                _instance = FindObjectOfType<PostProcessingCtrl>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(PostProcessingCtrl).ToString());
                    _instance = singletonObject.AddComponent<PostProcessingCtrl>();
                }
            }
            return _instance;
        }
    }

    // ?? ???????? ?????????? ???? ?? ???????? ?????? ??????
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        
        if (!volume.profile.TryGet(out vignette))
        {
            Debug.LogError("Vignette ?????? ???? ?? ????????.");
        }
        if (!volume.profile.TryGet(out colorAdjustments))
        {
            Debug.LogError("Color Adjustments ?????? ???? ?? ????????.");
        }
        if (!volume.profile.TryGet(out chromaticAberration))
        {
            Debug.LogError("Chromatic Aberration ?????? ???? ?? ????????.");
        }
        if (!volume.profile.TryGet(out liftGammaGain))
        {
            Debug.LogError("Lift Gamma Gain ?????? ???? ?? ????????.");
        }

        DefaultEffect();

        //PostProcessingCtrl.Instance.StartSmoothnessCycle(2, 4, hitColor, 0.5f); // È÷Æ®
        //StartCoroutine(PostProcessingCtrl.Instance.ChangeChromTimes(4, 2, 2, 1, 0.2f)); // È¥¶õ
        //StartCoroutine(ChangeGamma(5, levelUpColor)); // ½ºÅ³ Èí¼ö


        //StartChangeFG(FilmGrainLookup.Medium1, 0.43f, 0.05f, 10);
        //SetStartSceneEffect();
        //PostProcessingCtrl.Instance.FadeOutAndIn(2, 2); // ÆäÀÌµå ÀÎ ¾Æ¿ô
    }

    private void DefaultEffect()
    {
        vignette.color.value = defaultColor;
        vignette.intensity.value = defaultVignetteIntnensity;
        vignette.smoothness.value = defaultVignetteSmoothness;
        chromaticAberration.intensity.value = 0;
    }

    public void SetStartSceneEffect()
    {
        StartSmoothnessCycle(4, 4, defaultColor, 0.4f, 0.2f, 0.6f);
    }

    private void Update()
    {
        if (resetVigSign) StartCoroutine(ResetVignette());
    }

    public void FadeOutAndIn(float st_itv, float ed_itv)
    {
        Debug.Log("!!!!!!!!!!!!!!!!!!!!!");
        StartColorAdjustments(st_itv, ed_itv, -15);
    }

    public void StartColorAdjustments(float st_itv, float ed_itv, float exposure)
    {
        StartCoroutine(ChangeColorAdjustment(st_itv, ed_itv, exposure));
    }

    public void StartViggnette(float interval, float intentsity)
    {
        StartCoroutine(ChangeVignette(interval, intentsity));
    }

    IEnumerator ChangeColorAdjustment(float st_itv, float ed_itv, float exposure)
    {
        float elapsedTime = 0f;
        while(elapsedTime < st_itv)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / st_itv);
            colorAdjustments.postExposure.value = Mathf.Lerp(0, exposure, t);

            yield return null;
        }

        elapsedTime = 0f;
        while (elapsedTime < ed_itv)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / ed_itv);
            colorAdjustments.postExposure.value = Mathf.Lerp(exposure, 0, t);

            yield return null;
        }
    }

    public IEnumerator ChangeChromTimes(float times, float st_itv, float ed_itv, float intensity, float mid_intensity)
    {
        float elapsedTime = 0f;
        while (elapsedTime < 0.5f)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / 0.5f);
            chromaticAberration.intensity.value = Mathf.Lerp(0, mid_intensity, t);

            yield return null;
        }
        for (int i = 0; i < times; i++)
        {
            yield return StartCoroutine(ChangeChrom(st_itv, ed_itv, intensity, mid_intensity));
        }
        elapsedTime = 0f;
        while (elapsedTime < 0.5f)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / 0.5f);
            chromaticAberration.intensity.value = Mathf.Lerp(mid_intensity, 0, t);

            yield return null;
        }
    }

    IEnumerator ChangeChrom(float st_itv, float ed_itv, float intensity, float mid_intensity)
    {
        float elapsedTime = 0f;
        while (elapsedTime < st_itv)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / st_itv);
            chromaticAberration.intensity.value = Mathf.Lerp(mid_intensity, intensity, t);

            yield return null;
        }

        elapsedTime = 0f;
        while (elapsedTime < ed_itv)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / ed_itv);
            chromaticAberration.intensity.value = Mathf.Lerp(intensity, mid_intensity, t);

            yield return null;
        }
    }

    public IEnumerator ChangeGamma(float speed, Vector4 targetGamma)
    {
        Vector4 originGamma = liftGammaGain.gamma.value;

        // ?????????? targetGamma?? ????
        while (!VectorsAreApproximatelyEqual(liftGammaGain.gamma.value, targetGamma))
        {
            liftGammaGain.gamma.value = Vector4.Lerp(liftGammaGain.gamma.value, targetGamma, Time.deltaTime * speed);
            yield return null;
        }

        // ?????????? originGamma?? ????
        while (!VectorsAreApproximatelyEqual(liftGammaGain.gamma.value, originGamma))
        {
            liftGammaGain.gamma.value = Vector4.Lerp(liftGammaGain.gamma.value, originGamma, Time.deltaTime * speed);
            yield return null;
        }
    }

    // ?? ?????? ???? ?????? ???????? ????
    bool VectorsAreApproximatelyEqual(Vector4 vec1, Vector4 vec2, float tolerance = 0.01f)
    {
        return Vector4.Distance(vec1, vec2) < tolerance;
    }


    IEnumerator ChangeVignette(float interval, float intentsity)
    {
        float elapsedTime = 0f;
        float st = vignette.intensity.value;
        while (elapsedTime < interval)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / interval);
            vignette.intensity.value = Mathf.Lerp(st, intentsity, t);

            yield return null;
        }
    }

    public void StartSmoothnessCycle(int times, float duration, Color color, float intensity, float stsn = 0.2f, float edsn = 0.4f)
    {
        vignette.intensity.value = intensity;
        StartCoroutine(SmoothnessCycle(times, duration, color, SetResetVigSign, stsn, edsn));
    }

    private IEnumerator SmoothnessCycle(int times, float duration, Color color, CallBackFunction callback, float stsn, float edsn)
    {
        float halfDuration = duration / 2f;
        
        vignette.color.value = color;
        for (int i = 0; i < times; i++)
        {
            yield return StartCoroutine(ChangeSmoothness(stsn, edsn, halfDuration));
            yield return StartCoroutine(ChangeSmoothness(edsn, stsn, halfDuration));
        }

        callback();
    }

    private IEnumerator ChangeSmoothness(float startValue, float endValue, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            vignette.smoothness.value = Mathf.Lerp(startValue, endValue, t);
            yield return null;
        }
        vignette.smoothness.value = endValue;
    }

    /*
    private IEnumerator ReturnVignette(float startValue, float endValue, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            vignette.intensity.value = Mathf.Lerp(startValue, endValue, t);
            vignette.smoothness.value = Mathf.Lerp(startValue, endValue, t);
            yield return null;
        }
        vignette.intensity.value = endValue;
        vignette.smoothness.value = endValue;
    }
    */

    public void SetResetVigSign()
    {
        resetVigSign = true;
    }

    private IEnumerator ResetVignette(float duration = 2f)
    {
        resetVigSign = false;
        float elapsedTime = 0f;

        // ???? ?????? ???? ????
        float currentVignetteIntensity = vignette.intensity.value;
        Color currentVignetteColor = vignette.color.value;
        float currentVignetteSmoothness = vignette.smoothness.value;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            // ?????? ?? ?????????? ?????????? ????
            vignette.intensity.value = Mathf.Lerp(currentVignetteIntensity, defaultVignetteIntnensity, t);
            vignette.color.value = Color.Lerp(currentVignetteColor, defaultColor, t);
            vignette.smoothness.value = Mathf.Lerp(currentVignetteSmoothness, defaultVignetteSmoothness, t);

            yield return null;
        }

        // ?????????? ?????????? ????
        vignette.intensity.value = defaultVignetteIntnensity;
        vignette.color.value = defaultColor;
        vignette.smoothness.value = defaultVignetteSmoothness;
    }
}
