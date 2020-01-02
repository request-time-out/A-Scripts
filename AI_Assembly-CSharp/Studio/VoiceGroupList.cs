// Decompiled with JetBrains decompiler
// Type: Studio.VoiceGroupList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Studio
{
  public class VoiceGroupList : MonoBehaviour
  {
    [SerializeField]
    private Transform transformRoot;
    [SerializeField]
    private GameObject objectPrefab;
    [SerializeField]
    private VoiceCategoryList voiceCategoryList;
    [SerializeField]
    private VoiceList voiceList;
    private int select;
    private Dictionary<int, StudioNode> dicNode;

    public VoiceGroupList()
    {
      base.\u002Ector();
    }

    public bool active
    {
      get
      {
        return ((Component) this).get_gameObject().get_activeSelf();
      }
      set
      {
        if (((Component) this).get_gameObject().get_activeSelf() == value)
          return;
        ((Component) this).get_gameObject().SetActive(value);
      }
    }

    private void InitList()
    {
      int childCount = this.transformRoot.get_childCount();
      for (int index = 0; index < childCount; ++index)
        Object.Destroy((Object) ((Component) this.transformRoot.GetChild(index)).get_gameObject());
      this.transformRoot.DetachChildren();
      foreach (KeyValuePair<int, Info.GroupInfo> keyValuePair in Singleton<Info>.Instance.dicVoiceGroupCategory)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        VoiceGroupList.\u003CInitList\u003Ec__AnonStorey0 listCAnonStorey0 = new VoiceGroupList.\u003CInitList\u003Ec__AnonStorey0();
        // ISSUE: reference to a compiler-generated field
        listCAnonStorey0.\u0024this = this;
        GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.objectPrefab);
        if (!gameObject.get_activeSelf())
          gameObject.SetActive(true);
        gameObject.get_transform().SetParent(this.transformRoot, false);
        StudioNode component = (StudioNode) gameObject.GetComponent<StudioNode>();
        // ISSUE: reference to a compiler-generated field
        listCAnonStorey0.no = keyValuePair.Key;
        // ISSUE: method pointer
        component.addOnClick = new UnityAction((object) listCAnonStorey0, __methodptr(\u003C\u003Em__0));
        component.text = keyValuePair.Value.name;
        this.dicNode.Add(keyValuePair.Key, component);
      }
      this.select = -1;
      this.active = true;
    }

    private void OnSelect(int _no)
    {
      int select = this.select;
      if (!Utility.SetStruct<int>(ref this.select, _no))
        return;
      this.voiceCategoryList.InitList(_no);
      this.voiceList.active = false;
      StudioNode studioNode = (StudioNode) null;
      if (this.dicNode.TryGetValue(select, out studioNode))
        studioNode.select = false;
      if (!this.dicNode.TryGetValue(this.select, out studioNode))
        return;
      studioNode.select = true;
    }

    private void Start()
    {
      this.InitList();
    }
  }
}
