using System.Collections.Generic;
using Com.Amalto.Imdg.Cm;

namespace Library.Models {
	public class CallServiceParameters {
		/** Email address of the caller. */
		public string Username;
		/** Identifier of the recipient service. */
		public string ReceiverId;
		/** Define the Platform 6 specific `action` header value. */
		public string Action;
		/** Custom headers to be sent with the request. */
		public IEnumerable<CommonMessage.Types.Header> Headers;
		/** Custom attachments to be sent with the request. */
		public IEnumerable<CommonMessage.Types.Attachment> Attachments;
	}
}