using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Prism.Navigation;
using Prism.Forms.Extended.Mocks.Navigation;
using Xunit;

namespace Prism.Forms.Extended.Tests
{
    public class FluentNavigationTests
    {
        [Fact]
        public async Task NavigateFiresNavigateAsync()
        {
            var navService = new MockNavigationService()
            {
                Result = new MockNavigationResult
                {
                    Success = true
                }
            };

            var builder = navService.Navigate("ViewA");
            var success = false;
            var ex = await Record.ExceptionAsync(async () => success = await builder.ExecuteAsync());

            Assert.Null(ex);
            Assert.True(success);
            Assert.True(navService.NavigateCalled);
        }
        [Fact]
        public async Task NavigateUsesUriNotString()
        {
            var navService = new MockNavigationService()
            {
                Result = new MockNavigationResult
                {
                    Success = true
                }
            };

            var builder = navService.Navigate("ViewA");
            var success = false;
            var ex = await Record.ExceptionAsync(async () => success = await builder.ExecuteAsync());

            Assert.Null(ex);
            Assert.True(success);
            Assert.Null(navService.NavigationName);
            Assert.NotNull(navService.NavigationUri);
        }

        [Fact]
        public async Task NavigatePassesParameters()
        {
            var navService = new MockNavigationService()
            {
                Result = new MockNavigationResult
                {
                    Success = true
                }
            };

            var builder = navService.Navigate("ViewA")
                                    .WithParameter("message", "Hello World");
            var success = false;
            var ex = await Record.ExceptionAsync(async () => success = await builder.ExecuteAsync());

            Assert.Null(ex);
            Assert.True(success);
            Assert.Single(navService.ParametersUsed);
            Assert.Equal("Hello World", navService.ParametersUsed.GetValue<string>("message"));
        }

        [Fact]
        public async Task NavigateCatchesSpecificException()
        {
            var navService = new MockNavigationService()
            {
                Result = new MockNavigationResult
                {
                    Exception = new ArgumentNullException()
                }
            };

            bool catchAllFired = false;
            bool argumentNullFired = false;
            bool aritmeticExceptionFired = false;
            Exception caughtException = null;

            var builder = navService.Navigate("ViewA")
                                    .WithParameter("message", "Hello World")
                                    .Catch(e =>
                                    {
                                        caughtException = e;
                                        catchAllFired = true;
                                    })
                                    .Catch<ArithmeticException>(e =>
                                    {
                                        caughtException = e;
                                        aritmeticExceptionFired = true;
                                    })
                                    .Catch<ArgumentNullException>(e =>
                                    {
                                        caughtException = e;
                                        argumentNullFired = true;
                                    });
            var success = false;
            var ex = await Record.ExceptionAsync(async () => success = await builder.ExecuteAsync());

            Assert.Null(ex);
            Assert.False(success);

            Assert.False(catchAllFired);
            Assert.False(aritmeticExceptionFired);
            Assert.True(argumentNullFired);
            Assert.Equal(navService.Result.Exception, caughtException);
        }
    }
}
