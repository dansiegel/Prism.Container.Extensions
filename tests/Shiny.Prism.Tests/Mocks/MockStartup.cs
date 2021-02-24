﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Prism.DryIoc;
using Prism.Ioc;
using Shiny.Prism.Mocks.Delegates;
using Xunit.Abstractions;

namespace Shiny.Prism.Mocks
{
    public class MockStartup : PrismStartup, ILoggerProvider
    {
        private ITestOutputHelper _testOutputHelper { get; }

        public MockStartup(ITestOutputHelper testOutputHelper, bool setContainer = true)
            : base(setContainer ? RegenerateContainer() : null)
        {
            _testOutputHelper = testOutputHelper;

            if (!setContainer)
            {
                RegenerateContainer();
            }
        }

        protected override void ConfigureLogging(ILoggingBuilder builder, IPlatform platform)
        {
            base.ConfigureLogging(builder, platform);
            builder.ClearProviders();
            builder.AddProvider(this);
        }

        protected override void ConfigureServices(IServiceCollection services, IPlatform platform)
        {
            //services.UseBeacons<MockBeaconDelegate>();
            services.UseGps<MockGpsDelegate>();
            services.UseGeofencing<MockGeofenceDelegate>();
            //services.RegisterBleAdapterState<MockBleAdapterDelegate>();
        }

        // For Testing Only...
        static IContainerExtension RegenerateContainer()
        {
            PrismContainerExtension.Reset();
            return PrismContainerExtension.Init();
        }

        public void Write(Exception exception, params (string Key, string Value)[] parameters)
        {
            _testOutputHelper.WriteLine(exception.ToString());
            WriteParams(parameters);
        }

        public void Write(string eventName, string description, params (string Key, string Value)[] parameters)
        {
            _testOutputHelper.WriteLine($"Event: {eventName}");
            _testOutputHelper.WriteLine(description);
            WriteParams(parameters);
        }

        private void WriteParams((string Key, string Value)[] parameters)
        {
            foreach ((string Key, string Value) in parameters)
            {
                _testOutputHelper.WriteLine($"{Key}: {Value}");
            }
        }

        ILogger ILoggerProvider.CreateLogger(string categoryName)
        {
            return new Logger(categoryName, _testOutputHelper);
        }

        void IDisposable.Dispose()
        {
        }

        class Logger : ILogger
        {
            private string _name { get; }
            private ITestOutputHelper _output { get; }

            public Logger(string name, ITestOutputHelper output)
            {
                _name = name;
                _output = output;
            }

            public IDisposable BeginScope<TState>(TState state)
            {
                return new EmptyDisposable();
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return true;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                var logEntry = new LogEntry<TState>(logLevel, _name, eventId, state, exception, formatter);
                _output.WriteLine(logEntry.ToString());
            }

            class EmptyDisposable : IDisposable
            {
                public void Dispose() { }
            }
        }
    }
}
