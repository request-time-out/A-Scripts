// Decompiled with JetBrains decompiler
// Type: AIProject.PlantItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace AIProject
{
  public class PlantItem : MonoBehaviour
  {
    [SerializeField]
    private GameObject[] _states;

    public PlantItem()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      foreach (GameObject state in this._states)
        state.SetActive(false);
    }

    public void ChangeState(int state)
    {
      for (int index = 0; index < this._states.Length; ++index)
      {
        GameObject state1 = this._states[index];
        bool flag = index == state;
        if (state1.get_activeSelf() != flag)
          this._states[index].SetActive(flag);
      }
    }
  }
}
