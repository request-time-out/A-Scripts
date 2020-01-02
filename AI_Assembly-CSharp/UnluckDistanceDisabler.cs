// Decompiled with JetBrains decompiler
// Type: UnluckDistanceDisabler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class UnluckDistanceDisabler : MonoBehaviour
{
  public int _distanceDisable;
  public Transform _distanceFrom;
  public bool _distanceFromMainCam;
  public float _disableCheckInterval;
  public float _enableCheckInterval;
  public bool _disableOnStart;

  public UnluckDistanceDisabler()
  {
    base.\u002Ector();
  }

  public void Start()
  {
    if (this._distanceFromMainCam)
      this._distanceFrom = ((Component) Camera.get_main()).get_transform();
    this.InvokeRepeating("CheckDisable", this._disableCheckInterval + Random.get_value() * this._disableCheckInterval, this._disableCheckInterval);
    this.InvokeRepeating("CheckEnable", this._enableCheckInterval + Random.get_value() * this._enableCheckInterval, this._enableCheckInterval);
    this.Invoke("DisableOnStart", 0.01f);
  }

  public void DisableOnStart()
  {
    if (!this._disableOnStart)
      return;
    ((Component) this).get_gameObject().SetActive(false);
  }

  public void CheckDisable()
  {
    if (!((Component) this).get_gameObject().get_activeInHierarchy())
      return;
    Vector3 vector3 = Vector3.op_Subtraction(((Component) this).get_transform().get_position(), this._distanceFrom.get_position());
    if ((double) ((Vector3) ref vector3).get_sqrMagnitude() <= (double) (this._distanceDisable * this._distanceDisable))
      return;
    ((Component) this).get_gameObject().SetActive(false);
  }

  public void CheckEnable()
  {
    if (((Component) this).get_gameObject().get_activeInHierarchy())
      return;
    Vector3 vector3 = Vector3.op_Subtraction(((Component) this).get_transform().get_position(), this._distanceFrom.get_position());
    if ((double) ((Vector3) ref vector3).get_sqrMagnitude() >= (double) (this._distanceDisable * this._distanceDisable))
      return;
    ((Component) this).get_gameObject().SetActive(true);
  }
}
