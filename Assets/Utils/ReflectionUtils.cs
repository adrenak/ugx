using System;

public static class Utils {
    public static T ExtractField<T>(object obj, string typeName, string fieldName) {
        var incomingType = Type.GetType(typeName);
        if (incomingType == null)
            throw new Exception($"No type {incomingType} found");

        var converted = Convert.ChangeType(obj, incomingType);

        var field = incomingType.GetField(fieldName).GetValue(converted);
        if (field == null)
            throw new Exception($"No field {fieldName} found");

        return (T)field;
    }
}
