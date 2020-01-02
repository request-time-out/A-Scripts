// Decompiled with JetBrains decompiler
// Type: PlayfulSystems.LoadingScreen.CameraFade
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace PlayfulSystems.LoadingScreen
{
  public class CameraFade : MonoBehaviour
  {
    private Action onFadeDone;
    private const int guiDepth = -1000;
    private GUIStyle backgroundStyle;
    private Texture2D fadeTexture;
    private Color currentColor;
    private Color targetColor;
    private Color deltaColor;

    public CameraFade()
    {
      base.\u002Ector();
    }

    public void Init()
    {
      this.fadeTexture = new Texture2D(1, 1);
      this.backgroundStyle = new GUIStyle();
      this.backgroundStyle.get_normal().set_background(this.fadeTexture);
    }

    private void OnGUI()
    {
      if (Color.op_Inequality(this.currentColor, this.targetColor))
      {
        if ((double) Mathf.Abs((float) (this.currentColor.a - this.targetColor.a)) < (double) Mathf.Abs((float) this.deltaColor.a) * (double) Time.get_deltaTime())
        {
          this.currentColor = this.targetColor;
          this.SetColor(this.currentColor);
          this.deltaColor = Color.get_clear();
          if (this.onFadeDone != null)
            this.onFadeDone();
        }
        else
          this.SetColor(Color.op_Addition(this.currentColor, Color.op_Multiply(this.deltaColor, Time.get_deltaTime())));
      }
      if (this.currentColor.a > 0.0)
      {
        this.EnableAnim(true);
        GUI.set_depth(-1000);
        GUI.Label(new Rect(-2f, -2f, (float) (Screen.get_width() + 4), (float) (Screen.get_height() + 4)), (Texture) this.fadeTexture, this.backgroundStyle);
      }
      else
        this.EnableAnim(false);
    }

    private void SetColor(Color newColor)
    {
      this.currentColor = newColor;
      this.fadeTexture.SetPixel(0, 0, this.currentColor);
      this.fadeTexture.Apply();
    }

    public void StartFadeFrom(Color color, float fadeDuration, Action onFinished = null)
    {
      if ((double) fadeDuration <= 0.0)
        return;
      this.SetColor(color);
      this.onFadeDone = onFinished;
      this.targetColor = new Color((float) color.r, (float) color.g, (float) color.b, 0.0f);
      this.SetDeltaColor(fadeDuration);
      this.EnableAnim(true);
    }

    public void StartFadeTo(Color color, float fadeDuration, Action onFinished = null)
    {
      if ((double) fadeDuration <= 0.0)
        return;
      this.SetColor(new Color((float) color.r, (float) color.g, (float) color.b, 0.0f));
      this.onFadeDone = onFinished;
      this.targetColor = color;
      this.SetDeltaColor(fadeDuration);
      this.EnableAnim(true);
    }

    public void StartFadeFromTo(
      Color colorStart,
      Color colorEnd,
      float fadeDuration,
      Action onFinished = null)
    {
      if ((double) fadeDuration <= 0.0)
        return;
      this.SetColor(colorStart);
      this.onFadeDone = onFinished;
      this.targetColor = colorEnd;
      this.SetDeltaColor(fadeDuration);
      this.EnableAnim(true);
    }

    private void EnableAnim(bool active)
    {
      ((Behaviour) this).set_enabled(active);
    }

    private void SetDeltaColor(float duration)
    {
      this.deltaColor = Color.op_Division(Color.op_Subtraction(this.targetColor, this.currentColor), duration);
    }

    public bool IsFading()
    {
      return ((Behaviour) this).get_enabled() && Color.op_Inequality(this.currentColor, this.targetColor);
    }
  }
}
