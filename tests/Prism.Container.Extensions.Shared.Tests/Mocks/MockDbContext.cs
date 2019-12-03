using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Prism.Container.Extensions.Shared.Mocks
{
    public class MockDbContext : DbContext
    {
        public MockDbContext([NotNull] DbContextOptions<MockDbContext> options) 
            : base(options)
        {
        }
    }
}
