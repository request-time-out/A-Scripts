// Decompiled with JetBrains decompiler
// Type: DigitalRuby.RainMaker.DemoScript2D
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

namespace DigitalRuby.RainMaker
{
  public class DemoScript2D : MonoBehaviour
  {
    public Slider RainSlider;
    public RainScript2D RainScript;

    public DemoScript2D()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      RainScript2D rainScript = this.RainScript;
      float num1 = 0.5f;
      this.RainSlider.set_value(num1);
      double num2 = (double) num1;
      rainScript.RainIntensity = (float) num2;
      this.RainScript.EnableWind = true;
    }

    private void Update()
    {
      float num = (float) (Camera.get_main().ViewportToWorldPoint(Vector3.get_one()).x - Camera.get_main().ViewportToWorldPoint(Vector3.get_zero()).x);
      if (Input.GetKey((KeyCode) 276))
      {
        ((Component) Camera.get_main()).get_transform().Translate(Time.get_deltaTime() * (float) -((double) num * 0.100000001490116), 0.0f, 0.0f);
      }
      else
      {
        if (!Input.GetKey((KeyCode) 275))
          return;
        ((Component) Camera.get_main()).get_transform().Translate(Time.get_deltaTime() * (num * 0.1f), 0.0f, 0.0f);
      }
    }

    public void RainSliderChanged(float val)
    {
      this.RainScript.RainIntensity = val;
    }

    public void CollisionToggleChanged(bool val)
    {
      this.RainScript.CollisionMask = LayerMask.op_Implicit(!val ? 0 : -1);
    }
  }
}
