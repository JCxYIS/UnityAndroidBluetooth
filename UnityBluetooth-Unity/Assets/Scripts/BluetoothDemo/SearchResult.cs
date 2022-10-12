using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


namespace JC.BluetoothUnity.Demo
{
    [System.Serializable]
    public class SearchResult : MonoBehaviour
    {
        [SerializeField] Text _nameText;
        [SerializeField] Text _macText;
        [SerializeField] Button _connectButton;

        public void Init(string name, string mac)
        {
            _nameText.text = name;
            _macText.text = mac;
            _connectButton.onClick.AddListener(Connect);
        }

        public void Connect()
        {
            if (BluetoothManager.Connect(_macText.text))
            {
                FindObjectOfType<PanelController>().ShowPanel(PanelController.PanelType.Message);
            }
        }
    }
}