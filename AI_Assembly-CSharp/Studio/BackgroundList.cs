// Decompiled with JetBrains decompiler
// Type: Studio.BackgroundList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Studio
{
  public class BackgroundList : MonoBehaviour
  {
    public static string dirName = "bg";
    [SerializeField]
    private GameObject objectNode;
    [SerializeField]
    private Transform transformRoot;
    [SerializeField]
    private BackgroundCtrl backgroundCtrl;
    private List<string> listPath;
    private Dictionary<int, StudioNode> dicNode;
    private int select;

    public BackgroundList()
    {
      base.\u002Ector();
    }

    public void UpdateUI()
    {
      this.SetSelect(this.select, false);
      this.select = this.listPath.FindIndex((Predicate<string>) (v => v == Singleton<Studio.Studio>.Instance.sceneInfo.background));
      this.SetSelect(this.select, true);
    }

    private void OnClickSelect(int _idx)
    {
      this.SetSelect(this.select, false);
      this.select = _idx;
      this.SetSelect(this.select, true);
      this.backgroundCtrl.Load(this.select == -1 ? string.Empty : this.listPath[_idx]);
    }

    private void SetSelect(int _idx, bool _flag)
    {
      StudioNode studioNode = (StudioNode) null;
      if (!this.dicNode.TryGetValue(_idx, out studioNode))
        return;
      studioNode.select = _flag;
    }

    private void InitList()
    {
      for (int index = 0; index < this.transformRoot.get_childCount(); ++index)
        Object.Destroy((Object) ((Component) this.transformRoot.GetChild(index)).get_gameObject());
      this.transformRoot.DetachChildren();
      string[] files = Directory.GetFiles(UserData.Create(BackgroundList.dirName), "*.png");
      // ISSUE: reference to a compiler-generated field
      if (BackgroundList.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        BackgroundList.\u003C\u003Ef__mg\u0024cache0 = new Func<string, string>(Path.GetFileName);
      }
      // ISSUE: reference to a compiler-generated field
      Func<string, string> fMgCache0 = BackgroundList.\u003C\u003Ef__mg\u0024cache0;
      this.listPath = ((IEnumerable<string>) files).Select<string, string>(fMgCache0).ToList<string>();
      this.CreateNode(-1, "なし");
      int count = this.listPath.Count;
      for (int _idx = 0; _idx < count; ++_idx)
        this.CreateNode(_idx, Path.GetFileNameWithoutExtension(this.listPath[_idx]));
      this.select = this.listPath.FindIndex((Predicate<string>) (v => v == Singleton<Studio.Studio>.Instance.sceneInfo.background));
      this.SetSelect(this.select, true);
    }

    private void CreateNode(int _idx, string _text)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      BackgroundList.\u003CCreateNode\u003Ec__AnonStorey0 nodeCAnonStorey0 = new BackgroundList.\u003CCreateNode\u003Ec__AnonStorey0();
      // ISSUE: reference to a compiler-generated field
      nodeCAnonStorey0._idx = _idx;
      // ISSUE: reference to a compiler-generated field
      nodeCAnonStorey0.\u0024this = this;
      GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.objectNode);
      gameObject.get_transform().SetParent(this.transformRoot, false);
      StudioNode component = (StudioNode) gameObject.GetComponent<StudioNode>();
      component.active = true;
      // ISSUE: method pointer
      component.addOnClick = new UnityAction((object) nodeCAnonStorey0, __methodptr(\u003C\u003Em__0));
      component.text = _text;
      // ISSUE: reference to a compiler-generated field
      this.dicNode.Add(nodeCAnonStorey0._idx, component);
    }

    private void Start()
    {
      this.InitList();
    }
  }
}
