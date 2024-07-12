using System;
using System.Collections.Generic;

namespace Model 
{
	[Serializable]
	public class Code
	{
		public string id;
		public string storyId;
		public string addonId;
		public string userId;
		public string code;
		public string type;
		public bool redeemed;
		public long purchaseData;
		public long redeemData;
		public string redeemedBy;
		public float price;

		public Code( string id,
			 string storyId,
			 string addonId,
			 string userId,
			 string code,
			 string type,
			 bool redeemed,
			 long purchaseData,
			 long redeemData,
			 string redeemedBy,
			 float price)
		{
			this.id = id;
			this.storyId = storyId;
			this.addonId = addonId;
			this.userId = userId;
			this.code = code;
			this.type = type;
			this.redeemed = redeemed;
			this.purchaseData = purchaseData;
			this.redeemData = redeemData;
			this.redeemedBy = redeemedBy;
			this.price = price;
		}

	}

	[Serializable]
	public class Codes
	{
		public List<Code> codes;

		public Codes(List<Code> codes)
		{
			this.codes = codes;
		}
	}

}
