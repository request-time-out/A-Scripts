// Decompiled with JetBrains decompiler
// Type: LandingSpotController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class LandingSpotController : MonoBehaviour
{
  public bool _randomRotate;
  public Vector2 _autoCatchDelay;
  public Vector2 _autoDismountDelay;
  public float _maxBirdDistance;
  public float _minBirdDistance;
  public bool _takeClosest;
  public FlockController _flock;
  public bool _landOnStart;
  public bool _soarLand;
  public bool _onlyBirdsAbove;
  public float _landingSpeedModifier;
  public float _landingTurnSpeedModifier;
  public Transform _featherPS;
  public Transform _thisT;
  public int _activeLandingSpots;
  public float _snapLandDistance;
  public float _landedRotateSpeed;
  public float _gizmoSize;

  public LandingSpotController()
  {
    base.\u002Ector();
  }

  public void Start()
  {
    if (Object.op_Equality((Object) this._thisT, (Object) null))
      this._thisT = ((Component) this).get_transform();
    if (Object.op_Equality((Object) this._flock, (Object) null))
    {
      this._flock = (FlockController) Object.FindObjectOfType(typeof (FlockController));
      Debug.Log((object) (this.ToString() + " has no assigned FlockController, a random FlockController has been assigned"));
    }
    if (!this._landOnStart)
      return;
    this.StartCoroutine(this.InstantLandOnStart(0.1f));
  }

  public void ScareAll()
  {
    this.ScareAll(0.0f, 1f);
  }

  public void ScareAll(float minDelay, float maxDelay)
  {
    for (int index = 0; index < this._thisT.get_childCount(); ++index)
    {
      if (Object.op_Inequality((Object) ((Component) this._thisT.GetChild(index)).GetComponent<LandingSpot>(), (Object) null))
        ((MonoBehaviour) ((Component) this._thisT.GetChild(index)).GetComponent<LandingSpot>()).Invoke("ReleaseFlockChild", Random.Range(minDelay, maxDelay));
    }
  }

  public void LandAll()
  {
    for (int index = 0; index < this._thisT.get_childCount(); ++index)
    {
      if (Object.op_Inequality((Object) ((Component) this._thisT.GetChild(index)).GetComponent<LandingSpot>(), (Object) null))
        this.StartCoroutine(((LandingSpot) ((Component) this._thisT.GetChild(index)).GetComponent<LandingSpot>()).GetFlockChild(0.0f, 2f));
    }
  }

  [DebuggerHidden]
  public IEnumerator InstantLandOnStart(float delay)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new LandingSpotController.\u003CInstantLandOnStart\u003Ec__Iterator0()
    {
      delay = delay,
      \u0024this = this
    };
  }

  [DebuggerHidden]
  public IEnumerator InstantLand(float delay)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new LandingSpotController.\u003CInstantLand\u003Ec__Iterator1()
    {
      delay = delay,
      \u0024this = this
    };
  }
}
