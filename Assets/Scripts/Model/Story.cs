using System;
using System.Collections.Generic;

namespace Model 
{
	[Serializable]
	public class Activities{
		public string name;
		public string type;
		public string description;

		public Activities(string name,string type,string description){

			this.name = name;
			this.type = type;
			this.description = description;
		}
	}
		

    [Serializable]
    public class Story
    {
        public string id;
		public string androidData;
		public string iosData;
        public List<Addon> addons;
        public long created;
        public long lastUpdate;
        public string webserverUrl;
		public List<Activities> activities;
		public bool comingSoon;
		public string cover;
		public string shelfCover;
		public string category;
		public float price;
		public string age;
		public string author;
		public string description;
		public string title;
		public bool isDownloaded;
       
		public Story(string id,
			string androidData,
			string iosData,
			List<Addon> addons,
			long created,
			long lastUpdate,
			string webserverUrl,
			List<Activities> activities,
			bool comingSoon,
			string cover,
			string shelfCover,
			string category,
			float price,
			string age,
			string author,
			string description,
			string title,
			bool isDownloaded = false){

			this.androidData = androidData;
			this. iosData=iosData;
			this.addons = addons;
			this.created = created;
			this.lastUpdate = lastUpdate;
			this.webserverUrl = webserverUrl;
			this.activities = activities;
			this.comingSoon = comingSoon;
			this.cover = cover;
			this.shelfCover = shelfCover;
			this.category = category;
			this.price = price;
			this.age = age;
			this.author = author;
			this.description = description;
			this.title = title;
			this.isDownloaded = isDownloaded;
			
		}
			
    }

    [Serializable]
    public class Stories
    {
        public List<Story> stories;

        public Stories(List<Story> stories)
        {
            this.stories = stories;
        }
    }

}
