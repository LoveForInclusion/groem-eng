using System;

namespace Model 
{
    [Serializable]
    public class Addon
    {
        public string id;
        public string storyId;
        public long created;
        public long lastUpdate;
        public string webserverUrl;
        public string category;
        public float price;
        public string age;
        public bool comingSoon;
        public string shelfCover;
        public string data;
        public string title;

        public Addon(string id, string storyId, long created, long lastUpdate, string webserverUrl, string category, float price, string age, bool comingSoon, string shelfCover, string data, string title)
        {
            this.id = id;
            this.storyId = storyId;
            this.created = created;
            this.lastUpdate = lastUpdate;
            this.webserverUrl = webserverUrl;
            this.category = category;
            this.price = price;
            this.age = age;
            this.comingSoon = comingSoon;
            this.shelfCover = shelfCover;
            this.data = data;
            this.title = title;
        }
    }
}
