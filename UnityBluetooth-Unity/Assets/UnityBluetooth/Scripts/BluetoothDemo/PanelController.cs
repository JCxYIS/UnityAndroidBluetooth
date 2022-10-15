using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JC.BluetoothUnity.Demo
{
    public class PanelController : MonoBehaviour
    {
        [SerializeField] GameObject _searchPanelPrefab;
        [SerializeField] GameObject _messagePanelPrefab;        

        public enum PanelType { Search, Message }
        GameObject[] _panels;
        GameObject _currentPanel;


        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            // same order as the enum
            _panels = new GameObject[] { _searchPanelPrefab, _messagePanelPrefab };

        }

        // Start is called before the first frame update
        void Start()
        {
            ShowPanel(PanelType.Search);
        }


        public void ShowPanel(PanelType panel)
        {
            Destroy(_currentPanel);
            _currentPanel = Instantiate(_panels[(int)panel]);
        }
    }
}