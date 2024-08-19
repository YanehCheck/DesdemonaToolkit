using YanehCheck.EpicGamesUtils.Common.Enums.Items;
using YanehCheck.EpicGamesUtils.WpfUiApp.Models;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters.Exceptions;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters.Implementation.Enums;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters.Implementation;

/// <summary>
/// Represents a condition, which can be chained.
/// Uses dynamic type-based evaluation, rather than directly referencing properties of ItemModel.
/// </summary>
public class ChainedCondition {
    public string Property { get; set; } = "";
    public Operation Operation { get; set; }
    public ListOperation ListOperation { get; set; } = ListOperation.NotAListOperation;
    public object? Parameter { get; set; }
    public ChainedCondition? FollowingTerm { get; set; }
    public string? Remark { get; set; } 

    private const double Epsilon = 0.000001;
    private const int MaxClauses = 100000;


    public ChainedCondition() { }

    /// <summary>
    /// Represents a term, or a clause if chained together.
    /// </summary>
    /// <param name="property">Nameof property which will be checked.</param>
    /// <param name="listOperation">List operation. Values other than NotAListOperation applicable if parameter is typeof IEnumerable.</param>
    /// <param name="operation"><see cref="Operation"/></param>
    /// <param name="parameter"><see cref="Operation"/> should be defined for its data type. IEnumerable<type> is okay if we are doing a list operation.</param>
    /// <param name="followingTerm">Following term.</param>
    public ChainedCondition(string property, ListOperation listOperation, Operation operation, object? parameter, ChainedCondition? followingTerm) {
        Property = property;
        Operation = operation;
        ListOperation = listOperation;
        Parameter = parameter;
        FollowingTerm = followingTerm;
    }

    /// <summary>
    /// Is term (or whole clause if chained together) satisfied?
    /// </summary>
    public bool Satisfied(ItemOwnedModel item) => Matches(item) && (FollowingTerm?.Satisfied(item) ?? true);

    public int Count() {
        var condition = this;
        for (int i = 1; i < MaxClauses; i++) {
            if(condition!.FollowingTerm == null) {
                return i;
            }
            condition = condition.FollowingTerm;
        }

        throw new FilterSpecialException("Reached max number of terms in a clause");
    }

    public ChainedCondition Last() {
        var condition = this;
        for (int i = 0; i < MaxClauses; i++) {
            if (condition!.FollowingTerm is null) {
                return condition;
            }
            condition = condition.FollowingTerm;
        }

        throw new FilterSpecialException("Reached max number of terms in a clause");
    }

    private bool Matches(ItemOwnedModel item) {
        if (ListOperation == ListOperation.NotAListOperation) {
            return MatchesSingle(item, Parameter);
        }

        if (Parameter is IEnumerable<object?> enumerableParameter) {
            return ListOperation switch {
                ListOperation.Any => enumerableParameter.Any(p => MatchesSingle(item, p)),
                ListOperation.NotAny => !enumerableParameter.Any(p => MatchesSingle(item, p)),
                ListOperation.All => enumerableParameter.All(p => MatchesSingle(item, p)),
                ListOperation.NotAll => !enumerableParameter.All(p => MatchesSingle(item, p)),
                _ => throw new FilterUnsupportedOperationException("Unknown list operation.")
            };
        }

        throw new FilterUnsupportedOperationException("Unknown list operation.");
    }

    private bool MatchesSingle(ItemOwnedModel item, object? parameter) {
        var property = item.GetType().GetProperty(Property);
        if (property == null) {
            return false;
        }

        var propertyType = property.PropertyType;
        var propertyValue = property.GetValue(item);

        if(propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) {
            propertyType = Nullable.GetUnderlyingType(propertyType);
        }


        if (parameter == null) {
            return HandleNull(propertyValue, Operation, parameter);
        }
        else if (propertyValue == null) {
            return false;
        }

        if (propertyType == typeof(string)) {
            return HandleString((string) propertyValue!, Operation, parameter);
        }
        else if(propertyType == typeof(int)) {
            return HandleInt((int) propertyValue!, Operation, parameter);
        }
        else if(propertyType == typeof(double)) {
            return HandleDouble((double) propertyValue!, Operation, parameter);
        }
        else if(propertyType == typeof(DateTime)) {
            return HandleDateTime((DateTime) propertyValue!, Operation, parameter);
        }
        else if(propertyType == typeof(ItemSource)) {
            return HandleEnum((ItemSource) propertyValue!, Operation, parameter);
        }
        else if(propertyType == typeof(ItemRarity)) {
            return HandleEnum((ItemRarity) propertyValue!, Operation, parameter);
        }
        else if(propertyType == typeof(ItemType)) {
            return HandleEnum((ItemType) propertyValue!, Operation, parameter);
        }
        else if(propertyType == typeof(IEnumerable<string>)) {
            return HandleStringEnumerableValue((IEnumerable<string>) propertyValue!, Operation, parameter);
        }
        else if(propertyType == typeof(IEnumerable<ItemTag>)) {
            return HandleItemTagEnumerableValue((IEnumerable<ItemTag>) propertyValue!, Operation, parameter);
        }
        else {
            throw new FilterUnsupportedDataTypeException(
                $"Type {property.PropertyType} of property {property.Name} is unsupported.");
        }
    }

    private bool HandleItemTagEnumerableValue(IEnumerable<ItemTag> enumTagEnumerableValue, Operation operation, object? parameter) {
        if(parameter is ItemTag itemTagParameter) {
            return operation switch {
                Operation.Contains => enumTagEnumerableValue.Contains(itemTagParameter),
                Operation.NotContains => !enumTagEnumerableValue.Contains(itemTagParameter),
                _ => throw new FilterUnsupportedOperationException(
                    $"Operation {Enum.GetName(operation)!} is not valid for IEnumerable<ItemTag> type.")
            };
        }
        throw new FilterUnsupportedDataTypeException("Parameter is not type of ItemTag.");
    }

    private bool HandleStringEnumerableValue(IEnumerable<string> stringEnumerableValue, Operation operation, object? parameter) {
        if(parameter is string stringParameter) {
            return operation switch {
                Operation.Contains => stringEnumerableValue.Contains(stringParameter),
                Operation.NotContains => !stringEnumerableValue.Contains(stringParameter),
                _ => throw new FilterUnsupportedOperationException(
                    $"Operation {Enum.GetName(operation)!} is not valid for IEnumerable<string> type.")
            };
        }
        throw new FilterUnsupportedDataTypeException("Parameter is not type of string.");
    }

    private bool HandleEnum<T>(T value, Operation operation, object? parameter) where T : Enum, IConvertible {
        if(parameter is T enumParameter) {
            int intValue = value.ToInt32(null);
            int intParameter = enumParameter.ToInt32(null);
            return operation switch {
                Operation.Equals => intValue == intParameter,
                Operation.NotEquals => intValue != intParameter,
                Operation.GreaterThan => intValue > intParameter,
                Operation.GreaterThanOrEqual => intValue >= intParameter,
                Operation.LessThan => intValue < intParameter,
                Operation.LessThanOrEqual => intValue <= intParameter,
                _ => throw new FilterUnsupportedOperationException(
                    $"Operation {Enum.GetName(operation)!} is not valid for int type.")
            };
        }
        throw new FilterUnsupportedDataTypeException("Parameter is not type of int.");
    }

    private bool HandleDateTime(DateTime dateTimeValue, Operation operation, object? parameter) {
        if(parameter is DateTime dateTimeParameter) {
            return operation switch {
                Operation.Equals => dateTimeValue == dateTimeParameter,
                Operation.NotEquals => dateTimeValue != dateTimeParameter,
                Operation.GreaterThan => dateTimeValue > dateTimeParameter,
                Operation.GreaterThanOrEqual => dateTimeValue >= dateTimeParameter,
                Operation.LessThan => dateTimeValue < dateTimeParameter,
                Operation.LessThanOrEqual => dateTimeValue <= dateTimeParameter,
                _ => throw new FilterUnsupportedOperationException($"Operation {Enum.GetName(operation)!} is not valid for DateTime type.")
            };
        }

        throw new FilterUnsupportedDataTypeException("Parameter is not type of DateTime.");
    }

    private bool HandleDouble(double doubleValue, Operation operation, object? parameter) {
        if(parameter is double doubleParameter) {
            return operation switch {
                Operation.Equals => Math.Abs(doubleValue - doubleParameter) < Epsilon,
                Operation.NotEquals => Math.Abs(doubleValue - doubleParameter) >= Epsilon,
                Operation.StrictEquals => doubleValue == doubleParameter,
                Operation.StrictNotEquals => doubleValue != doubleParameter,
                Operation.GreaterThan => doubleValue > doubleParameter,
                Operation.GreaterThanOrEqual => doubleValue >= doubleParameter,
                Operation.LessThan => doubleValue < doubleParameter,
                Operation.LessThanOrEqual => doubleValue <= doubleParameter,
                _ => throw new FilterUnsupportedOperationException($"Operation {Enum.GetName(operation)!} is not valid for double type.")
            };
        }

        throw new FilterUnsupportedDataTypeException("Parameter is not type of double.");
    }

    private bool HandleInt(int intValue, Operation operation, object? parameter) {
        if(parameter is int intParameter) {
            return operation switch {
                Operation.Equals => intValue == intParameter,
                Operation.NotEquals => intValue != intParameter,
                Operation.GreaterThan => intValue > intParameter,
                Operation.GreaterThanOrEqual => intValue >= intParameter,
                Operation.LessThan => intValue < intParameter,
                Operation.LessThanOrEqual => intValue <= intParameter,
                _ => throw new FilterUnsupportedOperationException($"Operation {Enum.GetName(operation)!} is not valid for int type.")
            };
        }

        throw new FilterUnsupportedDataTypeException("Parameter is not type of int.");
    }

    private bool HandleString(string strValue, Operation operation, object? parameter) {
        if(parameter is string strParameter) {
            return operation switch {
                Operation.Equals => string.Equals(strValue, strParameter, StringComparison.CurrentCultureIgnoreCase),
                Operation.NotEquals => !string.Equals(strValue, strParameter, StringComparison.CurrentCultureIgnoreCase),
                Operation.StrictEquals => strValue == strParameter,
                Operation.StrictNotEquals => strValue != strParameter,
                Operation.Contains => strValue.Contains(strParameter, StringComparison.InvariantCultureIgnoreCase),
                Operation.NotContains => !strValue.Contains(strParameter, StringComparison.InvariantCultureIgnoreCase),
                Operation.GreaterThan => string.CompareOrdinal(strParameter, strValue) > 0,
                Operation.GreaterThanOrEqual => string.CompareOrdinal(strParameter, strValue) >= 0,
                Operation.LessThan => string.CompareOrdinal(strValue, strParameter) < 0,
                Operation.LessThanOrEqual => string.CompareOrdinal(strValue, strParameter) <= 0,
                _ => throw new FilterUnsupportedOperationException($"Operation {Enum.GetName(operation)!} is not valid for string type.")
            };
        }

        throw new FilterUnsupportedDataTypeException("Parameter is not type of string.");
    }

    private bool HandleNull(object? propertyValue, Operation operation, object? parameter) {
        return operation switch {
            Operation.Equals => propertyValue is null,
            Operation.NotEquals => propertyValue is not null,
            _ => throw new FilterUnsupportedOperationException($"Operation {Enum.GetName(operation)!} is not valid for null type.")
        };
    }

}