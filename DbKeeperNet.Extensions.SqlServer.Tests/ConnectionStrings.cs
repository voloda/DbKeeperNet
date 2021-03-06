﻿namespace DbKeeperNet.Extensions.SqlServer.Tests
{
    public static class ConnectionStrings
    {
        public static readonly string CommonConnectionString = @"server=.\sqlexpress;Integrated Security=true;Connection Timeout=120";
        public static readonly string MasterDatabase = CommonConnectionString + ";initial catalog=master";
        public static readonly string TestDatabaseName = "TestSqlExpress";
        public static readonly string TestDatabase = CommonConnectionString + ";initial catalog=" + TestDatabaseName;
        
    }
}