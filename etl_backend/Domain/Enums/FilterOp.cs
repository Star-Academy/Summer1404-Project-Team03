namespace Domain.Enums;

public enum FilterOp
{
    Eq, Ne, Gt, Ge, Lt, Le,
    Between, In,
    Contains, StartsWith, EndsWith,
    IsNull, IsNotNull
}