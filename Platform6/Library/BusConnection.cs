using System;
using System.Collections.Generic;
using Com.Amalto.Imdg.Cm;
using Newtonsoft.Json.Linq;

namespace Library {
	public class BusConnection {
		public static CommonMessage CreateCommonMessage(string senderId, string receiverId, IEnumerable<CommonMessage.Types.Header> headers, IEnumerable<CommonMessage.Types.Attachment> attachments) {
			return new CommonMessage {
				Id = Guid.NewGuid().ToString(),
				Destination = Constants.ReceiverIdPrefix + receiverId,
				ReplyTo = senderId,
				Headers = {headers},
				Attachments = {attachments}
			};
		}

		public static CommonMessage.Types.Header CreateHeader(string receiverId, string key, object value) {
			return new CommonMessage.Types.Header{ Key = receiverId != null ? FormatHeaderKey(receiverId, key) : key, Value = value.ToString() };
		}

		public static string FormatHeaderKey(string serviceId, string key) {
			return Constants.HeaderKeyPrefix + serviceId + Constants.IdSeparator + key;
		}

		// TODO: implement the same system than the platform6-client-nodejs
		public static void DisplayCommonMessage(string counterpartIdKey, CommonMessage commonMessage) {
			Console.WriteLine(JObject.Parse(commonMessage.ToString()) + "\n");
		}
	}
}