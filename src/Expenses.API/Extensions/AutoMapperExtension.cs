using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Expenses.API.Extensions
{
    public static class AutoMapperExtension
    {
        public static void AddAutoMapperExtension(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            //services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            //services.AddAutoMapper(typeof(Startup));
            //https://github.com/microsoft/vstest/issues/2008
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies().Where(assembly => !assembly.FullName.StartsWith("Microsoft.VisualStudio.TraceDataCollector")));
        }
    }
}
