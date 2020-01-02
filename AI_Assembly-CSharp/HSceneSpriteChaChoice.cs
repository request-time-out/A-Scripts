// Decompiled with JetBrains decompiler
// Type: HSceneSpriteChaChoice
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using AIProject;
using Manager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HSceneSpriteChaChoice : MonoBehaviour
{
  public Button[] OpenCharList;
  public Text LabelText;
  public GameObject Content;
  [SerializeField]
  private Button[] charlist;
  private ChaControl[] females;
  private ChaControl[] Males;
  private Actor[] actor;
  private HScene hScene;
  private HSceneManager hSceneManager;

  public HSceneSpriteChaChoice()
  {
    base.\u002Ector();
  }

  public void Init()
  {
    this.hScene = (HScene) ((Component) Singleton<HSceneFlagCtrl>.Instance).GetComponent<HScene>();
    this.hSceneManager = Singleton<HSceneManager>.Instance;
    this.females = this.hScene.GetFemales();
    this.Males = this.hScene.GetMales();
    this.actor = new Actor[4];
    for (int index = 0; index < this.charlist.Length; ++index)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      HSceneSpriteChaChoice.\u003CInit\u003Ec__AnonStorey0 initCAnonStorey0 = new HSceneSpriteChaChoice.\u003CInit\u003Ec__AnonStorey0();
      // ISSUE: reference to a compiler-generated field
      initCAnonStorey0.\u0024this = this;
      // ISSUE: reference to a compiler-generated field
      initCAnonStorey0.no = index;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      bool flag = initCAnonStorey0.no >= 2 ? Object.op_Inequality((Object) this.Males[initCAnonStorey0.no - 2], (Object) null) && Object.op_Inequality((Object) this.Males[initCAnonStorey0.no - 2].objTop, (Object) null) : Object.op_Inequality((Object) this.females[initCAnonStorey0.no], (Object) null) && Object.op_Inequality((Object) this.females[initCAnonStorey0.no].objTop, (Object) null);
      // ISSUE: reference to a compiler-generated field
      ((Component) this.charlist[initCAnonStorey0.no]).get_gameObject().SetActive(flag);
      if (flag)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.actor[initCAnonStorey0.no] = initCAnonStorey0.no >= 2 ? (Actor) ((Component) this.Males[initCAnonStorey0.no - 2]).GetComponentInParent<Actor>() : (Actor) ((Component) this.females[initCAnonStorey0.no]).GetComponentInParent<Actor>();
        // ISSUE: reference to a compiler-generated field
        if (initCAnonStorey0.no == 0)
        {
          Text labelText = this.LabelText;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          string str1 = !Object.op_Implicit((Object) this.actor[initCAnonStorey0.no]) ? ((Object) this.charlist[initCAnonStorey0.no]).get_name() : this.actor[initCAnonStorey0.no].CharaName;
          // ISSUE: reference to a compiler-generated field
          ((Text) ((Component) this.charlist[initCAnonStorey0.no]).GetComponentInChildren<Text>()).set_text(str1);
          string str2 = str1;
          labelText.set_text(str2);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ((Text) ((Component) this.charlist[initCAnonStorey0.no]).GetComponentInChildren<Text>()).set_text(!Object.op_Implicit((Object) this.actor[initCAnonStorey0.no]) ? ((Object) this.charlist[initCAnonStorey0.no]).get_name() : this.actor[initCAnonStorey0.no].CharaName);
        }
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: method pointer
      ((UnityEvent) this.charlist[initCAnonStorey0.no].get_onClick()).AddListener(new UnityAction((object) initCAnonStorey0, __methodptr(\u003C\u003Em__0)));
    }
    // ISSUE: method pointer
    ((UnityEvent) this.OpenCharList[0].get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003CInit\u003Em__0)));
    // ISSUE: method pointer
    ((UnityEvent) this.OpenCharList[1].get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003CInit\u003Em__1)));
  }

  public void SetAction(UnityAction action)
  {
    for (int index = 0; index < this.charlist.Length; ++index)
      ((UnityEvent) this.charlist[index].get_onClick()).AddListener(action);
  }

  public void CloseChoice()
  {
    ((Component) this.OpenCharList[0]).get_gameObject().SetActive(true);
    ((Component) this.OpenCharList[1]).get_gameObject().SetActive(false);
    this.Content.SetActive(false);
  }

  public void EndProc()
  {
    for (int index = 0; index < this.charlist.Length; ++index)
      ((UnityEventBase) this.charlist[index].get_onClick()).RemoveAllListeners();
    ((UnityEventBase) this.OpenCharList[0].get_onClick()).RemoveAllListeners();
    ((UnityEventBase) this.OpenCharList[1].get_onClick()).RemoveAllListeners();
  }

  public void SetMaleSelectBtn(bool setVal)
  {
    for (int index = 0; index < this.Males.Length; ++index)
    {
      if (Object.op_Inequality((Object) this.Males[index], (Object) null) && Object.op_Inequality((Object) this.Males[index].objTop, (Object) null))
        ((Component) this.charlist[index + 2]).get_gameObject().SetActive(setVal);
      else if (((Component) this.charlist[index + 2]).get_gameObject().get_activeSelf())
        ((Component) this.charlist[index + 2]).get_gameObject().SetActive(false);
    }
    if (setVal || this.hSceneManager.numFemaleClothCustom <= 1)
      return;
    this.hSceneManager.numFemaleClothCustom = 0;
    this.LabelText.set_text(!Object.op_Implicit((Object) this.actor[0]) ? ((Object) this.charlist[0]).get_name() : this.actor[0].CharaName);
  }
}
