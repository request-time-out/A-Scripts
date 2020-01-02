// Decompiled with JetBrains decompiler
// Type: LandingButtons
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class LandingButtons : MonoBehaviour
{
  public LandingSpotController _landingSpotController;
  public FlockController _flockController;
  public float hSliderValue;

  public LandingButtons()
  {
    base.\u002Ector();
  }

  public void OnGUI()
  {
    GUI.Label(new Rect(20f, 20f, 125f, 18f), "Landing Spots: " + (object) ((Component) this._landingSpotController).get_transform().get_childCount());
    if (GUI.Button(new Rect(20f, 40f, 125f, 18f), "Scare All"))
      this._landingSpotController.ScareAll();
    if (GUI.Button(new Rect(20f, 60f, 125f, 18f), "Land In Reach"))
      this._landingSpotController.LandAll();
    if (GUI.Button(new Rect(20f, 80f, 125f, 18f), "Land Instant"))
      this.StartCoroutine(this._landingSpotController.InstantLand(0.01f));
    if (GUI.Button(new Rect(20f, 100f, 125f, 18f), "Destroy"))
      this._flockController.destroyBirds();
    GUI.Label(new Rect(20f, 120f, 125f, 18f), "Bird Amount: " + (object) this._flockController._childAmount);
    this._flockController._childAmount = (int) GUI.HorizontalSlider(new Rect(20f, 140f, 125f, 18f), (float) this._flockController._childAmount, 0.0f, 250f);
  }
}
