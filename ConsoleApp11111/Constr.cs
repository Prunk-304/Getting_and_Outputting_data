using System.Linq.Expressions;
using System.Reflection.Emit;
using Microsoft.Extensions.Configuration;

namespace ConsoleApp1;

public class Constr: IConstr
{
    public dynamic build(string filename)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(filename, optional: false, reloadOnChange: true);
        var configuration = builder.Build();
        return configuration;
    }
}