// Decompiled with JetBrains decompiler
// Type: Exploder.Demo.TargetManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Exploder.Demo
{
  public class TargetManager : MonoBehaviour
  {
    private static TargetManager instance;
    public GameObject TargetObject;
    public TargetType TargetType;
    public Vector3 TargetPosition;
    public Image CrosshairGun;
    public Image CrosshairHand;
    public ExploderMouseLook MouseLookCamera;
    public Text PanelText;

    public TargetManager()
    {
      base.\u002Ector();
    }

    public static TargetManager Instance
    {
      get
      {
        return TargetManager.instance;
      }
    }

    private void Awake()
    {
      TargetManager.instance = this;
    }

    private void Start()
    {
      ExploderUtils.SetActive(((Component) this.CrosshairGun).get_gameObject(), true);
      ExploderUtils.SetActive(((Component) this.CrosshairHand).get_gameObject(), true);
      ExploderUtils.SetActive(((Component) this.PanelText).get_gameObject(), true);
    }

    private void Update()
    {
      Ray ray = this.MouseLookCamera.mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
      Debug.DrawRay(((Ray) ref ray).get_origin(), Vector3.op_Multiply(((Ray) ref ray).get_direction(), 10f), Color.get_red(), 0.0f);
      ((Graphic) this.CrosshairGun).set_color(Color.get_white());
      this.TargetObject = (GameObject) null;
      this.TargetType = TargetType.None;
      this.TargetPosition = Vector3.get_zero();
      List<RaycastHit> raycastHitList = new List<RaycastHit>((IEnumerable<RaycastHit>) Physics.RaycastAll(ray, float.PositiveInfinity));
      GameObject gameObject = (GameObject) null;
      if (raycastHitList.Count > 0)
      {
        raycastHitList.Sort((Comparison<RaycastHit>) ((a, b) =>
        {
          Vector3 vector3_1 = Vector3.op_Subtraction(((Component) this.MouseLookCamera).get_transform().get_position(), ((RaycastHit) ref a).get_point());
          float sqrMagnitude1 = ((Vector3) ref vector3_1).get_sqrMagnitude();
          ref float local = ref sqrMagnitude1;
          Vector3 vector3_2 = Vector3.op_Subtraction(((Component) this.MouseLookCamera).get_transform().get_position(), ((RaycastHit) ref b).get_point());
          double sqrMagnitude2 = (double) ((Vector3) ref vector3_2).get_sqrMagnitude();
          return local.CompareTo((float) sqrMagnitude2);
        }));
        RaycastHit raycastHit1 = raycastHitList[0];
        gameObject = ((Component) ((RaycastHit) ref raycastHit1).get_collider()).get_gameObject();
        RaycastHit raycastHit2 = raycastHitList[0];
        this.TargetPosition = ((RaycastHit) ref raycastHit2).get_point();
      }
      if (Object.op_Inequality((Object) gameObject, (Object) null))
      {
        this.TargetObject = gameObject;
        if (this.IsDestroyableObject(this.TargetObject))
          this.TargetType = TargetType.DestroyableObject;
        else if (this.IsUseObject(this.TargetObject))
        {
          UseObject component = (UseObject) this.TargetObject.GetComponent<UseObject>();
          if (Object.op_Implicit((Object) component))
          {
            Vector3 vector3 = Vector3.op_Subtraction(((Component) this.MouseLookCamera).get_transform().get_position(), ((Component) component).get_transform().get_position());
            if ((double) ((Vector3) ref vector3).get_sqrMagnitude() < (double) component.UseRadius * (double) component.UseRadius)
              this.TargetType = TargetType.UseObject;
          }
        }
        else
          this.TargetType = TargetType.Default;
      }
      switch (this.TargetType)
      {
        case TargetType.DestroyableObject:
          ((Behaviour) this.CrosshairHand).set_enabled(false);
          ((Behaviour) this.CrosshairGun).set_enabled(true);
          ((Graphic) this.CrosshairGun).set_color(Color.get_red());
          break;
        case TargetType.UseObject:
          ((Behaviour) this.CrosshairGun).set_enabled(false);
          ((Behaviour) this.CrosshairHand).set_enabled(true);
          ((Behaviour) this.PanelText).set_enabled(true);
          this.PanelText.set_text(((UseObject) this.TargetObject.GetComponent<UseObject>()).HelperText);
          break;
        case TargetType.Default:
        case TargetType.None:
          ((Behaviour) this.CrosshairHand).set_enabled(false);
          ((Behaviour) this.CrosshairGun).set_enabled(true);
          ((Graphic) this.CrosshairGun).set_color(Color.get_white());
          ((Behaviour) this.PanelText).set_enabled(false);
          break;
      }
      if (!Input.GetKeyDown((KeyCode) 101) || this.TargetType != TargetType.UseObject)
        return;
      UseObject component1 = (UseObject) this.TargetObject.GetComponent<UseObject>();
      if (!Object.op_Implicit((Object) component1))
        return;
      component1.Use();
    }

    private bool IsDestroyableObject(GameObject obj)
    {
      if (obj.CompareTag("Exploder"))
        return true;
      return Object.op_Implicit((Object) obj.get_transform().get_parent()) && this.IsDestroyableObject(((Component) obj.get_transform().get_parent()).get_gameObject());
    }

    private bool IsUseObject(GameObject obj)
    {
      if (obj.CompareTag("UseObject"))
        return true;
      return Object.op_Implicit((Object) obj.get_transform().get_parent()) && this.IsDestroyableObject(((Component) obj.get_transform().get_parent()).get_gameObject());
    }
  }
}
