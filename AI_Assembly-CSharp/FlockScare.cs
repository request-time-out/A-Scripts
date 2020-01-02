// Decompiled with JetBrains decompiler
// Type: FlockScare
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class FlockScare : MonoBehaviour
{
  public LandingSpotController[] landingSpotControllers;
  public float scareInterval;
  public float distanceToScare;
  public int checkEveryNthLandingSpot;
  public int InvokeAmounts;
  private int lsc;
  private int ls;
  private LandingSpotController currentController;

  public FlockScare()
  {
    base.\u002Ector();
  }

  private void CheckProximityToLandingSpots()
  {
    this.IterateLandingSpots();
    if (this.currentController._activeLandingSpots > 0 && this.CheckDistanceToLandingSpot(this.landingSpotControllers[this.lsc]))
      this.landingSpotControllers[this.lsc].ScareAll();
    this.Invoke(nameof (CheckProximityToLandingSpots), this.scareInterval);
  }

  private void IterateLandingSpots()
  {
    this.ls += this.checkEveryNthLandingSpot;
    this.currentController = this.landingSpotControllers[this.lsc];
    int childCount = ((Component) this.currentController).get_transform().get_childCount();
    if (this.ls <= childCount - 1)
      return;
    this.ls -= childCount;
    if (this.lsc < this.landingSpotControllers.Length - 1)
      ++this.lsc;
    else
      this.lsc = 0;
  }

  private bool CheckDistanceToLandingSpot(LandingSpotController lc)
  {
    Transform child = ((Component) lc).get_transform().GetChild(this.ls);
    if (Object.op_Inequality((Object) ((LandingSpot) ((Component) child).GetComponent<LandingSpot>()).landingChild, (Object) null))
    {
      Vector3 vector3 = Vector3.op_Subtraction(child.get_position(), ((Component) this).get_transform().get_position());
      if ((double) ((Vector3) ref vector3).get_sqrMagnitude() < (double) this.distanceToScare * (double) this.distanceToScare)
        return true;
    }
    return false;
  }

  private void Invoker()
  {
    for (int index = 0; index < this.InvokeAmounts; ++index)
      this.Invoke("CheckProximityToLandingSpots", this.scareInterval + this.scareInterval / (float) this.InvokeAmounts * (float) index);
  }

  private void OnEnable()
  {
    this.CancelInvoke("CheckProximityToLandingSpots");
    if (this.landingSpotControllers.Length <= 0)
      return;
    this.Invoker();
  }

  private void OnDisable()
  {
    this.CancelInvoke("CheckProximityToLandingSpots");
  }
}
