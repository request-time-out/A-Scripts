// Decompiled with JetBrains decompiler
// Type: DemoGUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class DemoGUI : MonoBehaviour
{
  public Texture HUETexture;
  public Material mat;
  public Position[] Positions;
  public GameObject[] Prefabs;
  private int currentNomber;
  private GameObject currentInstance;
  private GUIStyle guiStyleHeader;
  private float colorHUE;
  private float dpiScale;

  public DemoGUI()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    if ((double) Screen.get_dpi() < 1.0)
      this.dpiScale = 1f;
    this.dpiScale = (double) Screen.get_dpi() >= 200.0 ? Screen.get_dpi() / 200f : 1f;
    this.guiStyleHeader.set_fontSize((int) (15.0 * (double) this.dpiScale));
    this.guiStyleHeader.get_normal().set_textColor(new Color(1f, 1f, 1f));
    this.currentInstance = (GameObject) Object.Instantiate<GameObject>((M0) this.Prefabs[this.currentNomber], ((Component) this).get_transform().get_position(), (Quaternion) null);
  }

  private void OnGUI()
  {
    if (GUI.Button(new Rect(10f * this.dpiScale, 15f * this.dpiScale, 105f * this.dpiScale, 30f * this.dpiScale), "Previous Effect"))
      this.ChangeCurrent(-1);
    if (GUI.Button(new Rect(130f * this.dpiScale, 15f * this.dpiScale, 105f * this.dpiScale, 30f * this.dpiScale), "Next Effect"))
      this.ChangeCurrent(1);
    GUI.Label(new Rect(300f * this.dpiScale, 15f * this.dpiScale, 100f * this.dpiScale, 20f * this.dpiScale), "Prefab name is \"" + ((Object) this.Prefabs[this.currentNomber]).get_name() + "\"  \r\nHold any mouse button that would move the camera", this.guiStyleHeader);
    GUI.DrawTexture(new Rect(12f * this.dpiScale, 80f * this.dpiScale, 220f * this.dpiScale, 15f * this.dpiScale), this.HUETexture, (ScaleMode) 0, false, 0.0f);
    float colorHue = this.colorHUE;
    this.colorHUE = GUI.HorizontalSlider(new Rect(12f * this.dpiScale, 105f * this.dpiScale, 220f * this.dpiScale, 15f * this.dpiScale), this.colorHUE, 0.0f, 1530f);
    if ((double) Mathf.Abs(colorHue - this.colorHUE) > 0.001)
      this.ChangeColor();
    GUI.Label(new Rect(240f * this.dpiScale, 105f * this.dpiScale, 30f * this.dpiScale, 30f * this.dpiScale), "Effect color", this.guiStyleHeader);
  }

  private void ChangeColor()
  {
    Color color1 = this.Hue(this.colorHUE / (float) byte.MaxValue);
    foreach (Renderer componentsInChild in (Renderer[]) this.currentInstance.GetComponentsInChildren<Renderer>())
    {
      Material material = componentsInChild.get_material();
      if (!Object.op_Equality((Object) material, (Object) null) && material.HasProperty("_TintColor"))
      {
        Color color2 = material.GetColor("_TintColor");
        color1.a = color2.a;
        material.SetColor("_TintColor", color1);
      }
    }
    Light componentInChildren = (Light) this.currentInstance.GetComponentInChildren<Light>();
    if (!Object.op_Inequality((Object) componentInChildren, (Object) null))
      return;
    componentInChildren.set_color(color1);
  }

  private Color Hue(float H)
  {
    Color color;
    ((Color) ref color).\u002Ector(1f, 0.0f, 0.0f);
    if ((double) H >= 0.0 && (double) H < 1.0)
      ((Color) ref color).\u002Ector(1f, 0.0f, H);
    if ((double) H >= 1.0 && (double) H < 2.0)
      ((Color) ref color).\u002Ector(2f - H, 0.0f, 1f);
    if ((double) H >= 2.0 && (double) H < 3.0)
      ((Color) ref color).\u002Ector(0.0f, H - 2f, 1f);
    if ((double) H >= 3.0 && (double) H < 4.0)
      ((Color) ref color).\u002Ector(0.0f, 1f, 4f - H);
    if ((double) H >= 4.0 && (double) H < 5.0)
      ((Color) ref color).\u002Ector(H - 4f, 1f, 0.0f);
    if ((double) H >= 5.0 && (double) H < 6.0)
      ((Color) ref color).\u002Ector(1f, 6f - H, 0.0f);
    return color;
  }

  private void ChangeCurrent(int delta)
  {
    this.currentNomber += delta;
    if (this.currentNomber > this.Prefabs.Length - 1)
      this.currentNomber = 0;
    else if (this.currentNomber < 0)
      this.currentNomber = this.Prefabs.Length - 1;
    if (Object.op_Inequality((Object) this.currentInstance, (Object) null))
      Object.Destroy((Object) this.currentInstance);
    Vector3 position = ((Component) this).get_transform().get_position();
    if (this.Positions[this.currentNomber] == Position.Bottom)
    {
      ref Vector3 local = ref position;
      local.y = (__Null) (local.y - 1.0);
    }
    if (this.Positions[this.currentNomber] == Position.Bottom02)
    {
      ref Vector3 local = ref position;
      local.y = (__Null) (local.y - 0.800000011920929);
    }
    this.currentInstance = (GameObject) Object.Instantiate<GameObject>((M0) this.Prefabs[this.currentNomber], position, (Quaternion) null);
  }
}
