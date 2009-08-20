using System;
using System.Collections.Generic;
using System.Text;

namespace DbKeeperNet.Engine
{
    /// <summary>
    /// Interface declaring current update execution
    /// context.
    /// </summary>
    public interface IUpdateContext : IDisposable
    {
        /// <summary>
        /// Currently running update version (taken from Update XML element).
        /// </summary>
        string CurrentVersion { get; set; }
        /// <summary>
        /// Currently running update version step (taken from Update/UpdateStep[@Id] attribute).
        /// </summary>
        int CurrentStep { get; set; }
        /// <summary>
        /// Application unique update group identifier (can be namespace, GUID etc.)
        /// </summary>
        string CurrentAssemblyName { get; set; }
        /// <summary>
        /// Database services attached to this execution context. Database connection
        /// should be closed during disposing of this context instance.
        /// </summary>
        IDatabaseService DatabaseService { get; }
        /// <summary>
        /// Service which allows message logging
        /// </summary>
        ILoggingService Logger { get; }
        /// <summary>
        /// Verify precondition identified by <paramref name="name"/>
        /// with optional parameter <paramref name="param"/>.
        /// </summary>
        /// <param name="name">Precondition identifier.</param>
        /// <param name="parameters">Optional parameter</param>
        /// <returns>
        /// <list type="bullet">
        /// <item>true - condition was met, step can be executed.</item>
        /// <item>false - prevent step from execution.</item>
        /// </list>
        /// </returns>
        bool CheckPrecondition(string name, PreconditionParamType[] parameters);
        /// <summary>
        /// Method called by extension to register a precondition
        /// plugin.
        /// </summary>
        /// <param name="precondition">Instance of precondition</param>
        void RegisterPrecondition(IPrecondition precondition);
        /// <summary>
        /// Method called by extension to register a database service
        /// plugin.
        /// </summary>
        /// <param name="service">Instance of database service</param>
        void RegisterDatabaseService(IDatabaseService service);
        /// <summary>
        /// Method called by extension to register a logging service
        /// plugin.
        /// </summary>
        /// <param name="service">Instance of the logging service</param>
        void RegisterLoggingService(ILoggingService service);
        /// <summary>
        /// Initialize context database service based on given
        /// connection string name from App.Config.
        /// 
        /// Given connection string name must mu correctly mapped in App.Config section:
        /// <example>
        /// <![CDATA[
        /// <dbkeeper.net loggingService="fx">
        ///   <databaseServiceMappings>
        ///     <add connectString="mock" databaseService="MsSql" />
        /// ]]>
        /// </example>
        /// </summary>
        /// <param name="connectionString">Connection string name within App.Config</param>
        void InitializeDatabaseService(string connectionString);
        /// <summary>
        /// Force manual logging service initialization based on its name.
        /// If this method is not called, logging service from App.Config
        /// should be automatically initialized at the moment it's first time
        /// used:
        /// <code><![CDATA[<dbkeeper.net loggingService="fx">]]></code>
        /// </summary>
        /// <param name="serviceName">Logging service registration name (such as fx, dummy etc.)</param>
        void InitializeLoggingService(string serviceName);
        /// <summary>
        /// Default preconditions applied to the update step
        /// in the case that no step specific preconditions
        /// are declared.
        /// </summary>
        PreconditionType[] DefaultPreconditions { get; set; }
    }
}
