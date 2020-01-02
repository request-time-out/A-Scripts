// Decompiled with JetBrains decompiler
// Type: Exploder.Demo.DemoClickExplode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Exploder.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Exploder.Demo
{
  public class DemoClickExplode : MonoBehaviour
  {
    private GameObject[] DestroyableObjects;
    private ExploderObject Exploder;
    public Camera Camera;

    public DemoClickExplode()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      Application.set_targetFrameRate(60);
      this.Exploder = ExploderSingleton.Instance;
      if (this.Exploder.DontUseTag)
      {
        Object[] objectsOfType = Object.FindObjectsOfType(typeof (Explodable));
        List<GameObject> gameObjectList = new List<GameObject>(objectsOfType.Length);
        gameObjectList.AddRange(((IEnumerable) objectsOfType).Cast<Explodable>().Where<Explodable>((Func<Explodable, bool>) (ex => Object.op_Implicit((Object) ex))).Select<Explodable, GameObject>((Func<Explodable, GameObject>) (ex => ((Component) ex).get_gameObject())));
        this.DestroyableObjects = gameObjectList.ToArray();
      }
      else
        this.DestroyableObjects = GameObject.FindGameObjectsWithTag("Exploder");
    }

    private bool IsExplodable(GameObject obj)
    {
      return this.Exploder.DontUseTag ? Object.op_Inequality((Object) obj.GetComponent<Explodable>(), (Object) null) : obj.CompareTag(ExploderObject.Tag);
    }

    private void Update()
    {
      RaycastHit raycastHit;
      if (!Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1) || !Physics.Raycast(!Object.op_Implicit((Object) this.Camera) ? Camera.get_main().ScreenPointToRay(Input.get_mousePosition()) : this.Camera.ScreenPointToRay(Input.get_mousePosition()), ref raycastHit))
        return;
      GameObject gameObject = ((Component) ((RaycastHit) ref raycastHit).get_collider()).get_gameObject();
      if (!this.IsExplodable(gameObject))
        return;
      if (Input.GetMouseButtonDown(0))
        this.ExplodeObject(gameObject);
      else
        this.ExplodeAfterCrack(gameObject);
    }

    private void ExplodeObject(GameObject obj)
    {
      this.Exploder.ExplodeObject(obj, new ExploderObject.OnExplosion(this.OnExplosion));
    }

    private void OnExplosion(float time, ExploderObject.ExplosionState state)
    {
    }

    private void OnCracked(float time, ExploderObject.ExplosionState state)
    {
    }

    private void ExplodeAfterCrack(GameObject obj)
    {
    }

    private void OnGUI()
    {
      if (GUI.Button(new Rect(10f, 10f, 100f, 30f), "Reset") && !this.Exploder.DestroyOriginalObject)
      {
        foreach (GameObject destroyableObject in this.DestroyableObjects)
          ExploderUtils.SetActiveRecursively(destroyableObject, true);
        ExploderUtils.SetActive(((Component) this.Exploder).get_gameObject(), true);
      }
      if (!GUI.Button(new Rect(10f, 50f, 100f, 30f), "NextScene"))
        return;
      SceneManager.LoadScene(0);
    }
  }
}
