using System.Collections.Generic;
using Core.OnlineRuntimeModule.EnvironmentCustomize.Model;
using UnityEngine;

namespace Core.OnlineRuntimeModule.EnvironmentCustomize.View
{
    public class ItemObject: MonoBehaviour
    {
        [SerializeField] public Sprite icon;
        [SerializeField] public int ID;
        [SerializeField] public MeshRenderer meshRenderer;
        [SerializeField] private Material valid;
        [SerializeField] private Material invalid;

        public string databaseID;

        public void ImportData(EnvironmentItemData data)
        {
            var t = transform;
            t.rotation = Quaternion.Euler(0, data.RotY, 0);
            t.position = new Vector3(data.PosX, data.PosY, data.PosZ);
            databaseID = data.UID;
            meshRenderer.enabled = false;
        }

        public EnvironmentItemData ExportData()
        {
            var t = transform;
            var position = t.position;
            return new EnvironmentItemData()
            {
                ID = ID,
                UID = databaseID,
                PosX = position.x,
                PosY = position.y,
                PosZ = position.z,
                RotY = t.rotation.eulerAngles.y
            };
        }

        private void Awake()
        {
            isValid = true;
            meshRenderer.material = valid;
            meshRenderer.enabled = false;
            CollisionObject = new();
        }

        public bool isValid;
        public HashSet<GameObject> CollisionObject;

        private void Update()
        {
            if (CollisionObject.Count != 0)
            {
                if (!isValid) return;
                meshRenderer.material = invalid;
                isValid = false;
            }
            else
            {
                if (isValid) return;
                meshRenderer.material = valid;
                isValid = true;
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.name == "Ground")
                return;
            CollisionObject.Add(other.gameObject);
            meshRenderer.material = invalid;
            isValid = false;
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject.name == "Ground")
                return;
            CollisionObject.Remove(other.gameObject);
            isValid = CollisionObject.Count == 0;
            meshRenderer.material = isValid ? valid: invalid;
        }
    }
}