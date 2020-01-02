// Decompiled with JetBrains decompiler
// Type: Studio.OutsideSoundControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Studio
{
  public class OutsideSoundControl : MonoBehaviour
  {
    [SerializeField]
    private GameObject objectNode;
    [SerializeField]
    private Transform transformRoot;
    [SerializeField]
    private Button buttonRepeat;
    [SerializeField]
    private Sprite[] spriteRepeat;
    [SerializeField]
    private Button buttonStop;
    [SerializeField]
    private Button buttonPlay;
    [SerializeField]
    private Image imagePlayNow;
    [SerializeField]
    private Button buttonPause;
    [SerializeField]
    private Button buttonExpansion;
    [SerializeField]
    private Sprite[] spriteExpansion;
    [SerializeField]
    private GameObject objBottom;
    private List<string> listPath;
    private Dictionary<int, StudioNode> dicNode;
    private int select;

    public OutsideSoundControl()
    {
      base.\u002Ector();
    }

    private void OnClickRepeat()
    {
      Singleton<Studio.Studio>.Instance.outsideSoundCtrl.repeat = Singleton<Studio.Studio>.Instance.outsideSoundCtrl.repeat != BGMCtrl.Repeat.None ? BGMCtrl.Repeat.None : BGMCtrl.Repeat.All;
      ((Selectable) this.buttonRepeat).get_image().set_sprite(this.spriteRepeat[Singleton<Studio.Studio>.Instance.outsideSoundCtrl.repeat != BGMCtrl.Repeat.None ? 1 : 0]);
    }

    private void OnClickStop()
    {
      Singleton<Studio.Studio>.Instance.outsideSoundCtrl.Stop();
    }

    private void OnClickPlay()
    {
      Singleton<Studio.Studio>.Instance.outsideSoundCtrl.Play();
    }

    private void OnClickPause()
    {
    }

    private void OnClickExpansion()
    {
      this.objBottom.SetActive(!this.objBottom.get_activeSelf());
      ((Selectable) this.buttonExpansion).get_image().set_sprite(this.spriteExpansion[!this.objBottom.get_activeSelf() ? 0 : 1]);
    }

    private void OnClickSelect(int _idx)
    {
      StudioNode studioNode = (StudioNode) null;
      if (this.dicNode.TryGetValue(this.select, out studioNode))
        studioNode.select = false;
      this.select = _idx;
      if (this.select != -1)
        Singleton<Studio.Studio>.Instance.outsideSoundCtrl.fileName = this.listPath[_idx];
      if (!this.dicNode.TryGetValue(this.select, out studioNode))
        return;
      studioNode.select = true;
    }

    private void InitList()
    {
      for (int index = 0; index < this.transformRoot.get_childCount(); ++index)
        Object.Destroy((Object) ((Component) this.transformRoot.GetChild(index)).get_gameObject());
      this.transformRoot.DetachChildren();
      string[] files = Directory.GetFiles(UserData.Create("audio"), "*.wav");
      // ISSUE: reference to a compiler-generated field
      if (OutsideSoundControl.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        OutsideSoundControl.\u003C\u003Ef__mg\u0024cache0 = new Func<string, string>(Path.GetFileName);
      }
      // ISSUE: reference to a compiler-generated field
      Func<string, string> fMgCache0 = OutsideSoundControl.\u003C\u003Ef__mg\u0024cache0;
      this.listPath = ((IEnumerable<string>) files).Select<string, string>(fMgCache0).ToList<string>();
      int count = this.listPath.Count;
      for (int index = 0; index < count; ++index)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        OutsideSoundControl.\u003CInitList\u003Ec__AnonStorey0 listCAnonStorey0 = new OutsideSoundControl.\u003CInitList\u003Ec__AnonStorey0();
        // ISSUE: reference to a compiler-generated field
        listCAnonStorey0.\u0024this = this;
        GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.objectNode);
        gameObject.get_transform().SetParent(this.transformRoot, false);
        StudioNode component = (StudioNode) gameObject.GetComponent<StudioNode>();
        component.active = true;
        // ISSUE: reference to a compiler-generated field
        listCAnonStorey0.idx = index;
        // ISSUE: method pointer
        component.addOnClick = new UnityAction((object) listCAnonStorey0, __methodptr(\u003C\u003Em__0));
        component.text = Path.GetFileNameWithoutExtension(this.listPath[index]);
        // ISSUE: reference to a compiler-generated field
        this.dicNode.Add(listCAnonStorey0.idx, component);
      }
      this.select = this.listPath.FindIndex((Predicate<string>) (v => v == Singleton<Studio.Studio>.Instance.outsideSoundCtrl.fileName));
      StudioNode studioNode = (StudioNode) null;
      if (!this.dicNode.TryGetValue(this.select, out studioNode))
        return;
      studioNode.select = true;
    }

    private void Awake()
    {
      // ISSUE: method pointer
      ((UnityEvent) this.buttonRepeat.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickRepeat)));
      // ISSUE: method pointer
      ((UnityEvent) this.buttonStop.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickStop)));
      // ISSUE: method pointer
      ((UnityEvent) this.buttonPlay.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickPlay)));
      // ISSUE: method pointer
      ((UnityEvent) this.buttonPause.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickPause)));
      // ISSUE: method pointer
      ((UnityEvent) this.buttonExpansion.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickExpansion)));
      this.InitList();
    }

    private void OnEnable()
    {
      ((Selectable) this.buttonRepeat).get_image().set_sprite(this.spriteRepeat[Singleton<Studio.Studio>.Instance.outsideSoundCtrl.repeat != BGMCtrl.Repeat.None ? 1 : 0]);
    }

    private void Update()
    {
      ((Behaviour) this.imagePlayNow).set_enabled(Singleton<Studio.Studio>.Instance.outsideSoundCtrl.play);
    }
  }
}
