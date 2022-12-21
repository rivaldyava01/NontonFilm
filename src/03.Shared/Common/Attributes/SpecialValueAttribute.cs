namespace Zeta.NontonFilm.Shared.Common.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class SpecialValueAttribute : Attribute
{
    public SpecialValueAttribute(SpecialValueType valueType)
    {
        ValueType = valueType;
    }

    public SpecialValueType ValueType { get; }
}

public enum SpecialValueType
{
    Json = 0,
    Xml = 1
}
