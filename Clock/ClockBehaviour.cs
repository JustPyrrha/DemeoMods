using System;
using System.Linq;
using Prototyping;
using TMPro;
using UnityEngine;

namespace Clock
{
    public class ClockBehaviour : MonoBehaviour
    {
        private TextMeshPro _textMesh;

        private void Start()
        {
            var canvas = this.gameObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = RG.ActiveCamera;
            canvas.transform.position = new Vector3(-100, 65, 0);
            canvas.transform.rotation = Quaternion.Euler(new Vector3(-20, -90, 0));
            
            this._textMesh = canvas.gameObject.AddComponent<TextMeshPro>();
            this._textMesh.gameObject.SetActive(false);
            this._textMesh.fontSize = 45;
            this._textMesh.alignment = TextAlignmentOptions.Center;
            this._textMesh.text = DateTime.Now.ToString("hh:mm tt");
            this._textMesh.font = Resources.FindObjectsOfTypeAll<TMP_FontAsset>().First(x => x.name == "Demeo SDF");
            this._textMesh.gameObject.SetActive(true);
        }
        
        private void Update()
        {
            this._textMesh.text = DateTime.Now.ToString("hh:mm tt");
        }
    }
}