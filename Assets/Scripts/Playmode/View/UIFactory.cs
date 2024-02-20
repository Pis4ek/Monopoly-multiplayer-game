using Playmode.PlayData;
using Playmode.PlayData.ClientsData;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

namespace Playmode.View
{
    public class UIFactory
    {
        private DiContainer _container;
        private Settings _settings;

        public UIFactory(Settings settings, DiContainer container)
        {
            _container = container;
            _settings = settings;
        }

        public ChanceCellView CreateChanceCell(Vector2 position, Transform parent = null)
        {
            var go = _container.InstantiatePrefab(
                _settings.ChanceCellPrefab,
                parent);
            var cell = go.GetComponent<ChanceCellView>();
            go.transform.localPosition = position;

            return cell;
        }

        public EdgeCellView CreateEdgeCell(Vector2 position, Transform parent = null)
        {
            var go = _container.InstantiatePrefab(
                _settings.EdgeCellPrefab,
                parent);

            var cell = go.GetComponent<EdgeCellView>();
            go.transform.localPosition = position;

            return cell;
        }

        public BusinessCellView CreateBusinessCell(Vector2 position, Transform parent = null)
        {
            var go = _container.InstantiatePrefab(_settings.BusinessCellPrefab, parent);
            var cell = go.GetComponent<BusinessCellView>();
            go.transform.localPosition = position;

            return cell;
        }

        public CommunalCellView CreateCommunalCell(Vector2 position, Transform parent = null)
        {
            var go = _container.InstantiatePrefab(
                _settings.CommunalCellPrefab,
                parent);
            var cell = go.GetComponent<CommunalCellView>();
            go.transform.localPosition = position;

            return cell;
        }

        public InfrastructureCellView CreateInfrastructureCell(Vector2 position, Transform parent = null)
        {
            var go = _container.InstantiatePrefab(_settings.InfrastructureCellPrefab, parent);
            var cell = go.GetComponent<InfrastructureCellView>();
            go.transform.localPosition = position;

            return cell;
        }

        public PlayerOnMapView CreatePlayerOnMap(Vector2 position, Transform parent = null)
        {
            var go = _container.InstantiatePrefab(_settings.PlayerOnMapPrefab, parent);
            var player = go.GetComponent<PlayerOnMapView>();
            go.transform.position = position;
            return player;
        }

        public PlayerInfoPanel CreatePlayerInfoPanel(Vector2 position,
            ClientsPlayer player, Transform parent = null)
        {
            var go = _container.InstantiatePrefab(_settings.PlayerInfoPanelPrefab, parent);
            var panel = go.GetComponent<PlayerInfoPanel>();
            panel.Init(player);

            return panel;
        }

        public TradeCellPanel CreateTradeCellPanel(Transform parent = null)
        {
            return _container.InstantiatePrefab(
                _settings.TradeCellPanelPrefab,
                Vector3.zero,
                Quaternion.identity,
                parent)
                .GetComponent<TradeCellPanel>();
        }
        public LogsContainer CreateLogsContainer(Transform parent = null)
        {
            var obj = _container.InstantiatePrefab(_settings.LogContainerPrefab, parent);
            var component = obj.GetComponent<LogsContainer>();

            return component;
        }
        public TextMeshProUGUI CreateTextLogElement(Transform parent = null)
        {
            var obj = _container.InstantiatePrefab(_settings.LogTextPrefab, parent);
            var component = obj.GetComponent<TextMeshProUGUI>();

            return component;
        }

        [Serializable]
        public class Settings
        {
            public GameObject ChanceCellPrefab;
            public GameObject EdgeCellPrefab;
            public GameObject BusinessCellPrefab;
            public GameObject CommunalCellPrefab;
            public GameObject InfrastructureCellPrefab;
            public GameObject PlayerOnMapPrefab;
            public GameObject PlayerInfoPanelPrefab;
            public GameObject TradeCellPanelPrefab;
            public GameObject LogContainerPrefab;
            public GameObject LogTextPrefab;
        }
    }
}