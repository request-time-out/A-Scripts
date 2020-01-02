// Decompiled with JetBrains decompiler
// Type: Studio.LightList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Studio
{
  public class LightList : MonoBehaviour
  {
    [SerializeField]
    private GameObject objectNode;
    [SerializeField]
    private Transform transformRoot;

    public LightList()
    {
      base.\u002Ector();
    }

    public void OnClick(int _no)
    {
      Singleton<Studio.Studio>.Instance.AddLight(_no);
    }

    private void AddNode(int _key, string _name)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      LightList.\u003CAddNode\u003Ec__AnonStorey0 nodeCAnonStorey0 = new LightList.\u003CAddNode\u003Ec__AnonStorey0();
      // ISSUE: reference to a compiler-generated field
      nodeCAnonStorey0.\u0024this = this;
      GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.objectNode);
      gameObject.get_transform().SetParent(this.transformRoot, false);
      if (!gameObject.get_activeSelf())
        gameObject.SetActive(true);
      ListNode component = (ListNode) gameObject.GetComponent<ListNode>();
      // ISSUE: reference to a compiler-generated field
      nodeCAnonStorey0.key = _key;
      // ISSUE: method pointer
      component.AddActionToButton(new UnityAction((object) nodeCAnonStorey0, __methodptr(\u003C\u003Em__0)));
      component.text = _name;
    }

    private void Awake()
    {
      foreach (KeyValuePair<int, Info.LightLoadInfo> keyValuePair in Singleton<Info>.Instance.dicLightLoadInfo)
        this.AddNode(keyValuePair.Key, keyValuePair.Value.name);
    }
  }
}
