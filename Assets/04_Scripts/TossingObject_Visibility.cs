using UnityEngine;

public class TossingObject_Visibility : MonoBehaviour
{

    public Renderer[] mrs;

    private int prop_color;
    private int prop_colorMult;
    private int prop_mult;

    private void Awake() {
        mrs = this.GetComponentsInChildren<Renderer>();
        prop_color = Shader.PropertyToID("_HighlightColor");
        prop_colorMult = Shader.PropertyToID("_HighlightColorMult");
        prop_mult = Shader.PropertyToID("_HighlightMult");
    }

    public void Invisible() {
        foreach(Renderer mr in mrs) {
            mr.material.SetFloat(prop_mult, 0);
        }
    }

    public void Visible(Color? _col = null, float? _mult = null) {
        foreach(Renderer mr in mrs) {
            mr.material.SetFloat(prop_mult, 1);
            if(_col != null)
                mr.material.SetColor(prop_color, (Color)_col);
            if(_mult != null)
                mr.material.SetFloat(prop_colorMult, (float)_mult);
        }
        if(_mult != null) target = (float)_mult;
    }

    public void SetColor(Color _col, float? _mult = null) {
        foreach(Renderer mr in mrs) {
            mr.material.SetFloat(prop_mult, 1);
            mr.material.SetColor(prop_color, _col);
            if(_mult != null)
                mr.material.SetFloat(prop_colorMult, (float)_mult);
        }
        if(_mult != null) target = (float)_mult;
    }


    private float target = 1;
    private float colorMult = 25;
    private bool isInterpolation = false;
    public void Impact(float t) {
        isInterpolation = true;
        colorMult = t;
        foreach(Renderer mr in mrs) {
            mr.material.SetFloat(prop_colorMult, colorMult);
        }
    }

    private void Update() {
        if(isInterpolation) {
            colorMult += (target - colorMult) * Time.deltaTime * 10;
            if(Mathf.Abs(target - colorMult) > 0.01f) {
                foreach(Renderer mr in mrs) {
                    mr.material.SetFloat(prop_colorMult, colorMult);
                }
            } else {
                isInterpolation = true;
                foreach(Renderer mr in mrs) {
                    mr.material.SetFloat(prop_colorMult, target);
                }
            }
        }
    }



}
