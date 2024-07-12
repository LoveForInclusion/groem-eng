using System;
using System.Collections.Generic;
using Model;
using UnityEngine;
using UnityEngine.Networking;

namespace Model{


	[Serializable]
	public class IAPReceipt{

		public string Store;
		public string TransactionID;
		public string Payload;
		private Payload pl;

		public IAPReceipt(string Store, string TransactionID, string Payload){

			this.Store = Store;
			this.TransactionID = TransactionID;
			this.Payload = Payload;

			//this.pl = JsonUtility.FromJson<Payload> (Payload.Replace("\\",""));
		}

		public Payload GetPayload(){
			return pl;
		}

	}

	[Serializable]
	public class Payload{

		public string json;
		public string signature;
		private PayloadDefinition payloadDefinition;

		public Payload(string json, string signature){
			this.json = json;
			this.signature = signature;
			//this.payloadDefinition = JsonUtility.FromJson<PayloadDefinition> (json.Replace("\\", ""));
		}

		public PayloadDefinition getPayloadDefinition(){
			return payloadDefinition;
		}

	}

	[Serializable]
	public class PayloadDefinition{
    
		public string orderId;
		public string packageName;
		public string productId;
		public long purchaseTime;
		public int purchaseState;
		public string purchaseToken;

		public PayloadDefinition(string orderId, string packageName, string productId, long purchaseTime, int purchaseState, string purchaseToken){

			this.orderId = orderId;
			this.packageName = packageName;
			this.productId = productId;
			this.purchaseTime = purchaseTime;
			this.purchaseState = purchaseState;
			this.purchaseToken = purchaseToken;

		}

	}


}