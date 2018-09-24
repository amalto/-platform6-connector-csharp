using System;
using System.Collections.Generic;
using Com.Amalto.Imdg.Cm;
using Newtonsoft.Json.Linq;

namespace Library {
	/// Platform 6 bus connection helpers
	public class BusConnection {
		/// <summary>Create a common message</summary>
		/// <param name="senderId">Sender's id</param>
		/// <param name="receiverId">Receiver's id</param>
		/// <param name="headers">Array of headers (each header's key must be unique)</param>
		/// <param name="attachments">Array of attachments</param>
		/// <returns>A new common message</returns>
		public static CommonMessage CreateCommonMessage(
			string senderId, string receiverId, IEnumerable<CommonMessage.Types.Header> headers, IEnumerable<CommonMessage.Types.Attachment> attachments
		) {
			return new CommonMessage {
				Id = Guid.NewGuid().ToString(),
				Destination = Constants.ReceiverIdPrefix + receiverId,
				ReplyTo = senderId,
				Headers = {headers},
				Attachments = {attachments}
			};
		}

		/// <summary>Create a common message's header</summary>
		/// <param name="key">The key of the header</param>
		/// <param name="value">The value of the header</param>
		/// <returns>A new common message's header</returns>
		public static CommonMessage.Types.Header CreateHeader(string key, object value) {
			return new CommonMessage.Types.Header{ Key = key, Value = value.ToString() };
		}

		// TODO: implement the same system than the platform6-client-nodejs
		/// <summary>Display in the console a common message</summary>
		/// <param name="commonMessage">Common message to display</param>
		public static void DisplayCommonMessage(CommonMessage commonMessage) {
			Console.WriteLine(JObject.Parse(commonMessage.ToString()) + "\n");
		}
	}
}