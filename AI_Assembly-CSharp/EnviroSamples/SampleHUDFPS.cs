// Decompiled with JetBrains decompiler
// Type: EnviroSamples.SampleHUDFPS
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace EnviroSamples
{
  public class SampleHUDFPS : MonoBehaviour
  {
    public Rect startRect;
    public bool updateColor;
    public bool allowDrag;
    public float frequency;
    public int nbDecimal;
    private float accum;
    private int frames;
    private Color color;
    private string sFPS;
    private GUIStyle style;

    public SampleHUDFPS()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      this.StartCoroutine(this.FPS());
    }

    private void Update()
    {
      this.accum += Time.get_timeScale() / Time.get_deltaTime();
      ++this.frames;
    }

    [DebuggerHidden]
    private IEnumerator FPS()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new SampleHUDFPS.\u003CFPS\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    private void OnGUI()
    {
      if (this.style == null)
      {
        this.style = new GUIStyle(GUI.get_skin().get_label());
        this.style.get_normal().set_textColor(Color.get_white());
        this.style.set_alignment((TextAnchor) 4);
      }
      GUI.set_color(!this.updateColor ? Color.get_white() : this.color);
      // ISSUE: method pointer
      this.startRect = GUI.Window(0, this.startRect, new GUI.WindowFunction((object) this, __methodptr(DoMyWindow)), string.Empty);
    }

    private void DoMyWindow(int windowID)
    {
      GUI.Label(new Rect(0.0f, 0.0f, ((Rect) ref this.startRect).get_width(), ((Rect) ref this.startRect).get_height()), this.sFPS + " FPS", this.style);
      if (!this.allowDrag)
        return;
      GUI.DragWindow(new Rect(0.0f, 0.0f, (float) Screen.get_width(), (float) Screen.get_height()));
    }
  }
}
