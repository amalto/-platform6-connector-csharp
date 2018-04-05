using static Com.Amalto.Imdg.Cm.CommonMessage.Types;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Com.Amalto.Imdg.Cm;
using Hazelcast.Client;
using Hazelcast.Config;
using Hazelcast.Core;
using Library.Models;
using Library.Serializers;
using Newtonsoft.Json;

namespace Library {
    public class Service {
        private string _idKey;

        public IHazelcastInstance Client;
        public Task Deployed;

        public Service(DeployParameters parameters) {
            _idKey = Constants.SenderIdPrefix + parameters.Id;
            Deployed = DeployService(parameters);
        }

        private async Task CreateHazelcastClient() {
            Environment.SetEnvironmentVariable("hazelcast.logging.level", "info");
            Environment.SetEnvironmentVariable("hazelcast.logging.type", "console");

            var config = new ClientConfig();
            var hostname = Environment.GetEnvironmentVariable("HOSTNAME") ?? "localhost";
            var port = Environment.GetEnvironmentVariable("PORT") != null ? Convert.ToUInt32(Environment.GetEnvironmentVariable("PORT"), 10) : 5900;

            var serializerConfig = new SerializerConfig();
            serializerConfig.SetImplementation(new Message()).SetTypeClass(typeof(CommonMessage));

            config.GetSerializationConfig().AddSerializerConfig(serializerConfig);
            config.GetNetworkConfig().AddAddress(hostname + ":" + port);

            Client = Task.Run(() => HazelcastClient.NewHazelcastClient(config)).Result;
        }

        private async Task DeployService(DeployParameters parameters) {
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
                    BusConnection.CreateHeader(Constants.ServiceManagerId, "service.ui", JsonConvert.SerializeObject(parameters.Ui))
                }
            });
        }

        public async Task<CommonMessage> CallService(CallServiceParameters parameters) {
            var receiverId = parameters.ReceiverId;
            var headers = new List<Header> {BusConnection.CreateHeader(null, Constants.UserKey, parameters.Username)};
            var attachments = parameters.Attachments ?? new List<Attachment>();

            if (parameters.Action != null) headers.Add(BusConnection.CreateHeader(receiverId, Constants.Action, parameters.Action));
            if (parameters.Headers != null) headers.AddRange(parameters.Headers);

            var commonMessage = Task.Run(() => BusConnection.CreateCommonMessage(_idKey, parameters.ReceiverId, headers, attachments)).Result;
            return await SendCommonMessage(receiverId, commonMessage);
        }

        public async Task<CommonMessage> SendCommonMessage(string receiverId, CommonMessage commonMessage) {
            var receiverIdKey = Constants.ReceiverIdPrefix + receiverId;
            var request = Client.GetQueue<CommonMessage>(receiverIdKey);
            var isSent = Task.Run(() => request.Offer(commonMessage)).Result;

            if (!isSent) throw new Exception("Unable to send the common message with id " + commonMessage.Id + "!");

            BusConnection.DisplayCommonMessage(receiverIdKey, commonMessage);

            var response = Task.Run(() => Client.GetQueue<CommonMessage>(_idKey).Take()).Result;

            BusConnection.DisplayCommonMessage(receiverIdKey, response);

            if (!response.Id.Equals(commonMessage.Id))
                throw new Exception("Common message response's id " + response.Id + " is not the same as the common message request's id " + commonMessage.Id + "!");

            return response;
        }
    }
}
