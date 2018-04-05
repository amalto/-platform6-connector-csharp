using Com.Amalto.Imdg.Cm;
using Google.Protobuf;
using Hazelcast.IO;
using Hazelcast.IO.Serialization;

namespace Library.Serializers {
	public class Message: IStreamSerializer<CommonMessage> {
		public int GetTypeId() {
			return 10;
		}

		public void Write(IObjectDataOutput output, CommonMessage message) {
			output.WriteByteArray(message.ToByteArray());
		}

		public CommonMessage Read(IObjectDataInput input) {
			return CommonMessage.Parser.ParseFrom(input.ReadByteArray());
		}

		public void Destroy() {}
	}
}
