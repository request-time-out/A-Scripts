// Decompiled with JetBrains decompiler
// Type: ME_DemoGUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class ME_DemoGUI : MonoBehaviour
{
  public GameObject Character;
  public GameObject Model;
  public int Current;
  public GameObject[] Prefabs;
  public Light Sun;
  public ReflectionProbe ReflectionProbe;
  public Light[] NightLights;
  public Texture HUETexture;
  public bool UseMobileVersion;
  public GameObject MobileCharacter;
  public GameObject Target;
  public Color guiColor;
  private int currentNomber;
  private GameObject characterInstance;
  private GameObject modelInstance;
  private GUIStyle guiStyleHeader;
  private GUIStyle guiStyleHeaderMobile;
  private float dpiScale;
  private bool isDay;
  private float colorHUE;
  private float startSunIntensity;
  private Quaternion startSunRotation;
  private Color startAmbientLight;
  private float startAmbientIntencity;
  private float startReflectionIntencity;
  private LightShadows startLightShadows;
  private bool isButtonPressed;
  private GameObject instanceShieldProjectile;

  public ME_DemoGUI()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    if ((double) Screen.get_dpi() < 1.0)
      this.dpiScale = 1f;
    this.dpiScale = (double) Screen.get_dpi() >= 200.0 ? Screen.get_dpi() / 200f : 1f;
    this.guiStyleHeader.set_fontSize((int) (15.0 * (double) this.dpiScale));
    this.guiStyleHeader.get_normal().set_textColor(this.guiColor);
    this.guiStyleHeaderMobile.set_fontSize((int) (17.0 * (double) this.dpiScale));
    this.ChangeCurrent(this.Current);
    this.startSunIntensity = this.Sun.get_intensity();
    this.startSunRotation = ((Component) this.Sun).get_transform().get_rotation();
    this.startAmbientLight = RenderSettings.get_ambientLight();
    this.startAmbientIntencity = RenderSettings.get_ambientIntensity();
    this.startReflectionIntencity = RenderSettings.get_reflectionIntensity();
    this.startLightShadows = this.Sun.get_shadows();
  }

  private void OnGUI()
  {
    if (Input.GetKeyUp((KeyCode) 276) || Input.GetKeyUp((KeyCode) 275) || Input.GetKeyUp((KeyCode) 274))
      this.isButtonPressed = false;
    if (GUI.Button(new Rect(10f * this.dpiScale, 15f * this.dpiScale, 135f * this.dpiScale, 37f * this.dpiScale), "PREVIOUS EFFECT") || !this.isButtonPressed && Input.GetKeyDown((KeyCode) 276))
    {
      this.isButtonPressed = true;
      this.ChangeCurrent(-1);
    }
    if (GUI.Button(new Rect(160f * this.dpiScale, 15f * this.dpiScale, 135f * this.dpiScale, 37f * this.dpiScale), "NEXT EFFECT") || !this.isButtonPressed && Input.GetKeyDown((KeyCode) 275))
    {
      this.isButtonPressed = true;
      this.ChangeCurrent(1);
    }
    float num1 = 0.0f;
    if (GUI.Button(new Rect(10f * this.dpiScale, 63f * this.dpiScale + num1, 285f * this.dpiScale, 37f * this.dpiScale), "Day / Night") || !this.isButtonPressed && Input.GetKeyDown((KeyCode) 274))
    {
      this.isButtonPressed = true;
      if (Object.op_Inequality((Object) this.ReflectionProbe, (Object) null))
        this.ReflectionProbe.RenderProbe();
      this.Sun.set_intensity(this.isDay ? this.startSunIntensity : 0.05f);
      this.Sun.set_shadows(!this.isDay ? (LightShadows) 0 : (LightShadows) (int) this.startLightShadows);
      foreach (Light nightLight in this.NightLights)
        nightLight.set_shadows(this.isDay ? (LightShadows) 0 : (LightShadows) (int) this.startLightShadows);
      ((Component) this.Sun).get_transform().set_rotation(!this.isDay ? Quaternion.Euler(350f, 30f, 90f) : this.startSunRotation);
      RenderSettings.set_ambientLight(this.isDay ? this.startAmbientLight : new Color(0.2f, 0.2f, 0.2f));
      float num2 = this.UseMobileVersion ? 0.3f : 1f;
      RenderSettings.set_ambientIntensity(!this.isDay ? num2 : this.startAmbientIntencity);
      RenderSettings.set_reflectionIntensity(!this.isDay ? 0.2f : this.startReflectionIntencity);
      this.isDay = !this.isDay;
    }
    GUI.Label(new Rect(400f * this.dpiScale, (float) (15.0 * (double) this.dpiScale + (double) num1 / 2.0), 100f * this.dpiScale, 20f * this.dpiScale), "Prefab name is \"" + ((Object) this.Prefabs[this.currentNomber]).get_name() + "\"  \r\nHold any mouse button that would move the camera", this.guiStyleHeader);
    GUI.DrawTexture(new Rect(12f * this.dpiScale, 140f * this.dpiScale + num1, 285f * this.dpiScale, 15f * this.dpiScale), this.HUETexture, (ScaleMode) 0, false, 0.0f);
    float colorHue = this.colorHUE;
    this.colorHUE = GUI.HorizontalSlider(new Rect(12f * this.dpiScale, 147f * this.dpiScale + num1, 285f * this.dpiScale, 15f * this.dpiScale), this.colorHUE, 0.0f, 360f);
    if ((double) Mathf.Abs(colorHue - this.colorHUE) <= 0.001)
      return;
    PSMeshRendererUpdater componentInChildren1 = (PSMeshRendererUpdater) this.characterInstance.GetComponentInChildren<PSMeshRendererUpdater>();
    if (Object.op_Inequality((Object) componentInChildren1, (Object) null))
      componentInChildren1.UpdateColor(this.colorHUE / 360f);
    PSMeshRendererUpdater componentInChildren2 = (PSMeshRendererUpdater) this.modelInstance.GetComponentInChildren<PSMeshRendererUpdater>();
    if (!Object.op_Inequality((Object) componentInChildren2, (Object) null))
      return;
    componentInChildren2.UpdateColor(this.colorHUE / 360f);
  }

  private void ChangeCurrent(int delta)
  {
    this.currentNomber += delta;
    if (this.currentNomber > this.Prefabs.Length - 1)
      this.currentNomber = 0;
    else if (this.currentNomber < 0)
      this.currentNomber = this.Prefabs.Length - 1;
    if (Object.op_Inequality((Object) this.characterInstance, (Object) null))
    {
      Object.Destroy((Object) this.characterInstance);
      this.RemoveClones();
    }
    if (Object.op_Inequality((Object) this.modelInstance, (Object) null))
    {
      Object.Destroy((Object) this.modelInstance);
      this.RemoveClones();
    }
    this.characterInstance = (GameObject) Object.Instantiate<GameObject>((M0) this.Character);
    ((ME_AnimatorEvents) this.characterInstance.GetComponent<ME_AnimatorEvents>()).EffectPrefab = this.Prefabs[this.currentNomber];
    this.modelInstance = (GameObject) Object.Instantiate<GameObject>((M0) this.Model);
    GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.Prefabs[this.currentNomber]);
    gameObject.get_transform().set_parent(this.modelInstance.get_transform());
    gameObject.get_transform().set_localPosition(Vector3.get_zero());
    gameObject.get_transform().set_localRotation((Quaternion) null);
    ((PSMeshRendererUpdater) gameObject.GetComponent<PSMeshRendererUpdater>()).UpdateMeshEffect(this.modelInstance);
    if (!this.UseMobileVersion)
      return;
    this.CancelInvoke("ReactivateEffect");
  }

  private void RemoveClones()
  {
    foreach (GameObject gameObject in (GameObject[]) Object.FindObjectsOfType<GameObject>())
    {
      if (((Object) gameObject).get_name().Contains("(Clone)"))
        Object.Destroy((Object) gameObject);
    }
  }

  private void ReactivateEffect()
  {
    this.characterInstance.SetActive(false);
    this.characterInstance.SetActive(true);
    this.modelInstance.SetActive(false);
    this.modelInstance.SetActive(true);
  }
}
