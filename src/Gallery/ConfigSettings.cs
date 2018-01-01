using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Windows;

namespace Gallery
{
	public class ConfigSettings
	{
		#region : Private Fields :

		private readonly Uri _applicationUriSource = Application.Current.Host.Source;
		private static ConfigSettings _instance;
		private readonly Dictionary<string, string> _settings;

		#endregion : Private Fields :

		#region : Constructor :

		private ConfigSettings()
		{
			_settings = new Dictionary<string, string>();
		}

		#endregion : Constructor :

		#region : Public Properties :

		public static ConfigSettings Current
		{
			get { return _instance ?? (_instance = new ConfigSettings()); }
		}

		public string this[string key]
		{
			get { return _settings[key]; }
		}

		#endregion : Public Properties :

		#region : Public Events and Methods :

		public event EventHandler Loaded = delegate { };

		public void Load(Dictionary<string, string> initParams)
		{
			_settings.Add(Config.UsernameKey,initParams[Config.UsernameKey]);
			var webClient = new WebClient();
			webClient.OpenReadCompleted += GetConfigSettingsCompleted;
			var configServiceUri = new Uri(_applicationUriSource, initParams[Config.ConfigServiceSetting]);
			if (!initParams[Config.ConfigServiceSetting].StartsWith("../"))
			{
				configServiceUri = new Uri(initParams[Config.ConfigServiceSetting]);
			}
			webClient.OpenReadAsync(configServiceUri);
		}

		#endregion : Public Events and Methods :

		#region : Private Methods :

		private void GetConfigSettingsCompleted(object sender, OpenReadCompletedEventArgs e)
		{
			if (e.Error == null)
			{
				var ser = new DataContractJsonSerializer(typeof(Setting[]));
				var settings = (Setting[])ser.ReadObject(e.Result);
				settings.ToList().ForEach(a => 
				{
					if (!_settings.ContainsKey(a.Key))
					{
						_settings.Add(a.Key, a.Value);
					}
					else
					{
						_settings[a.Key] = a.Value;
					}
				});
				OnLoaded();
			}
		}

		protected void OnLoaded()
		{
			var handler = Loaded;
			if (handler != null)
			{
				handler(this, EventArgs.Empty);
			}
		}

		#endregion : Private Methods :

	} //end public class ConfigSettings

	public class Setting
	{
		public string Key { get; set; }
		public string Value { get; set; }
	}

} //end namespace Gallery
