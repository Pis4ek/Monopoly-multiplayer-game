using NaughtyAttributes;
using Playmode.PlayData;
using Playmode.ServerEnteties;
using Playmode.View;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace Testing_Objects
{
    public class ServerGameDataTester : MonoBehaviour
    {
        [Inject(Optional = true)] GameData _data;

        [SerializeField] PlayerID _playerID;
        [SerializeField] CellID _cellID;

        private CustomLayout _playersOnMapLayout;

        [Button("Print player info")]
        private void PrintPlayerInfo()
        {
            if (_data != null)
            {
                try
                {
                    string owned = "";
                    foreach (var c in _data.GetCellsByPlayer(_playerID))
                    {
                        owned += c.ToString() + "\n";
                    }
                    var mes = $"PLAYER{(int)_playerID + 1} INFO: \n{_data[_playerID]}";
                    mes += $"Cells: \n{owned}\n";
                    Debug.Log(mes);
                }
                catch
                {
                    Debug.LogError($"Data has not player with ID {_playerID}");
                }
            }
            else
            {
                Debug.LogError($"Server GameData is null");
            }
        }

        [Button("Print cell info")]
        private void PrintCellInfo()
        {
            if (_data != null)
            {
                try
                {
                    Debug.Log($"CELL{(int)_cellID + 1} INFO: \n{_data[(int)_cellID]}");
                }
                catch
                {
                    Debug.LogError($"Data has not cell with ID {_cellID}");
                }
            }
            else
            {
                Debug.LogError($"Server GameData is null");
            }
        }
    }
}