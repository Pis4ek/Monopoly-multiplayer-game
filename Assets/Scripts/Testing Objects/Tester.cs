using NaughtyAttributes;
using Playmode.PlayData;
using Playmode.View;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System;
using TMPro;
using UnityEngine.UI;

namespace Testing_Objects
{
    public class Tester : MonoBehaviour
    {
        [Header("Cubes")]
        [SerializeField] Vector2Int _result;


        [SerializeField] Transform _conteinerGO;
        [SerializeField] ScrollRect _scrollView;
        [SerializeField] GameObject _prefab;

        [Inject] CubeResultShower _shower;

        [Button("ShowCubeResult")]
        private void ShowCubeResult()
        {
            _shower.ShowResult(new(PlayerID.Player1, _result.x, _result.y));
        }

        [Button("TestSpawnObj")]
        private void TST()
        {
        }
    }
}