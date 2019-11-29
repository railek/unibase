using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Railek.Unibase
{
  [Serializable]
  [RequireComponent(typeof(BoxCollider))]
  public class SceneTrigger : MonoBehaviour
  {
      [SerializeField] private string[] visibleScenes;
      [SerializeField] private List<string> visibleSceneNames = new List<string>();

      private bool _visibleSceneNamesPopulated;

      private void OnTriggerEnter(Collider other)
      {
          if (!other.gameObject.CompareTag("Player"))
          {
              return;
          }

          if (!_visibleSceneNamesPopulated || visibleSceneNames.Count == 0)
          {
              PopulateVisibleSceneNames();
          }

          foreach (var sceneName in visibleSceneNames)
          {
              SceneController.Instance.LoadLevel(sceneName);
          }

          SceneController.Instance.UnloadLevels(visibleSceneNames.ToArray());
      }

      private void OnValidate()
      {
          PopulateVisibleSceneNames();
      }

      private void PopulateVisibleSceneNames()
      {
          visibleSceneNames = visibleScenes.Where(SceneController.Instance.SceneExists).ToList();
          visibleSceneNames.Add(gameObject.scene.name);
          _visibleSceneNamesPopulated = true;
      }

      #if UNITY_EDITOR
      [SerializeField] private Color unselectedGizmoColor = new Color(0.1f, 1.0f, 0.1f, .3f);
      [SerializeField] private Color selectedGizmoColor = new Color(0.1f, 1.0f, 0.1f, .6f);

      private void OnDrawGizmos()
      {
          Gizmos.color = unselectedGizmoColor;
          Gizmos.DrawWireCube(transform.position, GetComponent<Collider>().bounds.size);
      }

      private void OnDrawGizmosSelected()
      {
          Gizmos.color = selectedGizmoColor;
          Gizmos.DrawCube(transform.position, GetComponent<Collider>().bounds.size);
      }
      #endif
  }

}

