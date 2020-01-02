// Decompiled with JetBrains decompiler
// Type: Studio.LogoList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Studio
{
  public class LogoList : MonoBehaviour
  {
    [SerializeField]
    private GameObject objectNode;
    [SerializeField]
    private Transform transformRoot;
    [SerializeField]
    private Image imageLogo;
    [SerializeField]
    private Sprite[] spriteLogo;
    [SerializeField]
    private string[] strName;
    private int select;
    private Dictionary<int, ListNode> dicNode;
    private bool isInit;

    public LogoList()
    {
      base.\u002Ector();
    }

    public void UpdateInfo()
    {
      if (!this.isInit)
        return;
      foreach (KeyValuePair<int, ListNode> keyValuePair in this.dicNode)
        keyValuePair.Value.select = false;
      int logo = Studio.Studio.optionSystem.logo;
      ListNode listNode = (ListNode) null;
      if (this.dicNode.TryGetValue(logo, out listNode))
      {
        listNode.select = true;
        this.select = logo;
      }
      else
      {
        if (!this.dicNode.TryGetValue(-1, out listNode))
          return;
        listNode.select = true;
        this.select = -1;
      }
    }

    public void OnClick(int _no)
    {
      Studio.Studio.optionSystem.logo = _no;
      this.UpdateLogo();
      ListNode listNode = (ListNode) null;
      if (this.dicNode.TryGetValue(this.select, out listNode))
        listNode.select = false;
      if (this.dicNode.TryGetValue(_no, out listNode))
        listNode.select = true;
      this.select = _no;
    }

    public void Init()
    {
      for (int _key = 0; _key < this.strName.Length; ++_key)
        this.AddNode(_key, this.strName[_key]);
      ListNode listNode = (ListNode) null;
      if (this.dicNode.TryGetValue(Studio.Studio.optionSystem.logo, out listNode))
        listNode.select = true;
      this.UpdateLogo();
      this.select = Studio.Studio.optionSystem.logo;
      this.isInit = true;
    }

    private void AddNode(int _key, string _name)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      LogoList.\u003CAddNode\u003Ec__AnonStorey0 nodeCAnonStorey0 = new LogoList.\u003CAddNode\u003Ec__AnonStorey0();
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
      // ISSUE: reference to a compiler-generated field
      this.dicNode.Add(nodeCAnonStorey0.key, component);
    }

    private void UpdateLogo()
    {
      Sprite sprite = this.spriteLogo.SafeGet<Sprite>(Studio.Studio.optionSystem.logo);
      this.imageLogo.set_sprite(sprite);
      ((Graphic) this.imageLogo).set_color(!Object.op_Equality((Object) sprite, (Object) null) ? Color.get_white() : Color.get_clear());
    }
  }
}
