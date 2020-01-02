// Decompiled with JetBrains decompiler
// Type: EnviroTrigger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class EnviroTrigger : MonoBehaviour
{
  public EnviroInterior myZone;
  public string Name;

  public EnviroTrigger()
  {
    base.\u002Ector();
  }

  private void Start()
  {
  }

  private void Update()
  {
  }

  private void OnTriggerEnter(Collider col)
  {
    if (EnviroSky.instance.weatherSettings.useTag)
    {
      if (!(((Component) col).get_gameObject().get_tag() == ((Component) EnviroSky.instance).get_gameObject().get_tag()))
        return;
      this.EnterExit();
    }
    else
    {
      if (!Object.op_Implicit((Object) ((Component) col).get_gameObject().GetComponent<EnviroSky>()))
        return;
      this.EnterExit();
    }
  }

  private void OnTriggerExit(Collider col)
  {
    if (this.myZone.zoneTriggerType != EnviroInterior.ZoneTriggerType.Zone)
      return;
    if (EnviroSky.instance.weatherSettings.useTag)
    {
      if (!(((Component) col).get_gameObject().get_tag() == ((Component) EnviroSky.instance).get_gameObject().get_tag()))
        return;
      this.EnterExit();
    }
    else
    {
      if (!Object.op_Implicit((Object) ((Component) col).get_gameObject().GetComponent<EnviroSky>()))
        return;
      this.EnterExit();
    }
  }

  private void EnterExit()
  {
    if (Object.op_Inequality((Object) EnviroSky.instance.lastInteriorZone, (Object) this.myZone))
    {
      if (Object.op_Inequality((Object) EnviroSky.instance.lastInteriorZone, (Object) null))
        EnviroSky.instance.lastInteriorZone.StopAllFading();
      this.myZone.Enter();
    }
    else if (!EnviroSky.instance.interiorMode)
      this.myZone.Enter();
    else
      this.myZone.Exit();
  }

  private void OnDrawGizmos()
  {
    Gizmos.set_matrix(((Component) this).get_transform().get_worldToLocalMatrix());
    Gizmos.set_color(Color.get_blue());
    Gizmos.DrawCube(Vector3.get_zero(), Vector3.get_one());
  }
}
