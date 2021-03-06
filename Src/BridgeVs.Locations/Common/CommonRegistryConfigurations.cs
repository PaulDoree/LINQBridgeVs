﻿using System;
using Microsoft.Win32;
using System.Linq;
using BridgeVs.Shared.Options;

namespace BridgeVs.Shared.Common
{
    public class CommonRegistryConfigurations
    {
        private const string ErrorTrackingRegistryValue = "SentryErrorTracking";
        private const string SerializationMethodRegistryValue = "SerializationMethod";
        public const string LoggingRegistryValue = "Logging";

        // ReSharper disable once InconsistentNaming
        private const string LINQPadInstallationPathRegistryValue = "LINQPadInstallationPath";
        // ReSharper disable once InconsistentNaming
        private const string LINQPadVersionPathRegistryValue = "LINQPadVersion";

        public const string InstallationGuidRegistryValue = "UniqueId";


        // ReSharper disable once InconsistentNaming
        public static string GetLINQPadInstallationPath(string vsVersion)
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey($@"Software\LINQBridgeVs\{vsVersion}"))
            {
                return key?.GetValue(LINQPadInstallationPathRegistryValue) as string;
            }
        }
        public static void SetLINQPadInstallationPath(string vsVersion, string installationPath)
        {
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey($@"Software\LINQBridgeVs\{vsVersion}"))
            {
                key?.SetValue(LINQPadInstallationPathRegistryValue, installationPath);
            }
        }

        public static bool IsErrorTrackingEnabled(string vsVersion)
        {
            bool isTrackingEnabled = false;

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey($@"Software\LINQBridgeVs\{vsVersion}"))
            {
                string errorTracking = key?.GetValue(ErrorTrackingRegistryValue) as string;
                if (!string.IsNullOrEmpty(errorTracking))
                    isTrackingEnabled = Convert.ToBoolean(errorTracking);
            }

            return isTrackingEnabled;
        }

        public static void SetErrorTracking(string vsVersion, bool enabled)
        {
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey($@"Software\LINQBridgeVs\{vsVersion}"))
            {
                key?.SetValue(ErrorTrackingRegistryValue, enabled);
            }
        }

        /// <summary>
        /// Gets the assembly location. If an assembly is loaded at Runtime or it's loaded within IIS context, Assembly.Location property could be null
        /// This method reads the original location of the assembly that was Bridged
        /// </summary>
        /// <param name="type">The Type.</param>
        /// <param name="vsVersion">The Visual Studio version.</param>
        /// <returns></returns>
        public static string GetOriginalAssemblyLocation(Type @type, string vsVersion)
        {
            bool isSystemAssembly(string name) => name.Contains("Microsoft") || name.Contains("System") || name.Contains("mscorlib");

            string registryKeyPath = $@"Software\LINQBridgeVs\{vsVersion}\Solutions";

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(registryKeyPath))
            {
                if (key != null)
                {
                    string[] values = key.GetSubKeyNames();
                    RegistryKey registryKey = key;

                    foreach (string value in values)
                    {
                        RegistryKey subKey = registryKey.OpenSubKey(value);

                        string name = subKey?.GetValueNames().FirstOrDefault(p =>
                        {
                            if (!@type.IsGenericType)
                                return p == @type.Assembly.GetName().Name;
                            Type genericType = @type.GetGenericArguments()[0];

                            if (isSystemAssembly(genericType.Assembly.GetName().Name))
                                return false;

                            return p == genericType.Assembly.GetName().Name;
                        });

                        if (string.IsNullOrEmpty(name)) continue;

                        string assemblyLoc = (string)subKey.GetValue(name + "_location");

                        return assemblyLoc;
                    }
                }
            }
            return string.Empty;
        }

        public static void SetSerializationMethod(string vsVersion, string value)
        {
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey($@"Software\LINQBridgeVs\{vsVersion}"))
            {
                key?.SetValue(SerializationMethodRegistryValue, value);
            }
        }

        public static SerializationOption GetSerializationOption(string vsVersion)
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey($@"Software\LINQBridgeVs\{vsVersion}"))
            {
                string serializationOption = key?.GetValue(SerializationMethodRegistryValue) as string;
                if (!string.IsNullOrEmpty(serializationOption))
                {
                    return (SerializationOption)Enum.Parse(typeof(SerializationOption), serializationOption);
                }

                return SerializationOption.Binary;
            }
        }

        public static bool IsLoggingEnabled(string vsVersion)
        {
            bool isLoggingEnabled = false;

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey($@"Software\LINQBridgeVs\{vsVersion}"))
            {
                string logging = key?.GetValue(LoggingRegistryValue) as string;
                if (!string.IsNullOrEmpty(logging))
                    isLoggingEnabled = Convert.ToBoolean(logging);
            }

            return isLoggingEnabled;
        }

        public static void SetLogging(string vsVersion, bool enabled)
        {
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey($@"Software\LINQBridgeVs\{vsVersion}"))
            {
                key?.SetValue(LoggingRegistryValue, enabled);
            }
        }

        public static string GetUniqueGuid(string vsVersion)
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey($@"Software\LINQBridgeVs\{vsVersion}"))
            {
                return key?.GetValue(InstallationGuidRegistryValue) as string;

            }
        }

        public static void SetUniqueGuid(string vsVersion, string guid)
        {
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey($@"Software\LINQBridgeVs\{vsVersion}"))
            {
                key?.SetValue(InstallationGuidRegistryValue, guid);
            }
        }
    }
}
