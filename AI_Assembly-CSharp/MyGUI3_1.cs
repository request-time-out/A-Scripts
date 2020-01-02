// Decompiled with JetBrains decompiler
// Type: MyGUI3_1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

public class MyGUI3_1 : MonoBehaviour
{
  public Texture HUETexture;
  public int CurrentPrefabNomber;
  public float UpdateInterval;
  public Light DirLight;
  public GameObject Target;
  public GameObject TargetForRay;
  public GameObject TopPosition;
  public GameObject MiddlePosition;
  public Vector3 defaultRobotPos;
  public GameObject BottomPosition;
  public GameObject Plane1;
  public GameObject Plane2;
  public Material[] PlaneMaterials;
  public MyGUI3_1.GuiStat[] GuiStats;
  public GameObject[] Prefabs;
  private float oldLightIntensity;
  private Color oldAmbientColor;
  private GameObject currentGo;
  private bool isDay;
  private bool isDefaultPlaneTexture;
  private int current;
  private EffectSettings effectSettings;
  private bool isReadyEffect;
  private Quaternion defaultRobotRotation;
  private float colorHUE;
  private GUIStyle guiStyleHeader;
  private float dpiScale;

  public MyGUI3_1()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    if ((double) Screen.get_dpi() < 1.0)
      this.dpiScale = 1f;
    this.dpiScale = (double) Screen.get_dpi() >= 200.0 ? Screen.get_dpi() / 200f : 1f;
    this.oldAmbientColor = RenderSettings.get_ambientLight();
    this.oldLightIntensity = this.DirLight.get_intensity();
    this.guiStyleHeader.set_fontSize((int) (15.0 * (double) this.dpiScale));
    this.guiStyleHeader.get_normal().set_textColor(new Color(1f, 1f, 1f));
    this.current = this.CurrentPrefabNomber;
    this.InstanceCurrent(this.GuiStats[this.CurrentPrefabNomber]);
  }

  private void InstanceEffect(Vector3 pos)
  {
    this.currentGo = (GameObject) Object.Instantiate<GameObject>((M0) this.Prefabs[this.current], pos, this.Prefabs[this.current].get_transform().get_rotation());
    this.effectSettings = (EffectSettings) this.currentGo.GetComponent<EffectSettings>();
    this.effectSettings.Target = this.GetTargetObject(this.GuiStats[this.current]);
    this.effectSettings.EffectDeactivated += new EventHandler<EventArgs>(this.effectSettings_EffectDeactivated);
    if (this.GuiStats[this.current] == MyGUI3_1.GuiStat.Middle)
    {
      this.currentGo.get_transform().set_parent(this.GetTargetObject(MyGUI3_1.GuiStat.Middle).get_transform());
      this.currentGo.get_transform().set_position(this.GetInstancePosition(MyGUI3_1.GuiStat.Middle));
    }
    else
      this.currentGo.get_transform().set_parent(((Component) this).get_transform());
    this.effectSettings.CollisionEnter += (EventHandler<CollisionInfo>) ((n, e) =>
    {
      if (!Object.op_Inequality((Object) ((RaycastHit) ref e.Hit).get_transform(), (Object) null))
        return;
      Debug.Log((object) ((Object) ((RaycastHit) ref e.Hit).get_transform()).get_name());
    });
  }

  private GameObject GetTargetObject(MyGUI3_1.GuiStat stat)
  {
    switch (stat)
    {
      case MyGUI3_1.GuiStat.Ball:
        return this.Target;
      case MyGUI3_1.GuiStat.BallRotate:
        return this.Target;
      case MyGUI3_1.GuiStat.Bottom:
        return this.BottomPosition;
      case MyGUI3_1.GuiStat.Middle:
        this.MiddlePosition.get_transform().set_localPosition(this.defaultRobotPos);
        this.MiddlePosition.get_transform().set_localRotation(Quaternion.Euler(0.0f, 180f, 0.0f));
        return this.MiddlePosition;
      case MyGUI3_1.GuiStat.MiddleWithoutRobot:
        return ((Component) this.MiddlePosition.get_transform().get_parent()).get_gameObject();
      case MyGUI3_1.GuiStat.Top:
        return this.TopPosition;
      case MyGUI3_1.GuiStat.TopTarget:
        return this.BottomPosition;
      default:
        return ((Component) this).get_gameObject();
    }
  }

  private void effectSettings_EffectDeactivated(object sender, EventArgs e)
  {
    if (this.GuiStats[this.current] != MyGUI3_1.GuiStat.Middle)
      this.currentGo.get_transform().set_position(this.GetInstancePosition(this.GuiStats[this.current]));
    this.isReadyEffect = true;
  }

  private void OnGUI()
  {
    if (GUI.Button(new Rect(10f * this.dpiScale, 15f * this.dpiScale, 105f * this.dpiScale, 30f * this.dpiScale), "Previous Effect"))
      this.ChangeCurrent(-1);
    if (GUI.Button(new Rect(130f * this.dpiScale, 15f * this.dpiScale, 105f * this.dpiScale, 30f * this.dpiScale), "Next Effect"))
      this.ChangeCurrent(1);
    if (Object.op_Inequality((Object) this.Prefabs[this.current], (Object) null))
      GUI.Label(new Rect(300f * this.dpiScale, 15f * this.dpiScale, 100f * this.dpiScale, 20f * this.dpiScale), "Prefab name is \"" + ((Object) this.Prefabs[this.current]).get_name() + "\"  \r\nHold any mouse button that would move the camera", this.guiStyleHeader);
    if (GUI.Button(new Rect(10f * this.dpiScale, 60f * this.dpiScale, 225f * this.dpiScale, 30f * this.dpiScale), "Day/Night"))
    {
      this.DirLight.set_intensity(this.isDay ? this.oldLightIntensity : 0.0f);
      ((Component) this.DirLight).get_transform().set_rotation(!this.isDay ? Quaternion.Euler(350f, 30f, 90f) : Quaternion.Euler(400f, 30f, 90f));
      RenderSettings.set_ambientLight(this.isDay ? this.oldAmbientColor : new Color(0.1f, 0.1f, 0.1f));
      RenderSettings.set_ambientIntensity(!this.isDay ? 0.1f : 0.5f);
      RenderSettings.set_reflectionIntensity(!this.isDay ? 0.1f : 1f);
      this.isDay = !this.isDay;
    }
    GUI.DrawTexture(new Rect(12f * this.dpiScale, 110f * this.dpiScale, 220f * this.dpiScale, 15f * this.dpiScale), this.HUETexture, (ScaleMode) 0, false, 0.0f);
    float colorHue = this.colorHUE;
    this.colorHUE = GUI.HorizontalSlider(new Rect(12f * this.dpiScale, 135f * this.dpiScale, 220f * this.dpiScale, 15f * this.dpiScale), this.colorHUE, 0.0f, 360f);
    if ((double) Mathf.Abs(colorHue - this.colorHUE) > 0.001)
      this.ChangeColor();
    GUI.Label(new Rect(240f * this.dpiScale, 105f * this.dpiScale, 30f * this.dpiScale, 30f * this.dpiScale), "Effect color", this.guiStyleHeader);
  }

  private void Update()
  {
    if (this.isReadyEffect)
    {
      this.isReadyEffect = false;
      this.currentGo.SetActive(true);
    }
    if (this.GuiStats[this.current] == MyGUI3_1.GuiStat.BallRotate)
      this.currentGo.get_transform().set_localRotation(Quaternion.Euler(0.0f, Mathf.PingPong(Time.get_time() * 5f, 60f) - 50f, 0.0f));
    if (this.GuiStats[this.current] != MyGUI3_1.GuiStat.BallRotatex4)
      return;
    this.currentGo.get_transform().set_localRotation(Quaternion.Euler(0.0f, Mathf.PingPong(Time.get_time() * 30f, 100f) - 70f, 0.0f));
  }

  private void InstanceCurrent(MyGUI3_1.GuiStat stat)
  {
    switch (stat)
    {
      case MyGUI3_1.GuiStat.Ball:
        this.MiddlePosition.SetActive(false);
        this.InstanceEffect(((Component) this).get_transform().get_position());
        break;
      case MyGUI3_1.GuiStat.BallRotate:
        this.MiddlePosition.SetActive(false);
        this.InstanceEffect(((Component) this).get_transform().get_position());
        break;
      case MyGUI3_1.GuiStat.BallRotatex4:
        this.MiddlePosition.SetActive(false);
        this.InstanceEffect(((Component) this).get_transform().get_position());
        break;
      case MyGUI3_1.GuiStat.Bottom:
        this.MiddlePosition.SetActive(false);
        this.InstanceEffect(this.BottomPosition.get_transform().get_position());
        break;
      case MyGUI3_1.GuiStat.Middle:
        this.MiddlePosition.SetActive(true);
        this.InstanceEffect(((Component) this.MiddlePosition.get_transform().get_parent()).get_transform().get_position());
        break;
      case MyGUI3_1.GuiStat.MiddleWithoutRobot:
        this.MiddlePosition.SetActive(false);
        this.InstanceEffect(this.MiddlePosition.get_transform().get_position());
        break;
      case MyGUI3_1.GuiStat.Top:
        this.MiddlePosition.SetActive(false);
        this.InstanceEffect(this.TopPosition.get_transform().get_position());
        break;
      case MyGUI3_1.GuiStat.TopTarget:
        this.MiddlePosition.SetActive(false);
        this.InstanceEffect(this.TopPosition.get_transform().get_position());
        break;
    }
  }

  private Vector3 GetInstancePosition(MyGUI3_1.GuiStat stat)
  {
    switch (stat)
    {
      case MyGUI3_1.GuiStat.Ball:
        return ((Component) this).get_transform().get_position();
      case MyGUI3_1.GuiStat.BallRotate:
        return ((Component) this).get_transform().get_position();
      case MyGUI3_1.GuiStat.BallRotatex4:
        return ((Component) this).get_transform().get_position();
      case MyGUI3_1.GuiStat.Bottom:
        return this.BottomPosition.get_transform().get_position();
      case MyGUI3_1.GuiStat.Middle:
        return ((Component) this.MiddlePosition.get_transform().get_parent()).get_transform().get_position();
      case MyGUI3_1.GuiStat.MiddleWithoutRobot:
        return ((Component) this.MiddlePosition.get_transform().get_parent()).get_transform().get_position();
      case MyGUI3_1.GuiStat.Top:
        return this.TopPosition.get_transform().get_position();
      case MyGUI3_1.GuiStat.TopTarget:
        return this.TopPosition.get_transform().get_position();
      default:
        return ((Component) this).get_transform().get_position();
    }
  }

  private void ChangeCurrent(int delta)
  {
    Object.Destroy((Object) this.currentGo);
    this.CancelInvoke("InstanceDefaulBall");
    this.current += delta;
    if (this.current > this.Prefabs.Length - 1)
      this.current = 0;
    else if (this.current < 0)
      this.current = this.Prefabs.Length - 1;
    if (Object.op_Inequality((Object) this.effectSettings, (Object) null))
      this.effectSettings.EffectDeactivated -= new EventHandler<EventArgs>(this.effectSettings_EffectDeactivated);
    this.MiddlePosition.SetActive(this.GuiStats[this.current] == MyGUI3_1.GuiStat.Middle);
    this.InstanceEffect(this.GetInstancePosition(this.GuiStats[this.current]));
    if (!Object.op_Inequality((Object) this.TargetForRay, (Object) null))
      return;
    if (this.current == 14 || this.current == 22)
      this.TargetForRay.SetActive(true);
    else
      this.TargetForRay.SetActive(false);
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

  public MyGUI3_1.HSBColor ColorToHSV(Color color)
  {
    MyGUI3_1.HSBColor hsbColor = new MyGUI3_1.HSBColor(0.0f, 0.0f, 0.0f, (float) color.a);
    float r = (float) color.r;
    float g = (float) color.g;
    float b = (float) color.b;
    float num1 = Mathf.Max(r, Mathf.Max(g, b));
    if ((double) num1 <= 0.0)
      return hsbColor;
    float num2 = Mathf.Min(r, Mathf.Min(g, b));
    float num3 = num1 - num2;
    if ((double) num1 > (double) num2)
    {
      hsbColor.h = (double) g != (double) num1 ? ((double) b != (double) num1 ? ((double) b <= (double) g ? (float) (((double) g - (double) b) / (double) num3 * 60.0) : (float) (((double) g - (double) b) / (double) num3 * 60.0 + 360.0)) : (float) (((double) r - (double) g) / (double) num3 * 60.0 + 240.0)) : (float) (((double) b - (double) r) / (double) num3 * 60.0 + 120.0);
      if ((double) hsbColor.h < 0.0)
        hsbColor.h += 360f;
    }
    else
      hsbColor.h = 0.0f;
    hsbColor.h *= 1f / 360f;
    hsbColor.s = (float) ((double) num3 / (double) num1 * 1.0);
    hsbColor.b = num1;
    return hsbColor;
  }

  public Color HSVToColor(MyGUI3_1.HSBColor hsbColor)
  {
    float num1 = hsbColor.b;
    float num2 = hsbColor.b;
    float num3 = hsbColor.b;
    if ((double) hsbColor.s != 0.0)
    {
      float b = hsbColor.b;
      float num4 = hsbColor.b * hsbColor.s;
      float num5 = hsbColor.b - num4;
      float num6 = hsbColor.h * 360f;
      if ((double) num6 < 60.0)
      {
        num1 = b;
        num2 = (float) ((double) num6 * (double) num4 / 60.0) + num5;
        num3 = num5;
      }
      else if ((double) num6 < 120.0)
      {
        num1 = (float) (-((double) num6 - 120.0) * (double) num4 / 60.0) + num5;
        num2 = b;
        num3 = num5;
      }
      else if ((double) num6 < 180.0)
      {
        num1 = num5;
        num2 = b;
        num3 = (float) (((double) num6 - 120.0) * (double) num4 / 60.0) + num5;
      }
      else if ((double) num6 < 240.0)
      {
        num1 = num5;
        num2 = (float) (-((double) num6 - 240.0) * (double) num4 / 60.0) + num5;
        num3 = b;
      }
      else if ((double) num6 < 300.0)
      {
        num1 = (float) (((double) num6 - 240.0) * (double) num4 / 60.0) + num5;
        num2 = num5;
        num3 = b;
      }
      else if ((double) num6 <= 360.0)
      {
        num1 = b;
        num2 = num5;
        num3 = (float) (-((double) num6 - 360.0) * (double) num4 / 60.0) + num5;
      }
      else
      {
        num1 = 0.0f;
        num2 = 0.0f;
        num3 = 0.0f;
      }
    }
    return new Color(Mathf.Clamp01(num1), Mathf.Clamp01(num2), Mathf.Clamp01(num3), hsbColor.a);
  }

  private void ChangeColor()
  {
    Color color1 = this.Hue(this.colorHUE / (float) byte.MaxValue);
    foreach (Renderer componentsInChild in (Renderer[]) this.currentGo.GetComponentsInChildren<Renderer>())
    {
      Material material = componentsInChild.get_material();
      if (!Object.op_Equality((Object) material, (Object) null))
      {
        if (material.HasProperty("_TintColor"))
        {
          Color color2 = material.GetColor("_TintColor");
          MyGUI3_1.HSBColor hsv = this.ColorToHSV(color2);
          hsv.h = this.colorHUE / 360f;
          color1 = this.HSVToColor(hsv);
          color1.a = color2.a;
          material.SetColor("_TintColor", color1);
        }
        if (material.HasProperty("_CoreColor"))
        {
          Color color2 = material.GetColor("_CoreColor");
          MyGUI3_1.HSBColor hsv = this.ColorToHSV(color2);
          hsv.h = this.colorHUE / 360f;
          color1 = this.HSVToColor(hsv);
          color1.a = color2.a;
          material.SetColor("_CoreColor", color1);
        }
      }
    }
    foreach (Projector componentsInChild in (Projector[]) this.currentGo.GetComponentsInChildren<Projector>())
    {
      Material material = componentsInChild.get_material();
      if (!Object.op_Equality((Object) material, (Object) null) && material.HasProperty("_TintColor"))
      {
        Color color2 = material.GetColor("_TintColor");
        MyGUI3_1.HSBColor hsv = this.ColorToHSV(color2);
        hsv.h = this.colorHUE / 360f;
        color1 = this.HSVToColor(hsv);
        color1.a = color2.a;
        componentsInChild.get_material().SetColor("_TintColor", color1);
      }
    }
    Light componentInChildren = (Light) this.currentGo.GetComponentInChildren<Light>();
    if (!Object.op_Inequality((Object) componentInChildren, (Object) null))
      return;
    componentInChildren.set_color(color1);
  }

  public enum GuiStat
  {
    Ball,
    BallRotate,
    BallRotatex4,
    Bottom,
    Middle,
    MiddleWithoutRobot,
    Top,
    TopTarget,
  }

  public struct HSBColor
  {
    public float h;
    public float s;
    public float b;
    public float a;

    public HSBColor(float h, float s, float b, float a)
    {
      this.h = h;
      this.s = s;
      this.b = b;
      this.a = a;
    }
  }
}
