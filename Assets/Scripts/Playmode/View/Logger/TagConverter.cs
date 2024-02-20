using Playmode.PlayData.ClientsData;
using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using Zenject;

namespace Playmode.View
{
    public class TagConverter
    {
        //[Inject] private ClientsGameData _data;
        [Inject] private Converter _converter = new();


        public string ConvertTagsInString(string originalText)
        {
            originalText = ConvertPlayersTags(originalText);

            return originalText;
        }

        private string ConvertPlayersTags(string originalText)
        {
            Regex regex = new(@"<cp>(\w*)</cp>");
            MatchCollection matches = regex.Matches(originalText);
            foreach (Match match in matches)
            {
                var index = match.Value.Trim("</cp>".ToCharArray());
                Debug.Log($"PlayerIndex: {index}");
                var color = _converter.PlayerColors[(PlayerID)Convert.ToInt32(index)];
                var newValue = $"<color={color.ToRGBA()}>{index}</color>";
                originalText = originalText.Replace(match.Value, newValue);
            }
            return originalText;
        }

        private string ConvertBusinessTags(string originalText)
        {
            Regex regex = new(@"<cc>(\w*)</cc>");
            MatchCollection matches = regex.Matches(originalText);
            foreach (Match match in matches)
            {
                var index = match.Value.Trim("</cc>".ToCharArray());
                var color = _converter.BusinessColors[(BusinessType)Convert.ToInt32(index)];
                var newValue = $"<color={color.ToRGBA()}>{index}</color>";
                originalText = originalText.Replace(match.Value, newValue);
            }
            return originalText;
        }
    }
}