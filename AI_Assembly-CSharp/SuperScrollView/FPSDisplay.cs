// Decompiled with JetBrains decompiler
// Type: SuperScrollView.FPSDisplay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace SuperScrollView
{
  public class FPSDisplay : MonoBehaviour
  {
    private float deltaTime;
    private GUIStyle mStyle;

    public FPSDisplay()
    {
      base.\u002Ector();
    }

    private void Awake()
    {
      this.mStyle = new GUIStyle();
      this.mStyle.set_alignment((TextAnchor) 0);
      this.mStyle.get_normal().set_background((Texture2D) null);
      this.mStyle.set_fontSize(25);
      this.mStyle.get_normal().set_textColor(new Color(0.0f, 1f, 0.0f, 1f));
    }

    private void Update()
    {
      this.deltaTime += (float) (((double) Time.get_deltaTime() - (double) this.deltaTime) * 0.100000001490116);
    }

    private void OnGUI()
    {
      int width = Screen.get_width();
      int height = Screen.get_height();
      Rect rect;
      ((Rect) ref rect).\u002Ector(0.0f, 0.0f, (float) width, (float) (height * 2 / 100));
      string str = string.Format("   {0:0.} FPS", (object) (1f / this.deltaTime));
      GUI.Label(rect, str, this.mStyle);
    }
  }
}
