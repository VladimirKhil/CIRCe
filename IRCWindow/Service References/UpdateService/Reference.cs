﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IRCWindow.UpdateService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="UpdateService.IUpdateService")]
    public interface IUpdateService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUpdateService/GetProductVersion", ReplyAction="http://tempuri.org/IUpdateService/GetProductVersionResponse")]
        System.Version GetProductVersion(string productName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUpdateService/GetProductVersionByOS", ReplyAction="http://tempuri.org/IUpdateService/GetProductVersionByOSResponse")]
        System.Version GetProductVersionByOS(string productName, System.Version osVersion);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUpdateService/GetProductUpdate", ReplyAction="http://tempuri.org/IUpdateService/GetProductUpdateResponse")]
        System.Uri GetProductUpdate(string productName);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IUpdateService/UpdateUsage")]
        void UpdateUsage(string productName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUpdateService/SendErrorReport", ReplyAction="http://tempuri.org/IUpdateService/SendErrorReportResponse")]
        bool SendErrorReport(string application, System.Version version, System.DateTime time, string error);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUpdateService/SendErrorReportNew", ReplyAction="http://tempuri.org/IUpdateService/SendErrorReportNewResponse")]
        int SendErrorReportNew(string application, System.Version version, System.DateTime time, string error);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IUpdateServiceChannel : IRCWindow.UpdateService.IUpdateService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class UpdateServiceClient : System.ServiceModel.ClientBase<IRCWindow.UpdateService.IUpdateService>, IRCWindow.UpdateService.IUpdateService {
        
        public UpdateServiceClient() {
        }
        
        public UpdateServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public UpdateServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public UpdateServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public UpdateServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Version GetProductVersion(string productName) {
            return base.Channel.GetProductVersion(productName);
        }
        
        public System.Version GetProductVersionByOS(string productName, System.Version osVersion) {
            return base.Channel.GetProductVersionByOS(productName, osVersion);
        }
        
        public System.Uri GetProductUpdate(string productName) {
            return base.Channel.GetProductUpdate(productName);
        }
        
        public void UpdateUsage(string productName) {
            base.Channel.UpdateUsage(productName);
        }
        
        public bool SendErrorReport(string application, System.Version version, System.DateTime time, string error) {
            return base.Channel.SendErrorReport(application, version, time, error);
        }
        
        public int SendErrorReportNew(string application, System.Version version, System.DateTime time, string error) {
            return base.Channel.SendErrorReportNew(application, version, time, error);
        }
    }
}
