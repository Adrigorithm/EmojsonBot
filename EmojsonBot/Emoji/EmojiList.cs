using System.Collections.Generic;
using Newtonsoft.Json;

namespace EmojsonBot.Config
{
    public class EmojiList
    {
        public List<Emoji> emojis { get; set; }

        public string toJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}