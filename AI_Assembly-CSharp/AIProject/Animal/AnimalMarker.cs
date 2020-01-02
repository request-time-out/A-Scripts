// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.AnimalMarker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace AIProject.Animal
{
  public class AnimalMarker : MonoBehaviour
  {
    private int InstanceID;
    private bool IsAnimalActive;
    private AnimalBase _animal;

    public AnimalMarker()
    {
      base.\u002Ector();
    }

    public static Dictionary<int, AnimalBase> AnimalMarkerTable { get; } = new Dictionary<int, AnimalBase>();

    public static List<int> Keys { get; } = new List<int>();

    private void Awake()
    {
      if (!Object.op_Implicit((Object) this._animal))
        this._animal = (AnimalBase) ((Component) this).GetComponent<AnimalBase>();
      if (!Object.op_Implicit((Object) this._animal))
      {
        Object.Destroy((Object) this);
      }
      else
      {
        this.InstanceID = this._animal.InstanceID;
        this.IsAnimalActive = true;
      }
    }

    private void OnEnable()
    {
      if (!Object.op_Implicit((Object) this._animal))
        return;
      AnimalMarker.AnimalMarkerTable[this.InstanceID] = this._animal;
      if (AnimalMarker.Keys.Contains(this.InstanceID))
        return;
      AnimalMarker.Keys.Add(this.InstanceID);
    }

    private void OnDisable()
    {
      if (!this.IsAnimalActive)
        return;
      AnimalMarker.AnimalMarkerTable.Remove(this.InstanceID);
      AnimalMarker.Keys.RemoveAll((Predicate<int>) (x => x == this.InstanceID));
    }
  }
}
