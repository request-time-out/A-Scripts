// Decompiled with JetBrains decompiler
// Type: Exploder.HUDFPS
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

namespace Exploder
{
  public class HUDFPS : MonoBehaviour
  {
    public float updateInterval;
    private float accum;
    private int frames;
    private float timeleft;
    private Text text;

    public HUDFPS()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      this.text = (Text) ((Component) this).GetComponent<Text>();
      if (!Object.op_Implicit((Object) this.text))
      {
        Debug.Log((object) "UtilityFramesPerSecond needs a Text component!");
        ((Behaviour) this).set_enabled(false);
      }
      else
        this.timeleft = this.updateInterval;
    }

    private void Update()
    {
      this.timeleft -= Time.get_deltaTime();
      this.accum += Time.get_timeScale() / Time.get_deltaTime();
      ++this.frames;
      if ((double) this.timeleft > 0.0)
        return;
      float num = this.accum / (float) this.frames;
      this.text.set_text(string.Format("{0:F2} FPS", (object) num));
      if ((double) num < 30.0)
        ((Graphic) this.text).get_material().set_color(Color.get_yellow());
      else if ((double) num < 10.0)
        ((Graphic) this.text).get_material().set_color(Color.get_red());
      else
        ((Graphic) this.text).get_material().set_color(Color.get_black());
      this.timeleft = this.updateInterval;
      this.accum = 0.0f;
      this.frames = 0;
    }
  }
}
