namespace Library {
	public static class Constants {
		public const string IdSeparator = ".";
		public const string Platform6 = "platform6";
		public const string Platform6AppKey = Platform6 + IdSeparator;
		public const string Action = "action";
		public const string ActionDeploy = "deploy";

		public const string HeaderKeyPrefix = "b2" + IdSeparator;
		public const string ReceiverIdPrefix = "cmb" + IdSeparator;
		public const string SenderIdPrefix = "tmp" + IdSeparator;

		public const string UserKey = HeaderKeyPrefix + "user";

		public const string ServiceManagerId = Platform6AppKey + "manager";
	}
}