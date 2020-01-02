// Decompiled with JetBrains decompiler
// Type: Studio.VoiceCategoryList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Studio
{
  public class VoiceCategoryList : MonoBehaviour
  {
    [SerializeField]
    private Transform transformRoot;
    [SerializeField]
    private GameObject objectPrefab;
    [SerializeField]
    private ScrollRect scrollRect;
    [SerializeField]
    private VoiceList voiceList;
    private int group;
    private int select;
    private Dictionary<int, StudioNode> dicNode;

    public VoiceCategoryList()
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

    public void InitList(int _group)
    {
      if (!Utility.SetStruct<int>(ref this.group, _group))
        return;
      int childCount = this.transformRoot.get_childCount();
      for (int index = 0; index < childCount; ++index)
        Object.Destroy((Object) ((Component) this.transformRoot.GetChild(index)).get_gameObject());
      this.transformRoot.DetachChildren();
      this.scrollRect.set_verticalNormalizedPosition(1f);
      this.dicNode = new Dictionary<int, StudioNode>();
      foreach (KeyValuePair<int, Info.CategoryInfo> keyValuePair in (IEnumerable<KeyValuePair<int, Info.CategoryInfo>>) Singleton<Info>.Instance.dicVoiceGroupCategory[_group].dicCategory.OrderBy<KeyValuePair<int, Info.CategoryInfo>, int>((Func<KeyValuePair<int, Info.CategoryInfo>, int>) (v => v.Value.sort)))
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        VoiceCategoryList.\u003CInitList\u003Ec__AnonStorey0 listCAnonStorey0 = new VoiceCategoryList.\u003CInitList\u003Ec__AnonStorey0();
        // ISSUE: reference to a compiler-generated field
        listCAnonStorey0.\u0024this = this;
        if (this.CheckCategory(_group, keyValuePair.Key))
        {
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
      }
      this.select = -1;
      this.active = true;
      this.voiceList.active = false;
    }

    private bool CheckCategory(int _group, int _category)
    {
      Dictionary<int, Dictionary<int, Info.LoadCommonInfo>> dictionary = (Dictionary<int, Dictionary<int, Info.LoadCommonInfo>>) null;
      return Singleton<Info>.Instance.dicVoiceLoadInfo.TryGetValue(_group, out dictionary) && dictionary.ContainsKey(_category);
    }

    private void OnSelect(int _no)
    {
      int select = this.select;
      if (!Utility.SetStruct<int>(ref this.select, _no))
        return;
      this.voiceList.InitList(this.group, _no);
      StudioNode studioNode = (StudioNode) null;
      if (this.dicNode.TryGetValue(select, out studioNode))
        studioNode.select = false;
      if (!this.dicNode.TryGetValue(this.select, out studioNode))
        return;
      studioNode.select = true;
    }
  }
}
