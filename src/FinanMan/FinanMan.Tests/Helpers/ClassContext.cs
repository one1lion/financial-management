using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FinanMan.Tests.Helpers;

public abstract class ClassContext<T> where T : class
{
    private readonly Dictionary<Type, BuiltMocks> _mocks = new();

    private T? _sut = null;
    protected T? Sut => _sut ??= Resolve(_mocks);

    private class BuiltMocks
    {
        public object Mock { get; set; } = default!;
        public object? MockValue { get; set; }
    }

    protected Mock<TE> MockOf<TE>() where TE : class
    {
        if (_mocks.ContainsKey(typeof(TE)))
        {
            return (Mock<TE>)_mocks[typeof(TE)].Mock;
        }

        Mock<TE>? mock = null;
        if (typeof(TE).IsClass)
        {
            var parameters = GetParamInfoForConstructorOfType<TE>(false);

            if (parameters.Length > 0)
            {
                mock = new Mock<TE>(parameters.Select(t => GetDefault(t.ParameterType)).ToArray());
            }
        }

        mock ??= new Mock<TE>();

        _mocks.Add(typeof(TE), new BuiltMocks
        {
            Mock = mock,
            MockValue = mock.Object
        });
        return MockOf<TE>();
    }

    private static T? Resolve(IReadOnlyDictionary<Type, BuiltMocks> dictionary)
    {
        var allTypes = GetParamInfoForConstructorOfType<T>()
            .Select(t => t.ParameterType)
            .Select(t => dictionary.ContainsKey(t) ? dictionary[t].MockValue : null)
            .ToArray();

        return allTypes.Any()
            ? (T?)Activator.CreateInstance(typeof(T), allTypes)
            : (T?)Activator.CreateInstance(typeof(T));
    }


    private static ParameterInfo[] GetParamInfoForConstructorOfType(Type type, bool largest = true)
    {
        var entryPoints = type.GetConstructors().Where(t => t.IsPublic);
        var part = largest
            ? entryPoints.OrderByDescending(t => t.GetParameters().Length)
            : entryPoints.OrderBy(t => t.GetParameters().Length);

        return part.FirstOrDefault()?.GetParameters() ?? Array.Empty<ParameterInfo>();
    }

    private static object? GetDefault(Type type) => type.IsValueType ? Activator.CreateInstance(type) : null;

    private static ParameterInfo[] GetParamInfoForConstructorOfType<TE>(bool largest = true)
        => GetParamInfoForConstructorOfType(typeof(TE), largest);
}