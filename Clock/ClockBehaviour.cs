using System;
using System.Linq;
using Prototyping;
using TMPro;
using UnityEngine;

namespace Clock
{
    public class ClockBehaviour : MonoBehaviour
    {
        private TextMeshPro TextMesh;

        private void Start()
        {
            var canvas = this.gameObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = RG.ActiveCamera;
            canvas.transform.position = new Vector3(-100, 65, 0);
            canvas.transform.rotation = Quaternion.Euler(new Vector3(-20, -90, 0));
            
            this.TextMesh = canvas.gameObject.AddComponent<TextMeshPro>();
            this.TextMesh.gameObject.SetActive(false);
            this.TextMesh.fontSize = 45;
            this.TextMesh.alignment = TextAlignmentOptions.Center;
            this.TextMesh.text = DateTime.Now.ToString("hh:mm tt");
            this.TextMesh.font = Resources.FindObjectsOfTypeAll<TMP_FontAsset>().First(x => x.name == "Demeo SDF");
            this.TextMesh.gameObject.SetActive(true);
        }
        
        private void Update()
        {
            this.TextMesh.text = DateTime.Now.ToString("hh:mm tt");
        }
    }
}