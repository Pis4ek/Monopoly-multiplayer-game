using System.Collections.Generic;
using UnityEngine;

namespace Playmode.View
{
    public class CustomLayout
    {
        public float Spacing = 5f;
        public float Padding = 5f;
        //sdelat proverku na 1 element!!!
        public List<Vector2> CalcVerticalLayout(List<RectTransform> players, Rect container, out float preferredScale)
        {
            List<Vector2> points = new List<Vector2>(players.Count);
            var rect = container;
            var center = container.center;
            var allPlayersSize = CalcAllPlayersSize(players, true);

            float segmentSize = (rect.height - Spacing * (players.Count - 1) - 2 * Padding) / players.Count;

            preferredScale = 1f;
            if (allPlayersSize > segmentSize * 5) 
            {
                foreach (var player in players)
                {
                    var playerTransform = player.transform as RectTransform;
                    preferredScale = segmentSize / playerTransform.rect.size.y;
                }
            }

            var start = center.y - rect.height / 2 + Padding + segmentSize / 2;
            points.Add(new Vector2(center.x, start));

            for (int i = 1; i < players.Count; i++)
            {
                var point = points[i - 1].y + segmentSize + Spacing;
                points.Add(new Vector2(center.x, point));
            }

            return points;
        }
        //sdelat proverku na 1 element!!!
        public List<Vector2> CalcHorizontalLayout(List<RectTransform> players, Rect container, out float preferredScale)
        {
            List<Vector2> points = new List<Vector2>(players.Count);
            var rect = container;
            var center = container.center;
            var allPlayersSize = CalcAllPlayersSize(players, false);

            float segmentSize = (rect.width - Spacing * (players.Count - 1) - 2 * Padding) / players.Count;

            preferredScale = 1f;
            if (allPlayersSize > segmentSize * 5)
            {
                foreach (var player in players)
                {
                    var playerTransform = player.transform as RectTransform;
                    preferredScale = segmentSize / playerTransform.rect.size.x;
                }
            }

            var start = center.x - rect.width / 2 + Padding + segmentSize / 2;
            points.Add(new Vector2(start, center.y));

            for (int i = 1; i < players.Count; i++)
            {
                var point = points[i - 1].x + segmentSize + Spacing;
                points.Add(new Vector2(point, center.y));
            }

            return points;
        }
        //sdelat proverku na 1 element!!!
        public List<Vector2> CalcRoundLayout(List<RectTransform> players, Rect container, out float preferredScale)
        {
            //sdelat proverku na 1 element!!!

            List<Vector2> points = new List<Vector2>(players.Count);
            var center = container.center;
            var angle = 360 / players.Count * Mathf.Deg2Rad;
            var offcetVector = Vector2.up * container.width / 3;

            points.Add(offcetVector + center);

            for(int i = 1; i < players.Count; i++)
            {
                float newX = offcetVector.x * Mathf.Cos(angle) - offcetVector.y * Mathf.Sin(angle);
                float newY = offcetVector.x * Mathf.Sin(angle) + offcetVector.y * Mathf.Cos(angle);
                offcetVector = new Vector2(newX, newY);
                points.Add(offcetVector + center);
            }

            float biggestSize = 0f;
            foreach(var p in players)
            {
                if(biggestSize < p.rect.size.x)
                {
                    biggestSize = p.rect.size.x;
                }
            }
            preferredScale = 1f;
            if (Vector2.Distance(points[0], points[1]) < biggestSize)
            {
                preferredScale = Vector2.Distance(points[0], points[1]) / biggestSize;
            }
            return points;
        }

        private float CalcAllPlayersSize(List<RectTransform> players, bool isVertical)
        {
            float size = 0f;
            if (isVertical)
            {
                foreach(var player in players)
                {
                    size += player.rect.width;
                }
            }
            else
            {
                foreach (var player in players)
                {
                    size += player.rect.height;
                }
            }
            return size;
        }
    }
}
