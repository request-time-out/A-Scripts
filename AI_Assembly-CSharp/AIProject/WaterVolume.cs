// Decompiled with JetBrains decompiler
// Type: AIProject.WaterVolume
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEx.Misc;

namespace AIProject
{
  [RequireComponent(typeof (Collider))]
  public class WaterVolume : MonoBehaviour
  {
    [SerializeField]
    private LayerMask _waterLayer;
    private List<Rigidbody> _rigidbodies;

    public WaterVolume()
    {
      base.\u002Ector();
    }

    private void Update()
    {
      using (List<Rigidbody>.Enumerator enumerator = this._rigidbodies.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          Rigidbody current = enumerator.Current;
          if (!Object.op_Equality((Object) current, (Object) null) && !Object.op_Equality((Object) ((Component) current).get_gameObject(), (Object) null))
          {
            Vector3 velocity = current.get_velocity();
            velocity.y = (__Null) 0.0;
            current.set_velocity(velocity);
          }
        }
      }
    }

    private void OnTriggerEnter(Collider other)
    {
      if (!UnityExtensions.Contains(this._waterLayer, ((Component) other).get_gameObject().get_layer()))
        return;
      Rigidbody component = (Rigidbody) ((Component) other).GetComponent<Rigidbody>();
      if (Object.op_Equality((Object) component, (Object) null) || this._rigidbodies.Contains(component))
        return;
      this._rigidbodies.Add(component);
    }

    private void OnTriggerExit(Collider other)
    {
      if (!UnityExtensions.Contains(this._waterLayer, ((Component) other).get_gameObject().get_layer()))
        return;
      Rigidbody component = (Rigidbody) ((Component) other).GetComponent<Rigidbody>();
      if (Object.op_Equality((Object) component, (Object) null) || !this._rigidbodies.Contains(component))
        return;
      this._rigidbodies.Remove(component);
    }
  }
}
