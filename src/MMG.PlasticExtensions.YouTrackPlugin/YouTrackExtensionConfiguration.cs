﻿// *************************************************
// MMG.PlasticExtensions.YouTrackPlugin.YouTrackExtensionConfiguration.cs
// Last Modified: 12/24/2015 10:52 AM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

namespace MMG.PlasticExtensions.YouTrackPlugin
{
    using System;
    using System.Collections.Generic;
    using Codice.Client.IssueTracker;

    public class YouTrackExtensionConfiguration
    {
        private readonly IssueTrackerConfiguration _storedConfig;

        public YouTrackExtensionConfiguration(IssueTrackerConfiguration pStoredConfig)
        {
            _storedConfig = pStoredConfig;
        }

        internal class ParameterNames
        {
            internal const string BranchPrefix = "Branch Name Prefix";
            internal const string UserID = "User ID";
            internal const string Password = "Password";
            internal const string Host = "Host";
            internal const string ShowIssueStateInBranchTitle = "Show issues state in branch title";
            internal const string ClosedIssueStates = "Issue states considered closed";
        }

        public string BranchPrefix
        {
            get { return getValidParameterValue(ParameterNames.BranchPrefix); }
        }

        public string Host
        {
            get { return getValidParameterValue(ParameterNames.Host); }
        }

        public int? CustomPort
        {
            get { return 1; }
        }

        public string UserID
        {
            get { return getValidParameterValue(ParameterNames.UserID); }
        }

        public string Password
        {
            get { return getValidParameterValue(ParameterNames.Password); }
        }

        public bool UseSSL
        {
            get { return true; }
        }

        public bool ShowIssueStateInBranchTitle
        {
            get { return bool.Parse(getValidParameterValue(ParameterNames.ShowIssueStateInBranchTitle, "false")); }
        }

        /// <summary>
        /// Issue state(s) to not display in branch title when ShowIssueStateInBranchTitle = true.
        /// </summary>
        /// <remarks>Use commas to separate multiple states.</remarks>
        public string IgnoreIssueStateForBranchTitle
        {
            get { return getValidParameterValue(ParameterNames.ClosedIssueStates, "Completed"); }
        }

        public ExtensionWorkingMode WorkingMode
        {
            get
            {
                if (_storedConfig == null)
                    return ExtensionWorkingMode.TaskOnBranch;

                return _storedConfig.WorkingMode == ExtensionWorkingMode.None
                    ? ExtensionWorkingMode.TaskOnBranch
                    : _storedConfig.WorkingMode;
            }
        }

        public List<IssueTrackerConfigurationParameter> GetYouTrackParameters()
        {
            var parameters = new List<IssueTrackerConfigurationParameter>();

            parameters.Add
                (new IssueTrackerConfigurationParameter
                {
                    Name = ParameterNames.BranchPrefix,
                    Value = BranchPrefix,
                    Type = IssueTrackerConfigurationParameterType.BranchPrefix,
                    IsGlobal = true
                });
            parameters.Add
                (new IssueTrackerConfigurationParameter
                {
                    Name = ParameterNames.Host,
                    Value = Host,
                    Type = IssueTrackerConfigurationParameterType.Host,
                    IsGlobal = true
                });

            parameters.Add
                (new IssueTrackerConfigurationParameter
                {
                    Name = ParameterNames.UserID,
                    Value = UserID,
                    Type = IssueTrackerConfigurationParameterType.User,
                    IsGlobal = false
                });
            parameters.Add
                (new IssueTrackerConfigurationParameter
                {
                    Name = ParameterNames.Password,
                    Value = Password,
                    Type = IssueTrackerConfigurationParameterType.Password,
                    IsGlobal = false
                });
            parameters.Add
                (new IssueTrackerConfigurationParameter
                {
                    Name = ParameterNames.ShowIssueStateInBranchTitle,
                    Value = ShowIssueStateInBranchTitle.ToString(),
                    Type = IssueTrackerConfigurationParameterType.Boolean,
                    IsGlobal = false
                });
            parameters.Add
                (new IssueTrackerConfigurationParameter
                {
                    Name = ParameterNames.ClosedIssueStates,
                    Value = IgnoreIssueStateForBranchTitle,
                    Type = IssueTrackerConfigurationParameterType.Text,
                    IsGlobal = false
                });

            return parameters;
        }

        private string getValidParameterValue(string pParamName, string pDefaultValue = "")
        {
            var configValue = _storedConfig.GetValue(pParamName);

            if (string.IsNullOrEmpty(pDefaultValue) && string.IsNullOrEmpty(configValue))
                throw new ApplicationException(string.Format("The configuration value for '{0}' is required but was not provided!", pParamName));

            return string.IsNullOrEmpty(configValue)
                ? pDefaultValue
                : configValue;
        }
    }
}