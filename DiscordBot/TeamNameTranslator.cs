using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DiscordBot
{
    public class TeamNameTranslator
    {
        Dictionary<int, List<string>> teamDict;
        public TeamNameTranslator()
        {
            LoadTeamNames();
        }

        private void LoadTeamNames()
        {
            var jobj = JObject.Parse(File.ReadAllText("TeamMapping.json"));
            teamDict = JsonConvert.DeserializeObject<Dictionary<int, List<string>>>(jobj["TeamNames"].ToString());
        }

        public List<KeyValuePair<int, List<string>>> LookupIdsForName(string name)
        {
            var result = new List<KeyValuePair<int, List<string>>>();
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;

            foreach (var pair in teamDict)
            {
                if (pair.Value.Contains(textInfo.ToTitleCase(name)))
                {
                    result.Add(pair);
                }
            }
            return result;
        }
    }
}