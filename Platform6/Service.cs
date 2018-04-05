using static Com.Amalto.Imdg.Cm.CommonMessage.Types;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using csharp.Models;
using csharp.Serializers;
using Com.Amalto.Imdg.Cm;
using Hazelcast.Client;
using Hazelcast.Config;
using Hazelcast.Core;

namespace csharp {
    public class Service {
        private static string _idKey;

        public IHazelcastInstance Client;
        public Task Deployed;

        public Service(IDeployParameters parameters) {
            _idKey = Constants.SenderIdPrefix + parameters.Id;
            Deployed = DeployService(parameters);
        }

        private Task CreateHazelcastClient() {
            var config = new ClientConfig();
            var hostname = Environment.GetEnvironmentVariable("HOSTNAME") ?? "localhost";
            var port = Environment.GetEnvironmentVariable("PORT") == null ? Convert.ToUInt32(Environment.GetEnvironmentVariable("PORT"), 10) : 5900;

            var serializerConfig = new SerializerConfig();
            serializerConfig.SetImplementation(new Message()).SetTypeClass(typeof(CommonMessage));

            config.GetSerializationConfig().AddSerializerConfig(serializerConfig);
            config.GetNetworkConfig().AddAddress(hostname + ":" + port);

            Client = await HazelcastClient.NewHazelcastClient(config);
        }

        private async Task DeployService(IDeployParameters parameters) {
            if (Client == null) await CreateHazelcastClient();

            await CallService(new CallServiceParameters {
                Username = parameters.Username,
                ReceiverId = Constants.ServiceManagerId,
                Action = Constants.ActionDeploy,
                Headers = new List<Header> {
                    BusConnection.CreateHeader(Constants.ServiceManagerId, "node.id", Guid.NewGuid().ToString()),
                    BusConnection.CreateHeader(Constants.ServiceManagerId, "service.id", parameters.Id),
                    BusConnection.CreateHeader(Constants.ServiceManagerId, "service.path", parameters.Path),
                    BusConnection.CreateHeader(Constants.ServiceManagerId, "service.ctx", parameters.BasePath),
                    BusConnection.CreateHeader(Constants.ServiceManagerId, "service.version", parameters.Versions.Server),
                    BusConnection.CreateHeader(Constants.ServiceManagerId, "service.ui.version", parameters.Versions.Client),
                    BusConnection.CreateHeader(Constants.ServiceManagerId, "service.ui", parameters.Ui)
                }
            });
        }

        public async Task<CommonMessage> CallService(CallServiceParameters parameters) {
            var receiverId = parameters.ReceiverId;
            var headers = new List<Header> {BusConnection.CreateHeader(null, Constants.UserKey, parameters.Username)};

            if (parameters.Action != null) headers.Add(BusConnection.CreateHeader(receiverId, Constants.Action, parameters.Action));
            if (parameters.Headers != null) headers.AddRange(parameters.Headers);

            return await SendCommonMessage(receiverId, await BusConnection.CreateCommonMessage(_idKey, parameters.ReceiverId, headers, parameters.Attachments));
        }

        public async Task<CommonMessage> SendCommonMessage(string receiverId, CommonMessage commonMessage) {
            var receiverIdKey = Constants.ReceiverIdPrefix + receiverId;
            var request = Client.GetQueue<CommonMessage>(receiverIdKey);
            var isSent = await request.Offer(commonMessage);

            if (!isSent)
                throw new Exception("Unable to send the common message with id " + commonMessage.Id + "!");

            BusConnection.DisplayCommonMessage(receiverIdKey, commonMessage);

            var response = Client.GetQueue<CommonMessage>(_idKey).Take();

            BusConnection.DisplayCommonMessage(receiverIdKey, response);

            if (!response.Id.Equals(commonMessage.Id))
                throw new Exception("Common message response's id " + response.Id + " is not the same as the common message request's id " + commonMessage.Id + "!");

            return response;
        }

    }
}
