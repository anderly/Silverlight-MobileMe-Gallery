using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;

namespace Gallery.Web.Services
{
	[ServiceContract(Namespace = "urn:silverlightmobileme.codeplex.com")]
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
	public class ConfigService
	{
		[WebGet(UriTemplate = "/all",
				ResponseFormat = WebMessageFormat.Json,
				BodyStyle = WebMessageBodyStyle.Bare)]
		[OperationContract]
		public Setting[] GetAppSettings()
		{
			return GetAppSettingsJson();
		}

		[WebGet(UriTemplate = "/all/json",
				ResponseFormat = WebMessageFormat.Json,
				BodyStyle = WebMessageBodyStyle.Bare)]
		[OperationContract]
		public Setting[] GetAppSettingsJson()
		{
			return GetAllSettings();
		}

		[WebGet(UriTemplate = "/all/xml",
				ResponseFormat = WebMessageFormat.Xml,
				BodyStyle = WebMessageBodyStyle.Bare)]
		[OperationContract]
		public Setting[] GetAppSettingsXml()
		{
			return GetAllSettings();
		}

		[WebGet(UriTemplate = "/{key}",
				ResponseFormat = WebMessageFormat.Json,
				BodyStyle = WebMessageBodyStyle.Bare)]
		[OperationContract]
		public Setting GetAppSetting(string key)
		{
			return GetAppSettingJson(key);
		}

		[WebGet(UriTemplate = "/{key}/json",
		        ResponseFormat = WebMessageFormat.Json,
				BodyStyle = WebMessageBodyStyle.Bare)]
		[OperationContract]
		public Setting GetAppSettingJson(string key)
		{
			return GetSetting(key);
		}

		[WebGet(UriTemplate = "/{key}/xml",
				ResponseFormat = WebMessageFormat.Xml,
				BodyStyle = WebMessageBodyStyle.Bare)]
		[OperationContract]
		public Setting GetAppSettingXml(string key)
		{
			return GetSetting(key);
		}

		private static Setting[] GetAllSettings()
		{
			var settings = new Setting[ConfigurationManager.AppSettings.Count];
			// Get the appSettings.
			NameValueCollection appSettings = ConfigurationManager.AppSettings;

			// Get the collection enumerator.
			IEnumerator appSettingsEnum = appSettings.Keys.GetEnumerator();
			var i = 0;
			while (appSettingsEnum.MoveNext())
			{
				settings[i] = GetSetting(appSettings.Keys[i]);
				i++;
			}
			return settings;
		}

		private static Setting GetSetting(string key)
		{
			return new Setting
			{
				Key = key,
				Value = ConfigurationManager.AppSettings[key]
			};
		}

	}

	//[DataContract(Namespace = "urn:silverlightmobileme.codeplex.com")]
	public class Setting
	{
		//[DataMember]
		public string Key { get; set; }
		//[DataMember]
		public string Value { get; set; }
	}
}