using System;
using System.Configuration;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using System.ComponentModel;
using DevExpress.ExpressApp.Mobile;
using System.Collections.Generic;
using DevExpress.ExpressApp.Xpo;
using DevExpress.ExpressApp.Security;
using System.Reflection;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace BackgroundGeolocation.Mobile {
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/DevExpressExpressAppWebWebApplicationMembersTopicAll.aspx
    public partial class BackgroundGeolocationMobileApplication : MobileApplication {
        private DevExpress.ExpressApp.SystemModule.SystemModule module1;
        private DevExpress.ExpressApp.Mobile.SystemModule.SystemMobileModule module2;
        private BackgroundGeolocation.Module.BackgroundGeolocationModule module3;
        private BackgroundGeolocation.Module.Mobile.BackgroundGeolocationMobileModule module4;
        private DevExpress.ExpressApp.Security.SecurityModule securityModule1;
        private DevExpress.ExpressApp.Maps.Mobile.MapsMobileModule mapsMobileModule;

        #region Default XAF configuration options (https://www.devexpress.com/kb=T501418)
        static BackgroundGeolocationMobileApplication() {
            DevExpress.Persistent.Base.PasswordCryptographer.EnableRfc2898 = true;
            DevExpress.Persistent.Base.PasswordCryptographer.SupportLegacySha512 = false;
        }
        #endregion
        public BackgroundGeolocationMobileApplication() {
		    Tracing.Initialize();
            if(ConfigurationManager.ConnectionStrings["ConnectionString"] != null) {
                ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            }
#if EASYTEST
            if(ConfigurationManager.ConnectionStrings["EasyTestConnectionString"] != null) {
                ConnectionString = ConfigurationManager.ConnectionStrings["EasyTestConnectionString"].ConnectionString;
            }
#endif
            InitializeComponent();
            if(System.Diagnostics.Debugger.IsAttached && CheckCompatibilityType == CheckCompatibilityType.DatabaseSchema) {
                DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways;
            }

            AdditionalPhoneGapPlugins.Add(@"<plugin name=""cordova-background-geolocation-lt"" source=""npm"" spec=""2.7.3"">
			<variable name=""LOCATION_ALWAYS_USAGE_DESCRIPTION"" value=""Background location-tracking is required"" />
			<variable name=""LOCATION_WHEN_IN_USE_USAGE_DESCRIPTION"" value=""Background location-tracking is required"" />
			<variable name=""MOTION_USAGE_DESCRIPTION"" value=""Using the accelerometer increases battery-efficiency by intelligently toggling location-tracking only when the device is detected to be moving"" />
			<variable name=""LICENSE"" value=""YOUR_LICENSE_KEY"" />
			</plugin>");
            AdditionalPhoneGapPlugins.Add(@"<plugin name=""cordova-plugin-device"" source=""npm"" spec=""1.1.6""></plugin>");

            string geolocationScript = ReadResourceString("BackgroundGeolocation.Mobile.Geolocation.GeolocationScript.js");
            RegisterClientScriptOnApplicationStart("GeolocationScript", geolocationScript);
        }

        public static byte[] ReadResource(string resourceName) {
            byte[] buffer = null;
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName)) {
                if (stream != null) {
                    buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, (int)stream.Length);
                }
            }
            return buffer;
        }
        public static string ReadResourceString(string resourceName) {
            byte[] resourceBytes = ReadResource(resourceName);
            return Encoding.UTF8.GetString(resourceBytes).Replace("\r\n", "\\r\\n");
        }
        protected override void CreateDefaultObjectSpaceProvider(CreateCustomObjectSpaceProviderEventArgs args) {
            args.ObjectSpaceProvider = new XPObjectSpaceProvider(GetDataStoreProvider(args.ConnectionString, args.Connection), true);
            args.ObjectSpaceProviders.Add(new NonPersistentObjectSpaceProvider(TypesInfo, null));
        }
        private IXpoDataStoreProvider GetDataStoreProvider(string connectionString, System.Data.IDbConnection connection) {
            IXpoDataStoreProvider dataStoreProvider = null;
            if(!String.IsNullOrEmpty(connectionString)) {
                dataStoreProvider = new ConnectionStringDataStoreProvider(connectionString);
            }
            else if(connection != null) {
                dataStoreProvider = new ConnectionDataStoreProvider(connection);
            }
			return dataStoreProvider;
        }
        private void BackgroundGeolocationMobileApplication_DatabaseVersionMismatch(object sender, DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs e) {
#if EASYTEST
            e.Updater.Update();
            e.Handled = true;
#else
            if(System.Diagnostics.Debugger.IsAttached) {
                e.Updater.Update();
                e.Handled = true;
            }
            else {
				string message = "The application cannot connect to the specified database, " +
					"because the database doesn't exist, its version is older " +
					"than that of the application or its schema does not match " +
					"the ORM data model structure. To avoid this error, use one " +
					"of the solutions from the https://www.devexpress.com/kb=T367835 KB Article.";

                if(e.CompatibilityError != null && e.CompatibilityError.Exception != null) {
                    message += "\r\n\r\nInner exception: " + e.CompatibilityError.Exception.Message;
                }
                throw new InvalidOperationException(message);
            }
#endif
        }
        private void InitializeComponent() {
            this.module1 = new DevExpress.ExpressApp.SystemModule.SystemModule();
            this.module2 = new DevExpress.ExpressApp.Mobile.SystemModule.SystemMobileModule();
            this.module3 = new BackgroundGeolocation.Module.BackgroundGeolocationModule();
            this.module4 = new BackgroundGeolocation.Module.Mobile.BackgroundGeolocationMobileModule();
            this.securityModule1 = new DevExpress.ExpressApp.Security.SecurityModule();
            this.mapsMobileModule = new DevExpress.ExpressApp.Maps.Mobile.MapsMobileModule();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // BackgroundGeolocationMobileApplication
            // 
            this.ApplicationName = "BackgroundGeolocation";
            this.Modules.Add(this.module1);
            this.Modules.Add(this.module2);
            this.Modules.Add(this.module3);
            this.Modules.Add(this.module4);
            this.Modules.Add(this.securityModule1);
            this.Modules.Add(this.mapsMobileModule);
            this.DatabaseVersionMismatch += new System.EventHandler<DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs>(this.BackgroundGeolocationMobileApplication_DatabaseVersionMismatch);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
    }
}
